using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using netCommander.FileSystemEx;
using System.Runtime.InteropServices;

namespace netCommander
{
    class IOhelper
    {

        public const string UNICODE_PREFIX = @"\\?\";
        public const string UNICODE_PREFIX_UNC = @"\\?\UNC\";
        public const string DOS_DEVICE_PREFIX = @"\DosDevices";
        public const char FILESTREAM_DELIMITER = ':';
        public const string SMB_PREFIX = "\\";
        public const string UNPARSED_PREFIX = @"\??\";
        public const string UNPARSED_PREFIX_UNC = @"\??\UNC\";

        public static string AddUnparsedPrefix(string path)
        {
            if (path.StartsWith(UNPARSED_PREFIX) || (path.StartsWith(UNPARSED_PREFIX_UNC)))
            {
                return path;
            }

            if (path.StartsWith(SMB_PREFIX))
            {
                return UNPARSED_PREFIX_UNC + path.Remove(0, 2);
            }
            return UNPARSED_PREFIX + path;
        }

        public static uint PtrToUint(IntPtr ptr, int offset)
        {
            byte[] uint_bytes = new byte[4];
            for (int i = 0; i < 4; i++)
            {
                uint_bytes[i] = Marshal.ReadByte(ptr, offset + i);
            }
            uint ret = 0;
            ret = BitConverter.ToUInt32(uint_bytes, 0);
            return ret;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="ptr"></param>
        /// <param name="offset"></param>
        /// <param name="len">number of unicode characters</param>
        /// <returns></returns>
        public static string PtrToStringUni(IntPtr ptr, int offset, int len)
        {
            IntPtr string_ptr = IntPtr.Zero;
            if (offset == 0)
            {
                string_ptr = ptr;
            }
            else
            {
                string_ptr = new IntPtr(ptr.ToInt64() + (long)offset);
            }
            string ret = Marshal.PtrToStringUni(string_ptr, len);
            return ret;
        }

        //use only full win32 path
        public static bool PathIsFileStream(string path)
        {
            int delim_count = 0;
            for (int i = 0; i < path.Length; i++)
            {
                if (path[i] == FILESTREAM_DELIMITER)
                {
                    delim_count++;
                }
            }

            if (path.StartsWith(SMB_PREFIX))
            {
                return delim_count > 0;
            }
            else
            {
                return delim_count > 1;
            }
        }

        public static IntPtr strings_to_buffer(string param1, string param2)
        {
            string combined = string.Format("{0};{1}", param1, param2);
            IntPtr ret =System.Runtime.InteropServices.Marshal.StringToHGlobalUni(combined);
            return ret;
        }

        public static void strings_from_buffer(ref string param1, ref string param2, IntPtr buffer)
        {
            if (buffer == IntPtr.Zero)
            {
                return;
            }

            string combined = Marshal.PtrToStringUni(buffer);
            string[] splitted = combined.Split(new char[] { ';' }, 2, StringSplitOptions.RemoveEmptyEntries);
            param1 = splitted[0];
            param2 = splitted[1];
        }

        public static bool IsDirectoryEmpty(string file_name)
        {
            

            if (!IsDirectory(file_name))
            {
                return false;
            }

            string search_path=file_name;
            search_path=search_path.TrimEnd(new char[]{Path.DirectorySeparatorChar});
            search_path=search_path+Path.DirectorySeparatorChar+"*";
            WinAPiFSwrapper.WIN32_FIND_DATA_enumerable fs_enum = new WinAPiFSwrapper.WIN32_FIND_DATA_enumerable(search_path);
            int count = 0;
            foreach (WIN32_FIND_DATA data in fs_enum)
            {
                if (data.cFileName == ".")
                {
                    continue;
                }
                if (data.cFileName == "..")
                {
                    continue;
                }
                count++;
                if (count > 0)
                {
                    break;
                }
            }
            return (count == 0);
        }

        public static bool IsDirectory(string file_name)
        {
            return Directory.Exists(file_name);
        }

        public static FileSystemInfo GetFileSystemInfo(string file_name)
        {
            FileSystemInfo ret = null;
            if (File.Exists(file_name))
            {
                ret = new FileInfo(file_name);
            }
            else if (Directory.Exists(file_name))
            {
                ret = new DirectoryInfo(file_name);
            }
            return ret;
        }

        public static bool IsFileStream(string path)
        {
            string[] splitted = path.Split(new char[] { ':' });
            if (splitted.Length == 1)
            {
                return false;
            }
            if (splitted[0].Length == 1)
            {
                if (splitted.Length > 2)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else
            {
                return true;
            }

        }

        public static string GetKernelFileName(string path)
        {
            return DOS_DEVICE_PREFIX + Path.DirectorySeparatorChar + path;
        }

        public static string GetUnicodePath(string path)
        {
            string ret = string.Empty;
            if (path.StartsWith("\\"))
            {
                ret = UNICODE_PREFIX_UNC + path.Substring(2);
            }
            else
            {
                ret = UNICODE_PREFIX + path;
            }
            return ret;
        }

        private static bool ShowExactFileSize = false;
        public static string SizeToString(ulong size)
        {
            if (ShowExactFileSize)
            {
                return size.ToString("#,##0");
            }
            else
            {
                ulong bytesLen = 16384;
                ulong kiloLen = 16777216;


                if (size < bytesLen)
                {
                    return size.ToString("#,##0");
                }
                if (size < kiloLen)
                {
                    return (size / 1024).ToString("#,##0K");
                }
                else
                {
                    return (size / 1048576).ToString("#,##0M");
                }
            }
        }

        public static string SizeToString(long size)
        {
            if (ShowExactFileSize)
            {
                return size.ToString("#,##0");
            }
            else
            {
                long bytesLen = 16384;
                long kiloLen = 16777216;


                if (size < bytesLen)
                {
                    return size.ToString("#,##0");
                }
                if (size < kiloLen)
                {
                    return (size / 1024).ToString("#,##0K");
                }
                else
                {
                    return (size / 1048576).ToString("#,##0M");
                }

            }
        }

        public static string FileStreamAttributes2String(FileStreamAttributes attr)
        {
            switch (attr)
            {
                case FileStreamAttributes.CONTAINS_PROPERTIES:
                    return "Properties";

                case FileStreamAttributes.CONTAINS_SECURITY:
                    return "Security";

                case FileStreamAttributes.MODIFIED_WHEN_READ:
                    return "Modified when read";

                case FileStreamAttributes.NORMAL:
                    return "Normal";

                case FileStreamAttributes.SPARSE:
                    return "Sparse";

                default:
                    return "Unknown attribute";
            }
        }

        public static string FileStreamID2String(FileStreamID id)
        {
            switch (id)
            {
                case FileStreamID.ALTERNATE_DATA:
                    return "Alternative data";

                case FileStreamID.DATA:
                    return "Standard data";

                case FileStreamID.EA_DATA:
                    return "Extended attributes";

                case FileStreamID.INVALID:
                    return "Invalid stream";

                case FileStreamID.LINK:
                    return "Hard link info";

                case FileStreamID.OBJECT_ID:
                    return "Object identifiers";

                case FileStreamID.PROPERTY_DATA:
                    return "Property data";

                case FileStreamID.REPARSE_DATA:
                    return "Reparse point data";

                case FileStreamID.SECURITY_DATA:
                    return "Securiry data";

                case FileStreamID.SPARSE_BLOCK:
                    return "Sparse block";

                case FileStreamID.TXFS_DATA:
                    return "TXFS data";

                default:
                    return "Unknown";
            }
        }

        public static string NetserverTypeToString(NetApi.NetserverEnumType stype)
        {
            string ret = string.Empty;
            string delim = ", ";

            if ((stype & NetApi.NetserverEnumType.AFP) == NetApi.NetserverEnumType.AFP)
            {
                ret = ret + Options.GetLiteral(Options.LANG_SV_TYPE_AFP) + delim;
            }
            if ((stype & NetApi.NetserverEnumType.ALTERNATE_XPORT) == NetApi.NetserverEnumType.ALTERNATE_XPORT)
            {
                ret = ret + Options.GetLiteral(Options.LANG_SV_TYPE_ALTERNATE_XPORT) + delim;
            }
            if ((stype & NetApi.NetserverEnumType.BACKUP_BROWSER) == NetApi.NetserverEnumType.BACKUP_BROWSER)
            {
                ret = ret + Options.GetLiteral(Options.LANG_SV_TYPE_BACKUP_BROWSER) + delim;
            }
            if ((stype & NetApi.NetserverEnumType.CLUSTER_NT) == NetApi.NetserverEnumType.CLUSTER_NT)
            {
                ret = ret + Options.GetLiteral(Options.LANG_SV_TYPE_CLUSTER_NT) + delim;
            }
            if ((stype & NetApi.NetserverEnumType.CLUSTER_VS_NT) == NetApi.NetserverEnumType.CLUSTER_VS_NT)
            {
                ret = ret + Options.GetLiteral(Options.LANG_SV_TYPE_CLUSTER_VS_NT) + delim;
            }
            if ((stype & NetApi.NetserverEnumType.DCE) == NetApi.NetserverEnumType.DCE)
            {
                ret = ret + Options.GetLiteral(Options.LANG_SV_TYPE_DCE) + delim;
            }
            if ((stype & NetApi.NetserverEnumType.DFS) == NetApi.NetserverEnumType.DFS)
            {
                ret = ret + Options.GetLiteral(Options.LANG_SV_TYPE_DFS) + delim;
            }
            if ((stype & NetApi.NetserverEnumType.DIALIN_SERVER) == NetApi.NetserverEnumType.DIALIN_SERVER)
            {
                ret = ret + Options.GetLiteral(Options.LANG_SV_TYPE_DIALIN_SERVER) + delim;
            }
            if ((stype & NetApi.NetserverEnumType.DOMAIN_BAKCTRL) == NetApi.NetserverEnumType.DOMAIN_BAKCTRL)
            {
                ret = ret + Options.GetLiteral(Options.LANG_SV_TYPE_DOMAIN_BAKCTRL) + delim;
            }
            if ((stype & NetApi.NetserverEnumType.DOMAIN_CTRL) == NetApi.NetserverEnumType.DOMAIN_CTRL)
            {
                ret = ret + Options.GetLiteral(Options.LANG_SV_TYPE_DOMAIN_CTRL) + delim;
            }
            if ((stype & NetApi.NetserverEnumType.DOMAIN_ENUM) == NetApi.NetserverEnumType.DOMAIN_ENUM)
            {
                ret = ret + Options.GetLiteral(Options.LANG_SV_TYPE_DOMAIN_ENUM) + delim;
            }
            if ((stype & NetApi.NetserverEnumType.DOMAIN_MASTER) == NetApi.NetserverEnumType.DOMAIN_MASTER)
            {
                ret = ret + Options.GetLiteral(Options.LANG_SV_TYPE_DOMAIN_MASTER) + delim;
            }
            if ((stype & NetApi.NetserverEnumType.DOMAIN_MEMBER) == NetApi.NetserverEnumType.DOMAIN_MEMBER)
            {
                ret = ret + Options.GetLiteral(Options.LANG_SV_TYPE_DOMAIN_MEMBER) + delim;
            }
            if ((stype & NetApi.NetserverEnumType.LOCAL_LIST_ONLY) == NetApi.NetserverEnumType.LOCAL_LIST_ONLY)
            {
                ret = ret + Options.GetLiteral(Options.LANG_SV_TYPE_LOCAL_LIST_ONLY) + delim;
            }
            if ((stype & NetApi.NetserverEnumType.MASTER_BROWSER) == NetApi.NetserverEnumType.MASTER_BROWSER)
            {
                ret = ret + Options.GetLiteral(Options.LANG_SV_TYPE_MASTER_BROWSER) + delim;
            }
            if ((stype & NetApi.NetserverEnumType.NOVELL) == NetApi.NetserverEnumType.NOVELL)
            {
                ret = ret + Options.GetLiteral(Options.LANG_SV_TYPE_NOVELL) + delim;
            }
            if ((stype & NetApi.NetserverEnumType.NT) == NetApi.NetserverEnumType.NT)
            {
                ret = ret + Options.GetLiteral(Options.LANG_SV_TYPE_NT) + delim;
            }
            if ((stype & NetApi.NetserverEnumType.OSF) == NetApi.NetserverEnumType.OSF)
            {
                ret = ret + Options.GetLiteral(Options.LANG_SV_TYPE_OSF) + delim;
            }
            if ((stype & NetApi.NetserverEnumType.POTENTIAL_BROWSER) == NetApi.NetserverEnumType.POTENTIAL_BROWSER)
            {
                ret = ret + Options.GetLiteral(Options.LANG_SV_TYPE_POTENTIAL_BROWSER) + delim;
            }
            if ((stype & NetApi.NetserverEnumType.PRINTQ_SERVER) == NetApi.NetserverEnumType.PRINTQ_SERVER)
            {
                ret = ret + Options.GetLiteral(Options.LANG_SV_TYPE_PRINTQ_SERVER) + delim;
            }
            if ((stype & NetApi.NetserverEnumType.SERVER) == NetApi.NetserverEnumType.SERVER)
            {
                ret = ret + Options.GetLiteral(Options.LANG_SV_TYPE_SERVER) + delim;
            }
            if ((stype & NetApi.NetserverEnumType.SERVER_MFPN) == NetApi.NetserverEnumType.SERVER_MFPN)
            {
                ret = ret + Options.GetLiteral(Options.LANG_SV_TYPE_SERVER_MFPN) + delim;
            }
            if ((stype & NetApi.NetserverEnumType.SERVER_NT) == NetApi.NetserverEnumType.SERVER_NT)
            {
                ret = ret + Options.GetLiteral(Options.LANG_SV_TYPE_SERVER_NT) + delim;
            }
            if ((stype & NetApi.NetserverEnumType.SQLSERVER) == NetApi.NetserverEnumType.SQLSERVER)
            {
                ret = ret + Options.GetLiteral(Options.LANG_SV_TYPE_SQLSERVER) + delim;
            }
            if ((stype & NetApi.NetserverEnumType.TERMINALSERVER) == NetApi.NetserverEnumType.TERMINALSERVER)
            {
                ret = ret + Options.GetLiteral(Options.LANG_SV_TYPE_TERMINALSERVER) + delim;
            }
            if ((stype & NetApi.NetserverEnumType.TIME_SOURCE) == NetApi.NetserverEnumType.TIME_SOURCE)
            {
                ret = ret + Options.GetLiteral(Options.LANG_SV_TYPE_TIME_SOURCE) + delim;
            }
            if ((stype & NetApi.NetserverEnumType.VMS) == NetApi.NetserverEnumType.VMS)
            {
                ret = ret + Options.GetLiteral(Options.LANG_SV_TYPE_VMS) + delim;
            }
            if ((stype & NetApi.NetserverEnumType.WFW) == NetApi.NetserverEnumType.WFW)
            {
                ret = ret + Options.GetLiteral(Options.LANG_SV_TYPE_WFW) + delim;
            }
            if ((stype & NetApi.NetserverEnumType.WINDOWS) == NetApi.NetserverEnumType.WINDOWS)
            {
                ret = ret + Options.GetLiteral(Options.LANG_SV_TYPE_WINDOWS) + delim;
            }
            if ((stype & NetApi.NetserverEnumType.WORKSTATION) == NetApi.NetserverEnumType.WORKSTATION)
            {
                ret = ret + Options.GetLiteral(Options.LANG_SV_TYPE_WORKSTATION) + delim;
            }
            if ((stype & NetApi.NetserverEnumType.XENIX_SERVER) == NetApi.NetserverEnumType.XENIX_SERVER)
            {
                ret = ret + Options.GetLiteral(Options.LANG_SV_TYPE_XENIX_SERVER) + delim;
            }

            if (ret.EndsWith(delim))
            {
                ret = ret.Substring(0, ret.Length - 2);
            }

            return ret;
        }

        public static string FileAttributes2String(FileAttributes attr)
        {
            StringBuilder retB = new StringBuilder();

            if ((attr & FileAttributes.Archive) == FileAttributes.Archive)
            {
                retB.Append('A');
            }
            if ((attr & FileAttributes.Compressed) == FileAttributes.Compressed)
            {
                retB.Append('C');
            }
            if ((attr & FileAttributes.Encrypted) == FileAttributes.Encrypted)
            {
                retB.Append('E');
            }
            if ((attr & FileAttributes.Hidden) == FileAttributes.Hidden)
            {
                retB.Append('H');
            }
            if ((attr & FileAttributes.ReadOnly) == FileAttributes.ReadOnly)
            {
                retB.Append('R');
            }
            if ((attr & FileAttributes.ReparsePoint) == FileAttributes.ReparsePoint)
            {
                retB.Append('P');
            }
            if ((attr & FileAttributes.SparseFile) == FileAttributes.SparseFile)
            {
                retB.Append('F');
            }
            if ((attr & FileAttributes.System) == FileAttributes.System)
            {
                retB.Append('S');
            }
            if ((attr & FileAttributes.Temporary) == FileAttributes.Temporary)
            {
                retB.Append('T');
            }

            if (retB.Length == 0)
            {
                retB.Append('N');
            }


            return retB.ToString();
        }

        public static DriveInfo GetFirstDrive()
        {
            DriveInfo ret = null;
            DriveInfo[] infos = DriveInfo.GetDrives();
            if (infos.Length == 0)
            {
                return ret;
            }
            for (int i = 0; i < infos.Length; i++)
            {
                if (infos[i].DriveType == DriveType.Fixed)
                {
                    ret = infos[i];
                    break;
                }
            }
            if (ret == null)
            {
                ret = infos[0];
            }
            return ret;
        }

        public static string Flag2String_uint(object value, Type flag_type)
        {
            string ret = string.Empty;
            Array val_array = Enum.GetValues(flag_type);
            uint one_val_uint = 0;
            uint value_uint = (uint)value;

            uint disp_value_uint = 0;

            foreach(object one_val in val_array)
            {
                one_val_uint = (uint)one_val;
                if ((one_val_uint & value_uint) == one_val_uint)
                {
                    disp_value_uint = disp_value_uint | one_val_uint;
                    ret = ret + Enum.GetName(flag_type, one_val) + ", ";
                }
            }

            uint ostatok = disp_value_uint ^ value_uint;

            if (ret.EndsWith(", "))
            {
                ret = ret.Substring(0, ret.Length - 2);
            }

            return ret;
        }

        //public static IFileListProvider ParsePathToProvider(string inp)
        //{
        //    if (inp == @"\\")
        //    {
        //        //return netapi root brower
        //        return new NetapiServersList(SortFileType.None, SortFileOptions.None);
        //    }
        //    else if (inp.StartsWith(@"\\"))
        //    {
        //        //net path:
        //        string[] inp_split = inp.Split(new char[] { '\\' }, StringSplitOptions.RemoveEmptyEntries);
        //        if (inp_split.Length == 1)
        //        {
        //            return new NetapiShareList(inp_split[0], SortFileType.None, SortFileOptions.None);
        //        }
        //        else
        //        {
        //            return new FileListDirEx(inp, SortFileType.None, SortFileOptions.None);
        //        }
        //    }
        //    else
        //    {
        //        return new FileListDirEx(inp, SortFileType.None, SortFileOptions.None);
        //    }
        //}
    }
}
