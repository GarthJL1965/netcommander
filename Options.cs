using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Win32;
using System.Drawing;
using System.Drawing.Text;
using System.IO;
using System.Windows.Forms;
using System.Resources;
using System.Globalization;

namespace netCommander
{
    static class Options
    {
        #region lang resources
        public const string LANG_FILE = "File";
        public const string LANG_FIND = "Find";
        public const string LANG_PANEL = "Panel";
        public const string LANG_SORT = "Sort";
        public const string LANG_BROWSE = "Browse";
        public const string LANG_NETWORK = "Network";
        public const string LANG_JUMP_TO = "JumpTo";
        public const string LANG_FTP_SERVER = "FtpServer";
        public const string LANG_DRIVES = "Drives";
        public const string LANG_CONNECT_TO_FTP_SERVER = "ConnectToFtpServer";
        public const string LANG_LOGIN_TO_FTP_SERVER = "LoginToFtpServer";
        public const string LANG_WHERE_TO_JUMP = "WhereJump";
        public const string LANG_CANNOT_SHOW_0 = "CannotShow_0";
        public const string LANG_EXECUTE = "Execute";
        public const string LANG_0_KBYTE_SEC = "0_KbyteSec";
        public const string LANG_0_BYTES_FROM_1_2P_TRANSFERRED = "0_BytesFrom_1_(2)_Transferred";
        public const string LANG_0_BYTES_TRANSFERRED = "0_BytesTransferred";
        public const string LANG_COPY = "Copy";
        public const string LANG_0_BYTES_FROM_1_FILES_TRANSFERRED = "0_BytesFrom_1_FilesTransferred";
        public const string LANG_0_ARROW_1_2 = "0_Arrow_1_2";
        public const string LANG_WRONG_DESTINATION_0_IS_EXISTING_FILE = "WrongDestination_0_IsExistingFile";
        public const string LANG_CANNOT_CREATE_SOFT_LINK_0 = "CannotCreateSoftLink_0";
        public const string LANG_CANNOT_COPY_0_ARROW_1 = "ExceptionWhileTryingCopy_0_Arrow_1";
        public const string LANG_CANNOT_SET_SEC_ATTRIBUTES_0 = "FailedSetSecuritySttributesFile_0";
        public const string LANG_DESTINATION_0_EXISTS_AND_HIDDEN_CLEARING_ATTRS_PROHIBITED = "DestinationFile_0_ExistsAndHaveHiddenAttributeClearingAttributesIsProhibited";
        public const string LANG_DESTINATION_0_EXISTS_AND_READONLY_CLEARING_ATTRS_PROHIBITED = "DestinationFile_0_ExistsAndHaveReadonlyAttributeClearingAttributesIsProhibited";
        public const string LANG_CANNOT_CLEAR_HIDDEN_READONLY_ATTR_DESTINATION_0 = "CannotClearHiddenOrReadonlyAttributesOfDestinationFile_0";
        public const string LANG_CANNOT_CREATE_DIRECTORY_0 = "CannotCreateDirectory_0";
        public const string LANG_DESTINATION_0_NEWER_THEN_SOURCE_1_OVERWRITING_PROHIBITED = "Destination_0_NewerThenSource_1_OverwritingProhibited";
        public const string LANG_DESTINATION_0_EXIST_OVERWRITING_PROHIBITED = "Destination_0_ExistsOverwritingProhibited";
        public const string LANG_0_BYTES_FROM_1_2P_DOWNLOADED = "0_BytesFrom_1_2P_Downloaded";
        public const string LANG_0_BYTES_DOWNLOADED = "0_BytesDownloaded";
        public const string LANG_DOWNLOAD = "Download";
        public const string LANG_0_BYTES_FROM_1_FILES_DOWNLOADED = "0_BytesFrom_1_FileDownloaded";
        public const string LANG_WRONG_DESTINATION_SOURCE_0_DIRECTORY_DESTINATION_1_FILE = "WrongDestinationSource_0_IsDirectoryButDestination_1_IsExistingFile";
        public const string LANG_WRONG_DESTINATION_CANNOT_DOWNLOAD_MANY_ENTRIES_INTO_ONE_FILE = "WrongDestinationCannotDownloadMoreThenOneEntryIntoExistingFile";
        public const string LANG_CANNOT_READ_DIRECTORY_CONTENTS_0 = "FailedReadDirectoryList_0";
        public const string LANG_CANNOT_DOWNLOAD_0 = "FailedDownload_0";
        public const string LANG_CANNOT_READ_FTP_TRANSFER_SETTINGS = "CannotDeserializeFtpTransferSettings";
        public const string LANG_0_BYTES_FROM_1_2P_UPLOADED = "0_BytesFrom_1_2P_Uploaded";
        public const string LANG_0_BYTES_UPLOADED = "0_BytesUploaded";
        public const string LANG_UPLOAD = "Upload";
        public const string LANG_0_BYTES_FROM_1_FILES_UPLOADED = "0_BytesFrom_1_FileUploaded";
        public const string LANG_SOURCE_FILE_0_NOT_FOUND = "SourceFile_0_NotFound";
        public const string LANG_WRONG_DESTINATION_CANNOT_UPLOAD_MANY_ENTRIES_INTO_ONE_FILE = "WrongDestinationCannotUploadMoreThenOneEntryIntoExistingFile";
        public const string LANG_CANNOT_OPEN_SOURCE_FILE_0 = "CannotOpenSourceFile_0";
        public const string LANG_CANNOT_UPLOAD_0_ARROW_1 = "FailedToUpload_0_Arrow_1";
        public const string LANG_EXCEPTION = "Exception";
        public const string LANG_SOURCE = "Source";
        public const string LANG_MESSAGE = "Message";
        public const string LANG_STACK = "Stack";
        public const string LANG_0_1_SAVE_TO_LOG = "0_1_SaveToLog";
        public const string LANG_0_SAVE_TO_LOG = "0_SaveToLog";
        public const string LANG_SAVE_TO_LOG = "SaveToLog";
        public const string LANG_0_CONTINUE = "0_Continue";
        public const string LANG_0_1_CONTINUE = "0_1_Continue";
        public const string LANG_APP_NAME = "ApplicationName";
        public const string LANG_MOVE_RENAME = "MoveRename";
        public const string LANG_MOVE_RENAME_0_ARROW_1 = "MoveRename_0_Arrow_1";
        public const string LANG_WRONG_DESTINATION_CANNOT_MOVE_MANY_ENTRIES_INTO_ONE_FILE_0 = "WrongDestinationCannotMoveMoreThenOneObjectIntoExistingFile_0";
        public const string LANG_CANNOT_MOVE_0_ARROW_1 = "ErrorWhileMoveRename_0_Arrow_1";
        public const string LANG_CANNOT_EXECUTE_0 = "FailedToExecute_0";
        public const string LANG_CANNOT_PARSE_COMMAND_LINE = "FailedToParseCommandLine";
        public const string LANG_MASK = "Mask";
        public const string LANG_DESTINATION = "Destination";
        public const string LANG_ALLOW_DECRIPT = "AllowDecript";
        public const string LANG_SUPRESS_ERRORS = "SupressErrors";
        public const string LANG_PROCESS_RECURSIVELY = "ProcessRecursively";
        public const string LANG_SHOW_TOTAL_PROGRESS="ShowTotalProgress";
        public const string LANG_ALLOW_CLEAR_ATTRIBUTES = "AllowClearAttributes";
        public const string LANG_ALLOW_CLEAR_ATTRIBUTES_TOOLTIP = "AllowClearAttributesTooltip";
        public const string LANG_PROCESS_SYMLINK_AS_SYMLINK = "ProcessSymlinkAsSymlink";
        public const string LANG_PROCESS_SYMLINK_AS_SYMLINK_TOOLTIP = "ProcessSymlinkAsSymlinkTooltip";
        public const string LANG_COPY_SEC_ATTRIBUTES = "CopySecAttributes";
        public const string LANG_COPY_SEC_ATTRIBUTES_TOOLTIP = "CopySecAttributesTooltip";
        public const string LANG_PROCESS_EMPTY_DIRS = "ProcessEmptyDirs";
        public const string LANG_PROCESS_EMPTY_DIRS_TOOLTIP = "ProcessEmptyDirsTooltip";
        public const string LANG_NOT_OVERWRITE = "NotOverwrite";
        public const string LANG_OVERWRITE_ONLY_IF_SOURCE_NEWER = "OverwriteIfSourceNewer";
        public const string LANG_OVERWITE_ALWAYS = "OverwriteAlways";
        public const string LANG_OK = "OK";
        public const string LANG_CANCEL = "Cancel";
        public const string LANG_OVERWITE_EXISTING_FILES = "OverwriteExistingFiles";
        public const string LANG_MASK_TOOLTIP = "MaskTooltip";
        public const string LANG_ALLOW_DECRYPT_TOOLTIP = "AllowDecriptTooltip";
        public const string LANG_CLOSE_ON_FINISH = "CloseOnFinish";
        public const string LANG_USE_AS_TEMPLATE = "UseAsTemplate";
        public const string LANG_LINK_NAME = "LinkName";
        public const string LANG_LINK_TARGET = "LinkTarget";
        public const string LANG_LINK_TYPE = "LinkType";
        public const string LANG_LINK_HARD = "LinkHard";
        public const string LANG_LINK_MOUNTPOINT = "LinkMountpoint";
        public const string LANG_LINK_SYMBOLIC = "LinkSymbolic";
        public const string LANG_LINK_NAME_TOOLTIP = "LinkNameTooltip";
        public const string LANG_LINK_HARD_TOOLTIP = "LinkHardTooltip";
        public const string LANG_LINK_MOUNTPOINT_TOOLTIP = "LinkMountpointTooltip";
        public const string LANG_LINK_SYMBOLIC_TOOLTIP = "LinkSymbolicTooltip";
        public const string LANG_USER_NAME = "UserName";
        public const string LANG_PASSWORD = "Password";
        public const string LANG_DELETE_READONLY = "DeleteReadonly";
        public const string LANG_FILE_NAME = "FileName";
        public const string LANG_LOCATION = "Location";
        public const string LANG_CURRENT_DIRECTORY = "CurrentDirectory";
        public const string LANG_CURRENT_DRIVE = "CurrentDrive";
        public const string LANG_LOCAL_DRIVES = "LocalDrives";
        public const string LANG_FIXED_DRIVES = "FixedDrives";
        public const string LANG_REMOVABLE_DRIVES = "RemovableDrives";
        public const string LANG_NETWORK_DRIVES = "NetworkDrives";
        public const string LANG_SAVE_DEFAULTS = "SaveDefaults";
        public const string LANG_FILE_ATTRIBUTES = "FaileAttributes";
        public const string LANG_IGNORE = "Ignore";
        public const string LANG_FILE_SIZE = "FileSize";
        public const string LANG_AND = "And";
        public const string LANG_DATETIME = "DateTime";
        public const string LANG_BETWEEN = "Between";
        public const string LANG_CREATE_TIME = "CreateTime";
        public const string LANG_MODIFICATION_TIME = "ModificationTime";
        public const string LANG_ACCESS_TIME = "AccessTime";
        public const string LANG_FIND_RESULTS = "FindResults";
        public const string LANG_ABORT = "Abort";
        public const string LANG_GOTO = "GoTo";
        public const string LANG_CLOSE = "Close";
        public const string LANG_DONE = "Done";
        public const string LANG_FOUND_0_FILES = "Found_0_Files";
        public const string LANG_FILE_ATTRIBUTE_ARCHIVE = "FileAttributeArchive";
        public const string LANG_FILE_ATTRIBUTE_ARCHIVE_TOOLTIP = "FileAttributeArchiveTooltip";
        public const string LANG_FILE_ATTRIBUTE_COMPRESSED = "FileAttributeCompressed";
        public const string LANG_FILE_ATTRIBUTE_COMPRESSED_TOOLTIP = "FileAttributeCompressedTooltip";
        public const string LANG_FILE_ATTRIBUTE_DIRECTORY = "FileAttributeDirectory";
        public const string LANG_FILE_ATTRIBUTE_DIRECTORY_TOOLTIP = "FileAttributeDirectoryTooltip";
        public const string LANG_FILE_ATTRIBUTE_ENCRYPTED = "FileAttributeEncrypted";
        public const string LANG_FILE_ATTRIBUTE_ENCRYPTED_TOOLTIP = "FileAttributeEncryptedTooltip";
        public const string LANG_FILE_ATTRIBUTE_HIDDEN = "FileAttributeHidden";
        public const string LANG_FILE_ATTRIBUTE_HIDDEN_TOOLTIP = "FileAttributeHiddenTooltip";
        public const string LANG_FILE_ATTRIBUTE_NORMAL = "FileAttributeNormal";
        public const string LANG_FILE_ATTRIBUTE_NORMAL_TOOLTIP = "FileAttributeNormalTooltip";
        public const string LANG_FILE_ATTRIBUTE_NOT_CONTENT_INDEXED = "FileAttributeNotContentIndexed";
        public const string LANG_FILE_ATTRIBUTE_NOT_CONTENT_INDEXED_TOOLTIP = "FileAttributeNotContentIndexedTooltip";
        public const string LANG_FILE_ATTRIBUTE_OFFLINE = "FileAttributeOffline";
        public const string LANG_FILE_ATTRIBUTE_OFFLINE_TOOLTIP = "FileAttributeOfflineTooltip";
        public const string LANG_FILE_ATTRIBUTE_READONLY = "FileAttributeReadonly";
        public const string LANG_FILE_ATTRIBUTE_READONLY_TOOLTIP = "FileAttributeReadonlyTooltip";
        public const string LANG_FILE_ATTRIBUTE_REPARSE_POINT = "FileAttributeReparsePoint";
        public const string LANG_FILE_ATTRIBUTE_REPARSE_POINT_TOOLTIP = "FileAttributeReparsePointTooltip";
        public const string LANG_FILE_ATTRIBUTE_SPARSE = "FileAttributeSparse";
        public const string LANG_FILE_ATTRIBUTE_SPARSE_TOOLTIP = "FileAttributeSparseTooltip";
        public const string LANG_FILE_ATTRIBUTE_SYSTEM = "FileAttributeSystem";
        public const string LANG_FILE_ATTRIBUTE_SYSTEM_TOOLTIP = "FileAttributeSystemTooltip";
        public const string LANG_FILE_ATTRIBUTE_TEMPORARY = "FileAttributeTemporary";
        public const string LANG_FILE_ATTRIBUTE_TEMPORARY_TOOLTIP = "FileAttributeTemporaryTooltip";
        public const string LANG_CLEAR = "Clear";
        public const string LANG_STANDARS_INFORMATION = "StandardInformation";
        public const string LANG_FILE_ALTERNATE_NAME = "FileAltName";
        public const string LANG_FILE_ALLOCATION_SIZE = "FileAllocationSize";
        public const string LANG_SUBDIRS_SIZE_ALLOCATION = "SubdirsSizeAllocation";
        public const string LANG_SUBDIRS_FILES_DIRS_COUNT = "SubdirsFilesDirsCount";
        public const string LANG_CHANGE_TIME = "ChangeTime";
        public const string LANG_FILE_STREAMS = "FileStreams";
        public const string LANG_LINKS = "Links";
        public const string LANG_REPARSE_TAG = "ReparseTag";
        public const string LANG_LINKS_HARD_COUNT = "HardLinksCount";
        public const string LANG_FILE_SECURITY_ATTRIBUTES = "SecutiryAttributes";
        public const string LANG_FILE_VOLUME_INFORMATION = "VolumeInformation";
        public const string LANG_VOLUME_LABEL = "VolumeLabel";
        public const string LANG_FILESYSTEM_TYPE = "VolumeFStype";
        public const string LANG_FILESYATEM_CAPS = "VolumeFScaps";
        public const string LANG_FILESYSTEM_NAME_LEN = "VolumeFSnameLen";
        public const string LANG_VOLUME_QUOTA_LIMIT = "VolumeQuotaLimit";
        public const string LANG_VOLUME_QUOTA_THRESHOLD = "VolumeQuotaThreshold";
        public const string LANG_VOLUME_CONTROL_FLAGS = "VolumeControlFlags";
        public const string LANG_VOLUME_AVAILABLE_UNITS = "VolumeAvalableUnits";
        public const string LANG_BYTES = "Bytes";
        public const string LANG_VOLUME_BYTES_PER_UNIT = "VolumeBytesPerUnit";
        public const string LANG_VOLUME_PER_SECTOR = "VolumePerSector";
        public const string LANG_VOLUME_SERIAL_NUMBER = "VolumeSerialNumber";
        public const string LANG_VOLUME_DEVICE_TYPE = "VolumeDeviceType";
        public const string LANG_VOLUME_DEVICE_CHARACTERISTICS = "VolumeDeviceCharacteristics";
        public const string LANG_0_BYTES = "0_Bytes";
        public const string LANG_DATE_TIME_LONG_FORMAT = "DateTimeLongFormat";
        public const string LANG_CANNOT_READ_ATTRIBUTES_0 = "CannotReadAttributes_0";
        public const string LANG_FTP_SERVER_HOST_NAME = "ServerHostName";
        public const string LANG_FTP_TCP_PORT = "TcpPort";
        public const string LANG_FTP_TCP_PORT_TOOLTIP = "TcpPortTooltip";
        public const string LANG_FTP_TIMEOUT = "Timeout";
        public const string LANG_FTP_TIMEOUT_TOOLTIP = "TimeoutTooltip";
        public const string LANG_FTP_PASSIVE_MODE = "PassiveMode";
        public const string LANG_FTP_PASSIVE_MODE_TOOLTIP = "PassiveModeTooltip";
        public const string LANG_FTP_ENABLE_PROXY = "EnableProxy";
        public const string LANG_FTP_ENABLE_PROXY_TOOLTIP = "EnableProxyTooltip";
        public const string LANG_FTP_KEEP_ALIVE = "KeepAlive";
        public const string LANG_FTP_KEEP_ALIVE_TOOLTIP = "KeepAliveTooltip";
        public const string LANG_FTP_ANONYMOUS_LOGON = "AnonymousLogon";
        public const string LANG_FTP_ANONYMOUS_LOGON_TOOLTIP = "AnonymousLogonTooltip";
        public const string LANG_FTP_ENABLE_SSL = "EnableSSL";
        public const string LANG_FTP_SERVER_HOST_NAME_TOOLTIP = "ServerHostNameTooltip";
        public const string LANG_BUFFER_SIZE = "BufferSize";
        public const string LANG_MOVE_SEC_ATTRIBUTES_WHILE_CROSS_VOLUMES = "MoveSecurityAttributes";
        public const string LANG_WORKING_DIRECTORY = "WorkingDirectory";
        public const string LANG_ARGUMENTS = "Arguments";
        public const string LANG_VERB = "Verb";
        public const string LANG_USE_SHELLEXECUTE = "UseShellexecute";
        public const string LANG_RUN_AS_ANOTHER_USER = "RunAsAnotherUser";
        public const string LANG_RUN_IN_NEW_CONSOLE_WINDOW = "RunInNewConsole";
        public const string LANG_LOAD_PROFILE = "LoadProfile";
        public const string LANG_COPY_TO_FILE = "CopyToFile";
        public const string LANG_DELETE = "Delete";
        public const string LANG_DELETE_PROMPT = "DeletePromt";
        public const string LANG_CANNOT_DELETE_DEFAULT_DATA_STREAM = "CannotDeleteDefaultDataStream";
        public const string LANG_CANNOT_DELETE_0 = "CannotDelete0";
        public const string LANG_EXTENSION = "Extension";
        public const string LANG_CANNOT_RUN_0 = "CannotRun_0";
        public const string LANG_UP = "Up";
        public const string LANG_DIRECTORY = "Directory";
        public const string LANG_DIRECTORIES = "Directories";
        public const string LANG_FILES = "Files";
        public const string LANG_SELECTED = "Selected";
        public const string LANG_FTP_WAIT_RETRIEVE_DIRECTORY_LIST = "FtpWaitRetrieveDirectoryList";
        public const string LANG_ENTRIES = "Entries";
        public const string LANG_NETWORK_LOGIN = "NetworkLogin";
        public const string LANG_NETWORK_WAIT_WHILE_SCANNING = "NetworkWaitWhileScanning";
        public const string LANG_DIRECTORY_CREATE = "DirectoryCreate";
        public const string LANG_DIRECTORY_EXISTS = "DirectoryExists";
        public const string LANG_WRONG_DESTINATION = "WrongDestination";
        public const string LANG_DELETE_NOW_0 = "Delete_0";
        public const string LANG_ADD_TO_COMMAND_LINE_WITH_PATH = "AddToCommandlineWithPath";
        public const string LANG_ADD_TO_COMMAND_LINE = "AddToCommandline";
        public const string LANG_LINK_CREATE = "LinkCreate";
        public const string LANG_CANNOT_CREATE_HARD_LINK_DIR_0 = "CannotCreateHardlinkDirectory_0";
        public const string LANG_CANNOT_CREATE_LINK_0_ARROW_1 = "CannotCreateLink_0_1";
        public const string LANG_ADD = "Add";
        public const string LANG_FILE_0_NOT_FOUND = "File_0_NotFound";
        public const string LANG_PROPERTIES = "Properties";
        public const string LANG_QUERY_PROPERTIES = "QueryProperties";
        public const string LANG_DIRECTORY_CREATE_0 = "DirectoryCreate_0";
        public const string LANG_ACCOUNT = "AccountInfo";
        public const string LANG_CANNOT_EXCUTE_0_1 = "CannotExecute_0_1";
        public const string LANG_TOUCH = "Touch";
        public const string LANG_OPTIONS = "Options";
        public const string LANG_PANEL_FONT = "PanelFont";
        public const string LANG_TOTAL_VOLUME_SPACE = "VolumeSpace";
        public const string LANG_TOTAL_VOLUME_AVAILABLE = "VolumeAvailable";
        public const string LANG_VOLUME_SPACE_INFO = "VolumeSpaceInfo";
        public const string LANG_MENU_TEXT = "MenuText";
        public const string LANG_COMMAND_TEXT = "CommandText";
        public const string LANG_COMMAND_TEXT_HELP = "CommandTextHelp";
        public const string LANG_EDIT = "Edit";
        public const string LANG_USER_MENU = "UserMenu";
        public const string LANG_PROCESS_NO_WINDOW = "NoWindow";
        public const string LANG_FILE_VIEW = "FileView";
        public const string LANG_FILE_EDIT = "FileEdit";
        public const string LANG_FILE_SAVE = "FileSave";
        public const string LANG_FILE_SAVE_AS = "FileSaveAs";
        public const string LANG_HAVE_UNSAVED_CHANGES = "FileHaveUnsavedChanges";
        public const string LANG_CANNOT_CHANGE_ENCODING_HAVE_CHANGES = "CannotChangeEncodingHaveChanges";
        public const string LANG_REFRESH = "Refresh";
        public const string LANG_COPY_AS_ANSI = "CopyAnsi";
        public const string LANG_COPY_AS_UTF16 = "CopyAsUTF16";
        public const string LANG_TEXT_ENCODING = "TextEncoding";
        public const string LANG_TEXT_ENCODING_ANSI = "TextEncodingAnsi";
        public const string LANG_TEXT_ENCODING_ASCII = "TextEncodingASCII";
        public const string LANG_TEXT_ENCODING_UTF8 = "TextEncodingUtf8";
        public const string LANG_TEXT_ENCODING_UTF16 = "TextEncodingUtf16";
        public const string LANG_TEXT_ENCODING_OTHER = "TextEncodingOther";
        public const string LANG_TEXT_FIND = "TextFind";
        public const string LANG_TEXT_FIND_NEXT = "TextFindNext";
        public const string LANG_EXTRACT = "Extract";
        public const string LANG_EXTRACT_ATTRIBUTES = "ExtractAttributes";
        public const string LANG_PRESERVE_ATTRIBUTES = "PreserveAttributes";
        public const string LANG_OVERWRITE_EXISTING_ENTRIES = "OverwriteExistingEntries";
        public const string LANG_ARCHIVE_ADD = "ArchiveAdd";
        public const string LANG_CREATING_FILE_LIST_TO_ARCHIVE = "CreatingFileListToArchive";
        public const string LANG_WRONG_SOURCE = "WrongSource";
        public const string LANG_ARCHIVING_0_1 = "Archiving_0_1";
        public const string LANG_FAILED_ARCHIVE_0 = "FailedArchive_0";
        public const string LANG_COMMIT_ARCHIVE_UPDATES = "CommitArchiveUpdates";
        public const string LANG_ARCHIVE_PASS_NEEDED = "ArchivePassNeeded";
        public const string LANG_PLUGIN_MENU_HEADER = "PluginMenuTitle";
        public const string LANG_FAILED_TO_LOAD_PLUGIN_FROM_0 = "FailedToLoadPluginFrom_0";
        public const string LANG_PLUGIN_0_ERROR = "Plugin_0_Error";
        public const string LANG_SPECIAL_FOLDERS = "SpecialFolders";
        public const string LANG_HELP = "Help";
        public const string LANG_ABOUT = "AboutApp";
        public const string LANG_SV_TYPE_WORKSTATION = "TypeWorkstation";
        public const string LANG_SV_TYPE_SERVER = "TypeServer";
        public const string LANG_SV_TYPE_SQLSERVER = "TypeSqlServer";
        public const string LANG_SV_TYPE_DOMAIN_CTRL = "TypeDomainCtrl";
        public const string LANG_SV_TYPE_DOMAIN_BAKCTRL = "TypeDomainBackCtrl";
        public const string LANG_SV_TYPE_TIME_SOURCE = "TypeTimeSource";
        public const string LANG_SV_TYPE_AFP = "TypeAFP";
        public const string LANG_SV_TYPE_NOVELL = "TypeNovell";
        public const string LANG_SV_TYPE_DOMAIN_MEMBER = "TypeDomainMember";
        public const string LANG_SV_TYPE_LOCAL_LIST_ONLY = "TypeLocalListOnly";
        public const string LANG_SV_TYPE_PRINTQ_SERVER = "TypePrintQServer";
        public const string LANG_SV_TYPE_DIALIN_SERVER = "TypeDialinServer";
        public const string LANG_SV_TYPE_XENIX_SERVER = "TypeXenix";
        public const string LANG_SV_TYPE_SERVER_MFPN = "TypeServerMfpn";
        public const string LANG_SV_TYPE_NT = "TypeNT";
        public const string LANG_SV_TYPE_WFW = "TypeWFW";
        public const string LANG_SV_TYPE_SERVER_NT = "TypeServerNT";
        public const string LANG_SV_TYPE_POTENTIAL_BROWSER = "TypePotentialBrowser";
        public const string LANG_SV_TYPE_BACKUP_BROWSER = "TypeBackupBrowser";
        public const string LANG_SV_TYPE_MASTER_BROWSER = "TypeMasterBrowser";
        public const string LANG_SV_TYPE_DOMAIN_MASTER = "TypeDomainMaster";
        public const string LANG_SV_TYPE_DOMAIN_ENUM = "TypeDomainEnum";
        public const string LANG_SV_TYPE_WINDOWS = "TypeWindows";
        public const string LANG_SV_TYPE_TERMINALSERVER = "TypeTerminalserver";
        public const string LANG_SV_TYPE_CLUSTER_NT = "TypeClusterNT";
        public const string LANG_SV_TYPE_CLUSTER_VS_NT = "TypeClusterVsNT";
        public const string LANG_SV_TYPE_DFS = "TypeDFS";
        public const string LANG_SV_TYPE_DCE = "TypeDCE";
        public const string LANG_SV_TYPE_ALTERNATE_XPORT = "TypeAlternateXPort";
        public const string LANG_SV_TYPE_OSF = "TypeOSF";
        public const string LANG_SV_TYPE_VMS = "TypeVMS";
        public const string LANG_RESOURCE_INFO = "ResourceInfo";
        public const string LANG_RESOURCE_REMOTE_NAME = "ResourceRemoteName";
        public const string LANG_RESOURCE_DISPLAY_TYPE = "ResourceDisplayType";
        public const string LANG_RESOURCE_TYPE = "ResourceType";
        public const string LANG_RESOURCE_COMMENT = "ResourceComment";
        public const string LANG_NETWORK_PROVIDER = "NetworkProvider";
        public const string LANG_NETWORK_PROVIDER_TYPE = "NetworkProviderType";
        public const string LANG_VERSION = "Version";
        public const string LANG_SERVER_INFO = "ServerInfo";
        public const string LANG_SERVER_PLATFORM_ID = "ServerPlatformId";
        public const string LANG_SERVER_SOFTWARE_TYPE = "ServerSoftwareType";
        public const string LANG_SERVER_MAX_USERS = "ServerMaxUsers";
        public const string LANG_SERVER_DISC_TIME = "ServerDisconnectTime";
        public const string LANG_SERVER_ANNOUNCE_TIME = "ServerAnnounceTime";
        public const string LANG_SERVER_USERS_PER_LICENSE = "ServerUsersPerLicense";
        public const string LANG_SERVER_USER_PATH = "ServerUserPath";
        public const string LANG_SERVER_FEATURES = "ServerFeatures";
        public const string LANG_SERVER_DATE_TIME = "ServerDateTime";
        public const string LANG_SERVER_UPTIME_AFTER_RESET = "ServerUptimeAfterReset";
        public const string LANG_TRANSPORT_INFO = "TransportInfo";
        public const string LANG_TRANSPORT_ADDRESS = "TransportAddress";
        public const string LANG_TRANSPORT_NETWORK_ADDRESS = "TransportNetworkAddress";
        public const string LANG_DOMAIN = "Domain";
        public const string LANG_NUMBER_OF_CLIENTS = "NumberOfCLients";
        public const string LANG_SESSION_INFO = "SessionInfo";
        public const string LANG_COMPUTER_NAME = "ComputerName";
        public const string LANG_NUMBER_OF_OPEN_FILES = "NumberOfOpenFiles";
        public const string LANG_ACTIVE_TIME = "ActiveTime";
        public const string LANG_IDLE_TIME = "IdleTime";
        public const string LANG_SESSION_ESTABLISH_TYPE = "SessionEstablishType";
        public const string LANG_SESSION_CLIENT_TYPE = "SessionCLientType";
        public const string LANG_SESSION_TRANSPORT_NAME = "SessionTransportName";
        public const string LANG_NETWROK_OPEN_FILES_INFO = "NetworkOpenFilesInfo";
        public const string LANG_NETWORK_FILE_ID = "NetworkFileId";
        public const string LANG_PERMISSIONS = "Permissions";
        public const string LANG_LOCKS = "Locks";
        public const string LANG_PATH = "Path";
        public const string LANG_NETWORK_PROVIDER_NAME = "NetworkProviderName";
        public const string LANG_NAME = "Name";
        public const string LANG_TYPE = "Type";
        public const string LANG_MAX_CONNECTIONS = "MaxConnections";
        public const string LANG_CURRENT_CONNECTIONS = "CurrentConnections";
        public const string LANG_SHARE_PATH = "SharePath";
        public const string LANG_SHARE_INFO = "ShareInfo";
        public const string LANG_PROCESS_INFO = "Process";
        public const string LANG_PROCESS_NAME = "ProcessName";
        public const string LANG_PROCESS_ID = "ProcessId";
        public const string LANG_PRIORITY = "Prioprity";
        public const string LANG_PRIORITY_BOOST = "PriorityBoost";
        public const string LANG_PROCESS_RESPONDING = "ProcessResponding";
        public const string LANG_PROCESS_MAIN_MODULE = "ProcessMainModule";
        public const string LANG_PROCESS_WINDOW_TITLE = "ProcessWindowTitle";
        public const string LANG_START_TIME = "StartTime";
        public const string LANG_PROCESS_MEM_AND_CPU_USAGE = "ProcessMemAndCpuUsage";
        public const string LANG_NONPAGED_SYSTEM_MEMORY = "NonpagedSystemMemory";
        public const string LANG_PAGED_MEMORY = "PagedMemory";
        public const string LANG_PAGED_SYSTEM_MEMORY = "PagedSystemMemory";
        public const string LANG_PRIVATE_MEMORY = "PrivateMemory";
        public const string LANG_VIRTUAL_MEMORY = "VirtualMemory";
        public const string LANG_WORKING_SET = "WorkingSet";
        public const string LANG_USER_CPU_TIME = "UserCpuTime";
        public const string LANG_PRIVILEGED_CPU_TIME = "PrivilegedCpuTime";
        public const string LANG_TOTAL_CPU_TIME = "TotalCpuTime";
        public const string LANG_PROCESS_LOADED_MODULES_INFO = "ProcessLoadedModules";
        public const string LANG_MODULE_COMPANY_NAME = "ModuleCompanyName";
        public const string LANG_MODULE_PRODUCT_NAME = "ModuleProductName";
        public const string LANG_MEMORY = "Memory";
        public const string LANG_PROCESS_THREADS_INFO = "ProcessThreads";
        public const string LANG_THREAD_ID = "ThreadId";
        public const string LANG_STATE = "State";
        public const string LANG_CPU_LOAD = "CpuLoad";

