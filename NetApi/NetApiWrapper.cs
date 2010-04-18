using System;
using System.ComponentModel;
using System.Runtime.InteropServices;
using System.Collections.Generic;

namespace netCommander.NetApi
{
    class WinApiNETwrapper
    {

        public static SHARE_INFO_2 GetShareInfo_2(string server_name, string share_name)
        {
            IntPtr buffer = IntPtr.Zero;
            SHARE_INFO_2 ret = new SHARE_INFO_2();

            try
            {
                int res = WinApiNET.NetShareGetInfo(server_name, share_name, NetShareInfoLevel.INFO_2, ref buffer);
                if (res != WinApiNET.NERR_Success)
                {
                    throw new Win32Exception(res);
                }

                ret = (SHARE_INFO_2)Marshal.PtrToStructure(buffer, typeof(SHARE_INFO_2));
            }
            finally
            {
                if (buffer != IntPtr.Zero)
                {
                    WinApiNET.NetApiBufferFree(buffer);
                }
            }

            return ret;
        }

        public static Array NetFileEnum(string server_name, string base_path, string user_name, NetFileEnumLevel level)
        {
            Array ret = null;
            IntPtr buffer = IntPtr.Zero;
            uint prefmaxlen = WinApiNET.MAX_PREFERRED_LENGTH;
            int entriesread = 0;
            int totalentries = 0;
            uint resume_handle = 0;

            try
            {
                int res = WinApiNET.NetFileEnum
                    (server_name,
                    base_path,
                    user_name,
                    level,
                    ref buffer,
                    prefmaxlen,
                    ref entriesread,
                    ref totalentries,
                    ref resume_handle);
                if (res != WinApiNET.NERR_Success)
                {
                    throw new Win32Exception(res);
                }

                switch (level)
                {
                    case NetFileEnumLevel.INFO_2:
                        ret = FILE_INFO_2.FromBuffer(buffer, entriesread);
                        break;
                    case NetFileEnumLevel.INFO_3:
                        ret = FILE_INFO_3.FromBuffer(buffer, entriesread);
                        break;
                }
            }
            finally
            {
                if (buffer != IntPtr.Zero)
                {
                    WinApiNET.NetApiBufferFree(buffer);
                }
            }
            return ret;
        }

        public static Array NetSessionEnum(string server_name, string client_name, string user_name, NetSessionEnumLevel level)
        {
            Array ret = null;
            IntPtr buffer = IntPtr.Zero;
            uint prefmaxlen = WinApiNET.MAX_PREFERRED_LENGTH;
            int entriesread = 0;
            int totalentris = 0;
            uint resume_handle = 0;

            
            try
            {
                int res = WinApiNET.NetSessionEnum
                    (server_name,
                    client_name,
                    user_name,
                    level,
                    ref buffer,
                    prefmaxlen,
                    ref entriesread,
                    ref totalentris,
                    ref resume_handle);
                if (res != WinApiNET.NERR_Success)
                {
                    throw new Win32Exception(res);
                }

                switch (level)
                {
                    case NetSessionEnumLevel.INFO_0:
                        ret = SESSION_INFO_0.FromBuffer(buffer, entriesread);
                        break;
                    case NetSessionEnumLevel.INFO_1:
                        ret = SESSION_INFO_1.FromBuffer(buffer, entriesread);
                        break;
                    case NetSessionEnumLevel.INFO_2:
                        ret = SESSION_INFO_2.FromBuffer(buffer, entriesread);
                        break;
                    case NetSessionEnumLevel.INFO_10:
                        ret = SESSION_INFO_10.FromBuffer(buffer, entriesread);
                        break;
                    case NetSessionEnumLevel.INFO_502:
                        ret = SESSION_INFO_502.FromBuffer(buffer, entriesread);
                        break;
                }
            }
            finally
            {
                if (buffer != IntPtr.Zero)
                {
                    WinApiNET.NetApiBufferFree(buffer);
                }
            }
            return ret;
        }

