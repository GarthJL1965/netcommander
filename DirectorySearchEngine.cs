using System;
using System.Collections.Generic;
using System.Text;
using netCommander.FileSystemEx;
using System.IO;
using System.Threading;

namespace netCommander
{
    public class DirectorySearchEngine
    {
        public event SearchResultEventHandler SearchResult;

        private Thread work_thread = null;

        private DirectoryListFilter filter = null;
        private ulong file_size_min = 0;
        private ulong file_size_max = 0;

        public DirectorySearchEngine()
        {
            work_thread = new Thread(search_proc);
            work_thread.IsBackground = true;
            work_thread.Priority = ThreadPriority.BelowNormal;
            work_thread.SetApartmentState(ApartmentState.MTA);
        }

        private object active_lock = new object();
        private bool active_unsafe = false;
        public bool Active
        {
            get
            {
                bool ret = false;
                lock (active_lock)
                {
                    ret = active_unsafe;
                }
                return ret;
            }
            set
            {
                lock (active_lock)
                {
                    active_unsafe = value;
                }
            }
        }

        private object abort_lock = new object();
        private bool abort_unsafe = false;
        private bool Abort
        {
            get
            {
                bool ret = false;
                lock (abort_lock)
                {
                    ret = abort_unsafe;
                }
                return ret;
            }
            set
            {
                lock (abort_lock)
                {
                    abort_unsafe = value;
                }
            }
        }
        public void StopSearch()
        {
            Abort = true;
        }

        public void BeginSearch(DirectoryListFilter filter)
        {
            Abort = false;
            Active = true;
            this.filter = filter;
            file_size_max = (ulong)filter.SizeMaximum;
            file_size_min = (ulong)filter.SizeMinimum;
            //Thread.Sleep(100);
            work_thread.Start();
        }

        private void search_proc()
        {
            if (filter.InCurrentDrive)
            {
                search_in_directory_with_subdirs(filter.CurrentDrive);
                OnFinish();
                return;
            }

            

            if (filter.InCurrentDirectory)
            {
                if (filter.InCurrentDirectoryWithSubdirs)
                {
                    search_in_directory_with_subdirs(filter.CurrentDirectory);
                    OnFinish();
                    return;
                }
                else
                {
                    search_in_directory(filter.CurrentDirectory);
                    OnFinish();
                    return;
                }
            }

            if (filter.InFixedDrives)
            {
                search_in_fixed_drives();
            }

            if (filter.InRemovableDrives)
            {
                search_in_removable_drives();
            }

            if (filter.InNetworkDrives)
            {
                search_in_network_drives();
            }

            OnFinish();
        }

        private void search_in_fixed_drives()
        {
            DriveInfo[] dis = DriveInfo.GetDrives();
            for (int i = 0; i < dis.Length; i++)
            {
                if ((dis[i].IsReady) && (dis[i].DriveType == DriveType.Fixed))
                {
                    search_in_directory_with_subdirs(dis[i].RootDirectory.FullName);
                }
            }
        }

        private void search_in_network_drives()
        {
            DriveInfo[] dis = DriveInfo.GetDrives();
            for (int i = 0; i < dis.Length; i++)
            {
                if ((dis[i].IsReady) && (dis[i].DriveType == DriveType.Network))
                {
                    search_in_directory_with_subdirs(dis[i].RootDirectory.FullName);
                }
            }
        }

        private void search_in_removable_drives()
        {
            DriveInfo[] dis = DriveInfo.GetDrives();
            for (int i = 0; i < dis.Length; i++)
            {
                if ((dis[i].IsReady) && ((dis[i].DriveType == DriveType.CDRom) || (dis[i].DriveType == DriveType.Ram) || (dis[i].DriveType == DriveType.Removable)))
                {
                    search_in_directory_with_subdirs(dis[i].RootDirectory.FullName);
                }
            }
        }

        private void search_in_directory_with_subdirs(string dir_path)
        {
            //first find matches in current dir
            search_in_directory(dir_path);

            //check abort
            if (Abort)
            {
                return;
            }

            //see directories
            string search_path = IOhelper.GetUnicodePath(dir_path);
            search_path = search_path.TrimEnd(new char[] { Path.DirectorySeparatorChar });
            search_path = search_path + Path.DirectorySeparatorChar + "*";
            WinAPiFSwrapper.WIN32_FIND_DATA_enumerable fs_enum = new WinAPiFSwrapper.WIN32_FIND_DATA_enumerable(search_path, true);
            foreach (WIN32_FIND_DATA data in fs_enum)
            {
                //check abort
                if (Abort)
                {
                    break;
                }

                //skip some entries
                if (data.cFileName == "..")
                {
                    continue;
                }
                if (data.cFileName == ".")
                {
                    continue;
                }
                if ((data.dwFileAttributes & FileAttributes.Directory) == FileAttributes.Directory)
                {
                    //recursive call
                    search_in_directory_with_subdirs(Path.Combine(dir_path, data.cFileName));
                }
            }
        }