        public const string LANG_TERMINATE = "Terminate";
        public const string LANG_TERMINATE_PROCESS_ID_NAME="TerminateProcessIdName";
        public const string LANG_TERMINATE_NO_MAIN_WINDOW = "NoMainWindow";
        public const string LANG_TERMINATE_FAIL = "TerminateFail";
        public const string LANG_TERMINATE_SEND_CLOSE_WINDOW = "TerminateSendCloseWindow";
        public const string LANG_TERMINATE_KILL = "TerminateKill";
        public const string LANG_TERMINATE_DEBUG_MODE = "TerminateWithDebugPrivilegs";

        public const string LANG_PROCESSES = "Processes";
        
        public const string LANG_SELECT = "Select";

        private static ResourceManager rm = null;
        public static ResourceManager LangManager
        {
            get
            {
                if (rm == null)
                {
                    rm = ResourceManager.CreateFileBasedResourceManager
                        ("lang",
                        Path.Combine(Path.GetDirectoryName(Application.ExecutablePath), "lang"),
                        null);
                    
                }
                return rm;
            }
        }
        public static string GetLiteral(string key)
        {
            return LangManager.GetString(key, CultureInfo);
        }
        #endregion

        #region color scheme
        private static ColorScheme color_scheme = null;
        public static ColorScheme ColorScheme
        {
            get
            {
                if (color_scheme == null)
                {
                    color_scheme = ColorScheme.Parse(Path.Combine(Path.GetDirectoryName(Application.ExecutablePath), "colors.cfg"));
                }
                return color_scheme;
            }
        }
        public static ItemColors GetItemColors(string name, netCommander.winControls.ItemState state, ItemCategory category)
        {
            return ColorScheme.GetColors(name, state, category);
        }
        #endregion

