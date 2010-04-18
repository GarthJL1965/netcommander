using System;
using System.Collections.Generic;
using netCommander.WNet;
using netCommander.NetApi;
using System.ComponentModel;

namespace netCommander
{
    public class WnetResourceList : FileCollectionBase
    {
        private readonly string SORT_NAME = "Name";

        private SortedList<NETRESOURCE, object> internal_list = null;
        private IComparer<NETRESOURCE> internal_comparer = null;
        private NETRESOURCE resource_root = new NETRESOURCE();
        private InternalCache parent_cache = null;

        //private PanelCommandAddWnetConnection command_add_connection = new PanelCommandAddWnetConnection();
        private PanelCommandServerInfo command_server_info = new PanelCommandServerInfo();

        public WnetResourceList(int sort_criteria, bool reverse)
            : base(sort_criteria, reverse)
        {
            //root resouces
            resource_root = WinApiWNETwrapper.RootNetresource;
            internal_comparer = new InternalComparer(sort_criteria, reverse);
            internal_list = new SortedList<NETRESOURCE, object>(internal_comparer);
            init_commands();
        }

        public WnetResourceList(int sort_criteria, bool reverse, NETRESOURCE root)
            : base(sort_criteria, reverse)
        {
            resource_root = root;
            internal_comparer = new InternalComparer(sort_criteria, reverse);
            internal_list = new SortedList<NETRESOURCE, object>(internal_comparer);
            init_commands();
        }

        /// <summary>
        /// Use try with this ctor.
        /// </summary>
        /// <param name="sort_criteria"></param>
        /// <param name="reverse"></param>
        /// <param name="net_path"></param>
        public WnetResourceList(int sort_criteria, bool reverse, string net_path)
            : base(sort_criteria, reverse)
        {
            resource_root = WinApiWNETwrapper.GetResourceInfo(net_path);
            internal_comparer = new InternalComparer(sort_criteria, reverse);
            internal_list = new SortedList<NETRESOURCE, object>(internal_comparer);
            init_commands();
        }

        private void init_commands()
        {
            //AvailableCommands.Add(command_add_connection);

            AvailableCommands.Add(command_server_info);
        }

        public NETRESOURCE this[int index]
        {
            get
            {
                return index == 0 ? resource_root : internal_list.Keys[index - 1];
            }
        }

        public override ItemCategory GetItemCategory(int index)
        {
            if (index < 0)
            {
                return ItemCategory.Default;
            }
            return ItemCategory.Container;
        }

        public override string GetCommandlineTextShort(int index)
        {
            string ret = string.Empty;
            NETRESOURCE info = this[index];
            if (GetItemDisplayName(index)=="..")
            {
                return ret;
            }
            else
            {
                return info.lpRemoteName;
            }
        }

        public override string GetCommandlineTextLong(int index)
        {
            string ret = string.Empty;
            NETRESOURCE info = this[index];
            if (GetItemDisplayName(index) == "..")
            {
                return ret;
            }
            else
            {
                return info.lpRemoteName;
            }
        }

        private bool establish_connection(NETRESOURCE target_resource)
        {
            //ask creds
            string user_name = string.Empty;
            string ps = string.Empty;

            if (Messages.AskCredentials
                (Options.GetLiteral(Options.LANG_NETWORK_LOGIN),
                "Connect to " + target_resource.lpRemoteName,
                ref user_name,
                ref ps) != System.Windows.Forms.DialogResult.OK)
            {
                return false;
            }

            int res = WinApiWNET.WNetAddConnection2
                (ref target_resource,
                ps,
                user_name,
                WNetConnectOptions.None);
            if (res != WinApiWNET.NO_ERROR)
            {
                Win32Exception win_ex = new Win32Exception(res);
                Messages.ShowException(win_ex);
                return false;
            }
            return true;
        }

