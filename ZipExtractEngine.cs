using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using ICSharpCode.SharpZipLib;
using ICSharpCode.SharpZipLib.Zip;
using System.Threading;

namespace netCommander
{
    public class ZipExtractEngine
    {
        private readonly int buffer_size = Options.ArchiveStreamBufferSize;

        private ZipFile zip_file;
        private List<string> initial_sources;
        private List<ZipEntry> extract_list;
        private string initial_zip_dir;
        private string initial_destination;
        private bool abort_unsafe = false;
        private ArchiveExtractOptions opts;
        private byte[] buffer = new byte[0];
        private Thread work_thread = null;
        private int start_tick = 0;
        private string mask = "*";

        private MethodInvokerUpdateProgress update_progress_delegate_holder;

        private long total_bytes_transferred = 0;
        //private long current_file_bytes_transferred = 0;
        private long total_bytes = 0;

        private CopyFileProgressDialog progress_dialog;

        public event ItemEventHandler ExtractItemDone;
        public event EventHandler Done;

        public ZipExtractEngine
            (List<string> source,
            string destination_dir,
            ArchiveExtractOptions options,
            CopyFileProgressDialog dialog,
            string mask,
            ZipFile zipFile,
            string initial_zip_dir)
        {
            initial_sources = source;
            initial_destination = destination_dir;
            opts = options;
            progress_dialog = dialog;
            this.mask = mask;
            zip_file = zipFile;
            this.initial_zip_dir = initial_zip_dir;

            if (progress_dialog != null)
            {
                progress_dialog.FormClosing += new System.Windows.Forms.FormClosingEventHandler
                    (progress_dialog_FormClosing);
            }

            update_progress_delegate_holder = new MethodInvokerUpdateProgress(update_progress);
        }

        #region progress handling
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
            AbortSafe = true;

            //set cancel flag - will be check by CopyFileEx callback
            progress_dialog.CancelPressedSafe = true;

            //set internal flag - will will be check by CopyFileEx callback
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

        private long calc_speed()
        {
            int job_ticks = calc_tick_elapsed();
            if (job_ticks <= 0)
            {
                return int.MaxValue;
            }

            long ret_long_msec = (total_bytes_transferred) / (long)job_ticks;
            //now ret_ulong_msec is bytes per millisecond

            //calc bytes per sec
            long ret = ret_long_msec * 1000L;

            //and Kbyte per sec
            ret = ret / 1024L;

            return ret;
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

            if (progress_dialog.CancelPressedSafe)
            {
                AbortSafe = true;
                return;
            }

            //supress sync errors
            try
            {
                progress_dialog.Invoke(update_progress_delegate_holder, new object[] { e });
            }
            catch { }
        }

        private void update_progress(UpdateProgressArgs e)
        {
            //will be invoke in UI thread - canot use anything else e and progress_dialog members

            switch (e.Reason)
            {
                case UpdateProgressReason.ChunkFinish:
                    progress_dialog.SetProgress(e.TotalTransferred, e.TotalSize);
                    progress_dialog.labelSpeed.Text = string.Format
                        (Options.GetLiteral(Options.LANG_0_KBYTE_SEC), e.KBytesPerSec);
                    progress_dialog.labelStatusTotal.Text = string.Format
                        (Options.GetLiteral(Options.LANG_0_BYTES_TRANSFERRED), e.TotalTransferred);
                    break;

                case UpdateProgressReason.JobBegin:
                    progress_dialog.Text = Options.GetLiteral(Options.LANG_EXTRACT);
                    break;

                case UpdateProgressReason.JobDone:
                    progress_dialog.SetProgress(100, 100);
                    progress_dialog.labelSpeed.Text = string.Format
                        (Options.GetLiteral(Options.LANG_0_KBYTE_SEC), e.KBytesPerSec);
                    break;

                case UpdateProgressReason.StreamSwitch:
                    progress_dialog.labelStatus.Text =
                        string.Format
                        (Options.GetLiteral(Options.LANG_0_ARROW_1_2),
                        e.SourceFile,
                        e.DestinationFile,
                        IOhelper.SizeToString(e.StreamSize));
                    progress_dialog.labelSpeed.Text = string.Format
                        (Options.GetLiteral(Options.LANG_0_KBYTE_SEC), e.KBytesPerSec);
                    break;
            }
        }


