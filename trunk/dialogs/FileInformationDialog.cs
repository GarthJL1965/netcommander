using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.IO;
using netCommander.FileSystemEx;
using System.Runtime.InteropServices;
using System.Security.AccessControl;

namespace netCommander
{
    public partial class FileInformationDialog : Form
    {
        public FileInformationDialog()
        {
            InitializeComponent();
            set_lang();
        }

        private void set_lang()
        {
            if (LicenseManager.UsageMode == LicenseUsageMode.Designtime)
            {
                return;
            }

            tabPage1.Text = Options.GetLiteral(Options.LANG_STANDARS_INFORMATION);
            label1.Text = Options.GetLiteral(Options.LANG_FILE_NAME);
            label2.Text = Options.GetLiteral(Options.LANG_FILE_ALTERNATE_NAME);
            label3.Text = Options.GetLiteral(Options.LANG_FILE_ATTRIBUTES);
            label8.Text = Options.GetLiteral(Options.LANG_FILE_SIZE);
            label9.Text = Options.GetLiteral(Options.LANG_FILE_ALLOCATION_SIZE);
            label30.Text = Options.GetLiteral(Options.LANG_SUBDIRS_SIZE_ALLOCATION);
            label29.Text = Options.GetLiteral(Options.LANG_SUBDIRS_FILES_DIRS_COUNT);
            label4.Text = Options.GetLiteral(Options.LANG_CREATE_TIME);
            label5.Text = Options.GetLiteral(Options.LANG_MODIFICATION_TIME);
            label6.Text = Options.GetLiteral(Options.LANG_CHANGE_TIME);
            label7.Text = Options.GetLiteral(Options.LANG_ACCESS_TIME);
            buttonClose.Text = Options.GetLiteral(Options.LANG_CLOSE);
            tabPage2.Text = Options.GetLiteral(Options.LANG_FILE_STREAMS);
            tabPage5.Text = Options.GetLiteral(Options.LANG_LINKS);
            label10.Text = Options.GetLiteral(Options.LANG_REPARSE_TAG);
            label11.Text = Options.GetLiteral(Options.LANG_LINKS_HARD_COUNT);
            label27.Text = Options.GetLiteral(Options.LANG_LINK_TARGET);
            tabPage3.Text = Options.GetLiteral(Options.LANG_FILE_SECURITY_ATTRIBUTES);
            //tabPage4.Text = Options.GetLiteral(Options.LANG_FILE_VOLUME_INFORMATION);
            //label12.Text = Options.GetLiteral(Options.LANG_VOLUME_LABEL);
            //label13.Text = Options.GetLiteral(Options.LANG_FILESYSTEM_TYPE);
            //label14.Text = Options.GetLiteral(Options.LANG_FILESYATEM_CAPS);
            //label15.Text = Options.GetLiteral(Options.LANG_FILESYSTEM_NAME_LEN);
            //label16.Text = Options.GetLiteral(Options.LANG_VOLUME_QUOTA_LIMIT);
            //label25.Text = Options.GetLiteral(Options.LANG_VOLUME_QUOTA_THRESHOLD);
            //label17.Text = Options.GetLiteral(Options.LANG_VOLUME_CONTROL_FLAGS);
            //label18.Text = Options.GetLiteral(Options.LANG_VOLUME_AVAILABLE_UNITS);
            //label26.Text = Options.GetLiteral(Options.LANG_BYTES);
            //label20.Text = Options.GetLiteral(Options.LANG_VOLUME_BYTES_PER_UNIT);
            //label19.Text = Options.GetLiteral(Options.LANG_VOLUME_PER_SECTOR);
            //label21.Text = Options.GetLiteral(Options.LANG_CREATE_TIME);
            //label22.Text = Options.GetLiteral(Options.LANG_VOLUME_SERIAL_NUMBER);
            //label23.Text = Options.GetLiteral(Options.LANG_VOLUME_DEVICE_TYPE);
            //label24.Text = Options.GetLiteral(Options.LANG_VOLUME_DEVICE_CHARACTERISTICS);

        }