        private NETRESOURCE[] get_hidden_resources(NETRESOURCE res)
        {
            List<NETRESOURCE> ret = new List<NETRESOURCE>();

            try
            {
                string server_name = res.lpRemoteName.TrimStart(new char[] { '\\' });
                SHARE_INFO_1[] net_shares = WinApiNETwrapper.GetShareInfos_1(server_name);
                foreach (SHARE_INFO_1 info in net_shares)
                {
                    if (info.shi1_netname.EndsWith("$"))
                    {
                        NETRESOURCE hid_res = new NETRESOURCE();

                        if ((info.shi1_type & NetshareType.DEVICE) == NetshareType.DEVICE)
                        {
                            //IPC$
                            hid_res.dwDisplayType = ResourceDisplayType.GENERIC;
                            hid_res.dwType = ResourceType.ANY;
                        }
                        else if ((info.shi1_type & NetshareType.PRINTQ) == NetshareType.PRINTQ)
                        {
                            hid_res.dwDisplayType = ResourceDisplayType.SHAREADMIN;
                            hid_res.dwType = ResourceType.PRINT;
                        }
                        else if ((info.shi1_type & NetshareType.SPECIAL) == NetshareType.SPECIAL)
                        {
                            //C$
                            hid_res.dwDisplayType = ResourceDisplayType.SHAREADMIN;
                            hid_res.dwType = ResourceType.DISK;
                        }

                        hid_res.lpComment = info.shi1_remark;
                        hid_res.lpProvider = res.lpProvider;
                        hid_res.lpRemoteName = string.Format(@"{0}\{1}", res.lpRemoteName, info.shi1_netname);
                        ret.Add(hid_res);
                    }
                }

            }
            catch (Exception) { }
            return ret.ToArray();
        }

        protected override void internal_dispose()
        {
            //nothing to do
        }

        protected override void internal_sort(int criteria_index, bool reverse_order)
        {
            InternalComparer new_comparer = new InternalComparer(criteria_index, reverse_order);
            SortedList<NETRESOURCE, object> new_list = new SortedList<NETRESOURCE, object>(new_comparer);

            foreach (KeyValuePair<NETRESOURCE, object> kvp in internal_list)
            {
                new_list.Add(kvp.Key, kvp.Value);
            }

            internal_comparer = new_comparer;
            internal_list = new_list;
        }

        protected override void internal_refill()
        {
            List<NETRESOURCE> new_list = new List<NETRESOURCE>();

            //create enumerator
            WnetResourceEnumerator res_enum = null;

            try
            {
                OnLongOperaion(Options.GetLiteral(Options.LANG_NETWORK_WAIT_WHILE_SCANNING), true);
                res_enum = new WnetResourceEnumerator
                (resource_root,
                ResourceScope.GLOBALNET,
                ResourceType.ANY,
                ResourceUsage.NONE);
                OnLongOperaion(string.Empty, false);
            }
            catch (Win32Exception win_ex)
            {
                if (win_ex.NativeErrorCode == 5) //access denied
                {
                    //try to establish connection with net credetials
                    if (establish_connection(resource_root))
                    {
                        //retry
                        res_enum = new WnetResourceEnumerator
                                        (resource_root,
                                        ResourceScope.GLOBALNET,
                                        ResourceType.ANY,
                                        ResourceUsage.NONE);
                    }
                    else
                    {
                        throw win_ex;
                    }
                }
                else
                {
                    throw win_ex;
                }
            }
            finally
            {
                OnLongOperaion(string.Empty, false);
            }

            //fill
            foreach (NETRESOURCE one_res in res_enum)
            {
                new_list.Add(one_res);
            }

            //case if resourse_root is SMB server - add hidden shares
            if ((resource_root.dwDisplayType == ResourceDisplayType.SERVER) &&
                (WinApiWNETwrapper.GetNetworkInfo(resource_root.lpProvider).wNetType == NetworkType.LANMAN))
            {
                //retrieve fake hidden NETRESOURCEs
                OnLongOperaion(Options.GetLiteral(Options.LANG_NETWORK_WAIT_WHILE_SCANNING), true);
                NETRESOURCE[] hidden_resources = get_hidden_resources(resource_root);
                OnLongOperaion(string.Empty, false);
                new_list.AddRange(hidden_resources);
            }

            //fill work list
            internal_list.Clear();
            foreach (NETRESOURCE one_res in new_list)
            {
                internal_list.Add(one_res, null);
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
                ret = ".."; //ref to root
            }
            else
            {
                ret = string.Format
                ("{0}",
                internal_list.Keys[index - 1].lpRemoteName);
            }

            return ret;
        }

        public override string GetItemDisplayNameLong(int index)
        {
            string ret = string.Empty;

            if (index == 0)
            {
                ret = Options.GetLiteral(Options.LANG_UP);
            }
            else
            {
                ret = GetItemDisplayName(index);
            }
            return ret;
        }

        public override string GetItemDisplaySummaryInfo(int index)
        {
            NETRESOURCE res = new NETRESOURCE();
            string ret = string.Empty;

            res = index == 0 ? resource_root : internal_list.Keys[index - 1];
            
            ret = string.Format
            ("{0} {1}",
            res.dwDisplayType.ToString(),
            res.lpComment);

            return ret;
        }

        public override string GetSummaryInfo()
        {
            return string.Format
                ("{0} "+Options.GetLiteral(Options.LANG_ENTRIES)+" [{1}]",
                ItemCount - 1,
                resource_root.lpRemoteName);
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
                return
                    ((internal_list.Keys[index - 1].dwUsage & ResourceUsage.CONTAINER) ==
                    ResourceUsage.CONTAINER) ||
                    (internal_list.Keys[index - 1].dwType == ResourceType.DISK);
            }
        }

