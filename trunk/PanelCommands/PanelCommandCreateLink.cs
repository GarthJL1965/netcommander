using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using netCommander.FileSystemEx;
using System.IO;
using System.ComponentModel;
using System.Runtime.InteropServices;

namespace netCommander
{
    public class PanelCommandCreateLink : PanelCommandBase
    {
        public PanelCommandCreateLink()
            : base(Options.GetLiteral(Options.LANG_LINK_CREATE), Shortcut.AltF6)
        {

        }

        protected override void internal_command_proc()
        {
            QueryPanelInfoEventArgs e_current = new QueryPanelInfoEventArgs();
            QueryPanelInfoEventArgs e_other = new QueryPanelInfoEventArgs();
            OnQueryCurrentPanel(e_current);
            OnQueryOtherPanel(e_other);

            if (!(e_current.ItemCollection is DirectoryList))
            {
                return;
            }

            if (!(e_other.ItemCollection is DirectoryList))
            {
                Messages.ShowMessage(Options.GetLiteral(Options.LANG_WRONG_DESTINATION));
                return;
            }

            if (e_current.FocusedIndex == -1)
            {
                return;
            }

            DirectoryList dl_current = (DirectoryList)e_current.ItemCollection;
            DirectoryList dl_other = (DirectoryList)e_other.ItemCollection;
            bool group_mode = e_current.SelectedIndices.Length > 1;
            NTFSlinkType link_type = Options.LinkType;

            List<FileInfoEx> sels = new List<FileInfoEx>();
            if (e_current.SelectedIndices.Length > 0)
            {
                for (int i = 0; i < e_current.SelectedIndices.Length; i++)
                {
                    sels.Add(dl_current[e_current.SelectedIndices[i]]);
                }
            }
            else
            {
                sels.Add(dl_current[e_current.FocusedIndex]);
            }

            //show dialog
            CreateLinkDialog dialog = new CreateLinkDialog();
            dialog.Text = Options.GetLiteral(Options.LANG_LINK_CREATE);
            dialog.LinkType = link_type;
            dialog.textBoxLinkname.Text = string.Empty;
            if (group_mode)
            {
                dialog.textBoxLinkname.ReadOnly = true;
                dialog.textBoxLinktarget.Text = string.Format
                    ("{0} " + Options.GetLiteral(Options.LANG_ENTRIES),
                    sels.Count);
            }
            else
            {
                dialog.textBoxLinktarget.Text = sels[0].FileName;
            }

            if (dialog.ShowDialog() != DialogResult.OK)
            {
                return;
            }

            link_type = dialog.LinkType;
            Options.LinkType = link_type;

            foreach (FileInfoEx entry in sels)
            {
                IntPtr link_handle = IntPtr.Zero;
                //combine link name
                string link_name = string.Empty;
                if (dialog.textBoxLinkname.Text == string.Empty)
                {
                    //use target file name
                    link_name = Path.Combine(dl_other.DirectoryPath, entry.FileName);
                }
                else
                {
                    link_name = Path.Combine(dl_other.DirectoryPath, dialog.textBoxLinkname.Text);
                }

                try
                {
                    if ((entry.Directory) && (link_type == NTFSlinkType.Hard))
                    {
                        Messages.ShowMessage
                            (string.Format
                            (Options.GetLiteral(Options.LANG_CANNOT_CREATE_HARD_LINK_DIR_0),
                            entry.FullName));
                        continue;
                    }

                    if (link_type == NTFSlinkType.Hard)
                    {
                        int res = WinApiFS.CreateHardLink
                            (link_name,
                            entry.FullName,
                            IntPtr.Zero);
                        if (res == 0)
                        {
                            Messages.ShowException
                                (new Win32Exception(Marshal.GetLastWin32Error()),
                                string.Format
                                (Options.GetLiteral(Options.LANG_CANNOT_CREATE_LINK_0_ARROW_1),
                                entry.FullName,
                                link_name));
                        }
                        else
                        {
                            OnItemProcessDone(new ItemEventArs(entry.FileName));
                        }
                        continue;
                    }

                    if (link_type == NTFSlinkType.Symbolic)
                    {
                        try
                        {
                            WinAPiFSwrapper.CreateSymbolicLink(link_name, entry.FullName, entry.Directory);
                            OnItemProcessDone(new ItemEventArs(entry.FileName));
                        }
                        catch (Exception)
                        {
                            Messages.ShowException
                                (new Win32Exception(Marshal.GetLastWin32Error()),
                                string.Format
                                (Options.GetLiteral(Options.LANG_CANNOT_CREATE_LINK_0_ARROW_1),
                                entry.FullName,
                                link_name));
                        }
                        continue;
                    }

                    //link type is junction

                    WIN32_FIND_DATA link_data = new WIN32_FIND_DATA();
                    bool link_exist = WinAPiFSwrapper.GetFileInfo(link_name, ref link_data);

                    if (link_exist)
                    {
                        Messages.ShowMessage(string.Format(Options.GetLiteral(Options.LANG_DESTINATION_0_EXIST_OVERWRITING_PROHIBITED), link_name));
                        continue;
                    }

                    //jumction target must be directory, create directory
                    Directory.CreateDirectory(link_name);
                    //and retrieve handle
                    link_handle = WinApiFS.CreateFile_intptr
                        (link_name,
                        Win32FileAccess.WRITE_ATTRIBUTES | Win32FileAccess.WRITE_EA,
                        FileShare.ReadWrite,
                        IntPtr.Zero,
                        FileMode.Open,
                        CreateFileOptions.OPEN_REPARSE_POINT | CreateFileOptions.BACKUP_SEMANTICS,
                        IntPtr.Zero);
                    if (link_handle.ToInt32() == WinApiFS.INVALID_HANDLE_VALUE)
                    {
                        Win32Exception win_ex = new Win32Exception(Marshal.GetLastWin32Error());
                        Messages.ShowException
                            (win_ex,
                            string.Format
                            (Options.GetLiteral(Options.LANG_CANNOT_CREATE_LINK_0_ARROW_1),
                            link_name,
                            entry.FullName));
                        if (Directory.Exists(link_name))
                        {
                            Directory.Delete(link_name);
                        }
                        continue;
                    }
                    //now handle to link open
                    WinAPiFSwrapper.SetMountpoint(link_handle, entry.FullName, entry.FullName);

                    OnItemProcessDone(new ItemEventArs(entry.FileName));
                }//end try
                catch (Exception ex)
                {
                    Messages.ShowException
                        (ex,
                        string.Format
                        (Options.GetLiteral(Options.LANG_CANNOT_CREATE_LINK_0_ARROW_1),
                        entry.FullName,
                        link_name));
                }
                finally
                {
                    //close handle
                    if ((link_handle.ToInt32() != WinApiFS.INVALID_HANDLE_VALUE) && (link_handle != IntPtr.Zero))
                    {
                        WinApiFS.CloseHandle(link_handle);
                    }
                }
            }//end foreach

        }
    }
}
