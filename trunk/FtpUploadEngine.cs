using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Net;
using System.Threading;
using netCommander.FileSystemEx;

namespace netCommander
{
    public class FtpUploadEngine
    {
        private string[] initial_source;
        private FtpConnection connection;
        private string initial_destination;
        private FtpTransferOptions opts;
        private CopyFileProgressDialog progress;
        private Thread thread_background;
        private MethodInvokerUpdateProgress update_progress_delegate_holder;
        private long total_source_size = 0L;
        private FtpTransferProgress transfer_progress_delegate_holder;
        private int count_files_processed = 0;
        private long count_bytes_total_transferred = 0;
        private long count_bytes_current_transferred = 0;
        private int start_tick = 0;

        public event ItemEventHandler UploadItemDone;
        public event EventHandler Done;

        public FtpUploadEngine
            (string[] source,
            string destination,
            FtpConnection connection,
            FtpTransferOptions options,
            CopyFileProgressDialog progress_dialog)
        {
            initial_source = source;
            initial_destination = destination;
            this.connection = connection;
            opts = options;
            progress = progress_dialog;
            thread_background = new Thread(new ThreadStart(internal_do));
            update_progress_delegate_holder = new MethodInvokerUpdateProgress(update_progress);
            if (progress_dialog != null)
            {
                progress_dialog.FormClosing += new System.Windows.Forms.FormClosingEventHandler(progress_dialog_FormClosing);
            }
            transfer_progress_delegate_holder = new FtpTransferProgress(ftp_transfer_proc);
        }

        public void Run()
        {
            thread_background.Start();
            //internal_do(); //sync call
        }

        private object progress_closing_lock = new object();
        private bool progress_closing_unsafe = false;
        private bool ProgressCloseBeginSafe
        {
            get
            {
                bool ret = false;
                lock (progress_closing_lock)
                {
                    ret = progress_closing_unsafe;
                }
                return ret;
            }
            set
            {
                lock (progress_closing_lock)
                {
                    progress_closing_unsafe = value;
                }
                AbortSafe = value;
            }
        }

        void progress_dialog_FormClosing(object sender, System.Windows.Forms.FormClosingEventArgs e)
        {
            //progress dialog before user or system close
            //call from UI thread!

            //thread safe set abort_job flag - will be check by file enum functions 
            AbortSafe = true;

            //these will be check in transfer callback proc
            progress.CancelPressedSafe = true;

            //set internal flag - will will be check by some callbacks
            ProgressCloseBeginSafe = true;
        }

        private int calc_tick_elapsed()
        {
            int cur_tick = Environment.TickCount;
            int ret = 0;
            if (cur_tick < start_tick)
            {
                //occured ones on 41 days
                ret = (int.MaxValue - start_tick) + cur_tick;
            }
            else
            {
                ret = cur_tick - start_tick;
            }
            return ret;
        }

        /// <summary>
        /// Kbytes per sec
        /// </summary>
        /// <returns></returns>
        private long calc_speed()
        {
            int job_ticks = calc_tick_elapsed();
            if (job_ticks <= 0)
            {
                return int.MaxValue;
            }

            long ret_long_msec = (count_bytes_total_transferred + count_bytes_current_transferred) / (long)job_ticks;
            //now ret_ulong_msec is bytes per millisecond

            //calc bytes per sec
            long ret = ret_long_msec * 1000U;

            //and Kbyte per sec
            ret = ret / 1024U;

            return ret;
        }

        private object abort_lock = new object();
        private bool abort_unsafe = false;
        public bool AbortSafe
        {
            get
            {
                bool ret = false;
                lock (abort_lock)
                {
                    ret = abort_unsafe;
                }
                return ret;
            }
            set
            {
                lock (abort_lock)
                {
                    abort_unsafe = value;
                }
            }
        }

