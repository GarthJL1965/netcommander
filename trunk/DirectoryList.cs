using System;
using System.Collections.Generic;
using System.Text;
using netCommander.FileSystemEx;
using System.IO;

namespace netCommander
{
    public class DirectoryList : FileCollectionBase
    {
        private readonly string SORT_NAME = "Name";
        private readonly string SORT_SIZE = "Size";
        private readonly string SORT_DATE_CREATE = "Creation time";
        private readonly string SORT_DATE_ACCESS = "Access time";
        private readonly string SORT_DATE_MODIFICATION = "Modification time";
        private readonly string SORT_EXTENSION = "Extension";

        private SortedList<WIN32_FIND_DATA, object> internal_list;
        private string directory_path;
        private IComparer<WIN32_FIND_DATA> internal_comparer;

        private int cache_directory_count = 0;
        private int cache_files_count = 0;
        private ulong cache_size = 0UL;
        private int cache_directory_selected_count = 0;
        private int cache_files_selected_count = 0;
        private ulong cache_size_selected = 0UL;
        private int[] cache_selected_indices = new int[] { };

        private FileSystemWatcher internal_watcher = new FileSystemWatcher();
        private PanelCommandShowAFS command_show_afs = new PanelCommandShowAFS();
        private PanelCommandTouchFile command_touch = new PanelCommandTouchFile();
        private PanelCommandFileAttributesEdit command_attributes = new PanelCommandFileAttributesEdit();
        private PanelComandDeleteFile command_delete = new PanelComandDeleteFile();
        private PanelCommandCopyFile command_copy = new PanelCommandCopyFile();
        private PanelCommandMoveFile command_move = new PanelCommandMoveFile();
        private PanelComandCreateDirectory command_create_dir = new PanelComandCreateDirectory();
        //private PanelCommandBrowseFileStreams command_browse_streams = new PanelCommandBrowseFileStreams();
        private PanelCommandFileInfo command_fileinfo = new PanelCommandFileInfo();
        //private PanelCommandFileSystemSecurity command_security = new PanelCommandFileSystemSecurity();

        public DirectoryList(int sort_index, bool sort_reverse, string directory_path)
            : base(sort_index, sort_reverse)
        {
            this.directory_path = directory_path;
            internal_comparer = new InternalComparer(sort_index, sort_reverse);
            internal_list = new SortedList<WIN32_FIND_DATA, object>(internal_comparer);

            //set notification
            internal_watcher.IncludeSubdirectories = false;
            internal_watcher.NotifyFilter =
                NotifyFilters.Attributes |
                NotifyFilters.DirectoryName |
                NotifyFilters.FileName |
                NotifyFilters.Size;
            internal_watcher.Changed += new FileSystemEventHandler(internal_watcher_Changed);
            internal_watcher.Renamed += new RenamedEventHandler(internal_watcher_Renamed);
            internal_watcher.Created += new FileSystemEventHandler(internal_watcher_Created);
            internal_watcher.Deleted += new FileSystemEventHandler(internal_watcher_Deleted);
            internal_watcher.Error += new ErrorEventHandler(internal_watcher_Error);

            //add panel commands
            AvailableCommands.Add(command_show_afs);
            AvailableCommands.Add(command_touch);
            AvailableCommands.Add(command_attributes);
            AvailableCommands.Add(command_delete);
            AvailableCommands.Add(command_copy);
            AvailableCommands.Add(command_move);
            AvailableCommands.Add(command_create_dir);
            //AvailableCommands.Add(command_browse_streams);
            AvailableCommands.Add(command_fileinfo);
            //AvailableCommands.Add(command_security);
        }

        public string DirectoryPath
        {
            get
            {
                return directory_path;
            }
        }

        void internal_watcher_Error(object sender, ErrorEventArgs e)
        {
            Messages.ShowException(e.GetException());
        }

