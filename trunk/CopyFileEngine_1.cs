using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Threading;
using netCommander.FileSystemEx;
using System.ComponentModel;
using System.Runtime.InteropServices;

namespace netCommander
{
    /// <summary>
    /// self implementaion with dot net streams
    /// </summary>
    public class CopyFileEngine_1 : CopyFileEngineBase
    {
        //use read and write to framework streams

        public CopyFileEngine_1(string[] source, string destination, CopyEngineOptions options, CopyFileProgressDialog dialog)
            : base(source, destination, options, dialog)
        {

        }

        private int buffer_size;
        private int buffers_count;

        private long total_bytes_transferred = 0L;
        private int items_count = 0;
        private long total_size = 0L;
        private bool enable_total_progress = false;

        private byte[] buffer_sync = new byte[0];       //late init
        private StreamAsyncWriter buffers_async = null; //late init

        public override void DoCopy()
        {

            throw new NotImplementedException();
        }

        private object total_transferred_lock = new object();
        public long TotalTransferredSafe
        {
            get
            {
                long ret = 0L;
                lock (total_transferred_lock)
                {
                    ret = total_bytes_transferred;
                }
                return ret;
            }
            set
            {
                lock (total_transferred_lock)
                {
                    total_bytes_transferred = value;
                }
            }
        }

        private bool internal_is_same_volume(IntPtr h1, IntPtr h2)
        {
            BY_HANDLE_FILE_INFORMATION info1 = new BY_HANDLE_FILE_INFORMATION();
            BY_HANDLE_FILE_INFORMATION info2 = new BY_HANDLE_FILE_INFORMATION();
            int res = Native.GetFileInformationByHandle(h1, ref info1);
            if (res == 0)
            {
                return false;
            }
            res = Native.GetFileInformationByHandle(h2, ref info2);
            if (res == 0)
            {
                return false;
            }
            return (info1.dwVolumeSerialNumber == info2.dwVolumeSerialNumber);
        }

        private FileStream internal_create_source_stream(string file_name)
        {
            FileStream ret = null;
            IntPtr file_hanle = IntPtr.Zero;

            file_hanle = Native.CreateFile_intptr
                (file_name,
                Win32FileAccess.GENERIC_READ,
                FileShare.Read,
                IntPtr.Zero,
                FileMode.Open,
                CreateFileOptions.SEQUENTIAL_SCAN,
                IntPtr.Zero);
            if (file_hanle.ToInt32() == Native.INVALID_HANDLE_VALUE)
            {
                int win_err = Marshal.GetLastWin32Error();
                Win32Exception ex = new Win32Exception(win_err);
                throw ex;
            }

            try
            {
                ret = new FileStream
                (file_hanle,
                FileAccess.Read,
                true,
                buffer_size);
                return ret;
            }
            catch (Exception ex2)
            {
                Native.CloseHandle(file_hanle);
                throw ex2;
            }
        }

        private FileStream internal_create_destination_stream(string file_name, long file_size)
        {
            FileStream ret = null;
            IntPtr file_handle = IntPtr.Zero;

            file_handle = Native.CreateFile_intptr
                (file_name,
                Win32FileAccess.GENERIC_WRITE,
                FileShare.Read,
                IntPtr.Zero,
                (options & CopyEngineOptions.NoRewrite) == CopyEngineOptions.NoRewrite ? FileMode.CreateNew : FileMode.Create,
                CreateFileOptions.None,
                IntPtr.Zero);
            if (file_handle.ToInt32() == Native.INVALID_HANDLE_VALUE)
            {
                int win_err = Marshal.GetLastWin32Error();
                Win32Exception ex = new Win32Exception(win_err);
                throw ex;
            }

            try
            {
                ret = new FileStream
                (file_handle,
                FileAccess.Write,
                true,
                buffer_size);
                ret.SetLength(file_size);
                return ret;
            }
            catch (Exception ex2)
            {
                Native.CloseHandle(file_handle);
                throw ex2;
            }
        }

        private void copy_proc()
        {
            //now we have initial_source[] and destination

            //see initial_destination
            //initial_destination may be directory or file or file stream
            //and it may existent or no
            if (IOhelper.PathIsFileStream(initial_destination))
            {
                //copy_dest_file_stream();
            }


        }

        private void copy_recurs(string source_file, string destination_file)
        {

        }

        private void copy_destination_dir()
        {
            //i.e. existing directory
            //
            //copy all contens of inital_source INTO existing dir
            //
            for (int i = 0; i < initial_source.Length; i++)
            {
                if (AbortJob)
                {
                    break;
                }


            }

        }

