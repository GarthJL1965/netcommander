using System;
using System.Collections.Generic;
using System.Text;

namespace netCommander
{
    public abstract class CopyFileEngineBase : IDisposable
    {
        protected string[] initial_source;
        protected string initial_destination;
        protected CopyEngineOptions options;
        protected CopyFileProgressDialog progress_dialog;

        private MethodInvokerUpdateProgress update_progress_delegate_holder;
        private int start_tick = 0;

        public event ItemEventHandler CopyItemDone;
        public event EventHandler Done;

        public CopyFileEngineBase
            (string[] source,
            string destination,
            CopyEngineOptions options,
            CopyFileProgressDialog dialog)
        {
            initial_source = source;
            initial_destination = destination;
            this.options = options;
            progress_dialog = dialog;

            progress_dialog.FormClosing += new System.Windows.Forms.FormClosingEventHandler(progress_dialog_FormClosing);
            update_progress_delegate_holder = new MethodInvokerUpdateProgress(update_progress);
        }

        public abstract void DoCopy();

        protected void OnJobDone()
        {
            if (Done != null)
            {
                Done(this, new EventArgs());
            }
        }

        protected void OnItemDone(ItemEventArs e)
        {
            if (CopyItemDone != null)
            {
                CopyItemDone(this, e);
            }
        }

        private void progress_dialog_FormClosing(object sender, System.Windows.Forms.FormClosingEventArgs e)
        {
            //progress dialog before user or system close
            //call from UI thread!

            //thread safe set abort_job flag - will be check by file enum functions 
            AbortJob = true;

            //set cancel flag - will be check by CopyFileEx callback
            progress_dialog.CancelPressedSafe = true;

            //set internal flag - will will be check by CopyFileEx callback
            ProgressCloseBeginSafe = true;
        }

        private object progress_closing_lock = new object();
        private bool progress_closing_unsafe = false;
        protected bool ProgressCloseBeginSafe
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

        /// <summary>
        /// returns continue or abort all job
        /// </summary>
        /// <param name="info"></param>
        /// <param name="ex"></param>
        /// <returns></returns>
        protected bool process_error(string info, Exception ex)
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

        private bool abort_unsafe = false;
        private object abort_lock = new object();
        /// <summary>
        /// call this to safe abort all job
        /// </summary>
        public bool AbortJob
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

