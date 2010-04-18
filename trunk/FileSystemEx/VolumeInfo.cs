using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;
using System.IO;
using System.ComponentModel;

namespace netCommander.FileSystemEx
{
    public class VolumeInfo
    {

        public VolumeInfo(string RootPathName)
        {
            this.RootPathName = RootPathName;
            //initInfo(RootPathName);
        }

        public VolumeInfo(string RootPathName, bool force_fill)
            : this(RootPathName)
        {
            if (force_fill)
            {
                initInfo();
            }
        }

        private bool ready = false;

        private void initInfo()
        {
            if (ready)
            {
                return;
            }

            string root = RootPathName;
            int nameBufferSize = (WinApiFS.MAX_PATH + 1) * Marshal.SystemDefaultCharSize;
            int nameCharsCount = WinApiFS.MAX_PATH + 1;
            IntPtr ptVolumeNameBuffer=Marshal.AllocHGlobal(nameBufferSize);
            IntPtr ptVolumeSerialNumber=Marshal.AllocHGlobal(4);
            IntPtr ptMaximumComponentLength=Marshal.AllocHGlobal(4);
            IntPtr ptFileSystemFlags=Marshal.AllocHGlobal(4);
            IntPtr ptFileSystemName=Marshal.AllocHGlobal(nameBufferSize);
            try
            {
                int res = WinApiFS.GetVolumeInformation
                    (root,
                    ptVolumeNameBuffer,
                    nameCharsCount,
                    ptVolumeSerialNumber,
                    ptMaximumComponentLength,
                    ptFileSystemFlags,
                    ptFileSystemName,
                    nameCharsCount);
                if (res == 0) //not process 'device not ready (21)', 'network drive not found (53) 'path not found (3)'
                {
                    int errCode = Marshal.GetLastWin32Error();
                    if (errCode == 21)
                    {
                        DeviceReady = false;
                        ready = true;
                    }
                    else if (errCode == 53)
                    {
                        DeviceReady = false;
                        ready = true;
                    }
                    else if (errCode == 3)
                    {
                        DeviceReady = false;
                        ready = true;
                    }
                    else
                    {
                        Messages.ShowException
                            (new Win32Exception(errCode),
                            string.Format
                            ("Failed get volume {0} properties.",
                            root));
                        DeviceReady = false;
                        ready = true;
                    }
                }
                else
                {
                    DeviceReady = true;

                    RootPathName = root;
                    VolumeName = Marshal.PtrToStringAuto(ptVolumeNameBuffer);
                    MaximumComponentLength = Marshal.ReadInt32(ptMaximumComponentLength);
                    FileSystemFlags = (VolumeCaps)Marshal.ReadInt32(ptFileSystemFlags);
                    FileSystemName = Marshal.PtrToStringAuto(ptFileSystemName);

                    byte[] serialBytes = new byte[4];
                    Marshal.Copy(ptVolumeSerialNumber, serialBytes, 0, 4);
                    SerialNumber = BitConverter.ToUInt32(serialBytes, 0);
                }

                if (DeviceReady)
                {
                    VolumeSpaceInfo = VolumeSpaceInfo.GetInfo(RootPathName);
                }
                else
                {
                    VolumeSpaceInfo = new VolumeSpaceInfo();
                }

                DriveType = WinApiFS.GetDriveType(RootPathName);
            }
            finally
            {
                Marshal.FreeHGlobal(ptFileSystemFlags);
                Marshal.FreeHGlobal(ptFileSystemName);
                Marshal.FreeHGlobal(ptMaximumComponentLength);
                Marshal.FreeHGlobal(ptVolumeNameBuffer);
                Marshal.FreeHGlobal(ptVolumeSerialNumber);

                ready = true;
            }
        }

        public string RootPathName { get; private set; }

        public void Update()
        {
            ready = false;
        }

        public bool DeviceReady { get; private set; }

        private string _volumeName;
        public string VolumeName
        {
            get
            {
                initInfo();
                return _volumeName;
            }
            private set
            {
                _volumeName = value;
            }
        }

        private uint _serialNumber;
        public uint SerialNumber
        {
            get
            {
                initInfo();
                return _serialNumber;
            }
            private set
            {
                _serialNumber = value;
            }
        }

        private int _maximumComponentName;
        public int MaximumComponentLength
        {
            get
            {
                initInfo();
                return _maximumComponentName;
            }
            private set
            {
                _maximumComponentName = value;
            }
        }

        private VolumeCaps _fileSystemFlags;
        public VolumeCaps FileSystemFlags
        {
            get
            {
                initInfo();
                return _fileSystemFlags;
            }
            private set
            {
                _fileSystemFlags = value;
            }
        }

        private string _fileSystemName;
        public string FileSystemName
        {
            get
            {
                initInfo();
                return _fileSystemName;
            }
            private set
            {
                _fileSystemName = value;
            }
        }

        private VolumeSpaceInfo _volumeSpaceInfo;
        public VolumeSpaceInfo VolumeSpaceInfo
        {
            get
            {
                initInfo();
                return _volumeSpaceInfo;
            }
            set
            {
                _volumeSpaceInfo = value;
            }
        }

        private DriveType _driveType;
        public DriveType DriveType
        {
            get
            {
                initInfo();
                return _driveType;
            }
            set
            {
                _driveType = value;
            }
        }
    }

    public struct VolumeSpaceInfo
    {
        public ulong FreeBytesAvailable;
        public ulong TotalNumberOfBytes;
        public ulong TotalNumberOfFreeBytes;

        public static VolumeSpaceInfo GetInfo(string directory)
        {
            VolumeSpaceInfo ret = new VolumeSpaceInfo();

            int res = WinApiFS.GetDiskFreeSpaceEx(directory, ref ret.FreeBytesAvailable, ref ret.TotalNumberOfBytes, ref ret.TotalNumberOfFreeBytes);
            if (res == 0)
            {
                int winErr = Marshal.GetLastWin32Error();
                throw new System.ComponentModel.Win32Exception(winErr);
            }

            return ret;
        }
    }
}
