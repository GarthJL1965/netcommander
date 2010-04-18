using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using ICSharpCode.SharpZipLib.Zip;
using ICSharpCode.SharpZipLib;

namespace netCommander
{
    public class ZipDirectory : FileCollectionBase
    {
        private string zip_file_name = string.Empty;
        private string zip_virtual_dir = string.Empty;
        private SortedList<string, object> internal_list = null;
        private InternalComparer internal_comparer = null;
        private ZipFile zip_file = null;
        private bool lock_unsafe = false;

        private readonly string SORT_NAME = Options.GetLiteral(Options.LANG_FILE_NAME);
        private readonly string SORT_SIZE = Options.GetLiteral(Options.LANG_FILE_SIZE);
        private readonly string SORT_DATE = Options.GetLiteral(Options.LANG_DATETIME);

        private PanelCommandExtractZipEntries command_extract = new PanelCommandExtractZipEntries();
        private PanelComandCreateDirectory command_create_dir = new PanelComandCreateDirectory();
        private PanelCommandDeleteZipEntry command_del = new PanelCommandDeleteZipEntry();

        public ZipDirectory(string file_name,int sort_criteria, bool reverse)
            : base(sort_criteria, reverse)
        {
            zip_file_name = file_name;
            zip_virtual_dir = string.Empty;
            if (zip_file == null)
            {
                zip_file = new ZipFile(zip_file_name);
                zip_file.KeysRequired = new ZipFile.KeysRequiredEventHandler(keys_required_callback);
            }
            internal_comparer = new InternalComparer(sort_criteria, reverse, zip_file);
            internal_list = new SortedList<string, object>(internal_comparer);

            AvailableCommands.Add(command_extract);
            AvailableCommands.Add(command_create_dir);
            AvailableCommands.Add(command_del);
        }

        private string last_pass = string.Empty;
        private void keys_required_callback(Object sender, KeysRequiredEventArgs e)
        {


            if (last_pass == string.Empty)
            {
                if (Messages.AskCredentials
                    (Options.GetLiteral(Options.LANG_ARCHIVE_PASS_NEEDED),
                    e.FileName,
                    ref last_pass) != System.Windows.Forms.DialogResult.OK)
                {
                    return;
                }
            }

            e.Key = ICSharpCode.SharpZipLib.Encryption.PkzipClassic.GenerateKeys(ZipConstants.ConvertToArray(last_pass));
            
        }

        public ZipFile ZipFile
        {
            get
            {
                return zip_file;
            }
        }

        private object lock_lock = new object();
        public bool LockSafe
        {
            get
            {
                bool ret = false;
                lock (lock_lock)
                {
                    ret = lock_unsafe;
                }
                return ret;
            }
            set
            {
                lock (lock_lock)
                {
                    lock_unsafe = value;
                }
            }
        }

        public ZipEntry this[int index]
        {
            get
            {
                return zip_file.GetEntry(internal_list.Keys[index]);
            }
        }

        public string CurrentZipDirectory
        {
            get
            {
                return zip_virtual_dir;
            }
        }



        protected override void internal_dispose()
        {
            if ((zip_file != null) && (!LockSafe))
            {
                zip_file.Close();
            }
        }

        protected override void internal_sort(int criteria_index, bool reverse_order)
        {
            InternalComparer new_comparer = new InternalComparer(criteria_index, reverse_order,zip_file);
            SortedList<string, object> new_list = new SortedList<string, object>(new_comparer);

            foreach (KeyValuePair<string,object> list_entry in internal_list)
            {
                new_list.Add(list_entry.Key, null);
            }

            internal_list = new_list;
            internal_comparer = new_comparer;
        }

        

        public static List<string> GetDirectoryContent_1(ZipFile zip_file, string dir)
        {
            char path_separator = '/';
            char[] path_separator_param = new char[] { path_separator };
            List<string> ret = new List<string>();
            foreach (ZipEntry entry in zip_file)
            {
                if ((dir.Length != 0)&&(!dir.EndsWith("/")))
                {
                    dir = dir + path_separator;
                }

                if (entry.Name.Length < dir.Length)
                {
                    //skip short entries
                    continue;
                }

                if (!entry.Name.StartsWith(dir))
                {
                    //skip entries to other dirs
                    continue;
                }

                string name_without_dir = entry.Name.Substring(dir.Length);
                string[] name_without_dir_splitted = name_without_dir.Split(path_separator_param, StringSplitOptions.RemoveEmptyEntries);
                if ((name_without_dir_splitted.Length == 1) && (!entry.Name.EndsWith("/")))
                {
                    //it is file or emtry dir
                    ret.Add(entry.Name);
                    continue;
                }

                if (name_without_dir_splitted.Length == 0)
                {
                    continue;
                }

                //it is dir
                //take first path chunk
                string name_trimmed = dir + name_without_dir_splitted[0];
                if (ret.Contains(name_trimmed))
                {
                    //already exists
                    continue;
                }
                ret.Add(name_trimmed);
            }

            return ret;
        }

