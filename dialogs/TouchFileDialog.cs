using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace netCommander
{
    public partial class TouchFileDialog : Form
    {
        public TouchFileDialog()
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

            label1.Text = Options.GetLiteral(Options.LANG_CREATE_TIME);
            label2.Text = Options.GetLiteral(Options.LANG_MODIFICATION_TIME);
            label3.Text = Options.GetLiteral(Options.LANG_ACCESS_TIME);
            buttonCancel.Text = Options.GetLiteral(Options.LANG_CANCEL);
            buttonOK.Text = Options.GetLiteral(Options.LANG_OK);
        }

        private bool activated_once = false;
        private void TouchFileDialog_Activated(object sender, EventArgs e)
        {
            if (!activated_once)
            {
                dateTimePickerAccess.Checked = false;
                dateTimePickerCreation.Checked = false;
                dateTimePickerModification.Checked = false;
            }
            activated_once = true;
        }
    }
}