        public override void GetChildCollection
            (int index,
            ref FileCollectionBase new_collection,
            ref bool use_new,
            ref string preferred_focused_text)
        {
            if (!GetItemIsContainer(index))
            {
                use_new = false;
                return;
            }

            NETRESOURCE target_container = new NETRESOURCE();

            if (index == 0)
            {
                //go up to container of resourse_root

                //cache valid?
                if (parent_cache != null)
                {
                    //fill from cache
                    internal_list.Clear();
                    foreach (NETRESOURCE res in parent_cache.ResourceList)
                    {
                        internal_list.Add(res, null);
                    }
                    preferred_focused_text = resource_root.lpRemoteName;
                    resource_root = parent_cache.ResourceRoot;
                    parent_cache = parent_cache.ParentCache;
                    OnAfterRefill();
                    use_new = false;
                    return;
                }

                //workaround. 
                //Wnet throws exception while getting parent of resource like 'Microsoft Windows Network'
                if ((resource_root.lpRemoteName == null) ||
                    (resource_root.dwDisplayType == ResourceDisplayType.NETWORK))
                {
                    target_container = WinApiWNETwrapper.RootNetresource;
                }
                else
                {
                    target_container = WinApiWNETwrapper.GetParentResource(resource_root);
                }

                preferred_focused_text = resource_root.lpRemoteName;

                //set new resource_root
                resource_root = target_container;

                //and fill
                Refill();

                //and return
                use_new = false;
                return;
            }//end of UP brunch

            //else go to down
            target_container = internal_list.Keys[index - 1];

            //case disk share - switch to DirectoryList
            if (target_container.dwType == ResourceType.DISK)
            {
                //switch to DirectoryList
                DirectoryList new_source = new DirectoryList
                (SortCriteria, SortReverse, target_container.lpRemoteName);
                new_source.MainWindow = MainWindow;

                try
                {
                    new_source.Refill();
                    use_new = true;
                    new_collection = new_source;
                }
                catch (Win32Exception win5_ex)
                {
                    if (win5_ex.NativeErrorCode == 5) //access denied
                    {
                        //try to establish connection with creds
                        if (establish_connection(target_container))
                        {
                            try
                            {
                                //and retry
                                new_source.Refill();
                                use_new = true;
                                new_collection = new_source;
                            }
                            catch (Exception ex)
                            {
                                Messages.ShowException(ex);
                            }
                        }
                    }
                    else
                    {
                        Messages.ShowException(win5_ex);
                    }
                }
                catch (Exception ex)
                {
                    Messages.ShowException(ex);
                }
                return;
            }

            //prepare parent cache
            InternalCache new_cache = new InternalCache(target_container);
            new_cache.ParentCache = parent_cache;
            new_cache.ResourceList.AddRange(internal_list.Keys);
            new_cache.ResourceRoot = resource_root;

            try
            {
                resource_root = target_container;
                use_new = false;
                Refill();
                parent_cache = new_cache;
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
                if (internal_list.Keys[i].lpRemoteName == name)
                {
                    ret = i + 1;
                    break;
                }
            }
            return ret;
        }

        public override int[] FindItems()
        {
            throw new NotImplementedException();
        }

        public override string GetStatusText()
        {
            return resource_root.lpRemoteName == null ? "Network" : resource_root.lpRemoteName;
        }

        #region cache
        private class InternalCache
        {
            public List<NETRESOURCE> ResourceList { get; set; }
            public InternalCache ParentCache { get; set; }
            public NETRESOURCE  ResourceRoot { get; set; }

            public InternalCache(NETRESOURCE resource_root)
            {
                ResourceList = new List<NETRESOURCE>();
                ParentCache = null;
                ResourceRoot = resource_root;
            }
        }
        #endregion

        #region comparer
        private class InternalComparer : IComparer<NETRESOURCE>
        {
            private int _order = 1;
            private int sort_criteria = 0;

            public InternalComparer(int sort_criteria, bool sort_reverse)
            {
                this.sort_criteria = sort_criteria;
                _order = sort_reverse ? -1 : 1;
            }

            #region IComparer<NETRESOURCE> Members

            public int Compare(NETRESOURCE x, NETRESOURCE y)
            {
                int ret = 0;

                switch (sort_criteria)
                {
                    case 0:
                        ret = string.Compare(x.lpRemoteName, y.lpRemoteName);
                        break;

                    default:
                        ret = string.Compare(x.lpRemoteName, y.lpRemoteName);
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
