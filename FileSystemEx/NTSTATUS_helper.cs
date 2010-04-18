using System;
using System.Collections.Generic;
using System.Text;

namespace netCommander.FileSystemEx
{
    public class ExceptionNT : Exception
    {
        public uint NT_status { get; private set; }
        public IO_STATUS_BLOCK NT_status_block { get; private set; }


        public ExceptionNT(string message,uint nt_status)
            :base(message)
        {
            NT_status = nt_status;
        }

        public ExceptionNT(string message,uint nt_status,IO_STATUS_BLOCK status_block)
            :base(message)
        {
            NT_status = nt_status;
            NT_status_block = status_block;
        }
    }

    public class NTSTATUS_helper
    {
        public const uint STATUS_SUCCESS = 0x00000000;

        private NTSTATUS_helper() { }

        public static NTSTATUS_severity GetSeverity(uint nt_status)
        {
            if (nt_status < 0x40000000)
            {
                return NTSTATUS_severity.Success;
            }
            if (nt_status < 0x80000000)
            {
                return NTSTATUS_severity.Information;
            }
            if (nt_status < 0xC0000000)
            {
                return NTSTATUS_severity.Warning;
            }
            return NTSTATUS_severity.Error;
        }

        public static void ThrowOnError(uint nt_status, IO_STATUS_BLOCK status_block, NTSTATUS_severity throw_severity)
        {
            if (nt_status == STATUS_SUCCESS)
            {
                return;
            }

            NTSTATUS_severity sever = GetSeverity(nt_status);
            if (sever >= throw_severity)
            {
                string message = string.Format("NT kernel exception. Returns status 0x{0:X}.", nt_status);
                if (Enum.IsDefined(typeof(NTSTATUS_known), nt_status))
                {
                    message = message + " " + ((NTSTATUS_known)nt_status).ToString();
                }
                ExceptionNT ex_nt = new ExceptionNT
                (message, nt_status, status_block);
                ex_nt.Source = "kernel";
                throw ex_nt;
            }

        }

        public static void ThrowOnError(uint nt_status, NTSTATUS_severity throw_severity)
        {
            if (nt_status == STATUS_SUCCESS)
            {
                return;
            }

            NTSTATUS_severity sever = GetSeverity(nt_status);
            if (sever >= throw_severity)
            {
                string message = string.Format("NT kernel exception. Returns status 0x{0:X}.", nt_status);
                if (Enum.IsDefined(typeof(NTSTATUS_known), nt_status))
                {
                    message = message + " " + ((NTSTATUS_known)nt_status).ToString();
                }
                ExceptionNT ex_nt = new ExceptionNT
                (message, nt_status);
                ex_nt.Source = "kernel";
                throw ex_nt;
            }

        }
    }

    public enum NTSTATUS_severity
    {
        Success,
        Information,
        Warning,
        Error
    }

    public enum NTSTATUS_known : uint
    {
        STATUS_BUFFER_OVERFLOW = 0x80000005,
        STATUS_NO_MORE_FILES = 0x80000006,
        STATUS_DEVICE_BUSY = 0x80000011,
        STATUS_NO_MORE_EAS = 0x80000012,
        STATUS_NO_MORE_ENTRIES = 0x8000001A,
        STATUS_MEDIA_CHANGED = 0x8000001C,
        STATUS_FLT_BUFFER_TOO_SMALL = 0x801C0001,       //возможно 0x401C0001
        STATUS_UNSUCCESSFUL = 0xC0000001,
        STATUS_NOT_IMPLEMENTED = 0xC0000002,
        STATUS_INVALID_INFO_CLASS = 0xC0000003,
        STATUS_INFO_LENGTH_MISMATCH = 0xC0000004,
        STATUS_ACCESS_VIOLATION = 0xC0000005,
        STATUS_INVALID_HANDLE = 0xC0000008,
        STATUS_INVALID_PARAMETER = 0xC000000D,
        STATUS_NO_SUCH_DEVICE = 0xC000000E,
        STATUS_NO_SUCH_FILE = 0xC000000F,
        STATUS_INVALID_DEVICE_REQUEST = 0xC0000010,
        STATUS_ACCESS_DENIED = 0xC0000022,
        STATUS_BUFFER_TOO_SMALL = 0xC0000023,
        STATUS_DISK_CORRUPT_ERROR = 0xC0000032,
        STATUS_OBJECT_NAME_INVALID = 0xC0000033,
        STATUS_OBJECT_NAME_NOT_FOUND = 0xC0000034,
        STATUS_OBJECT_PATH_INVALID = 0xC0000039,
        STATUS_OBJECT_PATH_NOT_FOUND = 0xC000003A,
        STATUS_OBJECT_PATH_SYNTAX_BAD = 0xC000003B
    }
}