        private object abort_lock = new object();
        private bool AbortSafe
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
        #endregion

        #region extract thread
        public void Run()
        {
            work_thread = new Thread(internal_run);
            work_thread.Start();
        }

        private void prepare_extract_list()
        {
            extract_list = new List<ZipEntry>();
            foreach (string initial_src in initial_sources)
            {
                string current_source = FtpPath.Combine(initial_zip_dir, initial_src);
                if (current_source.StartsWith("/"))
                {
                    current_source = current_source.Substring(1);
                }

                ZipEntry current_entry = zip_file.GetEntry(current_source);
                if ((current_entry != null) && (current_entry.IsFile))
                {
                    if (!extract_list.Contains(current_entry))
                    {
                        extract_list.Add(current_entry);
                    }
                }
                else
                {
                    fill_extract_list(current_source);
                }
            }
            foreach (ZipEntry entry in extract_list)
            {
                total_bytes += entry.Size;
            }
        }

        private void fill_extract_list(string current_zip_dir)
        {
            char path_separator = '/';
            char[] path_separator_param = new char[] { path_separator };
            foreach (ZipEntry entry in zip_file)
            {
                if ((current_zip_dir.Length != 0) && (!current_zip_dir.EndsWith("/")))
                {
                    current_zip_dir = current_zip_dir + path_separator;
                }

                if (entry.Name.Length < current_zip_dir.Length)
                {
                    //skip short entries
                    continue;
                }

                if (!entry.Name.StartsWith(current_zip_dir))
                {
                    //skip entries to other dirs
                    continue;
                }

                if (entry.Name == current_zip_dir)
                {
                    continue;
                }

                if (entry.IsDirectory)
                {
                    fill_extract_list(entry.Name);
                }

                if (extract_list.Contains(entry))
                {
                    continue;
                }

                extract_list.Add(entry);
            }
        }

        private void internal_run()
        {
            start_tick = Environment.TickCount;

            try
            {
                //notify about job begin
                UpdateProgressArgs e = new UpdateProgressArgs();
                e.DestinationFile = string.Empty;
                e.EnableTotalProgress = false;
                e.FilesCopied = 0;
                e.KBytesPerSec = 0;
                e.Reason = UpdateProgressReason.JobBegin;
                e.SourceFile = string.Empty;
                e.StreamSize = 0;
                e.StreamTransferred = 0;
                e.TotalSize = 0;
                e.TotalTransferred = 0;
                update_progress_safe(e);

                prepare_extract_list();

                foreach (ZipEntry entry in extract_list)
                {
                    if (AbortSafe)
                    {
                        break;
                    }

                    string relative_path = entry.Name;
                    if (initial_zip_dir.Length != 0)
                    {
                        relative_path = entry.Name.Substring(initial_zip_dir.Length);
                        relative_path = relative_path.TrimStart(new char[] { '/' });
                    }

                    string destination_path = Path.Combine(initial_destination, relative_path.Replace('/', '\\'));
                    extract_one_entry(entry, destination_path);
                    if (ExtractItemDone != null)
                    {
                        ExtractItemDone(this, new ItemEventArs(FtpPath.GetFile(entry.Name)));
                    }
                }
            }
            finally
            {
                //notify job done
                if (Done != null)
                {
                    Done(this, new EventArgs());
                }
            }
        }

        

