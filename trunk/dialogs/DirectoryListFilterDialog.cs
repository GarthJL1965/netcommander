using System;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.ComponentModel;

namespace netCommander
{
    public partial class DirectoryListFilterDialog : Form
    {
        public DirectoryListFilterDialog()
            : this(false, null)
        {
            //InitializeComponent();

            //byte[] settings = Options.FilterDirectory;
            //SettingsDeserialize(settings);

            //groupBox4.Enabled = false;
        }

        public DirectoryListFilterDialog(bool enable_location_dialog,DirectoryListFilter filter)
        {
            InitializeComponent();

            

            groupBox4.Enabled = enable_location_dialog;

            if (filter != null)
            {
                DirectoryListFilter = filter;
            }
            else
            {
                fill_defaults();
            }

            set_lang();
        }

        private void set_lang()
        {
            if (LicenseManager.UsageMode == LicenseUsageMode.Designtime)
            {
                return;
            }

            tabPage1.Text = Options.GetLiteral(Options.LANG_FILE_NAME);
            groupBox4.Text = Options.GetLiteral(Options.LANG_LOCATION);
            radioButtonCurrentDirectory.Text = Options.GetLiteral(Options.LANG_CURRENT_DIRECTORY);
            checkBoxIncludeSubdirs.Text = Options.GetLiteral(Options.LANG_PROCESS_RECURSIVELY);
            radioButtonCurrentDrive.Text = Options.GetLiteral(Options.LANG_CURRENT_DRIVE);
            radioButtonLocalDrives.Text = Options.GetLiteral(Options.LANG_LOCAL_DRIVES);
            checkBoxFixedDrives.Text = Options.GetLiteral(Options.LANG_FIXED_DRIVES);
            checkBoxRemovableDrives.Text = Options.GetLiteral(Options.LANG_REMOVABLE_DRIVES);
            checkBoxNetworkDrives.Text = Options.GetLiteral(Options.LANG_NETWORK_DRIVES);
            buttonSaveDefault.Text = Options.GetLiteral(Options.LANG_SAVE_DEFAULTS);
            buttonCancel.Text = Options.GetLiteral(Options.LANG_CANCEL);
            buttonOK.Text = Options.GetLiteral(Options.LANG_OK);
            tabPage2.Text = Options.GetLiteral(Options.LANG_FILE_ATTRIBUTES);
            checkBoxIgnoreAttributes.Text = Options.GetLiteral(Options.LANG_IGNORE);
            tabPage3.Text = Options.GetLiteral(Options.LANG_FILE_SIZE);
            checkBoxIgnoreSize.Text = Options.GetLiteral(Options.LANG_IGNORE);
            label1.Text = Options.GetLiteral(Options.LANG_AND);
            tabPage4.Text = Options.GetLiteral(Options.LANG_DATETIME);
            checkBoxIgnoreTimeCreate.Text = Options.GetLiteral(Options.LANG_IGNORE);
            checkBoxIgnoreTimeAccess.Text = Options.GetLiteral(Options.LANG_IGNORE);
            checkBoxIgnoreTimeModification.Text = Options.GetLiteral(Options.LANG_IGNORE);
            label2.Text = Options.GetLiteral(Options.LANG_BETWEEN);
            label5.Text = Options.GetLiteral(Options.LANG_BETWEEN);
            label7.Text = Options.GetLiteral(Options.LANG_BETWEEN);
            label3.Text = Options.GetLiteral(Options.LANG_AND);
            label4.Text = Options.GetLiteral(Options.LANG_AND);
            label6.Text = Options.GetLiteral(Options.LANG_AND);
            groupBox1.Text = Options.GetLiteral(Options.LANG_CREATE_TIME);
            groupBox2.Text = Options.GetLiteral(Options.LANG_MODIFICATION_TIME);
            groupBox3.Text = Options.GetLiteral(Options.LANG_ACCESS_TIME);

        }

        string current_drive = string.Empty;

