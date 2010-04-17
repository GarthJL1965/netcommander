using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using System.Threading;

namespace netCommander
{
    public class PanelCommandFtpDownload : PanelCommandBase
    {
        public PanelCommandFtpDownload()
            : base(Options.GetLiteral(Options.LANG_DOWNLOAD), Shortcut.F5)
        {

        }

        private CopyFileProgressDialog dialog_progress = null;

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
                Messages.ShowMessage(Options.GetLiteral(Options.LANG_WRONG_DESTINATION));
                return;
            }

            FtpDirectoryList ftp_list = (FtpDirectoryList)e_current.ItemCollection;
            DirectoryList dir_list = (DirectoryList)e_other.ItemCollection;

            List<FtpEntryInfo> sel_source = new List<FtpEntryInfo>();
            if (e_current.SelectedIndices.Length == 0)
            {
                sel_source.Add(ftp_list[e_current.FocusedIndex]);
            }
            else
            {
                for (int i = 0; i < e_current.SelectedIndices.Length; i++)
                {
                    sel_source.Add(ftp_list[e_current.SelectedIndices[i]]);
                }
            }

            string destination = dir_list.DirectoryPath;

            //prepare and show user dialog
            FtpTransferOptions ftp_trans_opts = Options.FtpDownloadOptions;
            FtpTransferDialog dialog=new FtpTransferDialog();
            dialog.FtpTransferOptions = ftp_trans_opts;
            dialog.textBoxDestination.Text = destination;

            if (sel_source.Count == 1)
            {
                dialog.labelSourceFile.Text =
                    ftp_list.Connection.Options.ServerName + FtpPath.Combine(sel_source[0].DirectoryPath, sel_source[0].EntryName);
            }
            else
            {
                dialog.labelSourceFile.Text =
                    string.Format
                    ("{0} items from {1}",
                    sel_source.Count,
                    ftp_list.Connection.Options.ServerName);
            }

            if (dialog.ShowDialog() != DialogResult.OK)
            {
                return;
            }

            //retrieve user selection
            ftp_trans_opts = dialog.FtpTransferOptions;
            destination = dialog.textBoxDestination.Text;

            //save user selection
            Options.FtpDownloadOptions = ftp_trans_opts;

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

            //prepare doanload engine
            FtpDownloadEngine engine=new FtpDownloadEngine
            (sel_source.ToArray(),
            destination,
            ftp_list.Connection,
            ftp_trans_opts,
            dialog_progress);

            engine.Done += new EventHandler(engine_Done);
            engine.DownloadItemDone += new ItemEventHandler(engine_DownloadItemDone);

            engine.Run();
        }

        void engine_DownloadItemDone(object sender, ItemEventArs e)
        {
            //notify parent
            OnItemProcessDone(e);
        }

        void engine_Done(object sender, EventArgs e)
        {
            //call may be from back tread!

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