        void internal_watcher_Deleted(object sender, FileSystemEventArgs e)
        {
            int change_index = -1;
            WIN32_FIND_DATA old_data = new WIN32_FIND_DATA();

            //remove item
            change_index = internal_find_name(e.Name);
            if (change_index == -1)
            {
                return;
            }
            old_data = internal_list.Keys[change_index];
            internal_list.RemoveAt(change_index);
            OnItemRemove(change_index);
            if ((old_data.dwFileAttributes & FileAttributes.Directory) == FileAttributes.Directory)
            {
                cache_directory_count--;
            }
            else
            {
                cache_files_count--;
                cache_size -= old_data.FileSize;
            }
            OnAfterSummaryUpdate();
        }

        void internal_watcher_Created(object sender, FileSystemEventArgs e)
        {
            int new_index = -1;
            WIN32_FIND_DATA new_data = new WIN32_FIND_DATA();

            //new file or folder
            if (WinAPiFSwrapper.GetFileInfo(e.FullPath, ref new_data))
            {
                internal_list.Add(new_data, null);
                new_index = internal_list.IndexOfKey(new_data);
                //we updates all items from new_index to end
                for (int i = new_index; i < internal_list.Count; i++)
                {
                    OnItemUpdate(i);
                }
                if ((new_data.dwFileAttributes & FileAttributes.Directory) == FileAttributes.Directory)
                {
                    cache_directory_count++;
                }
                else
                {
                    cache_files_count++;
                    cache_size += new_data.FileSize;
                }
                OnAfterSummaryUpdate();
            }
        }

        void internal_watcher_Renamed(object sender, RenamedEventArgs e)
        {
            int change_index = -1;
            int new_index = -1;
            int[] affected_interval = new int[2];
            WIN32_FIND_DATA new_data = new WIN32_FIND_DATA();

            if (WinAPiFSwrapper.GetFileInfo(e.Name, ref new_data))
            {
                change_index = internal_find_name(e.OldName);
                if (change_index == -1)
                {
                    return;
                }
                internal_list.RemoveAt(change_index);
                internal_list.Add(new_data, null);
                new_index = internal_list.IndexOfKey(new_data);
                if (new_index == change_index)
                {
                    OnItemUpdate(new_index);
                }
                else
                {
                    affected_interval[0] = Math.Min(change_index, new_index);
                    affected_interval[1] = Math.Max(change_index, new_index);
                    for (int i = affected_interval[0]; i <= affected_interval[1]; i++)
                    {
                        OnItemUpdate(i);
                    }
                }
            }
        }

        void internal_watcher_Changed(object sender, FileSystemEventArgs e)
        {
            //find affected DATA item
            int change_index = -1;
            int new_index = -1;
            int[] affected_interval = new int[2];
            WIN32_FIND_DATA new_data = new WIN32_FIND_DATA();
            WIN32_FIND_DATA old_data = new WIN32_FIND_DATA();


            if (WinAPiFSwrapper.GetFileInfo(e.FullPath, ref new_data))
            {
                change_index = internal_find_name(e.Name);
                if (change_index == -1)
                {
                    //not found - does not occur, but...
                    return;
                }
                old_data = internal_list.Keys[change_index];
                internal_list.RemoveAt(change_index);
                internal_list.Add(new_data, null);
                new_index = internal_list.IndexOfKey(new_data);
                if (new_index == change_index)
                {
                    //updated item in same order - update only one item
                    OnItemUpdate(new_index);
                }
                else
                {
                    //updated item change its index
                    //in this case affected items is within new_index and change_index inclusive
                    affected_interval[0] = Math.Min(change_index, new_index);
                    affected_interval[1] = Math.Max(change_index, new_index);
                    for (int i = affected_interval[0]; i <= affected_interval[1]; i++)
                    {
                        OnItemUpdate(i);
                    }
                }
                //summary update
                cache_size = cache_size - old_data.FileSize + new_data.FileSize;
                OnAfterSummaryUpdate();
            }
        }

