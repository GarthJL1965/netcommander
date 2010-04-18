using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;

namespace netCommander.WNet
{
    public class WinApiWNET
    {
        private WinApiWNET()
        {

        }

        

        /// <summary>
        /// When provided with a remote path to a network resource,
        /// the WNetGetResourceInformation function identifies
        /// the network provider that owns the resource and obtains
        /// information about the type of the resource.
        /// The function is typically used in conjunction with
        /// the WNetGetResourceParent function to parse and interpret a network path typed in by a user.
        /// </summary>
        /// <param name="lpNetResource">ointer to a NETRESOURCE structure that specifies
        /// the network resource for which information is required. </param>
        /// <param name="lpBuffer">Pointer to the buffer to receive the result.
        /// On successful return, the first portion of the buffer is a NETRESOURCE 
        /// structure representing that portion of the input resource path that 
        /// is accessed through the WNet functions, rather than through system
        /// functions specific to the input resource type. (The remainder
        /// of the buffer contains the variable-length strings to which the members 
        /// of the NETRESOURCE structure point.) For example, if the input remote
        /// resource path is \\server\share\dir1\dir2, then the output NETRESOURCE structure
        /// contains information about the resource \\server\share. The \dir1\dir2 portion
        /// of the path is accessed through the file management functions. The lpRemoteName,
        /// lpProvider, dwType, dwDisplayType, and dwUsage members of NETRESOURCE are returned,
        /// with all other members set to NULL.</param>
        /// <param name="lpcbBuffer">Pointer to a location that, on entry, specifies the size
        /// of the lpBuffer buffer, in bytes. The buffer you allocate must be large enough to
        /// hold the NETRESOURCE structure, plus the strings to which its members point.
        /// If the buffer is too small for the result, this location receives the required buffer size,
        /// and the function returns ERROR_MORE_DATA.</param>
        /// <param name="lplpSystem">If the function returns successfully, this parameter points
        /// to a string in the output buffer that specifies the part of the resource that
        /// is accessed through system functions.</param>
        /// <returns>If the function succeeds, the return value is NO_ERROR.</returns>
        [DllImport("mpr.dll", CharSet = CharSet.Auto, SetLastError = false)]
        public static extern uint WNetGetResourceInformation
            (ref NETRESOURCE lpNetResource,
            IntPtr lpBuffer,
            ref int lpcbBuffer,
            ref IntPtr lplpSystem);


        [DllImport("mpr.dll", CharSet = CharSet.Auto, SetLastError = false)]
        public static extern int WNetGetNetworkInformation
            ([MarshalAs(UnmanagedType.LPTStr)]
            string lpProvider,
            ref  NETINFOSTRUCT lpNetInfoStruct);


        /// <summary>
        /// The WNetAddConnection2 function makes a connection to a network resource.
        /// The function can redirect a local device to the network resource.
        /// The WNetAddConnection2 function supersedes the WNetAddConnection function.
        /// If you can pass a handle to a window that the provider of network resources can
        /// use as an owner window for dialog boxes, call the WNetAddConnection3 function instead.
        /// </summary>
        /// <param name="lpNetResource">A pointer to a NETRESOURCE structure that specifies details
        /// of the proposed connection, such as information about the network resource,
        /// the local device, and the network resource provider.</param>
        /// <param name="lpPassword">A pointer to a constant null-terminated string that specifies
        /// a password to be used in making the network connection. If lpPassword is NULL,
        /// the function uses the current default password associated with the user specified
        /// by the lpUserName parameter. If lpPassword points to an empty string, the function
        /// does not use a password. If the connection fails because of an invalid password and
        /// the CONNECT_INTERACTIVE value is set in the dwFlags parameter, the function displays
        /// a dialog box asking the user to type the password.
        /// Windows Me/98/95:  This parameter must be NULL or an empty string.</param>
        /// <param name="lpUsername">A pointer to a constant null-terminated string that specifies
        /// a user name for making the connection. If lpUserName is NULL, the function uses
        /// the default user name. (The user context for the process provides the default user name.)
        /// The lpUserName parameter is specified when users want to connect to a network resource for
        /// which they have been assigned a user name or account other than the default
        /// user name or account. The user-name string represents a security context.
        /// It may be specific to a network provider.
        /// Windows Me/98/95:  This parameter must be NULL or an empty string.</param>
        /// <param name="dwFlags"></param>
        /// <returns>If the function succeeds, the return value is NO_ERROR.
        /// If the function fails, the return value is a system error code</returns>
        [DllImport("mpr.dll", CharSet = CharSet.Auto, SetLastError = false)]
        public static extern int WNetAddConnection2
            (ref NETRESOURCE lpNetResource,
            [MarshalAs(UnmanagedType.LPTStr)]
            string lpPassword,
            [MarshalAs(UnmanagedType.LPTStr)]
            string lpUsername,
            WNetConnectOptions dwFlags);

        [DllImport("mpr.dll", CharSet = CharSet.Auto, SetLastError = false)]
        public static extern int WNetAddConnection3
            (IntPtr hwndOwner,
            ref NETRESOURCE lpNetResource,
            [MarshalAs(UnmanagedType.LPTStr)]
            string lpPassword,
            [MarshalAs(UnmanagedType.LPTStr)]
            string lpUserName,
            WNetConnectOptions dwFlags);


