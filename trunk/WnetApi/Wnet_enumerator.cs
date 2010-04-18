using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;
using System.ComponentModel;
using System.Runtime.InteropServices;

namespace netCommander.WNet
{
    class WnetResourceEnumerator : IEnumerable<NETRESOURCE>
    {
        public WnetResourceEnumerator
            (NETRESOURCE resource_root,
            ResourceScope scope,
            ResourceType type,
            ResourceUsage usage)
        {
            internal_enumerator_holder = new InternalEnumerator
            (resource_root,
            scope,
            type,
            usage);
        }

        private InternalEnumerator internal_enumerator_holder = null;

        #region IEnumerable<NETRESOURCE> Members

        public IEnumerator<NETRESOURCE> GetEnumerator()
        {
            return internal_enumerator_holder;
        }

        #endregion

        #region IEnumerable Members

        IEnumerator IEnumerable.GetEnumerator()
        {
            return internal_enumerator_holder;
        }

        #endregion

        private class InternalEnumerator : IEnumerator<NETRESOURCE>
        {
            private NETRESOURCE resource_root = new NETRESOURCE();
            private ResourceScope inittial_scope;
            private ResourceType initial_type;
            private ResourceUsage initial_usage;
            private IntPtr enum_handle = IntPtr.Zero;

            private IntPtr buffer = IntPtr.Zero;
            private int buffer_size = 256;
            private int requested_entries = 1;

            private int current_entries_in_buffer = 0;
            private int current_entries_in_buffer_readed = 0;


            public InternalEnumerator
                (NETRESOURCE resource_root,
                ResourceScope scope,
                ResourceType type,
                ResourceUsage usage)
            {
                this.resource_root = resource_root;
                inittial_scope = scope;
                initial_type = type;
                initial_usage = usage;
                open_enum();
            }

            private void open_enum()
            {
                uint res = 0;

                res = WinApiWNET.WNetOpenEnum
                    (inittial_scope,
                    initial_type,
                    initial_usage,
                    ref resource_root,
                    ref enum_handle);

                //check result
                if (res != WinApiWNET.NO_ERROR)
                {
                    if (res == WinApiWNET.ERROR_EXTENDED_ERROR)
                    {
                        WNetException.ThrowOnError();
                    }
                    else
                    {
                        throw new Win32Exception((int)res);
                    }
                }
                //allocate buffer
                buffer = Marshal.AllocHGlobal(buffer_size);
            }

            private bool continue_enum()
            {
                int req_entries=requested_entries;

                uint res = WinApiWNET.WNetEnumResource
                    (enum_handle,
                    ref req_entries,
                    buffer,
                    ref buffer_size);
                //check result
                switch (res)
                {
                    case WinApiWNET.NO_ERROR:
                        //success
                        current_entries_in_buffer = req_entries;
                        current_entries_in_buffer_readed = 0;
                        return true;

                    case WinApiWNET.ERROR_NO_MORE_ITEMS:
                        //enumerate comletes
                        current_entries_in_buffer = 0;
                        current_entries_in_buffer_readed = 0;
                        return false;

                    case WinApiWNET.ERROR_MORE_DATA:
                        //buffer small?
                        //allocate new buffer
                        Marshal.FreeHGlobal(buffer);
                        buffer = Marshal.AllocHGlobal(buffer_size);
                        //and recall
                        return continue_enum();

                    case WinApiWNET.ERROR_EXTENDED_ERROR:
                        WNetException.ThrowOnError();
                        return false;

                    default:
                        throw new Win32Exception((int)res);
                        //return false;
                }

            }

            #region IEnumerator<NETRESOURCE> Members

            public NETRESOURCE Current
            {
                get
                {
                    NETRESOURCE ret = new NETRESOURCE();
                    NETRESOURCE.FromBuffer(buffer, current_entries_in_buffer_readed, ref ret);
                    current_entries_in_buffer_readed++;
                    return ret;
                }
            }

            #endregion

            #region IDisposable Members

            public void Dispose()
            {
                if (buffer != IntPtr.Zero)
                {
                    Marshal.FreeHGlobal(buffer);
                }
                if (enum_handle != IntPtr.Zero)
                {
                    WinApiWNET.WNetCloseEnum(enum_handle);
                }
            }

            #endregion

            #region IEnumerator Members

            object IEnumerator.Current
            {
                get { return Current; }
            }

            public bool MoveNext()
            {
                if (current_entries_in_buffer > current_entries_in_buffer_readed)
                {
                    //read next from previously processed buffer
                    return true;
                }

                //else fill buffer
                return continue_enum();
            }

            public void Reset()
            {
                throw new NotImplementedException();
            }

            #endregion
        }
    }
}