        private int internal_find_name(string name_to_find)
        {
            int ret = -1;

            for (int i = 0; i < internal_list.Count; i++)
            {
                if (internal_list.Keys[i].cFileName == name_to_find)
                {
                    ret = i;
                    break;
                }
            }

            return ret;
        }

        protected override void internal_sort(int criteria_index, bool reverse_order)
        {
            IComparer<WIN32_FIND_DATA> new_comparer = new InternalComparer(criteria_index, reverse_order);
            SortedList<WIN32_FIND_DATA, object> new_list = new SortedList<WIN32_FIND_DATA, object>(new_comparer);

            foreach (KeyValuePair<WIN32_FIND_DATA, object> kvp in internal_list)
            {
                new_list.Add(kvp.Key, kvp.Value);
            }

            internal_list = new_list;
            internal_comparer = new_comparer;

        }

        protected override void internal_dispose()
        {
            internal_watcher.Dispose();
        }

        protected override void internal_refill()
        {
            //stop change file system notyfications
            internal_watcher.EnableRaisingEvents = false;

            //we will fill new_list first
            //if errors will not occurs, fill work list
            List<WIN32_FIND_DATA> new_list = new List<WIN32_FIND_DATA>();
            try
            {
                string search_path = IOhelper.GetUnicodePath(directory_path);
                search_path = search_path.TrimEnd(new char[] { Path.DirectorySeparatorChar });
                search_path = search_path + Path.DirectorySeparatorChar + "*";
                WinAPiFSwrapper.WIN32_FIND_DATA_enumerable fs_enum = new WinAPiFSwrapper.WIN32_FIND_DATA_enumerable(search_path);
                foreach (WIN32_FIND_DATA data in fs_enum)
                {
                    if (data.cFileName == ".") //skip entry "." - that is current directory
                    {
                        continue;
                    }

                    new_list.Add(data);
                }

                //workaround for directory path like \\server_name\share$
                //for that path there is no '..' entry returned from fs_enum
                if ((directory_path.EndsWith("$")) && (directory_path.StartsWith(@"\\")))
                {
                    bool need_add = true;
                    for (int i = 0; i < new_list.Count; i++)
                    {
                        if (new_list[i].cFileName == "..")
                        {
                            need_add = false;
                            break;
                        }
                    }
                    if (need_add)
                    {
                        WIN32_FIND_DATA fake_net_data = new WIN32_FIND_DATA();
                        fake_net_data.cFileName = "..";
                        fake_net_data.dwFileAttributes = FileAttributes.Directory;
                        new_list.Add(fake_net_data);
                    }
                }

                //if no errors...
                internal_list.Clear();
                cache_directory_count = 0;
                cache_directory_selected_count = 0;
                cache_files_count = 0;
                cache_files_selected_count = 0;
                cache_selected_indices = new int[] { };
                cache_size = 0UL;
                cache_size_selected = 0UL;
                //update internal_list, counts, skip parent dir entry
                foreach (WIN32_FIND_DATA data in new_list)
                {
                    internal_list.Add(data, null);
                    if (data.cFileName == "..")
                    {
                        continue;
                    }
                    if ((data.dwFileAttributes & FileAttributes.Directory) == FileAttributes.Directory)
                    {
                        cache_directory_count++;
                    }
                    else
                    {
                        cache_files_count++;
                        cache_size += data.FileSize;
                    }
                }

                //set notificator
                internal_watcher.Filter = string.Empty;
                internal_watcher.Path = directory_path;
                try
                {
                    internal_watcher.EnableRaisingEvents = true;
                }
                catch (Exception ex)
                {
                    //Messages.ShowException(ex);
                }
            }

            catch (Exception ex)
            {
                throw ex;
            }
        }

        public override int FindIndexOfName(string name)
        {
            return internal_find_name(name);
        }

