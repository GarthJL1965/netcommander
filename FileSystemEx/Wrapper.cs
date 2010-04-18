using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Runtime.InteropServices;
using System.ComponentModel;
using Microsoft.Win32.SafeHandles;

namespace netCommander.FileSystemEx
{
    //public delegate FileTransferResult CopyFileDelegate(CopyFileItemEx item, CopyProgressRoutine callback);

    public delegate void DirectoryStatChunkComplete(string completed_dir,int file_count,int dir_count,ulong size);

    class WinAPiFSwrapper
    {
        private WinAPiFSwrapper()
        {

        }

        public static void ShellFileOperation
            (System.Windows.Forms.Form owner,
            FO_Func func,
            IEnumerable<string> from,
            IEnumerable<string> to,
            FO_Flags options)
        {
            SHFILEOPSTRUCT sh_struct = new SHFILEOPSTRUCT();

            sh_struct.fFlags = options;
            sh_struct.hNameMappings = IntPtr.Zero;
            sh_struct.hwnd = owner == null ? IntPtr.Zero : owner.Handle;
            sh_struct.lpszProgressTitle = string.Empty;
            sh_struct.pFrom = from == null ? IntPtr.Zero : Strings2PtrUni(from);
            sh_struct.pTo = to == null ? IntPtr.Zero : Strings2PtrUni(to);
            sh_struct.wFunc = func;

            try
            {
                int res = WinApiFS.SHFileOperation(ref sh_struct);
                if (res != 0)
                {
                    res = SHErrorToWinError(res);
                    System.ComponentModel.Win32Exception ex = new Win32Exception(res);
                    throw ex;
                }
            }
            finally
            {
                if (sh_struct.pFrom != IntPtr.Zero)
                {
                    Marshal.FreeHGlobal(sh_struct.pFrom);
                }
                if (sh_struct.pTo != IntPtr.Zero)
                {
                    Marshal.FreeHGlobal(sh_struct.pTo);
                }
            }


        }

        private static IntPtr Strings2PtrUni(IEnumerable<string> strings)
        {
            IntPtr ret = IntPtr.Zero;

            //составим строку
            StringBuilder sb = new StringBuilder();

            foreach (string one_string in strings)
            {
                sb.Append(one_string);
                //добавляем null char
                sb.Append('\0');
            }

            //в коце добаавляем еще одтн null char
            sb.Append('\0');

            string combined = sb.ToString();

            ret = Marshal.StringToHGlobalUni(combined);

            return ret;
        }

        /// <summary>
        /// code from unicode_far
        /// </summary>
        /// <param name="SHError"></param>
        /// <returns></returns>
        private static int SHErrorToWinError(int SHError)
        {
            int WinError = SHError;

            switch (SHError)
            {
                case 0x71:
                    WinError =  WinApiFS.ERROR_ALREADY_EXISTS;
                    break; // DE_SAMEFILE         The source and destination files are the same file.
                case 0x72:
                    WinError = WinApiFS.ERROR_INVALID_PARAMETER;
                    break;// DE_MANYSRC1DEST     Multiple file paths were specified in the source buffer, but only one destination file path.
                case 0x73:
                    WinError = WinApiFS.ERROR_NOT_SAME_DEVICE; break; // DE_DIFFDIR          Rename operation was specified but the destination path is a different directory. Use the move operation instead.
                case 0x74:
                    WinError = WinApiFS.ERROR_ACCESS_DENIED; break; // DE_ROOTDIR          The source is a root directory, which cannot be moved or renamed.
                case 0x75:
                    WinError = WinApiFS.ERROR_CANCELLED; break; // DE_OPCANCELLED      The operation was cancelled by the user, or silently cancelled if the appropriate flags were supplied to SHFileOperation.
                case 0x76:
                    WinError = WinApiFS.ERROR_BAD_PATHNAME; break; // DE_DESTSUBTREE      The destination is a subtree of the source.
                case 0x78:
                    WinError = WinApiFS.ERROR_ACCESS_DENIED; break; // DE_ACCESSDENIEDSRC  Security settings denied access to the source.
                case 0x79:
                    WinError = WinApiFS.ERROR_BUFFER_OVERFLOW; break; // DE_PATHTOODEEP      The source or destination path exceeded or would exceed MAX_PATH.
                case 0x7A:
                    WinError = WinApiFS.ERROR_INVALID_PARAMETER; break; // DE_MANYDEST         The operation involved multiple destination paths, which can fail in the case of a move operation.
                case 0x7C:
                    WinError = WinApiFS.ERROR_BAD_PATHNAME; break; // DE_INVALIDFILES     The path in the source or destination or both was invalid.
                case 0x7D:
                    WinError = WinApiFS.ERROR_INVALID_PARAMETER; break; // DE_DESTSAMETREE     The source and destination have the same parent folder.
                case 0x7E:
                    WinError = WinApiFS.ERROR_ALREADY_EXISTS; break; // DE_FLDDESTISFILE    The destination path is an existing file.
                case 0x80:
                    WinError = WinApiFS.ERROR_ALREADY_EXISTS; break; // DE_FILEDESTISFLD    The destination path is an existing folder.
                case 0x81:
                    WinError = WinApiFS.ERROR_BUFFER_OVERFLOW; break; // DE_FILENAMETOOLONG  The name of the file exceeds MAX_PATH.
                case 0x82:
                    WinError = WinApiFS.ERROR_WRITE_FAULT; break; // DE_DEST_IS_CDROM    The destination is a read-only CD-ROM, possibly unformatted.
                case 0x83:
                    WinError = WinApiFS.ERROR_WRITE_FAULT; break; // DE_DEST_IS_DVD      The destination is a read-only DVD, possibly unformatted.
                case 0x84:
                    WinError = WinApiFS.ERROR_WRITE_FAULT; break; // DE_DEST_IS_CDRECORD The destination is a writable CD-ROM, possibly unformatted.
                case 0x85:
                    WinError = WinApiFS.ERROR_DISK_FULL; break; // DE_FILE_TOO_LARGE   The file involved in the operation is too large for the destination media or file system.
                case 0x86:
                    WinError = WinApiFS.ERROR_READ_FAULT; break; // DE_SRC_IS_CDROM     The source is a read-only CD-ROM, possibly unformatted.
                case 0x87:
                    WinError = WinApiFS.ERROR_READ_FAULT; break; // DE_SRC_IS_DVD       The source is a read-only DVD, possibly unformatted.
                case 0x88:
                    WinError = WinApiFS.ERROR_READ_FAULT; break; // DE_SRC_IS_CDRECORD  The source is a writable CD-ROM, possibly unformatted.
                case 0xB7:
                    WinError = WinApiFS.ERROR_BUFFER_OVERFLOW; break; // DE_ERROR_MAX        MAX_PATH was exceeded during the operation.
                case 0x402:
                    WinError = WinApiFS.ERROR_PATH_NOT_FOUND; break; //                     An unknown error occurred. This is typically due to an invalid path in the source or destination. This error does not occur on Windows Vista and later.
                case 0x10000:
                    WinError = WinApiFS.ERROR_GEN_FAILURE; break; // ERRORONDEST         An unspecified error occurred on the destination.
            }

            return WinError;
        }

        /// <summary>
        /// file_name must be full path
        /// </summary>
        /// <param name="file_name"></param>
        /// <returns></returns>
        public static VolumeSpaceInfo GetVolumeSpaceInfo(string file_name)
        {
            VolumeSpaceInfo ret = new VolumeSpaceInfo();

            //is directory?
            bool is_dir = IOhelper.IsDirectory(file_name);

            IntPtr file_handle = IntPtr.Zero;

            //first try to get space info from nt call
            try
            {
                file_handle = WinAPiFSwrapper.CreateFileHandle
                        (file_name,
                        Win32FileAccess.READ_ATTRIBUTES | Win32FileAccess.READ_EA | Win32FileAccess.LIST_DIRECTORY,
                        FileShare.ReadWrite | FileShare.Delete,
                        FileMode.Open,
                        is_dir ? CreateFileOptions.BACKUP_SEMANTICS : CreateFileOptions.None);
                FILE_FS_FULLSIZE_INFORMATION vol_fs = ntApiFSwrapper.GetFileVolumeFullsizeInfo(file_handle);
                ret.FreeBytesAvailable = (ulong)vol_fs.UserAvailableAllocationBytes;
                ret.TotalNumberOfBytes = (ulong)vol_fs.TotalAllocationUserBytes;
                ret.TotalNumberOfFreeBytes = (ulong)vol_fs.TotalAvailableAllocationBytes;
                return ret;
            }
            catch (Exception) { }
            finally
            {
                if ((file_handle.ToInt32() != WinApiFS.INVALID_HANDLE_VALUE) && (file_handle != IntPtr.Zero))
                {
                    WinApiFS.CloseHandle(file_handle);
                }
            }

            //if nt call not success, try create DriveInfo from root path
            try
            {
                string root_path = Path.GetPathRoot(file_name);
                if ((root_path == null) || (root_path == string.Empty))
                {
                    root_path = file_name;
                }
                DriveInfo di = new DriveInfo(root_path);
                ret.FreeBytesAvailable = (ulong)di.AvailableFreeSpace;
                ret.TotalNumberOfBytes = (ulong)di.TotalSize;
                ret.TotalNumberOfFreeBytes = (ulong)di.TotalFreeSpace;
                return ret;
            }
            catch (Exception) { }

            //and return empty ret if DriveInfo creating failed
            return ret;
        }

        public static bool SetBackupRestorePrivileges(bool isWrite)
        {
            bool success;
            int lastError;
            Win32Exception win_ex = null;
            // Apparently we need to have backup privileges
            IntPtr token;
            TOKEN_PRIVILEGES_ONE tokenPrivileges = new TOKEN_PRIVILEGES_ONE();
            tokenPrivileges.Privileges = new LUID_AND_ATTRIBUTES[1];
            success = WinApiFS.OpenProcessToken(WinApiFS.GetCurrentProcess(), WinApiFS.TOKEN_ADJUST_PRIVILEGES, out token);
            lastError = Marshal.GetLastWin32Error();
            if (success)
            {
                success = WinApiFS.LookupPrivilegeValue
                    (null, isWrite ? WinApiFS.SE_BACKUP_NAME : WinApiFS.SE_RESTORE_NAME, out tokenPrivileges.Privileges[0].Luid);
                // null for local system
                lastError = Marshal.GetLastWin32Error();
                if (success)
                {
                    tokenPrivileges.PrivilegeCount = 1;
                    tokenPrivileges.Privileges[0].Attributes = WinApiFS.SE_PRIVILEGE_ENABLED;
                    success = WinApiFS.AdjustTokenPrivileges
                        (token, false, ref tokenPrivileges, Marshal.SizeOf(tokenPrivileges), IntPtr.Zero, IntPtr.Zero);
                    lastError = Marshal.GetLastWin32Error();
                }
                WinApiFS.CloseHandle(token);
            }

            win_ex = new Win32Exception(lastError);

            return success;
        }

