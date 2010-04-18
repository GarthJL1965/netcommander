using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using netCommander.FileSystemEx;
using Dolinay;

namespace netCommander
{
    public class DriveList : FileCollectionBase
    {
        public readonly string SORT_NAME = "Name";
        public readonly string SORT_FREE = "Available free space";

        private SortedList<VolumeInfo, object> internal_list;
        private IComparer<VolumeInfo> comparer;
        private StringBuilder string_builder = new StringBuilder();
        private DriveDetector drive_notifier = null;

        public DriveList(int sort_criteria, bool reverse_order)
            : base(sort_criteria, reverse_order)
        {
            comparer = new InternalComparer(sort_criteria, reverse_order);
            internal_list = new SortedList<VolumeInfo, object>(comparer);

            drive_notifier = new DriveDetector();
            drive_notifier.DeviceArrived += new DriveDetectorEventHandler(drive_notifier_DeviceArrived);
            drive_notifier.DeviceRemoved += new DriveDetectorEventHandler(drive_notifier_DeviceRemoved);
        }

        void drive_notifier_DeviceRemoved(object sender, DriveDetectorEventArgs e)
        {
            //remove drive

            //find its index
            int rem_ind = -1;
            for (int i = 0; i < internal_list.Count; i++)
            {
                if (internal_list.Keys[i].RootPathName == e.Drive)
                {
                    rem_ind = i;
                    break;
                }
            }

            if (rem_ind != -1)
            {
                //do we need remove drive from list?
                DriveInfo[] dis_temp = DriveInfo.GetDrives();
                bool need_remove = (dis_temp.Length != internal_list.Count);
                if (need_remove)
                {
                    internal_list.RemoveAt(rem_ind);
                    OnItemRemove(rem_ind);
                }
                else
                {
                    internal_list.Keys[rem_ind].Update();
                    OnItemUpdate(rem_ind);
                }
            }
        }

        void drive_notifier_DeviceArrived(object sender, DriveDetectorEventArgs e)
        {
            //if that drive already on collection (i.e. CDrom)
            int drive_ind = -1;
            for (int i = 0; i < internal_list.Count; i++)
            {
                if (internal_list.Keys[i].RootPathName == e.Drive)
                {
                    //found!
                    drive_ind = i;
                    break;
                }
            }

            if (drive_ind == -1)
            {
                //there new drive
                //try to create DriveInfo
                VolumeInfo di = new VolumeInfo(e.Drive);

                //insert into internal list
                internal_list.Add(di, null);

                int new_ind = internal_list.IndexOfKey(di);

                //and throw event
                OnItemInsert(new_ind);
            }
            else
            {
                //drive already in list, update it
                internal_list.Keys[drive_ind].Update();
                OnItemUpdate(drive_ind);
            }
        }

        protected override void internal_dispose()
        {
            drive_notifier.DeviceArrived -= drive_notifier_DeviceArrived;
            //drive_notifier.DeviceArrived
            drive_notifier.DeviceRemoved -= drive_notifier_DeviceRemoved;
            //drive_notifier.DeviceRemoved = null;
            drive_notifier.Dispose();
            drive_notifier = null;
            internal_list.Clear();
        }

        protected override void internal_sort(int criteria_index, bool reverse_order)
        {
            IComparer<VolumeInfo> new_comparer = new InternalComparer(criteria_index, reverse_order);
            SortedList<VolumeInfo, object> new_list = new SortedList<VolumeInfo, object>(new_comparer);

            foreach (KeyValuePair<VolumeInfo, object> kvp in internal_list)
            {
                new_list.Add(kvp.Key, null);
            }

            internal_list = new_list;
            comparer = new_comparer;
        }

        protected override void internal_refill()
        {
            internal_list.Clear();

            string[] drive_letters = WinAPiFSwrapper.GetLogicalDrives();
            foreach (string one_letter in drive_letters)
            {
                VolumeInfo vi = new VolumeInfo(one_letter,true);
                internal_list.Add(vi, null);
            }
        }

        public override string GetStatusText()
        {
            return "Drives list";
        }

        public override int FindIndexOfName(string name)
        {
            int ret = -1;
            for (int i = 0; i < internal_list.Count; i++)
            {
                if (internal_list.Keys[i].RootPathName == name)
                {
                    ret = i;
                    break;
                }
            }
            return ret;
        }

