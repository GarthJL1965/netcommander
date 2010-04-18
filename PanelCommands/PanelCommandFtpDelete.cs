using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Net;

namespace netCommander
{
    public class PanelCommandFtpDelete : PanelCommandBase
    {
        private bool supress_errors = false;
        private FtpConnection connection;
        private bool abort = false;

        public PanelCommandFtpDelete()
            : base(Options.GetLiteral(Options.LANG_DELETE), Shortcut.F8)
        {

        }

        protected override void internal_command_proc()
        {
            QueryPanelInfoEventArgs e = new QueryPanelInfoEventArgs();
            OnQueryCurrentPanel(e);

            if (e.FocusedIndex == -1)
            {
                return;
            }

            FtpDirectoryList ftp_list = (FtpDirectoryList)e.ItemCollection;
            connection = ftp_list.Connection;
            List<FtpEntryInfo> sels = new List<FtpEntryInfo>();
            if (e.SelectedIndices.Length == 0)
            {
                sels.Add(ftp_list[e.FocusedIndex]);
            }
            else
            {
                for (int i = 0; i < e.SelectedIndices.Length; i++)
                {
                    sels.Add(ftp_list[e.SelectedIndices[i]]);
                }
            }

            //show delete dialog
            DeleteFileDialog dialog = new DeleteFileDialog();
            dialog.Text = "Delete files";
            if (sels.Count == 1)
            {
                if (sels[0].Directory)
                {
                    //dialog.labelQuestion.Text =
                    //    string.Format
                    //    ("Do you REALLY want to delete '{0}'? This directory and all its contents it will be destroyed for ever.",
                    //    FtpPath.Combine(sels[0].DirectoryPath, sels[0].EntryName));
                }
                else
                {
                    //dialog.labelQuestion.Text = 
                    //    string.Format
                    //    ("Do you REALLY want to delete '{0}'? The file will be destroyed for ever.",
                    //    FtpPath.Combine(sels[0].DirectoryPath, sels[0].EntryName));
                }
            }
            else
            {
                //dialog.labelQuestion.Text =
                //    string.Format
                //    ("Do you REALLY want to delete {0} entries? All selected files and directories with the contents will be destroyed for ever.",
                //    sels.Count);
            }
            //dialog.checkBoxForceReadonly.Checked = false;
            //dialog.checkBoxForceReadonly.Enabled = false;
            //dialog.checkBoxSupressExceptions.Checked = false;

            if (dialog.ShowDialog() != DialogResult.OK)
            {
                return;
            }

            //save user selection
            //Options.DeleteSupressExceptions = dialog.checkBoxSupressExceptions.Checked;
            //supress_errors = dialog.checkBoxSupressExceptions.Checked;

            //and kill
            foreach (FtpEntryInfo info in sels)
            {
                delete_recursive(info, ftp_list.MainWindow);
            }
            if (ftp_list.MainWindow != null)
            {
                ftp_list.MainWindow.NotifyLongOperation
                    (string.Empty, false);
            }

            e.ItemCollection.Refill();
        }

        private void delete_recursive(FtpEntryInfo info,mainForm main_window)
        {
            if (abort)
            {
                return;
            }

            if (info.Directory)
            {
                //first we must delete all contents of directory
                //get contents
                //and notify main window
                if (main_window != null)
                {

                    main_window.NotifyLongOperation
                        (string.Format
                        (Options.GetLiteral(Options.LANG_FTP_WAIT_RETRIEVE_DIRECTORY_LIST)),
                        true);
                }
                List<FtpEntryInfo> contents = new List<FtpEntryInfo>();
                try
                {
                    contents =
                        connection.GetDirectoryDetailsList
                        (FtpPath.AppendEndingSeparator(FtpPath.Combine(info.DirectoryPath, info.EntryName)), true);
                }
                catch (Exception ex)
                {
                    abort = !process_error
                        (string.Format
                        (Options.GetLiteral(Options.LANG_CANNOT_READ_DIRECTORY_CONTENTS_0),
                        FtpPath.Combine(info.DirectoryPath, info.EntryName)),
                        ex);
                }
                if (abort)
                {
                    return;
                }
                foreach (FtpEntryInfo current_info in contents)
                {
                    delete_recursive(current_info, main_window);
                    if (abort)
                    {
                        return;
                    }
                }
                //and after this we can remove directory
                main_window.NotifyLongOperation
                        (string.Format
                        (Options.GetLiteral(Options.LANG_DELETE_NOW_0),
                        FtpPath.Combine(info.DirectoryPath, info.EntryName)),
                        true);
                try
                {
                    connection.DeleteDirectory(FtpPath.Combine(info.DirectoryPath, info.EntryName));
                }
                catch (Exception ex)
                {
                    abort = !process_error
                        (string.Format
                        (Options.GetLiteral(Options.LANG_CANNOT_DELETE_0),
                        FtpPath.Combine(info.DirectoryPath, info.EntryName)),
                        ex);
                }
            }
            else
            {
                //remove one file
                main_window.NotifyLongOperation
                        (string.Format
                        (Options.GetLiteral(Options.LANG_DELETE_NOW_0),
                        FtpPath.Combine(info.DirectoryPath, info.EntryName)),
                        true);
                try
                {
                    connection.DeleteFile(FtpPath.Combine(info.DirectoryPath, info.EntryName));
                }
                catch (Exception ex)
                {
                    abort = !process_error
                        (string.Format
                        (Options.GetLiteral(Options.LANG_CANNOT_DELETE_0),
                        FtpPath.Combine(info.DirectoryPath, info.EntryName)),
                        ex);
                }
            }
        }

        private bool process_error(string info, Exception ex)
        {
            if (supress_errors)
            {
                return true;
            }
            else
            {
                return Messages.ShowExceptionContinue(ex, info);
            }
        }
    }
}
