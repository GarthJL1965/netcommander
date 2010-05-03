using System;
using System.Collections.Generic;
using System.Text;
using netCommander.FileSystemEx;
using System.IO;
using System.ComponentModel;
using System.Runtime.InteropServices;
using System.Diagnostics;

namespace netCommander
{
    public class DirectoryList : FileCollectionBase
    {
        private readonly string SORT_NAME = Options.GetLiteral(Options.LANG_FILE_NAME);
        private readonly string SORT_SIZE = Options.GetLiteral(Options.LANG_FILE_SIZE);
        private readonly string SORT_DATE_CREATE = Options.GetLiteral(Options.LANG_CREATE_TIME);
        private readonly string SORT_DATE_ACCESS = Options.GetLiteral(Options.LANG_ACCESS_TIME);
        private readonly string SORT_DATE_MODIFICATION = Options.GetLiteral(Options.LANG_MODIFICATION_TIME);
        private readonly string SORT_EXTENSION = Options.GetLiteral(Options.LANG_EXTENSION);

        //private SortedList<WIN32_FIND_DATA, object> internal_list;
        private SortedList<FileInfoEx, object> internal_list;
        private string directory_path;
        private IComparer<FileInfoEx> internal_comparer;
        //private IComparer<WIN32_FIND_DATA> internal_comparer;
        //private bool directory_root=false;

        private int cache_directory_count = 0;
        private int cache_files_count = 0;
        private ulong cache_size = 0UL;
        private int cache_directory_selected_count = 0;
        private int cache_files_selected_count = 0;
        private ulong cache_size_selected = 0UL;
        private int[] cache_selected_indices = new int[] { };

        private FileSystemWatcher internal_watcher = new FileSystemWatcher();
        //private VolumeSpaceInfo internal_space_info = new VolumeSpaceInfo();

        //private PanelCommandShowAFS command_show_afs = new PanelCommandShowAFS();
        private PanelCommandTouchFile command_touch = new PanelCommandTouchFile();
        private PanelCommandFileAttributesEdit command_attributes = new PanelCommandFileAttributesEdit();
        private PanelComandDeleteFile command_delete = new PanelComandDeleteFile();
        private PanelCommandCopyFile command_copy = new PanelCommandCopyFile();
        private PanelCommandMoveFile command_move = new PanelCommandMoveFile();
        private PanelComandCreateDirectory command_create_dir = new PanelComandCreateDirectory();
        private PanelCommandFileInfo command_fileinfo = new PanelCommandFileInfo();
        private PanelCommandCreateLink command_create_link = new PanelCommandCreateLink();
        private PanelCommandRunExe command_run = new PanelCommandRunExe();
        private PanelCommandVolumeSpace command_volume_space = new PanelCommandVolumeSpace();
        private PanelCommandFileView command_view = new PanelCommandFileView();
        //private PanelCommandFileEdit command_edit = new PanelCommandFileEdit();

        public DirectoryList(int sort_index, bool sort_reverse, string directory_path)
            : base(sort_index, sort_reverse)
        {
            this.directory_path = directory_path;
            internal_comparer = new InternalComparer(sort_index, sort_reverse);
            internal_list = new SortedList<FileInfoEx, object>(internal_comparer);

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
            //AvailableCommands.Add(command_show_afs);
            AvailableCommands.Add(command_touch);
            AvailableCommands.Add(command_attributes);
            AvailableCommands.Add(command_delete);
            AvailableCommands.Add(command_copy);
            AvailableCommands.Add(command_move);
            AvailableCommands.Add(command_create_dir);
            //AvailableCommands.Add(command_browse_streams);
            AvailableCommands.Add(command_fileinfo);
            //AvailableCommands.Add(command_security);
            AvailableCommands.Add(command_create_link);
            AvailableCommands.Add(command_run);
            AvailableCommands.Add(command_volume_space);
            AvailableCommands.Add(command_view);
            //AvailableCommands.Add(command_edit); - VERY buggy now
        }

        public string DirectoryPath
        {
            get
            {
                return directory_path;
            }
        }

        public FileInfoEx this[int index]
        {
            get
            {
                return internal_list.Keys[index];
            }
        }

        public VolumeSpaceInfo VolumeSpaceInfo
        {
            get
            {
                return WinAPiFSwrapper.GetVolumeSpaceInfo(DirectoryPath);
            }
        }

