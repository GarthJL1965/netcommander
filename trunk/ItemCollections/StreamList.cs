using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Runtime.InteropServices;
using netCommander.FileSystemEx;
using System.ComponentModel;

namespace netCommander
{
    public class StreamList : FileCollectionBase
    {
        private const string SORT_NAME = "Name";
        private const string SORT_SIZE = "Size";

        private SortedList<FileStreamInfo, object> internal_list;
        private string file_name_full;
        private IComparer<FileStreamInfo> internal_comparer;
        private bool read_sec_streams;
        private ulong total_size;
        private FileStreamInfo internal_parent_info = null;

        public StreamList(string file_name, bool read_security_streams, int sort_criteria, bool sort_reverse)
            : base(sort_criteria, sort_reverse)
        {
            read_sec_streams = read_security_streams;
            file_name_full = file_name;
            internal_comparer = new InternalComparer(sort_criteria, sort_reverse);
            internal_list = new SortedList<FileStreamInfo, object>(internal_comparer);

            //fake info
            internal_parent_info = new FileStreamInfo("..", 0, FileStreamID.DATA, FileStreamAttributes.NORMAL);
        }

        public override string GetCommandlineTextShort(int index)
        {
            string ret = string.Empty;
            return ret;
        }

        public override string GetCommandlineTextLong(int index)
        {
            string ret = string.Empty;
            return ret;
        }

        public string FileParent
        {
            get
            {
                return file_name_full;
            }
        }

        protected override void internal_dispose()
        {
            internal_list.Clear();
        }

        protected override void internal_sort(int criteria_index, bool reverse_order)
        {
            InternalComparer new_comparer = new InternalComparer(criteria_index, reverse_order);
            SortedList<FileStreamInfo, object> new_list = new SortedList<FileStreamInfo, object>(new_comparer);

            foreach (KeyValuePair<FileStreamInfo, object> kvp in internal_list)
            {
                new_list.Add(kvp.Key, kvp.Value);
            }

            internal_list = new_list;
            internal_comparer = new_comparer;
        }

        protected override void internal_refill()
        {
            //get list
            List<FileStreamInfo> stream_list = new List<FileStreamInfo>();
            try
            {
                stream_list = WinAPiFSwrapper.GetFileStreamInfo_fromBackup(file_name_full, read_sec_streams);
            }
            catch (Exception ex)
            {
                Messages.ShowException(ex);
            }
            internal_list.Clear();
            total_size = 0UL;
            internal_list.Add(internal_parent_info, null);
            foreach (FileStreamInfo info in stream_list)
            {
                internal_list.Add(info, null);
                total_size += info.Size;
            }
        }

        public override string[] SortCriteriaAvailable
        {
            get { return new string[] { SORT_NAME, SORT_SIZE }; }
        }

        public override string GetItemDisplayName(int index)
        {
            FileStreamInfo fsi = internal_list.Keys[index];

            if (fsi.Name == string.Empty)
            {
                return "<Default stream>";
            }
            return fsi.Name;
        }

        public override string GetItemDisplayNameLong(int index)
        {
            

            FileStreamInfo fsi = internal_list.Keys[index];

            if (fsi == internal_parent_info)
            {
                return file_name_full;
            }
            if (fsi.Name == string.Empty)
            {
                return file_name_full;
            }
            return fsi.Name;
        }

        public override string GetItemDisplaySummaryInfo(int index)
        {
            FileStreamInfo info=internal_list.Keys[index];
            return 
                string.Format
                ("{0} [{1}] [{2}]",
                IOhelper.SizeToString(info.Size),
                IOhelper.FileStreamID2String(info.ID),
                IOhelper.FileStreamAttributes2String(info.Attributes));
        }

        public override string GetSummaryInfo()
        {
            return
                string.Format
                ("Total: {0} stream(s), {1}",
                internal_list.Count-1,
                IOhelper.SizeToString(total_size));
        }

        public override string GetSummaryInfo(int[] indices)
        {
            ulong sel_size = 0UL;
            for (int i = 0; i < indices.Length; i++)
            {
                sel_size += internal_list.Keys[indices[i]].Size;
            }
            return
                string.Format
                ("Selected {0} stream(s), {1}",
                indices.Length,
                IOhelper.SizeToString(sel_size));
        }

        public override int ItemCount
        {
            get { return internal_list.Count; }
        }

        public override bool GetItemSelectEnable(int index)
        {
            return (internal_list.Keys[index] != internal_parent_info);
        }

        public override bool GetItemIsContainer(int index)
        {
            return (internal_list.Keys[index] == internal_parent_info);
        }

        public override void GetChildCollection(int index, ref FileCollectionBase new_collection, ref bool use_new, ref string preferred_focused_text)
        {
            if (!GetItemIsContainer(index))
            {
                use_new = false;
                return;
            }

            //returns DirectoryList of parent folder
            string target_dir = Path.GetDirectoryName(file_name_full);
            
            new_collection = new DirectoryList(0, false, target_dir);
            bool err = false;

            try
            {
                new_collection.Refill();
            }
            catch (Exception ex)
            {
                err = true;
                Messages.ShowException(ex);
            }

            if (err)
            {
                use_new = false;
            }
            else
            {
                use_new = true;
                preferred_focused_text = Path.GetFileName(file_name_full);
            }
        }

        public override int FindIndexOfName(string name)
        {
            int ret = -1;

            for (int i = 0; i < internal_list.Count; i++)
            {
                if (internal_list.Keys[i].Name == name)
                {
                    ret = i;
                    break;
                }
            }

            return ret;
        }

        public override int[] FindItems()
        {
            return new int[] { };
        }

        public override string GetStatusText()
        {
            return
                string.Format
                ("Streams of {0}",
                file_name_full);
        }

        private class InternalComparer : IComparer<FileStreamInfo>
        {
            private int sort_criteria = 0;
            private int sort_order = 1;

            public InternalComparer(int criteria, bool reverse)
            {
                sort_criteria = criteria;
                sort_order = reverse ? -1 : 1;
            }

            #region IComparer<FileStreamInfo> Members

            public int Compare(FileStreamInfo x, FileStreamInfo y)
            {
                //place parent info first
                if (x.Name == "..")
                {
                    return -1;
                }
                if (y.Name == "..")
                {
                    return 1;
                }

                switch (sort_criteria)
                {
                    case 0: //Name
                        return string.Compare(x.Name, y.Name) * sort_order;

                    case 1: //Size
                        long delta_size = (long)x.Size - (long)y.Size;
                        if (delta_size > 0L)
                        {
                            return sort_order;
                        }
                        else if (delta_size < 0L)
                        {
                            return -sort_order;
                        }
                        else
                        {
                            return string.Compare(x.Name, y.Name) * sort_order;
                        }

                    default:
                        return string.Compare(x.Name, y.Name) * sort_order;
                }
            }

            #endregion
        }
    }
}
