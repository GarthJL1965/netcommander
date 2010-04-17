using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace netCommander
{
    public partial class VolumeSpaceInfoDialog : Form
    {
        public VolumeSpaceInfoDialog()
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
            labelTotalSize.Text = Options.GetLiteral(Options.LANG_TOTAL_VOLUME_SPACE);
            labelTotalAvailable.Text = Options.GetLiteral(Options.LANG_TOTAL_VOLUME_AVAILABLE);

        }
    }
}