        public string FileName { get; private set; }
        private bool is_directory = false;

        public void FillContents(string file_name)
        {
            FileName = file_name;
            Text = file_name;

            is_directory = IOhelper.IsDirectory(file_name);

            FillStandardPage(file_name);            
        }

        private void fill_standard_info()
        {
            IntPtr h_file = IntPtr.Zero;

            try
            {
                h_file = WinAPiFSwrapper.CreateFileHandle
                    (FileName,
                    Win32FileAccess.READ_ATTRIBUTES | Win32FileAccess.READ_EA,
                    FileShare.ReadWrite | FileShare.Delete,
                    FileMode.Open,
                    is_directory ? CreateFileOptions.BACKUP_SEMANTICS : CreateFileOptions.None);

                NT_FILE_STANDARD_INFORMATION s_info = ntApiFSwrapper.GetFileInfo_standard(h_file);

                

                textBoxSize.Text = string.Format
                (Options.GetLiteral(Options.LANG_0_BYTES),
                s_info.EndOfFile);
                textBoxAllocationSize.Text = string.Format
                    (Options.GetLiteral(Options.LANG_0_BYTES),
                    s_info.AllocationSize);
                textBoxHardLinks.Text = s_info.NumberOfLinks.ToString();
            }
            catch (Exception ex)
            {
                textBoxSize.Text = ex.Message;
            }
            finally
            {
                if ((h_file.ToInt32() != WinApiFS.INVALID_HANDLE_VALUE) && (h_file != IntPtr.Zero))
                {
                    WinApiFS.CloseHandle(h_file);
                }
            }
        }

        private void fill_basic_info()
        {
            IntPtr h_file = IntPtr.Zero;

            try
            {
                h_file = WinAPiFSwrapper.CreateFileHandle
                    (FileName,
                    Win32FileAccess.READ_ATTRIBUTES | Win32FileAccess.READ_EA,
                    FileShare.ReadWrite | FileShare.Delete,
                    FileMode.Open,
                    is_directory ? CreateFileOptions.BACKUP_SEMANTICS : CreateFileOptions.None);

                NT_FILE_BASIC_INFO b_info = ntApiFSwrapper.GetFileInfo_basic(h_file);

                
                textBoxAccess.Text = string.Format
                    (Options.GetLiteral(Options.LANG_DATE_TIME_LONG_FORMAT),
                    b_info.LastAccessTime.ToShortDateString(),
                    b_info.LastAccessTime.ToLongTimeString());

                textBoxAccessFT.Text = string.Format
                    ("{0:N0}", b_info.LastAccessFileTime);

                textBoxChange.Text = string.Format
                    (Options.GetLiteral(Options.LANG_DATE_TIME_LONG_FORMAT),
                    b_info.ChangeTime.ToShortDateString(),
                    b_info.ChangeTime.ToLongTimeString());

                textBoxChangeFT.Text = string.Format
                    ("{0:N0}", b_info.ChangeFileTime);

                textBoxCreation.Text = string.Format
                    (Options.GetLiteral(Options.LANG_DATE_TIME_LONG_FORMAT),
                    b_info.CreationTime.ToShortDateString(),
                    b_info.CreationTime.ToLongTimeString());

                textBoxCreationFT.Text = string.Format
                    ("{0:N0}", b_info.CreationFileTime);

                textBoxWrite.Text = string.Format
                    (Options.GetLiteral(Options.LANG_DATE_TIME_LONG_FORMAT),
                    b_info.LastWriteTime.ToShortDateString(),
                    b_info.LastWriteTime.ToLongTimeString());

                textBoxWriteFT.Text = string.Format
                    ("{0:N0}", b_info.LastWriteFileTime);
            }
            catch (Exception ex)
            {
                textBoxAccess.Text = ex.Message;
            }
            finally
            {
                if ((h_file.ToInt32() != WinApiFS.INVALID_HANDLE_VALUE) && (h_file != IntPtr.Zero))
                {
                    WinApiFS.CloseHandle(h_file);
                }
            }
        }

