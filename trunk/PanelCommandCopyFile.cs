using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using netCommander.FileSystemEx;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using System.Threading;

namespace netCommander
{
    class PanelCommandCopyFile : PanelCommandBase
    {
        public PanelCommandCopyFile()
        {
            ToolStripMenuItem menu = new ToolStripMenuItem();
            menu.ShortcutKeys = Keys.F5;
            menu.ShowShortcutKeys = true;
            menu.Text = "Copy";
            CommandMenu = menu;

            //copy_progress_callback = new CopyProgressRoutine
        }

        private CopyFileProgressDialog dialog_progress = null;
        private List<string> source_list = null;
        private string destination_path = string.Empty;
        //private bool destination_exact = false;
        private CopyEngineOptions engine_opts = Options.CopyEngineOptions;

        //private CopyProgressRoutine copy_progress_callback;
        private string current_destination = string.Empty;
        private string current_source = string.Empty;

        

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
            if (!(e_other.ItemCollection is DirectoryList))
            {
                Messages.ShowMessage("Cannot copy to current destination.");
                return;
            }

            //see source
            source_list = new List<string>();
            DirectoryList dl_source = (DirectoryList)e_current.ItemCollection;
            DirectoryList dl_target=(DirectoryList)e_other.ItemCollection;
            int[] sel_indices = e_current.SelectedIndices;
            if (sel_indices.Length == 0)
            {
                if (dl_source.GetItemDisplayNameLong(e_current.FocusedIndex) == "..")
                {
                    return;
                }
                //get focused entry
                source_list.Add(Path.Combine(dl_source.DirectoryPath, dl_source.GetItemDisplayNameLong(e_current.FocusedIndex)));
            }
            else
            {
                //get source from selection
                for (int i = 0; i < sel_indices.Length; i++)
                {
                    source_list.Add(Path.Combine(dl_source.DirectoryPath, dl_source.GetItemDisplayNameLong(sel_indices[i])));
                }
            }

            string dest_path = dl_target.DirectoryPath;

            //prepare copy dialog
            CopyFileDialog dialog = new CopyFileDialog();
            dialog.CopyEngineOptions = engine_opts;
            dialog.Text = "Copy";
            if (source_list.Count == 1)
            {
                dialog.labelSourceFile.Text = source_list[0];
            }
            else
            {
                dialog.labelSourceFile.Text = string.Format("{0} entries", source_list.Count);
            }
            dialog.textBoxDestination.Text = dest_path;

            if (dialog.ShowDialog() != DialogResult.OK)
            {
                return;
            }

            //save user selection
            engine_opts = dialog.CopyEngineOptions;
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
            CopyFileEngine copy_engine = new CopyFileEngine
            (source_list.ToArray(), dialog.textBoxDestination.Text, engine_opts, dialog_progress);
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
