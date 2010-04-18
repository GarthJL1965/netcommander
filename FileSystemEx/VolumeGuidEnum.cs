using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;
using System.ComponentModel;

namespace netCommander.FileSystemEx
{
    public class VolumeGuidEnumerable : IEnumerable<string>
    {
        #region IEnumerable<string> Members

        public IEnumerator<string> GetEnumerator()
        {
            return new InternalEnumerator();
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
            private IntPtr volume_guid_buffer = IntPtr.Zero;
            private int volume_guid_buffer_len_bytes = (WinApiFS.MAX_PATH + 1) * Marshal.SystemDefaultCharSize;
            private int volume_guid_buffer_len_tchars = WinApiFS.MAX_PATH + 1;

            public InternalEnumerator()
            {
                volume_guid_buffer = Marshal.AllocHGlobal(volume_guid_buffer_len_bytes);
            }

            #region IEnumerator<string> Members

            public string Current
            {
                get { return Marshal.PtrToStringAuto(volume_guid_buffer); }
            }

            #endregion

            #region IDisposable Members

            public void Dispose()
            {
                if (volume_guid_buffer != IntPtr.Zero)
                {
                    Marshal.FreeHGlobal(volume_guid_buffer);
                }
                WinApiFS.FindVolumeClose(search_handle);
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
                    search_handle = WinApiFS.FindFirstVolume(volume_guid_buffer, volume_guid_buffer_len_tchars);
                    if (search_handle.ToInt32() == WinApiFS.INVALID_HANDLE_VALUE)
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
                else
                {
                    res = WinApiFS.FindNextVolume(search_handle, volume_guid_buffer, volume_guid_buffer_len_tchars);
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