        //private void fill_volume_info()
        //{
        //    IntPtr h_file = IntPtr.Zero;

        //    try
        //    {
        //        h_file = WinAPiFSwrapper.CreateFileHandle
        //            (FileName,
        //            Win32FileAccess.READ_ATTRIBUTES | Win32FileAccess.READ_EA,
        //            FileShare.ReadWrite | FileShare.Delete,
        //            FileMode.Open,
        //            is_directory ? CreateFileOptions.BACKUP_SEMANTICS : CreateFileOptions.None);

        //        FILE_FS_VOLUME_INFORMATION vol_info = ntApiFSwrapper.GetFileVolumeInfo(h_file);
        //        textBoxVolumeCreationTime.Text = string.Format
        //            (Options.GetLiteral(Options.LANG_DATE_TIME_LONG_FORMAT),
        //            vol_info.VolumeCreationDateTime.ToShortDateString(),
        //            vol_info.VolumeCreationDateTime.ToLongTimeString(),
        //            vol_info.VolumeCreationDateTime.Millisecond);
        //        textBoxVolumeLabel.Text = vol_info.VolumeLabel;
        //        textBoxVolumeSerialNumber.Text = vol_info.VolumeSerialNumber.ToString();
        //    }
        //    catch (Exception ex)
        //    {
        //        textBoxVolumeLabel.Text = ex.Message;
        //    }
        //    finally
        //    {
        //        if ((h_file.ToInt32() != WinApiFS.INVALID_HANDLE_VALUE) && (h_file != IntPtr.Zero))
        //        {
        //            WinApiFS.CloseHandle(h_file);
        //        }
        //    }
        //}

        ////private void fill_volume_attr()
        ////{
        ////    IntPtr h_file = IntPtr.Zero;


        ////    try
        ////    {
        ////        h_file = WinAPiFSwrapper.CreateFileHandle
        ////            (FileName,
        ////            Win32FileAccess.READ_ATTRIBUTES | Win32FileAccess.READ_EA | Win32FileAccess.LIST_DIRECTORY,
        ////            FileShare.ReadWrite | FileShare.Delete,
        ////            FileMode.Open,
        ////            is_directory ? CreateFileOptions.BACKUP_SEMANTICS : CreateFileOptions.None);

        ////        FILE_FS_ATTRIBUTE_INFORMATION vol_attr = ntApiFSwrapper.GetFileVolumeAttributeInfo(h_file, IntPtr.Zero, 0);
        ////        textBoxVolumeFScaps.Text = vol_attr.VolumeAttributes.ToString();
        ////        textBoxVolumeFStype.Text = vol_attr.FileSystemName;
        ////        textBoxVolumeMaxNameLen.Text = vol_attr.MaximumComponentNameLength.ToString();

        ////        if ((vol_attr.VolumeAttributes & VolumeCaps.ReadOnly) != VolumeCaps.ReadOnly)
        ////        {
        ////            try
        ////            {
        ////                FILE_FS_FULLSIZE_INFORMATION vol_fs = ntApiFSwrapper.GetFileVolumeFullsizeInfo(h_file);
        ////                textBoxVolumeAvailableUnits.Text = string.Format
        ////                    ("{0:N0}",
        ////                    vol_fs.CallerAvailableAllocationUnits);
        ////                textBoxVolumeAvailableBytes.Text = string.Format
        ////                    ("{0:N0}",
        ////                    vol_fs.UserAvailableAllocationBytes);
        ////                textBoxVolumeBytesPerSector.Text = string.Format
        ////                    ("{0:N0}",
        ////                    vol_fs.BytesPerSector);
        ////                textBoxVolumeBytesPerSector.Text = string.Format
        ////                    ("{0:N0}",
        ////                    vol_fs.BytesPerSector);
        ////                textBoxVolumeBytesPerUnit.Text = string.Format
        ////                   ("{0:N0}",
        ////                   vol_fs.BytesPerSector * vol_fs.SectorsPerAllocationUnit);
        ////            }
        ////            catch (Exception ex)
        ////            {
        ////                textBoxVolumeAvailableUnits.Text = ex.Message;
        ////            }
        ////        }

