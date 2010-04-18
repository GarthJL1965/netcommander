using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Net;
using System.Text.RegularExpressions;

namespace netCommander
{
    #region ftp list
    public class FtpDirectoryList : FileCollectionBase
    {
        private readonly string SORT_0_NAME = Options.GetLiteral(Options.LANG_FILE_NAME);
        private readonly string SORT_1_TIMESTAMP = Options.GetLiteral(Options.LANG_MODIFICATION_TIME);
        private readonly string SORT_2_SIZE = Options.GetLiteral(Options.LANG_FILE_SIZE);

        private string internal_directory_path;
        private FtpConnection internal_conection;
        private SortedList<FtpEntryInfo, object> internal_list;
        private IComparer<FtpEntryInfo> internal_comparer;

        private PanelCommandFtpDownload command_download = new PanelCommandFtpDownload();
        private PanelCommandFtpCreateDirectory command_create_dir = new PanelCommandFtpCreateDirectory();
        private PanelCommandFtpDelete command_delete = new PanelCommandFtpDelete();

        public FtpDirectoryList(int sort_criteria, bool sort_reverse, FtpConnection connection)
            : base(sort_criteria, sort_reverse)
        {
            internal_comparer = new InternalComparer(sort_criteria, sort_reverse);
            internal_list = new SortedList<FtpEntryInfo, object>(internal_comparer);
            internal_conection = connection;
            internal_directory_path = string.Empty;

            AvailableCommands.Add(command_download);
            AvailableCommands.Add(command_delete);
            AvailableCommands.Add(command_create_dir);

            internal_conection.ForceUpdate += new EventHandler(internal_conection_ForceUpdate);
        }

        void internal_conection_ForceUpdate(object sender, EventArgs e)
        {
            if (MainWindow == null)
            {
                return;
            }

            if (MainWindow.InvokeRequired)
            {
                MainWindow.Invoke(new System.Windows.Forms.MethodInvoker(Refill));
            }
            else
            {
                Refill();
            }
        }

        public FtpEntryInfo this[int index]
        {
            get
            {
                return internal_list.Keys[index - 1];
            }
        }

        public FtpConnection Connection
        {
            get
            {
                return internal_conection;
            }
        }

        public override ItemCategory GetItemCategory(int index)
        {
            if (index < 0)
            {
                return ItemCategory.Default;
            }

            if (index == 0)
            {
                return ItemCategory.Container;
            }

            if (this[index].Directory)
            {
                return ItemCategory.Container;
            }

            return ItemCategory.Default;
        }

        public override string GetCommandlineTextShort(int index)
        {
            string ret = string.Empty;
            FtpEntryInfo info = this[index];
            if (info.EntryName == "..")
            {
                return ret;
            }
            else
            {
                return info.EntryName;
            }
        }

        public override string GetCommandlineTextLong(int index)
        {
            string ret = string.Empty;
            FtpEntryInfo info = this[index];
            if (info.EntryName == "..")
            {
                return ret;
            }
            else
            {
                return FtpPath.Combine(info.DirectoryPath, info.EntryName);
            }
        }

        public string DirectoryPath
        {
            get
            {
                return internal_directory_path;
            }
        }

        protected override void internal_dispose()
        {
           //nothing to do
        }

        protected override void internal_sort(int criteria_index, bool reverse_order)
        {
            InternalComparer new_comparer = new InternalComparer(criteria_index, reverse_order);
            SortedList<FtpEntryInfo, object> new_list = new SortedList<FtpEntryInfo, object>(new_comparer);

            foreach (KeyValuePair<FtpEntryInfo, object> kvp in internal_list)
            {
                new_list.Add(kvp.Key, kvp.Value);
            }

            internal_list = new_list;
            internal_comparer = new_comparer;
        }