        public override void GetChildCollection
            (int index,
            ref FileCollectionBase new_collection,
            ref bool use_new,
            ref string preferred_focus)
        {

            if (!GetItemIsContainer(index))
            {
                use_new = false;
                return;
            }

            //directory to go to..
            string target_dir = Path.Combine(directory_path, internal_list.Keys[index].cFileName);
            target_dir = Path.GetFullPath(target_dir);            

            //case for smb network paths
            if ((target_dir == directory_path) && (target_dir.StartsWith(@"\\")))
            {
                //go to share list
                try
                {

                    netCommander.WNet.NETRESOURCE target_res = 
                        netCommander.WNet.WinApiWNETwrapper.GetParentResource
                        (netCommander.WNet.WinApiWNETwrapper.GetResourceInfo(directory_path));
                    WnetResourceList netsh_list = new WnetResourceList(0, false, target_res);
                    netsh_list.MainWindow = MainWindow;
                    netsh_list.Refill();
                    new_collection = netsh_list;
                    use_new = true;
                    preferred_focus = directory_path;
                }
                catch (Exception ex)
                {
                    Messages.ShowException(ex);
                }
                return;
            }

            //preferred focus text
            if (internal_list.Keys[index].cFileName == "..")
            {
                //go up
                string old_dir = Path.GetFileName(directory_path);
                preferred_focus = old_dir;
            }
            else
            {
                //go down - focus on 0
                preferred_focus = string.Empty;
            }

            //use current DirectoryList
            use_new = false;
            //save current directory path
            //needed if exception occur while filling
            string dir_old = directory_path;
            directory_path = target_dir;
            try
            {
                Refill();
            }
            catch (Exception ex)
            {
                //directory path not change
                directory_path = dir_old;
                Messages.ShowException(ex);
            }
        }

        public override string[] SortCriteriaAvailable
        {
            get { return new string[] { SORT_NAME, SORT_EXTENSION, SORT_SIZE, SORT_DATE_CREATE, SORT_DATE_MODIFICATION, SORT_DATE_ACCESS }; }
        }

        public override string GetItemDisplayName(int index)
        {
            return internal_list.Keys[index].cFileName;
        }

        public override string GetItemDisplayNameLong(int index)
        {
            return GetItemDisplayName(index);
        }

        public override string GetStatusText()
        {
            return DirectoryPath;
        }

        public override string GetItemDisplaySummaryInfo(int index)
        {
            WIN32_FIND_DATA data = internal_list.Keys[index];
            DateTime mod_time = DateTime.FromFileTime(data.ftLastWriteTime);
            if ((data.dwFileAttributes & FileAttributes.Directory) == FileAttributes.Directory)
            {
                return string.Format
                    ("[Directory] [{0}] [{1} {2}]",
                    IOhelper.FileAttributes2String(data.dwFileAttributes),
                    mod_time.ToShortDateString(),
                    mod_time.ToShortTimeString());
            }
            else
            {
                return string.Format
                    ("{0} [{1}] [{2} {3}]",
                    IOhelper.SizeToString(data.FileSize),
                    IOhelper.FileAttributes2String(data.dwFileAttributes),
                    mod_time.ToShortDateString(),
                    mod_time.ToShortTimeString());
            }
        }

        public override string GetSummaryInfo()
        {
            return string.Format
                ("[{0} folders] [{1} files] [{2}]",
                cache_directory_count,
                cache_files_count,
                IOhelper.SizeToString(cache_size));
        }