        private string update_lit_0 = Options.GetLiteral(Options.LANG_0_KBYTE_SEC);
        private string update_lit_1 = Options.GetLiteral(Options.LANG_0_BYTES_FROM_1_2P_UPLOADED);
        private string update_lit_2 = Options.GetLiteral(Options.LANG_0_BYTES_UPLOADED);
        private string update_lit_3 = Options.GetLiteral(Options.LANG_0_ARROW_1_2);
        private ulong label_update_interval = (ulong)Options.ProgressUpdateInterval;
        private ulong label_update_count = 0;
        private ulong stream_transferred_pre_chunk = 0;
        private void update_progress(UpdateProgressArgs e)
        {
            //will be invoke in UI thread - canot use anything else e and progress_dialog

            switch (e.Reason)
            {
                case UpdateProgressReason.ChunkFinish:
                    //progress.SetProgressStream(e.StreamTransferred, e.StreamSize);
                    if (label_update_count > label_update_interval)
                    {
                        progress.labelSpeed.Text = string.Format(update_lit_0, e.KBytesPerSec);
                    }
                    if (e.EnableTotalProgress)
                    {
                        progress.SetProgress(e.TotalTransferred, e.TotalSize);
                        if (label_update_count > label_update_interval)
                        {
                            progress.labelStatusTotal.Text =
                                string.Format
                                (update_lit_1,
                                e.TotalTransferred,
                                e.TotalSize,
                                (e.TotalSize == 0) ? 0 : (double)e.TotalTransferred / (double)e.TotalSize);
                        }
                    }
                    else
                    {
                        progress.SetProgress(e.StreamTransferred, e.StreamSize);
                        if (label_update_count > label_update_interval)
                        {
                            progress.labelStatusTotal.Text =
                                string.Format(update_lit_2, e.TotalTransferred);
                        }
                    }
                    if (label_update_count > label_update_interval)
                    {
                        label_update_count = 0;
                    }
                    else
                    {
                        label_update_count += (e.StreamTransferred - stream_transferred_pre_chunk);
                    }
                    stream_transferred_pre_chunk = e.StreamTransferred;
                    break;

                case UpdateProgressReason.JobBegin:
                    label_update_count = 0;
                    progress.Text = Options.GetLiteral(Options.LANG_UPLOAD);
                    break;

                case UpdateProgressReason.JobDone:
                    progress.SetProgress(100, 100);
                    progress.labelSpeed.Text = string.Format(update_lit_0, e.KBytesPerSec);
                    //if (e.EnableTotalProgress)
                    //{
                    //    progress.SetProgressTotal(100, 100);
                    //}
                    progress.labelStatusTotal.Text =
                        string.Format
                        (Options.GetLiteral(Options.LANG_0_BYTES_FROM_1_FILES_UPLOADED), e.TotalTransferred, e.FilesCopied);
                    break;

                case UpdateProgressReason.StreamSwitch:
                    label_update_count = 0;
                    stream_transferred_pre_chunk = 0;
                    progress.labelStatus.Text =
                        string.Format
                        (update_lit_3,
                        e.SourceFile,
                        e.DestinationFile,
                        IOhelper.SizeToString(e.StreamSize));
                    progress.labelSpeed.Text = string.Format(update_lit_0, e.KBytesPerSec);
                    if (e.EnableTotalProgress)
                    {
                        progress.SetProgress(e.TotalTransferred, e.TotalSize);
                        progress.labelStatusTotal.Text =
                            string.Format
                            (update_lit_1,
                            e.TotalTransferred,
                            e.TotalSize,
                            (e.TotalSize == 0) ? 0 : (double)e.TotalTransferred / (double)e.TotalSize);
                    }
                    else
                    {
                        progress.labelStatusTotal.Text =
                            string.Format(update_lit_2, e.TotalTransferred);
                    }
                    break;

                case UpdateProgressReason.TotalSizeCalculated:
                    if (e.EnableTotalProgress)
                    {
                        progress.labelStatusTotal.Text =
                            string.Format
                            (update_lit_1,
                            e.TotalTransferred,
                            e.TotalSize,
                            (e.TotalSize == 0) ? 0 : (double)e.TotalTransferred / (double)e.TotalSize);
                    }
                    break;
            }
        }

        private void update_progress_safe(UpdateProgressArgs e)
        {
            if (progress == null)
            {
                return;
            }

            if (ProgressCloseBeginSafe)
            {
                return;
            }

            try
            {
                progress.Invoke(update_progress_delegate_holder, new object[] { e });
            }
            catch { } //supress all errors
        }

