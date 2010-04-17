using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;

namespace netCommander
{
    public class PanelCommandFtpCreateDirectory : PanelCommandBase
    {
        public PanelCommandFtpCreateDirectory()
            : base(Options.GetLiteral(Options.LANG_DIRECTORY_CREATE), Shortcut.F7)
        {

        }

        protected override void internal_command_proc()
        {
            QueryPanelInfoEventArgs e_current = new QueryPanelInfoEventArgs();
            OnQueryCurrentPanel(e_current);
            FtpDirectoryList ftp_list = (FtpDirectoryList)e_current.ItemCollection;

            string current_dir = ftp_list.DirectoryPath;

            //show dialog
            CreateDirectoryDialog dialog = new CreateDirectoryDialog();
            dialog.Text = Options.GetLiteral(Options.LANG_DIRECTORY_CREATE);
            dialog.labelParentDir.Text = current_dir;
            dialog.checkBoxUseTemplate.Enabled = false;
            dialog.textBoxTemplateDirectory.Enabled = false;

            if (dialog.ShowDialog() != DialogResult.OK)
            {
                return;
            }

            string new_dir=FtpPath.Combine(current_dir,dialog.textBoxDirectoryName.Text);
            //create dir tree
            if (ftp_list.MainWindow != null)
            {
                ftp_list.MainWindow.NotifyLongOperation
                    (string.Format
                    (Options.GetLiteral(Options.LANG_DIRECTORY_CREATE_0),
                    new_dir),
                    true);
            }

            try
            {
                ftp_list.Connection.CreateDirectoryTree(new_dir);
            }
            catch (Exception ex)
            {
                Messages.ShowException(ex);
            }

            if (ftp_list.MainWindow != null)
            {
                ftp_list.MainWindow.NotifyLongOperation
                    (string.Empty,
                    false);
            }

            ftp_list.Refill();
        }
    }
}
