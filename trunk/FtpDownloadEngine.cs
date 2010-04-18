using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Windows.Forms;
using System.Threading;
using netCommander.FileSystemEx;

namespace netCommander
{
    public class FtpDownloadEngine
    {
        private FtpConnection connection;
        private FtpTransferOptions opts;
        private FtpEntryInfo[] initial_source;
        private string initial_destination;
        private CopyFileProgressDialog progress;
        private MethodInvokerUpdateProgress update_progress_delegate_holder;
        private Thread thread_background;
        private long total_source_size = 0L;
        private FtpTransferProgress transfer_progress_delegate_holder;
        private int count_files_processed = 0;
        private long count_bytes_total_transferred = 0;
        private long count_bytes_current_transferred = 0;
        private int start_tick = 0;

        public event ItemEventHandler DownloadItemDone;
        public event EventHandler Done;

        public FtpDownloadEngine
            (FtpEntryInfo[] source,
            string destination,
            FtpConnection connection,
            FtpTransferOptions opts,
            CopyFileProgressDialog progress_window)
        {
            initial_source = source;
            initial_destination = destination;
            this.connection = connection;
            this.opts = opts;
            progress = progress_window;
            thread_background=new Thread(new ThreadStart(internal_do));
            update_progress_delegate_holder = new MethodInvokerUpdateProgress(update_progress);
            if (progress_window != null)
            {
                progress_window.FormClosing += new FormClosingEventHandler(progress_dialog_FormClosing);
            }
            transfer_progress_delegate_holder = new FtpTransferProgress(ftp_transfer_proc);
        }

        public void Run()
        {
            thread_background.Start();
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

        private string update_lit_0 = Options.GetLiteral(Options.LANG_0_KBYTE_SEC);
        private string update_lit_1 = Options.GetLiteral(Options.LANG_0_BYTES_FROM_1_2P_DOWNLOADED);
        private string update_lit_2 = Options.GetLiteral(Options.LANG_0_BYTES_DOWNLOADED);
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
                    progress.Text = Options.GetLiteral(Options.LANG_DOWNLOAD);
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
                        (Options.GetLiteral(Options.LANG_0_BYTES_FROM_1_FILES_DOWNLOADED),
                        e.TotalTransferred,
                        e.FilesCopied);
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
            update_args.TotalSize=(ulong)total_source_size;
            update_args.TotalTransferred = (ulong)(count_bytes_total_transferred + bytes_transferred);


            update_progress_safe(update_args);
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

            WIN32_FIND_DATA dest_data = new WIN32_FIND_DATA();
            //bool dest_exist = WinAPiFSwrapper.GetFileInfo(initial_destination, ref dest_data);

            foreach (FtpEntryInfo info in initial_source)
            {
                if (AbortSafe)
                {
                    break;
                }

                bool dest_exist = WinAPiFSwrapper.GetFileInfo(initial_destination, ref dest_data);
                if (info.Directory)
                {
                    if (dest_exist)
                    {
                        if ((dest_data.dwFileAttributes & FileAttributes.Directory) == FileAttributes.Directory)
                        {
                            //download source directory into directory=initial_destination/info.ENtryName
                            download_directory(info, Path.Combine(initial_destination, info.EntryName));
                        }
                        else
                        {
                            //show error
                            AbortSafe = !process_error
                                (string.Format
                                (Options.GetLiteral(Options.LANG_WRONG_DESTINATION_SOURCE_0_DIRECTORY_DESTINATION_1_FILE),
                                info.EntryName,
                                initial_destination),
                                null);
                            continue;
                        }
                    }
                    else
                    {
                        //dest not exist
                        if (initial_source.Length == 1)
                        {
                            //that is download one directory to directory with new name
                            download_directory(info, initial_destination);
                        }
                        else
                        {
                            //download driectory INTO initial_destination
                            download_directory(info, Path.Combine(initial_destination, info.EntryName));
                        }
                    }
                }
                else
                {
                    //info is file
                    if (dest_exist)
                    {
                        if ((dest_data.dwFileAttributes & FileAttributes.Directory) == FileAttributes.Directory)
                        {
                            //dest existing dir
                            //download one file into initial destination
                            download_one_file(info, Path.Combine(initial_destination, info.EntryName));
                        }
                        else
                        {
                            //dest is file
                            if (initial_source.Length != 1)
                            {
                                //cannot download many entries into one file
                                AbortSafe = !process_error
                                    (Options.GetLiteral(Options.LANG_WRONG_DESTINATION_CANNOT_DOWNLOAD_MANY_ENTRIES_INTO_ONE_FILE),
                                    null);
                                continue;
                            }
                            else
                            {
                                //overwrite existing
                                download_one_file(info, initial_destination);
                            }
                        }
                    }
                    else
                    {
                        //dest not exist and info is file
                        if (initial_source.Length == 1)
                        {
                            //assume that dest is new file name
                            download_one_file(info, initial_destination);
                        }
                        else
                        {
                            //assume that dest is new dir
                            download_one_file(info, Path.Combine(initial_destination, info.EntryName));
                        }
                    }
                }

                if (DownloadItemDone != null)
                {
                    DownloadItemDone(this, new ItemEventArs(info.EntryName));
                }
            }

            count_bytes_current_transferred = 0;
            //time to notify about job completed
            update_args.EnableTotalProgress = opts.ShowTotalProgress;
            update_args.FilesCopied = count_files_processed;
            update_args.KBytesPerSec = (ulong)calc_speed();
            update_args.Reason = UpdateProgressReason.JobDone;
            update_args.TotalSize = (ulong)total_source_size;
            update_args.TotalTransferred = (ulong)count_bytes_total_transferred;
            update_progress_safe(update_args);

            if (Done != null)
            {
                Done(this, new EventArgs());
            }
        }