        public override ItemCategory GetItemCategory(int index)
        {
            if ((index < 0) || (index >= ItemCount))
            {
                return ItemCategory.Default;
            }

            FileAttributes fa = this[index].FileAttributes;
            if ((fa & FileAttributes.Hidden) == FileAttributes.Hidden)
            {
                return ItemCategory.Hidden;
            }
            if ((fa & FileAttributes.Directory) == FileAttributes.Directory)
            {
                return ItemCategory.Container;
            }
            return ItemCategory.Default;
        }

        void internal_watcher_Error(object sender, ErrorEventArgs e)
        {
            //supress
            //Messages.ShowException(e.GetException());
        }

        void internal_watcher_Deleted(object sender, FileSystemEventArgs e)
        {
            int change_index = -1;
            FileInfoEx old_data = null;

            //remove item
            change_index = internal_find_name(e.Name);
            if (change_index == -1)
            {
                return;
            }
            old_data = internal_list.Keys[change_index];
            internal_list.RemoveAt(change_index);
            OnItemRemove(change_index);
            if (old_data.Directory)
            {
                cache_directory_count--;
            }
            else
            {
                cache_files_count--;
                cache_size -= old_data.Size;
            }
            OnAfterSummaryUpdate();
        }

        void internal_watcher_Created(object sender, FileSystemEventArgs e)
        {
            int new_index = -1;
            FileInfoEx new_info = null;
            WIN32_FIND_DATA new_data = new WIN32_FIND_DATA();

            //new file or folder
            if (WinAPiFSwrapper.GetFileInfo(e.FullPath, ref new_data))
            {
                new_info = new FileInfoEx(new_data, DirectoryPath);
                if (!internal_list.ContainsKey(new_info))
                {
                    internal_list.Add(new_info, null);
                    new_index = internal_list.IndexOfKey(new_info);
                    //we updates all items from new_index to end
                    for (int i = new_index; i < internal_list.Count; i++)
                    {
                        OnItemUpdate(i);
                    }
                    if (new_info.Directory)
                    {
                        cache_directory_count++;
                    }
                    else
                    {
                        cache_files_count++;
                        cache_size += new_info.Size;
                    }
                    OnAfterSummaryUpdate();
                }
            }
        }