        private void copy_destination_file()
        {
            //i.e. existing file
            //initial source must be ONE file
            //copy default and alternate file streams

            if (initial_source.Length > 1)
            {
                Messages.ShowMessage(string.Format
                    ("Wrong destination.\nCannot copy more then one file to file\n'{0}'.",
                    initial_destination));
                return;
            }

            WIN32_FIND_DATA src_data = new WIN32_FIND_DATA();
            if (!Wrapper.GetFileInfo(initial_source[0], ref src_data))
            {
                Messages.ShowMessage(string.Format
                    ("Cannot find source\n'{0}'.",
                    initial_source[0]));
                return;
            }

            if ((src_data.dwFileAttributes & FileAttributes.Directory) == FileAttributes.Directory)
            {
                Messages.ShowMessage("Cannot copy directory into existing file.");
                return;
            }

            string source = initial_source[0];
            string destination = initial_destination;
            string source_actual = string.Empty;
            string destination_actual = string.Empty;
            WrapperNT.FileStream_enum fs_enum = new WrapperNT.FileStream_enum(initial_source[0]);
            foreach (NT_FILE_STREAM_INFORMATION s_info in fs_enum)
            {
                if (s_info.StreamName == "::$DATA")
                {
                    //for default stream, skip stream name
                    source_actual = source;
                    destination_actual = destination;
                }
                else
                {
                    source_actual = source + s_info.StreamName;
                    destination_actual = destination + s_info.StreamName;
                }
                copy_one_item(source_actual, destination_actual);
            }
        }

        private void copy_destination_stream()
        {
            //initial source must be ONE file
            if (initial_source.Length > 1)
            {
                Messages.ShowMessage(string.Format
                    ("Wrong destination.\nCannot copy more then one file to file stream\n'{0}'.",
                    initial_destination));
                return;
            }

            WIN32_FIND_DATA src_data = new WIN32_FIND_DATA();
            if (!Wrapper.GetFileInfo(initial_source[0], ref src_data))
            {
                Messages.ShowMessage(string.Format
                    ("Cannot find source\n'{0}'.",
                    initial_source[0]));
                return;
            }

            if ((src_data.dwFileAttributes & FileAttributes.Directory) == FileAttributes.Directory)
            {
                Messages.ShowMessage("Cannot copy directory into file stream.");
                return;
            }

            copy_one_item(initial_source[0], initial_destination);
        }


        private void copy_one_item(string source, string destination)
        {
            FileStream src_stream = null;
            FileStream dest_stream = null;

            //try open source
            try
            {
                src_stream = internal_create_source_stream(source);
            }
            catch (Exception ex)
            {
                if (src_stream != null)
                {
                    src_stream.Close();
                }
                AbortJob = !process_error
                    (string.Format("Cannot open source file '{0}'", source),
                    ex);
                return;
            }

            //try open destination
            try
            {
                dest_stream = internal_create_destination_stream(destination, src_stream.Length);
            }
            catch (Exception ex)
            {
                src_stream.Close();
                if (dest_stream != null)
                {
                    dest_stream.Close();
                }
                AbortJob = !process_error
                    (string.Format("Cannot open destination file\n'{0}'.", destination),
                    ex);
                return;
            }

            CopyStreamItem copy_item = new CopyStreamItem();
            copy_item.BytesTransferred = 0;
            copy_item.DestinationName = destination;
            copy_item.DestinationStream = dest_stream;
            copy_item.SourceName = source;
            copy_item.SourceStream = src_stream;

            //which method use?
            if (internal_is_same_volume(src_stream.Handle, dest_stream.Handle))
            {
                //same volume - sync copy
                copy_io_stream_sync(copy_item);
            }
            else
            {
                //cross volumes - use async writer
                copy_io_stream_async(copy_item);
            }
        }

        private void copy_io_stream_sync(CopyStreamItem item)
        {
            if (buffer_sync.Length == 0)
            {
                buffer_sync = new byte[buffer_size];
            }

            int bytes_readed = 0;
            bool continue_copy = true;
            while (continue_copy)
            {
                bytes_readed = item.SourceStream.Read(buffer_sync, 0, buffer_size);

                //notifu chunk read

                item.DestinationStream.Write(buffer_sync, 0, bytes_readed);
                continue_copy = bytes_readed != 0;
                item.BytesTransferred += bytes_readed;

                //time to notify chunk write
            }
        }

        private void copy_io_stream_async(CopyStreamItem item)
        {
            if (buffer_sync == null)
            {
                buffers_async = new StreamAsyncWriter();
                buffers_async.WriteChunkCallback = new WriteChunk(write_chunk_finish_async);
                buffers_async.PrepareBuffers(buffers_count, buffer_size);
            }

            int bytes_readed = 0;
            bool continue_copy = true;
            while (continue_copy)
            {
                StreamWriterBuffer buf = buffers_async.WaitForFreeBuffer();
                bytes_readed = item.SourceStream.Read(buf.buffer, 0, buffer_size);
                buf.ActualBytes = bytes_readed;
                buf.DestinationStream = item.DestinationStream;
                buf.DestinationName = item.DestinationName;
                buffers_async.Enqueue(buf);

                //notify chunk read finish

                continue_copy = bytes_readed != 0;
            }
        }

