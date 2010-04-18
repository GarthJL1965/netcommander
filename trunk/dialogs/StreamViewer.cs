using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using netCommander.FileSystemEx;
using System.IO;
using System.Runtime.InteropServices;

namespace netCommander
{
    public partial class StreamViewer : UserControl
    {
        public StreamViewer()
        {
            InitializeComponent();
            set_lang();
        }

        private void set_lang()
        {
            if (LicenseManager.UsageMode == LicenseUsageMode.Designtime)
            {
                return;
            }

            columnHeaderAllocationSize.Text = Options.GetLiteral(Options.LANG_FILE_ALLOCATION_SIZE);
            columnHeaderName.Text = Options.GetLiteral(Options.LANG_FILE_NAME);
            columnHeaderSize.Text = Options.GetLiteral(Options.LANG_FILE_SIZE);
            buttonCopyTo.Text = Options.GetLiteral(Options.LANG_COPY_TO_FILE);
            buttonDelete.Text = Options.GetLiteral(Options.LANG_DELETE);
        }

        private void FillContents(IntPtr file_handle)
        {
            ntApiFSwrapper.FileStream_enum fs_enum = new ntApiFSwrapper.FileStream_enum(file_handle, IntPtr.Zero, 0);

            listViewStreams.Items.Clear();

            foreach (NT_FILE_STREAM_INFORMATION info in fs_enum)
            {
                InternalListViewItem new_item = new InternalListViewItem(info);
                listViewStreams.Items.Add(new_item);
            }

            if (listViewStreams.Items.Count == 0)
            {
                return;
            }

            listViewStreams.Items[0].Selected = true;
            buttonCopyTo.Enabled = true;
            buttonDelete.Enabled = true;
        }

        public void FillContents(string file_name)
        {
            //directory or file?

            FileName = file_name;
            IntPtr file_hanle=IntPtr.Zero;
            int win_err = 0;
            Win32Exception win_ex = null;

            try
            {

                if (IOhelper.IsDirectory(file_name))
                {
                    file_hanle = WinApiFS.CreateFile_intptr
                    (file_name,
                    Win32FileAccess.GENERIC_READ,
                    FileShare.ReadWrite,
                    IntPtr.Zero,
                    FileMode.Open,
                    CreateFileOptions.BACKUP_SEMANTICS,
                    IntPtr.Zero);
                }
                else
                {
                    file_hanle = WinApiFS.CreateFile_intptr
                        (file_name,
                        Win32FileAccess.GENERIC_READ,
                        FileShare.ReadWrite,
                        IntPtr.Zero,
                        FileMode.Open,
                        CreateFileOptions.None,
                        IntPtr.Zero);
                }

                if (file_hanle.ToInt32() == WinApiFS.INVALID_HANDLE_VALUE)
                {
                    win_err = Marshal.GetLastWin32Error();
                    win_ex = new Win32Exception(win_err);
                    return;
                }

                FillContents(file_hanle);
            }
            finally
            {
                if ((file_hanle != IntPtr.Zero) && (file_hanle.ToInt32() != WinApiFS.INVALID_HANDLE_VALUE))
                {
                    WinApiFS.CloseHandle(file_hanle);
                }

                if (win_ex != null)
                {
                    NT_FILE_STREAM_INFORMATION fake_info = new NT_FILE_STREAM_INFORMATION();
                    fake_info.StreamName = win_ex.Message;
                    listViewStreams.Items.Add(new InternalListViewItem(fake_info));
                }
            }
        }

        private byte[] copy_buffer = new byte[] { };
        private void internal_copy_to_file(NT_FILE_STREAM_INFORMATION fs_stream, string destination_file,bool fail_if_exists)
        {
            if (copy_buffer.Length == 0)
            {
                copy_buffer = new byte[1024 * 64];
            }


            string source_name = string.Format("{0}{1}", FileName, fs_stream.StreamName);


            IntPtr source_handle = IntPtr.Zero;
            IntPtr destination_handle = IntPtr.Zero;

            source_handle = WinApiFS.CreateFile_intptr
                (source_name,
                Win32FileAccess.GENERIC_READ,
                FileShare.Read,
                IntPtr.Zero,
                FileMode.Open,
                CreateFileOptions.None,
                IntPtr.Zero);
            if (source_handle.ToInt32() == WinApiFS.INVALID_HANDLE_VALUE)
            {
                throw new Win32Exception(Marshal.GetLastWin32Error());
            }

            destination_handle = WinApiFS.CreateFile_intptr
                (destination_file,
                Win32FileAccess.GENERIC_WRITE,
                FileShare.Read,
                IntPtr.Zero,
                fail_if_exists ? FileMode.CreateNew : FileMode.Create,
                CreateFileOptions.None,
                IntPtr.Zero);
            if (destination_handle.ToInt32() == WinApiFS.INVALID_HANDLE_VALUE)
            {
                WinApiFS.CloseHandle(source_handle);
                throw new Win32Exception(Marshal.GetLastWin32Error());
            }

            FileStream source_stream = null;
            FileStream destination_stream = null;
            try
            {
                source_stream = new FileStream(source_handle, FileAccess.Read, true);
                destination_stream = new FileStream(destination_handle, FileAccess.Write, true);
                int bytes_readed = 0;

                do
                {
                    bytes_readed = source_stream.Read(copy_buffer, 0, copy_buffer.Length);
                    destination_stream.Write(copy_buffer, 0, bytes_readed);
                } while (bytes_readed != 0);
            }
            finally
            {
                destination_stream.Close();
                source_stream.Close();
            }

        }

