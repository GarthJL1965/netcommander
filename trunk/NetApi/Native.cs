using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;

namespace netCommander.NetApi
{
    public class WinApiNET
    {
        private WinApiNET()
        {
        }

        [DllImport("netapi32.dll", SetLastError = false)]
        public static extern int NetShareGetInfo
            ([MarshalAs(UnmanagedType.LPWStr)] string servername,
            [MarshalAs(UnmanagedType.LPWStr)] string netname,
            NetShareInfoLevel level,
            ref IntPtr bufptr);

        /// <summary>
        /// Returns information about some or all open files on a server, depending on the parameters specified.
        /// Only members of the Administrators or Server Operators local group can successfully execute the
        /// NetFileEnum function
        /// </summary>
        /// <param name="servername">string that specifies the DNS or NetBIOS name of the remote
        /// server on which the function is to execute. If this parameter is NULL, the local computer is used</param>
        /// <param name="basepath"> string that specifies a qualifier for the returned information.
        /// If this parameter is NULL, all open resources are enumerated. If this parameter is not NULL,
        /// the function enumerates only resources that have the value of the basepath parameter
        /// as a prefix. (A prefix is the path component up to a backslash.)</param>
        /// <param name="username">string that specifies the name of the user or the name of the connection.
        /// If the string begins with two backslashes ("\\"), then it indicates the name of the connection,
        /// for example, "\\127.0.0.1" or "\\ClientName". The part of the connection name after the backslashes
        /// is the same as the client name in the session information structure returned by the NetSessionEnum
        /// function. If the string does not begin with two backslashes, then it indicates the name of the user.
        /// If this parameter is not NULL, its value serves as a qualifier for the enumeration. The files returned
        /// are limited to those that have user names or connection names that match the qualifier. If this
        /// parameter is NULL, no user-name qualifier is used.</param>
        /// <param name="level"></param>
        /// <param name="bufptr"></param>
        /// <param name="prefmaxlen"></param>
        /// <param name="entriesread"></param>
        /// <param name="totalentries"></param>
        /// <param name="resume_handle"></param>
        /// <returns>If the function succeeds, the return value is NERR_Success.</returns>
        [DllImport("netapi32.dll", SetLastError = false)]
        public static extern int NetFileEnum
            ([MarshalAs(UnmanagedType.LPWStr)] string servername,
            [MarshalAs(UnmanagedType.LPWStr)] string basepath,
            [MarshalAs(UnmanagedType.LPWStr)] string username,
            NetFileEnumLevel level,
            ref IntPtr bufptr,
            uint prefmaxlen,
            ref int entriesread,
            ref int totalentries,
            ref uint resume_handle);

        /// <summary>
        /// Provides information about sessions established on a server
        /// </summary>
        /// <param name="servername"> string that specifies the DNS or NetBIOS name of the remote server on which
        /// the function is to execute. If this parameter is NULL, the local computer is used</param>
        /// <param name="UncClientName">string that specifies the name of the computer session for which
        /// information is to be returned. If this parameter is NULL, NetSessionEnum returns information
        /// for all computer sessions on the server</param>
        /// <param name="username">string that specifies the name of the user for which information
        /// is to be returned. If this parameter is NULL, NetSessionEnum returns information for all users</param>
        /// <param name="level"></param>
        /// <param name="bufptr"></param>
        /// <param name="prefmaxlen"></param>
        /// <param name="entriesread"></param>
        /// <param name="totalentries"></param>
        /// <param name="resume_handle"></param>
        /// <returns>If the function succeeds, the return value is NERR_Success</returns>
        [DllImport("netapi32.dll", SetLastError = false)]
        public static extern int NetSessionEnum
            ([MarshalAs(UnmanagedType.LPWStr)] string servername,
            [MarshalAs(UnmanagedType.LPWStr)] string UncClientName,
            [MarshalAs(UnmanagedType.LPWStr)] string username,
            NetSessionEnumLevel level,
            ref IntPtr bufptr,
            uint prefmaxlen,
            ref int entriesread,
            ref int totalentries,
            ref uint resume_handle);

        [DllImport("netapi32.dll", SetLastError = false)]
        public static extern int NetServerTransportEnum
            ([MarshalAs(UnmanagedType.LPWStr)] string servername,
            NetServerTransportEnumLevel level,
            ref IntPtr buffer,
            uint prefmaxlen,
            ref int entriesread,
            ref int totalentries,
            ref uint resumehandle);

        [DllImport("netapi32.dll", SetLastError = false)]
        public static extern int NetRemoteTOD
            ([MarshalAs(UnmanagedType.LPWStr)] string UncServerName,
            ref IntPtr BufferPtr);

        /// <summary>
        /// The NetRemoteComputerSupports function queries the redirector to retrieve
        /// the optional features the remote system supports. Features include Unicode,
        /// Remote Procedure Call (RPC), and Remote Administration Protocol support.
        /// The function establishes a network connection if one does not exist.
        /// </summary>
        /// <param name="UncServerName">Must start with "\\".</param>
        /// <param name="OptionsWanted"></param>
        /// <param name="OptionsSupported"></param>
        /// <returns>If the function succeeds, the return value is NERR_Success</returns>
        [DllImport("netapi32.dll", SetLastError = false)]
        public static extern int NetRemoteComputerSupports
            ([MarshalAs(UnmanagedType.LPWStr)] string UncServerName,
            NetRemoteComputerSupportsFeatures OptionsWanted,
            ref NetRemoteComputerSupportsFeatures OptionsSupported);

        /// <summary>
        ///  retrieves current configuration information for the specified server
        /// </summary>
        /// <param name="servername">Pointer to a string that specifies the name of the remote
        /// server on which the function is to execute. If this parameter is NULL, the local computer is used.</param>
        /// <param name="level"></param>
        /// <param name="bufptr">This buffer is allocated by the system and must be freed using the NetApiBufferFree function.</param>
        /// <returns>If the function succeeds, the return value is NERR_Success.</returns>
        [DllImport("netapi32.dll", SetLastError = false)]
        public static extern int NetServerGetInfo
             ([MarshalAs(UnmanagedType.LPWStr)] string servername,
             NetserverInfoLevel level,
             ref IntPtr bufptr);

        /// <summary>
        /// The NetQueryDisplayInformation function returns
        /// user account, computer, or group account information. 
        /// Call this function to quickly enumerate account information for display in user interfaces.
        /// </summary>
        /// <param name="ServerName">constant string that specifies the DNS or NetBIOS
        /// name of the remote server on which the function is to execute.
        /// If this parameter is NULL, the local computer is used.</param>
        /// <param name="Level"></param>
        /// <param name="Index">Specifies the index of the first entry for which to retrieve information.
        /// Specify zero to retrieve account information beginning with
        /// the first display information entry</param>
        /// <param name="EntriesRequested">Specifies the maximum number of entries
        /// for which to retrieve information. On Windows2000 and later,
        /// each call to NetQueryDisplayInformation returns a maximum of 100 objects.</param>
        /// <param name="PreferredMaximumLength">Specifies the preferred maximum size,
        /// in bytes, of the system-allocated buffer returned in the SortedBuffer parameter.
        /// It is recommended that you set this parameter to MAX_PREFERRED_LENGTH.</param>
        /// <param name="ReturnedEntryCount">value that receives the number of entries 
        /// in the buffer returned in the SortedBuffer parameter.
        /// If this parameter is zero, there are no entries with an index as large
        /// as that specified. Entries may be returned when the function's return value
        /// is either NERR_Success or ERROR_MORE_DATA.</param>
        /// <param name="SortedBuffer">pointer to a system-allocated buffer that specifies a
        /// sorted list of the requested information. The format of this data depends
        /// on the value of the Level parameter. Because this buffer is allocated by the system,
        /// it must be freed using the NetApiBufferFree function. Note that you must free the buffer
        /// even if the function fails with ERROR_MORE_DATA. </param>
        /// <returns>If the function succeeds, the return value is NERR_Success.
        /// ERROR_MORE_DATA - more entries available, other - error code</returns>
        [DllImport("netapi32.dll", SetLastError = false, EntryPoint = "NetQueryDisplayInformation")]
        public static extern int NetQueryDisplayInformation
            ([MarshalAs(UnmanagedType.LPWStr)]
            string ServerName,
            NetqueryDisplayInfoLevel Level,
            int Index,
            int EntriesRequested,
            uint PreferredMaximumLength,
            ref int ReturnedEntryCount,
            ref IntPtr SortedBuffer);

