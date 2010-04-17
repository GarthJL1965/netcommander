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
            : base(Options.GetLiteral(Options.LANG_DELETE), Shortcut.F8)
        {

        }

        private DeleteFileOptions opts;
        private string delete_mask = "*";
        private mainForm main_window = null;

        protected override void internal_command_proc()
        {
            abort = false;
            QueryPanelInfoEventArgs e = new QueryPanelInfoEventArgs();
            OnQueryCurrentPanel(e);

            if (e.FocusedIndex == -1)
            {
                return;
            }

            if (!(e.ItemCollection is DirectoryList))
            {
                return;
            }

            DirectoryList dl=(DirectoryList)e.ItemCollection;
            main_window = dl.MainWindow;
            int[] sel_indices = e.SelectedIndices;
            //we must cache selection
            //becouse indexes will be change while deleting
            List<FileInfoEx> sel_files = new List<FileInfoEx>();
            if (sel_indices.Length > 0)
            {
                for (int i = 0; i < sel_indices.Length; i++)
                {
                    sel_files.Add(dl[sel_indices[i]]);
                }
            }
            else
            {
                if (dl[e.FocusedIndex].FileName == "..")
                {
                    Messages.ShowMessage(Options.GetLiteral(Options.LANG_WRONG_DESTINATION));
                    return;
                }
                sel_files.Add(dl[e.FocusedIndex]);
            }

            //now we have list for delete

            opts = Options.DeleteFileOptions;
            
            //show user dialog...
            DeleteFileDialog dialog = new DeleteFileDialog();
            dialog.Text = CommandMenu.Text;
            dialog.DeleteFileOptions = opts;
                dialog.textBoxMask.Text = "*";

            if (dialog.ShowDialog() != DialogResult.OK)
            {
                return;
            }

            opts = dialog.DeleteFileOptions;
            Options.DeleteFileOptions = opts;
            delete_mask = dialog.textBoxMask.Text;

            if (sel_files.Count == 1)
            {
                delete_entry(sel_files[0]);
            }
            else
            {
                foreach (FileInfoEx info in sel_files)
                {
                    if (abort)
                    {
                        break;
                    }
                    delete_entry(info);
                }
            }
            if (main_window != null)
            {
                main_window.NotifyLongOperation(string.Empty, false);
            }
        }

        private bool abort = false;
        private void delete_entry(FileInfoEx entry)
        {
            /*
             * We first try to delete directory (if set flag DeleteEmptyDirectories)
             * if it is link, DeleteFile finction deletes link, not target, even if target not empty
             * and return.
             * If flag DeleteEmptyDirectories not setted, we check LinkInformation and 
             * if it not null (that is entry is soft link) -> return, else
             * remove its contains.
             */

            int win_err = 0;
            int res = 0;
            if (abort)
            {
                return;
            }

            if (!check_readonly(entry))
            {
                return;
            }

            if (entry.Directory)
            {
                if (!check_readonly(entry))
                {
                    return;
                }

                //first try to delete dir, assume it is empty
                if ((opts & DeleteFileOptions.DeleteEmptyDirectories) == DeleteFileOptions.DeleteEmptyDirectories)
                {
                    res = WinApiFS.RemoveDirectory(entry.FullName);
                    if (res == 0)
                    {
                        win_err = Marshal.GetLastWin32Error();
                        if (win_err == WinApiFS.ERROR_DIR_NOT_EMPTY)
                        {
                            //directory not empty
                            if ((opts & DeleteFileOptions.RecursiveDeleteFiles) == DeleteFileOptions.RecursiveDeleteFiles)
                            {
                                //recursive delete needed
                                FileInfoExEnumerable dir_enum = new FileInfoExEnumerable
                                (Path.Combine(entry.FullName, "*"), false, false, false, true);
                                try
                                {
                                    foreach (FileInfoEx info in dir_enum)
                                    {
                                        delete_entry(info);
                                    }
                                }
                                catch (Exception ex)
                                {
                                    abort = !process_errors
                                        (string.Format(Options.GetLiteral(Options.LANG_CANNOT_DELETE_0), entry.FullName),
                                        ex);
                                }
                            }//end of dir enum
                            //now dir probably empty
                            res = WinApiFS.RemoveDirectory(entry.FullName);
                            if (res == 0)
                            {
                                //not success
                                win_err = Marshal.GetLastWin32Error();
                                if (win_err != WinApiFS.ERROR_DIR_NOT_EMPTY)
                                    //supress -> when delete on mask,
                                    //dir may be not empty
                                {
                                    abort = !process_errors
                                    (string.Format(Options.GetLiteral(Options.LANG_CANNOT_DELETE_0), entry.FullName),
                                            new Win32Exception(win_err));
                                }
                            }
                        }//end of dir not empty
                        else
                        {
                            abort = !process_errors
                                (string.Format(Options.GetLiteral(Options.LANG_CANNOT_DELETE_0), entry.FullName),
                                        new Win32Exception(win_err));
                        }
                    }//end of error process
                }
                else
                {
                    //not to delete empty dirs
                    //recursive delete needed (if not link) but directory not will remove
                    //check reparse tag
                    if ((opts & DeleteFileOptions.RecursiveDeleteFiles) == DeleteFileOptions.RecursiveDeleteFiles)
                    {
                        if (entry.LinkInformation == null)
                        {
                            FileInfoExEnumerable dir_enum = new FileInfoExEnumerable
                            (Path.Combine(entry.FullName, "*"), false, false, false, true);
                            try
                            {
                                foreach (FileInfoEx info in dir_enum)
                                {
                                    delete_entry(info);
                                }
                            }
                            catch (Exception ex)
                            {
                                abort = !process_errors
                                    (string.Format(Options.GetLiteral(Options.LANG_CANNOT_DELETE_0), entry.FullName),
                                    ex);
                            }
                        }
                    }
                }
            }//end of if(entry.Directory)
            else
            {
                //entry is file

                //check mask
                if (Wildcard.Match(delete_mask, entry.FileName, false))
                {
                    if (check_readonly(entry))
                    {
                        if (main_window != null)
                        {
                            main_window.NotifyLongOperation
                                (string.Format(Options.GetLiteral(Options.LANG_DELETE_NOW_0), entry.FullName),
                                true);
                        }
                        res = WinApiFS.DeleteFile(entry.FullName);
                        if (res == 0)
                        {
                            abort = !process_errors
                                (string.Format(Options.GetLiteral(Options.LANG_CANNOT_DELETE_0), entry.FullName),
                                new Win32Exception(Marshal.GetLastWin32Error()));
                        }
                    }
                }
            }
        }

        private bool check_readonly(FileInfoEx info)
        {
            if ((opts & DeleteFileOptions.DeleteReadonly) != DeleteFileOptions.DeleteReadonly)
            {
                return true;
            }

            if ((info.FileAttributes & FileAttributes.ReadOnly) == FileAttributes.ReadOnly)
            {
                try
                {
                    clear_readonly(info.FullName);
                    return true;
                }
                catch (Exception ex)
                {
                    abort = !process_errors
                        (string.Format
                        (Options.GetLiteral(Options.LANG_CANNOT_CLEAR_HIDDEN_READONLY_ATTR_DESTINATION_0),
                        info.FullName),
                        ex);
                }
            }
            else
            {
                return true;
            }
            return false;
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

        private bool process_errors(string info, Exception ex)
        {
            return Messages.ShowExceptionContinue(ex, info);
        }

    }

    [Flags()]
    public enum DeleteFileOptions
    {
        None = 0,
        RecursiveDeleteFiles = 0x1,
        DeleteEmptyDirectories = 0x2,
        //RecursiveDeleteLinkTargets = 0x4,
        //DeleteLinks = 0x8,
        DeleteReadonly = 0x10
    }
}