        public override int[] FindItems()
        {
            //show filter and find within current collection
            DirectoryListFilterDialog filter_dialog = new DirectoryListFilterDialog();
            if (filter_dialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                DirectoryListFilter filter = filter_dialog.DirectoryListFilter;
                List<int> retList = new List<int>();
                WIN32_FIND_DATA current_data = new WIN32_FIND_DATA();
                bool pass = true;

                for (int i = 0; i < ItemCount; i++)
                {
                    current_data = internal_list.Keys[i];
                    pass = true;
                    for (int j = 0; j < filter.Masks.Length; j++)
                    {
                        if (!Wildcard.Match(filter.Masks[j], current_data.cFileName, false))
                        {
                            pass = false;
                            break;
                        }
                    }

                    if (!pass)
                    {
                        continue;
                    }

                    if ((!filter.IgnoreFileAttributes) && ((current_data.dwFileAttributes & filter.FileAttributes) == 0))
                    {
                        pass = false;
                        continue;
                    }

                    if (!filter.IgnoreSize)
                    {
                        switch (filter.FilterSizeCriteria)
                        {
                            case FilterSizeCriteria.Between:
                                if ((current_data.FileSize < (ulong)filter.SizeMinimum) || (current_data.FileSize > (ulong)filter.SizeMaximum))
                                {
                                    pass = false;
                                    continue;
                                }
                                break;
                                
                            case FilterSizeCriteria.Equal:
                                if (current_data.FileSize != (ulong)filter.SizeMinimum)
                                {
                                    pass = false;
                                    continue;
                                }
                                break;

                            case FilterSizeCriteria.Greater:
                                if (current_data.FileSize <= (ulong)filter.SizeMinimum)
                                {
                                    pass = false;
                                    continue;
                                }
                                break;

                            case FilterSizeCriteria.GreaterOrEqual:
                                if (current_data.FileSize < (ulong)filter.SizeMinimum)
                                {
                                    pass = false;
                                    continue;
                                }
                                break;

                            case FilterSizeCriteria.Less:
                                if (current_data.FileSize >= (ulong)filter.SizeMinimum)
                                {
                                    pass = false;
                                    continue;
                                }
                                break;

                            case FilterSizeCriteria.LessOrEqual:
                                if (current_data.FileSize > (ulong)filter.SizeMinimum)
                                {
                                    pass = false;
                                    continue;
                                }
                                break;
                        }
                    }

                    if (!filter.IgnoreTimeAccess)
                    {
                        if ((current_data.ftLastAccessTime < filter.AccessBegin.ToFileTime()) || (current_data.ftLastAccessTime > filter.AccessEnd.ToFileTime()))
                        {
                            pass = false;
                            continue;
                        }
                    }

                    if (!filter.IgnoreTimeCreate)
                    {
                        if ((current_data.ftCreationTime < filter.CreateBegin.ToFileTime()) || (current_data.ftCreationTime > filter.CreateEnd.ToFileTime()))
                        {
                            pass = false;
                            continue;
                        }
                    }

                    if (!filter.IgnoreTimeModification)
                    {
                        if ((current_data.ftLastWriteTime < filter.ModificationBegin.ToFileTime()) || (current_data.ftLastWriteTime > filter.ModificationEnd.ToFileTime()))
                        {
                            pass = false;
                            continue;
                        }
                    }

                   //pass=true
                    if (pass)
                    {
                        retList.Add(i);
                    }
                }
                return retList.ToArray();
            }
            else
            {
                return new int[] { };
            }
        }

        private void internal_refresh_selected_cache(int[] indices)
        {
            cache_directory_selected_count = 0;
            cache_files_selected_count = 0;
            cache_size_selected = 0UL;
            for (int i = 0; i < indices.Length; i++)
            {
                if ((internal_list.Keys[indices[i]].dwFileAttributes & FileAttributes.Directory) == FileAttributes.Directory)
                {
                    cache_directory_selected_count++;
                }
                else
                {
                    cache_files_selected_count++;
                    cache_size_selected += internal_list.Keys[indices[i]].FileSize;
                }
            }
        }

        public override string GetSummaryInfo(int[] indices)
        {
            if (cache_selected_indices.Length != indices.Length)
            {
                //need refresh selection cache
                internal_refresh_selected_cache(indices);
                cache_selected_indices = indices;
            }
            else
            {
                //need check selection indices
                for (int i = 0; i < indices.Length; i++)
                {
                    if (indices[i] != cache_selected_indices[i])
                    {
                        //need refresh selection cache
                        internal_refresh_selected_cache(indices);
                        cache_selected_indices = indices;
                        break;
                    }
                }
            }

            //now selection cache must be valid
            return string.Format("Selected: {0} folders, {1} files, {2}", cache_directory_selected_count, cache_files_selected_count, IOhelper.SizeToString(cache_size_selected));
        }