        /// <summary>
        /// The NetQueryDisplayInformation function returns
        /// user account, computer, or group account information. 
        /// Call this function to quickly enumerate account information for display in user interfaces.
        /// </summary>
        /// <param name="ServerName">constant string that specifies the DNS or NetBIOS
        /// name of the remote server on which the function is to execute.
        /// If this parameter is NULL, the local computer is used.</param>
        /// <param name="Level"></param>
        /// <param name="Index">Specifies the index of the first entry for which to retrieve information.
        /// Specify zero to retrieve account information beginning with
        /// the first display information entry</param>
        /// <param name="EntriesRequested">Specifies the maximum number of entries
        /// for which to retrieve information. On Windows2000 and later,
        /// each call to NetQueryDisplayInformation returns a maximum of 100 objects.</param>
        /// <param name="PreferredMaximumLength">Specifies the preferred maximum size,
        /// in bytes, of the system-allocated buffer returned in the SortedBuffer parameter.
        /// It is recommended that you set this parameter to MAX_PREFERRED_LENGTH.</param>
        /// <param name="ReturnedEntryCount">value that receives the number of entries 
        /// in the buffer returned in the SortedBuffer parameter.
        /// If this parameter is zero, there are no entries with an index as large
        /// as that specified. Entries may be returned when the function's return value
        /// is either NERR_Success or ERROR_MORE_DATA.</param>
        /// <param name="SortedBuffer">pointer to a system-allocated buffer that specifies a
        /// sorted list of the requested information. The format of this data depends
        /// on the value of the Level parameter. Because this buffer is allocated by the system,
        /// it must be freed using the NetApiBufferFree function. Note that you must free the buffer
        /// even if the function fails with ERROR_MORE_DATA. </param>
        /// <returns>If the function succeeds, the return value is NERR_Success.
        /// ERROR_MORE_DATA - more entries available, other - error code</returns>
        [DllImport("netapi32.dll", SetLastError = false, EntryPoint = "NetQueryDisplayInformation")]
        public static extern int NetQueryDisplayInformation
            (IntPtr lpwServerName,
            NetqueryDisplayInfoLevel Level,
            int Index,
            int EntriesRequested,
            uint PreferredMaximumLength,
            ref int ReturnedEntryCount,
            ref IntPtr SortedBuffer);


        /// <summary>
        /// The NetUseAdd function establishes a connection between the local computer and a remote server.
        /// You can specify a local drive letter or a printer device to connect.
        /// If you do not specify a local drive letter or printer device, the function
        /// authenticates the client with the server for future connections.
        /// </summary>
        /// <param name="UncServerName">The UNC name of the computer on which to execute this function.
        /// If this parameter is NULL, then the local computer is used. If the UncServerName parameter
        /// specified is a remote computer, then the remote computer must support remote RPC calls
        /// using the legacy Remote Access Protocol mechanism.</param>
        /// <param name="Level">use 2</param>
        /// <param name="ptBuffer">A pointer to the buffer that specifies the data.
        /// The format of this data depends on the value of the Level parameter. </param>
        /// <param name="ParmError">A pointer to a value that receives the index of the first member
        /// of the information structure in error when the ERROR_INVALID_PARAMETER error is returned.
        /// If this parameter is NULL, the index is not returned on error.</param>
        /// <returns>If the function succeeds, the return value is NERR_Success.
        /// If the function fails, the return value is a system error code.</returns>
        [DllImport("netapi32.dll", SetLastError = false, EntryPoint = "NetUseAdd")]
        public static extern int NetUseAdd
            ([MarshalAs(UnmanagedType.LPTStr)]
            string UncServerName,
            int Level,
            IntPtr ptBuffer,
            ref int ParmError);

        /// <summary>
        /// The NetServerEnum function lists all servers of the specified type that are visible
        /// in a domain. For example, an application can call NetServerEnum to list all
        /// domain controllers only or all SQL servers only. You can combine bit masks to list
        /// several types. For example, a value of 0x00000003 combines the bit masks
        /// for SV_TYPE_WORKSTATION (0x00000001) and SV_TYPE_SERVER (0x00000002).
        /// If you require more information for a specific server, call the WNetEnumResource function.
        /// </summary>
        /// <param name="servername">Reserved; must be NULL</param>
        /// <param name="level"></param>
        /// <param name="lpBuffer">A pointer to the buffer that receives the data.
        /// The format of this data depends on the value of the level parameter.
        /// This buffer is allocated by the system and must be freed using
        /// the NetApiBufferFree function. Note that you must free the buffer even
        /// if the function fails with ERROR_MORE_DATA.</param>
        /// <param name="prefmaxlen">The preferred maximum length of returned data, in bytes.
        /// If you specify MAX_PREFERRED_LENGTH, the function allocates the amount of memory
        /// required for the data. If you specify another value in this parameter, it can restrict
        /// the number of bytes that the function returns. If the buffer size is insufficient
        /// to hold all entries, the function returns ERROR_MORE_DATA. For more information,
        /// see Network Management Function Buffers and Network Management Function Buffer Lengths.</param>
        /// <param name="entriesread">A pointer to a value that receives the count of
        /// elements actually enumerated.</param>
        /// <param name="totalentries">A pointer to a value that receives the total number
        /// of visible servers and workstations on the network. Note that applications should 
        /// consider this value only as a hint.</param>
        /// <param name="servertype">A value that filters the server entries to return from the enumeration.</param>
        /// <param name="domain">A pointer to a constant string that specifies the name of the domain for which a list of servers is to be returned. The domain name must be a NetBIOS domain name (for example, microsoft). The NetServerEnum function does not support DNS-style names (for example, microsoft.com). If this parameter is NULL, the primary domain is implied.</param>
        /// <param name="resume_handle">Reserved; must be set to zero.</param>
        /// <returns>f the function succeeds, the return value is NERR_Success. 
        /// If the function fails, the return value can be one of the following error codes</returns>
        [DllImport("netapi32.dll", SetLastError = false, EntryPoint = "NetServerEnum")]
        public static extern int NetServerEnum
            (IntPtr servername,
            NetserverEnumLevel level,
            ref IntPtr lpBuffer,
            uint prefmaxlen,
            ref int entriesread,
            ref int totalentries,
            NetserverEnumType servertype,
            IntPtr domain,
            ref uint resume_handle);

        /// <summary>
        /// The NetServerEnum function lists all servers of the specified type that are visible
        /// in a domain. For example, an application can call NetServerEnum to list all
        /// domain controllers only or all SQL servers only. You can combine bit masks to list
        /// several types. For example, a value of 0x00000003 combines the bit masks
        /// for SV_TYPE_WORKSTATION (0x00000001) and SV_TYPE_SERVER (0x00000002).
        /// If you require more information for a specific server, call the WNetEnumResource function.
        /// </summary>
        /// <param name="servername">Reserved; must be NULL</param>
        /// <param name="level"></param>
        /// <param name="lpBuffer">A pointer to the buffer that receives the data.
        /// The format of this data depends on the value of the level parameter.
        /// This buffer is allocated by the system and must be freed using
        /// the NetApiBufferFree function. Note that you must free the buffer even
        /// if the function fails with ERROR_MORE_DATA.</param>
        /// <param name="prefmaxlen">The preferred maximum length of returned data, in bytes.
        /// If you specify MAX_PREFERRED_LENGTH, the function allocates the amount of memory
        /// required for the data. If you specify another value in this parameter, it can restrict
        /// the number of bytes that the function returns. If the buffer size is insufficient
        /// to hold all entries, the function returns ERROR_MORE_DATA. For more information,
        /// see Network Management Function Buffers and Network Management Function Buffer Lengths.</param>
        /// <param name="entriesread">A pointer to a value that receives the count of
        /// elements actually enumerated.</param>
        /// <param name="totalentries">A pointer to a value that receives the total number
        /// of visible servers and workstations on the network. Note that applications should 
        /// consider this value only as a hint.</param>
        /// <param name="servertype">A value that filters the server entries to return from the enumeration.</param>
        /// <param name="domain">A pointer to a constant string that specifies the name of the domain for which a list of servers is to be returned. The domain name must be a NetBIOS domain name (for example, microsoft). The NetServerEnum function does not support DNS-style names (for example, microsoft.com). If this parameter is NULL, the primary domain is implied.</param>
        /// <param name="resume_handle">Reserved; must be set to zero.</param>
        /// <returns>f the function succeeds, the return value is NERR_Success. 
        /// If the function fails, the return value can be one of the following error codes</returns>
        [DllImport("netapi32.dll", SetLastError = false, EntryPoint = "NetServerEnum")]
        public static extern int NetServerEnum
            (IntPtr servername,
            NetserverEnumLevel level,
            ref IntPtr lpBuffer,
            uint prefmaxlen,
            ref int entriesread,
            ref int totalentries,
            NetserverEnumType servertype,
            [MarshalAs(UnmanagedType.LPWStr)]
            string domain,
            ref uint resume_handle);

        /// <summary>
        /// The NetShareEnum function retrieves information about each shared resource
        /// on a server. You can also use the WNetEnumResource function to retrieve
        /// resource information. However, WNetEnumResource does not enumerate hidden
        /// shares or users connected to a share.
        /// </summary>
        /// <param name="servername">Pointer to a string that specifies the DNS or NetBIOS name
        /// of the remote server on which the function is to execute.
        /// If this parameter is NULL, the local computer is used.</param>
        /// <param name="level">Specifies the information level of the data</param>
        /// <param name="ptBuffer">Pointer to the buffer that receives the data.
        /// The format of this data depends on the value of the level parameter.
        /// This buffer is allocated by the system and must be freed using the NetApiBufferFree
        /// function. Note that you must free the buffer even if the function fails with ERROR_MORE_DATA</param>
        /// <param name="prefmaxlen">Specifies the preferred maximum length of returned data,
        /// in bytes. If you specify MAX_PREFERRED_LENGTH, the function allocates the amount
        /// of memory required for the data. If you specify another value in this parameter,
        /// it can restrict the number of bytes that the function returns. If the buffer size
        /// is insufficient to hold all entries, the function returns ERROR_MORE_DATA.
        /// For more information, see Network Management Function Buffers and Network Management
        /// Function Buffer Lengths.</param>
        /// <param name="entriesread">Pointer to a value that receives the count
        /// of elements actually enumerated.</param>
        /// <param name="totalentries">Pointer to a value that receives the total number of entries
        /// that could have been enumerated. Note that applications should consider this value
        /// only as a hint.</param>
        /// <param name="resume_handle">Pointer to a value that contains a resume handle
        /// which is used to continue an existing share search. The handle should be zero
        /// on the first call and left unchanged for subsequent calls. If resume_handle is NULL,
        /// then no resume handle is stored.</param>
        /// <returns>If the function succeeds, the return value is NERR_Success.
        /// If the function fails, the return value is a system error code. </returns>
        [DllImport("netapi32.dll", SetLastError = false)]
        public static extern int NetShareEnum
            ([MarshalAs(UnmanagedType.LPWStr)]
            string servername,
            NET_INFO_LEVEL level,
            ref IntPtr ptBuffer,
            uint prefmaxlen,
            ref int entriesread,
            ref int totalentries,
            ref uint resume_handle);