        public static void DeleteSymbolicLink(IntPtr link_handle)
        {
            REPARSE_DATA_BUFFER_SYMLINK rep = REPARSE_DATA_BUFFER_SYMLINK.Init();
            rep.ReparseTag = WinApiFS.IO_REPARSE_TAG_SYMLINK;
            rep.SetNames(string.Empty, string.Empty);
            rep.PrintNameLength = 0;
            rep.PrintNameOffset = 0;
            rep.ReparseDataLength = 0;
            rep.SubstituteNameLength = 0;
            rep.SubstituteNameOffset = 0;

            int data_size = ntApiFS.REPARSE_DATA_BUFFER_HEADER_SIZE + rep.ReparseDataLength;
            int bytes_returned = 0;

            int res = WinApiFS.DeviceIoControl
                (link_handle,
                WinApiFS.FSCTL_DELETE_REPARSE_POINT,
                ref rep,
                data_size,
                IntPtr.Zero,
                0,
                ref bytes_returned,
                IntPtr.Zero);
            if (res == 0)
            {
                int win_err = Marshal.GetLastWin32Error();
                throw new Win32Exception(win_err);
            }
        }

        public static void DeleteSymbolicLink(string link_name)
        {
            IntPtr link_handle = IntPtr.Zero;

            try
            {
                link_handle = WinAPiFSwrapper.CreateFileHandle
                    (link_name,
                    Win32FileAccess.WRITE_ATTRIBUTES | Win32FileAccess.WRITE_EA,
                    FileShare.ReadWrite | FileShare.Delete,
                    FileMode.Open,
                    CreateFileOptions.BACKUP_SEMANTICS | CreateFileOptions.OPEN_REPARSE_POINT);
                DeleteMountpoint(link_handle);
            }
            finally
            {
                if ((link_handle.ToInt32() != WinApiFS.INVALID_HANDLE_VALUE) && (link_handle != IntPtr.Zero))
                {
                    WinApiFS.CloseHandle(link_handle);
                }
            }
        }


        public static void DeleteMountpoint(IntPtr link_handle)
        {
            REPARSE_DATA_BUFFER_MOUNTPOINT rep = REPARSE_DATA_BUFFER_MOUNTPOINT.Init();
            rep.ReparseTag = WinApiFS.IO_REPARSE_TAG_MOUNT_POINT;
            rep.SetNames(string.Empty, string.Empty);
            rep.PrintNameLength = 0;
            rep.PrintNameOffset = 0;
            rep.ReparseDataLength = 0;
            rep.SubstituteNameLength = 0;
            rep.SubstituteNameOffset = 0;

            int data_size = ntApiFS.REPARSE_DATA_BUFFER_HEADER_SIZE + rep.ReparseDataLength;
            int bytes_returned=0;

            int res = WinApiFS.DeviceIoControl
                (link_handle,
                WinApiFS.FSCTL_DELETE_REPARSE_POINT,
                ref rep,
                data_size,
                IntPtr.Zero,
                0,
                ref bytes_returned,
                IntPtr.Zero);
            if (res == 0)
            {
                int win_err = Marshal.GetLastWin32Error();
                throw new Win32Exception(win_err);
            }
        }

        public static void DeleteMountpoint(string link_name)
        {
            IntPtr link_handle = IntPtr.Zero;

            try
            {
                link_handle = WinAPiFSwrapper.CreateFileHandle
                    (link_name,
                    Win32FileAccess.WRITE_ATTRIBUTES | Win32FileAccess.WRITE_EA,
                    FileShare.ReadWrite | FileShare.Delete,
                    FileMode.Open,
                    CreateFileOptions.BACKUP_SEMANTICS | CreateFileOptions.OPEN_REPARSE_POINT);
                DeleteMountpoint(link_handle);
            }
            finally
            {
                if ((link_handle.ToInt32() != WinApiFS.INVALID_HANDLE_VALUE) && (link_handle != IntPtr.Zero))
                {
                    WinApiFS.CloseHandle(link_handle);
                }
            }
        }

        public static void SetMountpoint(IntPtr link_handle, string target_name,string print_name)
        {
            REPARSE_DATA_BUFFER_MOUNTPOINT rep = REPARSE_DATA_BUFFER_MOUNTPOINT.Init();
            string nt_target_name = IOhelper.AddUnparsedPrefix(target_name);
            int win_err = 0;
            int bytes_returned = 0;

            rep.ReparseTag = WinApiFS.IO_REPARSE_TAG_MOUNT_POINT;

            print_name = ((print_name == null) || (print_name == string.Empty)) ? target_name : print_name;

            rep.SetNames(nt_target_name, print_name);
            int data_size = ntApiFS.REPARSE_DATA_BUFFER_HEADER_SIZE + rep.ReparseDataLength;

            int res = WinApiFS.DeviceIoControl
                (link_handle,
                WinApiFS.FSCTL_SET_REPARSE_POINT,
                ref rep,
                data_size,
                IntPtr.Zero,
                0,
                ref bytes_returned,
                IntPtr.Zero);
            if (res == 0)
            {
                win_err = Marshal.GetLastWin32Error();
                throw new Win32Exception(win_err);
            }
        }

        /// <summary>
        /// creates junction. link_name and target_name must be directories
        /// </summary>
        /// <param name="link_name"></param>
        /// <param name="target_name"></param>
        public static void SetMountpoint(string link_name, string target_name,string print_name)
        {
            IntPtr link_handle = IntPtr.Zero;
            string nt_link_name = IOhelper.GetUnicodePath(link_name);
            int win_err = 0;

            try
            {
                link_handle = WinApiFS.CreateFile_intptr
                    (nt_link_name,
                    Win32FileAccess.WRITE_ATTRIBUTES | Win32FileAccess.WRITE_EA,
                    FileShare.ReadWrite,
                    IntPtr.Zero,
                    FileMode.Open,
                    CreateFileOptions.BACKUP_SEMANTICS | CreateFileOptions.OPEN_REPARSE_POINT,
                    IntPtr.Zero);
                if (link_handle.ToInt32() == WinApiFS.INVALID_HANDLE_VALUE)
                {
                    win_err = Marshal.GetLastWin32Error();
                    throw new Win32Exception(win_err);
                }

                SetMountpoint(link_handle, target_name,print_name);
            }
            finally
            {
                if ((link_handle != IntPtr.Zero) && (link_handle.ToInt32() != WinApiFS.INVALID_HANDLE_VALUE))
                {
                    WinApiFS.CloseHandle(link_handle);
                }
            }
        }

        public static void CreateSymbolicLink(string link_name, string link_target, bool is_directory)
        {
            int res = WinApiFS.CreateSymbolicLink
                (link_name,
                link_target,
                is_directory ? 1 : 0);
            //correct
            //res 0 if success
            if (res == 0)
            {
                int win_err = Marshal.GetLastWin32Error();
                throw new Win32Exception(win_err);
            }
        }

        public static REPARSE_DATA_BUFFER_MOUNTPOINT GetMountpointInfo(IntPtr file_handle)
        {
            REPARSE_DATA_BUFFER_MOUNTPOINT ret = REPARSE_DATA_BUFFER_MOUNTPOINT.Init();
            int win_err = 0;
            int bytes_returned = 0;

            int res = WinApiFS.DeviceIoControl
                (file_handle,
                WinApiFS.FSCTL_GET_REPARSE_POINT,
                IntPtr.Zero,
                0,
                ref ret,
                Marshal.SizeOf(ret),
                ref bytes_returned,
                IntPtr.Zero);
            if (res == 0)
            {
                win_err = Marshal.GetLastWin32Error();
                if (win_err == WinApiFS.ERROR_NOT_A_REPARSE_POINT)
                {
                    //return empty structure
                    return ret;
                }
                else
                {
                    throw new Win32Exception(win_err);
                }
            }
            return ret;
        }

        /// <summary>
        /// file_name myst be directory
        /// </summary>
        /// <param name="file_name"></param>
        /// <returns></returns>
        public static REPARSE_DATA_BUFFER_MOUNTPOINT GetMountpointInfo(string file_name)
        {
            IntPtr file_handle = IntPtr.Zero;
            string nt_file_name=IOhelper.GetUnicodePath(file_name);
            int win_err = 0;

            try
            {

                //SetBackupRestorePrivileges(false);

                file_handle = WinApiFS.CreateFile_intptr
                    (nt_file_name,
                    Win32FileAccess.READ_EA|Win32FileAccess.READ_ATTRIBUTES,
                    FileShare.ReadWrite,
                    IntPtr.Zero,
                    FileMode.Open,
                    //CreateFileOptions.OPEN_REPARSE_POINT,
                    CreateFileOptions.BACKUP_SEMANTICS | CreateFileOptions.OPEN_REPARSE_POINT,
                    IntPtr.Zero);
                if (file_handle.ToInt32() == WinApiFS.INVALID_HANDLE_VALUE)
                {
                    win_err = Marshal.GetLastWin32Error();
                    throw new Win32Exception(win_err);
                }
                return GetMountpointInfo(file_handle);
            }
            finally
            {
                if ((file_handle != IntPtr.Zero) && (file_handle.ToInt32() != WinApiFS.INVALID_HANDLE_VALUE))
                {
                    WinApiFS.CloseHandle(file_handle);
                }
            }
        }