        public static NET_DISPLAY_GROUP[] QueryDisplayInfoGroup(string server_name)
        {
            List<NET_DISPLAY_GROUP> ret_list = new List<NET_DISPLAY_GROUP>();
            IntPtr net_buffer = IntPtr.Zero;
            int res = 0;
            int res_free = 0;
            int request_index = 0;
            int max_request_entries = 100;
            int return_entries = 0;

            do
            {
                if ((server_name == null) || (server_name == string.Empty))
                {
                    res = WinApiNET.NetQueryDisplayInformation
                        (IntPtr.Zero,
                        NetqueryDisplayInfoLevel.Group,
                        request_index,
                        max_request_entries,
                        WinApiNET.MAX_PREFERRED_LENGTH,
                        ref return_entries,
                        ref net_buffer);
                }
                else
                {
                    res = WinApiNET.NetQueryDisplayInformation
                        (server_name,
                        NetqueryDisplayInfoLevel.Group,
                        request_index,
                        max_request_entries,
                        WinApiNET.MAX_PREFERRED_LENGTH,
                        ref return_entries,
                        ref net_buffer);
                }
                //now check result
                if (res == WinApiNET.NERR_Success)
                {
                    //success, add entries to return list
                    ret_list.AddRange(NET_DISPLAY_GROUP.FromPtr(net_buffer, return_entries));
                    //free net buffer
                    res_free = WinApiNET.NetApiBufferFree(net_buffer);
                    if (res_free != WinApiNET.NERR_Success)
                    {
                        throw new Win32Exception(res_free);
                    }
                    //and break cycle
                    break;
                }
                if (res == WinApiNET.ERROR_MORE_DATA)
                {
                    //success, but more entries available
                    ret_list.AddRange(NET_DISPLAY_GROUP.FromPtr(net_buffer, return_entries));
                    //free buffer
                    res_free = WinApiNET.NetApiBufferFree(net_buffer);
                    if (res_free != WinApiNET.NERR_Success)
                    {
                        throw new Win32Exception(res_free);
                    }
                    //set request_index (that is last member of ret_list)
                    request_index = ret_list[ret_list.Count - 1].grpi3_next_index;
                    //and continue cycle
                    continue;
                }
                //now res is error code
                Win32Exception win_ex = new Win32Exception(res);
                throw win_ex;

            } while (true);
            return ret_list.ToArray();
        }

        public static NET_DISPLAY_MACHINE[] QueryDisplayInfoMachine(string server_name)
        {
            List<NET_DISPLAY_MACHINE> ret_list = new List<NET_DISPLAY_MACHINE>();
            IntPtr net_buffer = IntPtr.Zero;
            int res = 0;
            int res_free = 0;
            int request_index = 0;
            int max_request_entries = 100;
            int return_entries = 0;

            do
            {
                if ((server_name == null) || (server_name == string.Empty))
                {
                    res = WinApiNET.NetQueryDisplayInformation
                        (IntPtr.Zero,
                        NetqueryDisplayInfoLevel.Machine,
                        request_index,
                        max_request_entries,
                        WinApiNET.MAX_PREFERRED_LENGTH,
                        ref return_entries,
                        ref net_buffer);
                }
                else
                {
                    res = WinApiNET.NetQueryDisplayInformation
                        (server_name,
                        NetqueryDisplayInfoLevel.Machine,
                        request_index,
                        max_request_entries,
                        WinApiNET.MAX_PREFERRED_LENGTH,
                        ref return_entries,
                        ref net_buffer);
                }
                //now check result
                if (res == WinApiNET.NERR_Success)
                {
                    //success, add entries to return list
                    ret_list.AddRange(NET_DISPLAY_MACHINE.FromPtr(net_buffer, return_entries));
                    //free net buffer
                    res_free = WinApiNET.NetApiBufferFree(net_buffer);
                    if (res_free != WinApiNET.NERR_Success)
                    {
                        throw new Win32Exception(res_free);
                    }
                    //and break cycle
                    break;
                }
                if (res == WinApiNET.ERROR_MORE_DATA)
                {
                    //success, but more entries available
                    ret_list.AddRange(NET_DISPLAY_MACHINE.FromPtr(net_buffer, return_entries));
                    //free buffer
                    res_free = WinApiNET.NetApiBufferFree(net_buffer);
                    if (res_free != WinApiNET.NERR_Success)
                    {
                        throw new Win32Exception(res_free);
                    }
                    //set request_index (that is last member of ret_list)
                    request_index = ret_list[ret_list.Count - 1].usri2_next_index;
                    //and continue cycle
                    continue;
                }
                //now res is error code
                Win32Exception win_ex = new Win32Exception(res);
                throw win_ex;

            } while (true);
            return ret_list.ToArray();
        }