        private void internal_copy_selected_to()
        {
            


            if (listViewStreams.SelectedItems.Count == 0)
            {
                return;
            }

            NT_FILE_STREAM_INFORMATION fs_info = new NT_FILE_STREAM_INFORMATION();
            string destination = string.Empty;

            if (listViewStreams.SelectedItems.Count == 1)
            {
                //destination is file name

                saveFileDialog1.InitialDirectory = Path.GetDirectoryName(FileName);
                if (saveFileDialog1.ShowDialog() == DialogResult.OK)
                {
                    destination = saveFileDialog1.FileName;
                    fs_info = ((InternalListViewItem)listViewStreams.SelectedItems[0]).InternalData;
                    try
                    {
                        this.UseWaitCursor = true;
                        internal_copy_to_file(fs_info, destination, false);
                        this.UseWaitCursor = false;
                    }
                    catch (Exception ex)
                    {
                        this.UseWaitCursor = false;
                        Messages.ShowException(ex);
                    }
                }

                return;
            }
            //else

            folderBrowserDialog1.SelectedPath = Path.GetDirectoryName(FileName);
            if (folderBrowserDialog1.ShowDialog() != DialogResult.OK)
            {
                return;
            }

            destination = folderBrowserDialog1.SelectedPath;
            
            foreach (InternalListViewItem item in listViewStreams.SelectedItems)
            {
                UseWaitCursor = true;
                fs_info = item.InternalData;
                string destination_name = string.Empty;

                try
                {
                    destination_name = fs_info.StreamName;
                    destination_name = destination_name.Replace(':', '_');
                    destination_name = Path.Combine(destination, destination_name);
                    internal_copy_to_file(fs_info, destination_name, true);
                    UseWaitCursor = false;
                }
                catch (Exception ex)
                {
                    UseWaitCursor = false;
                    Messages.ShowException
                        (ex,
                        string.Format
                        (Options.GetLiteral(Options.LANG_CANNOT_COPY_0_ARROW_1),
                        fs_info.StreamName,
                        destination_name));

                }
            }
        }

        private void internal_delete_selected()
        {
            if (listViewStreams.SelectedItems.Count == 0)
            {
                return;
            }

            if (Messages.ShowQuestionYesNo
                (Options.GetLiteral(Options.LANG_DELETE_PROMPT),
                Options.GetLiteral(Options.LANG_DELETE)) != DialogResult.Yes)
            {
                return;
            }

            NT_FILE_STREAM_INFORMATION stream_info=new NT_FILE_STREAM_INFORMATION();
            foreach (InternalListViewItem item in listViewStreams.SelectedItems)
            {
                try
                {
                    stream_info = item.InternalData;
                    if (stream_info.StreamName.StartsWith("::$"))
                    {
                        Messages.ShowMessage(Options.GetLiteral(Options.LANG_CANNOT_DELETE_DEFAULT_DATA_STREAM));
                    }
                    else
                    {
                        string del_name = string.Format("{0}{1}", FileName, stream_info.StreamName);
                        int res = WinApiFS.DeleteFile(del_name);
                        if (res == 0)
                        {
                            throw new Win32Exception(Marshal.GetLastWin32Error());
                        }
                    }
                }
                catch (Exception ex)
                {
                    Messages.ShowException
                        (ex,
                        string.Format
                        (Options.GetLiteral(Options.LANG_CANNOT_DELETE_DEFAULT_DATA_STREAM),
                        stream_info.StreamName));
                }
            }

            //update list
            FillContents(FileName);
        }

        public string FileName { get; private set; }

        private class InternalListViewItem : ListViewItem
        {
            public NT_FILE_STREAM_INFORMATION InternalData { get; private set; }

            public InternalListViewItem(NT_FILE_STREAM_INFORMATION data)
            {
                InternalData = data;
                Text = data.StreamName;
                SubItems.Add(string.Format("{0:N0}", data.StreamSize));
                SubItems.Add(string.Format("{0:N0}", data.StreamAllocationSize));
            }
        }

        private void buttonCopyTo_Click(object sender, EventArgs e)
        {
            internal_copy_selected_to();
        }

        private void buttonDelete_Click(object sender, EventArgs e)
        {
            internal_delete_selected();
        }
    }
}