        /// <summary>
        /// The WNetCancelConnection2 function cancels an existing network connection.
        /// You can also call the function to remove remembered network connections
        /// that are not currently connected.
        /// </summary>
        /// <param name="lpName">Pointer to a constant null-terminated string that specifies
        /// the name of either the redirected local device or the remote network resource
        /// to disconnect from. If this parameter specifies a redirected local device,
        /// the function cancels only the specified device redirection.
        /// If the parameter specifies a remote network resource, all connections without devices
        /// are canceled.</param>
        /// <param name="dwFlags">CONNECT_UPDATE_PROFILE or 0</param>
        /// <param name="fForce">Specifies whether the disconnection should occur
        /// if there are open files or jobs on the connection. If this parameter is FALSE,
        /// the function fails if there are open files or jobs.</param>
        /// <returns>If the function succeeds, the return value is NO_ERROR. 
        /// If the function fails, the return value is a system error code</returns>
        [DllImport("mpr.dll", CharSet = CharSet.Auto, SetLastError = false)]
        public static extern int WNetCancelConnection2
            ([MarshalAs(UnmanagedType.LPTStr)]
            string lpName,
            uint dwFlags,
            int fForce);


        //TODO: WNet functions not set last error!

        /// <summary>
        /// The WNetGetResourceParent function returns the parent of a network resource
        /// in the network browse hierarchy. Browsing begins at the location of the specified
        /// network resource.Call the WNetGetResourceInformation and WNetGetResourceParent functions
        /// to move up the network hierarchy. Call the WNetOpenEnum function to move down the hierarchy.
        /// </summary>
        /// <param name="lpNetResource">Pointer to a NETRESOURCE structure that specifies the network
        /// resource for which the parent name is required.Specify the members of the input
        /// NETRESOURCE structure as follows. The caller typically knows the values to provide
        /// for the lpProvider and dwType members after previous calls to WNetGetResourceInformation
        /// or WNetGetResourceParent.</param>
        /// <param name="lpBuffer">Pointer to a buffer to receive a single NETRESOURCE structure
        /// that represents the parent resource. The function returns the lpRemoteName, lpProvider, dwType,
        /// dwDisplayType, and dwUsage members of the structure; all other members are set to NULL.
        /// The lpRemoteName member points to the remote name for the parent resource. This name uses
        /// the same syntax as the one returned from an enumeration by the WNetEnumResource function.
        /// The caller can perform a string comparison to determine whether the WNetGetResourceParent
        /// resource is the same as that returned by WNetEnumResource. If the input resource has
        /// no parent on any of the networks, the lpRemoteName member is returned as NULL.
        /// The presence of the RESOURCEUSAGE_CONNECTABLE bit in the dwUsage member indicates
        /// that you can connect to the parent resource, but only when it is available on the network.</param>
        /// <param name="lpcbBuffer">Pointer to a location that, on entry, specifies the size
        /// of the lpBuffer buffer, in bytes. If the buffer is too small to hold the result,
        /// this location receives the required buffer size, and the function returns ERROR_MORE_DATA.</param>
        /// <returns>If the function succeeds, the return value is NO_ERROR</returns>
        [DllImport("mpr.dll", CharSet = CharSet.Auto, SetLastError = false)]
        public static extern uint WNetGetResourceParent
             (ref NETRESOURCE lpNetResource,
             IntPtr lpBuffer,
             ref int lpcbBuffer);


        /// <summary>
        /// The WNetOpenEnum function starts an enumeration of network resources
        /// or existing connections. You can continue the enumeration
        /// by calling the WNetEnumResource function.
        /// </summary>
        /// <param name="dwScope"></param>
        /// <param name="dwType">If a network provider cannot
        /// distinguish between print and disk resources, it can enumerate all resources.</param>
        /// <param name="dwUsage">This parameter is ignored
        /// unless the dwScope parameter is equal to RESOURCE_GLOBALNET.</param>
        /// <param name="lpNetResource">Pointer to a NETRESOURCE structure that specifies the container
        /// to enumerate. If the dwScope parameter is not RESOURCE_GLOBALNET, this parameter must be NULL.
        /// If this parameter is NULL, the root of the network is assumed.
        /// (The system organizes a network as a hierarchy; the root is the topmost
        /// container in the network.) If this parameter is not NULL, it must point
        /// to a NETRESOURCE structure. This structure can be filled in by the application
        /// or it can be returned by a call to the WNetEnumResource function.
        /// The NETRESOURCE structure must specify a container resource; that is,
        /// the RESOURCEUSAGE_CONTAINER value must be specified in the dwUsage parameter.'
        /// To enumerate all network resources, an application can begin the enumeration
        /// by calling WNetOpenEnum with the lpNetResource parameter set to NULL, and then
        /// use the returned handle to call WNetEnumResource to enumerate resources.
        /// If one of the resources in the NETRESOURCE array returned by the WNetEnumResource
        /// function is a container resource, you can call WNetOpenEnum to open the resource
        /// for further enumeration.</param>
        /// <param name="lphEnum">Pointer to an enumeration handle that can be used
        /// in a subsequent call to WNetEnumResource</param>
        /// <returns>If the function succeeds, the return value is NO_ERROR.
        /// If the function fails, the return value is a system error code</returns>
        [DllImport("mpr.dll", CharSet = CharSet.Auto, SetLastError = false)]
        public static extern uint WNetOpenEnum
            (ResourceScope dwScope,
            ResourceType dwType,
            ResourceUsage dwUsage,
            ref NETRESOURCE lpNetResource,
            ref IntPtr lphEnum);