        public static SERVER_TRANSPORT_INFO_1[] ServerTransportEnum_1(string servername)
        {
            IntPtr buffer = IntPtr.Zero;
            uint prefmaxlen = WinApiNET.MAX_PREFERRED_LENGTH;
            int entriesread = 0;
            int totalentries = 0;
            uint resumehandle = 0;
            SERVER_TRANSPORT_INFO_1[] ret = new SERVER_TRANSPORT_INFO_1[0];

            try
            {
                int res = WinApiNET.NetServerTransportEnum
                    (servername,
                    NetServerTransportEnumLevel.INFO_1,
                    ref buffer,
                    prefmaxlen,
                    ref entriesread,
                    ref totalentries,
                    ref resumehandle);
                if (res != WinApiNET.NERR_Success)
                {
                    throw new Win32Exception(res);
                }
                ret = SERVER_TRANSPORT_INFO_1.FromBuffer(buffer, entriesread);
            }
            finally
            {
                if (buffer != IntPtr.Zero)
                {
                    WinApiNET.NetApiBufferFree(buffer);
                }
            }

            return ret;
        }

        public static SERVER_TRANSPORT_INFO_0[] ServerTransportEnum_0(string servername)
        {
            IntPtr buffer = IntPtr.Zero;
            uint prefmaxlen = WinApiNET.MAX_PREFERRED_LENGTH;
            int entriesread = 0;
            int totalentries = 0;
            uint resumehandle = 0;
            SERVER_TRANSPORT_INFO_0[] ret = new SERVER_TRANSPORT_INFO_0[0];

            try
            {
                int res = WinApiNET.NetServerTransportEnum
                    (servername,
                    NetServerTransportEnumLevel.INFO_0,
                    ref buffer,
                    prefmaxlen,
                    ref entriesread,
                    ref totalentries,
                    ref resumehandle);
                if (res != WinApiNET.NERR_Success)
                {
                    throw new Win32Exception(res);
                }
                ret = SERVER_TRANSPORT_INFO_0.FromBuffer(buffer, entriesread);
            }
            finally
            {
                if (buffer != IntPtr.Zero)
                {
                    WinApiNET.NetApiBufferFree(buffer);
                }
            }

            return ret;
        }

