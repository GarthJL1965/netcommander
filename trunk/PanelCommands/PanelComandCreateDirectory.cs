using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using netCommander.FileSystemEx;
using System.IO;
using ICSharpCode.SharpZipLib;
using ICSharpCode.SharpZipLib.Zip;

namespace netCommander
{
    public class PanelComandCreateDirectory : PanelCommandBase
    {
        public PanelComandCreateDirectory()
            : base(Options.GetLiteral(Options.LANG_DIRECTORY_CREATE), Shortcut.F7)
        {

        }

        private void create_fs_directory(QueryPanelInfoEventArgs e_current,QueryPanelInfoEventArgs e_other)
        {
            DirectoryList dl_current = (DirectoryList)e_current.ItemCollection;
            DirectoryList dl_other = null;
            if (e_other.ItemCollection is DirectoryList)
            {
                dl_other = (DirectoryList)e_other.ItemCollection;
            }

            //prepare dialog
            CreateDirectoryDialog dialog = new CreateDirectoryDialog();
            dialog.Text = CommandMenu.Text;
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
                Messages.ShowMessage(Options.GetLiteral(Options.LANG_DIRECTORY_EXISTS));
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
                WinAPiFSwrapper.CreateDirectoryTree(new_directory_name, template_dir);
            }
            catch (Exception ex)
            {
                Messages.ShowException
                    (ex,
                    string.Format
                    (Options.GetLiteral(Options.LANG_CANNOT_CREATE_DIRECTORY_0),
                    new_directory_name));
            }
        }

        private void create_zip_directory(QueryPanelInfoEventArgs e_current)
        {
            ZipDirectory zd = (ZipDirectory)e_current.ItemCollection;

            //prepare dialog
            CreateDirectoryDialog dialog = new CreateDirectoryDialog();
            dialog.Text = CommandMenu.Text;
            dialog.labelParentDir.Text = zd.CurrentZipDirectory + "/";
            dialog.checkBoxUseTemplate.Checked = false;
            dialog.checkBoxUseTemplate.Enabled = false;
            dialog.textBoxTemplateDirectory.Enabled = false;

            if (dialog.ShowDialog() != DialogResult.OK)
            {
                return;
            }
            if (dialog.textBoxDirectoryName.Text.Length == 0)
            {
                Messages.ShowMessage("directory name cannot be empty");
                return;
            }

            string new_directory_name = string.Empty;
            if (zd.CurrentZipDirectory == string.Empty)
            {
                new_directory_name = dialog.textBoxDirectoryName.Text;
            }
            else
            {
                new_directory_name = zd.CurrentZipDirectory + "/" + dialog.textBoxDirectoryName.Text;
            }
            if (!new_directory_name.EndsWith("/"))
            {
                new_directory_name = new_directory_name + "/";
            }

            try
            {
                ZipEntry dir_entry = zd.ZipFile.EntryFactory.MakeDirectoryEntry(new_directory_name);
                dir_entry.Size = 0;
                dir_entry.CompressedSize = 0; //required, else exception throws
                zd.ZipFile.BeginUpdate();
                zd.ZipFile.Add(dir_entry);
                zd.ZipFile.CommitUpdate();
                zd.Refill();
            }
            catch (Exception ex)
            {
                Messages.ShowException(ex);
            }
        }

        protected override void internal_command_proc()
        {
            QueryPanelInfoEventArgs e_current = new QueryPanelInfoEventArgs();
            QueryPanelInfoEventArgs e_other = new QueryPanelInfoEventArgs();
            OnQueryCurrentPanel(e_current);
            OnQueryOtherPanel(e_other);

            if (e_current.ItemCollection is DirectoryList)
            {
                create_fs_directory(e_current, e_other);
            }
            else if (e_current.ItemCollection is ZipDirectory)
            {
                create_zip_directory(e_current);
            }
            
        }
    }
}