        /// <summary>
        /// The WNetOpenEnum function starts an enumeration of network resources
        /// or existing connections. You can continue the enumeration
        /// by calling the WNetEnumResource function.
        /// </summary>
        /// <param name="dwScope"></param>
        /// <param name="dwType">If a network provider cannot
        /// distinguish between print and disk resources, it can enumerate all resources.</param>
        /// <param name="dwUsage">This parameter is ignored
        /// unless the dwScope parameter is equal to RESOURCE_GLOBALNET.</param>
        /// <param name="lpNetResource">Pointer to a NETRESOURCE structure that specifies the container
        /// to enumerate. If the dwScope parameter is not RESOURCE_GLOBALNET, this parameter must be NULL.
        /// If this parameter is NULL, the root of the network is assumed.
        /// (The system organizes a network as a hierarchy; the root is the topmost
        /// container in the network.) If this parameter is not NULL, it must point
        /// to a NETRESOURCE structure. This structure can be filled in by the application
        /// or it can be returned by a call to the WNetEnumResource function.
        /// The NETRESOURCE structure must specify a container resource; that is,
        /// the RESOURCEUSAGE_CONTAINER value must be specified in the dwUsage parameter.'
        /// To enumerate all network resources, an application can begin the enumeration
        /// by calling WNetOpenEnum with the lpNetResource parameter set to NULL, and then
        /// use the returned handle to call WNetEnumResource to enumerate resources.
        /// If one of the resources in the NETRESOURCE array returned by the WNetEnumResource
        /// function is a container resource, you can call WNetOpenEnum to open the resource
        /// for further enumeration.</param>
        /// <param name="lphEnum">Pointer to an enumeration handle that can be used
        /// in a subsequent call to WNetEnumResource</param>
        /// <returns>If the function succeeds, the return value is NO_ERROR.
        /// If the function fails, the return value is a system error code</returns>
        [DllImport("mpr.dll", CharSet = CharSet.Auto, SetLastError = false)]
        public static extern uint WNetOpenEnum
            (ResourceScope dwScope,
            ResourceType dwType,
            ResourceUsage dwUsage,
            IntPtr lpNetResource,
            ref IntPtr lphEnum);

        [DllImport("mpr.dll", CharSet = CharSet.Auto, SetLastError = true)]
        public static extern uint WNetCloseEnum
            (IntPtr hEnum);

        /// <summary>
        /// The WNetEnumResource function continues an enumeration of network resources
        /// that was started by a call to the WNetOpenEnum function.
        /// </summary>
        /// <param name="hEnum">Handle that identifies an enumeration instance.
        /// This handle must be returned by the WNetOpenEnum function.</param>
        /// <param name="lpcCount">Pointer to a variable specifying the number of entries requested.
        /// If the number requested is –1, the function returns as many
        /// entries as possible. If the function succeeds, on return the variable
        /// pointed to by this parameter contains the number of entries actually read.</param>
        /// <param name="lpBuffer">Pointer to the buffer that receives the enumeration results.
        /// The results are returned as an array of NETRESOURCE structures.
        /// Note that the buffer you allocate must be large enough to hold the structures,
        /// plus the strings to which their members point. For more information,
        /// see the following Remarks section. The buffer is valid until the next call using
        /// the handle specified by the hEnum parameter. The order of NETRESOURCE structures
        /// in the array is not predictable.</param>
        /// <param name="lpBufferSize">Pointer to a variable that specifies the size
        /// of the lpBuffer parameter, in bytes. If the buffer is too small to receive even
        /// one entry, this parameter receives the required size of the buffer.</param>
        /// <returns></returns>
        [DllImport("mpr.dll", CharSet = CharSet.Auto, SetLastError = true)]
        public static extern uint WNetEnumResource
            (IntPtr hEnum,
            ref int lpcCount,
            IntPtr lpBuffer,
            ref int lpBufferSize);

