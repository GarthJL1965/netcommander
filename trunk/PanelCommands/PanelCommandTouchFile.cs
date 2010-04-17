using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.IO;

namespace netCommander
{
    class PanelCommandTouchFile : PanelCommandBase
    {

        public PanelCommandTouchFile()
            :base(Options.GetLiteral(Options.LANG_TOUCH),Shortcut.CtrlT)
        {
            
        }

        protected override void internal_command_proc()
        {
            try
            {
                QueryPanelInfoEventArgs e = new QueryPanelInfoEventArgs();
                OnQueryCurrentPanel(e);
                if (e.FocusedIndex == -1)
                {
                    return;
                }

                string target_file = string.Empty;
                DirectoryList dl = (DirectoryList)e.ItemCollection;
                int[] sel_indices = e.SelectedIndices;
                bool group_mode = (sel_indices.Length > 1);
                //we need cache list of selected files
                //becouse e.ItemCollection can change while processing (sort!)
                List<string> sel_names = new List<string>();
                for (int i = 0; i < sel_indices.Length; i++)
                {
                    sel_names.Add(e.ItemCollection.GetItemDisplayNameLong(sel_indices[i]));
                }


                if (!group_mode)
                {
                    if (sel_indices.Length == 1)
                    {
                        target_file = Path.Combine(dl.DirectoryPath, dl.GetItemDisplayNameLong(sel_indices[0]));
                    }
                    else
                    {
                        target_file = Path.Combine(dl.DirectoryPath, dl.GetItemDisplayNameLong(e.FocusedIndex));
                    }
                }
                if (target_file.EndsWith(".."))
                {
                    target_file = dl.DirectoryPath + Path.DirectorySeparatorChar;
                }

                FileSystemInfo target_fsi = null;
                if (!group_mode)
                {
                    target_fsi = IOhelper.GetFileSystemInfo(target_file);
                }

                TouchFileDialog dialog = new TouchFileDialog();

                dialog.Text = Options.GetLiteral(Options.LANG_TOUCH);

                if (!group_mode)
                {
                    dialog.textBoxFileName.Text = target_file;
                    dialog.dateTimePickerAccess.Value = target_fsi.LastAccessTime;
                    dialog.dateTimePickerCreation.Value = target_fsi.CreationTime;
                    dialog.dateTimePickerModification.Value = target_fsi.LastWriteTime;
                }
                else
                {
                    dialog.textBoxFileName.Text = string.Format("{0} "+Options.GetLiteral(Options.LANG_ENTRIES), sel_indices.Length);
                    dialog.textBoxFileName.Enabled = false;
                }

                if (dialog.ShowDialog() != DialogResult.OK)
                {
                    return;
                }

                if (!group_mode)
                {
                    if (dialog.textBoxFileName.Text != target_file)
                    {
                        //try create new file
                        netCommander.FileSystemEx.WinAPiFSwrapper.CreateNewEmptyFile(dialog.textBoxFileName.Text);
                        //if success
                        target_fsi = new FileInfo(dialog.textBoxFileName.Text);
                    }
                }

                //set times
                if (group_mode)
                {
                    foreach (string one_name in sel_names)
                    {
                        try
                        {
                            target_file = Path.Combine(dl.DirectoryPath, one_name);
                            target_fsi = IOhelper.GetFileSystemInfo(target_file);
                            if (dialog.dateTimePickerAccess.Checked)
                            {
                                target_fsi.LastAccessTime = dialog.dateTimePickerAccess.Value;
                            }
                            if (dialog.dateTimePickerCreation.Checked)
                            {
                                target_fsi.CreationTime = dialog.dateTimePickerCreation.Value;
                            }
                            if (dialog.dateTimePickerModification.Checked)
                            {
                                target_fsi.LastWriteTime = dialog.dateTimePickerModification.Value;
                            }
                            OnItemProcessDone(new ItemEventArs(one_name));
                        }
                        catch (Exception ex)
                        {
                            Messages.ShowException(ex);
                        }
                    }
                }
                else
                {
                    try
                    {
                        if (dialog.dateTimePickerAccess.Checked)
                        {
                            target_fsi.LastAccessTime = dialog.dateTimePickerAccess.Value;
                        }
                        if (dialog.dateTimePickerCreation.Checked)
                        {
                            target_fsi.CreationTime = dialog.dateTimePickerCreation.Value;
                        }
                        if (dialog.dateTimePickerModification.Checked)
                        {
                            target_fsi.LastWriteTime = dialog.dateTimePickerModification.Value;
                        }

                    }
                    catch (Exception ex)
                    {
                        Messages.ShowException(ex);
                    }
                }
            }
            catch (Exception ex)
            {
                Messages.ShowException(ex);
            }
        }// end of proc
    }//end of class
}

