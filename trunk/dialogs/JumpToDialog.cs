using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace netCommander
{
    public partial class JumpToDialog : Form
    {
        public JumpToDialog()
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

            buttonCancel.Text = Options.GetLiteral(Options.LANG_CANCEL);
            buttonOK.Text = Options.GetLiteral(Options.LANG_OK);
        }
    }
}