        ////        if ((vol_attr.VolumeAttributes & VolumeCaps.Quotas) == VolumeCaps.Quotas)
        ////        {
        ////            try
        ////            {
        ////                FILE_FS_CONTROL_INFORMATION vol_ctrl = ntApiFSwrapper.GetFileVolumeControlInfo(h_file);
        ////                textBoxVolumeControlFlags.Text = vol_ctrl.FileSystemControlFlags.ToString();
        ////                textBoxVolumeQuotaLimit.Text = string.Format
        ////                    ("{0:N0}",
        ////                    vol_ctrl.DefaultQuotaLimit);
        ////                textBoxVolumeQuoteThreshold.Text = string.Format
        ////                    ("{0:N0}",
        ////                    vol_ctrl.DefaultQuotaThreshold);
        ////            }
        ////            catch (Exception ex)
        ////            {
        ////                textBoxVolumeControlFlags.Text = ex.Message;
        ////            }
        ////        }
        ////    }//try
        ////    catch (Exception ex)
        ////    {
        ////        textBoxVolumeFScaps.Text = ex.Message;
        ////    }
        ////    finally
        ////    {
        ////        if ((h_file.ToInt32() != WinApiFS.INVALID_HANDLE_VALUE) && (h_file != IntPtr.Zero))
        ////        {
        ////            WinApiFS.CloseHandle(h_file);
        ////        }
        ////    }
        ////}

        //private void fill_device()
        //{
        //    IntPtr h_file = IntPtr.Zero;

        //    try
        //    {
        //        h_file = WinAPiFSwrapper.CreateFileHandle
        //            (FileName,
        //            Win32FileAccess.READ_ATTRIBUTES | Win32FileAccess.READ_EA,
        //            FileShare.ReadWrite | FileShare.Delete,
        //            FileMode.Open,
        //            is_directory ? CreateFileOptions.BACKUP_SEMANTICS : CreateFileOptions.None);

        //        FILE_FS_DEVICE_INFORMATION vol_dev = ntApiFSwrapper.GetFileVolumeDeviceInfo(h_file);
        //        textBoxVolumeDeviceCharateristics.Text = vol_dev.Characteristics.ToString();
        //        textBoxVolumeDeviceType.Text = vol_dev.DeviceType.ToString();
        //    }
        //    catch (Exception ex)
        //    {
        //        textBoxVolumeDeviceCharateristics.Text = ex.Message;
        //    }
        //    finally
        //    {
        //        if ((h_file.ToInt32() != WinApiFS.INVALID_HANDLE_VALUE) && (h_file != IntPtr.Zero))
        //        {
        //            WinApiFS.CloseHandle(h_file);
        //        }
        //    }
        //}

        private void fill_links(IO_REPARSE_TAG tag)
        {
            try
            {
                switch (tag)
                {
                    case IO_REPARSE_TAG.MOUNT_POINT:
                        REPARSE_DATA_BUFFER_MOUNTPOINT mp_buf = WinAPiFSwrapper.GetMountpointInfo(FileName);
                        textBoxSoftlinks.Text = mp_buf.SubstituteName;
                        break;

                    case IO_REPARSE_TAG.SYMLINK:
                        REPARSE_DATA_BUFFER_SYMLINK sl_buf = WinAPiFSwrapper.GetSymboliclinkInfo(FileName);
                        textBoxSoftlinks.Text = sl_buf.SubstituteName;
                        break;
                }
            }
            catch (Exception ex)
            {
                textBoxSoftlinks.Text = ex.Message;
            }
        }