        public DirectoryListFilter DirectoryListFilter
        {
            get
            {
                DirectoryListFilter ret = new DirectoryListFilter();
                ret.AccessBegin = dateTimePickerAccessBegin.Value;
                ret.AccessEnd = dateTimePickerAccessEnd.Value;
                ret.CreateBegin = dateTimePickerCreateBegin.Value;
                ret.CreateEnd = dateTimePickerCreateEnd.Value;
                ret.FileAttributes = (FileAttributes)((uint)flagBoxFileAttributes.FlagValue);
                ret.FilterSizeCriteria = (FilterSizeCriteria)comboBoxSizeCriteria.SelectedIndex;
                ret.IgnoreFileAttributes = checkBoxIgnoreAttributes.Checked;
                ret.IgnoreSize = checkBoxIgnoreSize.Checked;
                ret.IgnoreTimeAccess = checkBoxIgnoreTimeAccess.Checked;
                ret.IgnoreTimeCreate = checkBoxIgnoreTimeCreate.Checked;
                ret.IgnoreTimeModification = checkBoxIgnoreTimeModification.Checked;

                string[] masks = textBoxFileMask.Text.Split(new char[] { ';' }, StringSplitOptions.RemoveEmptyEntries);
                if ((masks == null) || (masks.Length == 0))
                {
                    masks = new string[] { "*" };
                }

                ret.Masks = masks;
                ret.ModificationBegin = dateTimePickerModificationBegin.Value;
                ret.ModificationEnd = dateTimePickerModificationEnd.Value;
                ret.SizeMaximum = long.Parse(textBoxSizeMaximum.Text);
                ret.SizeMinimum = long.Parse(textBoxSizeMinimum.Text);

                ret.InCurrentDirectory = radioButtonCurrentDirectory.Checked;
                ret.InCurrentDirectoryWithSubdirs = checkBoxIncludeSubdirs.Checked;
                ret.InCurrentDrive = radioButtonCurrentDrive.Checked;
                ret.InFixedDrives = checkBoxFixedDrives.Checked;
                ret.InNetworkDrives = checkBoxNetworkDrives.Checked;
                ret.InRemovableDrives = checkBoxRemovableDrives.Checked;
                ret.CurrentDirectory = textBoxCurrentDirectory.Text;
                ret.CurrentDrive = current_drive;

                return ret;
            }
            set
            {
                string mask_text = string.Empty;
                for (int i = 0; i < value.Masks.Length - 1; i++)
                {
                    mask_text = mask_text + value.Masks[i] + ";";
                }
                mask_text = mask_text + value.Masks[value.Masks.Length - 1];
                textBoxFileMask.Text = mask_text;
                textBoxSizeMaximum.Text = value.SizeMaximum.ToString();
                textBoxSizeMinimum.Text = value.SizeMinimum.ToString();
                comboBoxSizeCriteria.SelectedIndex = (int)value.FilterSizeCriteria;
                checkBoxIgnoreAttributes.Checked = value.IgnoreFileAttributes;
                checkBoxIgnoreSize.Checked = value.IgnoreSize;
                checkBoxIgnoreTimeAccess.Checked = value.IgnoreTimeAccess;
                checkBoxIgnoreTimeCreate.Checked = value.IgnoreTimeCreate;
                checkBoxIgnoreTimeModification.Checked = value.IgnoreTimeModification;
                flagBoxFileAttributes.FillBox(value.FileAttributes);
                dateTimePickerAccessBegin.Value = value.AccessBegin;
                dateTimePickerAccessEnd.Value = value.AccessEnd;
                dateTimePickerCreateBegin.Value = value.CreateBegin;
                dateTimePickerCreateEnd.Value = value.CreateEnd;
                dateTimePickerModificationBegin.Value = value.ModificationBegin;
                dateTimePickerModificationEnd.Value = value.ModificationEnd;

                radioButtonCurrentDirectory.Checked = value.InCurrentDirectory;
                checkBoxIncludeSubdirs.Checked = value.InCurrentDirectoryWithSubdirs;
                radioButtonCurrentDrive.Checked = value.InCurrentDrive;
                checkBoxFixedDrives.Checked = value.InFixedDrives;
                checkBoxNetworkDrives.Checked = value.InNetworkDrives;
                checkBoxRemovableDrives.Checked = value.InRemovableDrives;
                textBoxCurrentDirectory.Text = value.CurrentDirectory;
                current_drive = value.CurrentDrive;
            }
        }