        [DllImport("netapi32.dll", SetLastError = false)]
        public static extern int NetShareEnum
            (IntPtr lpwServername,
            NET_INFO_LEVEL level,
            ref IntPtr ptBuffer,
            uint prefmaxlen,
            ref int entriesread,
            ref int totalentries,
            ref uint resume_handle);

        [DllImport("netapi32.dll", SetLastError = false)]
        public static extern int NetApiBufferFree
            (IntPtr Buffer);

        public const int ERROR_MORE_DATA = 234;
        public const int NERR_Success = 0;
        public const uint MAX_PREFERRED_LENGTH = 0xFFFFFFFF;

        public const uint STYPE_DISKTREE = 0;
        public const uint STYPE_PRINTQ = 1;
        public const uint STYPE_DEVICE = 2;
        public const uint STYPE_IPC = 3;
        public const uint STYPE_SPECIAL = 0x80000000;
        public const uint STYPE_TEMPORARY = 0x40000000;

        public const int ACCESS_NONE = 0;
        public const int ACCESS_READ = 1;
        public const int ACCESS_WRITE = 2;
        public const int ACCESS_CREATE = 4;
        public const int ACCESS_EXEC = 8;
        public const int ACCESS_DELETE = 0x10;
        public const int ACCESS_ATRIB = 0x20;
        public const int ACCESS_PERM = 0x40;
        public const int ACCESS_ALL = ACCESS_READ | ACCESS_WRITE | ACCESS_CREATE | ACCESS_EXEC | ACCESS_DELETE | ACCESS_ATRIB | ACCESS_PERM;
        public const int ACCESS_GROUP = 0x8000;

        public const int PLATFORM_ID_DOS = 300;
        public const int PLATFORM_ID_OS2 = 400;
        public const int PLATFORM_ID_NT = 500;
        public const int PLATFORM_ID_OSF = 600;
        public const int PLATFORM_ID_VMS = 700;

        public const uint MAJOR_VERSION_MASK = 0x0F;

        public const uint SV_TYPE_WORKSTATION = 0x00000001;
        public const uint SV_TYPE_SERVER = 0x00000002;
        public const uint SV_TYPE_SQLSERVER = 0x00000004;
        public const uint SV_TYPE_DOMAIN_CTRL = 0x00000008;
        public const uint SV_TYPE_DOMAIN_BAKCTRL = 0x00000010;
        public const uint SV_TYPE_TIME_SOURCE = 0x00000020;
        public const uint SV_TYPE_AFP = 0x00000040;
        public const uint SV_TYPE_NOVELL = 0x00000080;
        public const uint SV_TYPE_DOMAIN_MEMBER = 0x00000100;
        public const uint SV_TYPE_PRINTQ_SERVER = 0x00000200;
        public const uint SV_TYPE_DIALIN_SERVER = 0x00000400;
        public const uint SV_TYPE_XENIX_SERVER = 0x00000800;
        public const uint SV_TYPE_SERVER_UNIX = SV_TYPE_XENIX_SERVER;
        public const uint SV_TYPE_NT = 0x00001000;
        public const uint SV_TYPE_WFW = 0x00002000;
        public const uint SV_TYPE_SERVER_MFPN = 0x00004000;
        public const uint SV_TYPE_SERVER_NT = 0x00008000;
        public const uint SV_TYPE_POTENTIAL_BROWSER = 0x00010000;
        public const uint SV_TYPE_BACKUP_BROWSER = 0x00020000;
        public const uint SV_TYPE_MASTER_BROWSER = 0x00040000;
        public const uint SV_TYPE_DOMAIN_MASTER = 0x00080000;
        public const uint SV_TYPE_SERVER_OSF = 0x00100000; //******
        public const uint SV_TYPE_SERVER_VMS = 0x00200000; //******
        public const uint SV_TYPE_WINDOWS = 0x00400000; /* Windows95 and above */
        public const uint SV_TYPE_DFS = 0x00800000;  /* Root of a DFS tree */
        public const uint SV_TYPE_CLUSTER_NT = 0x01000000;  /* NT Cluster */
        public const uint SV_TYPE_TERMINALSERVER = 0x02000000;  /* Terminal Server(Hydra) */
        public const uint SV_TYPE_CLUSTER_VS_NT = 0x04000000;  /* NT Cluster Virtual Server Name */
        public const uint SV_TYPE_DCE = 0x10000000;  /* IBM DSS (Directory and Security Services) or equivalent */
        public const uint SV_TYPE_ALTERNATE_XPORT = 0x20000000;  /* return list for alternate transport */
        public const uint SV_TYPE_LOCAL_LIST_ONLY = 0x40000000;  /* Return local list only */
        public const uint SV_TYPE_DOMAIN_ENUM = 0x80000000;
        public const uint SV_TYPE_ALL = 0xFFFFFFFF;  /* handy for NetServerEnum2 */

        public const int USE_WILDCARD = -1;
        public const int USE_DISKDEV = 0;
        public const int USE_SPOOLDEV = 1;
        public const int USE_CHARDEV = 2;
        public const int USE_IPC = 3;

        public const int USE_OK = 0;
        public const int USE_PAUSED = 1;
        public const int USE_SESSLOST = 2;
        public const int USE_DISCONN = 2;
        public const int USE_NETERR = 3;
        public const int USE_CONN = 4;
        public const int USE_RECONN = 5;

        public const int UF_SCRIPT = 0x0001;
        public const int UF_ACCOUNTDISABLE = 0x0002;
        public const int UF_HOMEDIR_REQUIRED = 0x0008;
        public const int UF_LOCKOUT = 0x0010;
        public const int UF_PASSWD_NOTREQD = 0x0020;
        public const int UF_PASSWD_CANT_CHANGE = 0x0040;
        public const int UF_ENCRYPTED_TEXT_PASSWORD_ALLOWED = 0x0080;

        public const int UF_TEMP_DUPLICATE_ACCOUNT = 0x0100;
        public const int UF_NORMAL_ACCOUNT = 0x0200;
        public const int UF_INTERDOMAIN_TRUST_ACCOUNT = 0x0800;
        public const int UF_WORKSTATION_TRUST_ACCOUNT = 0x1000;
        public const int UF_SERVER_TRUST_ACCOUNT = 0x2000;

        public const int UF_MACHINE_ACCOUNT_MASK =
            UF_INTERDOMAIN_TRUST_ACCOUNT |
            UF_WORKSTATION_TRUST_ACCOUNT |
            UF_SERVER_TRUST_ACCOUNT;
        public const int UF_ACCOUNT_TYPE_MASK =
            UF_TEMP_DUPLICATE_ACCOUNT |
            UF_NORMAL_ACCOUNT |
            UF_INTERDOMAIN_TRUST_ACCOUNT |
            UF_WORKSTATION_TRUST_ACCOUNT |
            UF_SERVER_TRUST_ACCOUNT;

        public const int UF_DONT_EXPIRE_PASSWD = 0x10000;
        public const int UF_MNS_LOGON_ACCOUNT = 0x20000;
        public const int UF_SMARTCARD_REQUIRED = 0x40000;
        public const int UF_TRUSTED_FOR_DELEGATION = 0x80000;
        public const int UF_NOT_DELEGATED = 0x100000;
        public const int UF_USE_DES_KEY_ONLY = 0x200000;
        public const int UF_DONT_REQUIRE_PREAUTH = 0x400000;
        public const int UF_PASSWORD_EXPIRED = 0x800000;
        public const int UF_TRUSTED_TO_AUTHENTICATE_FOR_DELEGATION = 0x1000000;
        public const int UF_NO_AUTH_DATA_REQUIRED = 0x2000000;
        public const int UF_PARTIAL_SECRETS_ACCOUNT = 0x4000000;
        public const int UF_USE_AES_KEYS = 0x8000000;

        public const int UF_SETTABLE_BITS =
            UF_SCRIPT |
            UF_ACCOUNTDISABLE |
            UF_LOCKOUT |
            UF_HOMEDIR_REQUIRED |
            UF_PASSWD_NOTREQD |
            UF_PASSWD_CANT_CHANGE |
            UF_ACCOUNT_TYPE_MASK |
            UF_DONT_EXPIRE_PASSWD |
            UF_MNS_LOGON_ACCOUNT |
            UF_ENCRYPTED_TEXT_PASSWORD_ALLOWED |
            UF_SMARTCARD_REQUIRED |
            UF_TRUSTED_FOR_DELEGATION |
            UF_NOT_DELEGATED |
            UF_USE_DES_KEY_ONLY |
            UF_DONT_REQUIRE_PREAUTH |
            UF_PASSWORD_EXPIRED |
            UF_TRUSTED_TO_AUTHENTICATE_FOR_DELEGATION |
            UF_NO_AUTH_DATA_REQUIRED |
            UF_USE_AES_KEYS |
            UF_PARTIAL_SECRETS_ACCOUNT;

