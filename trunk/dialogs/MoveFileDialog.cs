using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace netCommander
{
    public partial class MoveFileDialog : Form
    {
        public MoveFileDialog()
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
            checkBoxSupressErrors.Text = Options.GetLiteral(Options.LANG_SUPRESS_ERRORS);
            checkBoxMoveSecirityAttributes.Text = Options.GetLiteral(Options.LANG_MOVE_SEC_ATTRIBUTES_WHILE_CROSS_VOLUMES);
            checkBoxReplaceExisting.Text = Options.GetLiteral(Options.LANG_OVERWITE_EXISTING_FILES);
            buttonCancel.Text = Options.GetLiteral(Options.LANG_CANCEL);
            buttonOK.Text = Options.GetLiteral(Options.LANG_OK);
        }

        public MoveEngineOptions MoveEngineOptions
        {
            get
            {
                MoveEngineOptions ret = MoveEngineOptions.None;
                if (checkBoxMoveSecirityAttributes.Checked)
                {
                    ret = ret | MoveEngineOptions.MoveSecurityAttributes;
                }
                if (checkBoxSupressErrors.Checked)
                {
                    ret = ret | MoveEngineOptions.SupressErrors;
                }
                if (checkBoxReplaceExisting.Checked)
                {
                    ret = ret | MoveEngineOptions.ReplaceExistingFiles;
                }
                return ret;
            }
            set
            {
                checkBoxMoveSecirityAttributes.Checked = (value & MoveEngineOptions.MoveSecurityAttributes) == MoveEngineOptions.MoveSecurityAttributes;
                checkBoxSupressErrors.Checked = (value & MoveEngineOptions.SupressErrors) == MoveEngineOptions.SupressErrors;
                checkBoxReplaceExisting.Checked = (value & MoveEngineOptions.ReplaceExistingFiles) == MoveEngineOptions.ReplaceExistingFiles;
            }
        }
    }

    [Flags]
    public enum MoveEngineOptions
    {
        None = 0,
        SupressErrors = 0x1,
        MoveSecurityAttributes = 0x2,
        ReplaceExistingFiles = 0x4
    }
}