        private void checkBoxIgnoreAttributes_CheckedChanged(object sender, EventArgs e)
        {
            CheckBox cb = (CheckBox)sender;
            flagBoxFileAttributes.Enabled = !cb.Checked;
        }

        private void checkBoxIgnoreSize_CheckedChanged(object sender, EventArgs e)
        {
            CheckBox cb = (CheckBox)sender;
            comboBoxSizeCriteria.Enabled = !cb.Checked;
            textBoxSizeMaximum.Enabled = (!cb.Checked) & (comboBoxSizeCriteria.SelectedIndex == 5);
            textBoxSizeMinimum.Enabled = !cb.Checked;
        }

        private void comboBoxSizeCriteria_SelectedIndexChanged(object sender, EventArgs e)
        {
            ComboBox cb = (ComboBox)sender;
            textBoxSizeMaximum.Enabled = (cb.SelectedIndex == 5);
        }

        private void checkBoxIgnoreTimeCreate_CheckedChanged(object sender, EventArgs e)
        {
            CheckBox cb = (CheckBox)sender;
            dateTimePickerCreateBegin.Enabled = !cb.Checked;
            dateTimePickerCreateEnd.Enabled = !cb.Checked;
        }

        private void checkBoxIgnoreTimeModification_CheckedChanged(object sender, EventArgs e)
        {
            CheckBox cb = (CheckBox)sender;
            dateTimePickerModificationBegin.Enabled = !cb.Checked;
            dateTimePickerModificationEnd.Enabled = !cb.Checked;
        }

        private void checkBoxIgnoreTimeAccess_CheckedChanged(object sender, EventArgs e)
        {
            CheckBox cb = (CheckBox)sender;
            dateTimePickerAccessBegin.Enabled = !cb.Checked;
            dateTimePickerAccessEnd.Enabled = !cb.Checked;
        }

        //private byte[] SettingsSerialize()
        //{
        //    System.IO.MemoryStream ms = new System.IO.MemoryStream();

        //    //4 byte - length of mask
        //    int mask_len = Encoding.Unicode.GetByteCount(textBoxFileMask.Text);
        //    byte[] mask_len_bytes = BitConverter.GetBytes(mask_len);
        //    ms.Write(mask_len_bytes, 0, 4);

        //    //mask_len bytes - mask
        //    byte[] mask_bytes = Encoding.Unicode.GetBytes(textBoxFileMask.Text);
        //    ms.Write(mask_bytes, 0, mask_len);

        //    //ignore attributes
        //    byte[] ignore_attr_bytes = BitConverter.GetBytes(checkBoxIgnoreAttributes.Checked);
        //    ms.Write(ignore_attr_bytes, 0, 1);

        //    //attributes
        //    //FileAttributes fa = (int)flagBoxFileAttributes.FlagValue;
        //    uint fa_uint = (uint)flagBoxFileAttributes.FlagValue;
        //    byte[] fa_bytes = BitConverter.GetBytes(fa_uint);
        //    ms.Write(fa_bytes, 0, 4);

        //    //ignore size
        //    byte[] ignore_size_bytes = BitConverter.GetBytes(checkBoxIgnoreSize.Checked);
        //    ms.Write(ignore_size_bytes, 0, 1);

        //    //size criteria
        //    byte[] size_criteria_bytes = BitConverter.GetBytes(comboBoxSizeCriteria.SelectedIndex);
        //    ms.Write(size_criteria_bytes, 0, 4);

        //    //size minimum
        //    long size_min = 0;
        //    if (textBoxSizeMinimum.Text != string.Empty)
        //    {
        //        size_min = long.Parse(textBoxSizeMinimum.Text);
        //    }
        //    byte[] size_min_bytes = BitConverter.GetBytes(size_min);
        //    ms.Write(size_min_bytes, 0, 8);