        protected override void internal_refill()
        {
            try
            {
                OnLongOperaion(Options.GetLiteral(Options.LANG_FTP_WAIT_RETRIEVE_DIRECTORY_LIST), true);
                List<FtpEntryInfo> entry_list = 
                    internal_conection.GetDirectoryDetailsList
                    (FtpPath.AppendEndingSeparator(internal_directory_path),
                    false);
                internal_list.Clear();
                foreach (FtpEntryInfo info in entry_list)
                {
                    internal_list.Add(info, null);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                OnLongOperaion(string.Empty, false);
            }
        }

        public override string[] SortCriteriaAvailable
        {
            get { return new string[] { SORT_0_NAME, SORT_1_TIMESTAMP, SORT_2_SIZE }; }
        }

        public override string GetItemDisplayName(int index)
        {
            //at index 0 - ref to parent
            if (index == 0)
            {
                return "..";
            }
            else
            {
                return internal_list.Keys[index - 1].EntryName;
            }
        }

        public override string GetItemDisplayNameLong(int index)
        {
            if (index == 0)
            {
                return Options.GetLiteral(Options.LANG_UP);
            }
            else
            {
                return GetItemDisplayName(index);
            }
        }

        public override string GetItemDisplaySummaryInfo(int index)
        {
            if (index == 0)
            {
                return "";
            }
            else
            {
                FtpEntryInfo info = internal_list.Keys[index - 1];
                if (info.Directory)
                {
                    return string.Format
                        (Options.GetLiteral(Options.LANG_DIRECTORY) + " [{0}] [{1} {2}]",
                        info.Permission,
                        info.Timestamp.ToShortDateString(),
                        info.Timestamp.ToShortTimeString());
                }
                else
                {
                    return string.Format
                        ("{0} [{1}] [{2} {3}]",
                        IOhelper.SizeToString(info.Size),
                        info.Permission,
                        info.Timestamp.ToShortDateString(),
                        info.Timestamp.ToShortTimeString());
                }
            }
        }

        public override string GetSummaryInfo()
        {
            return string.Format
                ("{0} " + Options.GetLiteral(Options.LANG_ENTRIES) + " [{1}]",
                internal_list.Count,
                internal_conection.Options.ServerName);
        }

        public override string GetSummaryInfo(int[] indices)
        {
            if (indices.Length == 0)
            {
                return GetSummaryInfo();
            }
            else
            {
                long sel_size = 0L;
                int file_count = 0;
                int dir_count = 0;

                for (int i = 0; i < indices.Length; i++)
                {
                    if (indices[i] > internal_list.Count)
                    {
                        continue;
                    }

                    sel_size += internal_list.Keys[indices[i]-1].Size;
                    if (internal_list.Keys[indices[i]-1].Directory)
                    {
                        dir_count++;
                    }
                    else
                    {
                        file_count++;
                    }
                }

                return string.Format
                    (Options.GetLiteral
                    (Options.LANG_SELECTED) + ": {0} " +
                    Options.GetLiteral(Options.LANG_DIRECTORIES) + " {1} " +
                    Options.GetLiteral(Options.LANG_FILE_VOLUME_INFORMATION) + " {2}",
                    dir_count,
                    file_count,
                    IOhelper.SizeToString(sel_size));
            }
        }

        public override int ItemCount
        {
            get { return internal_list.Count + 1; }
        }

        public override bool GetItemSelectEnable(int index)
        {
            return index != 0;
        }

        public override bool GetItemIsContainer(int index)
        {
            if (index == 0)
            {
                return true;
            }
            else
            {
                return internal_list.Keys[index - 1].Directory;
            }
        }

        

        public override void GetChildCollection
            (int index,
            ref FileCollectionBase new_collection,
            ref bool use_new,
            ref string preferred_focused_text)
        {
            string old_dir = internal_directory_path;
            string new_dir = string.Empty;

            if (index == 0)
            {
                //go up
                if (internal_directory_path == string.Empty)
                {
                    //already root, return
                    return;
                }

                //need parent dir
                //   
                //   first
                //   first/second
                //   first/second/third...

                preferred_focused_text = FtpPath.GetFile(internal_directory_path);
                new_dir = FtpPath.GetDirectory(internal_directory_path);
                internal_directory_path = new_dir;
                use_new = false;

                try
                {
                    Refill();
                }
                catch (Exception ex)
                {
                    internal_directory_path = old_dir;
                    Messages.ShowException(ex);
                }
                return;
            }
            else
            {
                //go down
                if (internal_directory_path == string.Empty)
                {
                    new_dir = internal_list.Keys[index - 1].EntryName;
                }
                else
                {
                    new_dir = FtpPath.Combine(internal_directory_path, internal_list.Keys[index - 1].EntryName);
                }
                use_new = false;
                internal_directory_path = new_dir;

                try
                {
                    Refill();
                }
                catch (Exception ex)
                {
                    internal_directory_path = old_dir;
                    Messages.ShowException(ex);
                }
                return;
            }
        }

        public override int FindIndexOfName(string name)
        {
            int ret = -1;

            for (int i = 0; i < internal_list.Count; i++)
            {
                if (internal_list.Keys[i].EntryName == name)
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
            return string.Format("{0}/{1}", internal_conection.Options.ServerName, internal_directory_path);
        }

        #region comparer
        private class InternalComparer : IComparer<FtpEntryInfo>
        {
            private int _order = 1;
            private int criteria = 0;

            public InternalComparer(int sort_criteria, bool reverse)
            {
                _order = reverse ? -1 : 1;
                criteria = sort_criteria;
            }

            #region IComparer<FtpEntryInfo> Members

            public int Compare(FtpEntryInfo x, FtpEntryInfo y)
            {
                //place directory first
                if ((x.Directory) && (!y.Directory))
                {
                    //x - directory and y not directory
                    return -1;
                }
                if ((y.Directory) && (!x.Directory))
                {
                    //y - directory and x not directory
                    return 1;
                }

                switch (criteria)
                {
                    case 0:
                        //Name
                        return string.Compare(x.EntryName, y.EntryName) * _order;

                    case 1:
                        //timestamp
                        if (x.Timestamp == y.Timestamp)
                        {
                            return string.Compare(x.EntryName, y.EntryName) * _order;
                        }
                        else
                        {
                            return DateTime.Compare(x.Timestamp, y.Timestamp) * _order;
                        }

                    case 2:
                        //size
                        long delta_size = x.Size - y.Size;
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
                            return string.Compare(x.EntryName, y.EntryName) * _order;
                        }

                    default:
                        return string.Compare(x.EntryName, y.EntryName) * _order;
                }
            }

            #endregion
        }
        #endregion
    }
    #endregion

