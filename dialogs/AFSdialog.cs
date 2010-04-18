using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using netCommander.FileSystemEx;
using System.IO;

namespace netCommander
{
    public partial class AFSdialog : Form
    {
        public AFSdialog()
        {
            InitializeComponent();
        }

        public AFSdialog(string file_name)
        {
            InitializeComponent();

            internal_file_name = file_name;
            
            if (!check_volume(file_name))
            {
                Messages.ShowMessage("File system not support named streams.");
                return;
            }

            Text = string.Format("File streams [{0}]", file_name);

            fill_listview(file_name);
        }

        private string internal_file_name;

        private bool check_volume(string file_name)
        {
            string root_path = Path.GetPathRoot(file_name);
            VolumeInfo vi = new VolumeInfo(root_path);
            return ((vi.FileSystemFlags & VolumeCaps.NamedStreams) == VolumeCaps.NamedStreams);
        }

        private void fill_listview(string file_name)
        {
            List<FileStreamInfo> stream_list = new List<FileStreamInfo>();
            try
            {
                stream_list = WinAPiFSwrapper.GetFileStreamInfo_fromBackup(file_name, false);
            }
            catch (Exception ex)
            {
                Messages.ShowException(ex);
            }

            foreach (FileStreamInfo data in stream_list)
            {
                InternalListViewItem new_item = new InternalListViewItem(data);
                listViewResult.Items.Add(new_item);
            }

            if (listViewResult.Items.Count > 0)
            {
                listViewResult.Items[0].Selected = true;
            }
        }

        private class InternalListViewItem : ListViewItem
        {
            public FileStreamInfo InternalData { get; private set; }

            public InternalListViewItem(FileStreamInfo data)
            {
                InternalData = data;
                if (data.Name == string.Empty)
                {
                    Text = "<Default stream>";
                }
                else
                {
                    Text = data.Name;
                }
                SubItems.Add(data.Size.ToString("#,##0"));
                SubItems.Add(data.ID.ToString());
                SubItems.Add(data.Attributes.ToString());
            }
        }

        private void buttonCopyToFile_Click(object sender, EventArgs e)
        {
            copy_selected_to_file();
        }

        private void copy_selected_to_file()
        {
            if (listViewResult.SelectedItems.Count == 0)
            {
                return;
            }

            FileStreamInfo sel_info = ((InternalListViewItem)listViewResult.SelectedItems[0]).InternalData;

            string source = internal_file_name + sel_info.Name;

            saveFileDialog1.AddExtension = false;
            saveFileDialog1.AutoUpgradeEnabled = true;
            saveFileDialog1.CheckFileExists = false;
            saveFileDialog1.CheckFileExists = false;
            saveFileDialog1.OverwritePrompt = true;
            saveFileDialog1.ValidateNames = true;

            if (saveFileDialog1.ShowDialog() == DialogResult.OK)
            {
                string destinsation = saveFileDialog1.FileName;

                this.UseWaitCursor = true;

                try
                {
                    WinAPiFSwrapper.CopyFile
                        (source,
                        destinsation,
                        true,
                        null,
                        IntPtr.Zero,
                        131072);
                }
                catch (Exception ex)
                {
                    UseWaitCursor = false;
                    Messages.ShowException(ex);
                }

                UseWaitCursor = false;
            }

        }

        

        //private void buttonClose_Click(object sender, EventArgs e)
        //{
        //    Close();
        //}
    }
}
