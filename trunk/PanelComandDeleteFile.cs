using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.IO;
using netCommander.FileSystemEx;
using System.Runtime.InteropServices;
using System.ComponentModel;

namespace netCommander
{
    class PanelComandDeleteFile : PanelCommandBase
    {
        public PanelComandDeleteFile()
        {
            ToolStripMenuItem menu = new ToolStripMenuItem();
            menu.ShortcutKeys = Keys.F8;
            menu.ShowShortcutKeys = true;
            menu.Text = "Delete";
            CommandMenu = menu;
        }

        private WIN32_FIND_DATA fs_data = new WIN32_FIND_DATA();
        private bool force_readonly = false;
        private bool supress_exceptions = false;
        protected override void internal_command_proc()
        {
            QueryPanelInfoEventArgs e = new QueryPanelInfoEventArgs();
            OnQueryCurrentPanel(e);

            if (e.FocusedIndex == -1)
            {
                return;
            }

            DirectoryList dl=(DirectoryList)e.ItemCollection;
            int[] sel_indices = e.SelectedIndices;
            //we must cache selection
            //becouse indexes will be change while deleting
            List<string> sel_names = new List<string>();
            if (sel_indices.Length > 0)
            {
                for (int i = 0; i < sel_indices.Length; i++)
                {
                    sel_names.Add(Path.Combine(dl.DirectoryPath, dl.GetItemDisplayNameLong(sel_indices[i])));
                }
            }
            else
            {
                sel_names.Add(Path.Combine(dl.DirectoryPath, dl.GetItemDisplayNameLong(e.FocusedIndex)));
            }

            //prepare dialog
            DeleteFileDialog dialog = new DeleteFileDialog();
            dialog.Text = "Delete files";
            if (sel_names.Count == 1)
            {
                if (IOhelper.IsDirectory(sel_names[0]))
                {
                    dialog.labelQuestion.Text = string.Format("Do you REALLY want to delete '{0}'? This directory and all its contents it will be destroyed for ever.", sel_names[0]);
                }
                else
                {
                    dialog.labelQuestion.Text = string.Format("Do you REALLY want to delete '{0}'? The file will be destroyed for ever.", sel_names[0]);
                }
            }
            else
            {
                dialog.labelQuestion.Text = string.Format("Do you REALLY want to delete {0} entries? All selected files and directories with the contents will be destroyed for ever.", sel_names.Count);
            }
            dialog.checkBoxForceReadonly.Checked = Options.DeleteReadonly;
            dialog.checkBoxSupressExceptions.Checked = Options.DeleteSupressExceptions;

            if (dialog.ShowDialog() != DialogResult.OK)
            {
                return;
            }

            //save user selection
            Options.DeleteReadonly = dialog.checkBoxForceReadonly.Checked;
            Options.DeleteSupressExceptions = dialog.checkBoxSupressExceptions.Checked;
            force_readonly = dialog.checkBoxForceReadonly.Checked;
            supress_exceptions = dialog.checkBoxSupressExceptions.Checked;

            //and enum entries in sel_names and recursively delete each
            foreach (string one_name in sel_names)
            {
                delete_file_recurs(one_name);
            }

        }

        private void delete_file_recurs(string current_file)
        {
            bool readonly_changed = false;
            int res=0;
            int winErr = 0;
            try
            {
                Wrapper.GetFileInfo(current_file, ref fs_data);
            }
            catch (Exception ex)
            {
                process_exceptions(ex);

            }
            if ((fs_data.dwFileAttributes & FileAttributes.Directory) == FileAttributes.Directory)
            {
                try
                {
                    //this is deirectory
                    //current_file is directory
                    //enum entries and call recurs for each
                    string search_path = current_file;
                    search_path = search_path.TrimEnd(new char[] { Path.DirectorySeparatorChar });
                    search_path = search_path + Path.DirectorySeparatorChar + "*";
                    Wrapper.WIN32_FIND_DATA_enumerable fs_enum = new Wrapper.WIN32_FIND_DATA_enumerable(search_path);
                    foreach (WIN32_FIND_DATA data in fs_enum)
                    {
                        //skip refs to current and up dirs
                        if (data.cFileName == ".")
                        {
                            continue;
                        }
                        if (data.cFileName == "..")
                        {
                            continue;
                        }
                        delete_file_recurs(Path.Combine(current_file, data.cFileName));
                    }
                    //now directory current_file must be empty
                    //check attr
                    try
                    {
                        if ((fs_data.dwFileAttributes & FileAttributes.ReadOnly) == FileAttributes.ReadOnly)
                        {
                            if (force_readonly)
                            {
                                clear_readonly(current_file);
                                readonly_changed = true;
                            }
                            else
                            {
                                if (!supress_exceptions)
                                {
                                    Messages.ShowMessage(string.Format("Cannot delete '{0}'. Directory have readonly attribute.", current_file));
                                    return;
                                }
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        process_exceptions(ex);
                    }
                    //and delete
                    try
                    {
                        ///////////////////////////////////////////////////////////////////////////////////////
                        /* DEBUG */
                        //if (current_file.EndsWith("netcommander", StringComparison.CurrentCultureIgnoreCase))
                        //{
                        //    int j = 0;
                        //    j++;
                        //}
                        ///////////////////////////////////////////////////////////////////////////////////////
                        res = Native.RemoveDirectory(current_file);
                        if (res == 0)
                        {
                            //restore attr
                            if (readonly_changed)
                            {
                                set_readonly(current_file);
                            }
                            if (!supress_exceptions)
                            {
                                //throw exception
                                winErr = Marshal.GetLastWin32Error();
                                throw new Win32Exception(winErr);
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        process_exceptions(ex);
                    }
                }
                catch (Exception ex)
                {
                    process_exceptions(ex);
                }
            }
            else
            {
                //current_file is FILE
                //check attributes
                try
                {
                    if ((fs_data.dwFileAttributes & FileAttributes.ReadOnly) == FileAttributes.ReadOnly)
                    {
                        if (force_readonly)
                        {
                            clear_readonly(current_file);
                            readonly_changed = true;
                        }
                        else
                        {
                            if (!supress_exceptions)
                            {
                                Messages.ShowMessage(string.Format("Cannot delete '{0}'. File have readonly attribute.", current_file));
                                return;
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    process_exceptions(ex);
                }
                //now try delete file
                try
                {
                    res = Native.DeleteFile(current_file);
                    if (res == 0)
                    {
                        //restore attr
                        if (readonly_changed)
                        {
                            set_readonly(current_file);
                        }
                        if (!supress_exceptions)
                        {
                            winErr = Marshal.GetLastWin32Error();
                            throw new Win32Exception(winErr);
                        }
                    }
                }
                catch (Exception ex)
                {
                    process_exceptions(ex);
                }
            }
        }

        private void clear_readonly(string file_name)
        {
            FileSystemInfo fsi = IOhelper.GetFileSystemInfo(file_name);
            FileAttributes fa_existing = fsi.Attributes;
            FileAttributes fs_feeded=~((~fa_existing) | FileAttributes.ReadOnly);
            fsi.Attributes = fs_feeded;
        }

        private void set_readonly(string file_name)
        {
            FileSystemInfo fsi = IOhelper.GetFileSystemInfo(file_name);
            FileAttributes fa_existing = fsi.Attributes;
            FileAttributes fa_needed = FileAttributes.ReadOnly | fa_existing;
            fsi.Attributes = fa_needed;
        }

        private void process_exceptions(Exception ex)
        {
            if (!supress_exceptions)
            {
                Messages.ShowException(ex);
            }
        }

    }
}
