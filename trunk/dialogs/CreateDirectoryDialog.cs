using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace netCommander
{
    public partial class CreateDirectoryDialog : Form
    {
        public CreateDirectoryDialog()
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

            checkBoxUseTemplate.Text = Options.GetLiteral(Options.LANG_USE_AS_TEMPLATE);
            buttonCancel.Text = Options.GetLiteral(Options.LANG_CANCEL);
            buttonOK.Text = Options.GetLiteral(Options.LANG_OK);
        }

        private void checkBoxUseTemplate_CheckedChanged(object sender, EventArgs e)
        {
            textBoxTemplateDirectory.Enabled = ((CheckBox)sender).Checked;
        }
    }
}
