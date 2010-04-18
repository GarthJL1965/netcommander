using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace netCommander
{
    public partial class FtpTransferDialog : Form
    {
        public FtpTransferDialog()
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

            label2.Text = Options.GetLiteral(Options.LANG_SOURCE);
            label1.Text = Options.GetLiteral(Options.LANG_DESTINATION);
            groupBox1.Text = Options.GetLiteral(Options.LANG_OVERWITE_EXISTING_FILES);
            radioButtonOverwriteNo.Text = Options.GetLiteral(Options.LANG_NOT_OVERWRITE);
            radioButtonOvewriteSOurceNewer.Text = Options.GetLiteral(Options.LANG_OVERWRITE_ONLY_IF_SOURCE_NEWER);
            radioButtonOverwriteAlways.Text = Options.GetLiteral(Options.LANG_OVERWITE_ALWAYS);
            checkBoxShowTotal.Text = Options.GetLiteral(Options.LANG_SHOW_TOTAL_PROGRESS);
            checkBoxSupressErrors.Text = Options.GetLiteral(Options.LANG_SUPRESS_ERRORS);
            label3.Text = Options.GetLiteral(Options.LANG_BUFFER_SIZE);
            buttonCancel.Text = Options.GetLiteral(Options.LANG_CANCEL);
            buttonOK.Text = Options.GetLiteral(Options.LANG_OK);
        }

        public FtpTransferOptions FtpTransferOptions
        {
            get
            {
                FtpTransferOptions ret = FtpTransferOptions.Default();
                try
                {
                    ret.BufferSize = int.Parse(textBoxBufferSize.Text);
                    if (radioButtonOverwriteAlways.Checked)
                    {
                        ret.Overwrite = OverwriteExisting.Yes;
                    }
                    if (radioButtonOverwriteNo.Checked)
                    {
                        ret.Overwrite = OverwriteExisting.No;
                    }
                    if (radioButtonOvewriteSOurceNewer.Checked)
                    {
                        ret.Overwrite = OverwriteExisting.IfSourceNewer;
                    }
                    ret.ShowTotalProgress = checkBoxShowTotal.Checked;
                    ret.SupressErrors = checkBoxSupressErrors.Checked;

                }
                catch (Exception ex)
                {
                    Messages.ShowException(ex);
                }
                return ret;
            }
            set
            {
                textBoxBufferSize.Text = value.BufferSize.ToString();

                switch (value.Overwrite)
                {
                    case OverwriteExisting.IfSourceNewer:
                        radioButtonOvewriteSOurceNewer.Checked = true;
                        break;

                    case OverwriteExisting.No:
                        radioButtonOverwriteNo.Checked = true;
                        break;

                    case OverwriteExisting.Yes:
                        radioButtonOverwriteAlways.Checked = true;
                        break;
                }

                checkBoxShowTotal.Checked = value.ShowTotalProgress;
                checkBoxSupressErrors.Checked = value.SupressErrors;
            }
        }
    }
}
