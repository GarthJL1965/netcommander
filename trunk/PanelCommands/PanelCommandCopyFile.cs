using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using netCommander.FileSystemEx;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using System.Threading;
using ICSharpCode.SharpZipLib.Zip;

namespace netCommander
{
    class PanelCommandCopyFile : PanelCommandBase
    {
        public PanelCommandCopyFile()
            : base(Options.GetLiteral(Options.LANG_COPY), Shortcut.F5)
        {

        }

        private CopyFileProgressDialog dialog_progress = null;
        
        private string current_destination = string.Empty;
        private string current_source = string.Empty;

        private void add_to_zip(QueryPanelInfoEventArgs e_current, QueryPanelInfoEventArgs e_other)
        {
            mainForm main_window = (mainForm)Program.MainWindow;
            try
            {
                //this is sync operation
                ZipDirectory zd = (ZipDirectory)e_other.ItemCollection;
                DirectoryList dl = (DirectoryList)e_current.ItemCollection;

                //show add zip dialog
                ArchiveAddDialog dialog = new ArchiveAddDialog();
                dialog.Text = Options.GetLiteral(Options.LANG_ARCHIVE_ADD);
                dialog.textBoxSourceMask.Text = "*";
                dialog.ArchiveAddOptions = Options.ArchiveAddOptions;
                if (dialog.ShowDialog() != DialogResult.OK)
                {
                    return;
                }
                ArchiveAddOptions options = dialog.ArchiveAddOptions;
                Options.ArchiveAddOptions = options;

                //notify main window
                main_window.NotifyLongOperation(Options.GetLiteral(Options.LANG_CREATING_FILE_LIST_TO_ARCHIVE), true);

                //create file list to add (only files with path)
                string[] one_file_list = new string[0];
                List<string> root_paths = new List<string>();
                List<string> file_list = new List<string>();
                if (e_current.SelectedIndices.Length == 0)
                {
                    if (dl[e_current.FocusedIndex].FileName == "..")
                    {
                        Messages.ShowMessage(Options.GetLiteral(Options.LANG_WRONG_SOURCE));
                        return;
                    }
                    root_paths.Add(dl[e_current.FocusedIndex].FullName);
                }
                else
                {
                    for (int i = 0; i < e_current.SelectedIndices.Length; i++)
                    {
                        root_paths.Add(dl[e_current.SelectedIndices[i]].FullName);
                    }
                }
                foreach (string one_path in root_paths)
                {
                    if ((Directory.Exists(one_path)) && ((options & ArchiveAddOptions.Recursive) == ArchiveAddOptions.Recursive))
                    {
                        one_file_list = Directory.GetFiles(one_path, dialog.textBoxSourceMask.Text, SearchOption.AllDirectories);
                    }
                    else if ((File.Exists(one_path)) && (Wildcard.Match(dialog.textBoxSourceMask.Text, one_path)))
                    {
                        one_file_list = new string[] { one_path };
                    }
                    else
                    {
                        one_file_list = new string[0];
                    }
                    file_list.AddRange(one_file_list);
                }

                //call ZipFile.BeginUpdate
                ZipFile zf = zd.ZipFile;
                

                //for each source list item:
                //create zip file name from real file path, trimmed from dl current directory
                //call ZipFile.Add
                //notify main window
                string current_dir = dl.DirectoryPath;
                string zip_path = string.Empty;
                int zip_index = 0;
                foreach (string one_file in file_list)
                {
                    try
                    {
                        zip_path = one_file.Substring(current_dir.Length);
                        if (zd.CurrentZipDirectory != string.Empty)
                        {
                            zip_path = zd.CurrentZipDirectory + "/" + zip_path;
                        }
                        zip_path = ZipEntry.CleanName(zip_path);

                        //check for exist
                        zip_index = zf.FindEntry(zip_path, true);
                        if (zip_index != -1)
                        {
                            //entry with same name already exists
                            if ((options & ArchiveAddOptions.NeverRewrite) == ArchiveAddOptions.NeverRewrite)
                            {
                                //skip
                                continue;
                            }
                            if ((options & ArchiveAddOptions.RewriteIfSourceNewer) == ArchiveAddOptions.RewriteIfSourceNewer)
                            {
                                //check mod date
                                if (File.GetLastWriteTime(one_file) < zf[zip_index].DateTime)
                                {
                                    continue;
                                }
                                else
                                {
                                    //remove entry
                                    zf.BeginUpdate();
                                    zf.Delete(zf[zip_index]);
                                    zf.CommitUpdate();
                                }
                            }
                            if ((options & ArchiveAddOptions.RewriteAlways) == ArchiveAddOptions.RewriteAlways)
                            {
                                zf.BeginUpdate();
                                zf.Delete(zf[zip_index]);
                                zf.CommitUpdate();
                            }
                        }

                        //open source file
                        main_window.NotifyLongOperation
                            (string.Format
                            (Options.GetLiteral(Options.LANG_ARCHIVING_0_1),
                            one_file,
                            zip_path),
                            true);
                        InternalStreamSource static_source = null;
                        try
                        {
                            //TODO избавиться от флага SaveAttributes
                            //будем всегда сохранять - DONE
                            zf.BeginUpdate();
                            zf.Add(one_file, zip_path);
                            main_window.NotifyLongOperation(Options.GetLiteral(Options.LANG_COMMIT_ARCHIVE_UPDATES), true);
                            zf.CommitUpdate();
                        }
                        catch (Exception ex)
                        {
                            if ((options & ArchiveAddOptions.SupressErrors) != ArchiveAddOptions.SupressErrors)
                            {
                                string err_message = string.Format
                                    (Options.GetLiteral(Options.LANG_FAILED_ARCHIVE_0),
                                    zip_path);
                                if (Messages.ShowExceptionContinue(ex, err_message))
                                {
                                    continue;
                                }
                                else
                                {
                                    break;
                                }
                            }
                        }
                        finally
                        {
                            if (static_source != null)
                            {
                                static_source.Close();
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        if ((options & ArchiveAddOptions.SupressErrors) != ArchiveAddOptions.SupressErrors)
                        {
                            string err_message = string.Format
                                (Options.GetLiteral(Options.LANG_FAILED_ARCHIVE_0),
                                zip_path);
                            if (Messages.ShowExceptionContinue(ex, err_message))
                            {
                                continue;
                            }
                            else
                            {
                                break;
                            }
                        }
                    }
                }//end of foreach

                //Refill zd
                zd.Refill();

                //notify main window when done
            }
            catch (Exception ex)
            {
                Messages.ShowException(ex);
            }
            finally
            {
                main_window.NotifyLongOperation("Done", false);
            }
        }

        private class InternalStreamSource : IStaticDataSource
        {
            private FileStream internal_stream = null;

            public InternalStreamSource(string file_name)
            {
                internal_stream = new FileStream
                (file_name,
                FileMode.Open,
                FileAccess.Read,
                FileShare.Read);
            }

            public void Close()
            {
                if (internal_stream != null)
                {
                    internal_stream.Close();
                }
            }

            #region IStaticDataSource Members

            public Stream GetSource()
            {
                return internal_stream;
            }

            #endregion
        }

        private void upload_to_ftp(QueryPanelInfoEventArgs e_current,QueryPanelInfoEventArgs e_other)
        {
            DirectoryList source_directory = (DirectoryList)e_current.ItemCollection;
            FtpDirectoryList destination_ftp = (FtpDirectoryList)e_other.ItemCollection;

            List<string> source_list = new List<string>();
            if (e_current.SelectedIndices.Length == 0)
            {
                if (source_directory.GetItemDisplayNameLong(e_current.FocusedIndex) == "..")
                {
                    return;
                }

                source_list.Add
                    (Path.Combine
                    (source_directory.DirectoryPath,
                    source_directory.GetItemDisplayNameLong(e_current.FocusedIndex)));
            }
            else
            {
                for (int i = 0; i < e_current.SelectedIndices.Length; i++)
                {
                    source_list.Add
                        (Path.Combine
                        (source_directory.DirectoryPath,
                        source_directory.GetItemDisplayNameLong(e_current.SelectedIndices[i])));
                }
            }

            string dest_path = destination_ftp.DirectoryPath;
            FtpConnection ftp_conn = destination_ftp.Connection;

            //show upload dialog
            FtpTransferOptions ftp_trans_opts = Options.FtpUploadOptions;
            FtpTransferDialog dialog = new FtpTransferDialog();
            dialog.FtpTransferOptions = ftp_trans_opts;
            dialog.textBoxDestination.Text = dest_path;
            dialog.Text = Options.GetLiteral(Options.LANG_UPLOAD);
            if (source_list.Count == 1)
            {
                dialog.labelSourceFile.Text =
                    source_list[0];
            }
            else
            {
                dialog.labelSourceFile.Text =
                    string.Format
                    ("{0} " + Options.GetLiteral(Options.LANG_ENTRIES),
                    source_list.Count);
            }

            if (dialog.ShowDialog() != DialogResult.OK)
            {
                return;
            }

            //retrieve user selection
            ftp_trans_opts = dialog.FtpTransferOptions;
            dest_path = dialog.textBoxDestination.Text;

            //save user selection
            Options.FtpUploadOptions = ftp_trans_opts;


            //prepare progress window
            dialog_progress = new CopyFileProgressDialog();
            dialog_progress.labelError.Visible = false;
            dialog_progress.labelSpeed.Text = string.Empty;
            dialog_progress.labelStatus.Text = string.Empty;
            dialog_progress.labelStatusTotal.Text = string.Empty;
            dialog_progress.checkBoxCloseOnFinish.Checked = Options.CopyCloseProgress;

            dialog_progress.TopLevel = true;
            int x_center = Program.MainWindow.Left + Program.MainWindow.Width / 2;
            int y_center = Program.MainWindow.Top + Program.MainWindow.Height / 2;
            int x_dialog = x_center - dialog_progress.Width / 2;
            int y_dialog = y_center - dialog_progress.Height / 2;
            if (x_dialog < 0)
            {
                x_dialog = 0;
            }
            if (x_dialog < 0)
            {
                y_dialog = 0;
            }
            dialog_progress.Show();
            dialog_progress.Location = new System.Drawing.Point(x_dialog, y_dialog);

            //prepare upload engine
            FtpUploadEngine upload_engine = new FtpUploadEngine
            (source_list.ToArray(),
            dest_path,
            ftp_conn,
            ftp_trans_opts,
            dialog_progress);

            upload_engine.Done += new EventHandler(upload_engine_Done);
            upload_engine.UploadItemDone += new ItemEventHandler(upload_engine_UploadItemDone);

            upload_engine.Run();

            //ftp_conn.ClearCache(dest_path);
        }

        void upload_engine_UploadItemDone(object sender, ItemEventArs e)
        {
            OnItemProcessDone(e);
        }

        void upload_engine_Done(object sender, EventArgs e)
        {
            if (dialog_progress.InvokeRequired)
            {
                dialog_progress.Invoke(new MethodInvoker(on_copy_done));
            }
            else
            {
                on_copy_done();
            }


        }

        protected override void internal_command_proc()
        {
            QueryPanelInfoEventArgs e_current = new QueryPanelInfoEventArgs();
            QueryPanelInfoEventArgs e_other = new QueryPanelInfoEventArgs();
            OnQueryCurrentPanel(e_current);
            OnQueryOtherPanel(e_other);

            if (e_current.FocusedIndex == -1)
            {
                return;
            }

            if (e_other.ItemCollection is FtpDirectoryList)
            {
                //do ftp download
                upload_to_ftp(e_current, e_other);
                return;
            }
            else if (e_other.ItemCollection is ZipDirectory)
            {
                add_to_zip(e_current, e_other);
                return;
            }
            else if (!(e_other.ItemCollection is DirectoryList))
            {
                Messages.ShowMessage(Options.GetLiteral(Options.LANG_WRONG_DESTINATION));
                return;
            }

            //see source
            List<FileInfoEx> source_list = new List<FileInfoEx>();
            DirectoryList dl_source = (DirectoryList)e_current.ItemCollection;
            DirectoryList dl_target = (DirectoryList)e_other.ItemCollection;
            int[] sel_indices = e_current.SelectedIndices;
            if (sel_indices.Length == 0)
            {
                if (dl_source.GetItemDisplayNameLong(e_current.FocusedIndex) == "..")
                {
                    return;
                }
                //get focused entry
                source_list.Add(dl_source[e_current.FocusedIndex]);
            }
            else
            {
                //get source from selection
                for (int i = 0; i < sel_indices.Length; i++)
                {
                    source_list.Add(dl_source[sel_indices[i]]);
                }
            }

            string dest_path = dl_target.DirectoryPath;

            //prepare copy dialog
            CopyFileDialog dialog = new CopyFileDialog();
            dialog.CopyEngineOptions = Options.CopyEngineOptions;
            dialog.Text = Options.GetLiteral(Options.LANG_COPY);
           
                dialog.textBoxSourceMask.Text = "*";
            
            dialog.textBoxDestination.Text = string.Empty;

            if (dialog.ShowDialog() != DialogResult.OK)
            {
                return;
            }

            //save user selection
            CopyEngineOptions engine_opts = dialog.CopyEngineOptions;
            Options.CopyEngineOptions = engine_opts;

            //prepare progress dialog
            dialog_progress = new CopyFileProgressDialog();
            dialog_progress.labelError.Visible = false;
            dialog_progress.labelSpeed.Text = string.Empty;
            dialog_progress.labelStatus.Text = string.Empty;
            dialog_progress.labelStatusTotal.Text = string.Empty;
            dialog_progress.checkBoxCloseOnFinish.Checked = Options.CopyCloseProgress;

            dialog_progress.TopLevel = true;
            int x_center = Program.MainWindow.Left + Program.MainWindow.Width / 2;
            int y_center = Program.MainWindow.Top + Program.MainWindow.Height / 2;
            int x_dialog = x_center - dialog_progress.Width / 2;
            int y_dialog = y_center - dialog_progress.Height / 2;
            if (x_dialog < 0)
            {
                x_dialog = 0;
            }
            if (x_dialog < 0)
            {
                y_dialog = 0;
            }
            dialog_progress.Show();
            dialog_progress.Location = new System.Drawing.Point(x_dialog, y_dialog);

            //prepare copy engine
            bool dest_remote = true;

            try
            {
                FileInfoEx dest_info = new FileInfoEx();
                FileInfoEx.TryGet(dest_path, ref dest_info);
                NT_FS_DEVICE_CHARACTERISTICS chars = dest_info.GetDeviceInfo().Characteristics;
                if (((chars & NT_FS_DEVICE_CHARACTERISTICS.Remote) == NT_FS_DEVICE_CHARACTERISTICS.Remote) ||
                    ((chars & NT_FS_DEVICE_CHARACTERISTICS.Removable) == NT_FS_DEVICE_CHARACTERISTICS.Removable) ||
                    ((chars & NT_FS_DEVICE_CHARACTERISTICS.WebDAV) == NT_FS_DEVICE_CHARACTERISTICS.WebDAV))
                {
                    dest_remote = true;
                }
                else
                {
                    dest_remote = false;
                }
            }
            catch (Exception) { }

            dest_path = dialog.textBoxDestination.Text == string.Empty ? dest_path : Path.Combine(dest_path, dialog.textBoxDestination.Text);
            CopyFileEngine copy_engine = new CopyFileEngine
            (source_list, dest_path, engine_opts, dialog_progress, dialog.textBoxSourceMask.Text, dest_remote);
            copy_engine.Done += new EventHandler(copy_engine_Done);
            copy_engine.CopyItemDone += new ItemEventHandler(copy_engine_CopyItemDone);

            //and do job
            copy_engine.Do();


        }

        void copy_engine_CopyItemDone(object sender, ItemEventArs e)
        {
            //notify parent
            OnItemProcessDone(e);
        }

        void copy_engine_Done(object sender, EventArgs e)
        {
            //call may be from back tread!
            CopyFileEngine engine = (CopyFileEngine)sender;
            engine.Dispose();

            if (dialog_progress.InvokeRequired)
            {
                dialog_progress.Invoke(new MethodInvoker(on_copy_done));
            }
            else
            {
                on_copy_done();
            }
        }

        private void on_copy_done()
        {
            Options.CopyCloseProgress = dialog_progress.checkBoxCloseOnFinish.Checked;
            if (dialog_progress.checkBoxCloseOnFinish.Checked)
            {
                dialog_progress.Close();
            }
        }
    }
}
