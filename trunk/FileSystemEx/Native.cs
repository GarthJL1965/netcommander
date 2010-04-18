using System;
using System.IO;
using System.Runtime.InteropServices;

namespace netCommander.FileSystemEx
{
    class WinApiFS
    {
        private WinApiFS()
        {
        }





        [DllImport("shell32.dll", CharSet = CharSet.Unicode)]
        public static extern int SHFileOperation(ref SHFILEOPSTRUCT lpFileOp);


        public static uint CTL_CODE_MACRO(uint DeviceType, uint Function, uint Method, uint Access)
        {
            return ((DeviceType << 16) | (Access << 14) | (Function << 2) | Method);
        }

        [DllImport("advapi32.dll", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool AdjustTokenPrivileges(IntPtr TokenHandle,
            [MarshalAs(UnmanagedType.Bool)]bool DisableAllPrivileges,
            ref TOKEN_PRIVILEGES_ONE NewState,
            Int32 BufferLength,
            //ref TOKEN_PRIVILEGES PreviousState,					!! for some reason this won't accept null
            IntPtr PreviousState,
            IntPtr ReturnLength);

        [DllImport("advapi32.dll", SetLastError = true, CharSet = CharSet.Auto)]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool LookupPrivilegeValue(string lpSystemName, string lpName,
            out LUID lpLuid);

        [DllImport("advapi32.dll", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool OpenProcessToken(IntPtr ProcessHandle,
            UInt32 DesiredAccess, out IntPtr TokenHandle);

        [DllImport("kernel32.dll")]
        public static extern IntPtr GetCurrentProcess();

        #region DeviceIOCOntrol
        [DllImport("kernel32.dll", SetLastError = true, CharSet = CharSet.Unicode, EntryPoint = "DeviceIoControl")]
        public static extern int DeviceIoControl
            (IntPtr hDevice,
           uint dwIoControlCode,
            IntPtr InBuffer,
            int nInBufferSize,
            IntPtr OutBuffer,
            int nOutBufferSize,
            ref int pBytesReturned,
            IntPtr lpOverlapped);

        [DllImport("kernel32.dll", SetLastError = true, CharSet = CharSet.Unicode, EntryPoint = "DeviceIoControl")]
        public static extern int DeviceIoControl
            (IntPtr hDevice,
            uint dwIoControlCode,
            IntPtr InBuffer,
            int nInBufferSize,
            ref REPARSE_DATA_BUFFER_MOUNTPOINT OutBuffer,
            int nOutBufferSize,
            ref int pBytesReturned,
            IntPtr lpOverlapped);

        [DllImport("kernel32.dll", SetLastError = true, CharSet = CharSet.Unicode, EntryPoint = "DeviceIoControl")]
        public static extern int DeviceIoControl
            (IntPtr hDevice,
            uint dwIoControlCode,
            IntPtr InBuffer,
            int nInBufferSize,
            ref REPARSE_DATA_BUFFER_SYMLINK OutBuffer,
            int nOutBufferSize,
            ref int pBytesReturned,
            IntPtr lpOverlapped);

        [DllImport("kernel32.dll", SetLastError = true, CharSet = CharSet.Unicode, EntryPoint = "DeviceIoControl")]
        public static extern int DeviceIoControl
            (IntPtr hDevice,
            uint dwIoControlCode,
            ref REPARSE_DATA_BUFFER_MOUNTPOINT InBuffer,
            int nInBufferSize,
            IntPtr OutBuffer,
            int nOutBufferSize,
            ref int pBytesReturned,
            IntPtr lpOverlapped);

        [DllImport("kernel32.dll", SetLastError = true, CharSet = CharSet.Unicode, EntryPoint = "DeviceIoControl")]
        public static extern int DeviceIoControl
            (IntPtr hDevice,
            uint dwIoControlCode,
            ref REPARSE_DATA_BUFFER_SYMLINK InBuffer,
            int nInBufferSize,
            IntPtr OutBuffer,
            int nOutBufferSize,
            ref int pBytesReturned,
            IntPtr lpOverlapped);
        #endregion

        [DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        public static extern int CopyFile
            ([MarshalAs(UnmanagedType.LPTStr)]
            string lpExistingFileName,
            [MarshalAs(UnmanagedType.LPTStr)]
            string lpNewFileName,
            [MarshalAs(UnmanagedType.Bool)]
            bool bFailIfExists);


        [DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        public static extern int GetFileSizeEx
            (IntPtr hFile,
            ref LARGE_INTEGER lpFileSize);

        [DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        public static extern int CreateDirectory
            ([MarshalAs(UnmanagedType.LPTStr)]
            string lpPathName,
            IntPtr lpSecurityAttributes);

        [DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        public static extern int CreateDirectoryEx
            ([MarshalAs(UnmanagedType.LPTStr)]
            string lpTemplateDirectory,
            [MarshalAs(UnmanagedType.LPTStr)]
            string lpNewDirectory,
            IntPtr lpSecurityAttributes);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="nBufferLength">in TCHARs</param>
        /// <param name="lpBuffer"></param>
        /// <returns></returns>
        [DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        public static extern int GetLogicalDriveStrings
            (int nBufferLength,
            IntPtr lpBuffer);

        [DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        public static extern int GetLogicalDrives();

        [DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        public static extern DriveType GetDriveType
            ([MarshalAs(UnmanagedType.LPTStr)]
            string lpRootPathName);

        [DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        public static extern int GetDiskFreeSpaceEx
            ([MarshalAs(UnmanagedType.LPTStr)]
            string lpDirectoryName,
            ref ulong lpFreeBytesAvailable,
            ref ulong lpTotalNumberOfBytes,
            ref ulong lpTotalNumberOfFreeBytes);


        /// <summary>
        /// Retrieves the name of a volume on a computer.
        /// FindFirstVolume is used to begin scanning the volumes of a computer.
        /// </summary>
        /// <param name="lpszVolumeName_buffer">A pointer to a buffer that receives a null-terminated
        /// string that specifies a volume GUID path for the first volume that is found.</param>
        /// <param name="cchBufferLength">The length of the buffer to receive the volume GUID path, in TCHARs.</param>
        /// <returns>If the function succeeds, the return value is a search handle used
        /// in a subsequent call to the FindNextVolume and FindVolumeClose functions.
        /// If the function fails to find any volumes, the return value
        /// is the INVALID_HANDLE_VALUE error code. To get extended error information,
        /// call GetLastError.</returns>
        [DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        public static extern IntPtr FindFirstVolume
            (IntPtr lpszVolumeName_buffer,
            int cchBufferLength);

        /// <summary>
        /// Retrieves the name of a mounted folder on the specified volume. 
        /// FindFirstVolumeMountPoint is used to begin scanning the mounted folders on a volume.
        /// </summary>
        /// <param name="lpszRootPathName">A volume GUID path for the volume to scan for mounted folders.
        /// A trailing backslash is required.</param>
        /// <param name="lpszVolumeMountPoint_buffer">A pointer to a buffer that receives
        /// the name of the first mounted folder that is found.</param>
        /// <param name="cchBufferLength">The length of the buffer that receives the path to the mounted folder, in TCHARs.</param>
        /// <returns>If the function succeeds, the return value is a search handle used
        /// in a subsequent call to the FindNextVolumeMountPoint and FindVolumeMountPointClose 
        /// functions.If the function fails to find a mounted folder on the volume,
        /// the return value is the INVALID_HANDLE_VALUE error code. To get extended
        /// error information, call GetLastError.</returns>
        [DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        public static extern IntPtr FindFirstVolumeMountPoint
            ([MarshalAs(UnmanagedType.LPTStr)]
            string lpszRootPathName,
            IntPtr lpszVolumeMountPoint_buffer,
            int cchBufferLength);

        /// <summary>
        /// Continues a volume search started by a call to the FindFirstVolume function.
        /// FindNextVolume finds one volume per call.
        /// </summary>
        /// <param name="hFindVolume">The volume search handle returned by a previous call to the FindFirstVolume function.</param>
        /// <param name="lpszVolumeName_buffer">A pointer to a string that receives the volume GUID path that is found.</param>
        /// <param name="cchBufferLength">The length of the buffer that receives the volume GUID path, in TCHARs.</param>
        /// <returns>If the function succeeds, the return value is nonzero.
        /// If the function fails, the return value is zero.
        /// To get extended error information, call GetLastError. 
        /// If no matching files can be found, the GetLastError function returns
        /// the ERROR_NO_MORE_FILES error code. In that case, close the search with the FindVolumeClose function.</returns>
        [DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        public static extern int FindNextVolume
            (IntPtr hFindVolume,
            IntPtr lpszVolumeName_buffer,
            int cchBufferLength);

        /// <summary>
        /// Continues a mounted folder search started by a call to the FindFirstVolumeMountPoint function.
        /// FindNextVolumeMountPoint finds one mounted folder per call.
        /// </summary>
        /// <param name="hFindVolumeMountPoint">A mounted folder search handle returned by a previous call
        /// to the FindFirstVolumeMountPoint function.</param>
        /// <param name="lpszVolumeMountPoint_buffer">A pointer to a buffer that receives the name of the mounted folder that is found.</param>
        /// <param name="cchBufferLength">The length of the buffer that receives the mounted folder name, in TCHARs.</param>
        /// <returns>If the function succeeds, the return value is nonzero.
        /// If the function fails, the return value is zero. 
        /// To get extended error information, call GetLastError.
        /// If no more mounted folders can be found, the GetLastError function returns
        /// the ERROR_NO_MORE_FILES error code. In that case, close the search with
        /// the FindVolumeMountPointClose function.</returns>
        [DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        public static extern int FindNextVolumeMountPoint
            (IntPtr hFindVolumeMountPoint,
            IntPtr lpszVolumeMountPoint_buffer,
            int cchBufferLength);

        /// <summary>
        /// Closes the specified volume search handle. The FindFirstVolume
        /// and FindNextVolume functions use this search handle to locate volumes.
        /// </summary>
        /// <param name="hFindVolume">The volume search handle to be closed. 
        /// This handle must have been previously opened by the FindFirstVolume function.</param>
        /// <returns>If the function succeeds, the return value is nonzero.</returns>
        [DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        public static extern int FindVolumeClose
            (IntPtr hFindVolume);

        /// <summary>
        /// Closes the specified mounted folder search handle.
        /// The FindFirstVolumeMountPoint and FindNextVolumeMountPoint
        /// functions use this search handle to locate mounted folders on a specified volume.
        /// </summary>
        /// <param name="hFindVolumeMountPoint">The mounted folder search handle to be closed.
        /// This handle must have been previously opened by the FindFirstVolumeMountPoint function.</param>
        /// <returns>If the function succeeds, the return value is nonzero.</returns>
        [DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        public static extern int FindVolumeMountPointClose
            (IntPtr hFindVolumeMountPoint);


        [DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        public static extern int MoveFileWithProgress
            ([MarshalAs(UnmanagedType.LPTStr)]
            string lpExistingFileName,
            [MarshalAs(UnmanagedType.LPTStr)]
            string lpNewFileName,
            CopyProgressRoutine lpProgressRoutine,
            IntPtr lpData,
            MoveFileOptions dwFlags);

        [DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        public static extern int MoveFileWithProgress
            ([MarshalAs(UnmanagedType.LPTStr)]
            string lpExistingFileName,
            IntPtr lpNewFileName,
            IntPtr lpProgressRoutine,
            IntPtr lpData,
            MoveFileOptions dwFlags);

        /// <summary>
        /// Retrieves file information for the specified file.
        /// If FileInformationClass is FileStreamInfo and the calls succeed but no streams are returned,
        /// the error that is returned by GetLastError is ERROR_HANDLE_EOF.
        /// Requires Windows Vista
        /// </summary>
        /// <param name="hFile">A handle to the file that contains the information to be retrieved.</param>
        /// <param name="FileInformationClass">A FILE_INFO_BY_HANDLE_CLASS enumeration value that
        /// specifies the type of information to be retrieved.</param>
        /// <param name="lpFileInformation_buffer">A pointer to the buffer that receives
        /// the requested file information. The structure that is returned corresponds
        /// to the class that is specified by FileInformationClass.</param>
        /// <param name="dwBufferSize">The size of the lpFileInformation buffer, in bytes.</param>
        /// <returns>If the function succeeds, the return value is nonzero and file information data
        /// is contained in the buffer pointed to by the lpFileInformation parameter.</returns>
        [DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        public static extern int GetFileInformationByHandleEx
            (IntPtr hFile,
            FILE_INFO_BY_HANDLE_CLASS FileInformationClass,
            IntPtr lpFileInformation_buffer,
            int dwBufferSize);

        /// <summary>
        /// Retrieves file information for the specified file.
        /// </summary>
        /// <param name="hFile">A handle to the file that contains the information to be retrieved.
        /// This handle should not be a pipe handle.</param>
        /// <param name="lpFileInformation"></param>
        /// <returns>If the function succeeds, the return value is nonzero
        /// and file information data is contained in the buffer pointed
        /// to by the lpFileInformation parameter.</returns>
        [DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        public static extern int GetFileInformationByHandle
            (IntPtr hFile,
            ref BY_HANDLE_FILE_INFORMATION lpFileInformation);

        [DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        public static extern int GetBinaryType
            ([MarshalAs(UnmanagedType.LPTStr)]string lpApplicationName,
            ref FileBinaryType lpBinaryType);

        [DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        public static extern int MoveFileEx
            ([MarshalAs(UnmanagedType.LPTStr)]
            string lpExistingFileName,
            [MarshalAs(UnmanagedType.LPTStr)]
            string lpNewFileName,
            MoveFileOptions dwFlags);

        [DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        public static extern int MoveFileEx
            ([MarshalAs(UnmanagedType.LPTStr)]
            string lpExistingFileName,
            IntPtr lpNewFileName,
            MoveFileOptions dwFlags);

        /// <summary>
        /// delete empty directory
        /// </summary>
        /// <param name="lpPathName"></param>
        /// <returns></returns>
        [DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        public static extern int RemoveDirectory
            ([MarshalAs(UnmanagedType.LPTStr)]
            string lpPathName);

        [DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        public static extern int GetFullPathName
            ([MarshalAs(UnmanagedType.LPTStr)]
            string lpFileName,
            int nBufferLength,
            IntPtr lpBuffer,
            IntPtr lpFilePart);

        [DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        public static extern int CloseHandle(IntPtr hObject);

        [DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true, EntryPoint = "CreateFile")]
        public static extern IntPtr CreateFile_intptr
            ([MarshalAs(UnmanagedType.LPTStr)]
            string lpFileName,
            Win32FileAccess dwDesiredAccess,
            [MarshalAs(UnmanagedType.U4)]
            FileShare dwShareMode,
            IntPtr lpSecurityAttributes,
            [MarshalAs(UnmanagedType.U4)]
            FileMode dwCreationDisposition,
            CreateFileOptions dwFlagsAndAttributes,
            IntPtr hTemplateFile);

        [DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true, EntryPoint = "CreateFile")]
        public static extern Microsoft.Win32.SafeHandles.SafeFileHandle CreateFile_safe
            ([MarshalAs(UnmanagedType.LPTStr)]
            string lpFileName,
            Win32FileAccess dwDesiredAccess,
            [MarshalAs(UnmanagedType.U4)]
            FileShare dwShareMode,
            IntPtr lpSecurityAttributes,
            [MarshalAs(UnmanagedType.U4)]
            FileMode dwCreationDisposition,
            CreateFileOptions dwFlagsAndAttributes,
            IntPtr hTemplateFile);


        /// <summary>
        /// The BackupSeek function seeks forward in a data stream initially accessed
        /// by using the BackupRead or BackupWrite function.
        /// </summary>
        /// <param name="hFile">Handle to the file or directory.
        /// This handle is created by using the CreateFile function.</param>
        /// <param name="dwLowBytesToSeek">Low-order part of the number of bytes to seek.</param>
        /// <param name="dwHighBytesToSeek">High-order part of the number of bytes to seek.</param>
        /// <param name="lpdwLowByteSeeked">Pointer to a variable that receives the low-order bits
        /// of the number of bytes the function actually seeks.</param>
        /// <param name="lpdwHighByteSeeked">Pointer to a variable that receives
        /// the high-order bits of the number of bytes the function actually seeks.</param>
        /// <param name="lpContext">Pointer to an internal data structure used by the function.
        /// This structure must be the same structure that was initialized by the BackupRead function.
        /// An application must not touch the contents of this structure.</param>
        /// <returns>If the function could seek the requested amount, the function returns a nonzero value.</returns>
        [DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        public static extern int BackupSeek
            (IntPtr hFile,
            uint dwLowBytesToSeek,
            uint dwHighBytesToSeek,
            ref uint lpdwLowByteSeeked,
            ref uint lpdwHighByteSeeked,
            ref IntPtr lpContext);

        /// <summary>
        /// The BackupRead function can be used to back up a file or directory,
        /// including the security information. The function reads data associated
        /// with a specified file or directory into a buffer, which can then be
        /// written to the backup medium using the WriteFile function.
        /// </summary>
        /// <param name="hFile">Handle to the file or directory to be backed up. To obtain the handle,
        /// call the CreateFile function. The SACLs are not read unless the file handle was created
        /// with the ACCESS_SYSTEM_SECURITY access right. For more information, see File Security and Access Rights.</param>
        /// <param name="lpBuffer">Pointer to a buffer that receives the data.</param>
        /// <param name="nNumberOfBytesToRead">Length of the buffer, in bytes.
        /// The buffer size must be greater than the size of a WIN32_STREAM_ID structure.</param>
        /// <param name="lpNumberOfBytesRead">Pointer to a variable that receives the number of bytes read.
        /// If the function returns a nonzero value, and the variable pointed to by lpNumberOfBytesRead is zero,
        /// then all the data associated with the file handle has been read.</param>
        /// <param name="bAbort">Indicates whether you have finished using BackupRead on the handle.
        /// While you are backing up the file, specify this parameter as FALSE.
        /// Once you are done using BackupRead, you must call BackupRead one more
        /// time specifying TRUE for this parameter and passing the appropriate lpContext.
        /// lpContext must be passed when bAbort is TRUE; all other parameters are ignored.</param>
        /// <param name="bProcessSecurity">Indicates whether the function will restore
        /// the access-control list (ACL) data for the file or directory</param>
        /// <param name="lpContext">Pointer to a variable that receives a pointer to an
        /// internal data structure used by BackupRead to maintain context information
        /// during a backup operation. </param>
        /// <returns>If the function succeeds, the return value is nonzero.</returns>
        [DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        public static extern int BackupRead
            (IntPtr hFile,
            IntPtr lpBuffer,
            int nNumberOfBytesToRead,
            ref int lpNumberOfBytesRead,
            int bAbort,
            int bProcessSecurity,
            ref IntPtr lpContext);

        [DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        public static extern int BackupRead
            (IntPtr hFile,
            ref WIN32_STREAM_ID stream_info,
            int nNumberOfBytesToRead,
            ref int lpNumberOfBytesRead,
            int bAbort,
            int bProcessSecurity,
            ref IntPtr lpContext);

        /// <summary>
        /// only vista and server 2003 or later
        /// </summary>
        /// <param name="hFindStream"></param>
        /// <param name="lpFindStreamData"></param>
        /// <returns>If the function succeeds, the return value is nonzero. 
        /// If the function fails, the return value is zero. To get extended error information,
        /// call GetLastError. If no streams can be found, GetLastError returns ERROR_HANDLE_EOF.</returns>
        [DllImport("kernel32.dll", CharSet = CharSet.Unicode, SetLastError = true)]
        public static extern int FindNextStreamW
            (IntPtr hFindStream,
            ref WIN32_FIND_STREAM_DATA lpFindStreamData);

        /// <summary>
        /// only vista and server 2003 or later
        /// </summary>
        /// <param name="lpFileName"></param>
        /// <param name="InfoLevel"></param>
        /// <param name="lpFindStreamData"></param>
        /// <param name="dwFlags"></param>
        /// <returns></returns>
        [DllImport("kernel32.dll", CharSet = CharSet.Unicode, SetLastError = true)]
        public static extern IntPtr FindFirstStreamW
            ([MarshalAs(UnmanagedType.LPWStr)]
            string lpFileName,
            STREAM_INFO_LEVELS InfoLevel,
            ref WIN32_FIND_STREAM_DATA lpFindStreamData,
            int dwFlags);

        [DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        public static extern int SetFileAttributes
            ([MarshalAs(UnmanagedType.LPTStr)]
            string lpFileName,
            FileAttributes dwFileAttributes);

        [DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        public static extern int GetFileAttributesEx
            ([MarshalAs(UnmanagedType.LPTStr)]
            string lpFileName,
            GET_FILEEX_INFO_LEVELS fInfoLevelId,
            ref WIN32_FILE_ATTRIBUTE_DATA lpFileInformation);

        /// <summary>
        /// Moves an existing file or a directory, including its children.
        /// </summary>
        /// <param name="lpExistingFileName">The current name of the file or directory
        /// on the local computer. </param>
        /// <param name="lpNewFileName">The new name for the file or directory.
        /// The new name must not already exist. A new file may be on a different
        /// file system or drive. A new directory must be on the same drive.</param>
        /// <returns>If the function succeeds, the return value is nonzero</returns>
        [DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        public static extern int MoveFile
            ([MarshalAs(UnmanagedType.LPTStr)]
            string lpExistingFileName,
            [MarshalAs(UnmanagedType.LPTStr)]
            string lpNewFileName);

        /// <summary>
        /// Deletes an existing file
        /// </summary>
        /// <param name="lpFileName">The name of the file to be deleted.
        /// In the ANSI version of this function, the name is limited to MAX_PATH characters.
        /// To extend this limit to 32,767 wide characters, call the Unicode version of the function
        /// and prepend "\\?\" to the path. For more information, see Naming a File.</param>
        /// <returns>If the function succeeds, the return value is nonzero.</returns>
        [DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        public static extern int DeleteFile
            ([MarshalAs(UnmanagedType.LPTStr)]
            string lpFileName);

        /// <summary>
        /// Copies an existing file to a new file, notifying the application
        /// of its progress through a callback function
        /// </summary>
        /// <param name="lpExistingFileName">The name of an existing file. 
        /// In the ANSI version of this function, the name is limited to MAX_PATH characters.
        /// To extend this limit to 32,767 wide characters, call the Unicode version
        /// of the function and prepend "\\?\" to the path.</param>
        /// <param name="lpNewFileName">The name of the new file. 
        /// In the ANSI version of this function, the name is limited to MAX_PATH characters.
        /// To extend this limit to 32,767 wide characters, call the Unicode version
        /// of the function and prepend "\\?\" to the path.</param>
        /// <param name="lpProgressRoutine">The address of a callback function of type LPPROGRESS_ROUTINE
        /// that is called each time another portion of the file has been copied.
        /// This parameter can be NULL.</param>
        /// <param name="lpData">The argument to be passed to the callback function.
        /// This parameter can be NULL.</param>
        /// <param name="pbCancel">If this flag is set to TRUE during the copy operation, the operation is canceled.
        /// Otherwise, the copy operation will continue to completion.</param>
        /// <param name="dwCopyFlags"></param>
        /// <returns>If the function succeeds, the return value is nonzero</returns>
        [DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        public static extern int CopyFileEx
            ([MarshalAs(UnmanagedType.LPTStr)]
            string lpExistingFileName,
            [MarshalAs(UnmanagedType.LPTStr)]
            string lpNewFileName,
            CopyProgressRoutine lpProgressRoutine,
            IntPtr lpData,
            IntPtr pbCancel,
            CopyFileExOptions dwCopyFlags);

        [DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        public static extern int CopyFileEx
            ([MarshalAs(UnmanagedType.LPTStr)]
            string lpExistingFileName,
            [MarshalAs(UnmanagedType.LPTStr)]
            string lpNewFileName,
            CopyProgressRoutine lpProgressRoutine,
            IntPtr lpData,
            ref int pbCancel,
            CopyFileExOptions dwCopyFlags);

        [DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        public static extern IntPtr FindFirstFile
            ([MarshalAs(UnmanagedType.LPTStr)] string lpFileName,
            ref WIN32_FIND_DATA lpFindFileData);

        [DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        public static extern int FindNextFile
            (IntPtr hFindFile,
            ref WIN32_FIND_DATA lpFindFileData);

        [DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        public static extern int FindClose
        (IntPtr hFindFile);

        /// <summary>
        /// Establishes a hard link between an existing file and a new file.
        /// This function is only supported on the NTFS file system, and only for files, not directories.
        /// </summary>
        /// <param name="lpFileName">The name of the new file.</param>
        /// <param name="lpExistingFileName">The name of the existing file. </param>
        /// <param name="lpSecurityAttributes">Reserved; must be NULL</param>
        /// <returns>If the function succeeds, the return value is nonzero</returns>
        [DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        public static extern int CreateHardLink
            ([MarshalAs(UnmanagedType.LPTStr)]
            string lpFileName,
            [MarshalAs(UnmanagedType.LPTStr)]
            string lpExistingFileName,
            IntPtr lpSecurityAttributes);

        /// <summary>
        /// >=windows vista or server 2008
        /// </summary>
        /// <param name="lpSymlinkFileName"></param>
        /// <param name="lpTargetFileName"></param>
        /// <param name="isDirectory"></param>
        /// <returns></returns>
        [DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        public static extern int CreateSymbolicLink
            ([MarshalAs(UnmanagedType.LPTStr)]
            string lpSymlinkFileName,
            [MarshalAs(UnmanagedType.LPTStr)]
            string lpTargetFileName,
            int isDirectory);


        /// <summary>
        /// Retrieves information about the file system and
        /// volume associated with the specified root directory.
        /// </summary>
        /// <param name="lpRootPathName">A pointer to a string that contains the root directory
        /// of the volume to be described. If this parameter is NULL,
        /// the root of the current directory is used. A trailing backslash is required.
        /// For example, you specify \\MyServer\MyShare as "\\MyServer\MyShare\",
        /// or the C drive as "C:\".</param>
        /// <param name="lpVolumeNameBuffer">A pointer to a buffer that receives the name
        /// of a specified volume. The maximum buffer size is MAX_PATH+1.</param>
        /// <param name="nVolumeNameSize">The length of a volume name buffer, in TCHARs.
        /// The maximum buffer size is MAX_PATH+1.</param>
        /// <param name="lpVolumeSerialNumber">A pointer to a variable that receives
        /// the volume serial number. This parameter can be NULL if the serial number is not required.
        /// This function returns the volume serial number that the operating system assigns
        /// when a hard disk is formatted.
        /// To programmatically obtain the hard disk's serial number
        /// that the manufacturer assigns, use
        /// the Windows Management Instrumentation (WMI) Win32_PhysicalMedia property SerialNumber.</param>
        /// <param name="lpMaximumComponentLength">A pointer to a variable that receives the maximum length,
        /// in TCHARs, of a file name component that a specified file system supports. A file name component
        /// is the portion of a file name between backslashes. The value that is stored
        /// in the variable that *lpMaximumComponentLength points to is used to indicate
        /// that a specified file system supports long names. For example,
        /// for a FAT file system that supports long names, the function stores the value 255,
        /// rather than the previous 8.3 indicator. Long names can also be supported on systems
        /// that use the NTFS file system.</param>
        /// <param name="lpFileSystemFlags">A pointer to a variable that receives flags
        /// associated with the specified file system.</param>
        /// <param name="lpFileSystemNameBuffer">A pointer to a buffer that receives the name
        /// of the file system, for example, the FAT file system or the NTFS file system.
        /// The maximum buffer size is MAX_PATH+1.</param>
        /// <param name="nFileSystemNameSize">The length of the file system name buffer, in TCHARs.
        /// The maximum buffer size is MAX_PATH+1.</param>
        /// <returns>If all the requested information is retrieved, the return value is nonzero.</returns>
        [DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        public static extern int GetVolumeInformation
            ([MarshalAs(UnmanagedType.LPTStr)] string lpRootPathName,
            IntPtr lpVolumeNameBuffer,
            int nVolumeNameSize,
            IntPtr lpVolumeSerialNumber,
            IntPtr lpMaximumComponentLength,
            IntPtr lpFileSystemFlags,
            IntPtr lpFileSystemNameBuffer,
            int nFileSystemNameSize);


        public const int INVALID_HANDLE_VALUE = -1;
        public const uint MAXDWORD = 0xffffffff;
        public const int MAX_PATH = 260;

        /*
         * for use in WIN32_FIND_DATA
         */
        public const uint IO_REPARSE_TAG_DFS = 0x8000000A;
        public const uint IO_REPARSE_TAG_DFSR = 0x80000012;
        public const uint IO_REPARSE_TAG_HSM = 0xC0000004;
        public const uint IO_REPARSE_TAG_HSM2 = 0x80000006;
        public const uint IO_REPARSE_TAG_MOUNT_POINT = 0xA0000003;
        public const uint IO_REPARSE_TAG_SIS = 0x80000007;
        public const uint IO_REPARSE_TAG_SYMLINK = 0xA000000C;

        /*
         * file system capabilities
         */
        public const uint FILE_CASE_PRESERVED_NAMES = 0x00000002;
        public const uint FILE_CASE_SENSITIVE_SEARCH = 0x00000001;
        public const uint FILE_FILE_COMPRESSION = 0x00000010;
        public const uint FILE_NAMED_STREAMS = 0x00040000;
        public const uint FILE_PERSISTENT_ACLS = 0x00000008;
        public const uint FILE_READ_ONLY_VOLUME = 0x00080000;
        public const uint FILE_SEQUENTIAL_WRITE_ONCE = 0x00100000;
        public const uint FILE_SUPPORTS_ENCRYPTION = 0x00020000;
        public const uint FILE_SUPPORTS_OBJECT_IDS = 0x00010000;
        public const uint FILE_SUPPORTS_REPARSE_POINTS = 0x00000080;
        public const uint FILE_SUPPORTS_SPARSE_FILES = 0x00000040;
        public const uint FILE_SUPPORTS_TRANSACTIONS = 0x00200000;
        public const uint FILE_UNICODE_ON_DISK = 0x00000004;
        public const uint FILE_VOLUME_IS_COMPRESSED = 0x00008000;
        public const uint FILE_VOLUME_QUOTAS = 0x00000020;

        // Define CopyFileEx callback routine state change values
        public const int CALLBACK_CHUNK_FINISHED = 0x00000000;
        public const int CALLBACK_STREAM_SWITCH = 0x00000001;

        // Define possible return codes from the CopyFileEx callback routine
        public const int PROGRESS_CONTINUE = 0;
        public const int PROGRESS_CANCEL = 1;
        public const int PROGRESS_STOP = 2;
        public const int PROGRESS_QUIET = 3;

        // COpyFileEx flags
        public const int COPY_FILE_FAIL_IF_EXISTS = 0x00000001;
        public const int COPY_FILE_RESTARTABLE = 0x00000002;
        public const int COPY_FILE_OPEN_SOURCE_FOR_WRITE = 0x00000004;
        public const int COPY_FILE_ALLOW_DECRYPTED_DESTINATION = 0x00000008;
        public const int COPY_FILE_COPY_SYMLINK = 0x00000800;

        //Define access rights to files and directories
        public const uint DELETE = 0x00010000;
        public const uint READ_CONTROL = 0x00020000;
        public const uint WRITE_DAC = 0x00040000;
        public const uint WRITE_OWNER = 0x00080000;
        public const uint SYNCHRONIZE = 0x00100000;
        public const uint STANDARD_RIGHTS_REQUIRED = 0x000F0000;
        public const uint STANDARD_RIGHTS_READ = READ_CONTROL;
        public const uint STANDARD_RIGHTS_WRITE = READ_CONTROL;
        public const uint STANDARD_RIGHTS_EXECUTE = READ_CONTROL;
        public const uint FILE_READ_DATA = 0x0001;    // file & pipe
        public const uint FILE_LIST_DIRECTORY = 0x0001;    // directory
        public const uint FILE_WRITE_DATA = 0x0002;    // file & pipe
        public const uint FILE_ADD_FILE = 0x0002;    // directory
        public const uint FILE_APPEND_DATA = 0x0004;    // file
        public const uint FILE_ADD_SUBDIRECTORY = 0x0004;    // directory
        public const uint FILE_CREATE_PIPE_INSTANCE = 0x0004;    // named pipe
        public const uint FILE_READ_EA = 0x0008;    // file & directory
        public const uint FILE_WRITE_EA = 0x0010;    // file & directory
        public const uint FILE_EXECUTE = 0x0020;    // file
        public const uint FILE_TRAVERSE = 0x0020;    // directory
        public const uint FILE_DELETE_CHILD = 0x0040;    // directory
        public const uint FILE_READ_ATTRIBUTES = 0x0080;    // all
        public const uint FILE_WRITE_ATTRIBUTES = 0x0100;    // all
        public const uint FILE_ALL_ACCESS = STANDARD_RIGHTS_REQUIRED | SYNCHRONIZE | 0x1FF;
        public const uint FILE_GENERIC_READ = STANDARD_RIGHTS_READ | FILE_READ_DATA | FILE_READ_ATTRIBUTES | FILE_READ_EA | SYNCHRONIZE;
        public const uint FILE_GENERIC_WRITE = STANDARD_RIGHTS_WRITE | FILE_WRITE_DATA | FILE_WRITE_ATTRIBUTES | FILE_WRITE_EA | FILE_APPEND_DATA | SYNCHRONIZE;
        public const uint FILE_GENERIC_EXECUTE = STANDARD_RIGHTS_EXECUTE | FILE_READ_ATTRIBUTES | FILE_EXECUTE | SYNCHRONIZE;

        public const uint FILE_ATTRIBUTE_READONLY = 0x00000001;
        public const uint FILE_ATTRIBUTE_HIDDEN = 0x00000002;
        public const uint FILE_ATTRIBUTE_SYSTEM = 0x00000004;
        public const uint FILE_ATTRIBUTE_DIRECTORY = 0x00000010;
        public const uint FILE_ATTRIBUTE_ARCHIVE = 0x00000020;
        public const uint FILE_ATTRIBUTE_DEVICE = 0x00000040;
        public const uint FILE_ATTRIBUTE_NORMAL = 0x00000080;
        public const uint FILE_ATTRIBUTE_TEMPORARY = 0x00000100;
        public const uint FILE_ATTRIBUTE_SPARSE_FILE = 0x00000200;
        public const uint FILE_ATTRIBUTE_REPARSE_POINT = 0x00000400;
        public const uint FILE_ATTRIBUTE_COMPRESSED = 0x00000800;
        public const uint FILE_ATTRIBUTE_OFFLINE = 0x00001000;
        public const uint FILE_ATTRIBUTE_NOT_CONTENT_INDEXED = 0x00002000;
        public const uint FILE_ATTRIBUTE_ENCRYPTED = 0x00004000;
        public const uint FILE_ATTRIBUTE_VIRTUAL = 0x00010000;
        public const uint FILE_FLAG_WRITE_THROUGH = 0x80000000;
        public const uint FILE_FLAG_OVERLAPPED = 0x40000000;
        public const uint FILE_FLAG_NO_BUFFERING = 0x20000000;
        public const uint FILE_FLAG_RANDOM_ACCESS = 0x10000000;
        public const uint FILE_FLAG_SEQUENTIAL_SCAN = 0x08000000;
        public const uint FILE_FLAG_DELETE_ON_CLOSE = 0x04000000;
        public const uint FILE_FLAG_BACKUP_SEMANTICS = 0x02000000;
        public const uint FILE_FLAG_POSIX_SEMANTICS = 0x01000000;
        public const uint FILE_FLAG_OPEN_REPARSE_POINT = 0x00200000;
        public const uint FILE_FLAG_OPEN_NO_RECALL = 0x00100000;
        public const uint FILE_FLAG_FIRST_PIPE_INSTANCE = 0x00080000;

        //stream ID
        public const int BACKUP_INVALID = 0x00000000;
        public const int BACKUP_DATA = 0x00000001;
        public const int BACKUP_EA_DATA = 0x00000002;
        public const int BACKUP_SECURITY_DATA = 0x00000003;
        public const int BACKUP_ALTERNATE_DATA = 0x00000004;
        public const int BACKUP_LINK = 0x00000005;
        public const int BACKUP_PROPERTY_DATA = 0x00000006;
        public const int BACKUP_OBJECT_ID = 0x00000007;
        public const int BACKUP_REPARSE_DATA = 0x00000008;
        public const int BACKUP_SPARSE_BLOCK = 0x00000009;
        public const int BACKUP_TXFS_DATA = 0x0000000a;

        //stream attributes
        public const int STREAM_NORMAL_ATTRIBUTE = 0x00000000;
        public const int STREAM_MODIFIED_WHEN_READ = 0x00000001;
        public const int STREAM_CONTAINS_SECURITY = 0x00000002;
        public const int STREAM_CONTAINS_PROPERTIES = 0x00000004;
        public const int STREAM_SPARSE_ATTRIBUTE = 0x00000008;

        //move flags
        public const int MOVEFILE_COPY_ALLOWED = 0x2;
        public const int MOVEFILE_CREATE_HARDLINK = 0x10;
        public const int MOVEFILE_DELAY_UNTIL_REBOOT = 0x4;
        public const int MOVEFILE_FAIL_IF_NOT_TRACKABLE = 0x20;
        public const int MOVEFILE_REPLACE_EXISTING = 0x1;
        public const int MOVEFILE_WRITE_THROUGH = 0x8;

        //binary type
        public const int SCS_32BIT_BINARY = 0;
        public const int SCS_DOS_BINARY = 1;
        public const int SCS_WOW_BINARY = 2;
        public const int SCS_PIF_BINARY = 3;
        public const int SCS_POSIX_BINARY = 4;
        public const int SCS_OS216_BINARY = 5;
        public const int SCS_64BIT_BINARY = 6;

        //Control device codes
        public const uint FILE_DEVICE_FILE_SYSTEM = 0x00000009;

        //Control method codes
        public const uint METHOD_BUFFERED = 0;
        public const uint METHOD_IN_DIRECT = 1;
        public const uint METHOD_OUT_DIRECT = 2;
        public const uint METHOD_NEITHER = 3;

        //Control access codes
        public const uint FILE_ANY_ACCESS = 0;
        public const uint FILE_SPECIAL_ACCESS = FILE_ANY_ACCESS;
        public const uint FILE_READ_ACCESS = 0x0001;    // file & pipe
        public const uint FILE_WRITE_ACCESS = 0x0002;    // file & pipe

        //Control codes
        public static readonly uint FSCTL_SET_REPARSE_POINT = CTL_CODE_MACRO
            (FILE_DEVICE_FILE_SYSTEM,
            41,
            METHOD_BUFFERED,
            FILE_SPECIAL_ACCESS); // REPARSE_DATA_BUFFER
        public static readonly uint FSCTL_GET_REPARSE_POINT = CTL_CODE_MACRO
            (FILE_DEVICE_FILE_SYSTEM,
            42,
            METHOD_BUFFERED,
            FILE_ANY_ACCESS); // REPARSE_DATA_BUFFER
        public static readonly uint FSCTL_DELETE_REPARSE_POINT = CTL_CODE_MACRO
            (FILE_DEVICE_FILE_SYSTEM,
            43,
            METHOD_BUFFERED,
            FILE_SPECIAL_ACCESS); // REPARSE_DATA_BUFFER

        //error codes
        public const int ERROR_FILE_NOT_FOUND = 2;
        public const int ERROR_PATH_NOT_FOUND = 3;
        public const int ERROR_ACCESS_DENIED = 5;
        public const int ERROR_NOT_A_REPARSE_POINT = 4390;
        public const int ERROR_NO_MORE_FILES = 18;
        public const int ERROR_DIR_NOT_EMPTY = 145;
        public const int ERROR_FILE_EXISTS = 80;
        public const int ERROR_ALREADY_EXISTS = 183;
        public const int ERROR_INVALID_PARAMETER = 87;
        public const int ERROR_NOT_SAME_DEVICE = 17;
        public const int ERROR_CANCELLED = 1223;
        public const int ERROR_BAD_PATHNAME = 161;
        public const int ERROR_BUFFER_OVERFLOW = 111;
        public const int ERROR_WRITE_FAULT = 29;
        public const int ERROR_DISK_FULL = 112;
        public const int ERROR_READ_FAULT = 30;
        public const int ERROR_GEN_FAILURE = 31;

        public const uint TOKEN_ADJUST_PRIVILEGES = 0x0020;
        public const UInt32 SE_PRIVILEGE_ENABLED = 0x00000002;
        public const string SE_BACKUP_NAME = "SeBackupPrivilege";
        public const string SE_RESTORE_NAME = "SeRestorePrivilege";

        //file operations for shfileoperation
        public const int FO_MOVE = 0x0001;
        public const int FO_COPY = 0x0002;
        public const int FO_DELETE = 0x0003;
        public const int FO_RENAME = 0x0004;

        //file operation flags (WORD)
        public const ushort FOF_MULTIDESTFILES = 0x0001;
        public const ushort FOF_CONFIRMMOUSE = 0x0002;
        public const ushort FOF_SILENT = 0x0004;  // don't create progress/report
        public const ushort FOF_RENAMEONCOLLISION = 0x0008;
        public const ushort FOF_NOCONFIRMATION = 0x0010;  // Don't prompt the user.
        public const ushort FOF_WANTMAPPINGHANDLE = 0x0020;  // Fill in SHFILEOPSTRUCT.hNameMappings. Must be freed using SHFreeNameMappings
        public const ushort FOF_ALLOWUNDO = 0x0040;
        public const ushort FOF_FILESONLY = 0x0080;  // on *.*, do only files
        public const ushort FOF_SIMPLEPROGRESS = 0x0100;  // means don't show names of files
        public const ushort FOF_NOCONFIRMMKDIR = 0x0200;  // don't confirm making any needed dirs
        public const ushort FOF_NOERRORUI = 0x0400;  // don't put up error UI
        public const ushort FOF_NOCOPYSECURITYATTRIBS = 0x0800;  // dont copy NT file Security Attributes
        public const ushort FOF_NORECURSION = 0x1000;  // don't recurse ushorto directories.
        public const ushort FOF_NO_CONNECTED_ELEMENTS = 0x2000;  // don't operate on connected elements.
        public const ushort FOF_WANTNUKEWARNING = 0x4000;  // during delete operation, warn if nuking instead of recycling (partially overrides FOF_NOCONFIRMATION)
        public const ushort FOF_NORECURSEREPARSE = 0x8000;  // treat reparse points as objects, not containers
    }

    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
    public struct FILE_IO_PRIORITY_HINT_INFO
    {
        public PRIORITY_HINT PriorityHint;
    }

    [StructLayout(LayoutKind.Sequential, Pack = 8, CharSet = CharSet.Auto)]
    public struct FILE_ID_BOTH_DIR_INFO
    {
        public int NextEntryOffset;
        public uint FileIndex;
        public long CreationTime;
        public long LastAccessTime;
        public long LastWriteTime;
        public long ChangeTime;
        public ulong EndOfFile;
        public ulong AllocationSize;
        public FileAttributes FileAttributes;
        public int FileNameLength;
        public int EaSize;

        //CCHAR - может это int? - или byte. По идее должно быть int
        //потому что предыдущий член int и вместе они лучше выравниваются
        public int ShortNameLength;               
        [MarshalAs(UnmanagedType.ByValTStr,SizeConst=12)]
        public string ShortName;
        public ulong FileId;
        [MarshalAs(UnmanagedType.ByValTStr,SizeConst=WinApiFS.MAX_PATH)]
        public string FileName;
}

    [StructLayout(LayoutKind.Sequential, Pack = 4, CharSet = CharSet.Auto)]
    public struct FILE_ATTRIBUTE_TAG_INFO
    {
        public FileAttributes FileAttributes;
        public IO_REPARSE_TAG ReparseTag;
    }

    //Pack возможно не нужен - судя по структуре
    [StructLayout(LayoutKind.Sequential, Pack = 4, CharSet = CharSet.Auto)]
    public struct FILE_COMPRESSION_INFO
    {
        public ulong CompressedFileSize;
        public ushort CompressionFormat;
        public sbyte CompressionUnitShift;          //UCHAR
        public sbyte ChunkShift;                    //UCHAR
        public sbyte ClusterShift;                  //UCHAR
        [MarshalAs(UnmanagedType.ByValArray,SizeConst=3)]
        public sbyte[] Reserved;                    //UCHAR[3]
}

    /// <summary>
    /// Use only when calling GetFileInformationByHandleEx.
    /// </summary>
    [StructLayout(LayoutKind.Sequential, Pack = 8, CharSet = CharSet.Auto)]
    public struct FILE_STREAM_INFO
    {
        /// <summary>
        /// The offset for the next FILE_STREAM_INFO entry that is returned.
        /// This member is zero if no other entries follow this one.
        /// </summary>
        public int NextEntryOffset;
        public int StreamNameLength;
        public ulong StreamSize;
        public ulong StreamAllocationSize;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = WinApiFS.MAX_PATH)]
        public string StreamName;
}

    [StructLayout(LayoutKind.Sequential, Pack = 4, CharSet = CharSet.Auto)]
    public struct FILE_END_OF_FILE_INFO
    {
        /// <summary>
        /// The specified value for the new end of the file.
        /// </summary>
        public ulong EndOfFile;
    }

    [StructLayout(LayoutKind.Sequential, Pack = 4, CharSet = CharSet.Auto)]
    public struct FILE_ALLOCATION_INFO
    {
        /// <summary>
        /// The new file allocation size, in bytes.
        /// This value is typically a multiple of the sector
        /// or cluster size for the underlying physical device.
        /// </summary>
        public uint AllocationSize;
    }

    [StructLayout(LayoutKind.Sequential, Pack = 4, CharSet = CharSet.Auto)]
    public struct FILE_DISPOSITION_INFO
    {
        public bool DeleteFile;
    }

    [StructLayout(LayoutKind.Sequential, Pack = 4, CharSet = CharSet.Auto)]
    public struct FILE_RENAME_INFO
    {
        /// <summary>
        /// TRUE to replace the file; otherwise, FALSE.
        /// </summary>
        public bool ReplaceIfExists;
        /// <summary>
        /// A handle to the root directory in which the file to be renamed is located.
        /// </summary>
        public IntPtr RootDirectory;
        /// <summary>
        /// The size of FileName.
        /// </summary>
        public int FileNameLength;

        //сомнительный маршалинг
        /// <summary>
        /// new file name
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = WinApiFS.MAX_PATH)]
        public string FileName;
}

    [StructLayout(LayoutKind.Sequential, Pack = 4, CharSet = CharSet.Auto)]
    public struct FILE_NAME_INFO
    {
        public int FileNameLength;

        //Еще большой вопрос, как прокатит такой маршалинг.
        //размер SizeConst предположим MAX_PATH,
        //хотя есть подозрение, что он может и должен быть больше
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = WinApiFS.MAX_PATH)]
        public string FileName;
    }

    [StructLayout(LayoutKind.Sequential, Pack = 4)]
    public struct FILE_STANDARD_INFO
    {
        public ulong AllocationSize;
        public ulong EndOfFile;
        public int NumberOfLinks;
        public int DeletePending;
        public int Directory;
    }

    //pack не нужен
    [StructLayout(LayoutKind.Sequential)]
    public struct FILE_BASIC_INFO
    {
        public long CreationTime;
        public long LastAccessTime;
        public long LastWriteTime;
        public long ChangeTime;
        public FileAttributes FileAttributes;
    }

    [StructLayout(LayoutKind.Sequential, Pack = 4)]
    public struct BY_HANDLE_FILE_INFORMATION
    {
        public FileAttributes dwFileAttributes;
        public long ftCreationTime;
        public long ftLastAccessTime;
        public long ftLastWriteTime;
        public uint dwVolumeSerialNumber;
        public uint nFileSizeHigh;
        public uint nFileSizeLow;
        public int nNumberOfLinks;
        public uint nFileIndexHigh;
        public uint nFileIndexLow;

        public UInt64 FileSize
        {
            get
            {
                return (nFileSizeHigh * ((UInt64)WinApiFS.MAXDWORD + (UInt64)1)) + nFileSizeLow;
            }
        }

        public UInt64 FileIndex
        {
            get
            {
                return (nFileIndexHigh * ((UInt64)WinApiFS.MAXDWORD + (UInt64)1)) + nFileIndexLow;
            }
        }

        public DateTime CreationTime
        {
            get
            {
                return DateTime.FromFileTime(ftCreationTime);
            }
        }

        public DateTime LastWriteTime
        {
            get
            {
                return DateTime.FromFileTime(ftLastWriteTime);
            }
        }

        public DateTime LastAccessTime
        {
            get
            {
                return DateTime.FromFileTime(ftLastAccessTime);
            }
        }
    }

    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode, Pack = 4)]
    public struct WIN32_STREAM_ID
    {
        public FileStreamID dwStreamId;
        public FileStreamAttributes dwStreamAttributes;
        public ulong Size;
        public int dwStreamNameSize;
        //[MarshalAs(UnmanagedType.ByValTStr, SizeConst = Native.MAX_PATH + 36)] //[ANYSIZE_ARRAY] 
        //public string cStreamName;
    }

    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode, Pack = 4)]
    public struct WIN32_STREAM_ID_2
    {
        public FileStreamID dwStreamId;
        public FileStreamAttributes dwStreamAttributes;
        public ulong Size;
        public int dwStreamNameSize; 
        public string cStreamName;
    }

    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode, Pack = 4)]
    public struct WIN32_FIND_STREAM_DATA
    {
        public ulong StreamSize;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = WinApiFS.MAX_PATH + 36)]
        public string cStreamName;
    }

    /// <summary>
    /// Contains information about the file that is found
    /// by the FindFirstFile, FindFirstFileEx, or FindNextFile function
    /// </summary>
    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto, Pack = 4)]
    public struct WIN32_FIND_DATA
    {
        [MarshalAs(UnmanagedType.U4)]
        public FileAttributes dwFileAttributes;
        public long ftCreationTime;
        public long ftLastAccessTime;
        public long ftLastWriteTime;
        public uint nFileSizeHigh;
        public uint nFileSizeLow;
        /// <summary>
        /// If the dwFileAttributes member includes the FILE_ATTRIBUTE_REPARSE_POINT attribute,
        /// this member specifies the reparse point tag. Otherwise,
        /// this value is undefined and should not be used.
        /// </summary>
        public uint dwReserved0;
        public uint dwReserved1;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = WinApiFS.MAX_PATH)]
        public string cFileName;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 14)]
        public string cAlternateFileName;
        public UInt64 FileSize
        {
            get
            {
                return (nFileSizeHigh * ((UInt64)WinApiFS.MAXDWORD + (UInt64)1)) + nFileSizeLow;
            }
        }

        public IO_REPARSE_TAG ReparseTag
        {
            get
            {
                if ((dwFileAttributes & FileAttributes.ReparsePoint) == FileAttributes.ReparsePoint)
                {
                    return (IO_REPARSE_TAG)dwReserved0;
                }
                else
                {
                    return IO_REPARSE_TAG.NONE;
                }
            }
        }
    }

    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto, Pack = 4)]
    public struct WIN32_FILE_ATTRIBUTE_DATA
    {
        public FileAttributes dwFileAttributes;
        public long ftCreationTime;
        public long ftLastAccessTime;
        public long ftLastWriteTime;
        public uint nFileSizeHigh;
        public uint nFileSizeLow;

        public UInt64 FileSize
        {
            get
            {
                return (nFileSizeHigh * ((UInt64)WinApiFS.MAXDWORD + (UInt64)1)) + nFileSizeLow;
            }
        }
    }

    [StructLayout(LayoutKind.Explicit, Size = 8)]
    public struct LARGE_INTEGER
    {
        [FieldOffset(0)]
        public Int64 QuadPart;
        [FieldOffset(0)]
        public UInt32 LowPart;
        [FieldOffset(4)]
        public Int32 HighPart;
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct LUID
    {
        public UInt32 LowPart;
        public Int32 HighPart;
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct LUID_AND_ATTRIBUTES
    {
        public LUID Luid;
        public UInt32 Attributes;
    }

    public struct TOKEN_PRIVILEGES_ONE
    {
        public UInt32 PrivilegeCount;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 1)]		// !! think we only need one
        public LUID_AND_ATTRIBUTES[] Privileges;
    }


    /// <summary>
    /// Under x64, the SHFILEOPSTRUCT must be declared without the Pack = 1 parameter,
    /// or it will fail. This is a real pain if you want your code to be platform independent,
    /// as you have to declare two separate structures, one with Pack = 1, and one without.
    /// You then have to declare two different SHFileOperation calls, one for each of the structures.
    /// Then you have to decide which one to call depending on whether you are running on 32 or 64 bit.
    /// </summary>
    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode, Pack = 1)]
    public struct SHFILEOPSTRUCT
    {
        /// <summary>
        /// A window handle to the dialog box to display information about the status of the file operation.
        /// </summary>
        public IntPtr hwnd;
        public FO_Func wFunc;
        /// <summary>
        /// must be double-null terminated.
        /// Standard Microsoft MS-DOS wildcard characters, such as "*",
        /// are permitted only in the file-name position. Using a wildcard character
        /// elsewhere in the string will lead to unpredictable results.
        /// Although this member is declared as a single null-terminated string,
        /// it is actually a buffer that can hold multiple null-delimited file names.
        /// Each file name is terminated by a single NULL character.
        /// The last file name is terminated with a double NULL character ("\0\0") to indicate the end of the buffer.
        /// </summary>
        public IntPtr pFrom;
        /// <summary>
        /// must be double-null terminated.
        /// A pointer to the destination file or directory name. This parameter must be set
        /// to NULL if it is not used. Wildcard characters are not allowed.
        /// Their use will lead to unpredictable results. 
        /// Like pFrom, the pTo member is also a double-null terminated string and is handled in much the same way.
        /// For Copy and Move operations, the buffer can contain multiple destination file names 
        /// if the fFlags member specifies FOF_MULTIDESTFILES.
        /// </summary>
        public IntPtr pTo;
        public FO_Flags fFlags;
        [MarshalAs(UnmanagedType.Bool)]
        public bool fAnyOperationAborted;
        /// <summary>
        /// When the function returns, this member contains a handle to a name mapping object
        /// that contains the old and new names of the renamed files. This member is used only
        /// if the fFlags member includes the FOF_WANTMAPPINGHANDLE flag.
        /// </summary>
        public IntPtr hNameMappings;
        /// <summary>
        /// A pointer to the title of a progress dialog box. This is a null-terminated string.
        /// This member is used only if fFlags includes the FOF_SIMPLEPROGRESS flag.
        /// </summary>
        [MarshalAs(UnmanagedType.LPWStr)]
        public string lpszProgressTitle;
    }



    [Flags]
    public enum FO_Flags : ushort
    {
        None = 0,
        MultiDestinationFiles = WinApiFS.FOF_MULTIDESTFILES,
        ConfirmMouse = WinApiFS.FOF_CONFIRMMOUSE,
        Silent = WinApiFS.FOF_SILENT,
        RenameOnCollision = WinApiFS.FOF_RENAMEONCOLLISION,
        NoConfirmation = WinApiFS.FOF_NOCONFIRMATION,
        WantMappingHandle = WinApiFS.FOF_WANTMAPPINGHANDLE,
        AllowUndo = WinApiFS.FOF_ALLOWUNDO,
        FilesOnly = WinApiFS.FOF_FILESONLY,
        SimpleProgress = WinApiFS.FOF_SIMPLEPROGRESS,
        NoConfirmMakeDir = WinApiFS.FOF_NOCONFIRMMKDIR,
        NoErrorUI = WinApiFS.FOF_NOERRORUI,
        NoCopySecurityAttributes = WinApiFS.FOF_NOCOPYSECURITYATTRIBS,
        NoRecursion = WinApiFS.FOF_NORECURSION,
        NoConnectedElements = WinApiFS.FOF_NO_CONNECTED_ELEMENTS,
        WantNukeWarning = WinApiFS.FOF_WANTNUKEWARNING,
        /// <summary>
        /// deprecated; the operations engine always does the right thing
        /// on FolderLink objects (symlinks, reparse points, folder shortcuts)
        /// </summary>
        NoRecurseReparse = WinApiFS.FOF_NORECURSEREPARSE
    }

    public enum FO_Func
    {
        Move = WinApiFS.FO_MOVE,
        Copy = WinApiFS.FO_COPY,
        Delete = WinApiFS.FO_DELETE,
        Rename = WinApiFS.FO_RENAME
    }

    [Flags()]
    public enum FileStreamAttributes
    {
        NORMAL = WinApiFS.STREAM_NORMAL_ATTRIBUTE,
        /// <summary>
        /// Attribute set if the stream contains data that is modified when read.
        /// Allows the backup application to know that verification of data will fail.
        /// </summary>
        MODIFIED_WHEN_READ = WinApiFS.STREAM_MODIFIED_WHEN_READ,
        /// <summary>
        /// Stream contains security data (general attributes).
        /// Allows the stream to be ignored on cross-operations restore.
        /// </summary>
        CONTAINS_SECURITY = WinApiFS.STREAM_CONTAINS_SECURITY,
        CONTAINS_PROPERTIES = WinApiFS.STREAM_CONTAINS_PROPERTIES,
        SPARSE = WinApiFS.STREAM_SPARSE_ATTRIBUTE
    }

    public enum FileStreamID
    {
        INVALID = WinApiFS.BACKUP_INVALID,
        /// <summary>
        /// Standard data
        /// </summary>
        DATA = WinApiFS.BACKUP_DATA,
        /// <summary>
        /// Extended attribute data
        /// </summary>
        EA_DATA = WinApiFS.BACKUP_EA_DATA,
        /// <summary>
        /// Security descriptor data
        /// </summary>
        SECURITY_DATA = WinApiFS.BACKUP_SECURITY_DATA,
        /// <summary>
        /// Alternative data streams
        /// </summary>
        ALTERNATE_DATA = WinApiFS.BACKUP_ALTERNATE_DATA,
        /// <summary>
        /// Hard link information
        /// </summary>
        LINK = WinApiFS.BACKUP_LINK,
        /// <summary>
        /// Property data
        /// </summary>
        PROPERTY_DATA = WinApiFS.BACKUP_PROPERTY_DATA,
        /// <summary>
        /// Objects identifiers
        /// </summary>
        OBJECT_ID = WinApiFS.BACKUP_OBJECT_ID,
        /// <summary>
        /// Reparse points
        /// </summary>
        REPARSE_DATA = WinApiFS.BACKUP_REPARSE_DATA,
        /// <summary>
        /// Sparse file.
        /// </summary>
        SPARSE_BLOCK = WinApiFS.BACKUP_SPARSE_BLOCK,
        TXFS_DATA = WinApiFS.BACKUP_TXFS_DATA
    }

    [Flags()]
    public enum CreateFileOptions : uint
    {
        None=0,
        ATTRIBUTE_READONLY = WinApiFS.FILE_ATTRIBUTE_READONLY,
        ATTRIBUTE_HIDDEN = WinApiFS.FILE_ATTRIBUTE_HIDDEN,
        ATTRIBUTE_SYSTEM = WinApiFS.FILE_ATTRIBUTE_SYSTEM,
        ATTRIBUTE_DIRECTORY = WinApiFS.FILE_ATTRIBUTE_DIRECTORY,
        ATTRIBUTE_ARCHIVE = WinApiFS.FILE_ATTRIBUTE_ARCHIVE,
        ATTRIBUTE_DEVICE = WinApiFS.FILE_ATTRIBUTE_DEVICE,
        ATTRIBUTE_NORMAL = WinApiFS.FILE_ATTRIBUTE_NORMAL,
        ATTRIBUTE_TEMPORARY = WinApiFS.FILE_ATTRIBUTE_TEMPORARY,
        ATTRIBUTE_SPARSE_FILE = WinApiFS.FILE_ATTRIBUTE_SPARSE_FILE,
        ATTRIBUTE_REPARSE_POINT = WinApiFS.FILE_ATTRIBUTE_REPARSE_POINT,
        ATTRIBUTE_COMPRESSED = WinApiFS.FILE_ATTRIBUTE_COMPRESSED,
        ATTRIBUTE_OFFLINE = WinApiFS.FILE_ATTRIBUTE_OFFLINE,
        ATTRIBUTE_NOT_CONTENT_INDEXED = WinApiFS.FILE_ATTRIBUTE_OFFLINE,
        ATTRIBUTE_ENCRYPTED = WinApiFS.FILE_ATTRIBUTE_ENCRYPTED,
        ATTRIBUTE_VIRTUAL = WinApiFS.FILE_ATTRIBUTE_VIRTUAL,
        WRITE_THROUGH = WinApiFS.FILE_FLAG_WRITE_THROUGH,
        OVERLAPPED = WinApiFS.FILE_FLAG_OVERLAPPED,
        NO_BUFFERING = WinApiFS.FILE_FLAG_NO_BUFFERING,
        RANDOM_ACCESS = WinApiFS.FILE_FLAG_RANDOM_ACCESS,
        SEQUENTIAL_SCAN = WinApiFS.FILE_FLAG_SEQUENTIAL_SCAN,
        DELETE_ON_CLOSE = WinApiFS.FILE_FLAG_DELETE_ON_CLOSE,
        BACKUP_SEMANTICS = WinApiFS.FILE_FLAG_BACKUP_SEMANTICS,
        POSIX_SEMANTICS = WinApiFS.FILE_FLAG_POSIX_SEMANTICS,
        OPEN_REPARSE_POINT = WinApiFS.FILE_FLAG_OPEN_REPARSE_POINT,
        OPEN_NO_RECALL = WinApiFS.FILE_FLAG_OPEN_NO_RECALL,
        FIRST_PIPE_INSTANCE = WinApiFS.FILE_FLAG_FIRST_PIPE_INSTANCE
    }

    [Flags()]
    public enum Win32FileAccess : uint
    {
        READ_DATA = WinApiFS.FILE_READ_DATA,   // file & pipe
        LIST_DIRECTORY = WinApiFS.FILE_LIST_DIRECTORY,    // directory
        WRITE_DATA = WinApiFS.FILE_WRITE_DATA,    // file & pipe
        ADD_FILE = WinApiFS.FILE_ADD_FILE,    // directory
        APPEND_DATA = WinApiFS.FILE_APPEND_DATA,    // file
        ADD_SUBDIRECTORY = WinApiFS.FILE_ADD_SUBDIRECTORY,    // directory
        CREATE_PIPE_INSTANCE = WinApiFS.FILE_CREATE_PIPE_INSTANCE,    // named pipe
        READ_EA = WinApiFS.FILE_READ_EA,    // file & directory
        WRITE_EA = WinApiFS.FILE_WRITE_EA,    // file & directory
        EXECUTE = WinApiFS.FILE_EXECUTE,    // file
        TRAVERSE = WinApiFS.FILE_TRAVERSE,    // directory
        DELETE_CHILD = WinApiFS.FILE_DELETE_CHILD,    // directory
        READ_ATTRIBUTES = WinApiFS.FILE_READ_ATTRIBUTES,    // all
        WRITE_ATTRIBUTES = WinApiFS.FILE_WRITE_ATTRIBUTES,    // all
        ALL_ACCESS = WinApiFS.FILE_ALL_ACCESS,
        GENERIC_READ = WinApiFS.FILE_GENERIC_READ,
        GENERIC_WRITE = WinApiFS.FILE_GENERIC_WRITE,
        GENERIC_EXECUTE = WinApiFS.FILE_GENERIC_EXECUTE,
        None = 0
    }

    public enum IO_REPARSE_TAG : uint
    {
        DFS = WinApiFS.IO_REPARSE_TAG_DFS,
        DFSR = WinApiFS.IO_REPARSE_TAG_DFSR,
        HSM = WinApiFS.IO_REPARSE_TAG_HSM,
        HSM2 = WinApiFS.IO_REPARSE_TAG_HSM2,
        MOUNT_POINT = WinApiFS.IO_REPARSE_TAG_MOUNT_POINT,
        SIS = WinApiFS.IO_REPARSE_TAG_SIS,
        SYMLINK = WinApiFS.IO_REPARSE_TAG_SYMLINK,
        NONE = 0
    }

    [Flags()]
    public enum VolumeCaps : uint
    {
        /// <summary>
        /// The file system preserves the case of file names when it places a name on disk.
        /// </summary>
        CasePreservedNames = WinApiFS.FILE_CASE_PRESERVED_NAMES,
        /// <summary>
        /// The file system supports case-sensitive file names.
        /// </summary>
        CaseSensitiveNames = WinApiFS.FILE_CASE_SENSITIVE_SEARCH,
        /// <summary>
        /// The file system supports file-based compression.
        /// </summary>
        Compression = WinApiFS.FILE_FILE_COMPRESSION,
        /// <summary>
        /// The file system supports named streams.
        /// </summary>
        NamedStreams = WinApiFS.FILE_NAMED_STREAMS,
        /// <summary>
        /// he file system preserves and enforces access control lists (ACL).
        /// For example, the NTFS file system preserves and enforces ACLs,
        /// and the FAT file system does not.
        /// </summary>
        PersistentAcls = WinApiFS.FILE_PERSISTENT_ACLS,
        /// <summary>
        /// The specified volume is read-only.
        /// Windows 2000:  This value is not supported.
        /// </summary>
        ReadOnly = WinApiFS.FILE_READ_ONLY_VOLUME,
        /// <summary>
        /// The volume supports a single sequential write.
        /// </summary>
        SequentialWriteOnce = WinApiFS.FILE_SEQUENTIAL_WRITE_ONCE,
        /// <summary>
        /// The file system supports the Encrypted File System (EFS).
        /// </summary>
        Encription = WinApiFS.FILE_SUPPORTS_ENCRYPTION,
        /// <summary>
        /// The file system supports object identifiers.
        /// </summary>
        ObjectIDs = WinApiFS.FILE_SUPPORTS_OBJECT_IDS,
        /// <summary>
        /// The file system supports re-parse points.
        /// </summary>
        ReparsePoints = WinApiFS.FILE_SUPPORTS_REPARSE_POINTS,
        /// <summary>
        /// The file system supports sparse files.
        /// </summary>
        SparseFiles = WinApiFS.FILE_SUPPORTS_SPARSE_FILES,
        /// <summary>
        /// The volume supports transactions.
        /// </summary>
        Transactions = WinApiFS.FILE_SUPPORTS_TRANSACTIONS,
        /// <summary>
        /// The file system supports Unicode in file names as they appear on disk.
        /// </summary>
        UnicodeNames = WinApiFS.FILE_UNICODE_ON_DISK,
        /// <summary>
        /// The specified volume is a compressed volume, for example, a DoubleSpace volume.
        /// </summary>
        VolumeCompressed = WinApiFS.FILE_VOLUME_IS_COMPRESSED,
        /// <summary>
        /// he file system supports disk quotas.
        /// </summary>
        Quotas = WinApiFS.FILE_VOLUME_QUOTAS
    }

    public enum CopyFileExState
    {
        /// <summary>
        /// Another part of the data file was copied.
        /// </summary>
        CHUNK_FINISHED = WinApiFS.CALLBACK_CHUNK_FINISHED,
        /// <summary>
        /// Another stream was created and is about to be copied.
        /// This is the callback reason given when the callback routine is first invoked.
        /// </summary>
        STREAM_SWITCH = WinApiFS.CALLBACK_STREAM_SWITCH
    }

    public enum CopyFileExCallbackReturns
    {
        /// <summary>
        /// Continue the copy operation
        /// </summary>
        CONTINUE = WinApiFS.PROGRESS_CONTINUE,
        /// <summary>
        /// Cancel the copy operation and delete the destination file
        /// </summary>
        CANCEL = WinApiFS.PROGRESS_CANCEL,
        /// <summary>
        /// Stop the copy operation. It can be restarted at a later time.
        /// </summary>
        STOP = WinApiFS.PROGRESS_STOP,
        /// <summary>
        /// Continue the copy operation, but stop invoking CopyProgressRoutine to report progress.
        /// </summary>
        QUIET = WinApiFS.PROGRESS_QUIET
    }

    [Flags()]
    public enum CopyFileExOptions
    {
        /// <summary>
        /// The copy operation fails immediately if the target file already exists.
        /// </summary>
        FAIL_IF_EXISTS = WinApiFS.COPY_FILE_FAIL_IF_EXISTS,
        /// <summary>
        /// Progress of the copy is tracked in the target file in case the copy fails.
        /// The failed copy can be restarted at a later time by specifying the same values
        /// for lpExistingFileName and lpNewFileName as those used in the call that failed.
        /// </summary>
        RESTARTABLE = WinApiFS.COPY_FILE_RESTARTABLE,
        /// <summary>
        /// The file is copied and the original file is opened for write access.
        /// </summary>
        OPEN_SOURCE_FOR_WRITE = WinApiFS.COPY_FILE_OPEN_SOURCE_FOR_WRITE,
        /// <summary>
        /// An attempt to copy an encrypted file will succeed even
        /// if the destination copy cannot be encrypted.
        /// (from Win XP)
        /// </summary>
        ALLOW_DECRYPTED_DESTINATION = WinApiFS.COPY_FILE_ALLOW_DECRYPTED_DESTINATION,
        /// <summary>
        /// If the source file is a symbolic link, the destination file is also
        /// a symbolic link pointing to the same file that the source symbolic link is pointing to.
        /// (From Win vista)
        /// </summary>
        COPY_SYMLINK = WinApiFS.COPY_FILE_COPY_SYMLINK,
        None = 0
    }

    public enum GET_FILEEX_INFO_LEVELS
    {
        GetFileExInfoStandard,
        GetFileExMaxInfoLevel
    }

    public enum STREAM_INFO_LEVELS
    {
        FindStreamInfoStandard
    }

    public enum FileBinaryType
    {
        Binary_32 = WinApiFS.SCS_32BIT_BINARY,
        Binary_DOS = WinApiFS.SCS_DOS_BINARY,
        Binary_WOW = WinApiFS.SCS_WOW_BINARY,
        Pif = WinApiFS.SCS_PIF_BINARY,
        Binary_Posix = WinApiFS.SCS_POSIX_BINARY,
        Binary_OS2_16 = WinApiFS.SCS_OS216_BINARY,
        Binary_64 = WinApiFS.SCS_64BIT_BINARY,
        None = -1
    }

    [Flags()]
    public enum MoveFileOptions
    {
        CopyAllowed = WinApiFS.MOVEFILE_COPY_ALLOWED,
        CreateHardLink = WinApiFS.MOVEFILE_CREATE_HARDLINK,
        DelayUntilReboot = WinApiFS.MOVEFILE_DELAY_UNTIL_REBOOT,
        FailIfNotTrackable = WinApiFS.MOVEFILE_FAIL_IF_NOT_TRACKABLE,
        ReplaceExisting = WinApiFS.MOVEFILE_REPLACE_EXISTING,
        WriteThrough = WinApiFS.MOVEFILE_WRITE_THROUGH
    }

    public enum FILE_INFO_BY_HANDLE_CLASS
    {
        FileBasicInfo,
        FileStandardInfo,
        FileNameInfo,
        FileRenameInfo,
        FileDispositionInfo,
        FileAllocationInfo,
        FileEndOfFileInfo,
        FileStreamInfo,
        FileCompressionInfo,
        FileAttributeTagInfo,
        FileIdBothDirectoryInfo,
        FileIdBothDirectoryRestartInfo,
        FileIoPriorityHintInfo,
        MaximumFileInfoByHandleClass
    }

    public enum PRIORITY_HINT
    {
        IoPriorityHintVeryLow = 0,
        IoPriorityHintLow,
        IoPriorityHintNormal,
        MaximumIoPriorityHintType
    }

    public enum NTFSlinkType
    {
        Hard,
        Junction,
        Symbolic
    }

    /// <summary>
    /// An application-defined callback function used with the CopyFileEx,
    /// MoveFileTransacted, and MoveFileWithProgress functions.
    /// It is called when a portion of a copy or move operation is completed.
    /// The LPPROGRESS_ROUTINE type defines a pointer to this callback function.
    /// CopyProgressRoutine is a placeholder for the application-defined function name.
    /// </summary>
    /// <param name="TotalFileSize">The total size of the file, in bytes.</param>
    /// <param name="TotalBytesTransferred">The total number of bytes transferred from the source
    /// file to the destination file since the copy operation began.</param>
    /// <param name="StreamSize">The total size of the current file stream, in bytes.</param>
    /// <param name="StreamBytesTransferred">The total number of bytes in the current
    /// stream that have been transferred from the source file to the destination file
    /// since the copy operation began.</param>
    /// <param name="dwStreamNumber">A handle to the current stream. 
    /// The first time CopyProgressRoutine is called, the stream number is 1.</param>
    /// <param name="dwCallbackReason"></param>
    /// <param name="hSourceFile">A handle to the source file.</param>
    /// <param name="hDestinationFile">A handle to the destination file</param>
    /// <param name="lpData">Argument passed to CopyProgressRoutine by CopyFileEx,
    /// MoveFileTransacted, or MoveFileWithProgress.</param>
    /// <returns></returns>
    public delegate CopyFileExCallbackReturns CopyProgressRoutine
    (UInt64 TotalFileSize,
    UInt64 TotalBytesTransferred,
    UInt64 StreamSize,
    UInt64 StreamBytesTransferred,
    int dwStreamNumber,
    CopyFileExState dwCallbackReason,
    IntPtr hSourceFile,
    IntPtr hDestinationFile,
    IntPtr lpData);
}