        private void ftp_transfer_proc(long bytes_transferred, ref bool abort_transfer, object state)
        {
            TransferStateObject state_info = (TransferStateObject)state;

            count_bytes_current_transferred = bytes_transferred;
            try
            {
                abort_transfer = progress.CancelPressedSafe;
            }
            catch { }

            UpdateProgressArgs update_args = new UpdateProgressArgs();
            update_args.DestinationFile = state_info.DestinationFile;
            update_args.EnableTotalProgress = opts.ShowTotalProgress;
            update_args.FilesCopied = count_files_processed;
            update_args.KBytesPerSec = (ulong)calc_speed();
            update_args.Reason = UpdateProgressReason.ChunkFinish;
            update_args.SourceFile = state_info.SourceFile;
            update_args.StreamSize = (ulong)state_info.Size;
            update_args.StreamTransferred = (ulong)bytes_transferred;
            update_args.TotalSize = (ulong)total_source_size;
            update_args.TotalTransferred = (ulong)(count_bytes_total_transferred + bytes_transferred);

            update_progress_safe(update_args);
        }

        private long calc_source_size()
        {
            ulong ret = 0;
            int f_count = 0;
            int d_count = 0;
            WIN32_FIND_DATA data = new WIN32_FIND_DATA();
            for (int i = 0; i < initial_source.Length; i++)
            {
                if (WinAPiFSwrapper.GetFileInfo(initial_source[i], ref data))
                {
                    if ((data.dwFileAttributes & FileAttributes.Directory) == FileAttributes.Directory)
                    {
                        ret += WinAPiFSwrapper.GetDirectoryStat
                            (initial_source[i], true, false, true, ref f_count, ref d_count, null);
                    }
                    else
                    {
                        ret += data.FileSize;
                    }
                }
            }
            return (long)ret;
        }

        private void internal_do()
        {
            start_tick = Environment.TickCount;

            UpdateProgressArgs update_args = new UpdateProgressArgs();
            update_args.EnableTotalProgress = opts.ShowTotalProgress;
            update_args.FilesCopied = 0;
            update_args.KBytesPerSec = 0;
            update_args.Reason = UpdateProgressReason.JobBegin;
            update_args.StreamTransferred = 0;
            update_progress_safe(update_args);

            if (opts.ShowTotalProgress)
            {
                total_source_size = calc_source_size();
            }

            WIN32_FIND_DATA source_data = new WIN32_FIND_DATA();
            FtpEntryInfo dest_info=new FtpEntryInfo();
            //bool dest_exists = connection.GetEntryInfo(initial_destination, ref dest_info, false);

            for (int i = 0; i < initial_source.Length; i++)
            {
                if (AbortSafe)
                {
                    break;
                }

                if (!WinAPiFSwrapper.GetFileInfo(initial_source[i], ref source_data))
                {
                    AbortSafe = !process_error
                        (string.Format
                        (Options.GetLiteral(Options.LANG_SOURCE_FILE_0_NOT_FOUND),
                        initial_source[i]),
                        null);
                    continue;
                }

                bool dest_exists = connection.GetEntryInfo(initial_destination, ref dest_info, false);

                if ((source_data.dwFileAttributes & FileAttributes.Directory) == FileAttributes.Directory)
                {
                    if (dest_exists)
                    {
                        if (dest_info.Directory)
                        {
                            upload_directory_recurse
                                (initial_source[i],
                                FtpPath.Combine
                                (initial_destination, source_data.cFileName));
                        }//end of dest is dir
                        else
                        {
                            AbortSafe = !process_error
                                (string.Format
                                (Options.GetLiteral(Options.LANG_WRONG_DESTINATION_SOURCE_0_DIRECTORY_DESTINATION_1_FILE),
                                initial_destination[i],
                                initial_destination),
                                null);
                            continue;
                        }
                    }//end of dest_exists
                    else
                    {
                        if (initial_source.Length == 1)
                        {
                            upload_directory_recurse
                                (initial_source[i],
                                initial_destination);
                        }
                        else
                        {
                            upload_directory_recurse
                                (initial_source[i],
                                FtpPath.Combine(initial_destination, source_data.cFileName));
                        }
                    }//end of dest not exists
                }//end source is dir
                else
                {
                    //source is file
                    if (dest_exists)
                    {
                        if (dest_info.Directory)
                        {
                            upload_one_file
                                (initial_source[i],
                                FtpPath.Combine(initial_destination, source_data.cFileName));
                        }//end of dest is dir
                        else
                        {
                            if (initial_source.Length != 1)
                            {
                                //cannot upload many entries into one file
                                AbortSafe = !process_error
                                    (Options.GetLiteral(Options.LANG_WRONG_DESTINATION_CANNOT_UPLOAD_MANY_ENTRIES_INTO_ONE_FILE),
                                    null);
                                continue;
                            }
                            else
                            {
                                //try overwrite existing
                                upload_one_file(initial_source[i], initial_destination);
                            }
                        }
                    }//end of dest exists
                    else
                    {
                        //dest not exists and source is file
                        if (initial_source.Length == 1)
                        {
                            //assume that dest is new file name
                            upload_one_file(initial_source[i], initial_destination);
                        }
                        else
                        {
                            //assume that dest is new dir
                            upload_one_file
                                (initial_source[i],
                                FtpPath.Combine(initial_destination, source_data.cFileName));
                        }
                    }
                }//end of source is file

                if (UploadItemDone != null)
                {
                    UploadItemDone(this, new ItemEventArs(source_data.cFileName));
                }
            }//end for

            count_bytes_current_transferred = 0;
            //time to notify about job completed
            update_args.EnableTotalProgress = opts.ShowTotalProgress;
            update_args.FilesCopied = count_files_processed;
            update_args.KBytesPerSec = (ulong)calc_speed();
            update_args.Reason = UpdateProgressReason.JobDone;
            update_args.TotalSize = (ulong)total_source_size;
            update_args.TotalTransferred = (ulong)count_bytes_total_transferred;
            update_progress_safe(update_args);

            connection.ClearCache();
            connection.NotifyUpdateNeeded();

            if (Done != null)
            {
                Done(this, new EventArgs());
            }

        }//end proc

