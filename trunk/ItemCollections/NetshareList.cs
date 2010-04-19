using System;
using System.Collections.Generic;
using System.Text;
using netCommander.NetApi;
using System.Runtime.InteropServices;
using System.ComponentModel;
using netCommander.WNet;

namespace netCommander
{
    public class NetshareList : FileCollectionBase
    {
        private readonly string SORT_NAME = "Name";
        private SortedList<SHARE_INFO_1, object> internal_list;
        private IComparer<SHARE_INFO_1> internal_comparer;
        private NETRESOURCE server_ref = new NETRESOURCE();
        private string server_name = string.Empty;

        public NetshareList(int sort_criteria, bool sort_reverse, string server_name)
            :base(sort_criteria,sort_reverse)
        {
            this.server_name = server_name;
            server_ref.lpRemoteName = string.Format(@"\\{0}", server_name);
            internal_init();
        }

        public NetshareList(int sort_criteria, bool sort_reverse, NETRESOURCE server)
            : base(sort_criteria, sort_reverse)
        {
            server_ref = server;
            server_name = server.lpRemoteName.TrimStart(new char[] { '\\' });
            internal_init();
        }

        private void internal_init()
        {
            internal_comparer = new InternalComparer(SortCriteria, SortReverse);
            internal_list = new SortedList<SHARE_INFO_1, object>(internal_comparer);
        }

        protected override void internal_dispose()
        {
            //nothing to do
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

        protected override void internal_sort(int criteria_index, bool reverse_order)
        {
            IComparer<SHARE_INFO_1> new_comparer = new InternalComparer(criteria_index, reverse_order);
            SortedList<SHARE_INFO_1, object> new_list = new SortedList<SHARE_INFO_1, object>(new_comparer);

            foreach (KeyValuePair<SHARE_INFO_1, object> kvp in internal_list)
            {
                new_list.Add(kvp.Key, kvp.Value);
            }

            internal_comparer = new_comparer;
            internal_list = new_list;
        }

        protected override void internal_refill()
        {
            SHARE_INFO_1[] infos = WinApiNETwrapper.GetShareInfos_1(server_name);
            internal_list.Clear();
            foreach (SHARE_INFO_1 info in infos)
            {
                internal_list.Add(info, null);
            }
        }

        public override string[] SortCriteriaAvailable
        {
            get { return new string[] { SORT_NAME }; }
        }

        public override string GetItemDisplayName(int index)
        {
            string ret = string.Empty;
            if (index == 0)
            {
                //ref to server
                ret = "..";
            }
            else
            {
                ret = internal_list.Keys[index - 1].shi1_netname;
            }
            return ret;
        }

        public override string GetItemDisplayNameLong(int index)
        {
            string ret = string.Empty;
            if (index == 0)
            {
                ret = "<UP>";
            }
            else
            {
                ret = GetItemDisplayName(index);
            }
            return ret;
        }

        public override string GetItemDisplaySummaryInfo(int index)
        {
            string ret = string.Empty;
            if (index != 0)
            {
                SHARE_INFO_1 info=internal_list.Keys[index-1];
                ret = string.Format
                    ("{0} {1}",
                    info.shi1_type.ToString(),
                    info.shi1_remark);
            }
            return ret;
        }

        public override string GetSummaryInfo()
        {
            return string.Format
                ("{0} entry(s) [{1}]",
                internal_list.Count,
                server_ref.lpRemoteName);
        }

        public override string GetSummaryInfo(int[] indices)
        {
            return GetSummaryInfo();
        }

        public override int ItemCount
        {
            get { return internal_list.Count + 1; }
        }

        public override bool GetItemSelectEnable(int index)
        {
            return false;
        }

        public override bool GetItemIsContainer(int index)
        {
            if (index == 0)
            {
                return true;
            }
            else
            {
                SHARE_INFO_1 info= internal_list.Keys[index - 1];
                return (info.shi1_type == NetshareType.DISKTREE) || (info.shi1_type == NetshareType.SPECIAL);
            }
        }

        public override void GetChildCollection(int index, ref FileCollectionBase new_collection, ref bool use_new, ref string preferred_focused_text)
        {
            if (index == 0)
            {
                //return container from wnet
                try
                {
                    NETRESOURCE container = WinApiWNETwrapper.GetParentResource(server_ref);
                    WnetResourceList wnet_list = new WnetResourceList(SortCriteria, SortReverse, container);
                    preferred_focused_text = server_ref.lpRemoteName;
                    wnet_list.Refill();
                    use_new = true;
                    new_collection = wnet_list;
                }
                catch (Exception ex)
                {
                    Messages.ShowException(ex);
                }
                return;
            }

            //else return DirectoryList
            try
            {
                string target_dir = string.Format
                    ("{0}\\{1}",
                    server_ref.lpRemoteName,
                    internal_list.Keys[index - 1].shi1_netname);
                DirectoryList dir_list = new DirectoryList(SortCriteria, SortReverse, target_dir);
                dir_list.Refill();
                use_new = true;
                new_collection = dir_list;
            }
            catch (Exception ex)
            {
                Messages.ShowException(ex);
            }
            return;
        }

        public override int FindIndexOfName(string name)
        {
            int ret = -1;
            for (int i = 0; i < internal_list.Count; i++)
            {
                if (internal_list.Keys[i].shi1_netname == name)
                {
                    ret = i + 1;
                    break;
                }
            }
            return ret;
        }

        public override string GetStatusText()
        {
            return string.Format
                ("Shares on {0}",
                server_ref.lpRemoteName);
        }

        #region comparer
        private class InternalComparer : IComparer<SHARE_INFO_1>
        {
            private int _order = 1;
            private int sort_criteria = 0;

            public InternalComparer(int sort_criteria, bool sort_reverse)
            {
                this.sort_criteria = sort_criteria;
                _order = sort_reverse ? -1 : 1;
            }

            #region IComparer<SHARE_INFO_1> Members

            public int Compare(SHARE_INFO_1 x, SHARE_INFO_1 y)
            {
                int ret = 0;

                switch (sort_criteria)
                {
                    case 0:
                        ret = string.Compare(x.shi1_netname, y.shi1_netname);
                        break;

                    default:
                        ret = string.Compare(x.shi1_netname, y.shi1_netname);
                        break;
                }

                ret = _order * ret;

                return ret;
            }

            #endregion
        }
        #endregion
    }
}