        private const string REG_PATH = @"Software\maxxSoft\netCommander2";
        private const string KEY_FONT_FILE_PANEL = @"PanelFont";
        private const string KEY_PREFIX_WINDOW_STATE = @"State_";

        private const string KEY_FILTER_DIRECTORY = @"FilterDirectory";

        private const string KEY_COPY_CLOSE_PROGRESS = @"CopyCloseProgress";
        private const string KEY_COPY_ENGINE_OPTIONS = @"CopyEngineOpts";
        private const string KEY_MOVE_ENGINE_OPTIONS = @"MoveEngineOpts";
        private const string KEY_FTP_SETTINGS = @"FtpClient";
        private const string KEY_FTP_DOWNLOAD_SETTINGS = @"FtpDownloadOpts";
        private const string KEY_FTP_UPLOAD_SETTINGS = @"FtpUploadOpts";

        private const string KEY_DEFAULT_LINK_TYPE = @"LinkType";
        private const string KEY_DELETE_OPTIONS = @"DeleteOptions";
        private const string KEY_RUNEXE_OPTIONS = @"RunOptions";

        private const string KEY_LAST_DIRECTORY = @"LastDirectory";
        private const string KEY_PROGRESS_UPDATE_INTERVAL = @"ProgressUpdateInterval";

