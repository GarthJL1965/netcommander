using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using netCommander.FileSystemEx;
using System.Windows.Forms;
using System.ComponentModel;
using System.Runtime.InteropServices;
using System.Threading;
using System.Security.AccessControl;

namespace netCommander
{
    public class MoveFileEngine : IDisposable
    {
        private CopyFileProgressDialog progress_dialog;
        private List<FileInfoEx> initial_source;
        private string innitial_destination;
        private MoveEngineOptions options;
        private CopyProgressRoutine move_progress_delegate_holder;
        private Thread internal_thread;
        private IntPtr callback_data = IntPtr.Zero;
        private MoveFileOptions options_api;
        private MethodInvokerUpdateProgress update_progress_delegate_holder;
        private string initial_mask = "*";

        public MoveFileEngine
            (List<FileInfoEx> source,
            string destination,
            string mask,
            MoveEngineOptions options,
            CopyFileProgressDialog dialog)
        {
            initial_mask = mask;
            initial_source = source;
            innitial_destination = destination;
            this.options = options;
            progress_dialog = dialog;
            internal_thread = new Thread(move_proc);

            options_api = MoveFileOptions.CopyAllowed | MoveFileOptions.WriteThrough;
            if ((options & MoveEngineOptions.ReplaceExistingFiles) == MoveEngineOptions.ReplaceExistingFiles)
            {
                options_api = options_api | MoveFileOptions.ReplaceExisting;
            }

            if (progress_dialog != null)
            {
                progress_dialog.FormClosing += new FormClosingEventHandler(progress_dialog_FormClosing);
            }

            update_progress_delegate_holder = new MethodInvokerUpdateProgress(update_progress_unsafe);
            move_progress_delegate_holder = new CopyProgressRoutine(move_progress_proc);
        }

        public void Do()
        {
            internal_thread.Start();
        }

        public event EventHandler Done;

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
            AbortJobSafe = true;

            //set cancel flag - will be check by CopyFileEx callback
            progress_dialog.CancelPressedSafe = true;

            //set internal flag - will will be check by CopyFileEx callback
            ProgressCloseBeginSafe = true;
        }