        public static NET_DISPLAY_USER[] QueryDisplayInfoUser(string server_name)
        {
            List<NET_DISPLAY_USER> ret_list = new List<NET_DISPLAY_USER>();
            IntPtr net_buffer = IntPtr.Zero;
            int res = 0;
            int free_res = 0;
            int request_index = 0;
            int max_request_entries = 100;
            int return_entries = 0;

            do
            {
                if ((server_name == null) || (server_name == string.Empty))
                {
                    res = WinApiNET.NetQueryDisplayInformation
                    (IntPtr.Zero,
                    NetqueryDisplayInfoLevel.User,
                    request_index,
                    max_request_entries,
                    WinApiNET.MAX_PREFERRED_LENGTH,
                    ref return_entries,
                    ref net_buffer);
                }
                else
                {
                    res = WinApiNET.NetQueryDisplayInformation
                        (server_name,
                        NetqueryDisplayInfoLevel.User,
                        request_index,
                        max_request_entries,
                        WinApiNET.MAX_PREFERRED_LENGTH,
                        ref return_entries,
                        ref net_buffer);
                }
                if (res == WinApiNET.NERR_Success)
                {
                    //success, add entries to return list
                    ret_list.AddRange(NET_DISPLAY_USER.FromPtr(net_buffer, return_entries));
                    //free net buffer
                    free_res = WinApiNET.NetApiBufferFree(net_buffer);
                    if (free_res != WinApiNET.NERR_Success)
                    {
                        throw new Win32Exception(free_res);
                    }
                    //and break cycle
                    break;
                }
                if (res == WinApiNET.ERROR_MORE_DATA)
                {
                    //success, but more entries available
                    ret_list.AddRange(NET_DISPLAY_USER.FromPtr(net_buffer, return_entries));
                    //free buffer
                    free_res = WinApiNET.NetApiBufferFree(net_buffer);
                    if (free_res != WinApiNET.NERR_Success)
                    {
                        throw new Win32Exception(free_res);
                    }
                    //set request_index (that is last member of ret_list)
                    request_index = ret_list[ret_list.Count - 1].usri1_next_index;
                    //and continue cycle
                    continue;
                }
                //now res is error code
                Win32Exception win_ex = new Win32Exception(res);
                throw win_ex;

            } while (true);

            return ret_list.ToArray();
        }

        public static SERVER_INFO_100 GetServerInfo_100(string server_name)
        {
            IntPtr buffer = IntPtr.Zero;
            SERVER_INFO_100 ret = new SERVER_INFO_100();

            try
            {
                int res = WinApiNET.NetServerGetInfo(server_name, NetserverInfoLevel.INFO_100, ref buffer);
                if (res != WinApiNET.NERR_Success)
                {
                    throw new Win32Exception(res);
                }
                ret = (SERVER_INFO_100)Marshal.PtrToStructure(buffer, typeof(SERVER_INFO_100));
            }//end of try
            finally
            {
                if (buffer != IntPtr.Zero)
                {
                    WinApiNET.NetApiBufferFree(buffer);
                }
            }

            return ret;
        }

        public static SERVER_INFO_101 GetServerInfo_101(string server_name)
        {
            IntPtr buffer = IntPtr.Zero;
            SERVER_INFO_101 ret = new SERVER_INFO_101();

            try
            {
                int res = WinApiNET.NetServerGetInfo(server_name, NetserverInfoLevel.INFO_101, ref buffer);
                if (res != WinApiNET.NERR_Success)
                {
                    throw new Win32Exception(res);
                }
                ret = (SERVER_INFO_101)Marshal.PtrToStructure(buffer, typeof(SERVER_INFO_101));
            }//end of try
            finally
            {
                if (buffer != IntPtr.Zero)
                {
                    WinApiNET.NetApiBufferFree(buffer);
                }
            }

            return ret;
        }

        public static SERVER_INFO_102 GetServerInfo_102(string server_name)
        {
            IntPtr buffer = IntPtr.Zero;
            SERVER_INFO_102 ret = new SERVER_INFO_102();

            try
            {
                int res = WinApiNET.NetServerGetInfo(server_name, NetserverInfoLevel.INFO_102, ref buffer);
                if (res != WinApiNET.NERR_Success)
                {
                    throw new Win32Exception(res);
                }
                ret = (SERVER_INFO_102)Marshal.PtrToStructure(buffer, typeof(SERVER_INFO_102));
            }//end of try
            finally
            {
                if (buffer != IntPtr.Zero)
                {
                    WinApiNET.NetApiBufferFree(buffer);
                }
            }

            return ret;
        }