        public override int ItemCount
        {
            get { return internal_list.Count; }
        }

        public override bool GetItemSelectEnable(int index)
        {
            return (internal_list.Keys[index].cFileName != "..");
        }

        public override bool GetItemIsContainer(int index)
        {
            return ((internal_list.Keys[index].dwFileAttributes & FileAttributes.Directory) == FileAttributes.Directory);
        }

        private class InternalComparer : IComparer<WIN32_FIND_DATA>
        {
            private int sort_criteria = 0;
            private int _order = 1;

            public InternalComparer(int sort_criteria,bool sort_reverse)
            {
                this.sort_criteria = sort_criteria;
                _order = sort_reverse ? -1 : 1;
            }

            #region IComparer<WIN32_FIND_DATA> Members

            public int Compare(WIN32_FIND_DATA x, WIN32_FIND_DATA y)
            {
                //place .. first
                if (x.cFileName == "..")
                {
                    return -1;
                }
                if (y.cFileName == "..")
                {
                    return 1;
                }

                //place directory first
                if (((x.dwFileAttributes & FileAttributes.Directory) == FileAttributes.Directory) && 
                    ((y.dwFileAttributes & FileAttributes.Directory) != FileAttributes.Directory))
                {
                    //x - directory and y not directory
                    return -1;
                }
                if (((y.dwFileAttributes & FileAttributes.Directory) == FileAttributes.Directory) && 
                    ((x.dwFileAttributes & FileAttributes.Directory) != FileAttributes.Directory))
                {
                    //y - directory and x not directory
                    return 1;
                }

                switch (sort_criteria)
                {
                    case 0:
                        //NAME
                        return string.Compare(x.cFileName, y.cFileName) * _order;

                    case 1:
                        //Extension
                        int extension_res = string.Compare(Path.GetExtension(x.cFileName), Path.GetExtension(y.cFileName)) * _order;
                        if (extension_res == 0)
                        {
                            extension_res = string.Compare(x.cFileName, y.cFileName) * _order;
                        }
                        return extension_res;

                    case 2:
                        //SIZE
                        long delta_size = (long)x.FileSize - (long)y.FileSize;
                        if (delta_size > 0L)
                        {
                            return _order;
                        }
                        else if (delta_size < 0L)
                        {
                            return -_order;
                        }
                        else
                        {
                            return string.Compare(x.cFileName, y.cFileName) * _order;
                        }
                        
                    case 3:
                        //date create
                        long delta_create = x.ftCreationTime - y.ftCreationTime;
                        if (delta_create > 0L)
                        {
                            return _order;
                        }
                        else if (delta_create < 0L)
                        {
                            return -_order;
                        }
                        else
                        {
                            return string.Compare(x.cFileName, y.cFileName) * _order;
                        }

                    case 4:
                        //date modification
                        long delta_modif = x.ftLastWriteTime - y.ftLastWriteTime;
                        if (delta_modif > 0L)
                        {
                            return _order;
                        }
                        else if (delta_modif < 0L)
                        {
                            return -_order;
                        }
                        else
                        {
                            return string.Compare(x.cFileName, y.cFileName) * _order;
                        }

                    case 5:
                        //date access
                        long delta_acc = x.ftLastAccessTime - y.ftLastAccessTime;
                        if (delta_acc > 0L)
                        {
                            return _order;
                        }
                        else if (delta_acc < 0L)
                        {
                            return -_order;
                        }
                        else
                        {
                            return string.Compare(x.cFileName, y.cFileName) * _order;
                        }
                }

                return 0;
            }

            #endregion
        }







        
    }
}
