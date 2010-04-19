using System;
using System.Collections.Generic;
using System.Text;
using netCommander.NetApi;
using System.Runtime.InteropServices;
using System.ComponentModel;

namespace netCommander
{
    public class NetServerList : FileCollectionBase
    {
        private readonly string SORT_NAME = "Name";
        private SortedList<SERVER_INFO_101, object> internal_list;
        private IComparer<SERVER_INFO_101> internal_comparer;
        private string domain_name = null;

        public NetServerList(int sort_index, bool sort_reverse)
            : base(sort_index, sort_reverse)
        {
            internal_comparer = new InternalComparer(sort_index, sort_reverse);
            internal_list = new SortedList<SERVER_INFO_101, object>(internal_comparer);
        }

        public NetServerList(int sort_index, bool sort_reverse, string domain)
            : base(sort_index, sort_reverse)
        {
            internal_comparer = new InternalComparer(sort_index, sort_reverse);
            internal_list = new SortedList<SERVER_INFO_101, object>(internal_comparer);
            domain_name = domain;
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

        protected override void internal_dispose()
        {
            //notjing to do
        }

        protected override void internal_sort(int criteria_index, bool reverse_order)
        {
            IComparer<SERVER_INFO_101> new_comparer = new InternalComparer(criteria_index, reverse_order);
            SortedList<SERVER_INFO_101, object> new_list = new SortedList<SERVER_INFO_101, object>(new_comparer);

            foreach (KeyValuePair<SERVER_INFO_101, object> kvp in internal_list)
            {
                new_list.Add(kvp.Key, kvp.Value);
            }

            internal_list = new_list;
            internal_comparer = new_comparer;
        }

        protected override void internal_refill()
        {
            List<SERVER_INFO_101> new_list = null;
            if (domain_name == null)
            {
                new_list = new List<SERVER_INFO_101>
                    (WinApiNETwrapper.GetServerInfos_101(NetserverEnumType.ALL));
            }
            else
            {
                new_list = new List<SERVER_INFO_101>
                    (WinApiNETwrapper.GetServerInfos_101(domain_name, NetserverEnumType.ALL));
            }

            internal_list.Clear();
            foreach (SERVER_INFO_101 info in new_list)
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
            return internal_list.Keys[index].sv101_name;
        }

        public override string GetItemDisplayNameLong(int index)
        {
            return GetItemDisplayName(index);
        }

        public override string GetItemDisplaySummaryInfo(int index)
        {
            SERVER_INFO_101 info=internal_list.Keys[index];
            return string.Format
                ("{0}, version {1}.{2}",
                info.sv101_platform_id,
                info.GetVersionMajor(),
                info.sv101_version_minor);
        }

        public override string GetSummaryInfo()
        {
            return string.Format
                ("{0} entry(s)",
                ItemCount);
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

        public override void GetChildCollection(int index, ref FileCollectionBase new_collection, ref bool use_new, ref string preferred_focused_text)
        {
            throw new NotImplementedException();
        }

        public override int FindIndexOfName(string name)
        {
            int ret = -1;

            for (int i = 0; i < ItemCount; i++)
            {
                if (internal_list.Keys[i].sv101_name == name)
                {
                    ret = i;
                    break;
                }
            }
            return ret;
        }


        public override string GetStatusText()
        {
            if (domain_name == null)
            {
                return "Net servers";
            }
            else
            {
                return string.Format("Servers in [{0}]", domain_name);
            }
        }

        #region comparer
        private class InternalComparer : IComparer<SERVER_INFO_101>
        {
            private int _order = 1;
            private int sort_criteria = 0;

            public InternalComparer(int sort_criteria, bool sort_reverse)
            {
                this.sort_criteria = sort_criteria;
                _order = sort_reverse ? -1 : 1;
            }

            #region IComparer<SERVER_INFO_101> Members

            public int Compare(SERVER_INFO_101 x, SERVER_INFO_101 y)
            {
                int ret = 0;

                switch (sort_criteria)
                {
                    case 0:
                        ret = string.Compare(x.sv101_name, y.sv101_name);
                        break;

                    default:
                        ret = string.Compare(x.sv101_name, y.sv101_name);
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