        private void upload_directory_recurse(string source_dir, string destination_dir)
        {
            if (AbortSafe)
            {
                return;
            }

            //get fs entries
            //need try
            WinAPiFSwrapper.WIN32_FIND_DATA_enumerable fs_enum = null;
            try
            {
                fs_enum = new WinAPiFSwrapper.WIN32_FIND_DATA_enumerable
                (Path.Combine(source_dir, "*"), false);
            }
            catch (Exception ex)
            {
                AbortSafe = !process_error
                    (string.Format
                    (Options.GetLiteral(Options.LANG_CANNOT_READ_DIRECTORY_CONTENTS_0),
                    source_dir),
                    ex);
            }

            if (fs_enum == null)
            {
                return;
            }

            string source = string.Empty;
            string destination = string.Empty;

            foreach(WIN32_FIND_DATA data in fs_enum)
            {
                if (AbortSafe)
                {
                    return;
                }

                if (data.cFileName == ".")
                {
                    continue;
                }

                if (data.cFileName == "..")
                {
                    continue;
                }

                source = Path.Combine(source_dir, data.cFileName);
                destination = FtpPath.Combine(destination_dir, data.cFileName);
                if ((data.dwFileAttributes & FileAttributes.Directory) == FileAttributes.Directory)
                {
                    upload_directory_recurse(source, destination);
                }
                else
                {
                    //source is file
                    upload_one_file(source, destination);   
                }
            }
        }