        //    //size maximum
        //    long size_max = 0;
        //    if (textBoxSizeMaximum.Text != string.Empty)
        //    {
        //        size_max = long.Parse(textBoxSizeMaximum.Text);
        //    }
        //    byte[] size_max_bytes = BitConverter.GetBytes(size_max);
        //    ms.Write(size_max_bytes, 0, 8);

        //    //ignore create time
        //    byte[] ignore_create_bytes = BitConverter.GetBytes(checkBoxIgnoreTimeCreate.Checked);
        //    ms.Write(ignore_create_bytes, 0, 1);

        //    //create begin
        //    byte[] create_begin_bytes = BitConverter.GetBytes(dateTimePickerCreateBegin.Value.ToFileTime());
        //    ms.Write(create_begin_bytes, 0, 8);

        //    //create end
        //    byte[] create_end_bytes = BitConverter.GetBytes(dateTimePickerCreateEnd.Value.ToFileTime());
        //    ms.Write(create_end_bytes, 0, 8);

        //    //ignore modification
        //    byte[] ignore_modif_bytes = BitConverter.GetBytes(checkBoxIgnoreTimeModification.Checked);
        //    ms.Write(ignore_modif_bytes, 0, 1);

        //    //modification begin
        //    byte[] modif_begin_bytes = BitConverter.GetBytes(dateTimePickerModificationBegin.Value.ToFileTime());
        //    ms.Write(modif_begin_bytes, 0, 8);

        //    //modification end
        //    byte[] modif_end_bytes = BitConverter.GetBytes(dateTimePickerModificationEnd.Value.ToFileTime());
        //    ms.Write(modif_end_bytes, 0, 8);

        //    //ignore access
        //    byte[] ignore_access_bytes = BitConverter.GetBytes(checkBoxIgnoreTimeAccess.Checked);
        //    ms.Write(ignore_access_bytes, 0, 1);

        //    //access begin
        //    byte[] access_begin_bytes = BitConverter.GetBytes(dateTimePickerAccessBegin.Value.ToFileTime());
        //    ms.Write(access_begin_bytes, 0, 8);

        //    //access end
        //    byte[] access_end_bytes = BitConverter.GetBytes(dateTimePickerAccessEnd.Value.ToFileTime());
        //    ms.Write(access_end_bytes, 0, 8);

        //    //current directory only
        //    byte[] current_dir_only_bytes = BitConverter.GetBytes(radioButtonCurrentDirectory.Checked);
        //    ms.Write(current_dir_only_bytes, 0, 1);

        //    //include subdirs
        //    byte[] include_subdirs_bytes = BitConverter.GetBytes(checkBoxIncludeSubdirs.Checked);
        //    ms.Write(include_subdirs_bytes, 0, 1);

        //    //current drive
        //    byte[] current_drive_bytes = BitConverter.GetBytes(radioButtonCurrentDrive.Checked);
        //    ms.Write(current_dir_only_bytes, 0, 1);

        //    //all drives
        //    byte[] all_drives_bytes = BitConverter.GetBytes(radioButtonLocalDrives.Checked);
        //    ms.Write(all_drives_bytes, 0, 1);

        //    //fixed drives
        //    byte[] fixed_drives_bytes = BitConverter.GetBytes(checkBoxFixedDrives.Checked);
        //    ms.Write(fixed_drives_bytes, 0, 1);

        //    //removable drives
        //    byte[] removable_drives_bytes = BitConverter.GetBytes(checkBoxRemovableDrives.Checked);
        //    ms.Write(removable_drives_bytes, 0, 1);

        //    //network drives
        //    byte[] network_drives_bytes = BitConverter.GetBytes(checkBoxNetworkDrives.Checked);
        //    ms.Write(network_drives_bytes, 0, 1);

        //    ms.Position = 0;
        //    int ms_len = (int)ms.Length;
        //    byte[] ret = new byte[ms_len];
        //    ms.Read(ret, 0, ms_len);

        //    ms.Close();

        //    return ret;
        //}

        //private void SettingsDeserialize(byte[] settings)
        //{
        //    if (settings.Length == 0)
        //    {
        //        fill_defaults();
        //        return;
        //    }

        //    MemoryStream ms = null;
        //    try
        //    {
        //        ms = new MemoryStream(settings);