        public static REPARSE_DATA_BUFFER_SYMLINK GetSymboliclinkInfo(string file_name)
        {
            IntPtr file_handle = IntPtr.Zero;
            string nt_file_name = IOhelper.GetUnicodePath(file_name);
            int win_err = 0;

            try
            {

                //SetBackupRestorePrivileges(false);

                //file may be directory or file
                if (IOhelper.IsDirectory(file_name))
                {
                    file_handle = WinApiFS.CreateFile_intptr
                        (nt_file_name,
                        Win32FileAccess.READ_EA | Win32FileAccess.READ_ATTRIBUTES,
                        FileShare.ReadWrite,
                        IntPtr.Zero,
                        FileMode.Open,
                        //CreateFileOptions.OPEN_REPARSE_POINT,
                        CreateFileOptions.BACKUP_SEMANTICS | CreateFileOptions.OPEN_REPARSE_POINT,
                        IntPtr.Zero);
                }
                else
                {
                    file_handle = WinApiFS.CreateFile_intptr
                        (nt_file_name,
                        Win32FileAccess.READ_EA | Win32FileAccess.READ_ATTRIBUTES,
                        FileShare.ReadWrite,
                        IntPtr.Zero,
                        FileMode.Open,
                        CreateFileOptions.OPEN_REPARSE_POINT,
                        //CreateFileOptions.BACKUP_SEMANTICS | CreateFileOptions.OPEN_REPARSE_POINT,
                        IntPtr.Zero);
                }
                if (file_handle.ToInt32() == WinApiFS.INVALID_HANDLE_VALUE)
                {
                    win_err = Marshal.GetLastWin32Error();
                    throw new Win32Exception(win_err);
                }
                return GetSymboliclinkInfo(file_handle);
            }
            finally
            {
                if ((file_handle != IntPtr.Zero) && (file_handle.ToInt32() != WinApiFS.INVALID_HANDLE_VALUE))
                {
                    WinApiFS.CloseHandle(file_handle);
                }
            }
        }

        public static REPARSE_DATA_BUFFER_SYMLINK GetSymboliclinkInfo(IntPtr file_handle)
        {
            REPARSE_DATA_BUFFER_SYMLINK ret = REPARSE_DATA_BUFFER_SYMLINK.Init();
            int win_err = 0;
            int bytes_returned = 0;

            int res = WinApiFS.DeviceIoControl
                (file_handle,
                WinApiFS.FSCTL_GET_REPARSE_POINT,
                IntPtr.Zero,
                0,
                ref ret,
                Marshal.SizeOf(ret),
                ref bytes_returned,
                IntPtr.Zero);
            if (res == 0)
            {
                win_err = Marshal.GetLastWin32Error();
                if (win_err == WinApiFS.ERROR_NOT_A_REPARSE_POINT)
                {
                    //return empty structure
                    return ret;
                }
                else
                {
                    throw new Win32Exception(win_err);
                }
            }
            return ret;
        }

        /// <summary>
        /// copy files with dotnet streams
        /// </summary>
        /// <param name="source_file">may be stream or file</param>
        /// <param name="destination_file">may be stream or file</param>
        /// <param name="overwrite_existing"></param>
        /// <param name="callback"></param>
        /// <param name="callback_data"></param>
        public static void CopyFile
            (string source_file,
            string destination_file,
            bool overwrite_existing,
            CopyProgressRoutine callback,
            IntPtr callback_data,
            int buffer_size)
        {
            //allocate buffer
            byte[] buffer = new byte[buffer_size];

            SafeFileHandle source_handle = null;
            SafeFileHandle dest_handle = null;
            int win_err = 0;
            FileStream source_stream = null;
            FileStream dest_stream = null;
            int bytes_readed = 0;
            //bool continue_read = true;
            //ulong total_size = 0UL;
            ulong total_transferred = 0UL;
            CopyFileExCallbackReturns callback_ret = CopyFileExCallbackReturns.CONTINUE;

            FileStream t_s = new FileStream(source_handle, FileAccess.Read, buffer_size, true);
            IAsyncResult a_res = t_s.BeginRead(buffer, 0, buffer_size, null, null);
            int b_res = t_s.EndRead(a_res);
            

            try
            {
                //open existing file
                source_handle = WinApiFS.CreateFile_safe
                    (IOhelper.GetUnicodePath(source_file),
                    Win32FileAccess.GENERIC_READ,
                    FileShare.Read,
                    IntPtr.Zero,
                    FileMode.Open,
                    CreateFileOptions.SEQUENTIAL_SCAN,
                    IntPtr.Zero);
                if (source_handle.IsInvalid)
                {
                    win_err = Marshal.GetLastWin32Error();
                    throw new Win32Exception(win_err);
                }

                //create destination file
                dest_handle = WinApiFS.CreateFile_safe
                    (IOhelper.GetUnicodePath(destination_file),
                    Win32FileAccess.GENERIC_WRITE,
                    FileShare.Read,
                    IntPtr.Zero,
                    overwrite_existing ? FileMode.Create : FileMode.CreateNew,
                    CreateFileOptions.None,
                    source_handle.DangerousGetHandle());
                if (dest_handle.IsInvalid)
                {
                    win_err = Marshal.GetLastWin32Error();
                    throw new Win32Exception(win_err);
                }

                //create streams
                source_stream = new FileStream(source_handle, FileAccess.Read, buffer_size);
                dest_stream = new FileStream(dest_handle, FileAccess.Write, buffer_size);

                if ((callback != null) && (callback_ret != CopyFileExCallbackReturns.QUIET))
                {
                    callback_ret = callback
                        ((ulong)source_stream.Length,
                        0UL,
                        (ulong)source_stream.Length,
                        0UL,
                        1,
                        CopyFileExState.STREAM_SWITCH,
                        source_handle.DangerousGetHandle(),
                        dest_handle.DangerousGetHandle(),
                        callback_data);
                }

                if ((callback_ret == CopyFileExCallbackReturns.CANCEL) || (callback_ret == CopyFileExCallbackReturns.STOP))
                {
                    return;
                }

                //read first chunk
                bytes_readed = source_stream.Read(buffer, 0, buffer_size);
                while (bytes_readed != 0)
                {
                    dest_stream.Write(buffer, 0, bytes_readed);
                    total_transferred += (ulong)bytes_readed;

                    if ((callback != null) && (callback_ret != CopyFileExCallbackReturns.QUIET))
                    {
                        callback_ret = callback
                            ((ulong)source_stream.Length,
                            total_transferred,
                            (ulong)source_stream.Length,
                            total_transferred,
                            1,
                            CopyFileExState.CHUNK_FINISHED,
                            source_handle.DangerousGetHandle(),
                            dest_handle.DangerousGetHandle(),
                            callback_data);
                    }

                    if ((callback_ret == CopyFileExCallbackReturns.CANCEL) || (callback_ret == CopyFileExCallbackReturns.STOP))
                    {
                        break;
                    }

                    //read next chunk
                    bytes_readed = source_stream.Read(buffer, 0, buffer_size);
                }
            }
            finally
            {
                if (dest_stream != null)
                {
                    dest_stream.Close();
                }
                if (source_stream != null)
                {
                    source_stream.Close();
                }
                if ((dest_handle != null) && (!dest_handle.IsInvalid) && (!dest_handle.IsClosed))
                {
                    //dest_handle may be already closed, but...
                    WinApiFS.CloseHandle(dest_handle.DangerousGetHandle());
                }
                if ((source_handle != null) && (!source_handle.IsInvalid) && (!source_handle.IsClosed))
                {
                    WinApiFS.CloseHandle(source_handle.DangerousGetHandle());
                }
                if (callback_ret == CopyFileExCallbackReturns.CANCEL)
                {
                    //delete destinstion
                    WinApiFS.DeleteFile(IOhelper.GetUnicodePath(destination_file));
                }
            }
        }

        public static long GetFileSize(IntPtr file_handle)
        {
            LARGE_INTEGER large_integer = new LARGE_INTEGER();
            int res = WinApiFS.GetFileSizeEx(file_handle, ref large_integer);
            if (res == 0)
            {
                int win_err = Marshal.GetLastWin32Error();
                Win32Exception win_ex = new Win32Exception(win_err);
                throw win_ex;
            }
            return large_integer.QuadPart;
        }

        /// <summary>
        /// retruns size from handle, streams supported, if directory or not found, returns 0
        /// </summary>
        /// <param name="file_name"></param>
        /// <returns></returns>
        public static long GetFileSize(string file_name)
        {
            IntPtr file_handle = IntPtr.Zero;

            try
            {
                string uni_name = IOhelper.GetUnicodePath(file_name);

                file_handle = WinApiFS.CreateFile_intptr
                    (uni_name,
                    Win32FileAccess.GENERIC_READ,
                    FileShare.ReadWrite | FileShare.Delete,
                    IntPtr.Zero,
                    FileMode.Open,
                    CreateFileOptions.None,
                    IntPtr.Zero);
                if (file_handle.ToInt32() == WinApiFS.INVALID_HANDLE_VALUE)
                {
                    //ignore all errors, simply return 0
                    return 0L;
                }
                //successfully open
                LARGE_INTEGER large_integer = new LARGE_INTEGER();
                int res = WinApiFS.GetFileSizeEx(file_handle, ref large_integer);
                if (res == 0)
                {
                    int win_err = Marshal.GetLastWin32Error();
                    Win32Exception win_ex = new Win32Exception(win_err);
                    throw win_ex;
                }
                return large_integer.QuadPart;
            }
            finally
            {
                if ((file_handle != IntPtr.Zero) || (file_handle.ToInt32() != WinApiFS.INVALID_HANDLE_VALUE))
                {
                    WinApiFS.CloseHandle(file_handle);
                }
            }
        }

