using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace netCommander
{
    public partial class CreateLinkDialog : Form
    {
        public CreateLinkDialog()
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

            label1.Text = Options.GetLiteral(Options.LANG_LINK_NAME);
            label2.Text = Options.GetLiteral(Options.LANG_LINK_TARGET);
            groupBox1.Text = Options.GetLiteral(Options.LANG_LINK_TYPE);
            radioButtonHardlink.Text = Options.GetLiteral(Options.LANG_LINK_HARD);
            radioButtonMountpoint.Text = Options.GetLiteral(Options.LANG_LINK_MOUNTPOINT);
            radioButtonSymlink.Text = Options.GetLiteral(Options.LANG_LINK_SYMBOLIC);
            buttonCancel.Text = Options.GetLiteral(Options.LANG_CANCEL);
            buttonOK.Text = Options.GetLiteral(Options.LANG_OK);
            toolTip1.SetToolTip(textBoxLinkname, Options.GetLiteral(Options.LANG_LINK_NAME_TOOLTIP));
            toolTip1.SetToolTip(radioButtonHardlink, Options.GetLiteral(Options.LANG_LINK_HARD_TOOLTIP));
            toolTip1.SetToolTip(radioButtonMountpoint, Options.GetLiteral(Options.LANG_LINK_MOUNTPOINT_TOOLTIP));
            toolTip1.SetToolTip(radioButtonSymlink, Options.GetLiteral(Options.LANG_LINK_SYMBOLIC_TOOLTIP));
        }

        public netCommander.FileSystemEx.NTFSlinkType LinkType
        {
            get
            {
                if (radioButtonHardlink.Checked)
                {
                    return netCommander.FileSystemEx.NTFSlinkType.Hard;
                }
                if (radioButtonMountpoint.Checked)
                {
                    return netCommander.FileSystemEx.NTFSlinkType.Junction;
                }
                if (radioButtonSymlink.Checked)
                {
                    return netCommander.FileSystemEx.NTFSlinkType.Symbolic;
                }
                return netCommander.FileSystemEx.NTFSlinkType.Junction;
            }
            set
            {
                switch (value)
                {
                    case netCommander.FileSystemEx.NTFSlinkType.Hard:
                        radioButtonHardlink.Checked = true;
                        break;

                    case netCommander.FileSystemEx.NTFSlinkType.Junction:
                        radioButtonMountpoint.Checked = true;
                        break;

                    case netCommander.FileSystemEx.NTFSlinkType.Symbolic:
                        radioButtonSymlink.Checked = true;
                        break;
                }
            }
        }
    }
}