        public static NetRemoteComputerSupportsFeatures GetComputerSupports(string server_name)
        {
            NetRemoteComputerSupportsFeatures wanted = NetRemoteComputerSupportsFeatures.ANY;
            NetRemoteComputerSupportsFeatures supported = NetRemoteComputerSupportsFeatures.ANY;

            int res = WinApiNET.NetRemoteComputerSupports(server_name, wanted, ref supported);
            if (res != WinApiNET.NERR_Success)
            {
                throw new Win32Exception(res);
            }

            return supported;
        }

        public static TIME_OF_DAY_INFO GetServerTime(string server_name)
        {
            IntPtr buffer = IntPtr.Zero;
            TIME_OF_DAY_INFO ret = new TIME_OF_DAY_INFO();

            try
            {
                int res = WinApiNET.NetRemoteTOD(server_name, ref buffer);

                if (res != WinApiNET.NERR_Success)
                {
                    throw new Win32Exception(res);
                }

                ret = (TIME_OF_DAY_INFO)Marshal.PtrToStructure(buffer, typeof(TIME_OF_DAY_INFO));
            }
            finally
            {
                if (buffer != IntPtr.Zero)
                {
                    WinApiNET.NetApiBufferFree(buffer);
                }
            }
            return ret;
        }

        public static SERVER_INFO_101[] GetServerInfos_101(NetserverEnumType servers_type)
        {
            IntPtr ptBuffer = IntPtr.Zero;
            int entryes_readed = 0;
            int total_entryes = 0;
            uint resume_handle = 0;
            IntPtr pt_one_struct = IntPtr.Zero;

            try
            {
                int res = WinApiNET.NetServerEnum
                    (IntPtr.Zero,
                    NetserverEnumLevel.LEVEL_101,
                    ref ptBuffer,
                    WinApiNET.MAX_PREFERRED_LENGTH,
                    ref entryes_readed,
                    ref total_entryes,
                    servers_type,
                    IntPtr.Zero,
                    ref resume_handle);
                if (res != WinApiNET.NERR_Success)
                {
                    throw new Win32Exception(res);
                }

                int one_struct_len = Marshal.SizeOf(typeof(SERVER_INFO_101));
                SERVER_INFO_101[] ret = new SERVER_INFO_101[entryes_readed];
                pt_one_struct = Marshal.AllocHGlobal(one_struct_len);
                for (int i = 0; i < entryes_readed; i++)
                {
                    for (int offset = 0; offset < one_struct_len; offset++)
                    {
                        Marshal.WriteByte
                            (pt_one_struct,
                            offset,
                            Marshal.ReadByte(ptBuffer, i * one_struct_len + offset));
                    }
                    ret[i] = (SERVER_INFO_101)Marshal.PtrToStructure
                        (pt_one_struct,
                        typeof(SERVER_INFO_101));
                }
                return ret;
            }
            finally
            {
                if (ptBuffer != IntPtr.Zero)
                {
                    WinApiNET.NetApiBufferFree(ptBuffer);
                }
                if (pt_one_struct != null)
                {
                    Marshal.FreeHGlobal(pt_one_struct);
                }
            }
        }