        //        //mask
        //        byte[] int_bytes = new byte[4];
        //        ms.Read(int_bytes, 0, 4);
        //        int mask_len = BitConverter.ToInt32(int_bytes, 0);
        //        byte[] mask_bytes = new byte[mask_len];
        //        ms.Read(mask_bytes, 0, mask_len);
        //        textBoxFileMask.Text = Encoding.Unicode.GetString(mask_bytes);

        //        //ignore attr
        //        byte[] bool_bytes = new byte[1];
        //        ms.Read(bool_bytes, 0, 1);
        //        checkBoxIgnoreAttributes.Checked = BitConverter.ToBoolean(bool_bytes, 0);

        //        //attributes
        //        ms.Read(int_bytes, 0, 4);
        //        int attr_int = BitConverter.ToInt32(int_bytes, 0);
        //        FileAttributes fa = (FileAttributes)attr_int;
        //        flagBoxFileAttributes.FillBox(fa);

        //        //ignore size
        //        ms.Read(bool_bytes, 0, 1);
        //        checkBoxIgnoreSize.Checked = BitConverter.ToBoolean(bool_bytes, 0);

        //        //size criteria
        //        ms.Read(int_bytes, 0, 4);
        //        comboBoxSizeCriteria.SelectedIndex = BitConverter.ToInt32(int_bytes, 0);

        //        //size min
        //        byte[] long_bytes = new byte[8];
        //        ms.Read(long_bytes, 0, 8);
        //        textBoxSizeMinimum.Text = BitConverter.ToInt64(long_bytes, 0).ToString();

        //        //size max
        //        ms.Read(long_bytes, 0, 8);
        //        textBoxSizeMaximum.Text = BitConverter.ToInt64(long_bytes, 0).ToString();

        //        //ignore create
        //        ms.Read(bool_bytes, 0, 1);
        //        checkBoxIgnoreTimeCreate.Checked = BitConverter.ToBoolean(bool_bytes, 0);

        //        //create begin
        //        ms.Read(long_bytes, 0, 8);
        //        dateTimePickerCreateBegin.Value = DateTime.FromFileTime(BitConverter.ToInt64(long_bytes, 0));

        //        //create end
        //        ms.Read(long_bytes, 0, 8);
        //        dateTimePickerCreateEnd.Value = DateTime.FromFileTime(BitConverter.ToInt64(long_bytes, 0));

        //        //ignore modif
        //        ms.Read(bool_bytes, 0, 1);
        //        checkBoxIgnoreTimeModification.Checked = BitConverter.ToBoolean(bool_bytes, 0);

        //        //modif begin
        //        ms.Read(long_bytes, 0, 8);
        //        dateTimePickerModificationBegin.Value = DateTime.FromFileTime(BitConverter.ToInt64(long_bytes, 0));

        //        //modif end
        //        ms.Read(long_bytes, 0, 8);
        //        dateTimePickerModificationEnd.Value = DateTime.FromFileTime(BitConverter.ToInt64(long_bytes, 0));

        //        //ignore access
        //        ms.Read(bool_bytes, 0, 1);
        //        checkBoxIgnoreTimeAccess.Checked = BitConverter.ToBoolean(bool_bytes, 0);

        //        //access begin
        //        ms.Read(long_bytes, 0, 8);
        //        dateTimePickerAccessBegin.Value = DateTime.FromFileTime(BitConverter.ToInt64(long_bytes, 0));

        //        //access end
        //        ms.Read(long_bytes, 0, 8);
        //        dateTimePickerAccessEnd.Value = DateTime.FromFileTime(BitConverter.ToInt64(long_bytes, 0));

        //        //current directory only
        //        ms.Read(bool_bytes, 0, 1);
        //        radioButtonCurrentDirectory.Checked = BitConverter.ToBoolean(bool_bytes, 0);
              
        //        //include subdirs
        //        ms.Read(bool_bytes, 0, 1);
        //        checkBoxIncludeSubdirs.Checked = BitConverter.ToBoolean(bool_bytes, 0);