        private void fill_dir_size()
        {
            FileInfoEx dir_info = new FileInfoEx();
            FileInfoEx.TryGet(FileName, ref dir_info);

            if (!dir_info.Directory)
            {
                return;
            }

            long size = 0L;
            long allocation = 0L;
            long dir_count = 0L;
            long file_count = 0L;

            calc_size_count(dir_info, ref size, ref allocation, ref file_count, ref dir_count);

            textBoxDirectorySize.Text = string.Format
                ("{0:N0} / {1:N0} " + Options.GetLiteral(Options.LANG_BYTES), size, allocation);
            textBoxDirectoryAllocation.Text = string.Format("{0:N0} / {1:N0}", file_count, dir_count);
        }

        private void calc_size_count(FileInfoEx dir_info, ref long size, ref long allocation,ref long file_count,ref long dir_count)
        {
            FileInfoExEnumerable dir_enum = new FileInfoExEnumerable
            (Path.Combine(dir_info.FullName, "*"),
            true,
            true,
            true,
            true);
            foreach (FileInfoEx info in dir_enum)
            {
                IntPtr h_file = IntPtr.Zero;
                try
                {
                    h_file = WinAPiFSwrapper.CreateFileHandle
                        (info.FullName,
                        Win32FileAccess.READ_ATTRIBUTES | Win32FileAccess.READ_EA,
                        FileShare.ReadWrite | FileShare.Delete,
                        FileMode.Open,
                        info.Directory ? CreateFileOptions.BACKUP_SEMANTICS : CreateFileOptions.None);

                    NT_FILE_STANDARD_INFORMATION s_info = ntApiFSwrapper.GetFileInfo_standard(h_file);
                    size += s_info.EndOfFile;
                    allocation += s_info.AllocationSize;
                    //if (s_info.AllocationSize < s_info.EndOfFile)
                    //{
                    //    /*DEBUG*/
                    //    long delta = s_info.EndOfFile - s_info.AllocationSize;
                    //}
                }
                catch (Exception)
                {
                    //Messages.ShowException
                    //    (ex,
                    //    string.Format
                    //    (Options.GetLiteral(Options.LANG_CANNOT_READ_ATTRIBUTES_0),
                    //    info.FullName));
                    break;
                }
                finally
                {
                    if ((h_file.ToInt32() != WinApiFS.INVALID_HANDLE_VALUE) && (h_file != IntPtr.Zero))
                    {
                        WinApiFS.CloseHandle(h_file);
                    }
                }
                if (info.Directory)
                {
                    calc_size_count(info, ref size, ref allocation, ref file_count, ref dir_count);
                    dir_count++;
                }
                else
                {
                    file_count++;
                }
            }
        }

        private void FillStandardPage(string file_name)
        {
            IntPtr file_handle = IntPtr.Zero;
            string nt_file_name = IOhelper.GetUnicodePath(file_name);

            try
            {
                textBoxName.Text = string.Format
                    ("{0}",
                    Path.GetFileName(file_name));

                // find data --------------------------------------
                WIN32_FIND_DATA f_data = new WIN32_FIND_DATA();
                WinAPiFSwrapper.GetFileInfo(file_name, ref f_data);
                textBoxAttributes.Text = f_data.dwFileAttributes.ToString();
                textBoxAltname.Text = f_data.cAlternateFileName;
                textBoxReaprseTag.Text = f_data.ReparseTag.ToString();
                //--------------------------------------------------

                fileSystemSecurityViewer1.FillContains(file_name);

                try
                {
                    fill_basic_info();
                    //fill_device();
                    fill_standard_info();
                    //fill_volume_attr();
                    //fill_volume_info();
                    fill_links(f_data.ReparseTag);
                    fill_dir_size();
                }
                catch (Exception ex)
                {
                    Messages.ShowException(ex);
                }

                try
                {
                    streamViewer1.FillContents(file_name);
                }
                catch (Exception)
                {
                }

            }
            catch (Exception ex)
            {
                Messages.ShowException(ex);
            }
            finally
            {
                if ((file_handle != IntPtr.Zero) && (file_handle.ToInt32() != WinApiFS.INVALID_HANDLE_VALUE))
                {
                    WinApiFS.CloseHandle(file_handle);
                }
            }


        }
    }
}