    #region ftp entry
    public class FtpEntryInfo
    {
        /// <summary>
        /// List of REGEX formats for different FTP server listing formats
        /// </summary>
        /// <remarks>
        /// The first three are various UNIX/LINUX formats, fourth is for MS FTP
        /// in detailed mode and the last for MS FTP in 'DOS' mode.
        /// I wish VB.NET had support for Const arrays like C# but there you go
        /// </remarks>
        private static string[] FtpListParseFormats = new string[] { 
            "(?<dir>[\\-d])(?<permission>([\\-r][\\-w][\\-xs]){3})\\s+\\d+\\s+\\w+\\s+\\w+\\s+(?<size>\\d+)\\s+(?<timestamp>\\w+\\s+\\d+\\s+\\d{4})\\s+(?<name>.+)", 
            "(?<dir>[\\-d])(?<permission>([\\-r][\\-w][\\-xs]){3})\\s+\\d+\\s+\\d+\\s+(?<size>\\d+)\\s+(?<timestamp>\\w+\\s+\\d+\\s+\\d{4})\\s+(?<name>.+)", 
            "(?<dir>[\\-d])(?<permission>([\\-r][\\-w][\\-xs]){3})\\s+\\d+\\s+\\d+\\s+(?<size>\\d+)\\s+(?<timestamp>\\w+\\s+\\d+\\s+\\d{1,2}:\\d{2})\\s+(?<name>.+)", 
            "(?<dir>[\\-d])(?<permission>([\\-r][\\-w][\\-xs]){3})\\s+\\d+\\s+\\w+\\s+\\w+\\s+(?<size>\\d+)\\s+(?<timestamp>\\w+\\s+\\d+\\s+\\d{1,2}:\\d{2})\\s+(?<name>.+)", 
            "(?<dir>[\\-d])(?<permission>([\\-r][\\-w][\\-xs]){3})(\\s+)(?<size>(\\d+))(\\s+)(?<ctbit>(\\w+\\s\\w+))(\\s+)(?<size2>(\\d+))\\s+(?<timestamp>\\w+\\s+\\d+\\s+\\d{2}:\\d{2})\\s+(?<name>.+)", 
            "(?<timestamp>\\d{2}\\-\\d{2}\\-\\d{2}\\s+\\d{2}:\\d{2}[Aa|Pp][mM])\\s+(?<dir>\\<\\w+\\>){0,1}(?<size>\\d+){0,1}\\s+(?<name>.+)" };

