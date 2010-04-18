using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using netCommander.FileSystemEx;
using System.Runtime.InteropServices;
using System.ComponentModel;
using System.Threading;

namespace netCommander
{
    public class CopyFileEngine : IDisposable
    {
        private CopyEngineOptions options;
        private CopyFileExOptions copyFileEx_options;
        private CopyProgressRoutine copy_progress_routine_delegate_holder;
        private IntPtr callback_data = IntPtr.Zero;
        private bool abort_job = false;
        private CopyFileProgressDialog progress_dialog;
        private List<FileInfoEx> initial_source;
        private string initial_destination;
        private string initial_mask = "*";
        private bool destination_remote = false;

        private ulong total_source_bytes = 0;
        private ulong total_bytes_transferred = 0;
        private ulong current_file_bytes_transferred = 0;
        private int total_files_copied = 0;
        private int start_tick = 0;

        //private bool source_is_streams = false;
        //private bool destination_is_stream = false;

        private Thread copy_thread = null;
        private MethodInvokerUpdateProgress update_progress_delegate_holder;

        public event ItemEventHandler CopyItemDone;
        public event EventHandler Done;

        public void Do()
        {
            copy_thread.Start();
        }

        #region initializator - call from UI thread

		[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1805:DoNotInitializeUnnecessarily")]
        public CopyFileEngine
            (List<FileInfoEx> source,
            string destination,
            CopyEngineOptions options,
            CopyFileProgressDialog dialog,
            string mask,
            bool dest_remote)
        {
            this.options = options;
            copy_progress_routine_delegate_holder = new CopyProgressRoutine(copy_progress_proc);

            copyFileEx_options = CopyFileExOptions.None;
            if ((options & CopyEngineOptions.AllowDecryptDestination) == CopyEngineOptions.AllowDecryptDestination)
            {
                copyFileEx_options = copyFileEx_options | CopyFileExOptions.ALLOW_DECRYPTED_DESTINATION;
            }
            if (Options.IsVistaOrServer2003OrLater())
            {
                if ((options & CopyEngineOptions.CopySymlinkAsSymlink) == CopyEngineOptions.CopySymlinkAsSymlink)
                {
                    copyFileEx_options = copyFileEx_options | CopyFileExOptions.COPY_SYMLINK;
                }
            }
            if ((options & CopyEngineOptions.NoRewrite) == CopyEngineOptions.NoRewrite)
            {
                copyFileEx_options = copyFileEx_options | CopyFileExOptions.FAIL_IF_EXISTS;
            }
            if ((options & CopyEngineOptions.RewriteIfSourceNewer) == CopyEngineOptions.RewriteIfSourceNewer)
            {
                copyFileEx_options = copyFileEx_options | CopyFileExOptions.FAIL_IF_EXISTS;
            }

            progress_dialog = dialog;
            if (progress_dialog != null)
            {
                progress_dialog.FormClosing += new System.Windows.Forms.FormClosingEventHandler
                    (progress_dialog_FormClosing);
            }

            initial_source = source;
            initial_destination = destination;
            initial_mask = mask;
            destination_remote = dest_remote;

            update_progress_delegate_holder = new MethodInvokerUpdateProgress(update_progress);
            copy_thread = new Thread(internal_do);
        }

        private ulong calc_total_source_size()
        {
            ulong ret = 0UL;

            foreach (FileInfoEx info in initial_source)
            {
                ret += calc_size(info);
            }

            return ret;
        }

        private ulong calc_size(FileInfoEx info)
        {
            ulong ret = 0UL;

            if (((options & CopyEngineOptions.CopyFilesRecursive) == CopyEngineOptions.CopyFilesRecursive) &&
                (info.Directory))
            {
                if ((destination_remote) || (((options & CopyEngineOptions.CopySymlinkAsSymlink) != CopyEngineOptions.CopySymlinkAsSymlink) || (info.LinkInformation == null)))
                {
                    FileInfoExEnumerable dir_enum = new FileInfoExEnumerable
                    (Path.Combine(info.FullName, "*"),
                    true,
                    true,
                    true,
                    true);
                    foreach (FileInfoEx ch_info in dir_enum)
                    {
                        ret += calc_size(ch_info);
                    }
                }
            }
            else
            {
                if (Wildcard.Match(initial_mask, info.FileName, false))
                {
                    ret += info.Size;
                }
            }
            return ret;
        }

        #endregion

        #region interact with progress window
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
            }
        }