        public const uint SUPPORTS_REMOTE_ADMIN_PROTOCOL = 0x00000002;
        public const uint SUPPORTS_RPC = 0x00000004;
        public const uint SUPPORTS_SAM_PROTOCOL = 0x00000008;
        public const uint SUPPORTS_UNICODE = 0x00000010;
        public const uint SUPPORTS_LOCAL = 0x00000020;
        public const uint SUPPORTS_ANY = 0xFFFFFFFF;

        public const int SESS_GUEST = 0x00000001;  // session is logged on as a guest
        public const int SESS_NOENCRYPTION = 0x00000002;  // session is not using encryption

        

    }

    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto, Pack = 4)]
    public struct USE_INFO_2
    {
        [MarshalAs(UnmanagedType.LPTStr)]
        public string ui2_local;
        [MarshalAs(UnmanagedType.LPTStr)]
        public string ui2_remote;
        [MarshalAs(UnmanagedType.LPTStr)]
        public string ui2_password;
        NetapiUseStatus ui2_status;
        NetapiUseType ui2_asg_type;
        uint ui2_refcount;
        uint ui2_usecount;
        [MarshalAs(UnmanagedType.LPTStr)]
        public string ui2_username;
        [MarshalAs(UnmanagedType.LPTStr)]
        public string ui2_domainname;
    }

    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode, Pack = 4)]
    public struct SHARE_INFO_0
    {
        [MarshalAs(UnmanagedType.LPWStr)]
        public string shi0_netname;

        public static SHARE_INFO_0[] FromBuffer(IntPtr buffer, int entries_count)
        {
            int struct_len = Marshal.SizeOf(typeof(SHARE_INFO_0));
            IntPtr current_ptr = IntPtr.Zero;

            SHARE_INFO_0[] ret = new SHARE_INFO_0[entries_count];

            for (int i = 0; i < entries_count; i++)
            {
                current_ptr = new IntPtr(buffer.ToInt64() + (long)struct_len * (long)i);
                ret[i] = (SHARE_INFO_0)Marshal.PtrToStructure(current_ptr, typeof(SHARE_INFO_0));
            }
            return ret;
        }
    }



    //must be without pack
    //TODO: check other structs with pack attrubute
    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
    public struct SHARE_INFO_1
    {
        [MarshalAs(UnmanagedType.LPWStr)]
        public string shi1_netname;
        public NetshareType shi1_type;
        [MarshalAs(UnmanagedType.LPWStr)]
        public string shi1_remark;

        public static SHARE_INFO_1[] FromBuffer(IntPtr buffer, int entries_count)
        {
            int struct_len = Marshal.SizeOf(typeof(SHARE_INFO_1));
            IntPtr current_ptr = IntPtr.Zero;

            SHARE_INFO_1[] ret = new SHARE_INFO_1[entries_count];

            //dangerous tricks with pointers
            for (int i = 0; i < entries_count; i++)
            {
                current_ptr = new IntPtr(buffer.ToInt64() + (long)struct_len * (long)i);
                ret[i] = (SHARE_INFO_1)Marshal.PtrToStructure(current_ptr, typeof(SHARE_INFO_1));
            }
            return ret;
        }
    }

    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
    public struct SHARE_INFO_2
    {
        [MarshalAs(UnmanagedType.LPWStr)]
        public string shi2_netname;
        public NetshareType shi2_type;
        [MarshalAs(UnmanagedType.LPWStr)]
        public string shi2_remark;
        public NetsharePermissionShare shi2_permissions;
        public int shi2_max_uses;
        public int shi2_current_uses;
        [MarshalAs(UnmanagedType.LPWStr)]
        public string shi2_path;
        [MarshalAs(UnmanagedType.LPWStr)]
        public string shi2_passwd;

        public static SHARE_INFO_2[] FromBuffer(IntPtr buffer, int entries_count)
        {
            int struct_len = Marshal.SizeOf(typeof(SHARE_INFO_2));
            IntPtr current_ptr = IntPtr.Zero;

            SHARE_INFO_2[] ret = new SHARE_INFO_2[entries_count];

            for (int i = 0; i < entries_count; i++)
            {
                current_ptr = new IntPtr(buffer.ToInt64() + (long)struct_len * (long)i);
                ret[i] = (SHARE_INFO_2)Marshal.PtrToStructure(current_ptr, typeof(SHARE_INFO_2));
            }
            return ret;
        }
    }

    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode, Pack = 4)]
    public struct SHARE_INFO_502
    {
        [MarshalAs(UnmanagedType.LPWStr)]
        public string shi502_netname;
        public NetshareType shi502_type;
        [MarshalAs(UnmanagedType.LPWStr)]
        public string shi502_remark;
        public NetsharePermissionShare shi502_permissions;
        public uint shi502_max_uses;
        public uint shi502_current_uses;
        [MarshalAs(UnmanagedType.LPWStr)]
        public string shi502_path;
        [MarshalAs(UnmanagedType.LPWStr)]
        public string shi502_passwd;
        uint shi502_reserved;
        public IntPtr shi502_security_descriptor;

        public static SHARE_INFO_502[] FromBuffer(IntPtr buffer, int entries_count)
        {
            int struct_len = Marshal.SizeOf(typeof(SHARE_INFO_502));
            IntPtr current_ptr = IntPtr.Zero;

            SHARE_INFO_502[] ret = new SHARE_INFO_502[entries_count];

            for (int i = 0; i < entries_count; i++)
            {
                current_ptr = new IntPtr(buffer.ToInt64() + (long)struct_len * (long)i);
                ret[i] = (SHARE_INFO_502)Marshal.PtrToStructure(current_ptr, typeof(SHARE_INFO_502));
            }
            return ret;
        }
    }

    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
    public struct SERVER_INFO_100
    {
        public NetserverPlatformId sv100_platform_id;
        [MarshalAs(UnmanagedType.LPWStr)]
        public string sv100_name;
    }

    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
    public struct SERVER_INFO_101
    {
        public NetserverPlatformId sv101_platform_id;
        [MarshalAs(UnmanagedType.LPWStr)]
        public string sv101_name;
        public uint sv101_version_major;
        public uint sv101_version_minor;
        public NetserverEnumType sv101_type;
        [MarshalAs(UnmanagedType.LPWStr)]
        public string sv101_comment;

        public uint GetVersionMajor()
        {
            return WinApiNET.MAJOR_VERSION_MASK & sv101_version_major;
        }
    }

    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
    public struct SERVER_INFO_102
    {
        public NetserverPlatformId sv102_platform_id;
        [MarshalAs(UnmanagedType.LPWStr)]
        public string sv102_name;
        public uint sv102_version_major;
        public uint sv102_version_minor;
        public NetserverEnumType sv102_type;
        [MarshalAs(UnmanagedType.LPWStr)]
        public string sv102_comment;
        /// <summary>
        /// The number of users who can attempt to log on to the system server.
        /// Note that it is the license server that determines how many of these users can actually log on.
        /// </summary>
        public int sv102_users;
        /// <summary>
        /// The auto-disconnect time, in minutes. A session is disconnected if it is idle longer
        /// than the period of time specified by the sv102_disc member. If the value of sv102_disc
        /// is SV_NODISC, auto-disconnect is not enabled.
        /// </summary>
        public int sv102_disc;
        /// <summary>
        /// Specifies whether the server is visible to other computers in the same network domain.
        /// </summary>
        [MarshalAs(UnmanagedType.Bool)]
        public bool sv102_hidden;
        /// <summary>
        /// The network announce rate, in seconds. This rate determines how often the server
        /// is announced to other computers on the network. 
        /// </summary>
        public uint sv102_announce;
        /// <summary>
        /// The delta value for the announce rate, in milliseconds.
        /// The delta value allows randomly varied announce rates.
        /// </summary>
        public uint sv102_anndelta;
        /// <summary>
        /// The number of users per license.
        /// </summary>
        public uint sv102_licenses;
        /// <summary>
        /// A pointer to a Unicode string specifying the path to user directories.
        /// </summary>
        [MarshalAs(UnmanagedType.LPWStr)]
        public string sv102_userpath;

        public uint GetVersionMajor()
        {
            return WinApiNET.MAJOR_VERSION_MASK & sv102_version_major;
        }
    }

    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
    public struct NET_DISPLAY_USER
    {
        [MarshalAs(UnmanagedType.LPWStr)]
        public string usri1_name;

        [MarshalAs(UnmanagedType.LPWStr)]
        public string usri1_comment;

        /// <summary>
        /// combination of NetuserAccountType and NetuserInfoFlags 
        /// </summary>
        public uint usri1_flags;

        [MarshalAs(UnmanagedType.LPWStr)]
        public string usri1_full_name;

        public uint usri1_user_id;

        /// <summary>
        /// Specifies the index of the last entry returned by
        /// the NetQueryDisplayInformation function. Pass this value as the Index parameter
        /// to NetQueryDisplayInformation to return the next logical entry. Note that you
        /// should not use the value of this member for any purpose except to retrieve
        /// more data with additional calls to NetQueryDisplayInformation.
        /// </summary>
        public int usri1_next_index;

        public NetInfoFlags NetuserInfoFlags
        {
            get
            {
                //retrieve from usri1_flags
                //remove account type
                uint ret_u = ~((~usri1_flags) | WinApiNET.UF_ACCOUNT_TYPE_MASK);
                NetInfoFlags ret = (NetInfoFlags)ret_u;
                return ret;
            }
            set
            {
                //add to usri1_flags
                usri1_flags = usri1_flags | (uint)value;
            }
        }

        public NetAccountType NetuserAccountType
        {
            get
            {
                //accept mask
                uint ret_u = usri1_flags & WinApiNET.UF_ACCOUNT_TYPE_MASK;
                NetAccountType ret = (NetAccountType)ret_u;
                return ret;
            }
            set
            {
                //add to usri1_flags
                usri1_flags = usri1_flags | (uint)value;
            }
        }

        public static NET_DISPLAY_USER[] FromPtr(IntPtr buffer, int entries_count)
        {
            int struct_len = Marshal.SizeOf(typeof(NET_DISPLAY_USER));
            IntPtr current_ptr = IntPtr.Zero;
            
            NET_DISPLAY_USER[] ret = new NET_DISPLAY_USER[entries_count];

            for (int i = 0; i < entries_count; i++)
            {
                current_ptr = new IntPtr(buffer.ToInt64() + (long)struct_len * (long)i);
                ret[i] = (NET_DISPLAY_USER)Marshal.PtrToStructure(current_ptr, typeof(NET_DISPLAY_USER));
            }
            return ret;
        }
    }

    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
    public struct NET_DISPLAY_MACHINE
    {
        [MarshalAs(UnmanagedType.LPWStr)]
        public string usri2_name;

        [MarshalAs(UnmanagedType.LPWStr)]
        public string usri2_comment;

        public uint usri2_flags;
        public uint usri2_user_id;

        /// <summary>
        /// Specifies the index of the last entry returned by
        /// the NetQueryDisplayInformation function. Pass this value as the Index parameter
        /// to NetQueryDisplayInformation to return the next logical entry. Note that you
        /// should not use the value of this member for any purpose except to retrieve
        /// more data with additional calls to NetQueryDisplayInformation.
        /// </summary>
        public int usri2_next_index;

        public NetInfoFlags NetmachineInfoFlags
        {
            get
            {
                //retrieve from usri2_flags
                //remove account type
                uint ret_u = ~((~usri2_flags) | WinApiNET.UF_ACCOUNT_TYPE_MASK);
                NetInfoFlags ret = (NetInfoFlags)ret_u;
                return ret;
            }
            set
            {
                //add to usri1_flags
                usri2_flags = usri2_flags | (uint)value;
            }
        }

        public NetAccountType NetmachineAccountType
        {
            get
            {
                //accept mask
                uint ret_u = usri2_flags & WinApiNET.UF_ACCOUNT_TYPE_MASK;
                NetAccountType ret = (NetAccountType)ret_u;
                return ret;
            }
            set
            {
                //add to usri1_flags
                usri2_flags = usri2_flags | (uint)value;
            }
        }

        public static NET_DISPLAY_MACHINE[] FromPtr(IntPtr buffer, int entries_count)
        {
            int struct_len = Marshal.SizeOf(typeof(NET_DISPLAY_MACHINE));
            IntPtr current_ptr = IntPtr.Zero;

            NET_DISPLAY_MACHINE[] ret = new NET_DISPLAY_MACHINE[entries_count];

            for (int i = 0; i < entries_count; i++)
            {
                current_ptr = new IntPtr(buffer.ToInt64() + (long)struct_len * (long)i);
                ret[i] = (NET_DISPLAY_MACHINE)Marshal.PtrToStructure(current_ptr, typeof(NET_DISPLAY_MACHINE));
            }
            return ret;
        }
    }

    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
    public struct NET_DISPLAY_GROUP
    {
        [MarshalAs(UnmanagedType.LPWStr)]
        public string grpi3_name;
        [MarshalAs(UnmanagedType.LPWStr)]
        public string grpi3_comment;
        public uint grpi3_group_id;
        public uint grpi3_attributes;
        public int grpi3_next_index;

        public static NET_DISPLAY_GROUP[] FromPtr(IntPtr buffer, int entries_count)
        {
            int struct_len = Marshal.SizeOf(typeof(NET_DISPLAY_GROUP));
            IntPtr current_ptr = IntPtr.Zero;

            NET_DISPLAY_GROUP[] ret = new NET_DISPLAY_GROUP[entries_count];

            for (int i = 0; i < entries_count; i++)
            {
                current_ptr = new IntPtr(buffer.ToInt64() + (long)struct_len * (long)i);
                ret[i] = (NET_DISPLAY_GROUP)Marshal.PtrToStructure(current_ptr, typeof(NET_DISPLAY_GROUP));
            }
            return ret;
        }
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct TIME_OF_DAY_INFO
    {
        /// <summary>
        /// Specifies a DWORD value that contains the number of seconds since 00:00:00, January 1, 1970, GMT
        /// </summary>
        public uint tod_elapsedt;
        /// <summary>
        /// Specifies a DWORD value that contains the number of milliseconds from an arbitrary starting point (system reset).
        /// </summary>
        public uint tod_msecs;
        /// <summary>
        /// Specifies a DWORD value that contains the current hour. Valid values are 0 through 23
        /// </summary>
        public uint tod_hours;
        /// <summary>
        /// Specifies a DWORD value that contains the current minute. Valid values are 0 through 59
        /// </summary>
        public uint tod_mins;
        /// <summary>
        /// Specifies a DWORD value that contains the current second. Valid values are 0 through 59
        /// </summary>
        public uint tod_secs;
        /// <summary>
        /// Specifies a DWORD value that contains the current hundredth second (0.01 second). Valid values are 0 through 99
        /// </summary>
        public uint tod_hunds;
        /// <summary>
        /// Specifies the time zone of the server. This value is calculated, in minutes,
        /// from Greenwich Mean Time (GMT). For time zones west of Greenwich, the value is positive;
        /// for time zones east of Greenwich, the value is negative. A value of –1 indicates that the time zone is undefined.
        /// </summary>
        public int tod_timezone;
        /// <summary>
        /// Specifies a DWORD value that contains the time interval for each tick of the clock.
        /// Each integral integer represents one ten-thousandth second (0.0001 second).
        /// </summary>
        public uint tod_tinterval;
        /// <summary>
        /// Specifies a DWORD value that contains the day of the month. Valid values are 1 through 31
        /// </summary>
        public uint tod_day;
        /// <summary>
        /// Specifies a DWORD value that contains the month of the year. Valid values are 1 through 12
        /// </summary>
        public uint tod_month;
        /// <summary>
        /// Specifies a DWORD value that contains the year
        /// </summary>
        public uint tod_year;
        /// <summary>
        /// Specifies a DWORD value that contains the day of the week.
        /// Valid values are 0 through 6, where 0 is Sunday, 1 is Monday, and so on
        /// </summary>
        public uint tod_weekday;

        public DateTime GetCurrentDatetime()
        {
            return
                (new DateTime
                    ((int)tod_year,
                    (int)tod_month,
                    (int)tod_day,
                    (int)tod_hours,
                    (int)tod_mins,
                    (int)tod_secs,
                    (int)tod_hunds * 10)).AddMinutes(-(double)tod_timezone);
        }

        public TimeSpan GetUptime()
        {
            return new TimeSpan(0, 0, 0, 0, (int)tod_msecs);
        }
    }

    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
    public struct SERVER_TRANSPORT_INFO_1
    {
        /// <summary>
        /// The number of clients connected to the server that are
        /// using the transport protocol specified by the svti0_transportname member
        /// </summary>
        public uint svti1_numberofvcs;

        /// <summary>
        /// character string that contains the name of a transport device
        /// </summary>
        [MarshalAs(UnmanagedType.LPWStr)]
        public string svti1_transportname;

        /// <summary>
        ///  address the server is using on the transport device specified by the svti1_transportname member
        ///  it is pointer to byte array
        /// </summary>
        public IntPtr svti1_transportaddress;

        public uint svti1_transportaddresslength;

        /// <summary>
        /// character string that contains the address the network adapter is using.
        /// The string is transport-specific.
        /// </summary>
        [MarshalAs(UnmanagedType.LPWStr)]
        public string svti1_networkaddress;

        /// <summary>
        /// character string that contains the name of the domain to which the server should announce its presence
        /// </summary>
        [MarshalAs(UnmanagedType.LPWStr)]
        public string svti1_domain;

        public string TransportAddress
        {
            get
            {
                if ((svti1_transportaddress == IntPtr.Zero) || (svti1_transportaddresslength == 0))
                {
                    return string.Empty;
                }

                byte[] ret_bytes = new byte[svti1_transportaddresslength];
                Marshal.Copy(svti1_transportaddress, ret_bytes, 0, (int)svti1_transportaddresslength);
                string ret = Encoding.ASCII.GetString(ret_bytes);
                return ret;
            }
        }

        public static SERVER_TRANSPORT_INFO_1[] FromBuffer(IntPtr buffer, int entries_count)
        {
            int struct_len = Marshal.SizeOf(typeof(SERVER_TRANSPORT_INFO_1));
            IntPtr current_ptr = IntPtr.Zero;

            SERVER_TRANSPORT_INFO_1[] ret = new SERVER_TRANSPORT_INFO_1[entries_count];

            for (int i = 0; i < entries_count; i++)
            {
                current_ptr = new IntPtr(buffer.ToInt64() + (long)struct_len * (long)i);
                ret[i] = (SERVER_TRANSPORT_INFO_1)Marshal.PtrToStructure(current_ptr, typeof(SERVER_TRANSPORT_INFO_1));
            }
            return ret;
        }
    }

    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
    public struct SERVER_TRANSPORT_INFO_0
    {
        /// <summary>
        /// The number of clients connected to the server that are
        /// using the transport protocol specified by the svti0_transportname member
        /// </summary>
        public uint svti0_numberofvcs;

        /// <summary>
        /// character string that contains the name of a transport device
        /// </summary>
        [MarshalAs(UnmanagedType.LPWStr)]
        public string svti0_transportname;

        /// <summary>
        ///  address the server is using on the transport device specified by the svti0_transportname member
        ///  it is pointer to byte array
        /// </summary>
        public IntPtr svti0_transportaddress;

        public uint svti0_transportaddresslength;

        /// <summary>
        /// character string that contains the address the network adapter is using.
        /// The string is transport-specific.
        /// </summary>
        [MarshalAs(UnmanagedType.LPWStr)]
        public string svti0_networkaddress;

        public string TransportAddress
        {
            get
            {
                if ((svti0_transportaddress == IntPtr.Zero) || (svti0_transportaddresslength == 0))
                {
                    return string.Empty;
                }

                byte[] ret_bytes = new byte[svti0_transportaddresslength];
                Marshal.Copy(svti0_transportaddress, ret_bytes, 0, (int)svti0_transportaddresslength);
                string ret = Encoding.ASCII.GetString(ret_bytes);
                return ret;
            }
        }

        public static SERVER_TRANSPORT_INFO_0[] FromBuffer(IntPtr buffer, int entries_count)
        {
            int struct_len = Marshal.SizeOf(typeof(SERVER_TRANSPORT_INFO_0));
            IntPtr current_ptr = IntPtr.Zero;

            SERVER_TRANSPORT_INFO_0[] ret = new SERVER_TRANSPORT_INFO_0[entries_count];

            for (int i = 0; i < entries_count; i++)
            {
                current_ptr = new IntPtr(buffer.ToInt64() + (long)struct_len * (long)i);
                ret[i] = (SERVER_TRANSPORT_INFO_0)Marshal.PtrToStructure(current_ptr, typeof(SERVER_TRANSPORT_INFO_0));
            }
            return ret;
        }
    }

    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
    public struct SESSION_INFO_0
    {
        /// <summary>
        /// computer name
        /// </summary>
        [MarshalAs(UnmanagedType.LPWStr)]
        public string sesi0_cname;

        public static SESSION_INFO_0[] FromBuffer(IntPtr buffer, int entries_count)
        {
            int struct_len = Marshal.SizeOf(typeof(SESSION_INFO_0));
            IntPtr current_ptr = IntPtr.Zero;

            SESSION_INFO_0[] ret = new SESSION_INFO_0[entries_count];

            for (int i = 0; i < entries_count; i++)
            {
                current_ptr = new IntPtr(buffer.ToInt64() + (long)struct_len * (long)i);
                ret[i] = (SESSION_INFO_0)Marshal.PtrToStructure(current_ptr, typeof(SESSION_INFO_0));
            }
            return ret;
        }
    }

    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
    public struct SESSION_INFO_1
    {
        /// <summary>
        /// compuer name
        /// </summary>
        [MarshalAs(UnmanagedType.LPWStr)]
        public string sesi1_cname;

        /// <summary>
        ///  Unicode string specifying the name of the user who established the session
        /// </summary>
        [MarshalAs(UnmanagedType.LPWStr)]
        public string sesi1_username;

        /// <summary>
        /// value that contains the number of files, devices, and pipes opened during the session
        /// </summary>
        public int sesi1_num_opens;

        /// <summary>
        /// value that contains the number of seconds the session has been active
        /// </summary>
        public int sesi1_time;

        /// <summary>
        /// value that contains the number of seconds the session has been idle
        /// </summary>
        public int sesi1_idle_time;

        /// <summary>
        /// how the user established the session
        /// </summary>
        public NetSessionEstablishType sesi1_user_flags;

        public TimeSpan TimeActive
        {
            get
            {
                return new TimeSpan(0, 0, 0, sesi1_time);
            }
        }

        public TimeSpan TimeIdle
        {
            get
            {
                return new TimeSpan(0, 0, sesi1_idle_time);
            }
        }

        public static SESSION_INFO_1[] FromBuffer(IntPtr buffer, int entries_count)
        {
            int struct_len = Marshal.SizeOf(typeof(SESSION_INFO_1));
            IntPtr current_ptr = IntPtr.Zero;

            SESSION_INFO_1[] ret = new SESSION_INFO_1[entries_count];

            for (int i = 0; i < entries_count; i++)
            {
                current_ptr = new IntPtr(buffer.ToInt64() + (long)struct_len * (long)i);
                ret[i] = (SESSION_INFO_1)Marshal.PtrToStructure(current_ptr, typeof(SESSION_INFO_1));
            }
            return ret;
        }
    }

    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
    public struct SESSION_INFO_2
    {
        /// <summary>
        /// compuer name
        /// </summary>
        [MarshalAs(UnmanagedType.LPWStr)]
        public string sesi2_cname;

        /// <summary>
        ///  Unicode string specifying the name of the user who established the session
        /// </summary>
        [MarshalAs(UnmanagedType.LPWStr)]
        public string sesi2_username;

        /// <summary>
        /// value that contains the number of files, devices, and pipes opened during the session
        /// </summary>
        public int sesi2_num_opens;

        /// <summary>
        /// value that contains the number of seconds the session has been active
        /// </summary>
        public int sesi2_time;

        /// <summary>
        /// value that contains the number of seconds the session has been idle
        /// </summary>
        public int sesi2_idle_time;

        /// <summary>
        /// how the user established the session
        /// </summary>
        public NetSessionEstablishType sesi2_user_flags;

        /// <summary>
        /// Unicode string that specifies the type of client that established the session
        /// </summary>
        [MarshalAs(UnmanagedType.LPWStr)]
        public string sesi2_cltype_name;

        public TimeSpan TimeActive
        {
            get
            {
                return new TimeSpan(0, 0, sesi2_time);
            }
        }

        public TimeSpan TimeIdle
        {
            get
            {
                return new TimeSpan(0, 0, sesi2_idle_time);
            }
        }

        public static SESSION_INFO_2[] FromBuffer(IntPtr buffer, int entries_count)
        {
            int struct_len = Marshal.SizeOf(typeof(SESSION_INFO_2));
            IntPtr current_ptr = IntPtr.Zero;

            SESSION_INFO_2[] ret = new SESSION_INFO_2[entries_count];

            for (int i = 0; i < entries_count; i++)
            {
                current_ptr = new IntPtr(buffer.ToInt64() + (long)struct_len * (long)i);
                ret[i] = (SESSION_INFO_2)Marshal.PtrToStructure(current_ptr, typeof(SESSION_INFO_2));
            }
            return ret;
        }
    }

    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
    public struct SESSION_INFO_10
    {
        /// <summary>
        /// compuer name
        /// </summary>
        [MarshalAs(UnmanagedType.LPWStr)]
        public string sesi10_cname;

        /// <summary>
        ///  Unicode string specifying the name of the user who established the session
        /// </summary>
        [MarshalAs(UnmanagedType.LPWStr)]
        public string sesi10_username;

        /// <summary>
        /// value that contains the number of seconds the session has been active
        /// </summary>
        public int sesi10_time;

        /// <summary>
        /// value that contains the number of seconds the session has been idle
        /// </summary>
        public int sesi10_idle_time;

        public TimeSpan TimeActive
        {
            get
            {
                return new TimeSpan(0, 0, sesi10_time);
            }
        }

        public TimeSpan TimeIdle
        {
            get
            {
                return new TimeSpan(0, 0, sesi10_idle_time);
            }
        }

        public static SESSION_INFO_10[] FromBuffer(IntPtr buffer, int entries_count)
        {
            int struct_len = Marshal.SizeOf(typeof(SESSION_INFO_10));
            IntPtr current_ptr = IntPtr.Zero;

            SESSION_INFO_10[] ret = new SESSION_INFO_10[entries_count];

            for (int i = 0; i < entries_count; i++)
            {
                current_ptr = new IntPtr(buffer.ToInt64() + (long)struct_len * (long)i);
                ret[i] = (SESSION_INFO_10)Marshal.PtrToStructure(current_ptr, typeof(SESSION_INFO_10));
            }
            return ret;
        }
    }

    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
    public struct SESSION_INFO_502
    {
        /// <summary>
        /// compuer name
        /// </summary>
        [MarshalAs(UnmanagedType.LPWStr)]
        public string sesi502_cname;

        /// <summary>
        ///  Unicode string specifying the name of the user who established the session
        /// </summary>
        [MarshalAs(UnmanagedType.LPWStr)]
        public string sesi502_username;

        /// <summary>
        /// value that contains the number of files, devices, and pipes opened during the session
        /// </summary>
        public int sesi502_num_opens;

        /// <summary>
        /// value that contains the number of seconds the session has been active
        /// </summary>
        public int sesi502_time;

        /// <summary>
        /// value that contains the number of seconds the session has been idle
        /// </summary>
        public int sesi502_idle_time;

        /// <summary>
        /// how the user established the session
        /// </summary>
        public NetSessionEstablishType sesi502_user_flags;

        /// <summary>
        /// Unicode string that specifies the type of client that established the session
        /// </summary>
        [MarshalAs(UnmanagedType.LPWStr)]
        public string sesi502_cltype_name;

        /// <summary>
        /// name of the transport that the client is using to communicate with the server
        /// </summary>
        [MarshalAs(UnmanagedType.LPWStr)]
        public string sesi502_transport;

        public TimeSpan TimeActive
        {
            get
            {
                return new TimeSpan(0, 0, sesi502_time);
            }
        }

        public TimeSpan TimeIdle
        {
            get
            {
                return new TimeSpan(0, 0, sesi502_idle_time);
            }
        }

        public static SESSION_INFO_502[] FromBuffer(IntPtr buffer, int entries_count)
        {
            int struct_len = Marshal.SizeOf(typeof(SESSION_INFO_502));
            IntPtr current_ptr = IntPtr.Zero;

            SESSION_INFO_502[] ret = new SESSION_INFO_502[entries_count];

            for (int i = 0; i < entries_count; i++)
            {
                current_ptr = new IntPtr(buffer.ToInt64() + (long)struct_len * (long)i);
                ret[i] = (SESSION_INFO_502)Marshal.PtrToStructure(current_ptr, typeof(SESSION_INFO_502));
            }
            return ret;
        }
    }

    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
    public struct FILE_INFO_2
    {
        public int fi2_id;

        public static FILE_INFO_2[] FromBuffer(IntPtr buffer, int entries_count)
        {
            int struct_len = Marshal.SizeOf(typeof(FILE_INFO_2));
            IntPtr current_ptr = IntPtr.Zero;

            FILE_INFO_2[] ret = new FILE_INFO_2[entries_count];

            for (int i = 0; i < entries_count; i++)
            {
                current_ptr = new IntPtr(buffer.ToInt64() + (long)struct_len * (long)i);
                ret[i] = (FILE_INFO_2)Marshal.PtrToStructure(current_ptr, typeof(FILE_INFO_2));
            }
            return ret;
        }
    }

    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
    public struct FILE_INFO_3
    {
        public int fi3_id;
        public NetFilePermission fi3_permission;
        public int fi3_num_locks;
        [MarshalAs(UnmanagedType.LPWStr)]
        public string fi3_pathname;
        [MarshalAs(UnmanagedType.LPWStr)]
        public string fi3_username;

        public static FILE_INFO_3[] FromBuffer(IntPtr buffer, int entries_count)
        {
            int struct_len = Marshal.SizeOf(typeof(FILE_INFO_3));
            IntPtr current_ptr = IntPtr.Zero;

            FILE_INFO_3[] ret = new FILE_INFO_3[entries_count];

            for (int i = 0; i < entries_count; i++)
            {
                current_ptr = new IntPtr(buffer.ToInt64() + (long)struct_len * (long)i);
                ret[i] = (FILE_INFO_3)Marshal.PtrToStructure(current_ptr, typeof(FILE_INFO_3));
            }
            return ret;
        }
    }


    [Flags()]
    public enum NetsharePermissionShare
    {
        NONE = WinApiNET.ACCESS_NONE,
        READ = WinApiNET.ACCESS_READ,
        WRITE = WinApiNET.ACCESS_WRITE,
        CREATE = WinApiNET.ACCESS_CREATE,
        EXEC = WinApiNET.ACCESS_EXEC,
        DELETE = WinApiNET.ACCESS_DELETE,
        ATRIB = WinApiNET.ACCESS_ATRIB,
        PERM = WinApiNET.ACCESS_PERM,
        ALL = WinApiNET.ACCESS_ALL,
        GROUP = WinApiNET.ACCESS_GROUP
    }

    [Flags()]
    public enum NetshareType:uint
    {
        DISKTREE = WinApiNET.STYPE_DISKTREE,
        PRINTQ = WinApiNET.STYPE_PRINTQ,
        DEVICE = WinApiNET.STYPE_DEVICE,
        IPC = WinApiNET.STYPE_IPC,
        SPECIAL = WinApiNET.STYPE_SPECIAL,
        TEMPORARY = WinApiNET.STYPE_TEMPORARY
    }

    public enum NET_INFO_LEVEL
    {
        LEVEL_0=0,
        LEVEL_1=1,
        LEVEL_2=2,
        LEVEL_502=502
    }

    public enum NetserverPlatformId
    {
        DOS = WinApiNET.PLATFORM_ID_DOS,
        OS2 = WinApiNET.PLATFORM_ID_OS2,
        NT = WinApiNET.PLATFORM_ID_NT,
        OSF = WinApiNET.PLATFORM_ID_OSF,
        VMS = WinApiNET.PLATFORM_ID_VMS
    }

    [Flags()]
    public enum NetserverEnumType : uint
    {
        /// <summary>
        /// A LAN Manager workstation
        /// </summary>
        WORKSTATION = WinApiNET.SV_TYPE_WORKSTATION,
        /// <summary>
        /// A LAN Manager server
        /// </summary>
        SERVER = WinApiNET.SV_TYPE_SERVER,
        /// <summary>
        /// Any server running with Microsoft SQL Server
        /// </summary>
        SQLSERVER = WinApiNET.SV_TYPE_SQLSERVER,
        /// <summary>
        /// Primary domain controller
        /// </summary>
        DOMAIN_CTRL = WinApiNET.SV_TYPE_DOMAIN_CTRL,
        /// <summary>
        /// Backup domain controller
        /// </summary>
        DOMAIN_BAKCTRL = WinApiNET.SV_TYPE_DOMAIN_BAKCTRL,
        /// <summary>
        /// Server running the Timesource service
        /// </summary>
        TIME_SOURCE = WinApiNET.SV_TYPE_TIME_SOURCE,
        /// <summary>
        /// Apple File Protocol server
        /// </summary>
        AFP = WinApiNET.SV_TYPE_AFP,
        /// <summary>
        /// Novell server
        /// </summary>
        NOVELL = WinApiNET.SV_TYPE_NOVELL,
        /// <summary>
        /// LAN Manager 2.x domain member
        /// </summary>
        DOMAIN_MEMBER = WinApiNET.SV_TYPE_DOMAIN_MEMBER,
        /// <summary>
        /// Servers maintained by the browser
        /// </summary>
        LOCAL_LIST_ONLY = WinApiNET.SV_TYPE_LOCAL_LIST_ONLY,
        /// <summary>
        /// Server sharing print queue
        /// </summary>
        PRINTQ_SERVER = WinApiNET.SV_TYPE_PRINTQ_SERVER,
        /// <summary>
        /// Server running dial-in service
        /// </summary>
        DIALIN_SERVER = WinApiNET.SV_TYPE_DIALIN_SERVER,
        /// <summary>
        /// Xenix server
        /// </summary>
        XENIX_SERVER = WinApiNET.SV_TYPE_XENIX_SERVER,
        /// <summary>
        /// Microsoft File and Print for NetWare
        /// </summary>
        SERVER_MFPN = WinApiNET.SV_TYPE_SERVER_MFPN,
        /// <summary>
        /// Windows Server 2003, Windows XP, Windows 2000, or Windows NT
        /// </summary>
        NT = WinApiNET.SV_TYPE_NT,
        /// <summary>
        /// Server running Windows for Workgroups
        /// </summary>
        WFW = WinApiNET.SV_TYPE_WFW,
        /// <summary>
        /// Windows Server 2003, Windows 2000 server, or Windows NT server that is not a domain controller
        /// </summary>
        SERVER_NT = WinApiNET.SV_TYPE_SERVER_NT,
        /// <summary>
        /// Server that can run the browser service
        /// </summary>
        POTENTIAL_BROWSER = WinApiNET.SV_TYPE_POTENTIAL_BROWSER,
        /// <summary>
        /// Server running a browser service as backup
        /// </summary>
        BACKUP_BROWSER = WinApiNET.SV_TYPE_BACKUP_BROWSER,
        /// <summary>
        /// Server running the master browser service
        /// </summary>
        MASTER_BROWSER = WinApiNET.SV_TYPE_MASTER_BROWSER,
        /// <summary>
        /// Server running the domain master browser
        /// </summary>
        DOMAIN_MASTER = WinApiNET.SV_TYPE_DOMAIN_MASTER,
        /// <summary>
        /// Primary domain
        /// </summary>
        DOMAIN_ENUM = WinApiNET.SV_TYPE_DOMAIN_ENUM,
        /// <summary>
        /// Windows Me, Windows 98, or Windows 95
        /// </summary>
        WINDOWS = WinApiNET.SV_TYPE_WINDOWS,
        /// <summary>
        /// All servers
        /// </summary>
        ALL = WinApiNET.SV_TYPE_ALL,
        /// <summary>
        /// Terminal Server
        /// </summary>
        TERMINALSERVER = WinApiNET.SV_TYPE_TERMINALSERVER,
        /// <summary>
        /// Server clusters available in the domain
        /// </summary>
        CLUSTER_NT = WinApiNET.SV_TYPE_CLUSTER_NT,
        /// <summary>
        /// Cluster virtual servers available in the domain
        /// </summary>
        CLUSTER_VS_NT = WinApiNET.SV_TYPE_CLUSTER_VS_NT,

        DFS = WinApiNET.SV_TYPE_DFS,

        /// <summary>
        /// IBM DSS (Directory and Security Services) or equivalent
        /// </summary>
        DCE = WinApiNET.SV_TYPE_DCE,  /* IBM DSS (Directory and Security Services) or equivalent */

        /// <summary>
        /// return list for alternate transport
        /// </summary>
        ALTERNATE_XPORT = WinApiNET.SV_TYPE_ALTERNATE_XPORT, /* return list for alternate transport */

        OSF = WinApiNET.SV_TYPE_SERVER_OSF,
        VMS = WinApiNET.SV_TYPE_SERVER_VMS
    }

    public enum NetserverEnumLevel
    {
        LEVEL_100=100,
        LEVEL_101=101
    }

    public enum NetapiUseType
    {
        /// <summary>
        /// Matches the type of the server's shared resources.
        /// Wildcards can be used only with the NetUseAdd function,
        /// and only when the ui2_local member is a null string. 
        /// </summary>
        WILDCARD = WinApiNET.USE_WILDCARD,
        DISKDEV = WinApiNET.USE_DISKDEV,
        SPOOLDEV = WinApiNET.USE_SPOOLDEV,
        CHARDEV = WinApiNET.USE_CHARDEV,
        IPC = WinApiNET.USE_IPC
    }

    public enum NetapiUseStatus
    {
        OK = WinApiNET.USE_OK,
        PAUSED = WinApiNET.USE_PAUSED,
        SESSLOST = WinApiNET.USE_SESSLOST,
        DISCONN = WinApiNET.USE_DISCONN,
        NETERR = WinApiNET.USE_NETERR,
        CONN = WinApiNET.USE_CONN,
        RECONN = WinApiNET.USE_RECONN
    }

    [Flags()]
    public enum NetInfoFlags
    {
        None=0,

        /// <summary>
        /// The logon script executed. This value must be set.
        /// </summary>
        UF_SCRIPT = WinApiNET.UF_SCRIPT,

        /// <summary>
        /// The user's account is disabled.
        /// </summary>
        UF_ACCOUNTDISABLE = WinApiNET.UF_ACCOUNTDISABLE,

        /// <summary>
        /// No password is required.
        /// </summary>
        UF_PASSWD_NOTREQD = WinApiNET.UF_PASSWD_NOTREQD,

        /// <summary>
        /// The user cannot change the password.
        /// </summary>
        UF_PASSWD_CANT_CHANGE = WinApiNET.UF_PASSWD_CANT_CHANGE,

        /// <summary>
        /// The account is currently locked out (blocked).
        /// For the NetUserSetInfo function, this value can be cleared
        /// to unlock a previously locked account. This value cannot be used to lock a previously unlocked account.
        /// </summary>
        UF_LOCKOUT = WinApiNET.UF_LOCKOUT,

        /// <summary>
        /// Represents the password, which will never expire on the account.
        /// </summary>
        UF_DONT_EXPIRE_PASSWD = WinApiNET.UF_DONT_EXPIRE_PASSWD,

        /// <summary>
        /// The account is enabled for delegation. This is a security-sensitive setting;
        /// accounts with this option enabled should be tightly controlled.
        /// This setting allows a service running under the account to assume
        /// a client's identity and authenticate as that user to other remote servers on the network.
        /// </summary>
        UF_TRUSTED_FOR_DELEGATION = WinApiNET.UF_TRUSTED_FOR_DELEGATION,

        /// <summary>
        /// The user's password is stored under reversible encryption in the Active Directory.
        /// </summary>
        UF_ENCRYPTED_TEXT_PASSWORD_ALLOWED = WinApiNET.UF_ENCRYPTED_TEXT_PASSWORD_ALLOWED,

        /// <summary>
        /// Marks the account as "sensitive"; other users cannot act as delegates of this user account.
        /// </summary>
        UF_NOT_DELEGATED = WinApiNET.UF_NOT_DELEGATED,

        /// <summary>
        /// Requires the user to log on to the user account with a smart card.
        /// </summary>
        UF_SMARTCARD_REQUIRED = WinApiNET.UF_SMARTCARD_REQUIRED,

        /// <summary>
        /// Restrict this principal to use only Data Encryption Standard (DES) encryption types for keys.
        /// </summary>
        UF_USE_DES_KEY_ONLY = WinApiNET.UF_USE_DES_KEY_ONLY,

        /// <summary>
        /// This account does not require Kerberos preauthentication for logon.
        /// </summary>
        UF_DONT_REQUIRE_PREAUTH = WinApiNET.UF_DONT_REQUIRE_PREAUTH,

        /// <summary>
        /// The user's password has expired.
        /// </summary>
        UF_PASSWORD_EXPIRED = WinApiNET.UF_PASSWORD_EXPIRED,

        /// <summary>
        /// The account is trusted to authenticate a user outside of the Kerberos security
        /// package and delegate that user through constrained delegation.
        /// This is a security-sensitive setting; accounts with this option enabled
        /// should be tightly controlled. This setting allows a service running under the account
        /// to assert a client's identity and authenticate as that user to specifically configured
        /// services on the network.
        /// Windows XP/2000:  This value is not supported.
        /// </summary>
        UF_TRUSTED_TO_AUTHENTICATE_FOR_DELEGATION = WinApiNET.UF_TRUSTED_TO_AUTHENTICATE_FOR_DELEGATION,
    }

    public enum NetAccountType
    {

        /// <summary>
        /// This is a default account type that represents a typical user.
        /// </summary>
        UF_NORMAL_ACCOUNT = WinApiNET.UF_NORMAL_ACCOUNT,

        /// <summary>
        /// This is an account for users whose primary account is in another domain.
        /// This account provides user access to this domain, but not to any domain that
        /// trusts this domain. The User Manager refers to this account type as a local user account.
        /// </summary>
        UF_TEMP_DUPLICATE_ACCOUNT = WinApiNET.UF_TEMP_DUPLICATE_ACCOUNT,

        /// <summary>
        /// This is a computer account for a workstation or a server that is a member of this domain.
        /// </summary>
        UF_WORKSTATION_TRUST_ACCOUNT = WinApiNET.UF_WORKSTATION_TRUST_ACCOUNT,

        /// <summary>
        /// This is a computer account for a backup domain controller that is a member of this domain.
        /// </summary>
        UF_SERVER_TRUST_ACCOUNT = WinApiNET.UF_SERVER_TRUST_ACCOUNT,

        /// <summary>
        /// This is a permit to trust account for a domain that trusts other domains.
        /// </summary>
        UF_INTERDOMAIN_TRUST_ACCOUNT = WinApiNET.UF_INTERDOMAIN_TRUST_ACCOUNT
    }

    public enum NetqueryDisplayInfoLevel
    {
        User = 1,
        Machine = 2,
        Group = 3
    }

    public enum NetserverInfoLevel
    {
        INFO_100 = 100,
        INFO_101 = 101,
        INFO_102 = 102,
    }

    [Flags()]
    public enum NetRemoteComputerSupportsFeatures : uint
    {
        REMOTE_ADMIN_PROTOCOL = WinApiNET.SUPPORTS_REMOTE_ADMIN_PROTOCOL,
        RPC = WinApiNET.SUPPORTS_RPC,
        SAM = WinApiNET.SUPPORTS_SAM_PROTOCOL,
        UNICODE = WinApiNET.SUPPORTS_UNICODE,
        ANY = WinApiNET.SUPPORTS_ANY,
        LOCAL = WinApiNET.SUPPORTS_LOCAL
    }

    public enum NetServerTransportEnumLevel
    {
        INFO_0 = 0,
        INFO_1 = 1
    }

    [Flags()]
    public enum NetSessionEstablishType
    {
        NORMAL = 0,
        GUEST = WinApiNET.SESS_GUEST,
        NOENCRIPTION = WinApiNET.SESS_NOENCRYPTION
    }

    public enum NetSessionEnumLevel
    {
        INFO_0 = 0,
        INFO_1 = 1,
        INFO_2 = 2,
        INFO_10 = 10,
        INFO_502 = 502
    }

    [Flags()]
    public enum NetFilePermission
    {
        NONE = 0,
        READ = WinApiNET.ACCESS_READ, // user has read access
        WRITE = WinApiNET.ACCESS_WRITE, // user has write access
        CREATE = WinApiNET.ACCESS_CREATE, // user has create access

        //from open specifications:
        EXECUTE = WinApiNET.ACCESS_EXEC,     //Permission to execute a resourse
        DELETE = WinApiNET.ACCESS_DELETE,  //Permission to delete a resource.
        MODIFY_ATTRIBUTES = WinApiNET.ACCESS_ATRIB,  //Permission to modify the attributes of a resource.
        MODIFY_PERMISSIONS = WinApiNET.ACCESS_PERM
    }

    public enum NetFileEnumLevel
    {
        INFO_2 = 2,
        INFO_3 = 3
    }

    public enum NetShareInfoLevel
    {
        INFO_0 = 0,
        INFO_1 = 1,
        INFO_2 = 2,
        INFO_501 = 501,
        INFO_502 = 502,
        INFO_503 = 503,
        INFO_1005 = 1005
    }
}