        public string DirectoryPath { get; private set; }
        public string EntryName { get; private set; }
        public bool Directory { get; private set; }
        public string Permission { get; private set; }
        public long Size { get; private set; }
        public DateTime Timestamp { get; private set; }

        public FtpEntryInfo Clone()
        {
            FtpEntryInfo ret = new FtpEntryInfo();
            ret.Directory = Directory;
            ret.DirectoryPath = DirectoryPath;
            ret.EntryName = EntryName;
            ret.Permission = Permission;
            ret.Size = Size;
            ret.Timestamp = Timestamp;
            return ret;
        }

        public static FtpEntryInfo FromDetailedListLine(string line,string directory_path)
        {
            int try_count = FtpListParseFormats.Length;
            Match reg_result = null;

            for (int i = 0; i < try_count; i++)
            {
                reg_result = Regex.Match(line, FtpListParseFormats[i]);
                if (reg_result.Success)
                {
                    break;
                }
            }

            if (!reg_result.Success)
            {
                throw new ApplicationException(string.Format
                    ("Cannot parse line '{0}'.", line));
            }

            FtpEntryInfo ret = new FtpEntryInfo();

            //now success
            //get groups
            string dir_value = reg_result.Groups["dir"].Value;
            ret.Directory = ((dir_value != "") && (dir_value != "-"));

            ret.Permission = reg_result.Groups["permission"].Value;

            string size_value = reg_result.Groups["size"].Value;
            long size_l = 0L;
            long.TryParse(size_value, out size_l);
            ret.Size = size_l;

            string ts_value = reg_result.Groups["timestamp"].Value;
            DateTime ts_dt = new DateTime();
            DateTime.TryParse(ts_value, out ts_dt);
            ret.Timestamp = ts_dt;

            if (directory_path == FtpPath.PathSeparator)
            {
                ret.DirectoryPath = string.Empty;
            }
            else
            {
                ret.DirectoryPath = directory_path;
            }

            ret.EntryName = reg_result.Groups["name"].Value;

            return ret;
        }

        public FtpEntryInfo()
        {

        }

    }
#endregion

    #region ftp connection
    public class FtpConnectionOptions
    {
        public string ServerName { get; set; }
        public int Port { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public int Timeout { get; set; }
        public bool Passive { get; set; }
        public bool KeepAlive { get; set; }
        public bool EnableSsl { get; set; }
        public bool EnableProxy { get; set; }
        public bool Anonymous { get; set; }

        public FtpConnectionOptions()
        {
            ServerName = string.Empty;
            Port = 21;
            UserName = "anonymous";
            Password = "anonymous@";
            Timeout = 300000;
            Passive = true;
            KeepAlive = true;
            EnableSsl = false;
            EnableProxy = true;
        }

        public FtpConnectionOptions(string server_name)
            : this()
        {
            ServerName = server_name;
        }

        public string Serialize()
        {
            StringBuilder sb = new StringBuilder();
            char delim=';';

            sb.Append(ServerName);
            sb.Append(delim);
            sb.Append(Port);
            sb.Append(delim);
            sb.Append(Timeout);
            sb.Append(delim);
            sb.Append(Passive);
            sb.Append(delim);
            sb.Append(KeepAlive);
            sb.Append(delim);
            sb.Append(EnableSsl);
            sb.Append(delim);
            sb.Append(EnableProxy);
            sb.Append(delim);
            sb.Append(Anonymous);
            sb.Append(delim);
            sb.Append(UserName);

            return sb.ToString();
        }

        public static FtpConnectionOptions FromString(string inp)
        {
            string[] inps = inp.Split(new char[] { ';' });

            if (inps.Length != 9)
            {
                throw new ApplicationException("Cannot deserialize ftp settings");
            }

            FtpConnectionOptions ret = new FtpConnectionOptions();
            ret.ServerName = inps[0];
            ret.Port=int.Parse(inps[1]);
            ret.Timeout=int.Parse(inps[2]);
            ret.Passive=bool.Parse(inps[3]);
            ret.KeepAlive = bool.Parse(inps[4]);
            ret.EnableSsl = bool.Parse(inps[5]);
            ret.EnableProxy = bool.Parse(inps[6]);
            ret.Anonymous = bool.Parse(inps[7]);
            ret.UserName = inps[8];

            return ret;
        }
    }