        public override void GetChildCollection(int index, ref FileCollectionBase new_collection, ref bool use_new, ref string preferred_focus)
        {
            internal_list.Keys[index].Update();

            if (!internal_list.Keys[index].DeviceReady)
            {
                Messages.ShowMessage(string.Format("Device {0} not ready.", internal_list.Keys[index].RootPathName));
                use_new = false;
                return;
            }

            
            preferred_focus = string.Empty;
            new_collection = new DirectoryList(0, false, internal_list.Keys[index].RootPathName);
            try
            {
                new_collection.Refill();
                new_collection.MainWindow = this.MainWindow;
                use_new = true;
            }
            catch (Exception ex)
            {
                Messages.ShowException(ex);
            }
        }

        public override string[] SortCriteriaAvailable
        {
            get { return new string[] { SORT_NAME, SORT_FREE }; }
        }

        public override string GetItemDisplayName(int index)
        {
            string_builder.Remove(0, string_builder.Length);

            string_builder.Append(internal_list.Keys[index].RootPathName);

            if (internal_list.Keys[index].DeviceReady)
            {
                string_builder.Append(" ");
                string_builder.Append(internal_list.Keys[index].VolumeName);
            }

            return string_builder.ToString();
        }

        public override string GetItemDisplayNameLong(int index)
        {
            string_builder.Remove(0, string_builder.Length);

            string_builder.Append(internal_list.Keys[index].RootPathName);
            string_builder.Append(" [");
            string_builder.Append(internal_list.Keys[index].DriveType.ToString());
            string_builder.Append("]");

                if (internal_list.Keys[index].DeviceReady)
                {
                    string_builder.Append(" [");
                    string_builder.Append(internal_list.Keys[index].VolumeName);
                    string_builder.Append("] [");
                    string_builder.Append(internal_list.Keys[index].FileSystemName);
                    string_builder.Append("]");
                }
                else
                {
                    //string_builder.Append(" [Device not ready]");
                }
            return string_builder.ToString();
        }

        public override string GetItemDisplaySummaryInfo(int index)
        {
            string_builder.Remove(0, string_builder.Length);

            if (internal_list.Keys[index].DeviceReady)
            {
                string_builder.Append(string.Format("[Total: {0}] [Available: {1}]", IOhelper.SizeToString(internal_list.Keys[index].VolumeSpaceInfo.TotalNumberOfBytes),IOhelper.SizeToString(internal_list.Keys[index].VolumeSpaceInfo.FreeBytesAvailable)));
            }

            return string_builder.ToString();
        }

        public override string GetSummaryInfo()
        {
            return string.Format("{0} {1}", Environment.MachineName, Environment.OSVersion.ToString());
        }

        public override string GetSummaryInfo(int[] indices)
        {
            return GetSummaryInfo();
        }

        public override int ItemCount
        {
            get { return internal_list.Count; }
        }

        public override bool GetItemSelectEnable(int index)
        {
            return false;
        }

        public override bool GetItemIsContainer(int index)
        {
            return true;
        }

        private class InternalComparer : IComparer<VolumeInfo>
        {
            public InternalComparer(int criteria_index,bool reverse)
            {
                if ((criteria_index < 0) | (criteria_index > 1))
                {
                    throw new ApplicationException("There no such sorting.");
                }

                criteria = criteria_index;

                if (reverse)
                {
                    _order = -1;
                }
            }

            private int criteria = 0;
            private int _order = 1;

            #region IComparer<VOlumeInfo> Members

            public int Compare(VolumeInfo x, VolumeInfo y)
            {
                switch (criteria)
                {
                    case 0:
                        //NAME
                        return string.Compare(x.RootPathName, y.RootPathName) * _order;

                    case 1:
                        //available free size
                        ulong _x = 0;
                        ulong _y = 0;
                        if (x.DeviceReady)
                        {
                            _x = x.VolumeSpaceInfo.FreeBytesAvailable;
                        }
                        if (y.DeviceReady)
                        {
                            _y = y.VolumeSpaceInfo.FreeBytesAvailable;
                        }
                        if (_x == _y)
                        {
                            return 0;
                        }
                        else
                        {
                            return ((_x > _y) ? 1 : -1) * _order;
                        }
                }

                return 0;
            }

            #endregion
        }







        public override int[] FindItems()
        {
            return new int[] { };
        }
    }
}