        /// <summary>
        /// The WNetGetLastError function retrieves the most recent extended error code
        /// set by a WNet function. The network provider reported this error code;
        /// it will not generally be one of the errors included in the SDK header file WinError.h.
        /// </summary>
        /// <param name="lpError">Pointer to a variable that receives the error code reported
        /// by the network provider. The error code is specific to the network provider.</param>
        /// <param name="lpErrorBuf">Pointer to the buffer that receives the null-terminated
        /// string describing the error.</param>
        /// <param name="nErrorBufSize">Size of the buffer pointed to by the lpErrorBuf parameter,
        /// in characters. If the buffer is too small for the error string,
        /// the string is truncated but still null-terminated. A buffer of at least
        /// 256 characters is recommended.</param>
        /// <param name="lpNameBuf">Pointer to the buffer that receives the null-terminated
        /// string identifying the network provider that raised the error.</param>
        /// <param name="nNameBufSize">Size of the buffer pointed to by the lpNameBuf parameter,
        /// in characters. If the buffer is too small for the error string,
        /// the string is truncated but still null-terminated.</param>
        /// <returns>If the function succeeds, and it obtains the last error that the network
        /// provider reported, the return value is NO_ERROR.
        /// If the caller supplies an invalid buffer, the return value is ERROR_INVALID_ADDRESS.</returns>
        [DllImport("mpr.dll", CharSet = CharSet.Auto, SetLastError = true)]
        public static extern uint WNetGetLastError
            (ref uint lpError,
            IntPtr lpErrorBuf,
            int nErrorBufSize,
            IntPtr lpNameBuf,
            int nNameBufSize);


        public const uint NO_ERROR = 0;
        public const uint ERROR_MORE_DATA = 234;
        public const uint ERROR_EXTENDED_ERROR = 1208;
        public const uint ERROR_NO_MORE_ITEMS = 259;
        public const UInt32 RESOURCE_CONNECTED = 0x00000001;
        public const UInt32 RESOURCE_GLOBALNET = 0x00000002;
        public const UInt32 RESOURCE_REMEMBERED = 0x00000003;
        public const UInt32 RESOURCETYPE_ANY = 0x00000000;
        public const UInt32 RESOURCETYPE_DISK = 0x00000001;
        public const UInt32 RESOURCETYPE_PRINT = 0x00000002;
        public const uint RESOURCEDISPLAYTYPE_GENERIC = 0x00000000;
        public const uint RESOURCEDISPLAYTYPE_DOMAIN = 0x00000001;
        public const uint RESOURCEDISPLAYTYPE_SERVER = 0x00000002;
        public const uint RESOURCEDISPLAYTYPE_SHARE = 0x00000003;
        public const uint RESOURCEDISPLAYTYPE_FILE = 0x00000004;
        public const uint RESOURCEDISPLAYTYPE_GROUP = 0x00000005;
        public const uint RESOURCEDISPLAYTYPE_NETWORK = 0x00000006;
        public const uint RESOURCEDISPLAYTYPE_ROOT = 0x00000007;
        public const uint RESOURCEDISPLAYTYPE_SHAREADMIN = 0x00000008;
        public const uint RESOURCEDISPLAYTYPE_DIRECTORY = 0x00000009;
        public const uint RESOURCEDISPLAYTYPE_TREE = 0x0000000A;
        public const uint RESOURCEDISPLAYTYPE_NDSCONTAINER = 0x0000000A;
        public const uint RESOURCEUSAGE_CONNECTABLE = 0x00000001;
        public const uint RESOURCEUSAGE_CONTAINER = 0x00000002;
        public const uint RESOURCEUSAGE_NOLOCALDEVICE = 0x00000004;
        public const uint RESOURCEUSAGE_SIBLING = 0x00000008;
        public const uint RESOURCEUSAGE_ATTACHED = 0x00000010;

        public const uint CONNECT_UPDATE_PROFILE = 0x00000001;
        public const uint CONNECT_UPDATE_RECENT = 0x00000002;
        public const uint CONNECT_TEMPORARY = 0x00000004;
        public const uint CONNECT_INTERACTIVE = 0x00000008;
        public const uint CONNECT_PROMPT = 0x00000010;
        public const uint CONNECT_NEED_DRIVE = 0x00000020;
        public const uint CONNECT_REFCOUNT = 0x00000040;
        public const uint CONNECT_REDIRECT = 0x00000080;
        public const uint CONNECT_LOCALDRIVE = 0x00000100;
        public const uint CONNECT_CURRENT_MEDIA = 0x00000200;
        public const uint CONNECT_DEFERRED = 0x00000400;
        public const uint CONNECT_RESERVED = 0xFF000000;
        public const uint CONNECT_COMMANDLINE = 0x00000800;
        public const uint CONNECT_CMD_SAVECRED = 0x00001000;
        public const uint CONNECT_CRED_RESET = 0x00002000;

