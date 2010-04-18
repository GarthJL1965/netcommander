using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace netCommander
{
    public partial class CopyFileDialog : Form
    {
        public CopyFileDialog()
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

            label2.Text = Options.GetLiteral(Options.LANG_MASK);
            label1.Text = Options.GetLiteral(Options.LANG_DESTINATION);
            checkBoxAllowDecryptedDestination.Text = Options.GetLiteral(Options.LANG_ALLOW_DECRIPT);
            checkBoxSupressErrors.Text = Options.GetLiteral(Options.LANG_SUPRESS_ERRORS);
            checkBoxCopyRecursively.Text = Options.GetLiteral(Options.LANG_PROCESS_RECURSIVELY);
            checkBoxShowTotalProgress.Text = Options.GetLiteral(Options.LANG_SHOW_TOTAL_PROGRESS);
            checkBoxClearAttributes.Text = Options.GetLiteral(Options.LANG_ALLOW_CLEAR_ATTRIBUTES);
            checkBoxCopySymlink.Text = Options.GetLiteral(Options.LANG_PROCESS_SYMLINK_AS_SYMLINK);
            checkBoxCopySecurityAttributes.Text = Options.GetLiteral(Options.LANG_COPY_SEC_ATTRIBUTES);
            checkBoxCopyEmptyDirs.Text = Options.GetLiteral(Options.LANG_PROCESS_EMPTY_DIRS);
            radioRewriteNo.Text = Options.GetLiteral(Options.LANG_NOT_OVERWRITE);
            radioButtonRewriteIfSourceNewer.Text = Options.GetLiteral(Options.LANG_OVERWRITE_ONLY_IF_SOURCE_NEWER);
            radioButtonRewriteAll.Text = Options.GetLiteral(Options.LANG_OVERWITE_ALWAYS);
            buttonOK.Text = Options.GetLiteral(Options.LANG_OK);
            buttonCancel.Text = Options.GetLiteral(Options.LANG_CANCEL);
            groupBox1.Text = Options.GetLiteral(Options.LANG_OVERWITE_EXISTING_FILES);

            toolTip1.SetToolTip(textBoxSourceMask, Options.GetLiteral(Options.LANG_MASK_TOOLTIP));
            toolTip1.SetToolTip(checkBoxAllowDecryptedDestination, Options.GetLiteral(Options.LANG_ALLOW_DECRYPT_TOOLTIP));
            toolTip1.SetToolTip(checkBoxClearAttributes, Options.GetLiteral(Options.LANG_ALLOW_CLEAR_ATTRIBUTES_TOOLTIP));
            toolTip1.SetToolTip(checkBoxCopySymlink, Options.GetLiteral(Options.LANG_PROCESS_SYMLINK_AS_SYMLINK_TOOLTIP));
            toolTip1.SetToolTip(checkBoxCopySecurityAttributes, Options.GetLiteral(Options.LANG_COPY_SEC_ATTRIBUTES_TOOLTIP));
            toolTip1.SetToolTip(checkBoxCopyEmptyDirs, Options.GetLiteral(Options.LANG_PROCESS_EMPTY_DIRS_TOOLTIP));

        }

        public CopyEngineOptions CopyEngineOptions
        {
            get
            {
                CopyEngineOptions ret = CopyEngineOptions.None;
                if (checkBoxAllowDecryptedDestination.Checked)
                {
                    ret = ret | CopyEngineOptions.AllowDecryptDestination;
                }
                if (checkBoxClearAttributes.Checked)
                {
                    ret = ret | CopyEngineOptions.AllowClearAttributes;
                }
                if (checkBoxCopySecurityAttributes.Checked)
                {
                    ret = ret | CopyEngineOptions.CopySecurityAttributes;
                }
                if (checkBoxCopySymlink.Checked)
                {
                    ret = ret | CopyEngineOptions.CopySymlinkAsSymlink;
                }
                if (checkBoxSupressErrors.Checked)
                {
                    ret = ret | CopyEngineOptions.SupressErrors;
                }
                if (radioButtonRewriteAll.Checked)
                {
                    ret = ret | CopyEngineOptions.RewriteAll;
                }
                if (radioButtonRewriteIfSourceNewer.Checked)
                {
                    ret = ret | CopyEngineOptions.RewriteIfSourceNewer;
                }
                if (radioRewriteNo.Checked)
                {
                    ret = ret | CopyEngineOptions.NoRewrite;
                }
                if (checkBoxShowTotalProgress.Checked)
                {
                    ret = ret | CopyEngineOptions.CalculateTotalSize;
                }
                if (checkBoxCopyEmptyDirs.Checked)
                {
                    ret = ret | CopyEngineOptions.CreateEmptyDirectories;
                }
                if (checkBoxCopyRecursively.Checked)
                {
                    ret = ret | CopyEngineOptions.CopyFilesRecursive;
                }
                return ret;
            }
            set
            {
                checkBoxAllowDecryptedDestination.Checked = ((value & CopyEngineOptions.AllowDecryptDestination) == CopyEngineOptions.AllowDecryptDestination);
                checkBoxClearAttributes.Checked = ((value & CopyEngineOptions.AllowClearAttributes) == CopyEngineOptions.AllowClearAttributes);
                checkBoxCopySecurityAttributes.Checked = (value & CopyEngineOptions.CopySecurityAttributes) == CopyEngineOptions.CopySecurityAttributes;
                checkBoxCopySymlink.Checked = (value & CopyEngineOptions.CopySymlinkAsSymlink) == CopyEngineOptions.CopySymlinkAsSymlink;
                checkBoxSupressErrors.Checked = (value & CopyEngineOptions.SupressErrors) == CopyEngineOptions.SupressErrors;
                radioButtonRewriteAll.Checked = (value & CopyEngineOptions.RewriteAll) == CopyEngineOptions.RewriteAll;
                radioButtonRewriteIfSourceNewer.Checked = (value & CopyEngineOptions.RewriteIfSourceNewer) == CopyEngineOptions.RewriteIfSourceNewer;
                radioRewriteNo.Checked = (value & CopyEngineOptions.NoRewrite) == CopyEngineOptions.NoRewrite;
                checkBoxShowTotalProgress.Checked = (value & CopyEngineOptions.CalculateTotalSize) == CopyEngineOptions.CalculateTotalSize;
                checkBoxCopyEmptyDirs.Checked = (value & CopyEngineOptions.CreateEmptyDirectories) == CopyEngineOptions.CreateEmptyDirectories;
                checkBoxCopyRecursively.Checked = (value & CopyEngineOptions.CopyFilesRecursive) == CopyEngineOptions.CopyFilesRecursive;
            }
        }
    

    }

    [Flags]
    public enum CopyEngineOptions
    {
        None = 0,
        AllowDecryptDestination = 0x1,
        SupressErrors = 0x2,
        AllowClearAttributes = 0x4,
        CopySymlinkAsSymlink = 0x8,
        CopySecurityAttributes = 0x10,
        NoRewrite = 0x20,
        RewriteIfSourceNewer = 0x40,
        RewriteAll = 0x80,
        CalculateTotalSize = 0x100,
        CopyFilesRecursive = 0x200,
        CreateEmptyDirectories = 0x400
    }
}
