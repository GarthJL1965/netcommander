using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using netCommander.FileSystemEx;
using System.IO;

namespace netCommander
{
    public class PanelComandCreateDirectory : PanelCommandBase
    {
        public PanelComandCreateDirectory()
        {
            ToolStripMenuItem menu = new ToolStripMenuItem();
            menu.ShortcutKeys = Keys.F7;
            menu.ShowShortcutKeys = true;
            menu.Text = "Create directory";
            CommandMenu = menu;
        }

        protected override void internal_command_proc()
        {
            QueryPanelInfoEventArgs e_current = new QueryPanelInfoEventArgs();
            QueryPanelInfoEventArgs e_other = new QueryPanelInfoEventArgs();
            OnQueryCurrentPanel(e_current);
            OnQueryOtherPanel(e_other);
            DirectoryList dl_current = (DirectoryList)e_current.ItemCollection;
            DirectoryList dl_other = null;
            if (e_other.ItemCollection is DirectoryList)
            {
                dl_other = (DirectoryList)e_other.ItemCollection;
            }

            //prepare dialog
            CreateDirectoryDialog dialog = new CreateDirectoryDialog();
            dialog.Text = "Create directory";
            dialog.labelParentDir.Text = dl_current.DirectoryPath + Path.DirectorySeparatorChar;
            dialog.checkBoxUseTemplate.Checked = false;
            dialog.textBoxTemplateDirectory.Enabled = false;
            if ((dl_other == null) || (e_other.FocusedIndex <= 0))
            {
                dialog.checkBoxUseTemplate.Checked = false;
                dialog.textBoxTemplateDirectory.Enabled = false;
            }
            else
            {
                dialog.textBoxTemplateDirectory.Text = Path.Combine(dl_other.DirectoryPath, dl_other.GetItemDisplayNameLong(e_other.FocusedIndex));
            }

            if (dialog.ShowDialog() != DialogResult.OK)
            {
                return;
            }

            if (dialog.textBoxDirectoryName.Text == string.Empty)
            {
                Messages.ShowMessage("Directory exists.");
                return;
            }

            string new_directory_name = Path.Combine(dl_current.DirectoryPath, dialog.textBoxDirectoryName.Text);
            string template_dir = string.Empty;
            if (dialog.checkBoxUseTemplate.Checked)
            {
                template_dir = dialog.textBoxTemplateDirectory.Text;
            }

            try
            {
                Wrapper.CreateDirectoryTree(new_directory_name, template_dir);
            }
            catch (Exception ex)
            {
                Messages.ShowException(ex, string.Format("Failed to create directory '{0}'.", new_directory_name));
            }
        }
    }
}