        private void write_chunk_finish_async(string destination, int chunk_len)
        {
            //will be call from write thread from buffers_async!
            TotalTransferredSafe = TotalTransferredSafe + chunk_len;

            //now notify chunk write finish

        }

    }//end class

    public class StreamAsyncWriter : IDisposable
    {
        private Queue<StreamWriterBuffer> queue_write = new Queue<StreamWriterBuffer>();
        private object queue_write_lock = new object();
        private Queue<StreamWriterBuffer> queue_free = new Queue<StreamWriterBuffer>();
        private object queue_free_lock = new object();
        private EventWaitHandle wait_queue_free_ready = new EventWaitHandle(false, EventResetMode.AutoReset);
        private EventWaitHandle wait_queque_write_ready = new EventWaitHandle(false, EventResetMode.AutoReset);

        public StreamAsyncWriter()
        {
            Thread write_thread = new Thread(internal_write);
            write_thread.Start();
        }

        public WriteChunk WriteChunkCallback;

        public int ActiveBuffersCount
        {
            get
            {
                int ret = 0;
                lock (queue_write_lock)
                {
                    ret = queue_write.Count;
                }
                return ret;
            }
        }

        public int FreeBuffersCount
        {
            get
            {
                int ret = 0;
                lock (queue_free_lock)
                {
                    ret = queue_free.Count;
                }
                return ret;
            }
        }

        public StreamWriterBuffer WaitForFreeBuffer()
        {
            wait_queue_free_ready.Reset();
            if (FreeBuffersCount == 0)
            {
                wait_queue_free_ready.WaitOne();
            }
            StreamWriterBuffer ret = null;
            lock (queue_free_lock)
            {
                ret = queue_free.Dequeue();
            }
            return ret;
        }

        public void Enqueue(StreamWriterBuffer chunk)
        {
            lock (queue_write_lock)
            {
                queue_write.Enqueue(chunk);
            }
            //allow write thread proceed
            wait_queque_write_ready.Set();
        }

        public void PrepareBuffers(int buffers_count, int buffer_size)
        {
            for (int i = 0; i < buffers_count; i++)
            {
                StreamWriterBuffer new_buf = new StreamWriterBuffer(buffer_size);
                lock (queue_free_lock)
                {
                    queue_free.Enqueue(new_buf);
                }
            }
            wait_queue_free_ready.Set();
        }

        private StreamWriterBuffer WaitForReadyBuffer()
        {
            wait_queque_write_ready.Reset();
            StreamWriterBuffer ret = null;
            if (ActiveBuffersCount == 0)
            {
                wait_queque_write_ready.WaitOne();
            }
            lock (queue_write_lock)
            {
                ret = queue_write.Dequeue();
            }
            return ret;
        }

        private void ReleaseBuffer(StreamWriterBuffer chunk)
        {
            lock (queue_free_lock)
            {
                queue_free.Enqueue(chunk);
            }
            wait_queue_free_ready.Set();
        }

        private void internal_write()
        {
            while (true)
            {
                StreamWriterBuffer chunk = WaitForReadyBuffer();
                if (chunk.DestinationStream == null)
                {
                    break;
                }
                chunk.DestinationStream.Write(chunk.buffer, 0, chunk.ActualBytes);
                ReleaseBuffer(chunk);
                if (WriteChunkCallback != null)
                {
                    WriteChunkCallback(chunk.DestinationName, chunk.ActualBytes);
                }
            }//end while
        }


        #region IDisposable Members

        public void Dispose()
        {
            //add to write queue buffer with destination_stream=null
            //when write thread dequeue such buffer, write thread terminates
            StreamWriterBuffer fake_buffer = new StreamWriterBuffer(0);
            Enqueue(fake_buffer);
        }

        #endregion
    }

    public class CopyStreamItem
    {
        public string SourceName { get; set; }
        public string DestinationName { get; set; }
        public Stream SourceStream { get; set; }
        public Stream DestinationStream { get; set; }
        public long BytesTransferred { get; set; }
    }

    public class StreamWriterBuffer
    {
        public byte[] buffer;
        public string DestinationName { get; set; }
        public Stream DestinationStream { get; set; }
        public int ActualBytes { get; set; }
        public int BufferSize { get; private set; }

        public StreamWriterBuffer(int buffer_size)
        {
            BufferSize = buffer_size;
            buffer = new byte[buffer_size];
            ActualBytes = 0;
            DestinationStream = null;
            DestinationName = string.Empty;
        }


    }

    public class CopyChunkTransferredEventArgs : EventArgs
    {
        public CopyStreamItem CopyItem { get; private set; }

        public CopyChunkTransferredEventArgs(CopyStreamItem item)
        {
            CopyItem = item;
        }
    }
    public delegate void CopyChunkTransferredEventHandler(object sender, CopyChunkTransferredEventArgs e);
    public delegate void WriteChunk(string destinaton_name, int chunk_length);
}