        //net provider types
        public const uint WNNC_NET_MSNET = 0x00010000;
        public const uint WNNC_NET_LANMAN = 0x00020000;
        public const uint WNNC_NET_NETWARE = 0x00030000;
        public const uint WNNC_NET_VINES = 0x00040000;
        public const uint WNNC_NET_10NET = 0x00050000;
        public const uint WNNC_NET_LOCUS = 0x00060000;
        public const uint WNNC_NET_SUN_PC_NFS = 0x00070000;
        public const uint WNNC_NET_LANSTEP = 0x00080000;
        public const uint WNNC_NET_9TILES = 0x00090000;
        public const uint WNNC_NET_LANTASTIC = 0x000A0000;
        public const uint WNNC_NET_AS400 = 0x000B0000;
        public const uint WNNC_NET_FTP_NFS = 0x000C0000;
        public const uint WNNC_NET_PATHWORKS = 0x000D0000;
        public const uint WNNC_NET_LIFENET = 0x000E0000;
        public const uint WNNC_NET_POWERLAN = 0x000F0000;
        public const uint WNNC_NET_BWNFS = 0x00100000;
        public const uint WNNC_NET_COGENT = 0x00110000;
        public const uint WNNC_NET_FARALLON = 0x00120000;
        public const uint WNNC_NET_APPLETALK = 0x00130000;
        public const uint WNNC_NET_INTERGRAPH = 0x00140000;
        public const uint WNNC_NET_SYMFONET = 0x00150000;
        public const uint WNNC_NET_CLEARCASE = 0x00160000;
        public const uint WNNC_NET_FRONTIER = 0x00170000;
        public const uint WNNC_NET_BMC = 0x00180000;
        public const uint WNNC_NET_DCE = 0x00190000;
        public const uint WNNC_NET_AVID = 0x001A0000;
        public const uint WNNC_NET_DOCUSPACE = 0x001B0000;
        public const uint WNNC_NET_MANGOSOFT = 0x001C0000;
        public const uint WNNC_NET_SERNET = 0x001D0000;
        public const uint WNNC_NET_RIVERFRONT1 = 0x001E0000;
        public const uint WNNC_NET_RIVERFRONT2 = 0x001F0000;
        public const uint WNNC_NET_DECORB = 0x00200000;
        public const uint WNNC_NET_PROTSTOR = 0x00210000;
        public const uint WNNC_NET_FJ_REDIR = 0x00220000;
        public const uint WNNC_NET_DISTINCT = 0x00230000;
        public const uint WNNC_NET_TWINS = 0x00240000;
        public const uint WNNC_NET_RDR2SAMPLE = 0x00250000;
        public const uint WNNC_NET_CSC = 0x00260000;
        public const uint WNNC_NET_3IN1 = 0x00270000;
        public const uint WNNC_NET_EXTENDNET = 0x00290000;
        public const uint WNNC_NET_STAC = 0x002A0000;
        public const uint WNNC_NET_FOXBAT = 0x002B0000;
        public const uint WNNC_NET_YAHOO = 0x002C0000;
        public const uint WNNC_NET_EXIFS = 0x002D0000;
        public const uint WNNC_NET_DAV = 0x002E0000;
        public const uint WNNC_NET_KNOWARE = 0x002F0000;
        public const uint WNNC_NET_OBJECT_DIRE = 0x00300000;
        public const uint WNNC_NET_MASFAX = 0x00310000;
        public const uint WNNC_NET_HOB_NFS = 0x00320000;
        public const uint WNNC_NET_SHIVA = 0x00330000;
        public const uint WNNC_NET_IBMAL = 0x00340000;
        public const uint WNNC_NET_LOCK = 0x00350000;
        public const uint WNNC_NET_TERMSRV = 0x00360000;
        public const uint WNNC_NET_SRT = 0x00370000;
        public const uint WNNC_NET_QUINCY = 0x00380000;
        public const uint WNNC_NET_OPENAFS = 0x00390000;
        public const uint WNNC_NET_AVID1 = 0x003A0000;
        public const uint WNNC_NET_DFS = 0x003B0000;
        public const uint WNNC_NET_KWNP = 0x003C0000;
        public const uint WNNC_NET_ZENWORKS = 0x003D0000;
        public const uint WNNC_NET_DRIVEONWEB = 0x003E0000;
        public const uint WNNC_NET_VMWARE = 0x003F0000;
        public const uint WNNC_NET_RSFX = 0x00400000;
        public const uint WNNC_NET_MFILES = 0x00410000;
        public const uint WNNC_NET_MS_NFS = 0x00420000;
        public const uint WNNC_NET_GOOGLE = 0x00430000;
        public const uint WNNC_CRED_MANAGER = 0xFFFF0000;
    }

    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
    public struct CONNECTDLGSTRUCT
    {
        public int cbStructureSize;
        public IntPtr hwndOwner;
        [MarshalAs(UnmanagedType.LPStruct)]
        public NETRESOURCE lpConnRes;
        public ConnectDialogFlags dwFlags;
        public int dwDevNum;

        public static CONNECTDLGSTRUCT Prepare()
        {
            CONNECTDLGSTRUCT ret = new CONNECTDLGSTRUCT();
            ret.cbStructureSize = Marshal.SizeOf(typeof(CONNECTDLGSTRUCT));
            return ret;
        }

        public static CONNECTDLGSTRUCT Prepare(ConnectDialogFlags flags)
        {
            CONNECTDLGSTRUCT ret = CONNECTDLGSTRUCT.Prepare();
            ret.dwFlags = flags;
            return ret;
        }

        public static CONNECTDLGSTRUCT Prepare(ConnectDialogFlags flags, IntPtr window)
        {
            CONNECTDLGSTRUCT ret = CONNECTDLGSTRUCT.Prepare(flags);
            ret.hwndOwner = window;
            return ret;
        }

