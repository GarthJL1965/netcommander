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
            List<Wrapper.FileStreamInfo> stream_list = Wrapper.GetFileStreamInfo_fromBackup(file_name, true);

            foreach (Wrapper.FileStreamInfo data in stream_list)
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
            public Wrapper.FileStreamInfo InternalData { get; private set; }

            public InternalListViewItem(Wrapper.FileStreamInfo data)
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
    }
}