        private List<string> get_directory_content(string dir)
        {
            return GetDirectoryContent_1(zip_file, dir);
        }
   
        protected override void internal_refill()
        {
            try
            {
                

                internal_list.Clear();

                //add parent ref
                string parent_ref = "..";
                internal_list.Add(parent_ref, null);

                List<string> dir_content = get_directory_content(zip_virtual_dir);
                foreach (string name in dir_content)
                {
                    internal_list.Add(name, null);
                }
               
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public override string[] SortCriteriaAvailable
        {
            get { return new string[] { SORT_NAME, SORT_SIZE, SORT_DATE }; }
        }

        public override string GetItemDisplayName(int index)
        {
            string name = internal_list.Keys[index];

            if (name == "..")
            {
                return name;
            }

            return Path.GetFileName(name);
        }

        public override string GetItemDisplayNameLong(int index)
        {
            return GetItemDisplayName(index);
        }

        public override string GetItemDisplaySummaryInfo(int index)
        {
            string name = internal_list.Keys[index];

            if (name == "..")
            {
                return Options.GetLiteral(Options.LANG_UP);
            }

            
            ZipEntry entry = zip_file.GetEntry(name);
            if (entry == null)
            {
                //try to get slash-ended entry
                entry = zip_file.GetEntry(name + "/");

                if (entry == null)
                {
                    //this is fake dir
                    return "[" + Options.GetLiteral(Options.LANG_DIRECTORY) + "]";
                }
            }


            FileAttributes fa = FileAttributes.Normal;
            if (entry.IsDOSEntry)
            {
                try
                {
                    fa = (FileAttributes)entry.ExternalFileAttributes;
                }
                catch (Exception) { }
            }

            if (entry.IsDirectory)
            {
                return string.Format
                    ("[" + Options.GetLiteral(Options.LANG_DIRECTORY) + "] [{0}] [{1} {2}]",
                    IOhelper.FileAttributes2String(fa),
                    entry.DateTime.ToShortDateString(),
                    entry.DateTime.ToShortTimeString());
            }
            else
            {
                return string.Format
                    ("{0} [{1}] [{2}] [{3} {4}]",
                    IOhelper.SizeToString(entry.Size),
                    IOhelper.SizeToString(entry.CompressedSize),
                    IOhelper.FileAttributes2String(fa),
                    entry.DateTime.ToShortDateString(),
                    entry.DateTime.ToShortTimeString());
            }
        }

        public override string GetSummaryInfo()
        {
            return zip_file_name;
        }

        public override string GetSummaryInfo(int[] indices)
        {
            return Options.GetLiteral(Options.LANG_SELECTED) + " " + indices.Length.ToString() + " " + Options.GetLiteral(Options.LANG_ENTRIES);
        }

        public override int ItemCount
        {
            get { return internal_list.Count; }
        }

        public override bool GetItemSelectEnable(int index)
        {
            return internal_list.Keys[index] != "..";
        }

        public override ItemCategory GetItemCategory(int index)
        {
            if ((index < 0) || (index >= ItemCount))
            {
                return ItemCategory.Default;
            }

            string name = internal_list.Keys[index];
            ZipEntry entry = zip_file.GetEntry(name);

            if (entry == null)
            {
                return ItemCategory.Container;
            }

            if (entry.IsDirectory)
            {
                return ItemCategory.Container;
            }

            return ItemCategory.Default;
        }

        public override bool GetItemIsContainer(int index)
        {
            string name = internal_list.Keys[index];   

            if (name == "..")
            {
                return true;
            }

            ZipEntry entry = zip_file.GetEntry(name);
            if (entry == null)
            {
                //fake dir
                return true;
            }

            return entry.IsDirectory;
        }

        

        public override void GetChildCollection
            (int index,
            ref FileCollectionBase new_collection,
            ref bool use_new,
            ref string preferred_focused_text)
        {
            if (LockSafe)
            {
                Messages.ShowMessage("Cannot change directory now.");
                return;
            }

            if (!GetItemIsContainer(index))
            {
                use_new = false;
                return;
            }

            //directory to go to..
            string target_dir = internal_list.Keys[index];
            string old_virt_dir = string.Empty;

            if (target_dir == "..")
            {
                //up
                if (zip_virtual_dir == string.Empty)
                {
                    //go to directory list
                    try
                    {
                        new_collection = new DirectoryList(0, false, Path.GetDirectoryName(zip_file_name));
                        preferred_focused_text = Path.GetFileName(zip_file_name);
                        use_new = true;
                        new_collection.MainWindow = (mainForm)Program.MainWindow;
                        new_collection.Refill();
                    }
                    catch (Exception ex)
                    {
                        Messages.ShowException(ex);
                    }
                    return;
                }
                else
                {
                    //up
                    
                    old_virt_dir = zip_virtual_dir;
                    zip_virtual_dir = FtpPath.GetDirectory(zip_virtual_dir);
                    preferred_focused_text = old_virt_dir;
                    try
                    {
                        Refill();
                    }
                    catch (Exception ex)
                    {
                        zip_virtual_dir = old_virt_dir;
                        Messages.ShowException(ex);
                    }
                    return;
                }
            }
            else
            {
                //go to down
                
                old_virt_dir = zip_virtual_dir;
                
                    //down
                    zip_virtual_dir = target_dir;
                
                try
                {
                    Refill();
                }
                catch (Exception ex)
                {
                    zip_virtual_dir = old_virt_dir;
                    Messages.ShowException(ex);
                }
                return;
            }
            
        }

        public override int FindIndexOfName(string name)
        {
            int ret = -1;
            for (int i = 0; i < ItemCount; i++)
            {
                if (internal_list.Keys[i] == name)
                {
                    ret = i;
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
            return string.Format
                ("ZIP {0}:{1}",
                zip_file_name,
                zip_virtual_dir);
        }

        public override string GetCommandlineTextShort(int index)
        {
            return GetItemDisplayName(index);
        }

        public override string GetCommandlineTextLong(int index)
        {
            return GetItemDisplayNameLong(index);
        }

        #region comparer
        private class InternalComparer : IComparer<string>
        {
            private int sort_criteria = 0;
            private int sort_order = 1;
            private ZipFile zip_file = null;

            public InternalComparer(int sort_criteria, bool sort_reverse,ZipFile zip_file)
            {
                this.sort_criteria = sort_criteria;
                this.sort_order = sort_reverse ? -1 : 1;
                this.zip_file = zip_file;
            }

            #region IComparer<string> Members

            public int Compare(string x, string y)
            {
                //place .. first
                if (x.EndsWith(".."))
                {
                    return -1;
                }
                if (y.EndsWith(".."))
                {
                    return 1;
                }

                ZipEntry x_entry = zip_file.GetEntry(x);
                ZipEntry y_entry = zip_file.GetEntry(y);

                //place directory first
                if (((x_entry == null) || (x_entry.IsDirectory)) && ((y_entry != null) && (!y_entry.IsDirectory)))
                {
                    //x - directory and y not directory
                    return -1;
                }
                if (((y_entry == null) || (y_entry.IsDirectory)) && ((x_entry != null) && (!x_entry.IsDirectory)))
                {
                    //y - directory and x not directory
                    return 1;
                }

                switch (sort_criteria)
                {
                    case 0:
                        //name
                        return string.Compare(x, y) * sort_order;

                    case 1:
                        //size

                        long x_size = x_entry == null ? 0 : x_entry.Size;
                        long y_size = y_entry == null ? 0 : y_entry.Size;

                        long delta_size = x_size - y_size;
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
                            return string.Compare(x, y) * sort_order;
                        }

                    case 2:
                        //date
                        DateTime x_date = x_entry == null ? DateTime.MinValue : x_entry.DateTime;
                        DateTime y_date = y_entry == null ? DateTime.MinValue : x_entry.DateTime;

                        return DateTime.Compare(x_date, y_date) * sort_order;

                    default:
                        return string.Compare(x, y) * sort_order;
                }
            }


            
            #endregion
        }
        #endregion
    }

    
}