        private void search_in_directory(string dir_path)
        {
            OnDirectoryChange(dir_path);
            string search_mask=string.Empty;
            for (int i = 0; i < filter.Masks.Length; i++)
            {
                if (Abort)
                {
                    break;
                }
                search_mask = Path.Combine(dir_path, filter.Masks[i]);
                WinAPiFSwrapper.WIN32_FIND_DATA_enumerable file_enum = 
                    new WinAPiFSwrapper.WIN32_FIND_DATA_enumerable(search_mask, true);
                foreach (WIN32_FIND_DATA data in file_enum)
                {
                    if (Abort)
                    {
                        break;
                    }

                    //skip some entries
                    if (data.cFileName == "..")
                    {
                        continue;
                    }

                    if (data.cFileName == ".")
                    {
                        continue;
                    }

                    if (filter_match(data))
                    {
                        OnFind(dir_path, data);
                    }
                }
            }
        }

        private bool filter_match(WIN32_FIND_DATA data)
        {
            //check attributes
            if (!filter.IgnoreFileAttributes)
            {
                if ((data.dwFileAttributes & filter.FileAttributes) == 0)
                {
                    return false;
                }
            }

            //check size
            if (!filter.IgnoreSize)
            {
                ulong file_size = data.FileSize;
                switch (filter.FilterSizeCriteria)
                {
                    case FilterSizeCriteria.Between:
                        if ((file_size < file_size_min) || (file_size > file_size_max))
                        {
                            return false;
                        }
                        break;

                    case FilterSizeCriteria.Equal:
                        if (file_size != file_size_min)
                        {
                            return false;
                        }
                        break;

                    case FilterSizeCriteria.Greater:
                        if (file_size <= file_size_min)
                        {
                            return false;
                        }
                        break;

                    case FilterSizeCriteria.GreaterOrEqual:
                        if (file_size < file_size_min)
                        {
                            return false;
                        }
                        break;

                    case FilterSizeCriteria.Less:
                        if (file_size >= file_size_min)
                        {
                            return false;
                        }
                        break;

                    case FilterSizeCriteria.LessOrEqual:
                        if (file_size > file_size_min)
                        {
                            return false;
                        }
                        break;
                }
            }
            //check creation time
            if (!filter.IgnoreTimeCreate)
            {
                if ((data.ftCreationTime < filter.CreateBegin.ToFileTime()) || (data.ftCreationTime > filter.AccessEnd.ToFileTime()))
                {
                    return false;
                }
            }

            //check modification time
            if (!filter.IgnoreTimeModification)
            {
                if ((data.ftLastWriteTime < filter.ModificationBegin.ToFileTime()) || (data.ftLastWriteTime > filter.ModificationEnd.ToFileTime()))
                {
                    return false;
                }
            }

            //check access time
            if (!filter.IgnoreTimeAccess)
            {
                if ((data.ftLastAccessTime < filter.AccessBegin.ToFileTime()) || (data.ftLastAccessTime > filter.AccessEnd.ToFileTime()))
                {
                    return false;
                }
            }

            return true;

        }

        private void OnDirectoryChange(string dir_path)
        {
            if (SearchResult != null)
            {
                SearchResult(this, new SearchResultEventArgs(dir_path));
            }
        }

        private void OnFind(string dir_path,WIN32_FIND_DATA find)
        {
            if(SearchResult!=null)
            {
                SearchResult(this, new SearchResultEventArgs(dir_path, find));
            }
        }

        private void OnFinish()
        {
            if (SearchResult != null)
            {
                SearchResult(this, new SearchResultEventArgs(SearchResultReason.SearchFinish));
            }
            Active = false;
        }
    }

    public enum SearchResultReason
    {
        FindFile,
        ChangeDirectory,
        SearchFinish
    }

    public delegate void SearchResultEventHandler(object sender,SearchResultEventArgs e);
    public class SearchResultEventArgs : EventArgs
    {
        public SearchResultReason Reason { get; set; }
        public string DirectoryPath { get; set; }
        public WIN32_FIND_DATA Find { get; set; }

        public SearchResultEventArgs()
        {

        }

        public SearchResultEventArgs(SearchResultReason reason)
        {
            Reason = reason;
        }

        public SearchResultEventArgs(string dir_path)
        {
            Reason = SearchResultReason.ChangeDirectory;
            DirectoryPath = dir_path;
        }

        public SearchResultEventArgs(string dir_path,WIN32_FIND_DATA find)
        {
            Reason = SearchResultReason.FindFile;
            DirectoryPath = dir_path;
            Find = find;
        }
    }
}
