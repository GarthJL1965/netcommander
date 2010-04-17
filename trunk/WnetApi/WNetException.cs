using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;

namespace netCommander.WNet
{
    class WNetException
    {
        private const int buffer_size = 256;

        private WNetException()
        {
        }

        public static void ThrowOnError()
        {
            uint errCode = 0;
            IntPtr errBuffer = IntPtr.Zero;
            IntPtr nameBuffer = IntPtr.Zero;
            ApplicationException netEx = null;
            try
            {
                errBuffer = Marshal.AllocHGlobal(buffer_size * Marshal.SystemDefaultCharSize);
                nameBuffer = Marshal.AllocHGlobal(buffer_size * Marshal.SystemDefaultCharSize);
                uint res = WinApiWNET.WNetGetLastError
                    (ref errCode,
                    errBuffer,
                    buffer_size,
                    nameBuffer,
                    buffer_size);
                if (res == WinApiWNET.NO_ERROR)
                {
                    netEx = new ApplicationException(Marshal.PtrToStringAuto(errBuffer));
                    netEx.Source = Marshal.PtrToStringAuto(nameBuffer);
                    throw netEx;
                }
                else
                {
                    throw new System.ComponentModel.Win32Exception(Marshal.GetLastWin32Error());
                }
            }
            finally
            {
                if (errBuffer != IntPtr.Zero)
                {
                    Marshal.FreeHGlobal(errBuffer);
                }
                if (nameBuffer != IntPtr.Zero)
                {
                    Marshal.FreeHGlobal(nameBuffer);
                }
            }
        }
    }
}