        void progress_dialog_FormClosing(object sender, System.Windows.Forms.FormClosingEventArgs e)
        {
            //progress dialog before user or system close
            //call from UI thread!

            //thread safe set abort_job flag - will be check by file enum functions 
            stop();

            //set cancel flag - will be check by CopyFileEx callback
            progress_dialog.CancelPressedSafe = true;

            //set internal flag - will will be check by CopyFileEx callback
            ProgressCloseBeginSafe = true;
        }

        /// <summary>
        /// returns ticks elapsed from start_tick
        /// must be call from back tread
        /// </summary>
        /// <returns></returns>
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
        private ulong calc_speed()
        {
            int job_ticks = calc_tick_elapsed();
            if (job_ticks <= 0)
            {
                return int.MaxValue;
            }

            ulong ret_ulong_msec = (total_bytes_transferred + current_file_bytes_transferred) / (ulong)job_ticks;
            //now ret_ulong_msec is bytes per millisecond

            //calc bytes per sec
            ulong ret = ret_ulong_msec * 1000U;

            //and Kbyte per sec
            ret = ret / 1024U;

            return ret;
        }

        //callback from CopyFileEx, call from back back thread
        private CopyFileExCallbackReturns copy_progress_proc(UInt64 TotalFileSize,
            UInt64 TotalBytesTransferred,
            UInt64 StreamSize,
            UInt64 StreamBytesTransferred,
            int dwStreamNumber,
            CopyFileExState dwCallbackReason,
            IntPtr hSourceFile,
            IntPtr hDestinationFile,
            IntPtr lpData)
        {

            //early exit if progress dialog absent or begin close
            if (progress_dialog == null)
            {
                return CopyFileExCallbackReturns.QUIET;
            }
            if (ProgressCloseBeginSafe)
            {
                stop();
                return CopyFileExCallbackReturns.CANCEL;
            }

            UpdateProgressArgs e = new UpdateProgressArgs();
            current_file_bytes_transferred = TotalBytesTransferred;
            ulong speed = calc_speed();
            if (dwCallbackReason == CopyFileExState.STREAM_SWITCH)
            {
                //new stream begin copy

                //retrieve file names from lpData
                string src = string.Empty;
                string dst = string.Empty;
                IOhelper.strings_from_buffer(ref src, ref dst, lpData);

                //set update args
                e.DestinationFile = dst;
                e.SourceFile = src;
                e.Reason = UpdateProgressReason.StreamSwitch;
            }
            else
            {
                e.Reason = UpdateProgressReason.ChunkFinish;
            }

            //set update args
            e.KBytesPerSec = speed;
            e.StreamSize = StreamSize;
            e.StreamTransferred = StreamBytesTransferred;
            e.TotalTransferred = total_bytes_transferred + current_file_bytes_transferred;
            e.FilesCopied = total_files_copied;
            e.EnableTotalProgress =
                ((options & CopyEngineOptions.CalculateTotalSize) == CopyEngineOptions.CalculateTotalSize);
            e.TotalSize = total_source_bytes;

            //check progress dialog
            if (ProgressCloseBeginSafe)
            {
                stop();
                return CopyFileExCallbackReturns.CANCEL;
            }

            //and try invoke
            //sync errors may be if progress dialog close before invoking
            update_progress_safe(e);

            if (ProgressCloseBeginSafe)
            {
                stop();
                return CopyFileExCallbackReturns.CANCEL;
            }

            //need try becouse sync error may occur if progress_dialog closed
            try
            {
                if (progress_dialog.CancelPressedSafe)
                {
                    stop();
                    return CopyFileExCallbackReturns.CANCEL;
                }
                else
                {
                    return CopyFileExCallbackReturns.CONTINUE;
                }
            }
            catch
            {
                //and if error occur -> stop copy
                stop();
                return CopyFileExCallbackReturns.CANCEL;
            }
        }

        private void update_progress_safe(UpdateProgressArgs e)
        {
            if (progress_dialog == null)
            {
                return;
            }

            if (ProgressCloseBeginSafe)
            {
                return;
            }

            //supress sunc errors
            try
            {
                progress_dialog.Invoke(update_progress_delegate_holder, new object[] { e });
            }
            catch { }
        }

