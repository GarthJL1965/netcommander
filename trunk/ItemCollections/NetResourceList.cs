using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;
using netCommander.NetApi;
using netCommander.WNet;

namespace netCommander
{
    public class NetResourceList
    {
    }

    public class NetResourceInfo
    {
        public NetResourceSource Source { get; private set; }
        public string SMBname { get; private set; }
        public string Remark { get; private set; }
        public string Provider { get; private set; }
        public NetResourceType Type { get; private set; }
        public NETRESOURCE WNetResource { get; private set; }
        public SHARE_INFO_1 NetShareInfo { get; private set; }

        public NetResourceInfo(NETRESOURCE res)
        {
            Source = NetResourceSource.WNet;

            //if(res.

        }
    }

    public enum NetResourceSource
    {
        WNet,
        Net
    }

    public enum NetResourceType
    {
        Special,
        Disk,
        Print,
        Server,
        Network,
        Domain
    }
}