        //callback from CopyFileEx, call from back back thread
        private CopyFileExCallbackReturns move_progress_proc(UInt64 TotalFileSize,
            UInt64 TotalBytesTransferred,
            UInt64 StreamSize,
            UInt64 StreamBytesTransferred,
            int dwStreamNumber,
            CopyFileExState dwCallbackReason,
            IntPtr hSourceFile,
            IntPtr hDestinationFile,
            IntPtr lpData)
        {
            if (progress_dialog == null)
            {
                return CopyFileExCallbackReturns.QUIET;
            }

            if (ProgressCloseBeginSafe)
            {
                return CopyFileExCallbackReturns.CANCEL;
            }

            UpdateProgressArgs e = new UpdateProgressArgs();
            if (dwCallbackReason == CopyFileExState.STREAM_SWITCH)
            {
                string src = string.Empty;
                string dts = string.Empty;
                IOhelper.strings_from_buffer(ref src, ref dts, lpData);
                e.SourceFile = src;
                e.DestinationFile = dts;
                e.Reason = UpdateProgressReason.StreamSwitch;
            }
            else
            {
                e.Reason = UpdateProgressReason.ChunkFinish;
            }

            e.EnableTotalProgress = false;
            e.StreamSize = StreamSize;
            e.StreamTransferred = StreamBytesTransferred;

            update_progress_safe(e);

            //need try becouse sync error may occur if progress_dialog closed
            try
            {
                if (progress_dialog.CancelPressedSafe)
                {
                    AbortJobSafe = true;
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
                AbortJobSafe = true;
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

            //supress sync errors
            try
            {
                progress_dialog.Invoke(update_progress_delegate_holder, new object[] { e });
            }
            catch { }
        }

        private void update_progress_unsafe(UpdateProgressArgs e)
        {
            //must be invoked on UI thread
            switch (e.Reason)
            {
                case UpdateProgressReason.ChunkFinish:
                    progress_dialog.SetProgress(e.StreamTransferred, e.StreamSize);
                    break;

                case UpdateProgressReason.JobBegin:
                    progress_dialog.Text = Options.GetLiteral(Options.LANG_MOVE_RENAME);
                    progress_dialog.labelSpeed.Text = string.Empty;
                    progress_dialog.labelStatus.Text = string.Empty;
                    progress_dialog.labelStatusTotal.Text = string.Empty;
                    //progress_dialog.progressBarTotal.Enabled = false;
                    break;

                case UpdateProgressReason.JobDone:
                    progress_dialog.labelStatusTotal.Text = "Done";
                    break;

                case UpdateProgressReason.StreamSwitch:
                    progress_dialog.labelStatus.Text = string.Format
                        (Options.GetLiteral(Options.LANG_MOVE_RENAME_0_ARROW_1),
                        e.SourceFile,
                        e.DestinationFile);
                    progress_dialog.SetProgress(e.StreamTransferred, e.StreamSize);
                    break;
            }
        }

        private object abort_job_lock=new object();
        private bool abort_job_unsafe = false;
        public bool AbortJobSafe
        {
            get
            {
                bool ret = false;
                lock (abort_job_lock)
                {
                    ret = abort_job_unsafe;
                }
                return ret;
            }
            set
            {
                lock (abort_job_lock)
                {
                    abort_job_unsafe = value;
                }
            }
        }

        private void move_proc()
        {
            //notify about job begin
            UpdateProgressArgs e_begin = new UpdateProgressArgs();
            e_begin.EnableTotalProgress = false;
            e_begin.Reason = UpdateProgressReason.JobBegin;
            update_progress_safe(e_begin);

            //check source and destination
            //string cur_source = string.Empty;
            //WIN32_FIND_DATA src_data = new WIN32_FIND_DATA();
            //WIN32_FIND_DATA dst_data = new WIN32_FIND_DATA();
            FileInfoEx dest_info = new FileInfoEx();
            bool dst_exist = FileInfoEx.TryGet(innitial_destination, ref dest_info);
            FileInfoEx cur_source = null;

            //bool dst_exist = WinAPiFSwrapper.GetFileInfo(innitial_destination, ref dst_data);
            
            for (int i = 0; i < initial_source.Count; i++)
            {
                if (AbortJobSafe)
                {
                    break;
                }
                try
                {
                    cur_source = initial_source[i];
                    if (!Wildcard.Match(initial_mask, cur_source.FileName))
                    {
                        continue;
                    }
                    //WinAPiFSwrapper.GetFileInfo(cur_source, ref src_data);
                    if (cur_source.Directory)
                    {
                        //src is directory
                        if (!dst_exist)
                        {
                            //destination not exist - treat as new directory
                            move_one_item(cur_source, innitial_destination);
                            dst_exist = FileInfoEx.TryGet(innitial_destination, ref dest_info);
                        }
                        else
                        {
                            //destination exist
                            if (dest_info.Directory)
                            {
                                //and it is dir -> move cur_source into initial_destination
                                move_one_item(cur_source, Path.Combine(innitial_destination, cur_source.FileName));
                            }
                            else
                            {
                                //move dir to existing file -> not normally
                                AbortJobSafe = !process_errors
                                    (string.Format
                                    (Options.GetLiteral(Options.LANG_WRONG_DESTINATION_SOURCE_0_DIRECTORY_DESTINATION_1_FILE),
                                    cur_source.FileName,
                                    innitial_destination),
                                    null);
                                continue;
                            }
                        }
                    }
                    else
                    {
                        //source is file
                        if (!dst_exist)
                        {
                            //dest not exists
                            if (initial_source.Count == 1)
                            {
                                //treat dest as file
                                move_one_item(cur_source, innitial_destination);
                            }
                            else
                            {
                                //treat dest as dir., move source into new dir
                                Directory.CreateDirectory(innitial_destination);
                                dst_exist = FileInfoEx.TryGet(innitial_destination, ref dest_info);
                                move_one_item(cur_source, Path.Combine(innitial_destination, cur_source.FileName));
                            }
                        }
                        else
                        {
                            //dst exists
                            if (dest_info.Directory)
                            {
                                //move file into initial_destination
                                move_one_item(cur_source, Path.Combine(innitial_destination, cur_source.FileName));
                            }
                            else
                            {
                                //dst exists and it is existing file
                                if (initial_source.Count == 1)
                                {
                                    //move file with new name, overwriting existing
                                    move_one_item(cur_source, innitial_destination);
                                }
                                else
                                {
                                    //not mormal: more then one item into one existing file
                                    AbortJobSafe = !process_errors
                                        (string.Format
                                        (Options.GetLiteral(Options.LANG_WRONG_DESTINATION_CANNOT_MOVE_MANY_ENTRIES_INTO_ONE_FILE_0),
                                        innitial_destination),
                                        null);
                                    continue;
                                }
                            }
                        }//end of dest exists brunch
                    }//end of src is file
                }//end of try

                catch (Exception ex)
                {
                    AbortJobSafe = !process_errors
                        (string.Format
                        (Options.GetLiteral(Options.LANG_CANNOT_MOVE_0_ARROW_1),
                        initial_source[i].FullName,
                        innitial_destination),
                        ex);
                }
            }//end of for...

            //notify about end of job
            UpdateProgressArgs e_end = new UpdateProgressArgs();
            e_end.Reason = UpdateProgressReason.JobDone;
            update_progress_safe(e_end);
            if (Done != null)
            {
                Done(this, new EventArgs());
            }
        }//end of proc

        /// <summary>
        /// work only if source and destination on one volume
        /// </summary>
        /// <param name="source">can be file or dir</param>
        /// <param name="destination">file or dir</param>
        private void move_one_item(FileInfoEx source, string destination)
        {
            //prepare callback buffer
            if (callback_data != IntPtr.Zero)
            {
                Marshal.FreeHGlobal(callback_data);
            }
            callback_data = IOhelper.strings_to_buffer(source.FullName, destination);

            //prepare uni names
            string source_uni = IOhelper.GetUnicodePath(source.FullName);
            string destination_uni = IOhelper.GetUnicodePath(destination);

            //call update dialog before...
            UpdateProgressArgs e_begin_move = new UpdateProgressArgs();
            e_begin_move.DestinationFile = destination;
            e_begin_move.EnableTotalProgress = false;
            e_begin_move.Reason = UpdateProgressReason.StreamSwitch;
            e_begin_move.SourceFile = source.FullName;
            update_progress_safe(e_begin_move);

            //and call MoveFileWithProgress
            int res = WinApiFS.MoveFileWithProgress
                (source_uni,
                destination_uni,
                move_progress_delegate_holder,
                callback_data,
                options_api);

            if (res == 0)
            {
                int win_err = Marshal.GetLastWin32Error();
                Win32Exception win_ex=new Win32Exception(win_err);
                AbortJobSafe = !process_errors
                    (string.Format
                    (Options.GetLiteral(Options.LANG_CANNOT_MOVE_0_ARROW_1),
                    source,
                    destination),
                    win_ex);
                return;
            }
            //success move
            //notify item done - not needed, source item deleted
        }

        private bool process_errors(string info, Exception ex)
        {
            if ((options & MoveEngineOptions.SupressErrors) == MoveEngineOptions.SupressErrors)
            {
                return true;
            }

            return Messages.ShowExceptionContinue(ex, info);
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
}