        public static void CreateDirectoryTree(string new_directory_name, string template_directory_name)
        {
            int res = 0;
            if ((template_directory_name == null) || (template_directory_name == string.Empty))
            {
                res = WinApiFS.CreateDirectory(new_directory_name, IntPtr.Zero);
            }
            else
            {
                res = WinApiFS.CreateDirectoryEx(template_directory_name, new_directory_name, IntPtr.Zero);
            }
            if (res == 0)
            {
                int win_err = Marshal.GetLastWin32Error();
                if (win_err == 183) //ERROR_ALREADY_EXISTS
                {
                    return;
                }
                if (win_err == 3) //ERROR_PATH_NOT_FOUND
                {
                    //call recursive to create intermediate direcroies
                    string parent_dir = Path.GetDirectoryName(new_directory_name);
                    if (parent_dir == null)
                    {
                        //drive not found
                        win_err = 15;//ERROR_INVALID_DRIVE
                        throw new Win32Exception(win_err);
                    }
                    CreateDirectoryTree(parent_dir, string.Empty);
                    //and call api
                    if ((template_directory_name == null) || (template_directory_name == string.Empty))
                    {
                        res = WinApiFS.CreateDirectory(new_directory_name, IntPtr.Zero);
                    }
                    else
                    {
                        res = WinApiFS.CreateDirectoryEx(template_directory_name, new_directory_name, IntPtr.Zero);
                    }
                    if (res == 0)
                    {
                        win_err = Marshal.GetLastWin32Error();
                        throw new Win32Exception(win_err);
                    }
                    else
                    {
                        return;
                    }
                }
                throw new Win32Exception(win_err);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="file1"></param>
        /// <param name="file2"></param>
        /// <returns></returns>
        public static bool IsSameVolume(string file1, string file2)
        {
            string path1 = file1;
            string path2 = file2;
            BY_HANDLE_FILE_INFORMATION info1 = new BY_HANDLE_FILE_INFORMATION();
            BY_HANDLE_FILE_INFORMATION info2 = new BY_HANDLE_FILE_INFORMATION();
            bool exist = false;
            bool ret = false;
            //check file1
            while (!exist)
            {
                exist = GetFileInfo(path1, ref info1);
                if (!exist)
                {
                    path1 = Path.GetDirectoryName(path1);
                    if (path1 == null)
                    {
                        //i.e. path1 was root path
                        return false;
                    }
                }
            }
            //check file2
            exist = false;
            while (!exist)
            {
                exist = GetFileInfo(path2, ref info2);
                if (!exist)
                {
                    path2 = Path.GetDirectoryName(path2);
                    if (path2 == null)
                    {
                        return false;
                    }
                }
            }

            //now we have info
            ret = info1.dwVolumeSerialNumber == info2.dwVolumeSerialNumber;

            return ret;
        }

        public static ulong GetDirectoryStat(string file_name, bool include_subdirs, bool include_AFS, bool include_sec_attributes, ref int file_count, ref int dir_count,DirectoryStatChunkComplete callback)
        {
            ulong ret = 0;

            string search_path = file_name;
            search_path.TrimEnd(new char[] { Path.DirectorySeparatorChar });
            search_path = search_path + Path.DirectorySeparatorChar + "*";
            search_path = IOhelper.GetUnicodePath(search_path);

            WinAPiFSwrapper.WIN32_FIND_DATA_enumerable fs_enum = new WIN32_FIND_DATA_enumerable(search_path, true);
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

                if (((data.dwFileAttributes & FileAttributes.Directory) == FileAttributes.Directory) && (include_subdirs))
                {
                    ret += GetDirectoryStat(Path.Combine(file_name, data.cFileName), include_subdirs, include_AFS, include_sec_attributes, ref file_count, ref dir_count,callback);
                    dir_count++;
                    if (callback != null)
                    {
                        callback(file_name, file_count, dir_count, ret);
                    }
                    continue;
                }

                if (!include_AFS)
                {
                    ret += data.FileSize;
                    file_count++;
                }
                else
                {
                    List<FileStreamInfo> streams = WinAPiFSwrapper.GetFileStreamInfo_fromBackup(Path.Combine(file_name, data.cFileName), include_sec_attributes);
                    foreach (FileStreamInfo one_stream in streams)
                    {
                        ret += one_stream.Size;
                    }
                    file_count++;
                }
            }
            return ret;
        }

        public static void SetFileAttributes(string file_name, FileAttributes attributes)
        {
            string uni_name=IOhelper.GetUnicodePath(file_name);
            int res = WinApiFS.SetFileAttributes(uni_name, attributes);
            if (res == 0)
            {
                int win_err = Marshal.GetLastWin32Error();
                throw new Win32Exception(win_err);
            }
        }

        public static WIN32_FILE_ATTRIBUTE_DATA GetFileAttributes(string file_name)
        {
            string uni_name = IOhelper.GetUnicodePath(file_name);
            WIN32_FILE_ATTRIBUTE_DATA ret = new WIN32_FILE_ATTRIBUTE_DATA();
            int res = WinApiFS.GetFileAttributesEx
                (uni_name,
                GET_FILEEX_INFO_LEVELS.GetFileExInfoStandard,
                ref ret);
            if (res == 0)
            {
                int win_err = Marshal.GetLastWin32Error();
                throw new Win32Exception(win_err);
            }
            return ret;
        }

        private static string[] Buffer2Array(IntPtr buffer, int chars_len)
        {
            int bytes_len = chars_len * Marshal.SystemDefaultCharSize;
            byte[] buffer_bytes = new byte[bytes_len];
            Marshal.Copy(buffer, buffer_bytes, 0, bytes_len);

            Encoding enc = null;
            if (Marshal.SystemDefaultCharSize == 2)
            {
                enc = Encoding.Unicode;
            }
            else
            {
                enc = Encoding.Default;
            }

            string strArray = enc.GetString(buffer_bytes);
            string[] ret = strArray.Split(new char[] { '\0' }, StringSplitOptions.RemoveEmptyEntries);
            return ret;
        }

        public static string[] GetLogicalDrives()
        {
            int buffer_len=WinApiFS.MAX_PATH;
            IntPtr buffer = IntPtr.Zero;
            string[] ret;

            try
            {
                buffer = Marshal.AllocHGlobal(buffer_len * Marshal.SystemDefaultCharSize);
                int res = WinApiFS.GetLogicalDriveStrings(buffer_len, buffer);
                if (res == 0)
                {
                    int winErr = Marshal.GetLastWin32Error();
                    throw new Win32Exception(winErr);
                }
                else if (res > buffer_len)
                {
                    throw new ApplicationException("Buffer to small.");
                }
                else
                {
                    ret = Buffer2Array(buffer, res);
                    return ret;
                }
            }
            finally
            {
                if (buffer != IntPtr.Zero)
                {
                    Marshal.FreeHGlobal(buffer);
                }
            }

        }

        public static void CreateHardLink(string new_link_name, string existing_file_name)
        {
            int res = WinApiFS.CreateHardLink
                (new_link_name,
                existing_file_name,
                IntPtr.Zero);
            if (res == 0)
            {
                throw new Win32Exception(Marshal.GetLastWin32Error());
            }
        }

        public static FILE_BASIC_INFO GetFileInfo_Basic(string file_name)
        {


            IntPtr file_handle = IntPtr.Zero;

            try
            {
                file_handle = CreateFileHandle
                    (file_name,
                    Win32FileAccess.READ_ATTRIBUTES,
                    FileShare.ReadWrite,
                    FileMode.Open,
                    CreateFileOptions.SEQUENTIAL_SCAN);
                FILE_BASIC_INFO ret = GetFileInfo_Basic(file_handle);

                return ret;
            }
            finally
            {
                if (file_handle != IntPtr.Zero)
                {
                    CloseFileHandle(file_handle);
                }
            }
        }

        public static FILE_BASIC_INFO GetFileInfo_Basic(IntPtr file_handle)
        {
            IntPtr buffer = IntPtr.Zero;

            try
            {
                int buffer_len = Marshal.SizeOf(typeof(FILE_BASIC_INFO));
                buffer = Marshal.AllocHGlobal(buffer_len);

                int res = WinApiFS.GetFileInformationByHandleEx
                    (file_handle,
                    FILE_INFO_BY_HANDLE_CLASS.FileBasicInfo,
                    buffer,
                    buffer_len);

                if (res == 0)
                {
                    throw new Win32Exception(Marshal.GetLastWin32Error());
                }

                FILE_BASIC_INFO ret = new FILE_BASIC_INFO();
                Marshal.PtrToStructure(buffer, ret);
                return ret;
            }
            finally
            {
                if (buffer != IntPtr.Zero)
                {
                    Marshal.FreeHGlobal(buffer);
                }
            }
        }

        public static BY_HANDLE_FILE_INFORMATION GetFileInfo(IntPtr file_handle)
        {
            BY_HANDLE_FILE_INFORMATION ret = new BY_HANDLE_FILE_INFORMATION();
            int res_info = WinApiFS.GetFileInformationByHandle
                (file_handle,
                ref ret);
            if (res_info == 0)
            {
                int err_code_1 = Marshal.GetLastWin32Error();
                Win32Exception ex_win = new Win32Exception(err_code_1);
                ApplicationException ex = new ApplicationException
                ("Failed GetFileInformationByHandle call. " + ex_win.Message, ex_win);
                ex.Source = ex_win.Source;
                throw ex;
            }
            return ret;
        }

        public static void CloseFileHandle(IntPtr file_handle)
        {
            int res = WinApiFS.CloseHandle(file_handle);
            if (res == 0)
            {
                throw new Win32Exception(Marshal.GetLastWin32Error());
            }
        }

        public static IntPtr CreateFileHandle
            (string file_name,
            Win32FileAccess file_access,
            FileShare file_share,
            FileMode file_mode,
            CreateFileOptions options)
        {
            IntPtr ret = WinApiFS.CreateFile_intptr
                (IOhelper.GetUnicodePath(file_name),
                file_access,
                file_share,
                IntPtr.Zero,
                file_mode,
                options,
                IntPtr.Zero);
            if (ret.ToInt32() == WinApiFS.INVALID_HANDLE_VALUE)
            {
                throw new Win32Exception(Marshal.GetLastWin32Error());
            }
            return ret;
        }

        public static bool GetFileInfo(string file_name, ref BY_HANDLE_FILE_INFORMATION info)
        {
            //пробуем открыть файл
            IntPtr file_handle = IntPtr.Zero;

            try
            {
                file_handle = WinApiFS.CreateFile_intptr
                    (file_name,
                    Win32FileAccess.READ_ATTRIBUTES,
                    FileShare.ReadWrite,
                    IntPtr.Zero,
                    FileMode.Open,
                    CreateFileOptions.BACKUP_SEMANTICS,
                    IntPtr.Zero);

                if (file_handle.ToInt32() == WinApiFS.INVALID_HANDLE_VALUE)
                {
                    int win_err = Marshal.GetLastWin32Error();
                    if (win_err == 2)
                    {
                        return false;
                    }
                    if (win_err == 3)
                    {
                        return false;
                    }
                    throw new Win32Exception(win_err);
                }

                //если успешно открыли...
                int res_info = WinApiFS.GetFileInformationByHandle
                    (file_handle,
                    ref info);
                if (res_info == 0)
                {
                    int err_code_1 = Marshal.GetLastWin32Error();
                    Win32Exception ex_win = new Win32Exception(err_code_1);
                    throw ex_win;
                }

                return true;
            }
            finally
            {
                if ((file_handle != IntPtr.Zero) && (file_handle.ToInt32() != WinApiFS.INVALID_HANDLE_VALUE))
                {
                    CloseFileHandle(file_handle);
                }
            }
        }

        public static BY_HANDLE_FILE_INFORMATION GetFileInfo(string file_name)
        {
            //пробуем открыть файл
            IntPtr file_handle = IntPtr.Zero;

            try
            {
                file_handle = CreateFileHandle
                    (file_name,
                    Win32FileAccess.READ_ATTRIBUTES,
                    FileShare.ReadWrite,
                    FileMode.Open,
                    CreateFileOptions.None);

                if (file_handle.ToInt32() == WinApiFS.INVALID_HANDLE_VALUE)
                {
                    int win_err = Marshal.GetLastWin32Error();
                    throw new Win32Exception(win_err);
                }

                //если успешно открыли...
                BY_HANDLE_FILE_INFORMATION ret = new BY_HANDLE_FILE_INFORMATION();
                int res_info = WinApiFS.GetFileInformationByHandle
                    (file_handle,
                    ref ret);
                if (res_info == 0)
                {
                    int err_code_1 = Marshal.GetLastWin32Error();
                    Win32Exception ex_win = new Win32Exception(err_code_1);
                    throw ex_win;
                }

                return ret;
            }
            finally
            {
                if ((file_handle != IntPtr.Zero) && (file_handle.ToInt32() != WinApiFS.INVALID_HANDLE_VALUE))
                {
                    CloseFileHandle(file_handle);
                }
            }
        }

        public static FileBinaryType GetBinaryType(string file_name)
        {
            FileBinaryType ret = FileBinaryType.None;
            int res = WinApiFS.GetBinaryType
                (IOhelper.GetUnicodePath(file_name),
                ref ret);

            //нсли res==0 файл неисполгяемый или исключение
            //в любом случае возвращаем FileBinaryType.None
            return ret;
        }

        public static void CreateNewEmptyFile(string file_name)
        {
            FileStream fs = null;
            try
            {
                fs = CreateStreamEx
                    (file_name,
                    FileAccess.Write,
                    FileShare.None,
                    FileMode.CreateNew,
                    CreateFileOptions.SEQUENTIAL_SCAN);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (fs != null)
                {
                    fs.Close();
                }
            }
        }

        public static FileStream CreateStreamEx(string file_name, FileAccess access, FileShare share, FileMode mode, CreateFileOptions options)
        {
            FileStream ret = null;

            Win32FileAccess desiredAccess = Win32FileAccess.None;
            switch (access)
            {
                case FileAccess.Read:
                    desiredAccess = Win32FileAccess.GENERIC_READ;
                    break;
                case FileAccess.ReadWrite:
                    desiredAccess = Win32FileAccess.GENERIC_READ | Win32FileAccess.GENERIC_WRITE;
                    break;
                case FileAccess.Write:
                    desiredAccess = Win32FileAccess.GENERIC_WRITE;
                    break;
            }

            Microsoft.Win32.SafeHandles.SafeFileHandle handle = WinApiFS.CreateFile_safe
                (IOhelper.GetUnicodePath(file_name),
                desiredAccess,
                share,
                IntPtr.Zero,
                mode,
                options,
                IntPtr.Zero);

            if (handle.IsInvalid)
            {
                throw new Win32Exception(Marshal.GetLastWin32Error());
            }

            ret = new FileStream(handle, access);

            return ret;
        }

        public static Exception MoveFile(string existing_file, string new_name)
        {
            MoveFileOptions opts =
                MoveFileOptions.FailIfNotTrackable |
                MoveFileOptions.WriteThrough;
            int res = WinApiFS.MoveFileEx
                (existing_file,
                new_name,
                opts);
            if (res == 0)
            {
                Win32Exception ex = new Win32Exception(Marshal.GetLastWin32Error());
                return ex;
            }
            return null;
        }

        public static void DeleteFileOnReboot(string existing_file)
        {
            MoveFileOptions opts = MoveFileOptions.DelayUntilReboot;
            int res = WinApiFS.MoveFileWithProgress
                (existing_file,
                IntPtr.Zero,
                IntPtr.Zero,
                IntPtr.Zero,
                opts);
            if (res == 0)
            {
                Win32Exception ex = new Win32Exception(Marshal.GetLastWin32Error());
                throw ex;
            }
        }

        //public static FileTransferResult MoveFile
        //    (CopyFileItemEx item,
        //    CopyProgressRoutine callback)
        //{
        //    try
        //    {
        //        FileInfoEx destination_info = new FileInfoEx(item.DestinationFile);
        //        FileInfoEx source_info = new FileInfoEx(item.SourceFile);

        //        bool destination_hidden = false;
        //        bool destination_readonly = false;

        //        if (destination_info.Exist)
        //        {
        //            //see on options

        //            destination_hidden = (destination_info.Attributes & FileAttributes.Hidden) == FileAttributes.Hidden;
        //            destination_readonly = (destination_info.Attributes & FileAttributes.ReadOnly) == FileAttributes.ReadOnly;

        //            //проверяем, надо ли пропустить 
        //            if ((item.SkipExisting) && (!item.OverwriteExisting))
        //            {
        //                return new FileTransferResult(item.SourceFile, item.DestinationFile, TransferResult.Skip);
        //            }

        //            if ((item.SkipHidden) && (!item.OverwriteHidden) && destination_hidden)
        //            {
        //                return new FileTransferResult(item.SourceFile, item.DestinationFile, TransferResult.Skip);
        //            }

        //            if ((item.SkipReadonly) && (!item.OverwriteReadonly) && destination_readonly)
        //            {
        //                return new FileTransferResult(item.SourceFile, item.DestinationFile, TransferResult.Skip);
        //            }

        //            if ((item.OverwriteExisting || item.OverwriteHidden || item.OverwriteReadonly) &&
        //                (item.OverwriteOnlyIfSourceNewer) &&
        //                (DateTime.Compare(destination_info.LastWriteTime, source_info.LastWriteTime) >= 0))
        //            {
        //                return new FileTransferResult(item.SourceFile, item.DestinationFile, TransferResult.Skip);
        //            }

        //            //пропускать не надо
        //            if (destination_hidden)
        //            {
        //                if (item.OverwriteHidden)
        //                {
        //                    //reset hidden attr
        //                    destination_info.Attributes = destination_info.Attributes & (~FileAttributes.Hidden);
        //                }
        //                else
        //                {
        //                    return new FileTransferResult(item.SourceFile, item.DestinationFile, TransferResult.Fail_DestinationHidden);
        //                }
        //            }
        //            if (destination_readonly)
        //            {
        //                if (item.OverwriteReadonly)
        //                {
        //                    destination_info.Attributes = destination_info.Attributes & (~FileAttributes.ReadOnly);
        //                }
        //                else
        //                {
        //                    //if (destination_hidden)
        //                    //{
        //                    //    //restore hidden flag
        //                    //    destination_info.Attributes = destination_info.Attributes | FileAttributes.Hidden;
        //                    //}
        //                    return new FileTransferResult(item.SourceFile, item.DestinationFile, TransferResult.Fail_DestinationReadOnly);
        //                }
        //            }
        //            if (!item.OverwriteExisting)
        //            {
        //                return new FileTransferResult(item.SourceFile, item.DestinationFile, TransferResult.Fail_DestinationExist);
        //            }
        //        }

        //        //если дошли до этого места, вызываем native move
        //        MoveFileOptions opts = MoveFileOptions.CopyAllowed | MoveFileOptions.FailIfNotTrackable;
        //        if (destination_info.Exist)
        //        {
        //            opts = opts | MoveFileOptions.ReplaceExisting;
        //        }

        //        int move_res = Native.MoveFileWithProgress
        //            (IOhelper.GetUnicodePath(item.SourceFile),
        //            IOhelper.GetUnicodePath(item.DestinationFile),
        //            callback,
        //            IntPtr.Zero,
        //            opts);

        //        //смтрим на результат
        //        if (res == 0)
        //        {
        //            int winErr = Marshal.GetLastWin32Error();
        //            if (winErr == 1235) //error_query_abort
        //            {
        //                return new FileTransferResult(item.SourceFile, item.DestinationFile, TransferResult.Query_Abort);
        //            }
        //            else if (winErr == 3)//path not found
        //            {
        //                return new FileTransferResult(item.SourceFile, item.DestinationFile, TransferResult.Fail_DestinationDirNotFound);
        //            }
        //            else
        //            {
        //                return new FileTransferResult(item.SourceFile, item.DestinationFile, TransferResult.Fail_Other, new Win32Exception(winErr));
        //            }
        //        }
        //        else
        //        {
        //            return new FileTransferResult(item.SourceFile, item.DestinationFile);
        //        }

        //        //если дошли до этого места, перемещение удачно
        //        return new FileTransferResult(item.SourceFile, item.DestinationFile);
        //    }
        //    catch (Exception ex)
        //    {
        //        return new FileTransferResult(item.SourceFile, item.DestinationFile, TransferResult.Fail_Other, ex);
        //    }
        //}

        //public static int DeleteAllEntries(IEnumerable<FileInfoEx> entries, bool delete_readonly)
        //{
        //    int ret = 0;
        //    int res = 0;
        //    int winErr = 0;
        //    foreach (FileInfoEx ones in entries)
        //    {
        //        if ((ones.Attributes & FileAttributes.Directory) == FileAttributes.Directory)
        //        {
        //            if ((ones.Name != ".") || (ones.Name != ".."))
        //            {
        //                List<FileInfoEx> child_entries = FileInfoEx.GetFSentriesWithoutCurrentDir(ones.FullName);
        //                if (child_entries.Count > 0)
        //                {
        //                    ret=ret+DeleteAllEntries(child_entries, delete_readonly);
        //                }
        //                //now handle readonly
        //                if ((delete_readonly) && ((ones.Attributes & FileAttributes.ReadOnly) == FileAttributes.ReadOnly))
        //                {
        //                    ones.Attributes = ones.Attributes & (~FileAttributes.ReadOnly);
        //                }
        //                res = Native.RemoveDirectory(ones.FullName);
        //                if (res == 0)
        //                {
        //                    ret++;
        //                    winErr = Marshal.GetLastWin32Error();
        //                    //TODO: analyze winErr
        //                }
        //            }
        //        }
        //        else
        //        {
        //            if ((delete_readonly) && ((ones.Attributes & FileAttributes.ReadOnly) == FileAttributes.ReadOnly))
        //            {
        //                ones.Attributes = ones.Attributes & (~FileAttributes.ReadOnly);
        //            }
        //            res = Native.DeleteFile(ones.FullName);
        //            if (res == 0)
        //            {
        //                ret++;
        //                winErr = Marshal.GetLastWin32Error();
        //                //TODO: switch on winErr
        //            }
        //        }
        //    }
        //    return ret; //that is number of unsuccesful deletes
        //}

        public static string GetFullPathName(string file_name)
        {
            IntPtr name_buffer = IntPtr.Zero;
            //IntPtr file_name_part = IntPtr.Zero;
            int res = 0;
            int buf_len = WinApiFS.MAX_PATH * Marshal.SystemDefaultCharSize;
            try
            {
                name_buffer = Marshal.AllocHGlobal(buf_len);
                res = WinApiFS.GetFullPathName
                    (file_name,
                    buf_len,
                    name_buffer,
                    IntPtr.Zero);
                if (res == 0)
                {
                    throw new Win32Exception(Marshal.GetLastWin32Error());
                }
                if (res > (buf_len / Marshal.SystemDefaultCharSize) - 1)
                {
                    name_buffer = Marshal.ReAllocHGlobal(name_buffer, new IntPtr((res + 1) * Marshal.SystemDefaultCharSize));
                    res = WinApiFS.GetFullPathName
                        (file_name,
                        res * Marshal.SystemDefaultCharSize,
                        name_buffer,
                        IntPtr.Zero);
                    if (res == 0)
                    {
                        throw new Win32Exception(Marshal.GetLastWin32Error());
                    }
                }
                return Marshal.PtrToStringAuto(name_buffer, res);
            }
            finally
            {
                if (name_buffer != IntPtr.Zero)
                {
                    Marshal.FreeHGlobal(name_buffer);
                }
            }
        }

        private static API_LONG _ulong2uint(ulong inp)
        {
            API_LONG ret = new API_LONG();

            byte[] bytes = BitConverter.GetBytes(inp);
            ret.Low = BitConverter.ToUInt32(bytes, 0);
            ret.High = BitConverter.ToUInt32(bytes, 4);

            return ret;
        }

        public static List<FileStreamInfo> GetFileStreamInfo_fromBackup(string file_name, bool read_security_streams)
        {
            List<FileStreamInfo> ret_list = new List<FileStreamInfo>();
            IntPtr file_handle = IntPtr.Zero;
            IntPtr backup_context = IntPtr.Zero;
            int res = 0;
            int readed_bytes = 0;
            uint seeked_bytes_low = 0;
            uint seeked_bytes_high = 0;
            bool continue_read = true;
            WIN32_STREAM_ID stream_id = new WIN32_STREAM_ID();
            int stream_id_len = Marshal.SizeOf(stream_id);
            string stream_name = string.Empty;
            int read_security = 0;
            if (read_security_streams)
            {
                read_security = 1;
            }

            try
            {
                //open file
                file_handle = WinApiFS.CreateFile_intptr
                    (file_name,
                    Win32FileAccess.GENERIC_READ,
                    FileShare.ReadWrite,
                    IntPtr.Zero,
                    FileMode.Open,
                    CreateFileOptions.BACKUP_SEMANTICS | CreateFileOptions.ATTRIBUTE_NORMAL,
                    IntPtr.Zero);
                //handle exceptions
                if (file_handle.ToInt32() == WinApiFS.INVALID_HANDLE_VALUE)
                {
                    continue_read = false;
                    throw new Win32Exception(Marshal.GetLastWin32Error());
                }
                //now we have open file handle
                while (continue_read)
                {
                    res = WinApiFS.BackupRead
                        (file_handle,
                        ref stream_id,
                        stream_id_len,
                        ref readed_bytes,
                        0,
                        read_security,
                        ref backup_context);
                    //handle exceptions
                    if (res == 0)
                    {
                        continue_read = false;
                        throw new Win32Exception(Marshal.GetLastWin32Error());
                    }
                    if (readed_bytes == 0)
                    {
                        //end of file
                        continue_read = false;
                        break;
                    }
                    if (stream_id.dwStreamNameSize > 0)
                    {
                        //if name exist, read it
                        int s_name_len = stream_id.dwStreamNameSize;
                        IntPtr name_buffer = IntPtr.Zero;
                        try
                        {
                            name_buffer = Marshal.AllocHGlobal(s_name_len);
                            res = WinApiFS.BackupRead
                                (file_handle,
                                name_buffer,
                                stream_id.dwStreamNameSize,
                                ref readed_bytes,
                                0,
                                read_security,
                                ref backup_context);
                            stream_name = Marshal.PtrToStringUni(name_buffer, s_name_len / 2);
                        }
                        finally
                        {
                            if (name_buffer != IntPtr.Zero)
                            {
                                Marshal.FreeHGlobal(name_buffer);
                            }
                        }
                    }
                    else
                    {
                        stream_name = string.Empty;
                    }

                    //save result
                    FileStreamInfo newInfo = new FileStreamInfo
                    (stream_name,
                    stream_id.Size,
                    stream_id.dwStreamId,
                    stream_id.dwStreamAttributes);
                    ret_list.Add(newInfo);

                    //and seek to next stream
                    API_LONG seek_to_ulong = _ulong2uint(stream_id.Size);
                    res = WinApiFS.BackupSeek
                        (file_handle,
                        seek_to_ulong.Low,
                        seek_to_ulong.High,
                        ref seeked_bytes_low,
                        ref seeked_bytes_high,
                        ref backup_context);

                }//end while
                return ret_list;
            }
            finally
            {
                if (file_handle != IntPtr.Zero)
                {
                    if ((backup_context != IntPtr.Zero) || (backup_context.ToInt32() != WinApiFS.INVALID_HANDLE_VALUE))
                    {
                        //reset backup context
                        res = WinApiFS.BackupRead(file_handle, IntPtr.Zero, 0, ref readed_bytes, 1, 0, ref backup_context);
                    }
                    //and close file
                    res = WinApiFS.CloseHandle(file_handle);
                }
            }
        }

        /// <summary>
        /// vista and server 2003 or later, returns only data streams
        /// </summary>
        /// <param name="file_name"></param>
        /// <returns></returns>
        public static List<WIN32_FIND_STREAM_DATA> GetFileStreamInfo(string file_name)
        {
            //FileMode.
            List<WIN32_FIND_STREAM_DATA> ret_list = new List<WIN32_FIND_STREAM_DATA>();
            IntPtr search_handle = IntPtr.Zero;
            int res = 1;
            WIN32_FIND_STREAM_DATA find_data = new WIN32_FIND_STREAM_DATA();
            int winErr = 0;
            try
            {
                search_handle = WinApiFS.FindFirstStreamW
                    (file_name,
                    STREAM_INFO_LEVELS.FindStreamInfoStandard,
                    ref find_data,
                    0);
                if (search_handle.ToInt32() == WinApiFS.INVALID_HANDLE_VALUE)
                {
                    winErr = Marshal.GetLastWin32Error();
                    search_handle = IntPtr.Zero;
                    if (winErr == 38)//достигнут коней файла - потоков нет (?)
                    {
                        return ret_list;
                    }
                    else
                    {
                        throw new Win32Exception(winErr);
                    }
                }
                if (search_handle != IntPtr.Zero)
                {
                    ret_list.Add(find_data);
                    while (true)
                    {
                        res = WinApiFS.FindNextStreamW(search_handle, ref find_data);
                        if (res == 0)
                        {
                            winErr = Marshal.GetLastWin32Error();
                            if (winErr != 38) //no more data
                            {
                                throw new Win32Exception(winErr);
                            }
                            break;
                        }
                        else
                        {
                            ret_list.Add(find_data);
                        }
                    }
                }
                return ret_list;
            }
            finally
            {
                if (search_handle != IntPtr.Zero)
                {
                    WinApiFS.FindClose(search_handle);
                }
            }
        }

        public static List<WIN32_FIND_DATA> GetFilesInDirectory(string directory_name)
        {
            List<WIN32_FIND_DATA> ret_list = new List<WIN32_FIND_DATA>();
            IntPtr search_handle = IntPtr.Zero;
            int res = 1;
            WIN32_FIND_DATA find_data = new WIN32_FIND_DATA();
            int winErr = 0;
            string to_search = IOhelper.GetUnicodePath(directory_name + Path.DirectorySeparatorChar + '*');
            try
            {
                //open search handle
                search_handle = WinApiFS.FindFirstFile(to_search, ref find_data);
                if (search_handle.ToInt32() == WinApiFS.INVALID_HANDLE_VALUE)
                {
                    winErr = Marshal.GetLastWin32Error();
                    search_handle = IntPtr.Zero;
                    if (winErr == 2) //file not found
                    {
                        return ret_list;
                    }
                    else
                    {
                        throw new Win32Exception(winErr);
                    }
                }
                ret_list.Add(find_data);
                while (true)
                {
                    res = WinApiFS.FindNextFile(search_handle, ref find_data);
                    if (res == 0) //1008
                    {
                        winErr = Marshal.GetLastWin32Error();
                        if (winErr == 18) //no more data
                        {
                            break;
                        }
                        else
                        {
                            throw new Win32Exception(winErr);
                        }
                    }
                    else
                    {
                        ret_list.Add(find_data);
                    }
                }
                return ret_list;
            }
            finally
            {
                if (search_handle != IntPtr.Zero)
                {
                    WinApiFS.FindClose(search_handle);
                }
            }
        }

        public static bool GetFileInfo(string file_name, ref WIN32_FIND_DATA file_info)
        {
            IntPtr search_handle = IntPtr.Zero;
            try
            {
                search_handle = WinApiFS.FindFirstFile(file_name, ref file_info);
                if (search_handle.ToInt32() == WinApiFS.INVALID_HANDLE_VALUE)
                {
                    int winErr = Marshal.GetLastWin32Error();
                    search_handle = IntPtr.Zero;
                    if (winErr == 2) //file not found
                    {
                        //if file_name is 'c:\'?
                        //i.e. root directory
                        if (Path.GetPathRoot(file_name) == file_name)
                        {
                            //root exist?
                            DriveInfo[] d_infos = DriveInfo.GetDrives();
                            bool exists = false;
                            foreach (DriveInfo di in d_infos)
                            {
                                if (di.RootDirectory.Name == file_name)
                                {
                                    exists = true;
                                    break;
                                }
                            }
                            if (exists)
                            {
                                file_info.cAlternateFileName = string.Empty;
                                file_info.cFileName = string.Empty;
                                file_info.dwFileAttributes = FileAttributes.Directory;
                                return true;
                            }
                        }
                        return false;
                    }
                    else if (winErr == 3) //path not found
                    {
                        return false;
                    }
                    else if (winErr == 123) //ERROR_INVALID_NAME
                    {
                        return false;
                    }
                        /*
                         * fix problem while copy to network smb root path
                         */
                    else if (winErr == 5) //access denied
                    {
                        //may be if path is smb resource root, i.e. \\server\resource
                        string err5_root = Path.GetPathRoot(file_name);
                        file_info.dwFileAttributes = FileAttributes.Directory;
                        return (err5_root == file_name);
                    }
                    else if (winErr == 53) //network path not found
                    {
                        string err53_root = Path.GetPathRoot(file_name);
                        file_info.dwFileAttributes = FileAttributes.Directory;
                        return (err53_root == file_name);
                    }
                    else
                    {
                        //throw new Win32Exception(winErr);
                        return false;
                    }
                }
                else
                {
                    return true;
                }
            }
            finally
            {
                if (search_handle != IntPtr.Zero)
                {
                    WinApiFS.FindClose(search_handle);
                }
            }
        }

        public static FileTransferResult DeleteFile(CopyFileItemEx item)
        {

            int res = 0;
            try
            {
                FileInfo source_info = new FileInfo(item.SourceFile);

                if ((source_info.Attributes & FileAttributes.ReadOnly) == FileAttributes.ReadOnly)
                {
                    if (item.SkipReadonly)
                    {
                        return new FileTransferResult(item.SourceFile, string.Empty, TransferResult.Skip);
                    }
                    else if (!item.OverwriteReadonly)
                    {
                        return new FileTransferResult(item.SourceFile, string.Empty, TransferResult.Fail_DestinationReadOnly);
                    }
                    else
                    {
                        source_info.Attributes = source_info.Attributes & (~FileAttributes.ReadOnly);
                    }
                }
                res = WinApiFS.DeleteFile(item.SourceFile);
                if (res == 0)
                {
                    return new FileTransferResult(item.SourceFile, string.Empty, TransferResult.Fail_Other, new Win32Exception(Marshal.GetLastWin32Error()));
                }
                else
                {
                    return new FileTransferResult(item.SourceFile, string.Empty);
                }
            }
            catch (Exception ex)
            {
                return new FileTransferResult(item.SourceFile, string.Empty, TransferResult.Fail_Other, ex);
            }
        }


        //public static FileTransferResult CopyStreamEx(CopyFileItemEx item, CopyProgressRoutine callback)
        //{
        //    //копировщик для (возможно) любых потоков

        //    FileStream stream_source = null;
        //    FileStream stream_destination = null;
        //    FileTransferResult ret = null;

        //    try
        //    {
        //        //открываем источник
        //        stream_source = CreateStreamEx
        //            (item.SourceFile,
        //            FileAccess.Read,
        //            FileShare.Read,
        //            FileMode.Open,
        //            CreateFileOptions.SEQUENTIAL_SCAN);

        //        //открываем назначение
        //        //по идее, если файл(=поток) уже существует и OverwriteExisting не установлено,
        //        //то будет исключение
        //        //(при FileMode.CreateNew CreateStreamEx должен выкинуть исключение, если файл уже существует)
        //        //Возможно лучше перед копированием сразу выделить место на диске
        //        //как это делает COpyFileEx 
        //        FileMode dest_mode = FileMode.CreateNew;
        //        if (item.OverwriteExisting)
        //        {
        //            dest_mode = FileMode.Create;
        //        }
        //        stream_destination = CreateStreamEx
        //            (item.DestinationFile,
        //            FileAccess.Write,
        //            FileShare.Read,
        //            dest_mode,
        //            CreateFileOptions.SEQUENTIAL_SCAN);

        //        ulong bytes_total_transferred = 0;
        //        ulong bytes_source_len = (ulong)stream_source.Length;
        //        int readed = 0;
        //        int buffer_len = Options.StreamCopyBufferSize;
        //        byte[] buffer = new byte[buffer_len];
        //        bool quiet = (callback == null);
        //        IntPtr handle_source = stream_source.SafeFileHandle.DangerousGetHandle();
        //        IntPtr handle_destination = stream_destination.SafeFileHandle.DangerousGetHandle();
        //        bool cancelled = false;

        //        //вызываем callback перед началом копирования
        //        if (!quiet)
        //        {
        //            callback
        //                (bytes_source_len,
        //                0,
        //                bytes_source_len,
        //                0,
        //                0,
        //                CopyFileExState.STREAM_SWITCH,
        //                handle_source,
        //                handle_destination,
        //                IntPtr.Zero);
        //        }

        //        //повторяем пока не прочитаются весь исходлный поток
        //        while (bytes_total_transferred < bytes_source_len)
        //        {
        //            //читаем в буфер
        //            readed = stream_source.Read(buffer, 0, buffer_len);
        //            //записываем в целевой поток readed байт
        //            stream_destination.Write(buffer, 0, readed);
        //            //обновляем счетчик
        //            bytes_total_transferred += (ulong)readed;
        //            //вызываем callbcak
        //            if (!quiet)
        //            {
        //                CopyFileExCallbackReturns call_ret = callback
        //                    (bytes_source_len,
        //                    bytes_total_transferred,
        //                    bytes_source_len,
        //                    bytes_total_transferred,
        //                    0,
        //                    CopyFileExState.CHUNK_FINISHED,
        //                    handle_source,
        //                    handle_destination,
        //                    IntPtr.Zero);

        //                //обработка call_ret
        //                switch (call_ret)
        //                {
        //                    case CopyFileExCallbackReturns.CANCEL:
        //                    case CopyFileExCallbackReturns.STOP:
        //                        //завершаем чтение-запись с удалением destination
        //                        stream_destination.Close();
        //                        stream_source.Close();

        //                        CopyFileItemEx del_item = new CopyFileItemEx();
        //                        del_item.OverwriteExisting = true;
        //                        del_item.OverwriteHidden = true;
        //                        del_item.OverwriteReadonly = true;
        //                        del_item.SourceFile = item.DestinationFile;
        //                        FileTransferResult del_res = DeleteFile(del_item);
        //                        if (del_res.Exception != null)
        //                        {
        //                            throw del_res.Exception;
        //                        }
        //                        cancelled = true;
        //                        break;

        //                    case CopyFileExCallbackReturns.QUIET:
        //                        quiet = true;
        //                        break;
        //                }//end switch

        //                if (cancelled)
        //                {
        //                    ret = new FileTransferResult
        //                        (item.SourceFile, item.DestinationFile, TransferResult.Query_Abort);
        //                    break;
        //                }
        //            }//end if(!quiet)
        //        }//end while

        //        if (!cancelled)
        //        {
        //            ret = new FileTransferResult(item.SourceFile, item.DestinationFile, TransferResult.OK);
        //        }

        //        //вызываем callback после окончания копирования
        //        //но только если не cancelled - в этом случае файловые хэндлы
        //        //уже будут недействительны
        //        if ((!quiet) && (!cancelled))
        //        {
        //            callback
        //                (bytes_source_len,
        //                bytes_total_transferred,
        //                bytes_source_len,
        //                bytes_total_transferred,
        //                0,
        //                CopyFileExState.STREAM_SWITCH,
        //                handle_source,
        //                handle_destination,
        //                IntPtr.Zero);
        //        }
        //    }//end try
        //    catch (Exception ex)
        //    {
        //        ret = new FileTransferResult(item.SourceFile, item.DestinationFile, TransferResult.Fail_Other, ex);
        //    }
        //    finally
        //    {
        //        if (stream_source != null)
        //        {
        //            stream_source.Close();
        //        }
        //        if (stream_destination != null)
        //        {
        //            stream_destination.Close();
        //        }
        //    }

        //    return ret;
        //}

        //    public static FileTransferResult CopyFileEx(CopyFileItemEx item, CopyProgressRoutine callback)
        //    {
        //        try
        //        {
        //            FileInfoEx destination_info = new FileInfoEx(item.DestinationFile);
        //            FileInfoEx source_info = new FileInfoEx(item.SourceFile);

        //            bool destination_hidden = false;
        //            bool destination_readonly = false;

        //            if (destination_info.Exist)
        //            {
        //                //see on options

        //                destination_hidden = (destination_info.Attributes & FileAttributes.Hidden) == FileAttributes.Hidden;
        //                destination_readonly = (destination_info.Attributes & FileAttributes.ReadOnly) == FileAttributes.ReadOnly;

        //                if ((item.SkipExisting) && (!item.OverwriteExisting))
        //                {
        //                    return new FileTransferResult(item.SourceFile, item.DestinationFile, TransferResult.Skip);
        //                }

        //                if ((item.SkipHidden) && (!item.OverwriteHidden) && destination_hidden)
        //                {
        //                    return new FileTransferResult(item.SourceFile, item.DestinationFile, TransferResult.Skip);
        //                }

        //                if ((item.SkipReadonly) && (!item.OverwriteReadonly) && destination_readonly)
        //                {
        //                    return new FileTransferResult(item.SourceFile, item.DestinationFile, TransferResult.Skip);
        //                }

        //                if ((item.OverwriteExisting || item.OverwriteHidden || item.OverwriteReadonly) && (item.OverwriteOnlyIfSourceNewer) && (DateTime.Compare(destination_info.LastWriteTime, source_info.LastWriteTime) >= 0))
        //                {
        //                    return new FileTransferResult(item.SourceFile, item.DestinationFile, TransferResult.Skip);
        //                }

        //                if (destination_hidden)
        //                {
        //                    if (item.OverwriteHidden)
        //                    {
        //                        //reset hidden attr
        //                        destination_info.Attributes = destination_info.Attributes & (~FileAttributes.Hidden);
        //                    }
        //                    else
        //                    {
        //                        return new FileTransferResult(item.SourceFile, item.DestinationFile, TransferResult.Fail_DestinationHidden);
        //                    }
        //                }
        //                if (destination_readonly)
        //                {
        //                    if (item.OverwriteReadonly)
        //                    {
        //                        destination_info.Attributes = destination_info.Attributes & (~FileAttributes.ReadOnly);
        //                    }
        //                    else
        //                    {
        //                        //if (destination_hidden)
        //                        //{
        //                        //    //restore hidden flag
        //                        //    destination_info.Attributes = destination_info.Attributes | FileAttributes.Hidden;
        //                        //}
        //                        return new FileTransferResult(item.SourceFile, item.DestinationFile, TransferResult.Fail_DestinationReadOnly);
        //                    }
        //                }
        //                if (!item.OverwriteExisting)
        //                {
        //                    return new FileTransferResult(item.SourceFile, item.DestinationFile, TransferResult.Fail_DestinationExist);
        //                }

        //            }

        //            int res = 0;
        //            int ptCancel = 0;
        //            CopyFileExOptions copy_opts = CopyFileExOptions.None;

        //            if (item.AllowDecriptDestination)
        //            {
        //                copy_opts = copy_opts | CopyFileExOptions.ALLOW_DECRYPTED_DESTINATION;
        //            }

        //            res = Native.CopyFileEx
        //                (item.SourceFile,
        //                item.DestinationFile,
        //                callback,
        //                IntPtr.Zero,
        //                ref ptCancel,
        //                copy_opts);

        //            if (res == 0)
        //            {
        //                int winErr = Marshal.GetLastWin32Error();
        //                if (winErr == 1235) //error_query_abort
        //                {
        //                    return new FileTransferResult(item.SourceFile, item.DestinationFile, TransferResult.Query_Abort);
        //                }
        //                else if (winErr == 3)//path not found
        //                {
        //                    return new FileTransferResult(item.SourceFile, item.DestinationFile, TransferResult.Fail_DestinationDirNotFound);
        //                }
        //                else
        //                {
        //                    return new FileTransferResult(item.SourceFile, item.DestinationFile, TransferResult.Fail_Other, new Win32Exception(winErr));
        //                }
        //            }
        //            else
        //            {
        //                return new FileTransferResult(item.SourceFile, item.DestinationFile);
        //            }
        //        }
        //        catch (Exception ex)
        //        {
        //            return new FileTransferResult(item.SourceFile, item.DestinationFile, TransferResult.Fail_Other, ex);
        //        }
        //    }
        //}

        public class FileTransferResult
        {
            public string SourceFile { get; private set; }
            public string DestinationFile { get; private set; }
            public TransferResult ResultCode { get; set; }
            public Exception Exception { get; private set; }


            public FileTransferResult(string source, string destination)
            {
                SourceFile = source;
                DestinationFile = destination;
                ResultCode = TransferResult.OK;
                Exception = null;
            }

            public FileTransferResult(string source, string destination, TransferResult result)
            {
                SourceFile = source;
                DestinationFile = destination;
                ResultCode = result;
                Exception = null;
            }

            public FileTransferResult(string source, string destination, TransferResult result, Exception resultException)
            {
                SourceFile = source;
                DestinationFile = destination;
                ResultCode = result;
                Exception = resultException;
            }
        }

        public class CopyFileItemEx
        {
            public bool OverwriteExisting { get; set; }
            public bool SkipExisting { get; set; }
            public bool OverwriteReadonly { get; set; }
            public bool SkipReadonly { get; set; }
            public bool OverwriteHidden { get; set; }
            public bool SkipHidden { get; set; }
            public bool AllowDecriptDestination { get; set; }
            public bool OverwriteOnlyIfSourceNewer { get; set; }
            public string SourceFile { get; set; }
            public string DestinationFile { get; set; }

            public CopyFileItemEx()
            {
                AllowDecriptDestination = false;
                SkipExisting = false;
                SkipHidden = false;
                SkipReadonly = false;
                OverwriteExisting = false;
                OverwriteHidden = false;
                OverwriteReadonly = false;
                OverwriteOnlyIfSourceNewer = false;
            }

            public CopyFileItemEx UseAsTemplate(string source, string destination)
            {
                CopyFileItemEx ret = new CopyFileItemEx();
                ret.AllowDecriptDestination = AllowDecriptDestination;
                ret.SkipExisting = SkipExisting;
                ret.SkipHidden = SkipHidden;
                ret.SkipReadonly = SkipReadonly;
                ret.OverwriteExisting = OverwriteExisting;
                ret.OverwriteHidden = OverwriteHidden;
                ret.OverwriteOnlyIfSourceNewer = OverwriteOnlyIfSourceNewer;
                ret.OverwriteReadonly = OverwriteReadonly;
                ret.DestinationFile = destination;
                ret.SourceFile = source;

                return ret;
            }

            public CopyFileItemEx UseAsTemplate(TransferFileItem pair)
            {
                return UseAsTemplate(pair.SourceFile, pair.DestinationFile);
            }
        }

        public class TransferFileItem
        {
            public string SourceFile { get; set; }
            public string DestinationFile { get; set; }

            public TransferFileItem()
            {
                SourceFile = string.Empty;
                DestinationFile = string.Empty;
            }

            public TransferFileItem(string source, string destination)
            {
                SourceFile = source;
                DestinationFile = destination;
            }
        }

        

        public struct API_LONG
        {
            public uint Low;
            public uint High;
            public ulong GetUlong()
            {
                byte[] low_bytes = BitConverter.GetBytes(Low);
                byte[] high_bytes = BitConverter.GetBytes(High);
                byte[] ulong_bytes = new byte[8];
                for (int i = 0; i < 4; i++)
                {
                    ulong_bytes[i] = low_bytes[i];
                    ulong_bytes[i + 4] = high_bytes[i];
                }
                return BitConverter.ToUInt64(ulong_bytes, 0);
            }
        }

        #region enum WIN32_FIND_DATA
        public class WIN32_FIND_DATA_enumerable : IEnumerable<WIN32_FIND_DATA>
        {

            private InternalEnumerator internal_enumerator = null;

            public WIN32_FIND_DATA_enumerable(string search_path)
            {
                internal_enumerator = new InternalEnumerator(search_path, false);
            }

            public WIN32_FIND_DATA_enumerable(string search_path, bool supress_access_denied)
            {
                internal_enumerator = new InternalEnumerator(search_path, supress_access_denied);
            }

            public WIN32_FIND_DATA_enumerable(string search_path, bool supress_access_denied, bool supress_path_not_found)
            {
                internal_enumerator = new InternalEnumerator(search_path, supress_access_denied, supress_path_not_found);
            }

            #region IEnumerable<WIN32_FIND_DATA> Members

            public IEnumerator<WIN32_FIND_DATA> GetEnumerator()
            {
                return internal_enumerator;
            }

            #endregion

            #region IEnumerable Members

            System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
            {
                return internal_enumerator;
            }

            #endregion

            private class InternalEnumerator : IEnumerator<WIN32_FIND_DATA>
            {
                public InternalEnumerator(string search_path, bool supress_access_denied)
                {
                    internal_path_mask = search_path;
                    this.supress_access_denied = supress_access_denied;
                }

                public InternalEnumerator(string search_path, bool supress_access_denied,bool supress_path_not_found)
                {
                    internal_path_mask = search_path;
                    this.supress_access_denied = supress_access_denied;
                    this.supress_path_not_found = supress_path_not_found;
                }

                private bool supress_path_not_found=false;
                private bool supress_access_denied = false; //win error 5
                private IntPtr search_handle = IntPtr.Zero;
                private WIN32_FIND_DATA internal_current = new WIN32_FIND_DATA();
                private string internal_path_mask = string.Empty;

                #region IEnumerator<WIN32_FIND_DATA> Members

                public WIN32_FIND_DATA Current
                {
                    get { return internal_current; }
                }

                #endregion

                #region IDisposable Members

                public void Dispose()
                {
                    if (search_handle != IntPtr.Zero)
                    {
                        WinApiFS.FindClose(search_handle);
                    }
                }

                #endregion

                #region IEnumerator Members

                object System.Collections.IEnumerator.Current
                {
                    get { return internal_current; }
                }

                public bool MoveNext()
                {
                    int res = 0;
                    int winErr = 0;

                    if (search_handle == IntPtr.Zero)
                    {
                        //try init search
                        search_handle = WinApiFS.FindFirstFile(internal_path_mask, ref internal_current);
                        if (search_handle.ToInt32() == WinApiFS.INVALID_HANDLE_VALUE)
                        {
                            winErr = Marshal.GetLastWin32Error();
                            
                            if (winErr == WinApiFS.ERROR_FILE_NOT_FOUND) //file not found
                            {
                                return false;
                            }

                            if (winErr == WinApiFS.ERROR_ACCESS_DENIED) //access denied
                            {
                                if (supress_access_denied)
                                {
                                    return false;
                                }
                                else
                                {
                                    throw new Win32Exception(winErr);
                                }
                            }

                            if (winErr == WinApiFS.ERROR_PATH_NOT_FOUND)
                            {
                                if (supress_path_not_found)
                                {
                                    return false;
                                }
                                else
                                {
                                    throw new Win32Exception(winErr);
                                }
                            }

                            throw new Win32Exception(winErr);
                        }
                        else
                        {
                            return true;
                        }
                    }
                    else
                    {
                        res = WinApiFS.FindNextFile(search_handle, ref internal_current);
                        if (res == 0)
                        {
                            winErr = Marshal.GetLastWin32Error();
                            WinApiFS.FindClose(search_handle);
                            if (winErr == 18) //no more files
                            {
                                return false;
                            }
                            else
                            {
                                throw new Win32Exception(winErr);
                            }
                        }
                        else
                        {
                            return true;
                        }
                    }
                }

                public void Reset()
                {
                    throw new NotImplementedException();
                }

                #endregion
            }
        }
        #endregion

        

        public enum TransferResult
        {
            OK,
            Skip,
            Query_Abort,
            Fail_DestinationExist,
            Fail_DestinationReadOnly,
            Fail_DestinationHidden,
            Fail_CannotEncrypt,
            Fail_DestinationDirNotFound,
            Fail_Other
        }
    }

