using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace netCommander
{
    public partial class TerminateProcessDialog : Form
    {
        public TerminateProcessDialog()
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

            radioButtonKill.Text = netCommander.Options.GetLiteral(netCommander.Options.LANG_TERMINATE_KILL);
            radioButtonSendCloseMainWindow.Text = netCommander.Options.GetLiteral(netCommander.Options.LANG_TERMINATE_SEND_CLOSE_WINDOW);
            checkBoxUseDebugMode.Text = netCommander.Options.GetLiteral(netCommander.Options.LANG_TERMINATE_DEBUG_MODE);
            buttonCancel.Text = netCommander.Options.GetLiteral(netCommander.Options.LANG_CANCEL);
            buttonOK.Text = netCommander.Options.GetLiteral(netCommander.Options.LANG_OK);
        }

        public TerminateProcessOptions Options
        {
            get
            {
                TerminateProcessOptions ret = TerminateProcessOptions.None;
                ret = radioButtonSendCloseMainWindow.Checked ? TerminateProcessOptions.CloseMainWindow : ret;
                ret = radioButtonKill.Checked ? ret | TerminateProcessOptions.Kill : ret;
                ret = checkBoxUseDebugMode.Checked ? ret | TerminateProcessOptions.DebugMode : ret;
                return ret;
            }
            set
            {
                radioButtonKill.Checked = (value & TerminateProcessOptions.Kill) == TerminateProcessOptions.Kill;
                radioButtonSendCloseMainWindow.Checked = (value & TerminateProcessOptions.CloseMainWindow) == TerminateProcessOptions.CloseMainWindow;
                checkBoxUseDebugMode.Checked = (value & TerminateProcessOptions.DebugMode) == TerminateProcessOptions.DebugMode;
            }
        }
    }

    [Flags()]
    public enum TerminateProcessOptions
    {
        None = 0,
        CloseMainWindow = 0x1,
        Kill = 0x2,
        DebugMode = 0x4
    }
}
