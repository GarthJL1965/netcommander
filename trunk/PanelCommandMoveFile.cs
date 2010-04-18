using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.IO;

namespace netCommander
{
    public class PanelCommandMoveFile : PanelCommandBase
    {
        public PanelCommandMoveFile()
        {
            ToolStripMenuItem menu = new ToolStripMenuItem();
            menu.ShortcutKeys = Keys.F6;
            menu.ShowShortcutKeys = true;
            menu.Text = "Move/rename";
            CommandMenu = menu;
        }

        private CopyFileProgressDialog dialog_progress;

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
                Messages.ShowMessage("Wrong destination.");
                return;
            }

            //see source
            DirectoryList dl_source = (DirectoryList)e_current.ItemCollection;
            DirectoryList dl_target = (DirectoryList)e_other.ItemCollection;
            List<string> source_list = new List<string>();
            int[] sel_indices = e_current.SelectedIndices;
            if (sel_indices.Length == 0)
            {
                if (dl_source.GetItemDisplayNameLong(e_current.FocusedIndex) == "..")
                {
                    return;
                }

                source_list.Add(Path.Combine(dl_source.DirectoryPath, dl_source.GetItemDisplayNameLong(e_current.FocusedIndex)));
            }
            else
            {
                for (int i = 0; i < sel_indices.Length; i++)
                {
                    source_list.Add(Path.Combine(dl_source.DirectoryPath, dl_source.GetItemDisplayNameLong(sel_indices[i])));
                }
            }

            //prepare move dialog
            MoveFileDialog dialog = new MoveFileDialog();
            dialog.MoveEngineOptions = Options.MoveEngineOptions;
            dialog.Text = "Move/rename";
            if (source_list.Count == 1)
            {
                dialog.labelSourceFile.Text = source_list[0];
            }
            else
            {
                dialog.labelSourceFile.Text = string.Format("{0} entries", source_list.Count);
            }
            dialog.textBoxDestination.Text = dl_target.DirectoryPath;

            //and show
            if (dialog.ShowDialog() != DialogResult.OK)
            {
                return;
            }

            MoveEngineOptions move_opts = dialog.MoveEngineOptions;
            Options.MoveEngineOptions = move_opts;

            //prepare progress dialog
            dialog_progress = new CopyFileProgressDialog();
            dialog_progress.labelError.Visible = false;
            dialog_progress.labelSpeed.Text = string.Empty;
            dialog_progress.labelStatus.Text = string.Empty;
            dialog_progress.labelStatusTotal.Text = string.Empty;
            dialog_progress.checkBoxCloseOnFinish.Checked = Options.CopyCloseProgress;

            //calc location - it is not modal!
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

            //show progress
            dialog_progress.Show();
            dialog_progress.Location = new System.Drawing.Point(x_dialog, y_dialog);

            //prepare move engine
            MoveFileEngine move_engine = new MoveFileEngine
            (source_list.ToArray(),
            dialog.textBoxDestination.Text,
            move_opts,
            dialog_progress);
            move_engine.Done += new EventHandler(move_engine_Done);

            //and run
            move_engine.Do();

        }

        void move_engine_Done(object sender, EventArgs e)
        {
            //call may be from back tread!
            MoveFileEngine engine = (MoveFileEngine)sender;
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