        public static CONNECTDLGSTRUCT Prepare(ConnectDialogFlags flags, IntPtr window, NETRESOURCE resource)
        {
            CONNECTDLGSTRUCT ret = CONNECTDLGSTRUCT.Prepare(flags, window);
            resource.lpLocalName = null;
            resource.dwType = ResourceType.DISK;
            ret.lpConnRes = resource;
            return ret;
        }
    }

    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
    public struct NETINFOSTRUCT
    {
        public int cbStructureSize;
        public int dwProviderVersion;
        public NetworkStatus dwStatus;
        public int dwCharacteristics;
        //public int dwHandle; //ULONG_PTR
        public IntPtr dwHandle;
        public NetworkType wNetType;
        /// <summary>
        /// Set of bit flags indicating the valid print numbers for redirecting local printer devices,
        /// with the low-order bit corresponding to LPT1.
        /// </summary>
        public int dwPrinters;
        /// <summary>
        /// Set of bit flags indicating the valid local disk devices
        /// for redirecting disk drives, with the low-order bit corresponding to A:.
        /// </summary>
        public int dwDrives;

        public static NETINFOSTRUCT Prepare()
        {
            NETINFOSTRUCT ret = new NETINFOSTRUCT();
            ret.cbStructureSize = Marshal.SizeOf(typeof(NETINFOSTRUCT));
            return ret;
        }
    }

    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
    public struct NETRESOURCE : IEquatable<NETRESOURCE>
    {
        public ResourceScope dwScope;
        public ResourceType dwType;
        public ResourceDisplayType dwDisplayType;
        public ResourceUsage dwUsage;
        /// <summary>
        /// If the dwScope member is equal to RESOURCE_CONNECTED or RESOURCE_REMEMBERED,
        /// this member is a pointer to a null-terminated character string that specifies
        /// the name of a local device. This member is NULL if the connection does not use a device.
        /// </summary>
        [MarshalAs(UnmanagedType.LPTStr)]
        public string lpLocalName;
        /// <summary>
        /// If the entry is a network resource, this member is a pointer to a null-terminated
        /// character string that specifies the remote network name.If the entry is a current
        /// or persistent connection, lpRemoteName member points to the network name associated
        /// with the name pointed to by the lpLocalName member. The string can be MAX_PATH characters
        /// in length, and it must follow the network provider's naming conventions.
        /// </summary>
        [MarshalAs(UnmanagedType.LPTStr)]
        public string lpRemoteName;
        /// <summary>
        /// A pointer to a NULL-terminated string that contains a comment supplied by the network provider.
        /// </summary>
        [MarshalAs(UnmanagedType.LPTStr)]
        public string lpComment;
        /// <summary>
        /// A pointer to a NULL-terminated string that contains the name of the provider
        /// that owns the resource. This member can be NULL if the provider name is unknown.
        /// To retrieve the provider name, you can call the WNetGetProviderName function.
        /// </summary>
        [MarshalAs(UnmanagedType.LPTStr)]
        public string lpProvider;

        //public NETRESOURCE GetParentResource()
        //{
        //    return WinApiWNETwrapper.GetParentResource(this);
        //}

        //public NETRESOURCE[] GetChildResources()
        //{
        //    WNetContainer cont = new WNetContainer(this);
        //    return cont.GetNetresources();
        //}

        public static NETRESOURCE[] FromBuffer(IntPtr buffer, int entries_count)
        {
            int struct_len = Marshal.SizeOf(typeof(NETRESOURCE));
            IntPtr current_ptr = IntPtr.Zero;

            NETRESOURCE[] ret = new NETRESOURCE[entries_count];

            for (int i = 0; i < entries_count; i++)
            {
                current_ptr = new IntPtr(buffer.ToInt64() + (long)struct_len * (long)i);
                ret[i] = (NETRESOURCE)Marshal.PtrToStructure(current_ptr, typeof(NETRESOURCE));
            }
            return ret;
        }

        public static void FromBuffer(IntPtr buffer, int entry_index, ref NETRESOURCE resource)
        {
            int struct_len = Marshal.SizeOf(typeof(NETRESOURCE));
            IntPtr ptr = new IntPtr(buffer.ToInt64() + (long)struct_len * (long)entry_index);
            resource = (NETRESOURCE)Marshal.PtrToStructure(ptr, typeof(NETRESOURCE));
        }

        #region IEquatable<NETRESOURCE> Members

        public bool Equals(NETRESOURCE other)
        {
            return string.Equals(this.lpRemoteName, other.lpRemoteName, StringComparison.CurrentCultureIgnoreCase);
        }

        #endregion
    }

    [Flags()]
    public enum ResourceUsage : uint
    {
        /// <summary>
        /// Use when dwScope is not equal to RESOURCE_GLOBALNET
        /// </summary>
        NONE = 0,
        /// <summary>
        /// The resource is a connectable resource; the name pointed to by the lpRemoteName member
        /// can be passed to the WNetAddConnection function to make a network connection.
        /// </summary>
        CONNECTABLE = WinApiWNET.RESOURCEUSAGE_CONNECTABLE,
        /// <summary>
        /// The resource is a container resource; the name pointed to by the lpRemoteName member
        /// can be passed to the WNetOpenEnum function to enumerate the resources in the container.
        /// </summary>
        CONTAINER = WinApiWNET.RESOURCEUSAGE_CONTAINER,
        /// <summary>
        /// The resource is not a local device.
        /// </summary>
        NOLOCALDEVICE = WinApiWNET.RESOURCEUSAGE_NOLOCALDEVICE,
        /// <summary>
        /// The resource is a sibling. This value is not used by Windows.
        /// </summary>
        SIBLING = WinApiWNET.RESOURCEUSAGE_SIBLING,
        /// <summary>
        /// The resource must be attached. This value specifies that a function
        /// to enumerate resource this should fail if the caller is not authenticated,
        /// even if the network permits enumeration without authentication.
        /// </summary>
        ATTACHED = WinApiWNET.RESOURCEUSAGE_ATTACHED
    }

