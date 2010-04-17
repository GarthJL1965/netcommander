using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;
using System.ComponentModel;

namespace netCommander.FileSystemEx
{
    class VolumeMountPointEnumerable : IEnumerable<string>
    {
        private string volume_guid = string.Empty;

        public VolumeMountPointEnumerable(string VolumeGuid)
        {
            volume_guid = VolumeGuid;
        }

        #region IEnumerable<string> Members

        public IEnumerator<string> GetEnumerator()
        {
            return new InternalEnumerator(volume_guid);
        }

        #endregion

        #region IEnumerable Members

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        #endregion

        private class InternalEnumerator : IEnumerator<string>
        {
            private IntPtr search_handle = IntPtr.Zero;
            private IntPtr mount_point_buffer = IntPtr.Zero;
            private int mount_point_buffer_len_bytes = (WinApiFS.MAX_PATH + 1) * Marshal.SystemDefaultCharSize;
            private int mount_point_buffer_len_tchars = WinApiFS.MAX_PATH + 1;
            private string volume_guid = string.Empty;

            public InternalEnumerator(string volume_guid)
            {
                this.volume_guid = volume_guid;
                mount_point_buffer = Marshal.AllocHGlobal(mount_point_buffer_len_bytes);
            }

            #region IEnumerator<string> Members

            public string Current
            {
                get { return Marshal.PtrToStringAuto(mount_point_buffer); }
            }

            #endregion

            #region IDisposable Members

            public void Dispose()
            {
                if (mount_point_buffer != IntPtr.Zero)
                {
                    Marshal.FreeHGlobal(mount_point_buffer);
                }
                WinApiFS.FindVolumeMountPointClose(search_handle);
            }

            #endregion

            #region IEnumerator Members

            object System.Collections.IEnumerator.Current
            {
                get { return Current; }
            }

            public bool MoveNext()
            {
                int err_code = 0;
                int res = 0;
                if (search_handle == IntPtr.Zero)
                {
                    search_handle = WinApiFS.FindFirstVolumeMountPoint(volume_guid, mount_point_buffer, mount_point_buffer_len_tchars);
                    if (search_handle.ToInt32() == WinApiFS.INVALID_HANDLE_VALUE)
                    {
                        err_code = Marshal.GetLastWin32Error();
                        if (err_code == 18)
                        {
                            //no more files
                            return false;
                        }
                        else if (err_code == 21)
                        {
                            //device not ready
                            return false;
                        }
                        else
                        {
                            throw new Win32Exception(err_code);
                        }
                    }
                    else
                    {
                        return true;
                    }
                }
                else
                {
                    res = WinApiFS.FindNextVolumeMountPoint(search_handle, mount_point_buffer, mount_point_buffer_len_tchars);
                    if (res == 0)
                    {
                        err_code = Marshal.GetLastWin32Error();
                        if (err_code == 18)
                        {
                            //no more files
                            return false;
                        }
                        else
                        {
                            throw new Win32Exception(err_code);
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
}