        public static SERVER_INFO_101[] GetServerInfos_101(string domainName, NetserverEnumType servers_type)
        {
            IntPtr ptBuffer=IntPtr.Zero;
            int entryes_readed=0;
            int total_entryes=0;
            uint resume_handle=0;
            IntPtr pt_one_struct=IntPtr.Zero;

            try
            {
                int res = WinApiNET.NetServerEnum
                    (IntPtr.Zero,
                    NetserverEnumLevel.LEVEL_101,
                    ref ptBuffer,
                    WinApiNET.MAX_PREFERRED_LENGTH,
                    ref entryes_readed,
                    ref total_entryes,
                    servers_type,
                    domainName,
                    ref resume_handle);
                if (res != WinApiNET.NERR_Success)
                {
                    throw new Win32Exception(res);
                }

                int one_struct_len = Marshal.SizeOf(typeof(SERVER_INFO_101));
                SERVER_INFO_101[] ret = new SERVER_INFO_101[entryes_readed];
                pt_one_struct = Marshal.AllocHGlobal(one_struct_len);
                for (int i = 0; i < entryes_readed; i++)
                {
                    for (int offset = 0; offset < one_struct_len; offset++)
                    {
                        Marshal.WriteByte
                            (pt_one_struct,
                            offset,
                            Marshal.ReadByte(ptBuffer, i * one_struct_len + offset));
                    }
                    ret[i] = (SERVER_INFO_101)Marshal.PtrToStructure
                        (pt_one_struct,
                        typeof(SERVER_INFO_101));
                }
                return ret;
            }
            finally
            {
                if (ptBuffer != IntPtr.Zero)
                {
                    WinApiNET.NetApiBufferFree(ptBuffer);
                }
                if (pt_one_struct != null)
                {
                    Marshal.FreeHGlobal(pt_one_struct);
                }
            }
        }

        public static SHARE_INFO_2[] GetShareInfos_2(string server_name)
        {
            List<SHARE_INFO_2> ret_list = new List<SHARE_INFO_2>();
            IntPtr net_buffer = IntPtr.Zero;
            int entries_readed = 0;
            int entries_total = 0;
            uint resume_handle = 0;
            int res = 0;
            int res_free = 0;

            do
            {
                if ((server_name == null) || (server_name == string.Empty))
                {
                    res = WinApiNET.NetShareEnum
                        (IntPtr.Zero,
                        NET_INFO_LEVEL.LEVEL_2,
                        ref net_buffer,
                        WinApiNET.MAX_PREFERRED_LENGTH,
                        ref entries_readed,
                        ref entries_total,
                        ref resume_handle);
                }
                else
                {
                    res = WinApiNET.NetShareEnum
                        (server_name,
                        NET_INFO_LEVEL.LEVEL_2,
                        ref net_buffer,
                        WinApiNET.MAX_PREFERRED_LENGTH,
                        ref entries_readed,
                        ref entries_total,
                        ref resume_handle);
                }
                //check result
                if (res == WinApiNET.NERR_Success)
                {
                    //success, add to result list
                    ret_list.AddRange(SHARE_INFO_2.FromBuffer(net_buffer, entries_readed));
                    //free buffer
                    res_free = WinApiNET.NetApiBufferFree(net_buffer);
                    if (res_free != WinApiNET.NERR_Success)
                    {
                        throw new Win32Exception(res_free);
                    }
                    //break cycle
                    break;
                }
                if (res == WinApiNET.ERROR_MORE_DATA)
                {
                    //success, but more data available
                    ret_list.AddRange(SHARE_INFO_2.FromBuffer(net_buffer, entries_readed));
                    //free buffer
                    res_free = WinApiNET.NetApiBufferFree(net_buffer);
                    if (res_free != WinApiNET.NERR_Success)
                    {
                        throw new Win32Exception(res_free);
                    }
                    //continue cycle
                    continue;
                }
                //now res is error code
                Win32Exception win_ex = new Win32Exception(res);
                throw win_ex;
            } while (true);
            return ret_list.ToArray();
        }