    public enum ResourceDisplayType : uint
    {
        /// <summary>
        /// The method used to display the object does not matter.
        /// </summary>
        GENERIC = WinApiWNET.RESOURCEDISPLAYTYPE_GENERIC,
        /// <summary>
        /// The object should be displayed as a domain.
        /// </summary>
        DOMAIN = WinApiWNET.RESOURCEDISPLAYTYPE_DOMAIN,
        /// <summary>
        /// The object should be displayed as a server.
        /// </summary>
        SERVER = WinApiWNET.RESOURCEDISPLAYTYPE_SERVER,
        /// <summary>
        /// The object should be displayed as a share.
        /// </summary>
        SHARE = WinApiWNET.RESOURCEDISPLAYTYPE_SHARE,
        /// <summary>
        /// The object should be displayed as a file.
        /// </summary>
        FILE = WinApiWNET.RESOURCEDISPLAYTYPE_FILE,
        /// <summary>
        /// The object should be displayed as a group.
        /// </summary>
        GROUP = WinApiWNET.RESOURCEDISPLAYTYPE_GROUP,
        /// <summary>
        /// The object should be displayed as a network.
        /// </summary>
        NETWORK = WinApiWNET.RESOURCEDISPLAYTYPE_NETWORK,
        /// <summary>
        /// The object should be displayed as a logical root for the entire network.
        /// </summary>
        ROOT = WinApiWNET.RESOURCEDISPLAYTYPE_ROOT,
        /// <summary>
        /// The object should be displayed as a administrative share.
        /// </summary>
        SHAREADMIN = WinApiWNET.RESOURCEDISPLAYTYPE_SHAREADMIN,
        /// <summary>
        /// The object should be displayed as a directory.
        /// </summary>
        DIRECTORY = WinApiWNET.RESOURCEDISPLAYTYPE_DIRECTORY,
        /// <summary>
        /// The object should be displayed as a tree. This display type was used
        /// for a NetWare Directory Service (NDS) tree by the NetWare Workstation service
        /// supported on Windows XP and earlier.
        /// </summary>
        TREE = WinApiWNET.RESOURCEDISPLAYTYPE_TREE,
        /// <summary>
        /// The object should be displayed as a Netware Directory Service container.
        /// This display type was used by the NetWare Workstation service
        /// supported on Windows XP and earlier.
        /// </summary>
        NDSCONTAINER = WinApiWNET.RESOURCEDISPLAYTYPE_NDSCONTAINER
    }

    public enum ResourceType : uint
    {
        /// <summary>
        /// All resources.
        /// </summary>
        ANY = WinApiWNET.RESOURCETYPE_ANY,
        /// <summary>
        /// Disk resources.
        /// </summary>
        DISK = WinApiWNET.RESOURCETYPE_DISK,
        /// <summary>
        /// Print resources.
        /// </summary>
        PRINT = WinApiWNET.RESOURCETYPE_PRINT
    }

    public enum ResourceScope : uint
    {
        /// <summary>
        /// Enumerate currently connected resources. The dwUsage member cannot be specified.
        /// </summary>
        CONNECTED = WinApiWNET.RESOURCE_CONNECTED,
        /// <summary>
        /// Enumerate all resources on the network. The dwUsage member is specified
        /// </summary>
        GLOBALNET = WinApiWNET.RESOURCE_GLOBALNET,
        /// <summary>
        /// Enumerate remembered (persistent) connections. The dwUsage member cannot be specified.
        /// </summary>
        REMEMBERED = WinApiWNET.RESOURCE_REMEMBERED
    }