        private const string KEY_USER_COMMANDS = @"UserCommands";
        private const string KEY_CODE_PAGE = @"CodePage";
        private const string KEY_VIEW_FONT = @"FileViewFont";
        private const string KEY_RENDER_MODE = @"RenderMode";

        private const string KEY_ARCHIVE_EXTRACT_OPTIONS = @"ExtractOptions";
        private const string KEY_ARCHIVE_STREAM_BUFFER_SIZE = @"ArchiveStreamBufferSize";
        private const string KEY_ARCHIVE_ADD_OPTIONS = @"ArchiveAddOptions";
        private const string KEY_CULTURE_CODE = @"CultureCode";
        private const string KEY_PROCESS_TERMINATE_OPTIONS = @"TerminateOptions";

        private static RegistryKey GetRegKey()
        {
            return Registry.CurrentUser.CreateSubKey(REG_PATH);
        }

        public const string REGEX_PARSE_COMMAND = @"""(?:(?<="")([^""]+)""\s*)|\s*([^""\s]+)";

        private static CultureInfo culture_info = null;
        public static CultureInfo CultureInfo
        {
            get
            {
                if (culture_info != null)
                {
                    return culture_info;
                }

                string culture_code = string.Empty;
                using (RegistryKey reg = GetRegKey())
                {
                    culture_code = (string)reg.GetValue(KEY_CULTURE_CODE, string.Empty);
                    reg.Close();
                }
                if (String.IsNullOrEmpty(culture_code))
                {
                    culture_info = CultureInfo.CurrentCulture;
                }
                else
                {
                    try
                    {
                        culture_info = new CultureInfo(culture_code, true);
                    }
                    catch
                    {
                        culture_info = CultureInfo.CurrentCulture;
                    }
                }
                return culture_info;
            }
        }

        public static int ArchiveStreamBufferSize
        {
            get
            {
                int ret=0x8000;
                using (RegistryKey reg = GetRegKey())
                {
                    ret=(int)reg.GetValue(KEY_ARCHIVE_STREAM_BUFFER_SIZE,ret);
                    reg.Close();
                }
                return ret;
            }
            set
            {
                using (RegistryKey reg = GetRegKey())
                {
                    reg.SetValue(KEY_ARCHIVE_STREAM_BUFFER_SIZE,value,RegistryValueKind.DWord);
                    reg.Close();
                }
            }
        }

        public static ToolStripRenderMode MenuRenderMode
        {
            get
            {
                ToolStripRenderMode ret = ToolStripRenderMode.ManagerRenderMode;
                using (RegistryKey reg = GetRegKey())
                {
                    ret = (ToolStripRenderMode)((int)reg.GetValue(KEY_RENDER_MODE, (int)ret));
                    reg.Close();
                }
                return ret;
            }
            set
            {
                using (RegistryKey reg = GetRegKey())
                {
                    reg.SetValue(KEY_RENDER_MODE, (int)value, RegistryValueKind.DWord);
                    reg.Close();
                }
            }
        }

        public static void ReadUserMenu(UserMenu user_menu)
        {
            RegistryKey reg = Registry.CurrentUser.CreateSubKey(REG_PATH + @"\" + KEY_USER_COMMANDS);

            string[] subkey_names = reg.GetValueNames();

            if ((subkey_names == null) || (subkey_names.Length == 0))
            {
                return;
            }

            for (int i = 0; i < subkey_names.Length; i++)
            {
                string menu_text = subkey_names[i];
                string[] value_parts = (string[])reg.GetValue(menu_text, new string[] { string.Empty, "0" });

                UserMenuEntry new_entry = new UserMenuEntry(value_parts[0], menu_text);
                new_entry.Options = (ProcessStartFlags)int.Parse(value_parts[1]);
                
                user_menu.Add(new_entry);
            }
            reg.Close();
        }

        public static void WriteUserMenu(UserMenu user_menu)
        {
            RegistryKey reg = Registry.CurrentUser.CreateSubKey(REG_PATH + @"\" + KEY_USER_COMMANDS);

            string[] subkey_names = reg.GetValueNames();

            if ((subkey_names != null) && (subkey_names.Length != 0))
            {
                for (int i = 0; i < subkey_names.Length; i++)
                {
                    reg.DeleteValue(subkey_names[i]);
                }
            }

            //now write
            for (int i = 0; i < user_menu.Count; i++)
            {
                string[] value_parts = new string[2];
                value_parts[0] = user_menu[i].CommandText;
                value_parts[1] = ((int)user_menu[i].Options).ToString();
                reg.SetValue(user_menu[i].Text, value_parts, RegistryValueKind.MultiString);
            }
        }

        public static int ProgressUpdateInterval
        {
            get
            {
                int ret=1048576;
                using (RegistryKey reg = GetRegKey())
                {
                    ret = (int)reg.GetValue(KEY_PROGRESS_UPDATE_INTERVAL, ret);
                    reg.Close();
                }
                return ret;
            }
            set
            {
                using (RegistryKey reg = GetRegKey())
                {
                    reg.SetValue(KEY_PROGRESS_UPDATE_INTERVAL, value, RegistryValueKind.DWord);
                    reg.Close();
                }
            }
        }

        public static int CodePage
        {
            get
            {
                int ret = System.Text.Encoding.Default.CodePage;
                using (RegistryKey reg = GetRegKey())
                {
                    ret = (int)reg.GetValue(KEY_CODE_PAGE, ret);
                    reg.Close();
                }
                return ret;
            }
            set
            {
                using (RegistryKey reg = GetRegKey())
                {
                    reg.SetValue(KEY_CODE_PAGE, value, RegistryValueKind.DWord);
                    reg.Close();
                }
            }
        }

        public static string LastDirectory
        {
            get
            {
                string ret = string.Empty;
                using (RegistryKey reg = GetRegKey())
                {
                    ret = (string)reg.GetValue(KEY_LAST_DIRECTORY, ret);
                    reg.Close();
                }
                return ret;
            }
            set
            {
                using (RegistryKey reg = GetRegKey())
                {
                    reg.SetValue(KEY_LAST_DIRECTORY, value);
                }
            }
        }

        public static ArchiveAddOptions ArchiveAddOptions
        {
            get
            {
                ArchiveAddOptions ret = ArchiveAddOptions.NeverRewrite | ArchiveAddOptions.Recursive | ArchiveAddOptions.SaveAttributes;
                using (RegistryKey reg = GetRegKey())
                {
                    ret = (ArchiveAddOptions)reg.GetValue(KEY_ARCHIVE_ADD_OPTIONS, ret);
                    reg.Close();
                }
                return ret;
            }
            set
            {
                using (RegistryKey reg = GetRegKey())
                {
                    reg.SetValue(KEY_ARCHIVE_ADD_OPTIONS, (int)value, RegistryValueKind.DWord);
                }
            }
        }

        public static ArchiveExtractOptions ArchiveExtractOptions
        {
            get
            {
                ArchiveExtractOptions ret = ArchiveExtractOptions.NeverOverwite | ArchiveExtractOptions.ExtractAttributes | ArchiveExtractOptions.ExtractRecursively;
                using (RegistryKey reg = GetRegKey())
                {
                    ret = (ArchiveExtractOptions)reg.GetValue(KEY_ARCHIVE_EXTRACT_OPTIONS, ret);
                    reg.Close();
                }
                return ret;
            }
            set
            {
                using (RegistryKey reg = GetRegKey())
                {
                    reg.SetValue(KEY_ARCHIVE_EXTRACT_OPTIONS, (int)value, RegistryValueKind.DWord);
                }
            }
        }

        public static TerminateProcessOptions TerminateProcessOptions
        {
            get
            {
                TerminateProcessOptions ret = TerminateProcessOptions.CloseMainWindow;
                using (RegistryKey reg = GetRegKey())
                {
                    ret = (TerminateProcessOptions)reg.GetValue(KEY_PROCESS_TERMINATE_OPTIONS, ret);
                    reg.Close();
                }
                return ret;
            }
            set
            {
                using (RegistryKey reg = GetRegKey())
                {
                    reg.SetValue(KEY_PROCESS_TERMINATE_OPTIONS, (int)value, RegistryValueKind.DWord);
                }
            }
        }

        public static RunExeOptions RunExeOptions
        {
            get
            {
                RunExeOptions ret = RunExeOptions.None;
                using (RegistryKey reg = GetRegKey())
                {
                    ret = (RunExeOptions)reg.GetValue(KEY_RUNEXE_OPTIONS, ret);
                    reg.Close();
                }
                return ret;
            }
            set
            {
                using (RegistryKey reg = GetRegKey())
                {
                    reg.SetValue(KEY_RUNEXE_OPTIONS, (int)value, RegistryValueKind.DWord);
                }
            }
        }

        public static DeleteFileOptions DeleteFileOptions
        {
            get
            {
                DeleteFileOptions ret = DeleteFileOptions.None;
                using (RegistryKey reg = GetRegKey())
                {
                    ret = (DeleteFileOptions)reg.GetValue(KEY_DELETE_OPTIONS, ret);
                    reg.Close();
                }
                return ret;
            }
            set
            {
                using (RegistryKey reg = GetRegKey())
                {
                    reg.SetValue(KEY_DELETE_OPTIONS, (int)value, RegistryValueKind.DWord);
                }
            }
        }

        public static netCommander.FileSystemEx.NTFSlinkType LinkType
        {
            get
            {
                netCommander.FileSystemEx.NTFSlinkType ret = netCommander.FileSystemEx.NTFSlinkType.Junction;
                using (RegistryKey reg = GetRegKey())
                {
                    ret = (netCommander.FileSystemEx.NTFSlinkType)reg.GetValue(KEY_DEFAULT_LINK_TYPE, ret);
                    reg.Close();
                }
                return ret;
            }
            set
            {
                using (RegistryKey reg = GetRegKey())
                {
                    reg.SetValue(KEY_DEFAULT_LINK_TYPE, (int)value, RegistryValueKind.DWord);
                }
            }
        }

        public static FtpTransferOptions FtpDownloadOptions
        {
            get
            {
                FtpTransferOptions ret = FtpTransferOptions.Default();
                using (RegistryKey reg = GetRegKey())
                {
                    ret = FtpTransferOptions.FromBytes((byte[])reg.GetValue(KEY_FTP_DOWNLOAD_SETTINGS, ret.Serialize()));
                    reg.Close();
                }
                return ret;
            }
            set
            {
                using (RegistryKey reg = GetRegKey())
                {
                    reg.SetValue(KEY_FTP_DOWNLOAD_SETTINGS, value.Serialize(), RegistryValueKind.Binary);
                    reg.Close();
                }
            }
        }

        public static FtpTransferOptions FtpUploadOptions
        {
            get
            {
                FtpTransferOptions ret = FtpTransferOptions.Default();
                using (RegistryKey reg = GetRegKey())
                {
                    ret = FtpTransferOptions.FromBytes((byte[])reg.GetValue(KEY_FTP_UPLOAD_SETTINGS, ret.Serialize()));
                    reg.Close();
                }
                return ret;
            }
            set
            {
                using (RegistryKey reg = GetRegKey())
                {
                    reg.SetValue(KEY_FTP_UPLOAD_SETTINGS, value.Serialize(), RegistryValueKind.Binary);
                    reg.Close();
                }
            }
        }

        public static FtpConnectionOptions FtpOptions
        {
            get
            {
                FtpConnectionOptions ret = new FtpConnectionOptions();
                using (RegistryKey reg = GetRegKey())
                {
                    string opts_str = (string)reg.GetValue(KEY_FTP_SETTINGS, string.Empty);
                    if (opts_str != string.Empty)
                    {
                        ret = FtpConnectionOptions.FromString(opts_str);
                    }
                    reg.Close();
                }
                return ret;
            }
            set
            {
                using (RegistryKey reg = GetRegKey())
                {
                    reg.SetValue(KEY_FTP_SETTINGS, value.Serialize());
                    reg.Close();
                }
            }
        }

        public static MoveEngineOptions MoveEngineOptions
        {
            get
            {
                MoveEngineOptions ret = MoveEngineOptions.None;
                using (RegistryKey reg = GetRegKey())
                {
                    int ret_int = (int)reg.GetValue(KEY_MOVE_ENGINE_OPTIONS, (int)ret);
                    ret = (MoveEngineOptions)ret_int;
                }
                return ret;
            }
            set
            {
                using (RegistryKey reg = GetRegKey())
                {
                    reg.SetValue(KEY_MOVE_ENGINE_OPTIONS, (int)value, RegistryValueKind.DWord);
                }
            }
        }

        public static CopyEngineOptions CopyEngineOptions
        {
            get
            {
                CopyEngineOptions ret = CopyEngineOptions.NoRewrite;
                using (RegistryKey reg = GetRegKey())
                {
                    int ret_int = (int)reg.GetValue(KEY_COPY_ENGINE_OPTIONS, (int)ret);
                    ret = (CopyEngineOptions)ret_int;
                }
                return ret;
            }
            set
            {
                using (RegistryKey reg = GetRegKey())
                {
                    reg.SetValue(KEY_COPY_ENGINE_OPTIONS, (int)value, RegistryValueKind.DWord);
                }
            }
        }

        public static bool CopyCloseProgress
        {
            get
            {
                bool ret = false;
                using (RegistryKey reg = GetRegKey())
                {
                    ret = int2bool((int)reg.GetValue(KEY_COPY_CLOSE_PROGRESS, bool2int(ret)));
                }
                return ret;
            }
            set
            {
                using (RegistryKey reg = GetRegKey())
                {
                    reg.SetValue(KEY_COPY_CLOSE_PROGRESS, bool2int(value), RegistryValueKind.DWord);
                }
            }
        }

        public static byte[] FilterDirectory
        {
            get
            {
                byte[] ret = new byte[0];
                using (RegistryKey reg = GetRegKey())
                {
                    ret = (byte[])reg.GetValue(KEY_FILTER_DIRECTORY, ret);
                }
                return ret;
            }
            set
            {
                using (RegistryKey reg = GetRegKey())
                {
                    reg.SetValue(KEY_FILTER_DIRECTORY, value, RegistryValueKind.Binary);
                }
            }
        }

        public static void SaveWindowState(string ID, Form window)
        {
            byte[] state = State2Bytes(window);
            using (RegistryKey reg = GetRegKey())
            {
                reg.SetValue
                    (KEY_PREFIX_WINDOW_STATE + ID,
                    state,
                    RegistryValueKind.Binary);
                reg.Close();
            }
        }

        public static void SetWindowState(string ID, Form window)
        {
            using (RegistryKey reg = GetRegKey())
            {
                try
                {
                    byte[] state = (byte[])reg.GetValue
                        (KEY_PREFIX_WINDOW_STATE + ID,
                        new byte[] { });
                    if (state.Length == 24)
                    {
                        Bytes2State(state, window);
                    }
                }
                catch (Exception ex)
                {
                    ApplicationException custom_ex = new ApplicationException
                    ("Failed to set window state. " + ex.Message,
                    ex);
                    custom_ex.Source = ex.Source;
                    Messages.ShowException(custom_ex);
                }
                reg.Close();
            }
        }
        
        public static Font FontFilePanel
        {
            get
            {
                Font ret = null;
                using (RegistryKey reg = GetRegKey())
                {
                    try
                    {
                        string[] fromReg = (string[])reg.GetValue(KEY_FONT_FILE_PANEL, new string[] { });
                        if (fromReg.Length != 3)
                        {
                            ret = new Font(FontFamily.GenericMonospace, 10, FontStyle.Regular);
                        }
                        else
                        {
                            ret = new Font
                            (fromReg[0],
                            float.Parse(fromReg[1], CultureInfo.InvariantCulture),
                            (FontStyle)Enum.Parse(typeof(FontStyle), fromReg[2], true));
                        }
                    }
                    catch
                    {
                        ret = new Font(FontFamily.GenericMonospace, 10, FontStyle.Regular);
                    }
                    reg.Close();
                }
                if (ret == null)
                {
                    new Font(FontFamily.GenericMonospace, 10, FontStyle.Regular);
                }
                return ret;
            }
            set
            {
                using (RegistryKey reg = GetRegKey())
                {
                    string[] toReg = new string[3];
                    toReg[0] = value.Name;
                    toReg[1] = value.Size.ToString(CultureInfo.InvariantCulture);
                    toReg[2] = value.Style.ToString();
                    reg.SetValue(KEY_FONT_FILE_PANEL, toReg, RegistryValueKind.MultiString);
                    reg.Close();
                }
            }
        }

        public static Font FontView
        {
            get
            {
                Font ret = null;
                using (RegistryKey reg = GetRegKey())
                {
                    try
                    {
                        string[] fromReg = (string[])reg.GetValue(KEY_VIEW_FONT, new string[] { });
                        if (fromReg.Length != 3)
                        {
                            ret = new Font(FontFamily.GenericMonospace, 10, FontStyle.Regular);
                        }
                        else
                        {
                            ret = new Font
                            (fromReg[0],
                            float.Parse(fromReg[1],CultureInfo.InvariantCulture),
                            (FontStyle)Enum.Parse(typeof(FontStyle), fromReg[2], true));
                        }
                    }
                    catch
                    {
                        ret = new Font(FontFamily.GenericMonospace, 10, FontStyle.Regular);
                    }
                    reg.Close();
                }
                if (ret == null)
                {
                    new Font(FontFamily.GenericMonospace, 10, FontStyle.Regular);
                }
                return ret;
            }
            set
            {
                using (RegistryKey reg = GetRegKey())
                {
                    string[] toReg = new string[3];
                    toReg[0] = value.Name;
                    toReg[1] = value.Size.ToString(CultureInfo.InvariantCulture);
                    toReg[2] = value.Style.ToString();
                    reg.SetValue(KEY_VIEW_FONT, toReg, RegistryValueKind.MultiString);
                    reg.Close();
                }
            }
        }

        public static bool IsVistaOrServer2003OrLater()
        {
            OperatingSystem os = Environment.OSVersion;
            if (os.Version.Major > 5)
            {
                return true;
            }
            if ((os.Version.Major > 4) && (os.Version.Minor > 1))
            {
                return true;
            }
            return false;
        }

        private static byte[] Rect2Bytes(Rectangle rect)
        {
            byte[] ret = new byte[16];
            byte[] iBytes=new byte[4];

            iBytes = BitConverter.GetBytes(rect.Left);
            Array.Copy(iBytes, 0, ret, 0, 4);

            iBytes = BitConverter.GetBytes(rect.Top);
            Array.Copy(iBytes, 0, ret, 4, 4);

            iBytes = BitConverter.GetBytes(rect.Right);
            Array.Copy(iBytes, 0, ret, 8, 4);

            iBytes = BitConverter.GetBytes(rect.Bottom);
            Array.Copy(iBytes, 0, ret, 12, 4);

            return ret;
        }

        private static Rectangle Bytes2Rect(byte[] bytes)
        {
            int left = BitConverter.ToInt32(bytes, 0);
            int top = BitConverter.ToInt32(bytes, 4);
            int right = BitConverter.ToInt32(bytes, 8);
            int bottom = BitConverter.ToInt32(bytes, 12);

            return Rectangle.FromLTRB(left, top, right, bottom);
        }

        private static byte[] State2Bytes(Form window)
        {
            /*
             * 0->3     Start position
             * 4->7     Form state
             * 8->11    left
             * 12->15   top
             * 16->19   right
             * 20->23   bottom
             * 
             * всего 24 байта
             */

            MemoryStream mStream = new MemoryStream(24);
            BinaryWriter bWriter = new BinaryWriter(mStream);

            bWriter.Write((int)window.StartPosition);
            bWriter.Write((int)window.WindowState);

            //Rectangle save_rect = window.RestoreBounds;
            Rectangle save_rect = window.Bounds;
            if (window.WindowState != FormWindowState.Normal)
            {
                save_rect = window.RestoreBounds;
            }

            bWriter.Write(save_rect.Left);
            bWriter.Write(save_rect.Top);
            bWriter.Write(save_rect.Right);
            bWriter.Write(save_rect.Bottom);

            return mStream.ToArray();
        }

        private static void Bytes2State(byte[] state, Form window)
        {
            /*
            * 0->3     Start position
            * 4->7     Form state
            * 8->11    left
            * 12->15   top
            * 16->19   right
            * 20->23   bottom
            * 
            * всего 24 байта
            */

            FormStartPosition start_pos = (FormStartPosition)BitConverter.ToInt32(state, 0);
            FormWindowState win_state = (FormWindowState)BitConverter.ToInt32(state, 4);
            int left = BitConverter.ToInt32(state, 8);
            int top = BitConverter.ToInt32(state, 12);
            int right = BitConverter.ToInt32(state, 16);
            int bottom = BitConverter.ToInt32(state, 20);
            Rectangle win_rect = Rectangle.FromLTRB(left, top, right, bottom);

            //window.StartPosition = start_pos;
            window.StartPosition = FormStartPosition.Manual;
            window.WindowState = win_state;
            //window.Bounds = win_rect;
            window.SetBounds(win_rect.X, win_rect.Y, win_rect.Width, win_rect.Height);
        }

        private static int bool2int(bool inp)
        {
            return inp ? 1 : 0;
        }

        private static bool int2bool(int inp)
        {
            return (inp != 0);
        }
    }
}