        public static SHARE_INFO_1[] GetShareInfos_1(string server_name)
        {
            List<SHARE_INFO_1> ret_list = new List<SHARE_INFO_1>();
            IntPtr net_buffer = IntPtr.Zero;
            int entries_readed = 0;
            int entries_total = 0;
            uint resume_handle = 0;
            int res = 0;
            int res_free = 0;

            do
            {
                if ((server_name == null) || (server_name == string.Empty))
                {
                    res = WinApiNET.NetShareEnum
                        (IntPtr.Zero,
                        NET_INFO_LEVEL.LEVEL_1,
                        ref net_buffer,
                        WinApiNET.MAX_PREFERRED_LENGTH,
                        ref entries_readed,
                        ref entries_total,
                        ref resume_handle);
                }
                else
                {
                    res = WinApiNET.NetShareEnum
                        (server_name,
                        NET_INFO_LEVEL.LEVEL_1,
                        ref net_buffer,
                        WinApiNET.MAX_PREFERRED_LENGTH,
                        ref entries_readed,
                        ref entries_total,
                        ref resume_handle);
                }
                //check result
                if (res == WinApiNET.NERR_Success)
                {
                    //success, add to result list
                    ret_list.AddRange(SHARE_INFO_1.FromBuffer(net_buffer, entries_readed));
                    //free buffer
                    res_free = WinApiNET.NetApiBufferFree(net_buffer);
                    if (res_free != WinApiNET.NERR_Success)
                    {
                        throw new Win32Exception(res_free);
                    }
                    //break cycle
                    break;
                }
                if (res == WinApiNET.ERROR_MORE_DATA)
                {
                    //success, but more data available
                    ret_list.AddRange(SHARE_INFO_1.FromBuffer(net_buffer, entries_readed));
                    //free buffer
                    res_free = WinApiNET.NetApiBufferFree(net_buffer);
                    if (res_free != WinApiNET.NERR_Success)
                    {
                        throw new Win32Exception(res_free);
                    }
                    //continue cycle
                    continue;
                }
                //now res is error code
                Win32Exception win_ex = new Win32Exception(res);
                throw win_ex;
            } while (true);
            return ret_list.ToArray();
        }

        public static SHARE_INFO_0[] GetShareInfos_0(string server_name)
        {
            List<SHARE_INFO_0> ret_list = new List<SHARE_INFO_0>();
            IntPtr net_buffer = IntPtr.Zero;
            int entries_readed = 0;
            int entries_total = 0;
            uint resume_handle = 0;
            int res = 0;
            int res_free = 0;

            do
            {
                if ((server_name == null) || (server_name == string.Empty))
                {
                    res = WinApiNET.NetShareEnum
                        (IntPtr.Zero,
                        NET_INFO_LEVEL.LEVEL_0,
                        ref net_buffer,
                        WinApiNET.MAX_PREFERRED_LENGTH,
                        ref entries_readed,
                        ref entries_total,
                        ref resume_handle);
                }
                else
                {
                    res = WinApiNET.NetShareEnum
                        (server_name,
                        NET_INFO_LEVEL.LEVEL_0,
                        ref net_buffer,
                        WinApiNET.MAX_PREFERRED_LENGTH,
                        ref entries_readed,
                        ref entries_total,
                        ref resume_handle);
                }
                //check result
                if (res == WinApiNET.NERR_Success)
                {
                    //success, add to result list
                    ret_list.AddRange(SHARE_INFO_0.FromBuffer(net_buffer, entries_readed));
                    //free buffer
                    res_free = WinApiNET.NetApiBufferFree(net_buffer);
                    if (res_free != WinApiNET.NERR_Success)
                    {
                        throw new Win32Exception(res_free);
                    }
                    //break cycle
                    break;
                }
                if (res == WinApiNET.ERROR_MORE_DATA)
                {
                    //success, but more data available
                    ret_list.AddRange(SHARE_INFO_0.FromBuffer(net_buffer, entries_readed));
                    //free buffer
                    res_free = WinApiNET.NetApiBufferFree(net_buffer);
                    if (res_free != WinApiNET.NERR_Success)
                    {
                        throw new Win32Exception(res_free);
                    }
                    //continue cycle
                    continue;
                }
                //now res is error code
                Win32Exception win_ex = new Win32Exception(res);
                throw win_ex;
            } while (true);
            return ret_list.ToArray();
        }
    }
}