    [Flags()]
    public enum WNetConnectOptions : uint
    {
        None=0,
        /// <summary>
        /// If this flag is set, the operating system may interact with the user for authentication purposes.
        /// </summary>
        INTERACTIVE = WinApiWNET.CONNECT_INTERACTIVE,
        /// <summary>
        /// This flag instructs the system not to use any default settings for user names
        /// or passwords without offering the user the opportunity to supply an alternative.
        /// This flag is ignored unless CONNECT_INTERACTIVE is also set.
        /// </summary>
        PROMPT = WinApiWNET.CONNECT_PROMPT,
        /// <summary>
        /// This flag forces the redirection of a local device when making the connection. 
        /// If the lpLocalName member of NETRESOURCE specifies a local device to redirect, 
        /// this flag has no effect, because the operating system still attempts to redirect
        /// the specified device. When the operating system automatically chooses a local device,
        /// the dwType member must not be equal to RESOURCETYPE_ANY. If this flag is not set,
        /// a local device is automatically chosen for redirection only if the network requires
        /// a local device to be redirected. Windows Server 2003 and Windows XP:  When the system
        /// automatically assigns network drive letters, letters are assigned beginning with Z:, then Y:,
        /// and ending with C:. This reduces collision between per-logon drive letters
        /// (such as network drive letters) and global drive letters (such as disk drives).
        /// Note that earlier versions of Windows assigned drive letters beginning with C: and ending with Z:.
        /// </summary>
        REDIRECT = WinApiWNET.CONNECT_REDIRECT,
        /// <summary>
        /// The network resource connection should be remembered.If this bit flag is set,
        /// the operating system automatically attempts to restore the connection when the user
        /// logs on. The operating system remembers only successful connections that redirect local
        /// devices. It does not remember connections that are unsuccessful or deviceless connections.
        /// (A deviceless connection occurs when the lpLocalName member is NULL or points to an empty string.)
        /// If this bit flag is clear, the operating system does not automatically restore the connection
        /// at logon.
        /// </summary>
        UPDATE_PROFILE = WinApiWNET.CONNECT_UPDATE_PROFILE,
        /// <summary>
        /// If this flag is set, the operating system prompts the user for authentication using
        /// the command line instead of a graphical user interface (GUI). This flag is ignored
        /// unless CONNECT_INTERACTIVE is also set. Windows 2000/NT and Windows Me/98/95:
        /// This value is not supported.
        /// </summary>
        COMMANDLINE = WinApiWNET.CONNECT_COMMANDLINE,
        /// <summary>
        /// If this flag is set, and the operating system prompts for a credential,
        /// the credential should be saved by the credential manager. If the credential manager
        /// is disabled for the caller's logon session, or if the network provider does not support
        /// saving credentials, this flag is ignored. This flag is also ignored unless you set the
        /// CONNECT_COMMANDLINE flag. Windows 2000/NT and Windows Me/98/95:  This value is not supported.
        /// </summary>
        CMD_SAVECRED = WinApiWNET.CONNECT_CMD_SAVECRED
    }

    public enum NetworkType : ushort
    {
        MSNET = 0x0001,
        LANMAN = 0x0002,
        NETWARE = 0x0003,
        VINES = 0x0004,
        NET10 = 0x0005,
        LOCUS = 0x0006,
        SUN_PC_NFS = 0x0007,
        LANSTEP = 0x0008,
        TITLES_9 = 0x0009,
        LANTASTIC = 0x000A,
        AS400 = 0x000B,
        FTP_NFS = 0x000C,
        PATHWORKS = 0x000D,
        LIFENET = 0x000E,
        POWERLAN = 0x000F,
        BWNFS = 0x0010,
        COGENT = 0x0011,
        FARALLON = 0x0012,
        APPLETALK = 0x0013,
        INTERGRAPH = 0x0014,
        SYMFONET = 0x0015,
        CLEARCASE = 0x0016,
        FRONTIER = 0x0017,
        BMC = 0x0018,
        DCE = 0x0019,
        AVID = 0x001A,
        DOCUSPACE = 0x001B,
        MANGOSOFT = 0x001C,
        SERNET = 0x001D,
        RIVERFRONT1 = 0x001E,
        RIVERFRONT2 = 0x001F,
        DECORB = 0x0020,
        PROTSTOR = 0x0021,
        FJ_REDIR = 0x0022,
        DISTINCT = 0x0023,
        TWINS = 0x0024,
        RDR2SAMPLE = 0x0025,
        CSC = 0x0026,
        IN31 = 0x0027,
        EXTENDNET = 0x0029,
        STAC = 0x002A,
        FOXBAT = 0x002B,
        YAHOO = 0x002C,
        EXIFS = 0x002D,
        DAV = 0x002E,
        KNOWARE = 0x002F,
        OBJECT_DIRE = 0x0030,
        MASFAX = 0x0031,
        HOB_NFS = 0x0032,
        SHIVA = 0x0033,
        IBMAL = 0x0034,
        LOCK = 0x0035,
        TERMSRV = 0x0036,
        SRT = 0x0037,
        QUINCY = 0x0038,
        OPENAFS = 0x0039,
        AVID1 = 0x003A,
        DFS = 0x003B,
        KWNP = 0x003C,
        ZENWORKS = 0x003D,
        DRIVEONWEB = 0x003E,
        VMWARE = 0x003F,
        RSFX = 0x0040,
        MFILES = 0x0041,
        MS_NFS = 0x0042,
        GOOGLE = 0x0043,
        WNNC_CRED_MANAGER = 0xFFFF
    }

    public enum NetworkStatus
    {
        Running = 0, //NO_ERROR
        None = 1222, //ERROR_NO_NETWORK
        Busy = 170 //ERROR_BUSY
    }

    [Flags()]
    public enum ConnectDialogFlags
    {
        RO_PATH = 0x00000001, /* Resource path should be read-only    */
        CONN_POINT = 0x00000002, /* Netware -style movable connection point enabled */
        USE_MRU = 0x00000004, /* Use MRU combobox  */
        HIDE_BOX = 0x00000008, /* Hide persistent connect checkbox  */
        PERSIST = 0x00000010, /* Force persistent connection */
        NOT_PERSIST = 0x00000020, /* Force connection NOT persistent */
    }
}

    