    public class FileStreamInfo
    {
        private string intern_stream_name = string.Empty;
        private ulong intern_stream_size = 0;
        private FileStreamID intern_stream_id = FileStreamID.DATA;
        private FileStreamAttributes intern_stream_attributes = FileStreamAttributes.NORMAL;

        public FileStreamInfo(string stream_name, ulong stream_size, FileStreamID id, FileStreamAttributes attributes)
        {
            intern_stream_name = stream_name;
            intern_stream_size = stream_size;
            intern_stream_id = id;
            intern_stream_attributes = attributes;
        }

        public FileStreamID ID
        {
            get
            {
                return intern_stream_id;
            }
        }

        public FileStreamAttributes Attributes
        {
            get
            {
                return intern_stream_attributes;
            }
        }

        public string Name
        {
            get
            {
                if (intern_stream_name == string.Empty)
                {
                    return string.Empty;
                }
                else
                {
                    int index_of_tt = intern_stream_name.IndexOf(":$");
                    if (index_of_tt >= 0)
                    {
                        return intern_stream_name.Substring(0, index_of_tt);
                    }
                    else
                    {
                        return intern_stream_name;
                    }
                }
            }
        }

        public ulong Size
        {
            get
            {
                return intern_stream_size;
            }
        }

    }
}