        //        //current drive
        //        ms.Read(bool_bytes, 0, 1);
        //        radioButtonCurrentDrive.Checked = BitConverter.ToBoolean(bool_bytes, 0);

        //        //all drives
        //        ms.Read(bool_bytes, 0, 1);
        //        radioButtonLocalDrives.Checked = BitConverter.ToBoolean(bool_bytes, 0);

        //        //fixed drives
        //        ms.Read(bool_bytes, 0, 1);
        //        checkBoxFixedDrives.Checked = BitConverter.ToBoolean(bool_bytes, 0);

        //        //removable drives
        //        ms.Read(bool_bytes, 0, 1);
        //        checkBoxRemovableDrives.Checked = BitConverter.ToBoolean(bool_bytes, 0);

        //        //network drives
        //        ms.Read(bool_bytes, 0, 1);
        //        checkBoxNetworkDrives.Checked = BitConverter.ToBoolean(bool_bytes, 0);
        //    }
        //    catch (Exception ex)
        //    {
        //        Messages.ShowMessage(ex.Message+" Failed to read settings. Load defaults.");
        //        fill_defaults();
        //    }
        //    finally
        //    {
        //        if (ms != null)
        //        {
        //            ms.Close();
        //        }
        //    }
        //}

        private void fill_defaults()
        {
            textBoxFileMask.Text = "*";
            textBoxSizeMaximum.Text = "0";
            textBoxSizeMinimum.Text = "0";
            comboBoxSizeCriteria.SelectedIndex = 0;
            checkBoxIgnoreAttributes.Checked = true;
            checkBoxIgnoreSize.Checked = true;
            checkBoxIgnoreTimeAccess.Checked = true;
            checkBoxIgnoreTimeCreate.Checked = true;
            checkBoxIgnoreTimeModification.Checked = true;
            flagBoxFileAttributes.FillBox(FileAttributes.Normal);
            dateTimePickerAccessBegin.Value = DateTime.Now.AddMonths(-1);
            dateTimePickerAccessEnd.Value = DateTime.Now;
            dateTimePickerCreateBegin.Value = DateTime.Now.AddMonths(-1);
            dateTimePickerCreateEnd.Value = DateTime.Now;
            dateTimePickerModificationBegin.Value = DateTime.Now.AddMonths(-1);
            dateTimePickerModificationEnd.Value = DateTime.Now;

            textBoxCurrentDirectory.Text = string.Empty;
            radioButtonCurrentDirectory.Checked = true;
        }

        private void buttonSaveDefault_Click(object sender, EventArgs e)
        {
            DirectoryListFilter.Save();
        }

        private void radioButtonCurrentDirectory_CheckedChanged(object sender, EventArgs e)
        {
            RadioButton rb = (RadioButton)sender;
            textBoxCurrentDirectory.Enabled = rb.Checked;
            checkBoxIncludeSubdirs.Enabled = rb.Checked;

            checkBoxNetworkDrives.Enabled = !rb.Checked;
            checkBoxRemovableDrives.Enabled = !rb.Checked;
            checkBoxFixedDrives.Enabled = !rb.Checked;
        }

        private void radioButtonLocalDrives_CheckedChanged(object sender, EventArgs e)
        {
            RadioButton rb = (RadioButton)sender;

            textBoxCurrentDirectory.Enabled = !rb.Checked;
            checkBoxIncludeSubdirs.Enabled = !rb.Checked;

            checkBoxNetworkDrives.Enabled = rb.Checked;
            checkBoxRemovableDrives.Enabled = rb.Checked;
            checkBoxFixedDrives.Enabled = rb.Checked;
        }

        private void radioButtonCurrentDrive_CheckedChanged(object sender, EventArgs e)
        {
            RadioButton rb = (RadioButton)sender;

            textBoxCurrentDirectory.Enabled = !rb.Checked;
            checkBoxIncludeSubdirs.Enabled = !rb.Checked;

            checkBoxNetworkDrives.Enabled = !rb.Checked;
            checkBoxRemovableDrives.Enabled = !rb.Checked;
            checkBoxFixedDrives.Enabled = !rb.Checked;
        }
    }
}