        private void download_directory(FtpEntryInfo source_dir_info, string dest_dir)
        {
            //we need list of dir contents
            List<FtpEntryInfo> contents = new List<FtpEntryInfo>();
            string dest_file = string.Empty;

            try
            {
                contents = connection.GetDirectoryDetailsList
                    (FtpPath.AppendEndingSeparator
                    (FtpPath.Combine(source_dir_info.DirectoryPath, source_dir_info.EntryName)),
                    true);
            }
            catch (Exception ex)
            {
                AbortSafe = !process_error
                    (string.Format
                    (Options.GetLiteral(Options.LANG_CANNOT_READ_DIRECTORY_CONTENTS_0),
                    FtpPath.Combine(source_dir_info.DirectoryPath, source_dir_info.EntryName)),
                    ex);
            }

            //if success
            foreach (FtpEntryInfo info in contents)
            {
                if (AbortSafe)
                {
                    break;
                }

                dest_file = Path.Combine(dest_dir, info.EntryName);
                if (info.Directory)
                {
                    //directory - call recursively
                    download_directory(info, dest_file);
                }
                else
                {
                    //that is file
                    download_one_file
                        (info,
                        dest_file);
                }
            }

        }

        private void download_one_file(FtpEntryInfo source_info, string dest_file)
        {
            IntPtr dest_handle = IntPtr.Zero;
            FileStream dest_stream = null;
            UpdateProgressArgs update_args = new UpdateProgressArgs();
            count_bytes_current_transferred = 0;

            if (progress != null)
            {
                try
                {
                    AbortSafe = progress.CancelPressedSafe;
                }
                catch { }
            }

            if (AbortSafe)
            {
                return;
            }

            try
            {
                //time to notify about begin download file
                update_args.DestinationFile = dest_file;
                update_args.EnableTotalProgress = opts.ShowTotalProgress;
                update_args.FilesCopied = count_files_processed;
                update_args.KBytesPerSec = (ulong)calc_speed();
                update_args.Reason = UpdateProgressReason.StreamSwitch;
                update_args.SourceFile = FtpPath.Combine(source_info.DirectoryPath, source_info.EntryName);
                update_args.StreamSize = (ulong)source_info.Size;
                update_args.StreamTransferred = 0;
                update_args.TotalSize = (ulong)total_source_size;
                update_args.TotalTransferred = (ulong)count_bytes_total_transferred;
                update_progress_safe(update_args);

                //destination file exist?
                WIN32_FIND_DATA dest_data = new WIN32_FIND_DATA();
                bool dest_exist = WinAPiFSwrapper.GetFileInfo(dest_file, ref dest_data);

                DateTime source_date = connection.GetStamp(FtpPath.Combine(source_info.DirectoryPath, source_info.EntryName));

                if (dest_exist)
                {
                    switch (opts.Overwrite)
                    {
                        case OverwriteExisting.No:
                            //overwrite prohibited
                            AbortSafe = !process_error
                                (string.Format
                                (Options.GetLiteral(Options.LANG_DESTINATION_0_EXIST_OVERWRITING_PROHIBITED),
                                dest_file),
                                null);
                            //and return
                            return;

                        case OverwriteExisting.IfSourceNewer:
                            //check file date
                            //get datetime from server
                            
                            if (source_date <= DateTime.FromFileTime(dest_data.ftLastWriteTime))
                            {
                                //source older
                                AbortSafe = !process_error
                                    (string.Format
                                    (Options.GetLiteral(Options.LANG_DESTINATION_0_NEWER_THEN_SOURCE_1_OVERWRITING_PROHIBITED),
                                    dest_file,
                                    source_info.EntryName),
                                    null);
                                //return
                                return;
                            }
                            break;
                    }
                }

                //now we can overwrite destination if it exists

                //create destination directory
                WinAPiFSwrapper.CreateDirectoryTree(Path.GetDirectoryName(dest_file), string.Empty);

                //open dest file (handle will be close at finally block with stream.close())
                dest_handle = WinAPiFSwrapper.CreateFileHandle
                    (dest_file,
                    Win32FileAccess.GENERIC_WRITE,
                    FileShare.Read,
                    FileMode.Create,
                    CreateFileOptions.None);

                //create destination stream
                dest_stream = new FileStream
                (dest_handle,
                FileAccess.Write,
                true);

                //set destinstion length
                dest_stream.SetLength(source_info.Size);

                //and call download
                long transferred = connection.DownloadFile
                    (FtpPath.Combine(source_info.DirectoryPath, source_info.EntryName),
                    dest_stream,
                    transfer_progress_delegate_holder,
                    new TransferStateObject(FtpPath.Combine(source_info.DirectoryPath, source_info.EntryName), dest_file, source_info.Size),
                    opts.BufferSize);

                dest_stream.Close();

                //and set attributes
                File.SetLastWriteTime(dest_file, source_date);

                //now time to update count
                count_files_processed++;
                count_bytes_total_transferred += count_bytes_current_transferred;
            }
            catch (Exception ex)
            {
                AbortSafe = !process_error
                    (string.Format(Options.GetLiteral(Options.LANG_CANNOT_DOWNLOAD_0), source_info.EntryName),
                    ex);
            }
            finally
            {
                if (dest_stream != null)
                {
                    dest_stream.Close();
                }
            }
        }