    public class FtpConnection
    {
        private const int BUFFER_SIZE = 2048;

        public FtpConnectionOptions Options { get; private set; }
        private Dictionary<string, List<FtpEntryInfo>> internal_cache = new Dictionary<string, List<FtpEntryInfo>>();
        private object cache_lock = new object();

        public event EventHandler ForceUpdate;

        public void NotifyUpdateNeeded()
        {
            if (ForceUpdate != null)
            {
                ForceUpdate(this, new EventArgs());
            }
        }

        private bool cache_retrieve(string directory, List<FtpEntryInfo> list_to_fill)
        {
            lock (cache_lock)
            {
                if (internal_cache.ContainsKey(directory))
                {
                    list_to_fill.AddRange(internal_cache[directory]);
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }

        private void cache_update(string directory, List<FtpEntryInfo> contents)
        {
            lock (cache_lock)
            {
                List<FtpEntryInfo> cache_list = new List<FtpEntryInfo>();
                cache_list.AddRange(contents);
                internal_cache[directory] = cache_list;
            }
        }

        private void cache_remove(string directory)
        {
            directory = FtpPath.AppendEndingSeparator(directory);
            lock (cache_lock)
            {
                if (internal_cache.ContainsKey(directory))
                {
                    internal_cache.Remove(directory);
                }
            }
        }

        public void ClearCache(string directory)
        {
            cache_remove(directory);
        }

        public void ClearCache()
        {
            lock (cache_lock)
            {
                internal_cache.Clear();
            }
        }

        public FtpConnection(string server_uri)
            : this(new FtpConnectionOptions(server_uri))
        {

        }

        public FtpConnection(FtpConnectionOptions options)
        {
            Options = options;
        }

        public long GetSize(string path_name)
        {
            long ret = 0;
            FtpWebRequest req = null;
            FtpWebResponse resp = null;

            try
            {
                req = create_request(path_name);
                req.Method = WebRequestMethods.Ftp.GetFileSize;
                resp = (FtpWebResponse)req.GetResponse();
                ret = resp.ContentLength;
            }
            finally
            {
                if (resp != null)
                {
                    resp.Close();
                }
            }

            return ret;
        }

        public DateTime GetStamp(string path_name)
        {
            DateTime ret = DateTime.MinValue;
            FtpWebRequest req = null;
            FtpWebResponse resp = null;

            try
            {
                req = create_request(path_name);
                req.Method = WebRequestMethods.Ftp.GetDateTimestamp;
                resp = (FtpWebResponse)req.GetResponse();
                ret = resp.LastModified;
            }
            finally
            {
                if (resp != null)
                {
                    resp.Close();
                }
            }

            return ret;
        }

        /// <summary>
        /// path_name must be /-ended
        /// </summary>
        /// <param name="path_name"></param>
        /// <returns></returns>
        public List<string> GetDirectoryList(string path_name)
        {
            StreamReader s_reader = null;
            List<string> ret = new List<string>();
            FtpWebRequest req = null;
            FtpWebResponse resp = null;

            try
            {
                req = create_request(path_name);
                req.Method = WebRequestMethods.Ftp.ListDirectory;
                resp = (FtpWebResponse)req.GetResponse();

                s_reader = new StreamReader(resp.GetResponseStream(), Encoding.UTF8);
                string line = string.Empty;
                while ((line = s_reader.ReadLine()) != null)
                {
                    ret.Add(line);
                }
            }
            finally
            {
                if (resp != null)
                {
                    resp.Close();
                }
            }
            return ret;
        }

        public bool GetEntryInfo(string path_name, ref FtpEntryInfo info_to_fill,bool force_update)
        {
            string directory = FtpPath.GetDirectory(path_name);
            string file = FtpPath.GetFile(path_name);
            bool ret = false;

            //get parent directory contents
            List<FtpEntryInfo> dir_list = new List<FtpEntryInfo>();
            try
            {
                dir_list = GetDirectoryDetailsList(FtpPath.AppendEndingSeparator(directory), force_update);
            }
            catch { } //supress all errors

            //find entry
            foreach (FtpEntryInfo info in dir_list)
            {
                if (info.EntryName == file)
                {
                    info_to_fill = info.Clone();
                    ret = true;
                }
            }

            return ret;
        }

        /// <summary>
        /// path_name must be /-ended
        /// </summary>
        /// <param name="path_name"></param>
        /// <returns></returns>
        public List<FtpEntryInfo> GetDirectoryDetailsList(string path_name, bool force_update)
        {
            StreamReader s_reader = null;
            List<FtpEntryInfo> ret = new List<FtpEntryInfo>();
            FtpWebRequest req = null;
            FtpWebResponse resp = null;

            if (!force_update)
            {
                force_update = !cache_retrieve(path_name, ret);
            }

            if (force_update)
            {
                try
                {
                    req = create_request(path_name);
                    req.Method = WebRequestMethods.Ftp.ListDirectoryDetails;
                    resp = (FtpWebResponse)req.GetResponse();

                    s_reader = new StreamReader(resp.GetResponseStream(), Encoding.UTF8);
                    string line = string.Empty;
                    while ((line = s_reader.ReadLine()) != null)
                    {
                        FtpEntryInfo entry = FtpEntryInfo.FromDetailedListLine(line, path_name);
                        ret.Add(entry);
                    }
                    //update cache
                    cache_update(path_name, ret);
                }
                finally
                {
                    if (resp != null)
                    {
                        resp.Close();
                    }
                }
            }

            return ret;
        }

        public FtpStatusCode DeleteFile(string path_name)
        {
            FtpWebRequest req = null;
            FtpWebResponse resp = null;
            FtpStatusCode ret = FtpStatusCode.Undefined;

            try
            {
                req = create_request(path_name);
                req.Method = WebRequestMethods.Ftp.DeleteFile;
                resp = (FtpWebResponse)req.GetResponse();
                ret = resp.StatusCode;
                cache_remove(FtpPath.GetDirectory(path_name));
            }
            finally
            {
                if (resp != null)
                {
                    resp.Close();
                }
            }
            return ret;
        }

        public FtpStatusCode AppendFile(string path_name, Stream data_to_append, FtpTransferProgress callback,object state)
        {
            FtpWebRequest req = null;
            FtpWebResponse resp = null;

            try
            {
                req = create_request(path_name);
                req.Method = WebRequestMethods.Ftp.AppendFile;
                Stream req_stream = req.GetRequestStream();

                //allocate buffer
                byte[] buffer = new byte[BUFFER_SIZE];
                int bytes_readed = 0;

                //callback variables
                long bytes_transferred = 0L;
                bool abort = false;

                do
                {
                    //call callback
                    bytes_transferred += bytes_readed;
                    if (callback != null)
                    {
                        callback(bytes_transferred, ref abort, state);
                    }
                    if (abort)
                    {
                        break;
                    }
                    bytes_readed = data_to_append.Read(buffer, 0, BUFFER_SIZE);
                    req_stream.Write(buffer, 0, bytes_readed);
                } while (bytes_readed != 0);

                //get response
                req_stream.Close();
                resp = (FtpWebResponse)req.GetResponse();
                cache_remove(FtpPath.GetDirectory(path_name));
                return resp.StatusCode;
            }
            finally
            {
                if (resp != null)
                {
                    resp.Close();
                }
            }
        }

        public long DownloadFile(string path_name, Stream destination, FtpTransferProgress callback, object state, int buffer_size)
        {
            FtpWebRequest req = null;
            FtpWebResponse resp = null;
            long bytes_transferred = 0L;

            try
            {
                req = create_request(path_name);
                req.Method = WebRequestMethods.Ftp.DownloadFile;
                resp = (FtpWebResponse)req.GetResponse();
                Stream resp_stream = resp.GetResponseStream();

                //allocate buffer
                if (buffer_size == 0)
                {
                    buffer_size = BUFFER_SIZE;
                }
                byte[] buffer = new byte[buffer_size];
                int bytes_readed = 0;

                //callback variables
                
                bool abort = false;

                do
                {
                    //call callback
                    bytes_transferred += bytes_readed;
                    if (callback != null)
                    {
                        callback(bytes_transferred, ref abort, state);
                    }
                    if (abort)
                    {
                        break;
                    }
                    bytes_readed = resp_stream.Read(buffer, 0, buffer_size);
                    destination.Write(buffer, 0, bytes_readed);
                } while (bytes_readed!=0);

                resp_stream.Close();
            }
            finally
            {
                if (resp != null)
                {
                    resp.Close();
                }
            }
            return bytes_transferred;
        }

        public FtpStatusCode CreateDirectory(string path_name)
        {
            FtpWebRequest req = null;
            FtpWebResponse resp = null;

            try
            {
                req = create_request(path_name);
                req.Method = WebRequestMethods.Ftp.MakeDirectory;
                resp = (FtpWebResponse)req.GetResponse();
                cache_remove(FtpPath.GetDirectory(path_name));
                return resp.StatusCode;
            }
            finally
            {
                if (resp != null)
                {
                    resp.Close();
                }
            }
        }

        public void CreateDirectoryTree(string path_name)
        {
            //check if already exists
            FtpEntryInfo current_info = new FtpEntryInfo();
            if (GetEntryInfo(path_name, ref current_info, false))
            {
                //already exists, return
                return;
            }

            //check if parent exist
            string parent_dir = FtpPath.GetDirectory(path_name);

            if (parent_dir == path_name)
            {
                //that is path_name is root path
                return;
            }

            FtpEntryInfo parent_info = new FtpEntryInfo();
            if (GetEntryInfo(parent_dir, ref parent_info, false))
            {
                //parent exists
                if (!parent_info.Directory)
                {
                    //but it is file - abnormal
                    throw new ApplicationException
                        (string.Format
                        ("Failed create directory '{0}'. Parent entry is file.",
                        path_name));
                }
                //try create directory path_name
                CreateDirectory(path_name);
            }
            else
            {
                //parent not exists
                CreateDirectoryTree(parent_dir);
                CreateDirectory(path_name);
            }
        }

        public FtpStatusCode DeleteDirectory(string path_name)
        {
            FtpWebRequest req = null;
            FtpWebResponse resp = null;

            try
            {
                req = create_request(path_name);
                req.Method = WebRequestMethods.Ftp.RemoveDirectory;
                resp = (FtpWebResponse)req.GetResponse();
                cache_remove(FtpPath.GetDirectory(path_name));
                return resp.StatusCode;
            }
            finally
            {
                if (resp != null)
                {
                    resp.Close();
                }
            }
        }

        public FtpStatusCode Rename(string existing_path_name, string new_name)
        {
            FtpWebRequest req = null;
            FtpWebResponse resp = null;

            try
            {
                req = create_request(existing_path_name);
                req.Method = WebRequestMethods.Ftp.Rename;
                req.RenameTo = new_name;
                resp = (FtpWebResponse)req.GetResponse();
                cache_remove(FtpPath.GetDirectory(existing_path_name));
                return resp.StatusCode;
            }
            finally
            {
                if (resp != null)
                {
                    resp.Close();
                }
            }
        }

        public FtpStatusCode UploadFile
            (string destination_path_name, Stream source, FtpTransferProgress callback, object state,int buffer_size)
        {
            FtpWebRequest req = null;
            FtpWebResponse resp = null;

            try
            {
                req = create_request(destination_path_name);
                req.Method = WebRequestMethods.Ftp.UploadFile;
                Stream req_stream = req.GetRequestStream();

                //allocate buffer
                if (buffer_size == 0)
                {
                    buffer_size = BUFFER_SIZE;
                }
                byte[] buffer = new byte[buffer_size];
                int bytes_readed = 0;

                //callback variables
                long bytes_transferred = 0L;
                bool abort = false;

                do
                {
                    //call callback
                    bytes_transferred += bytes_readed;
                    if (callback != null)
                    {
                        callback(bytes_transferred, ref abort, state);
                    }
                    if (abort)
                    {
                        break;
                    }

                    bytes_readed = source.Read(buffer, 0, buffer_size);
                    req_stream.Write(buffer, 0, bytes_readed);
                } while (bytes_readed != 0);

                req_stream.Close();
                resp = (FtpWebResponse)req.GetResponse();
                //cache_remove(FtpPath.GetDirectory(destination_path_name));
                return resp.StatusCode;
            }
            finally
            {
                if (resp != null)
                {
                    resp.Close();
                }
            }
        }

        private FtpWebRequest create_request(string path_name)
        {
            Uri req_uri = new Uri
                (string.Format
                ("{0}:{1}/{2}",
                Options.ServerName.TrimEnd(new char[] { '/' }),
                Options.Port,
                path_name));

            FtpWebRequest ret = (FtpWebRequest)FtpWebRequest.Create(req_uri);
            ret.Credentials = new NetworkCredential(Options.UserName, Options.Password);
            ret.EnableSsl = Options.EnableSsl;
            ret.KeepAlive = Options.KeepAlive;
            if (Options.EnableProxy)
            {
                ret.Proxy = WebRequest.GetSystemWebProxy();
            }
            else
            {
                ret.Proxy = null;
            }
            ret.ReadWriteTimeout = Options.Timeout;
            ret.Timeout = Options.Timeout;
            ret.UsePassive = Options.Passive;

            return ret;
        }
    }

    public class FtpPath
    {
        private FtpPath()
        {

        }

        private static char[] delim = new char[] { '/' };
        public const string PathSeparator = "/";

        public static string AppendEndingSeparator(string path)
        {
            if (path.EndsWith(PathSeparator))
            {
                return path;
            }

            return path + PathSeparator;
        }

        /// <summary>
        /// path=one/two/three/for output=one/two/three
        /// path=/one/two/three/for/ output=/one/two/three
        /// path=/one output=/
        /// path=one output=[empty]
        /// path=[empty] output=[empty]
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public static string GetDirectory(string path)
        {
            if (path == "/")
            {
                return string.Empty;
            }

            //find last separator
            int last_separator_pos = path.LastIndexOf(PathSeparator);
            if (last_separator_pos == -1)
            {
                //no separotor found
                return string.Empty;
            }

            return path.Substring(0, last_separator_pos);
        }

        /// <summary>
        /// if path=/ output=[empty]
        /// else output is last path chunk without without leading and ending /
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public static string GetFile(string path)
        {
            if (path == PathSeparator)
            {
                return string.Empty;
            }

            //split on PathSeparator
            string[] splitted = path.Split(delim, StringSplitOptions.RemoveEmptyEntries);
            if (splitted.Length > 1)
            {
                return splitted[splitted.Length - 1];
            }
            else if (splitted.Length == 1)
            {
                return splitted[0];
            }
            else
            {
                return path;
            }
        }

        public static string Combine(string path1, string path2)
        {
            return Combine(new string[] { path1, path2 }, 2);
        }

        public static string Combine(string[] paths,int count)
        {
            StringBuilder ret = new StringBuilder();
            int max = Math.Min(count, paths.Length);

            if ((paths == null) || (max == 0))
            {
                return string.Empty;
            }

            if (max == 1)
            {
                return paths[0];
            }

            //add first chunk
            //without ONE ending /
            if (paths[0].EndsWith(PathSeparator))
            {
                ret.Append(paths[0], 0, paths[0].Length - 1);
            }
            else
            {
                ret.Append(paths[0]);
            }
            
            for (int i = 1; i < max; i++)
            {
                if (!paths[i].StartsWith(PathSeparator))
                {
                    ret.Append(PathSeparator);
                }
                if (paths[i].EndsWith(PathSeparator))
                {
                    ret.Append(paths[i], 0, paths[i].Length - 1);
                }
                else
                {
                    ret.Append(paths[i]);
                }
            }

            return ret.ToString();
        }
    }
    #endregion

    public delegate void FtpTransferProgress(long bytes_transferred, ref bool abort_transfer, object state);


}