        private string update_lit_0 = Options.GetLiteral(Options.LANG_0_KBYTE_SEC);
        private string update_lit_1 = Options.GetLiteral(Options.LANG_0_BYTES_FROM_1_2P_TRANSFERRED);
        private string update_lit_2 = Options.GetLiteral(Options.LANG_0_BYTES_TRANSFERRED);
        private string update_lit_3 = Options.GetLiteral(Options.LANG_0_ARROW_1_2);
        private ulong label_update_interval = (ulong)Options.ProgressUpdateInterval;
        private ulong label_update_count = 0;
        private ulong stream_transferred_pre_chunk = 0;
        private void update_progress(UpdateProgressArgs e)
        {
            //will be invoke in UI thread - canot use anything else e and progress_dialog members
            switch (e.Reason)
            {
                case UpdateProgressReason.ChunkFinish:
                    //progress_dialog.SetProgressStream(e.StreamTransferred, e.StreamSize);
                    if (label_update_count > label_update_interval)
                    {
                        progress_dialog.labelSpeed.Text = string.Format(update_lit_0, e.KBytesPerSec);
                    }
                    if (e.EnableTotalProgress)
                    {
                        progress_dialog.SetProgress(e.TotalTransferred, e.TotalSize);
                        if (label_update_count > label_update_interval)
                        {
                            progress_dialog.labelStatusTotal.Text =
                                string.Format
                                (update_lit_1,
                                e.TotalTransferred,
                                e.TotalSize,
                                (e.TotalSize == 0) ? 0 : (double)e.TotalTransferred / (double)e.TotalSize);
                        }
                    }
                    else
                    {
                        progress_dialog.SetProgress(e.StreamTransferred, e.StreamSize);
                        if (label_update_count > label_update_interval)
                        {
                            progress_dialog.labelStatusTotal.Text =
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
                    progress_dialog.Text = Options.GetLiteral(Options.LANG_COPY);
                    break;

                case UpdateProgressReason.JobDone:
                    label_update_count = 0;
                    progress_dialog.SetProgress(100, 100);
                    progress_dialog.labelSpeed.Text = string.Format(update_lit_0, e.KBytesPerSec);
                    //if (e.EnableTotalProgress)
                    //{
                    //    progress_dialog.SetProgress(100, 100);
                    //}
                    progress_dialog.labelStatusTotal.Text =
                        string.Format
                        (Options.GetLiteral(Options.LANG_0_BYTES_FROM_1_FILES_TRANSFERRED),
                        e.TotalTransferred,
                        e.FilesCopied);
                    break;

                case UpdateProgressReason.StreamSwitch:
                    stream_transferred_pre_chunk = 0;
                    label_update_count = 0;
                    progress_dialog.labelStatus.Text =
                        string.Format
                        (update_lit_3,
                        e.SourceFile,
                        e.DestinationFile,
                        IOhelper.SizeToString(e.StreamSize));
                    progress_dialog.labelSpeed.Text = string.Format(update_lit_0, e.KBytesPerSec);
                    if (e.EnableTotalProgress)
                    {
                        progress_dialog.SetProgress(e.TotalTransferred, e.TotalSize);
                        progress_dialog.labelStatusTotal.Text =
                            string.Format
                            (update_lit_1,
                            e.TotalTransferred,
                            e.TotalSize,
                            (e.TotalSize == 0) ? 0 : (double)e.TotalTransferred / (double)e.TotalSize);
                    }
                    else
                    {
                        progress_dialog.labelStatusTotal.Text =
                            string.Format(update_lit_2, e.TotalTransferred);
                    }
                    break;

                case UpdateProgressReason.TotalSizeCalculated:
                    if (e.EnableTotalProgress)
                    {
                        progress_dialog.labelStatusTotal.Text =
                            string.Format
                            (update_lit_1,
                            e.TotalTransferred,
                            e.TotalSize,
                            (e.TotalSize == 0) ? 0 : (double)e.TotalTransferred / (double)e.TotalSize);
                    }
                    break;
            }
        }

        #endregion

        #region internal copy proc

        private void internal_do()
        {
            //back thread
            UpdateProgressArgs e_begin = new UpdateProgressArgs();
            e_begin.Reason = UpdateProgressReason.JobBegin;
            update_progress_safe(e_begin);

            //calc total source size, if needed
            if ((options & CopyEngineOptions.CalculateTotalSize) == CopyEngineOptions.CalculateTotalSize)
            {
                total_source_bytes = calc_total_source_size();
                UpdateProgressArgs e_total_calc = new UpdateProgressArgs();
                e_total_calc.KBytesPerSec = 0;
                e_total_calc.DestinationFile = string.Empty;
                e_total_calc.EnableTotalProgress = true;
                e_total_calc.FilesCopied = 0;
                e_total_calc.Reason = UpdateProgressReason.TotalSizeCalculated;
                e_total_calc.SourceFile = string.Empty;
                e_total_calc.StreamSize = 0;
                e_total_calc.StreamTransferred = 0;
                e_total_calc.TotalSize = total_source_bytes;
                e_total_calc.TotalTransferred = 0;
                update_progress_safe(e_total_calc);
            }

            //set begin job time
            start_tick = Environment.TickCount;

            FileInfoEx dest_info = new FileInfoEx();
            bool dest_exist = false;

            //go throw initial_source
            foreach (FileInfoEx source_info in initial_source)
            {
                if (abort_job)
                {
                    break;
                }

                //check destination
                if (!dest_exist)
                {
                    dest_exist = FileInfoEx.TryGet(initial_destination, ref dest_info);
                }

                if (dest_exist)
                {
                    //destination exists
                    if (dest_info.Directory)
                    {
                        if (source_info.Directory)
                        {
                            copy_dir(source_info, Path.Combine(initial_destination, source_info.FileName));
                        }
                        else
                        {
                            copy_one_file(source_info, Path.Combine(initial_destination, source_info.FileName));
                        }
                    }//end of destination is directory
                    else
                    {
                        if ((initial_source.Count == 1) && (!source_info.Directory))
                        {
                            copy_one_file(source_info, initial_destination);
                        }
                        else
                        {
                            process_error
                                (string.Format
                                (Options.GetLiteral(Options.LANG_WRONG_DESTINATION_0_IS_EXISTING_FILE),
                                initial_destination),
                                null);
                            stop();
                        }
                    }//end of destination is file
                }//end of dest exists
                else
                {
                    if (initial_source.Count == 1)
                    {
                        if (source_info.Directory)
                        {
                            //tract initial_destination as new directory:
                            copy_dir(source_info, initial_destination);
                        }
                        else
                        {
                            //or new file
                            copy_one_file(source_info, initial_destination);
                        }
                    }//end of single source
                    else
                    {
                        //initial destination will be directory
                        //and copy INTO it
                        if (source_info.Directory)
                        {
                            copy_dir(source_info, Path.Combine(initial_destination, source_info.FileName));
                        }
                        else
                        {
                            copy_one_file(source_info, Path.Combine(initial_destination, source_info.FileName));
                        }
                    }//source not single
                }//end of dest not exists
            }//end foreach

            UpdateProgressArgs e_finish = new UpdateProgressArgs();
            e_finish.KBytesPerSec = calc_speed();
            e_finish.DestinationFile = string.Empty;
            e_finish.EnableTotalProgress = ((options & CopyEngineOptions.CalculateTotalSize) == CopyEngineOptions.CalculateTotalSize);
            e_finish.FilesCopied = total_files_copied;
            e_finish.Reason = UpdateProgressReason.JobDone;
            e_finish.SourceFile = string.Empty;
            e_finish.StreamSize = 0;
            e_finish.StreamTransferred = 0;
            e_finish.TotalSize = total_source_bytes;
            e_finish.TotalTransferred = total_bytes_transferred;
            update_progress_safe(e_finish);

            if (Done != null)
            {
                Done(this, new EventArgs());
            }
        }

        private object stop_lock = new object();
        /// <summary>
        /// call this to safe abort file enum
        /// </summary>
        private void stop()
        {
            lock (stop_lock)
            {
                abort_job = true;
            }
        }

        private void copy_dir(FileInfoEx source_info, string destination_dir)
        {
            if ((source_info.LinkInformation != null) && (!destination_remote) && ((options & CopyEngineOptions.CopySymlinkAsSymlink) == CopyEngineOptions.CopySymlinkAsSymlink))
            {
                try
                {
                    //copy link as link and returns
                    Directory.CreateDirectory
                        (destination_dir);
                    switch (source_info.ReparseTag)
                    {
                        case IO_REPARSE_TAG.MOUNT_POINT:
                            WinAPiFSwrapper.SetMountpoint(destination_dir, source_info.LinkInformation.SubstituteName,source_info.LinkInformation.PrintName);
                            break;

                        case IO_REPARSE_TAG.SYMLINK:
                            //need test
                            //link_target and unparsed prefix! need testing behavior of CreateSymbolicLink
                            WinAPiFSwrapper.CreateSymbolicLink(destination_dir, source_info.LinkInformation.SubstituteName, true);
                            //WinAPiFSwrapper.SetSymbolicLink(destination_dir, source_info.LinkInformation.SubstituteName, source_info.LinkInformation.PrintName, false);
                            break;
                    }
                }
                catch (Exception ex)
                {
                    if (!process_error
                        (string.Format
                        (Options.GetLiteral(Options.LANG_CANNOT_CREATE_SOFT_LINK_0),
                        destination_dir),
                        ex))
                    {
                        stop();
                    }
                }
                OnCopyItemDone(new ItemEventArs(source_info.FileName));
                return;
            }

            if ((options & CopyEngineOptions.CopyFilesRecursive) == CopyEngineOptions.CopyFilesRecursive)
            {
                FileInfoExEnumerable dir_enum = new FileInfoExEnumerable
                (Path.Combine(source_info.FullName, "*"),
                true,
                true,
                true,
                true);
                foreach (FileInfoEx info in dir_enum)
                {
                    if (abort_job)
                    {
                        return;
                    }
                    if (info.Directory)
                    {
                        copy_dir(info, Path.Combine(destination_dir, info.FileName));
                    }
                    else
                    {
                        copy_one_file(info, Path.Combine(destination_dir, info.FileName));
                    }
                }
            }

            if ((options & CopyEngineOptions.CreateEmptyDirectories) == CopyEngineOptions.CreateEmptyDirectories)
            {
                create_empty_dir(source_info.FullName, destination_dir);
                OnCopyItemDone(new ItemEventArs(source_info.FileName));
            }
        }

        private void copy_one_file(FileInfoEx source_info, string destination_file)
        {
            //check mask
            if (!Wildcard.Match(initial_mask, source_info.FileName))
            {
                return;
            }

            bool success = false;

            //now prepare callback buffer
            if (callback_data != IntPtr.Zero)
            {
                Marshal.FreeHGlobal(callback_data);
            }
            callback_data = IOhelper.strings_to_buffer(source_info.FullName, destination_file);

            //prepare uni names
            string source_uni = IOhelper.GetUnicodePath(source_info.FullName);
            string dest_uni = IOhelper.GetUnicodePath(destination_file);

            //string source_uni = source_info.FullName;
            //string dest_uni = destination_file;

            //and call CopyFileEx first time
            int res = WinApiFS.CopyFileEx
                (source_uni,
                dest_uni,
                copy_progress_routine_delegate_holder,
                callback_data,
                IntPtr.Zero,
                copyFileEx_options);

            if (res == 0)
            {
                int win_err = Marshal.GetLastWin32Error();
                //process win_err and do second take if needed
                CopyFileExOptions temp_copy_options = copyFileEx_options;
                if (process_CopyFileEx_errors(source_info, destination_file, win_err, ref temp_copy_options))
                {
                    //second take
                    res = WinApiFS.CopyFileEx
                        (source_uni,
                        dest_uni,
                        copy_progress_routine_delegate_holder,
                        callback_data,
                        IntPtr.Zero,
                        temp_copy_options);
                    if (res == 0)
                    {
                        win_err = Marshal.GetLastWin32Error();
                        Win32Exception win_ex_2 = new Win32Exception(win_err);
                        if (!process_error
                            (string.Format
                            (Options.GetLiteral(Options.LANG_CANNOT_COPY_0_ARROW_1),
                            source_info.FullName,
                            destination_file),
                            win_ex_2))
                        {
                            stop();
                        }
                    }
                    else
                    {
                        success = true;
                    }
                }
            }//end of res==0 brunch
            else
            {
                //copy success
                success = true;
            }

            if (success)
            {
                //update counts
                total_bytes_transferred += current_file_bytes_transferred;
                current_file_bytes_transferred = 0;
                total_files_copied++;

                //time to notify current panel -> unset selection
                ItemEventArs e_file_done = new ItemEventArs(source_info.FileName);
                OnCopyItemDone(e_file_done);

                //set sec attributes if needed
                if ((options & CopyEngineOptions.CopySecurityAttributes) == CopyEngineOptions.CopySecurityAttributes)
                {
                    try
                    {
                        File.SetAccessControl(destination_file, File.GetAccessControl(source_info.FullName));
                    }
                    catch (Exception ex)
                    {
                        if (!process_error
                            (string.Format
                            (Options.GetLiteral(Options.LANG_CANNOT_SET_SEC_ATTRIBUTES_0),
                            destination_file),
                            ex))
                        {
                            stop();
                        }
                    }
                }
            }
        }

        /// <summary>
        /// create new empty dir, copy security attr if needed
        /// </summary>
        /// <param name="source_dir">may be empty string</param>
        /// <param name="destination_dir"></param>
        private void create_empty_dir(string source_dir, string destination_dir)
        {
            WIN32_FIND_DATA dest_dir_data = new WIN32_FIND_DATA();
            if (WinAPiFSwrapper.GetFileInfo(destination_dir, ref dest_dir_data))
            {
                //already exists
                return;
            }
            else
            {
            	if ((!string.IsNullOrEmpty(source_dir)) && ((options & CopyEngineOptions.CopySecurityAttributes) == CopyEngineOptions.CopySecurityAttributes))
                {
                    Directory.CreateDirectory(destination_dir, Directory.GetAccessControl(source_dir));
            	}
                else
                {
                    Directory.CreateDirectory(destination_dir);
                }
            }
        }

        private bool clear_dest_attributes(string destination_file, WIN32_FIND_DATA dest_data)
        {
            FileAttributes fa_existing = dest_data.dwFileAttributes;
            FileAttributes fa_needed = dest_data.dwFileAttributes;

            if ((fa_existing & FileAttributes.Hidden) == FileAttributes.Hidden)
            {
                if ((options & CopyEngineOptions.AllowClearAttributes) == CopyEngineOptions.AllowClearAttributes)
                {
                    fa_needed = ~((~fa_existing) | FileAttributes.Hidden);
                }
                else
                {
                    if (!process_error
                        (string.Format
                        (Options.GetLiteral(Options.LANG_DESTINATION_0_EXISTS_AND_HIDDEN_CLEARING_ATTRS_PROHIBITED),
                        destination_file),
                        null))
                    {
                        stop();
                    }
                    return false;
                }
            }
            if ((fa_existing & FileAttributes.ReadOnly) == FileAttributes.ReadOnly)
            {
                if ((options & CopyEngineOptions.AllowClearAttributes) == CopyEngineOptions.AllowClearAttributes)
                {
                    fa_needed = ~((~fa_existing) | FileAttributes.ReadOnly);
                }
                else
                {
                    if (!process_error
                        (string.Format
                        (Options.GetLiteral(Options.LANG_DESTINATION_0_EXISTS_AND_READONLY_CLEARING_ATTRS_PROHIBITED),
                        destination_file),
                        null))
                    {
                        stop();
                    }
                    return false;
                }
            }

            if (fa_existing != fa_needed)
            {
                try
                {
                    WinAPiFSwrapper.SetFileAttributes(destination_file, fa_needed);
                }
                catch (Exception ex)
                {
                    if (!process_error
                        (string.Format
                        (Options.GetLiteral(Options.LANG_CANNOT_CLEAR_HIDDEN_READONLY_ATTR_DESTINATION_0),
                        destination_file),
                        ex))
                    {
                        stop();
                    }
                    return false;
                }
            }

            return true;
        }

        /// <summary>
        /// returns true if source is newer, destinstion must exists
        /// </summary>
        /// <param name="source_file"></param>
        /// <param name="source_data"></param>
        /// <param name="destination_file"></param>
        /// <returns></returns>
        private static bool check_file_dates(WIN32_FIND_DATA source_data, WIN32_FIND_DATA destination_data)
        {
            return (source_data.ftLastWriteTime > destination_data.ftLastWriteTime);
        }

        /// <summary>
        /// show error window if needed and returns do second take or no, if no caller must continue to next source
        /// </summary>
        /// <param name="source"></param>
        /// <param name="src_data"></param>
        /// <param name="destination"></param>
        /// <param name="win_error"></param>
        /// <returns></returns>
        private bool process_CopyFileEx_errors(FileInfoEx source_info, string destination, int win_error, ref CopyFileExOptions temp_options)
        {
            WIN32_FIND_DATA dst_data = new WIN32_FIND_DATA();

            switch (win_error)
            {
                case WinApiFS.ERROR_PATH_NOT_FOUND: //ERROR_PATH_NOT_FOUND
                    //there is no target directory, create it
                    string src_dir_3 = Path.GetDirectoryName(source_info.FullName);
                    string dst_dir_3 = Path.GetDirectoryName(destination);
                    try
                    {
                        create_empty_dir(src_dir_3, dst_dir_3);
                    }
                    catch (Exception ex_3)
                    {
                        if (process_error
                            (string.Format
                            (Options.GetLiteral(Options.LANG_CANNOT_CREATE_DIRECTORY_0),
                            dst_dir_3),
                            ex_3))
                        {
                            return false;
                        }
                        else
                        {
                            stop();
                            return false;
                        }
                    }
                    return true;

                case WinApiFS.ERROR_FILE_EXISTS: //ERROR_FILE_EXISTS
                    if ((options & CopyEngineOptions.RewriteIfSourceNewer) == CopyEngineOptions.RewriteIfSourceNewer)
                    {
                        //overwrite enable if source newer
                        WinAPiFSwrapper.GetFileInfo(destination, ref dst_data);
                        if (source_info.WriteFileTime > dst_data.ftLastWriteTime)
                        {
                            temp_options = ~((~temp_options) | CopyFileExOptions.FAIL_IF_EXISTS);
                            return clear_dest_attributes(destination, dst_data);
                        }
                        else
                        {
                            if (process_error
                                (string.Format
                                (Options.GetLiteral(Options.LANG_DESTINATION_0_NEWER_THEN_SOURCE_1_OVERWRITING_PROHIBITED),
                                destination,
                                source_info.FullName),
                                null))
                            {
                                return false;
                            }
                            else
                            {
                                stop();
                                return false;
                            }
                        }
                    }

                    //destination exists and overwrite prohibited
                    if (process_error
                        (string.Format
                        (Options.GetLiteral(Options.LANG_DESTINATION_0_EXIST_OVERWRITING_PROHIBITED),
                        destination),
                        null))
                    {
                        return false;
                    }
                    else
                    {
                        stop();
                        return false;
                    }

                case 5: //ERROR_ACCESS_DENIED
                    //destination file have readonly or hidden attribute
                    //clear attributes
                    WinAPiFSwrapper.GetFileInfo(destination, ref dst_data);
                    return clear_dest_attributes(destination, dst_data);

            } //end switch

            //default:
            Win32Exception ex = new Win32Exception(win_error);
            if (process_error
                (string.Format
                (Options.GetLiteral(Options.LANG_CANNOT_COPY_0_ARROW_1),
                source_info.FullName,
                destination),
                ex))
            {
                return false;
            }
            else
            {
                stop();
                return false;
            }   
        }

        #endregion

        /// <summary>
        /// returns continue or abort all job
        /// </summary>
        /// <param name="info"></param>
        /// <param name="ex"></param>
        /// <returns></returns>
        private bool process_error(string info, Exception ex)
        {
            if ((options & CopyEngineOptions.SupressErrors) == CopyEngineOptions.SupressErrors)
            {
                return true;
            }
            else
            {
                return Messages.ShowExceptionContinue(ex, info);
            }
        }

        private void OnCopyItemDone(ItemEventArs e)
        {
            if (CopyItemDone != null)
            {
                CopyItemDone(this, e);
            }
        }

        #region IDisposable Members

        public void Dispose()
        {
            if (callback_data != IntPtr.Zero)
            {
                Marshal.FreeHGlobal(callback_data);
            }
            
        }

        #endregion
    }

    public enum UpdateProgressReason
    {
        JobBegin,
        JobDone,
        TotalSizeCalculated,
        StreamSwitch,
        ChunkFinish,
    }

    public delegate void MethodInvokerUpdateProgress(UpdateProgressArgs args);
    public class UpdateProgressArgs
    {
        public string DestinationFile { get; set; }
        public string SourceFile { get; set; }
        public UpdateProgressReason Reason { get; set; }
        public ulong KBytesPerSec { get; set; }
        public ulong StreamSize { get; set; }
        public ulong StreamTransferred { get; set; }
        public ulong TotalTransferred { get; set; }
        public int FilesCopied { get; set; }
        public bool EnableTotalProgress { get; set; }
        public ulong TotalSize { get; set; }
    }
    

}