        private long calc_source_size()
        {
            long ret = 0L;

            foreach (FtpEntryInfo info in initial_source)
            {
                if (info.Directory)
                {
                    ret += calc_dir_size(FtpPath.AppendEndingSeparator(FtpPath.Combine(info.DirectoryPath, info.EntryName)));
                }
                else
                {
                    ret += info.Size;
                }
            }

            //notify about source size calculated
            UpdateProgressArgs e = new UpdateProgressArgs();
            e.EnableTotalProgress = opts.ShowTotalProgress;
            e.Reason = UpdateProgressReason.TotalSizeCalculated;
            e.TotalSize = (ulong)ret;
            update_progress_safe(e);
            
            return ret;
        }

        private long calc_dir_size(string dir_name)
        {
            long ret = 0L;

            //get file info list from server
            List<FtpEntryInfo> dir_list = new List<FtpEntryInfo>();
            try
            {
                dir_list = connection.GetDirectoryDetailsList(dir_name, false);
            }
            catch (Exception ex)
            {
                AbortSafe = !process_error
                    (string.Format
                    (Options.GetLiteral(Options.LANG_CANNOT_READ_DIRECTORY_CONTENTS_0),
                    dir_name),
                    ex);
            }
            if (!AbortSafe)
            {
                foreach (FtpEntryInfo info in dir_list)
                {
                    if (info.Directory)
                    {
                        ret += calc_dir_size(FtpPath.AppendEndingSeparator(FtpPath.Combine(info.DirectoryPath, info.EntryName)));
                    }
                    else
                    {
                        ret += info.Size;
                    }
                }
            }
            return ret;
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
            public TransferStateObject(string source,string destination,long size)
            {
                SourceFile = source;
                DestinationFile = destination;
                Size = size;
            }
        }
    }

    public struct FtpTransferOptions
    {
        public int BufferSize;
        public bool SupressErrors;
        public bool ShowTotalProgress;
        public OverwriteExisting Overwrite;

        public byte[] Serialize()
        {
            byte[] ret = new byte[10];

            byte[] int_bytes = BitConverter.GetBytes(BufferSize);
            Array.Copy(int_bytes, 0, ret, 0, 4);

            byte[] bool_bytes = BitConverter.GetBytes(SupressErrors);
            Array.Copy(bool_bytes, 0, ret, 4, 1);

            bool_bytes = BitConverter.GetBytes(ShowTotalProgress);
            Array.Copy(bool_bytes, 0, ret, 5, 1);

            int_bytes = BitConverter.GetBytes((int)Overwrite);
            Array.Copy(int_bytes, 0, ret, 6, 4);

            return ret;
        }

        public static FtpTransferOptions FromBytes(byte[] bytes)
        {
            if (bytes.Length != 10)
            {
                throw new ApplicationException(Options.GetLiteral(Options.LANG_CANNOT_READ_FTP_TRANSFER_SETTINGS));
            }

            FtpTransferOptions ret = new FtpTransferOptions();

            ret.BufferSize = BitConverter.ToInt32(bytes, 0);
            ret.SupressErrors = BitConverter.ToBoolean(bytes, 4);
            ret.ShowTotalProgress = BitConverter.ToBoolean(bytes, 5);
            ret.Overwrite = (OverwriteExisting)BitConverter.ToInt32(bytes, 6);

            return ret;
        }

        public static FtpTransferOptions Default()
        {
            FtpTransferOptions ret = new FtpTransferOptions();
            ret.BufferSize = 2048;
            ret.Overwrite = OverwriteExisting.No;
            ret.ShowTotalProgress = true;
            ret.SupressErrors = false;

            return ret;
        }
    }

    public enum OverwriteExisting
    {
        No,
        IfSourceNewer,
        Yes
    }
}