        void internal_watcher_Renamed(object sender, RenamedEventArgs e)
        {
            int change_index = -1;
            int new_index = -1;
            int[] affected_interval = new int[2];
            WIN32_FIND_DATA new_data = new WIN32_FIND_DATA();
            FileInfoEx new_info = null;

            if (WinAPiFSwrapper.GetFileInfo(e.Name, ref new_data))
            {
                change_index = internal_find_name(e.OldName);
                if (change_index == -1)
                {
                    return;
                }
                new_info = new FileInfoEx(new_data, DirectoryPath);
                internal_list.RemoveAt(change_index);
                internal_list.Add(new_info, null);
                new_index = internal_list.IndexOfKey(new_info);
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
            FileInfoEx old_info = null;
            FileInfoEx new_info = null;

            if (WinAPiFSwrapper.GetFileInfo(e.FullPath, ref new_data))
            {
                change_index = internal_find_name(e.Name);
                if (change_index == -1)
                {
                    //not found - does not occur, but...
                    return;
                }
                old_info = internal_list.Keys[change_index];
                internal_list.RemoveAt(change_index);
                new_info = new FileInfoEx(new_data, DirectoryPath);
                internal_list.Add(new_info, null);
                new_index = internal_list.IndexOfKey(new_info);
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
                cache_size = cache_size - old_info.Size + new_info.Size;
                OnAfterSummaryUpdate();
            }
        }

        protected override void DoDefaultAction(int index, bool shift)
        {
            //start process (<enter> <shift-enter> in non-container items)

            string file_name = this[index].FullName;

            ProcessStartInfo psi = new ProcessStartInfo();
            psi.CreateNoWindow = false;
            if (MainWindow != null)
            {
                psi.ErrorDialog = true;
                psi.ErrorDialogParentHandle = MainWindow.Handle;
            }
            psi.WindowStyle = ProcessWindowStyle.Normal;
            psi.WorkingDirectory = DirectoryPath;
            psi.UseShellExecute = !shift;
            psi.FileName = file_name;

            try
            {
                Process.Start(psi);
            }
            catch (Exception ex)
            {
                Messages.ShowException
                    (ex,
                    string.Format(Options.GetLiteral(Options.LANG_CANNOT_RUN_0), psi.FileName));
            }
        }

        private int internal_find_name(string name_to_find)
        {
            int ret = -1;

            for (int i = 0; i < internal_list.Count; i++)
            {
                if (internal_list.Keys[i].FileName == name_to_find)
                {
                    ret = i;
                    break;
                }
            }

            return ret;
        }

        protected override void internal_sort(int criteria_index, bool reverse_order)
        {
            IComparer<FileInfoEx> new_comparer = new InternalComparer(criteria_index, reverse_order);
            SortedList<FileInfoEx, object> new_list = new SortedList<FileInfoEx, object>(new_comparer);

            foreach (KeyValuePair<FileInfoEx, object> kvp in internal_list)
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
            List<FileInfoEx> new_list = new List<FileInfoEx>();
            try
            {
                string search_path = IOhelper.GetUnicodePath(directory_path);
                search_path = search_path.TrimEnd(new char[] { Path.DirectorySeparatorChar });
                search_path = search_path + Path.DirectorySeparatorChar + "*";
                WinAPiFSwrapper.WIN32_FIND_DATA_enumerable fs_enum =
                    new WinAPiFSwrapper.WIN32_FIND_DATA_enumerable(search_path);
                foreach (WIN32_FIND_DATA data in fs_enum)
                {
                    if (data.cFileName == ".") //skip entry "." - that is current directory
                    {
                        continue;
                    }

                    if (data.cFileName == "..")
                    {
                        continue;
                    }

                    new_list.Add(new FileInfoEx(data, directory_path));
                }

                //add entry '..'
                string parent_test = Path.GetDirectoryName(directory_path);
                if ((parent_test != null) && (parent_test != string.Empty))
                {
                    new_list.Add(FileInfoEx.CreateAsParent(directory_path));
                }
                else if (directory_path.StartsWith("\\"))
                {
                    //that is UNC root
                    new_list.Add(FileInfoEx.CreateAsParent(directory_path));
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
                foreach (FileInfoEx data in new_list)
                {
                    internal_list.Add(data, null);
                    if (data.FileName == "..")
                    {
                        continue;
                    }
                    if (data.Directory)
                    {
                        cache_directory_count++;
                    }
                    else
                    {
                        cache_files_count++;
                        cache_size += data.Size;
                    }
                }

                //update space info
                //internal_space_info = WinAPiFSwrapper.GetVolumeSpaceInfo(DirectoryPath);

                //set notificator
                try
                {
                    internal_watcher.Filter = string.Empty;
                    internal_watcher.Path = directory_path;
                    internal_watcher.EnableRaisingEvents = true;
                }
                catch (Exception)
                {
                    //supress
                    //Messages.ShowException(ex);
                }

                try
                {
                    Directory.SetCurrentDirectory(DirectoryPath);
                }
                catch (Exception) { }
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

        public override string GetCommandlineTextShort(int index)
        {
            string ret = string.Empty;
            FileInfoEx info = this[index];
            if (info.FileName == "..")
            {
                return ret;
            }
            else
            {
                return info.FileName;
            }
        }

        public override string GetCommandlineTextLong(int index)
        {
            string ret = string.Empty;
            FileInfoEx info = this[index];
            if (info.FileName == "..")
            {
                return ret;
            }
            else
            {
                return info.FullName;
            }
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
            string target_dir = internal_list.Keys[index].FullName;
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
            if (internal_list.Keys[index].FileName == "..")
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

            //zip handler
            if (target_dir.EndsWith(".zip", StringComparison.InvariantCultureIgnoreCase))
            {
                new_collection = new ZipDirectory(target_dir, 0, false);
                new_collection.MainWindow = (mainForm)Program.MainWindow;
                try
                {
                    new_collection.Refill();
                    use_new = true;
                }
                catch (Exception ex)
                {
                    Messages.ShowException(ex);
                }
                return;
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
            return internal_list.Keys[index].FileName;
        }

        public override string GetItemDisplayNameLong(int index)
        {
            return GetItemDisplayName(index);
        }

        public override string GetStatusText()
        {
            return string.Format
                ("{0}",
                DirectoryPath);
        }

        public override string GetItemDisplaySummaryInfo(int index)
        {
            FileInfoEx data = internal_list.Keys[index];

            if (data.FileName == "..")
            {
                return Options.GetLiteral(Options.LANG_UP);
            }

            DateTime mod_time = data.WriteTime;

            if (data.Directory)
            {

                return string.Format
                    ("[" + Options.GetLiteral(Options.LANG_DIRECTORY) + "] [{0}] [{1} {2}]",
                    IOhelper.FileAttributes2String(data.FileAttributes),
                    mod_time.ToShortDateString(),
                    mod_time.ToShortTimeString());
            }
            else
            {
                return string.Format
                    ("{0} [{1}] [{2} {3}]",
                    IOhelper.SizeToString(data.Size),
                    IOhelper.FileAttributes2String(data.FileAttributes),
                    mod_time.ToShortDateString(),
                    mod_time.ToShortTimeString());
            }
        }

        public override string GetSummaryInfo()
        {
            return string.Format
                ("[{0} " + Options.GetLiteral(Options.LANG_DIRECTORIES) + "] [{1} " + Options.GetLiteral(Options.LANG_FILES) + "] [{2}]",
                cache_directory_count,
                cache_files_count,
                IOhelper.SizeToString(cache_size));

            //alternate:
            //return string.Format
            //    ("Available {0} from {1}",
            //    IOhelper.SizeToString(internal_space_info.FreeBytesAvailable),
            //    IOhelper.SizeToString(internal_space_info.TotalNumberOfBytes));
        }

        public override int[] FindItems()
        {
            //show filter and find within current collection
            DirectoryListFilterDialog filter_dialog = new DirectoryListFilterDialog();
            if (filter_dialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                DirectoryListFilter filter = filter_dialog.DirectoryListFilter;
                List<int> retList = new List<int>();
                FileInfoEx current_data = new FileInfoEx();
                bool pass = true;

                for (int i = 0; i < ItemCount; i++)
                {
                    current_data = internal_list.Keys[i];
                    pass = true;
                    for (int j = 0; j < filter.Masks.Length; j++)
                    {
                        if (!Wildcard.Match(filter.Masks[j], current_data.FileName, false))
                        {
                            pass = false;
                            break;
                        }
                    }

                    if (!pass)
                    {
                        continue;
                    }

                    if ((!filter.IgnoreFileAttributes) &&
                        ((current_data.FileAttributes & filter.FileAttributes) == 0))
                    {
                        pass = false;
                        continue;
                    }

                    if (!filter.IgnoreSize)
                    {
                        switch (filter.FilterSizeCriteria)
                        {
                            case FilterSizeCriteria.Between:
                                if ((current_data.Size < (ulong)filter.SizeMinimum) || (current_data.Size > (ulong)filter.SizeMaximum))
                                {
                                    pass = false;
                                    continue;
                                }
                                break;

                            case FilterSizeCriteria.Equal:
                                if (current_data.Size != (ulong)filter.SizeMinimum)
                                {
                                    pass = false;
                                    continue;
                                }
                                break;

                            case FilterSizeCriteria.Greater:
                                if (current_data.Size <= (ulong)filter.SizeMinimum)
                                {
                                    pass = false;
                                    continue;
                                }
                                break;

                            case FilterSizeCriteria.GreaterOrEqual:
                                if (current_data.Size < (ulong)filter.SizeMinimum)
                                {
                                    pass = false;
                                    continue;
                                }
                                break;

                            case FilterSizeCriteria.Less:
                                if (current_data.Size >= (ulong)filter.SizeMinimum)
                                {
                                    pass = false;
                                    continue;
                                }
                                break;

                            case FilterSizeCriteria.LessOrEqual:
                                if (current_data.Size > (ulong)filter.SizeMinimum)
                                {
                                    pass = false;
                                    continue;
                                }
                                break;
                        }
                    }

                    if (!filter.IgnoreTimeAccess)
                    {
                        if ((current_data.AccessFileTime < filter.AccessBegin.ToFileTime()) ||
                            (current_data.AccessFileTime > filter.AccessEnd.ToFileTime()))
                        {
                            pass = false;
                            continue;
                        }
                    }

                    if (!filter.IgnoreTimeCreate)
                    {
                        if ((current_data.CreationFileTime < filter.CreateBegin.ToFileTime()) ||
                            (current_data.CreationFileTime > filter.CreateEnd.ToFileTime()))
                        {
                            pass = false;
                            continue;
                        }
                    }

                    if (!filter.IgnoreTimeModification)
                    {
                        if ((current_data.WriteFileTime < filter.ModificationBegin.ToFileTime()) ||
                            (current_data.WriteFileTime > filter.ModificationEnd.ToFileTime()))
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
                if (internal_list.Keys[indices[i]].Directory)
                {
                    cache_directory_selected_count++;
                }
                else
                {
                    cache_files_selected_count++;
                    cache_size_selected += internal_list.Keys[indices[i]].Size;
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
            return string.Format
                (Options.GetLiteral(Options.LANG_SELECTED) + ": {0} " +
                Options.GetLiteral(Options.LANG_DIRECTORIES) + ", {1} " +
                Options.GetLiteral(Options.LANG_FILES) + ", {2}",
                cache_directory_selected_count,
                cache_files_selected_count,
                IOhelper.SizeToString(cache_size_selected));
        }

        public override int ItemCount
        {
            get { return internal_list.Count; }
        }

        public override bool GetItemSelectEnable(int index)
        {
            return (internal_list.Keys[index].FileName != "..");
        }

        public override bool GetItemIsContainer(int index)
        {
            return (internal_list.Keys[index].Directory || internal_list.Keys[index].FileName.EndsWith(".zip", StringComparison.InvariantCultureIgnoreCase));
        }

        #region dragdrop implementation
        public override System.Windows.Forms.IDataObject GetDragdropObject(int index)
        {
            return new InternalDataObject(new string[] { this[index].FullName });
        }

        public override System.Windows.Forms.DragDropEffects GetDragdropEffects(int index)
        {
            return System.Windows.Forms.DragDropEffects.Copy | System.Windows.Forms.DragDropEffects.Move;
        }

        public override void DragdropDo(System.Windows.Forms.DragEventArgs e)
        {
            string drops=string.Empty;
            string[] drop_data=(string[])e.Data.GetData(InternalDataObject.FILE_DROP);
            for(int i=0;i<drop_data.Length;i++)
            {
                drops=drops+drop_data[i]+"; ";
            }

            Messages.ShowMessage(string.Format("drop [{0}]", drops));

            
        }

        public override void DragdropEnterFeedback(System.Windows.Forms.DragEventArgs e, out bool accept_dragdrop)
        {
            if (e.Data.GetDataPresent(InternalDataObject.FILE_DROP))
            {
                e.Effect = System.Windows.Forms.DragDropEffects.Copy;
                accept_dragdrop = true;
            }
            else
            {
                e.Effect = System.Windows.Forms.DragDropEffects.None;
                accept_dragdrop = false;
            }
        }

        public override void DragdropOverFeedback(System.Windows.Forms.DragEventArgs e)
        {
            if (e.Data.GetDataPresent(InternalDataObject.FILE_DROP))
            {
                e.Effect = System.Windows.Forms.DragDropEffects.Copy;
            }
            else
            {
                e.Effect = System.Windows.Forms.DragDropEffects.None;
            }
        }

        #endregion

        #region comparer
        private class InternalComparer : IComparer<FileInfoEx>
        {
            private int sort_criteria = 0;
            private int _order = 1;

            public InternalComparer(int sort_criteria, bool sort_reverse)
            {
                this.sort_criteria = sort_criteria;
                _order = sort_reverse ? -1 : 1;
            }

            #region IComparer<WIN32_FIND_DATA> Members

            public int Compare(FileInfoEx x, FileInfoEx y)
            {
                //place .. first
                if (x.FileName == "..")
                {
                    return -1;
                }
                if (y.FileName == "..")
                {
                    return 1;
                }

                //place directory first
                if (x.Directory && (!y.Directory))
                {
                    //x - directory and y not directory
                    return -1;
                }
                if (y.Directory && (!x.Directory))
                {
                    //y - directory and x not directory
                    return 1;
                }

                switch (sort_criteria)
                {
                    case 0:
                        //NAME
                        return string.Compare(x.FileName, y.FileName) * _order;

                    case 1:
                        //Extension
                        int extension_res = string.Compare(x.FileNameExtention, y.FileNameExtention) * _order;
                        if (extension_res == 0)
                        {
                            extension_res = string.Compare(x.FileName, y.FileName) * _order;
                        }
                        return extension_res;

                    case 2:
                        //SIZE
                        long delta_size = (long)x.Size - (long)y.Size;
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
                            return string.Compare(x.FileName, y.FileName) * _order;
                        }

                    case 3:
                        //date create
                        long delta_create = x.CreationFileTime - y.CreationFileTime;
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
                            return string.Compare(x.FileName, y.FileName) * _order;
                        }

                    case 4:
                        //date modification
                        long delta_modif = x.WriteFileTime - y.WriteFileTime;
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
                            return string.Compare(x.FileName, y.FileName) * _order;
                        }

                    case 5:
                        //date access
                        long delta_acc = x.AccessFileTime - y.AccessFileTime;
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
                            return string.Compare(x.FileName, y.FileName) * _order;
                        }
                }

                return 0;
            }

            #endregion
        }

        private class InternalComparer_1 : IComparer<WIN32_FIND_DATA>
        {
            private int sort_criteria = 0;
            private int _order = 1;

            public InternalComparer_1(int sort_criteria, bool sort_reverse)
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
        #endregion

        #region idataobject
        private class InternalDataObject : System.Windows.Forms.IDataObject
        {
            private string[] internal_file_names;
            public const string FILE_DROP = "FileDrop";

            public InternalDataObject(string[] file_names)
            {
                internal_file_names = file_names;
            }

            public object GetData(Type format)
            {
                if (format == typeof(String[]))
                {
                    return internal_file_names;
                }
                else
                {
                    return null;
                }
            }

            public object GetData(string format)
            {
                if (format == FILE_DROP)
                {
                    return internal_file_names;
                }
                else
                {
                    return null;
                }
            }

            public object GetData(string format, bool autoConvert)
            {
                return GetData(format);
            }

            public bool GetDataPresent(Type format)
            {
                return (format == typeof(String[]));
            }

            public bool GetDataPresent(string format)
            {
                return format == FILE_DROP;
            }

            public bool GetDataPresent(string format, bool autoConvert)
            {
                return format == FILE_DROP;
            }

            public string[] GetFormats()
            {
                return new string[] { FILE_DROP };
            }

            public string[] GetFormats(bool autoConvert)
            {
                return new string[] { FILE_DROP };   
            }

            public void SetData(object data)
            {
                if (data is string[])
                {
                    internal_file_names = (string[])data;
                }
                else
                {
                    throw new ArgumentException();
                }
            }

            public void SetData(Type format, object data)
            {
                SetData(data);
            }

            public void SetData(string format, object data)
            {
                SetData(data);
            }

            public void SetData(string format, bool autoConvert, object data)
            {
                SetData(data);
            }
        }
        #endregion
    }

    public class FileInfoEx
    {
        private WIN32_FIND_DATA find_data;
        private string parent_directory = string.Empty;

        public FileInfoEx()
        {

        }

        public FileInfoEx(WIN32_FIND_DATA search_data, string directory)
        {
            find_data = search_data;
            parent_directory = directory;
        }

        public static FileInfoEx CreateAsParent(string directory)
        {
            FileInfoEx ret = new FileInfoEx();
            ret.parent_directory = directory;
            ret.find_data.cFileName = "..";
            ret.find_data.dwFileAttributes = FileAttributes.Directory;
            return ret;
        }

        public bool Directory
        {
            get
            {
                return (find_data.dwFileAttributes & FileAttributes.Directory) == FileAttributes.Directory;
            }
        }

        public FileAttributes FileAttributes
        {
            get
            {
                return find_data.dwFileAttributes;
            }
        }

        public string AlternateFileName
        {
            get
            {
                return find_data.cAlternateFileName;
            }
        }

        public string FileName
        {
            get
            {
                return find_data.cFileName;
            }
        }

        public IO_REPARSE_TAG ReparseTag
        {
            get
            {
                return find_data.ReparseTag;
            }
        }

        public ulong Size
        {
            get
            {
                return find_data.FileSize;
            }
        }

        public long CreationFileTime
        {
            get
            {
                return find_data.ftCreationTime;
            }
        }

        public DateTime CreationTime
        {
            get
            {
                return DateTime.FromFileTime(find_data.ftCreationTime);
            }
        }

        public DateTime CreationTimeUTC
        {
            get
            {
                return DateTime.FromFileTimeUtc(find_data.ftCreationTime);
            }
        }

        public long AccessFileTime
        {
            get
            {
                return find_data.ftLastAccessTime;
            }
        }

        public DateTime AccessTime
        {
            get
            {
                return DateTime.FromFileTime(find_data.ftLastAccessTime);
            }
        }

        public DateTime AccessTimeUTC
        {
            get
            {
                return DateTime.FromFileTimeUtc(find_data.ftLastAccessTime);
            }
        }

        public long WriteFileTime
        {
            get
            {
                return find_data.ftLastWriteTime;
            }
        }

        public DateTime WriteTime
        {
            get
            {
                return DateTime.FromFileTime(find_data.ftLastWriteTime);
            }
        }

        public DateTime WriteTimeUTC
        {
            get
            {
                return DateTime.FromFileTimeUtc(find_data.ftLastWriteTime);
            }
        }

        public string ParentDirectory
        {
            get
            {
                return parent_directory;
            }
        }

        public string FullName
        {
            get
            {
                return Path.Combine(ParentDirectory, FileName);
            }
        }

        public string FileNameExtention
        {
            get
            {
                return Path.GetExtension(FileName);
            }
        }

        public IntPtr OpenHandle
            (Win32FileAccess desired_access,
            FileShare share,
            CreateFileOptions options)
        {
            IntPtr ret = IntPtr.Zero;

            ret = WinApiFS.CreateFile_intptr
                (FullName,
                desired_access,
                share,
                IntPtr.Zero,
                FileMode.Open,
                options,
                IntPtr.Zero);
            if (ret.ToInt32() == WinApiFS.INVALID_HANDLE_VALUE)
            {
                throw new Win32Exception(Marshal.GetLastWin32Error());
            }
            return ret;
        }

        public FILE_FS_DEVICE_INFORMATION GetDeviceInfo()
        {
            IntPtr file_handle = IntPtr.Zero;

            try
            {
                file_handle = OpenHandle
                    (Win32FileAccess.READ_ATTRIBUTES | Win32FileAccess.READ_EA,
                    FileShare.ReadWrite | FileShare.Delete,
                    Directory ? CreateFileOptions.BACKUP_SEMANTICS : CreateFileOptions.None);
                return ntApiFSwrapper.GetFileVolumeDeviceInfo(file_handle);
            }
            finally
            {
                if ((file_handle.ToInt32() != WinApiFS.INVALID_HANDLE_VALUE) && (file_handle != IntPtr.Zero))
                {
                    WinApiFS.CloseHandle(file_handle);
                }
            }
        }

        private LinkInfo intern_link_info = null;
        public LinkInfo LinkInformation
        {
            get
            {
                switch (ReparseTag)
                {
                    case IO_REPARSE_TAG.MOUNT_POINT:
                        if (intern_link_info == null)
                        {
                            intern_link_info = new LinkInfo(WinAPiFSwrapper.GetMountpointInfo(FullName));
                        }
                        break;

                    case IO_REPARSE_TAG.SYMLINK:
                        if (intern_link_info == null)
                        {
                            intern_link_info = new LinkInfo(WinAPiFSwrapper.GetSymboliclinkInfo(FullName));
                        }
                        break;
                }
                return intern_link_info;
            }
        }

        public static bool TryGet(string full_name, ref FileInfoEx info)
        {
            WIN32_FIND_DATA data=new WIN32_FIND_DATA();
            if (WinAPiFSwrapper.GetFileInfo(full_name, ref data))
            {
                string dir_name = Path.GetDirectoryName(full_name);
                if (dir_name == null)
                {
                    //that is full_name is root directory
                    dir_name = full_name;
                }

                info = new FileInfoEx(data, dir_name);
                return true;
            }
            else
            {
                //case if full_name like "\\server\resource"
                if (full_name.StartsWith(IOhelper.SMB_PREFIX))
                {
                    WNet.NETRESOURCE res_info;
                    try
                    {
                        //check existance
                        res_info = WNet.WinApiWNETwrapper.GetResourceInfo(full_name);
                    }
                    catch (Exception)
                    {
                        //not exists
                        return false;
                    }

                    //return fake FileInfoEx
                    WIN32_FIND_DATA res_fake_data = new WIN32_FIND_DATA();
                    res_fake_data.cFileName = full_name;
                    res_fake_data.dwFileAttributes = FileAttributes.Directory;
                    info = new FileInfoEx(res_fake_data, full_name);
                    return true;
                }

                return false;
            }
        }
    }

    public class FileInfoExEnumerable : IEnumerable<FileInfoEx>
    {
        private string search_path;
        private bool supress_access_denied = false;
        private bool supress_path_not_found = false;
        private bool supress_file_not_found = false;
        private bool supress_dot_entries = false;

        public FileInfoExEnumerable(string search_path)
        {
            this.search_path = search_path;
        }

        public FileInfoExEnumerable
            (string search_path,
            bool supress_access_denied,
            bool supress_path_not_found,
            bool supress_file_not_found,
            bool supress_dot_entries)
        {
            this.search_path = search_path;
            this.supress_access_denied = supress_access_denied;
            this.supress_file_not_found = supress_file_not_found;
            this.supress_path_not_found = supress_path_not_found;
            this.supress_dot_entries = supress_dot_entries;
        }



        #region IEnumerable<FileInfoEx> Members

        public IEnumerator<FileInfoEx> GetEnumerator()
        {
            return new InternalEnumeratorSimpleSearch
            (search_path,
            supress_path_not_found,
            supress_file_not_found,
            supress_access_denied,
            supress_dot_entries);
        }

        #endregion

        #region IEnumerable Members

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        #endregion

        private class InternalEnumeratorSimpleSearch : IEnumerator<FileInfoEx>
        {
            private string search_dir = string.Empty;
            private string search_path = string.Empty;
            private IntPtr search_handle = IntPtr.Zero;
            private WIN32_FIND_DATA current_data;
            private bool supress_path_not_found = false;
            private bool supress_file_not_found = false;
            private bool supress_access_denied = false;
            private bool supress_dot_entries = false;

            public InternalEnumeratorSimpleSearch
                (string search_path,
                bool supress_path_not_found,
                bool supress_file_not_found,
                bool supress_access_denied,
                bool supress_dot_entries)
            {
                this.search_path = IOhelper.GetUnicodePath(search_path);
                search_dir = Path.GetDirectoryName(search_path);
                this.supress_access_denied = supress_access_denied;
                this.supress_file_not_found = supress_file_not_found;
                this.supress_path_not_found = supress_path_not_found;
                this.supress_dot_entries = supress_dot_entries;
            }

            #region IEnumerator<FileInfoEx> Members

            public FileInfoEx Current
            {
                get 
                {
                    return new FileInfoEx(current_data, search_dir);
                }
            }

            #endregion

            #region IDisposable Members

            public void Dispose()
            {
                if ((search_handle.ToInt32() != WinApiFS.INVALID_HANDLE_VALUE) && (search_handle != IntPtr.Zero))
                {
                    WinApiFS.FindClose(search_handle);
                }
            }

            #endregion

            private bool init_search_handle()
            {
                //create search handle
                search_handle = WinApiFS.FindFirstFile
                (search_path,
                ref current_data);
                if (search_handle.ToInt32() == WinApiFS.INVALID_HANDLE_VALUE)
                {
                    int win_err = Marshal.GetLastWin32Error();
                    switch (win_err)
                    {
                        case WinApiFS.ERROR_ACCESS_DENIED:
                            if (!supress_access_denied)
                            {
                                throw new Win32Exception(win_err);
                            }
                            break;

                        case WinApiFS.ERROR_FILE_NOT_FOUND:
                            if (!supress_file_not_found)
                            {
                                throw new Win32Exception(win_err);
                            }
                            break;

                        case WinApiFS.ERROR_PATH_NOT_FOUND:
                            if (!supress_path_not_found)
                            {
                                throw new Win32Exception(win_err);
                            }
                            break;

                        default:
                            throw new Win32Exception(win_err);
                    }
                    return false;
                }
                else
                {
                    if (supress_dot_entries)
                    {
                        while ((current_data.cFileName == ".") || (current_data.cFileName == ".."))
                        {
                            bool ret = next_search();
                            if (ret == false)
                            {
                                return false;
                            }
                        }
                        return true;
                    }
                    else
                    {
                        return true;
                    }
                }
            }

            private bool next_search()
            {
                int res = WinApiFS.FindNextFile
                        (search_handle,
                        ref current_data);
                if (res == 0)
                {
                    int win_err = Marshal.GetLastWin32Error();
                    switch (win_err)
                    {
                        case WinApiFS.ERROR_NO_MORE_FILES:
                            return false;

                        default:
                            throw new Win32Exception(win_err);
                    }
                    
                }
                else
                {
                    if (supress_dot_entries)
                    {
                        while ((current_data.cFileName == ".") || (current_data.cFileName == ".."))
                        {
                            bool ret = next_search();
                            if (ret == false)
                            {
                                return false;
                            }
                        }
                        return true;
                    }
                    else
                    {
                        return true;
                    }
                }
            }

            #region IEnumerator Members

            object System.Collections.IEnumerator.Current
            {
                get { return Current; }
            }

            public bool MoveNext()
            {
                if (search_handle == IntPtr.Zero)
                {
                    return init_search_handle();
                }
                else //search already open
                {
                    return next_search();
                }
            }

            public void Reset()
            {
                throw new NotImplementedException();
            }

            #endregion
        }
    }

    public class LinkInfo
    {
        public string SubstituteName { get; set; }
        public string PrintName { get; set; }
        public NTFSlinkType LinkType { get; set; }
        public bool Relative { get; set; }

        public LinkInfo()
        {
            SubstituteName = string.Empty;
            PrintName = string.Empty;
            LinkType = NTFSlinkType.Junction;
            Relative = false;
        }

        public LinkInfo(REPARSE_DATA_BUFFER_MOUNTPOINT data)
        {
            SubstituteName = data.SubstituteName;
            PrintName = data.PrintName;
            LinkType = NTFSlinkType.Junction;
        }

        public LinkInfo(REPARSE_DATA_BUFFER_SYMLINK data)
        {
            SubstituteName = data.SubstituteName;
            PrintName = data.PrintName;
            LinkType = NTFSlinkType.Symbolic;
        }
    }
}