        protected void update_progress_safe(UpdateProgressArgs e)
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
                if (progress_dialog.InvokeRequired)
                {
                    progress_dialog.Invoke(update_progress_delegate_holder, new object[] { e });
                }
                else
                {
                    update_progress(e);
                }
            }
            catch { }
        }

        private void update_progress(UpdateProgressArgs e)
        {
            //will be invoke in UI thread - canot use anything else e and progress_dialog members
            switch (e.Reason)
            {
                case UpdateProgressReason.ChunkFinish:
                    progress_dialog.SetProgressStream(e.StreamTransferred, e.StreamSize);
                    progress_dialog.labelSpeed.Text = string.Format("{0:N0} Kbyte/sec", e.KBytesPerSec);
                    //progress_dialog.labelStatus.Text =
                    //    string.Format
                    //    ("'{0}' -> '{1}' ({2})",
                    //    e.SourceFile,
                    //    e.DestinationFile,
                    //    IOhelper.SizeToString(e.StreamSize));
                    if (e.EnableTotalProgress)
                    {
                        progress_dialog.SetProgressTotal(e.TotalTransferred, e.TotalSize);
                        progress_dialog.labelStatusTotal.Text =
                            string.Format
                            ("{0:N0} bytes from {1:N0} ({2:P0}) transferred",
                            e.TotalTransferred,
                            e.TotalSize,
                            (e.TotalSize == 0) ? 0 : (double)e.TotalTransferred / (double)e.TotalSize);
                    }
                    else
                    {
                        progress_dialog.labelStatusTotal.Text =
                            string.Format("{0:N0} bytes transferred", e.TotalTransferred);
                    }
                    break;

                case UpdateProgressReason.JobBegin:
                    progress_dialog.Text = "Copy";
                    start_tick = Environment.TickCount;
                    break;

                case UpdateProgressReason.JobDone:
                    progress_dialog.SetProgressStream(100, 100);
                    progress_dialog.labelSpeed.Text = string.Format("{0:N0} Kbyte/sec", e.KBytesPerSec);
                    if (e.EnableTotalProgress)
                    {
                        progress_dialog.SetProgressTotal(100, 100);
                    }
                    progress_dialog.labelStatusTotal.Text =
                        string.Format
                        ("{0:N0} bytes from {1:N0} file(s) transferred", e.TotalTransferred, e.FilesCopied);
                    break;

                case UpdateProgressReason.StreamSwitch:
                    progress_dialog.labelStatus.Text =
                        string.Format
                        ("'{0}' -> '{1}' ({2})",
                        e.SourceFile,
                        e.DestinationFile,
                        IOhelper.SizeToString(e.StreamSize));
                    //progress_dialog.labelSpeed.Text = string.Format("{0:N0} Kbyte/sec", e.KBytesPerSec);
                    if (e.EnableTotalProgress)
                    {
                        progress_dialog.SetProgressTotal(e.TotalTransferred, e.TotalSize);
                        //progress_dialog.labelStatusTotal.Text =
                        //    string.Format
                        //    ("{0:N0} bytes from {1:N0} ({2:P0}) transferred",
                        //    e.TotalTransferred,
                        //    e.TotalSize,
                        //    (e.TotalSize == 0) ? 0 : (double)e.TotalTransferred / (double)e.TotalSize);
                    }
                    else
                    {
                        //progress_dialog.labelStatusTotal.Text =
                        //string.Format("{0:N0} bytes transferred", e.TotalTransferred);
                    }
                    break;

                case UpdateProgressReason.TotalSizeCalculated:
                    if (e.EnableTotalProgress)
                    {
                        progress_dialog.labelStatusTotal.Text =
                            string.Format
                            ("{0:N0} bytes from {1:N0} ({2:P0}) transferred",
                            e.TotalTransferred,
                            e.TotalSize,
                            (e.TotalSize == 0) ? 0 : (double)e.TotalTransferred / (double)e.TotalSize);
                    }
                    break;
            }
        }

        /// <summary>
        /// Kb per sec
        /// </summary>
        /// <param name="total_transferred"></param>
        /// <returns></returns>
        protected ulong calc_speed(ulong total_transferred)
        {
            int job_ticks = calc_tick_elapsed();
            if (job_ticks <= 0)
            {
                return int.MaxValue;
            }

            ulong ret_ulong_msec = (total_transferred) / (ulong)job_ticks;
            //now ret_ulong_msec is bytes per millisecond

            //calc bytes per sec
            ulong ret = ret_ulong_msec * 1000U;

            //and Kbyte per sec
            ret = ret / 1024U;

            return ret;
        }

        /// <summary>
        /// returns ticks elapsed from start_tick
        /// must be call from back tread
        /// </summary>
        /// <returns></returns>
        protected int calc_tick_elapsed()
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

        #region IDisposable Members

        public virtual void Dispose() { }

        #endregion
    }

    public delegate void MethodInvokerUpdateProgress(UpdateProgressArgs e);
    public class UpdateProgressArgs
    {
        public ulong StreamSize { get; set; }
        public ulong StreamTransferred { get; set; }
        public string SourceFile { get; set; }
        public string DestinationFile { get; set; }
        public ulong TotalTransferred { get; set; }
        public ulong TotalSize { get; set; }
        public ulong KBytesPerSec { get; set; }
        public UpdateProgressReason Reason { get; set; }
        public int FilesCopied { get; set; }
        public bool EnableTotalProgress { get; set; }

        public UpdateProgressArgs(){}

        public UpdateProgressArgs
            (string source,
            string destination,
            UpdateProgressReason reason,
            ulong stream_size,
            ulong stream_transferred,
            ulong total_transferred,
            ulong total_size,
            ulong speed,
            int items_copied,
            bool enable_total_progress)
        {
            SourceFile = source;
            DestinationFile = destination;
            Reason = reason;
            StreamSize = stream_size;
            StreamTransferred = stream_transferred;
            TotalTransferred = total_transferred;
            TotalSize = total_size;
            KBytesPerSec=speed;
            FilesCopied=items_copied;
            EnableTotalProgress=enable_total_progress;
        }
    }

    public enum UpdateProgressReason
    {
        ChunkFinish,
        StreamSwitch,
        JobBegin,
        JobDone,
        TotalSizeCalculated
    }
}