        private void extract_one_entry(ZipEntry entry, string destination_file)
        {
            try
            {
                if (entry.IsDirectory)
                {
                    extract_directory(entry, destination_file);
                    return;
                }

                if (!check_destination(entry, destination_file))
                {
                    return;
                }

                UpdateProgressArgs e = new UpdateProgressArgs();
                e.DestinationFile = destination_file;
                e.EnableTotalProgress = false;
                e.FilesCopied = 0;
                e.KBytesPerSec = (ulong)calc_speed();
                e.Reason = UpdateProgressReason.StreamSwitch;
                e.SourceFile = entry.Name;
                e.StreamSize = (ulong)entry.Size;
                e.StreamTransferred = 0;
                e.TotalSize = 0;
                e.TotalTransferred = (ulong)total_bytes_transferred;
                update_progress_safe(e);

                //open streams
                Stream in_stream = null;
                FileStream out_stream = null;
                try
                {
                    in_stream = zip_file.GetInputStream(entry);
                    out_stream = new FileStream
                    (destination_file,
                    (opts & ArchiveExtractOptions.NeverOverwite) == ArchiveExtractOptions.NeverOverwite ? FileMode.CreateNew : FileMode.Create,
                    FileAccess.Write,
                    FileShare.Read);
                    copy_streams(in_stream, out_stream, entry.Size);
                }
                finally
                {
                    if (in_stream != null)
                    {
                        in_stream.Close();
                    }
                    if (out_stream != null)
                    {
                        out_stream.Flush();
                        out_stream.Close();
                    }
                }
                if ((File.Exists(destination_file)) && ((opts & ArchiveExtractOptions.ExtractAttributes) == ArchiveExtractOptions.ExtractAttributes))
                {
                    FileInfo fi = new FileInfo(destination_file);
                    try
                    {
                        fi.LastWriteTime = entry.DateTime;
                    }
                    catch (Exception ex)
                    {
                        AbortSafe = !process_error
                            (string.Format
                            ("cannot apply timestamp to '{0}'",
                            destination_file),
                            ex);
                    }
                    if (entry.IsDOSEntry)
                    {
                        try
                        {
                            fi.Attributes = (FileAttributes)entry.ExternalFileAttributes;
                        }
                        catch (Exception ex)
                        {
                            AbortSafe = !process_error
                                (string.Format
                                ("cannot apply attributes to '{0}'",
                                destination_file),
                                ex);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                AbortSafe = !process_error(string.Empty, ex);
            }
        }

        private void extract_directory(ZipEntry dir_entry, string destination_path)
        {
            if (!Directory.Exists(destination_path))
            {
                Directory.CreateDirectory(destination_path);
            }

            if ((opts & ArchiveExtractOptions.ExtractAttributes) == ArchiveExtractOptions.ExtractAttributes)
            {
                DirectoryInfo di = new DirectoryInfo(destination_path);
                di.LastWriteTime = dir_entry.DateTime;
                if (dir_entry.IsDOSEntry)
                {
                    di.Attributes = (FileAttributes)dir_entry.ExternalFileAttributes;
                }
            }
        }

        private bool check_destination(ZipEntry entry, string destination_file)
        {
            //check date if needed
            //create dir if needed

            string dest_dir = Path.GetDirectoryName(destination_file);
            if (!Directory.Exists(dest_dir))
            {
                Directory.CreateDirectory(dest_dir);
            }

            if ((File.Exists(destination_file)) && ((opts & ArchiveExtractOptions.OverwriteIfSourceNewer) == ArchiveExtractOptions.OverwriteIfSourceNewer))
            {
                if (entry.DateTime > File.GetLastWriteTime(destination_file))
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else
            {
                return true;
            }
        }

        private void copy_streams(Stream source_stream, Stream destination_stream,long destination_size)
        {
            if (buffer.Length != buffer_size)
            {
                buffer = new byte[buffer_size];
            }

            UpdateProgressArgs e = new UpdateProgressArgs();
            e.TotalSize = (ulong)total_bytes;
            e.Reason = UpdateProgressReason.ChunkFinish;
            e.SourceFile = string.Empty;
            e.StreamSize = (ulong)destination_size;
            e.DestinationFile = string.Empty;
            e.EnableTotalProgress = true;
            e.FilesCopied = 0;

            int bytes_readed = 0;
            long bytes_writed=0;
            while ((bytes_readed = source_stream.Read(buffer, 0, buffer_size)) != 0)
            {
                if (AbortSafe)
                {
                    break;
                }
                destination_stream.Write(buffer, 0, bytes_readed);

                bytes_writed = bytes_writed + bytes_readed;
                total_bytes_transferred = total_bytes_transferred + bytes_readed;

                //time to notify chunk done
                
                e.KBytesPerSec = (ulong)calc_speed();
                e.StreamTransferred = (ulong)bytes_writed;
                e.TotalTransferred = (ulong)total_bytes_transferred;
                update_progress_safe(e);
            }
        }
        #endregion

        private bool process_error(string info, Exception ex)
        {
            if ((opts & ArchiveExtractOptions.SupressExceptions) == ArchiveExtractOptions.SupressExceptions)
            {
                return true;
            }

            return Messages.ShowExceptionContinue(ex, info);
        }
    }
}