        private void upload_one_file(string source_file, string destination_path)
        {
            if (AbortSafe)
            {
                return;
            }

            IntPtr source_handle = IntPtr.Zero;
            FileStream source_stream = null;
            UpdateProgressArgs update_args = new UpdateProgressArgs();

            try
            {
                //open source file
                try
                {
                    source_handle = WinAPiFSwrapper.CreateFileHandle
                        (source_file,
                        Win32FileAccess.GENERIC_READ,
                        FileShare.Read,
                        FileMode.Open,
                        CreateFileOptions.None);
                    source_stream = new FileStream
                        (source_handle,
                        FileAccess.Read,
                        true);

                    //time to notify about begin download file
                    update_args.DestinationFile = destination_path;
                    update_args.EnableTotalProgress = opts.ShowTotalProgress;
                    update_args.FilesCopied = count_files_processed;
                    update_args.KBytesPerSec = (ulong)calc_speed();
                    update_args.Reason = UpdateProgressReason.StreamSwitch;
                    update_args.SourceFile = source_file;
                    update_args.StreamSize = (ulong)source_stream.Length;
                    update_args.StreamTransferred = 0;
                    update_args.TotalSize = (ulong)total_source_size;
                    update_args.TotalTransferred = (ulong)count_bytes_total_transferred;
                    update_progress_safe(update_args);

                }
                catch (Exception ex)
                {
                    AbortSafe = !process_error
                        (string.Format
                        (Options.GetLiteral(Options.LANG_CANNOT_OPEN_SOURCE_FILE_0),
                        source_file),
                        ex);
                }
                if (source_stream == null)
                {
                    return;
                }

                //need check target dir exist and create if needed
                string parent_dir = FtpPath.GetDirectory(destination_path);
                connection.CreateDirectoryTree(parent_dir);

                //check destination is exists
                FtpEntryInfo test_info = new FtpEntryInfo();
                if (connection.GetEntryInfo(destination_path, ref test_info, false))
                {
                    //destination already exists
                    switch (opts.Overwrite)
                    {
                        case OverwriteExisting.IfSourceNewer:
                            //get timestamp
                            DateTime dest_date = connection.GetStamp(destination_path);
                            if (dest_date < File.GetLastWriteTime(source_file))
                            {
                                //delete existing target
                                connection.DeleteFile(destination_path);
                            }
                            else
                            {
                                AbortSafe = !process_error
                                    (string.Format
                                    (Options.GetLiteral(Options.LANG_DESTINATION_0_NEWER_THEN_SOURCE_1_OVERWRITING_PROHIBITED),
                                    destination_path,
                                    source_file),
                                    null);
                                return;
                            }
                            break;

                        case OverwriteExisting.No:
                            AbortSafe = !process_error
                                (string.Format
                                (Options.GetLiteral(Options.LANG_DESTINATION_0_EXIST_OVERWRITING_PROHIBITED),
                                destination_path),
                                null);
                            return;

                        case OverwriteExisting.Yes:
                            connection.DeleteFile(destination_path);
                            break;
                    }
                }

                //update counter
                count_bytes_current_transferred = 0;

                //upload...
                try
                {
                    connection.UploadFile
                        (destination_path,
                        source_stream,
                        transfer_progress_delegate_holder,
                        new TransferStateObject(source_file, destination_path, source_stream.Length),
                        opts.BufferSize);
                }
                catch (Exception ex)
                {
                    AbortSafe = !process_error
                        (string.Format
                        (Options.GetLiteral(Options.LANG_CANNOT_UPLOAD_0_ARROW_1),
                        source_file,
                        destination_path),
                        ex);
                }

                //update counter
                count_bytes_total_transferred += count_bytes_current_transferred;
                count_bytes_current_transferred = 0;
                count_files_processed++;

                //and notify client
                if (UploadItemDone != null)
                {
                    UploadItemDone(this, new ItemEventArs(source_file));
                }

            }
            catch (Exception ex)
            {
                AbortSafe = !process_error
                    (string.Format
                    (Options.GetLiteral(Options.LANG_CANNOT_UPLOAD_0_ARROW_1),
                    source_file,
                    destination_path),
                    ex);
            }
            finally
            {
                if (source_stream != null)
                {
                    source_stream.Close();
                }
            }
        }

        private bool process_error(string info, Exception ex)
        {
            if (opts.SupressErrors)
            {
                return true;
            }
            else
            {
                return Messages.ShowExceptionContinue(ex, info);
            }
        }

        private class TransferStateObject
        {
            public string SourceFile { get; private set; }
            public string DestinationFile { get; private set; }
            public long Size { get; private set; }
            public TransferStateObject(string source, string destination, long size)
            {
                SourceFile = source;
                DestinationFile = destination;
                Size = size;
            }
        }
    }
}
