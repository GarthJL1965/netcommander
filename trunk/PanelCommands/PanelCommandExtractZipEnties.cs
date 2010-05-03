using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using ICSharpCode.SharpZipLib;
using ICSharpCode.SharpZipLib.Zip;
using System.IO;

namespace netCommander
{
    public class PanelCommandExtractZipEntries : PanelCommandBase
    {
        private CopyFileProgressDialog dialog_progress = new CopyFileProgressDialog();
        private ZipDirectory zip_directory = null;

        public PanelCommandExtractZipEntries()
        {
            MenuItem menu = new MenuItem();
            menu.Text = Options.GetLiteral(Options.LANG_EXTRACT);
            menu.Shortcut = Shortcut.F5;
            CommandMenu = menu;
        }

        protected override void internal_command_proc()
        {
            QueryPanelInfoEventArgs e_current = new QueryPanelInfoEventArgs();
            QueryPanelInfoEventArgs e_other = new QueryPanelInfoEventArgs();
            OnQueryCurrentPanel(e_current);
            OnQueryOtherPanel(e_other);

            //int buffer_size = 0x8000;

            if (!(e_other.ItemCollection is DirectoryList))
            {
                Messages.ShowMessage
                    (Options.GetLiteral(Options.LANG_WRONG_DESTINATION));
                return;
            }

            DirectoryList dl = (DirectoryList)e_other.ItemCollection;
            string dest_dir = dl.DirectoryPath;
            ZipDirectory zd = (ZipDirectory)e_current.ItemCollection;
            ZipFile source_zip_file = zd.ZipFile;
            string zip_current_dir=zd.CurrentZipDirectory;
            ArchiveExtractOptions opts = Options.ArchiveExtractOptions;

            //show user dialog
            ExtractDialog dialog = new ExtractDialog();
            dialog.ArchiveExtractOptions = opts;
            dialog.textBoxSourceMask.Text = "*";
            dialog.Text = Options.GetLiteral(Options.LANG_EXTRACT);
            if (dialog.ShowDialog() != DialogResult.OK)
            {
                return;
            }

            //retrieve options
            opts = dialog.ArchiveExtractOptions;
            string dest_path = Path.Combine(dest_dir, dialog.textBoxDestination.Text);
            string source_mask = dialog.textBoxSourceMask.Text;
            Options.ArchiveExtractOptions = opts;

            //retrieve source list
            List<string> source_list = new List<string>();
            if ((e_current.SelectedIndices.Length == 0) && (e_current.FocusedIndex != 0))
            {
                //focused index==0 always ref to parent entry, so skip it
                source_list.Add(zd.GetItemDisplayName(e_current.FocusedIndex));
            }
            else
            {
                for (int i = 0; i < e_current.SelectedIndices.Length; i++)
                {
                    if (e_current.SelectedIndices[i] == 0)
                    {
                        continue;
                    }

                    source_list.Add(zd.GetItemDisplayName(e_current.SelectedIndices[i]));
                }
            }

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

            ZipExtractEngine engine = new ZipExtractEngine
            (source_list,
            dest_path,
            opts,
            dialog_progress,
            dialog.textBoxSourceMask.Text,
            zd.ZipFile,
            zd.CurrentZipDirectory);

            engine.Done += new EventHandler(engine_Done);
            engine.ExtractItemDone += new ItemEventHandler(engine_ExtractItemDone);

            zip_directory = zd;
            zd.LockSafe = true;

            engine.Run();

            //ZipEntry source_entry = zd[e_current.FocusedIndex];
            //if (source_entry == null)
            //{
            //    return;
            //}

            //Stream out_stream = source_zip_file.GetInputStream(source_entry);
            //string target_file_name = Path.Combine(dest_dir, source_entry.Name);
            //FileStream writer = new FileStream(target_file_name, FileMode.CreateNew, FileAccess.Write, FileShare.Read);
            //byte[] buffer = new byte[buffer_size];
            //int bytes_readed = 0;
            //while ((bytes_readed = out_stream.Read(buffer, 0, buffer_size)) != 0)
            //{
            //    writer.Write(buffer, 0, bytes_readed);
            //}
            //writer.Flush();
            //writer.Close();
            //out_stream.Close();

        }

        void engine_ExtractItemDone(object sender, ItemEventArs e)
        {
            OnItemProcessDone(e);
        }

        void engine_Done(object sender, EventArgs e)
        {
            //call may be from back tread!
            //CopyFileEngine engine = (ZipExtractEngine)sender;
            //engine.Dispose();

            zip_directory.LockSafe = false;

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

        private void extract_one_entry
            (ZipFile zip_file,
            ZipEntry zip_entry,
            string destination_file,
            ArchiveExtractOptions options)
        {
            //check destination if ovewrite newer

            //open streams

            //copy streams

            //close streams
        }

        
    }

    [Flags()]
    public enum ArchiveExtractOptions
    {
        None = 0,
        SupressExceptions = 0x1,
        NeverOverwite = 0x2,
        OverwriteIfSourceNewer = 0x4,
        OverwriteAlways = 0x8,
        ExtractAttributes = 0x10,
        CreateEmptyDirectories = 0x20,
        ExtractRecursively = 0x40
    }
}
