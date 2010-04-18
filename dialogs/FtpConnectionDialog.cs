using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace netCommander
{
    public partial class FtpConnectionDialog : Form
    {
        public FtpConnectionDialog()
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

            label1.Text = Options.GetLiteral(Options.LANG_FTP_SERVER_HOST_NAME);
            label2.Text = Options.GetLiteral(Options.LANG_FTP_TCP_PORT);
            label3.Text = Options.GetLiteral(Options.LANG_FTP_TIMEOUT);
            checkBoxPassive.Text = Options.GetLiteral(Options.LANG_FTP_PASSIVE_MODE);
            checkBoxEnableProxy.Text = Options.GetLiteral(Options.LANG_FTP_ENABLE_PROXY);
            checkBoxKeepAlive.Text = Options.GetLiteral(Options.LANG_FTP_KEEP_ALIVE);
            checkBoxAnonymous.Text = Options.GetLiteral(Options.LANG_FTP_ANONYMOUS_LOGON);
            checkBoxEnableSsl.Text = Options.GetLiteral(Options.LANG_FTP_ENABLE_SSL);
            buttonCancel.Text = Options.GetLiteral(Options.LANG_CANCEL);
            buttonOK.Text = Options.GetLiteral(Options.LANG_OK);
            toolTip1.SetToolTip(textBoxServerName, Options.GetLiteral(Options.LANG_FTP_SERVER_HOST_NAME_TOOLTIP));
            toolTip1.SetToolTip(numericUpDownPort, Options.GetLiteral(Options.LANG_FTP_TCP_PORT_TOOLTIP));
            toolTip1.SetToolTip(numericUpDownTimeout, Options.GetLiteral(Options.LANG_FTP_TIMEOUT_TOOLTIP));
            toolTip1.SetToolTip(checkBoxPassive, Options.GetLiteral(Options.LANG_FTP_PASSIVE_MODE_TOOLTIP));
            toolTip1.SetToolTip(checkBoxEnableProxy, Options.GetLiteral(Options.LANG_FTP_ENABLE_PROXY_TOOLTIP));
            toolTip1.SetToolTip(checkBoxKeepAlive, Options.GetLiteral(Options.LANG_FTP_KEEP_ALIVE_TOOLTIP));
            toolTip1.SetToolTip(checkBoxAnonymous, Options.GetLiteral(Options.LANG_FTP_ANONYMOUS_LOGON_TOOLTIP));

        }

        public FtpConnectionOptions FtpConnectionOptions
        {
            get
            {
                FtpConnectionOptions ret = new FtpConnectionOptions();

                ret.Anonymous = checkBoxAnonymous.Checked;
                ret.EnableProxy = checkBoxEnableProxy.Checked;
                ret.EnableSsl = checkBoxEnableSsl.Checked;
                ret.KeepAlive = checkBoxKeepAlive.Checked;
                ret.Passive = checkBoxPassive.Checked;
                ret.Port = (int)numericUpDownPort.Value;
                ret.ServerName = textBoxServerName.Text;
                ret.Timeout = (int)numericUpDownTimeout.Value;

                return ret;
            }
            set
            {
                checkBoxAnonymous.Checked = value.Anonymous;
                checkBoxEnableProxy.Checked = value.EnableProxy;
                checkBoxEnableSsl.Checked = value.EnableSsl;
                checkBoxKeepAlive.Checked = value.KeepAlive;
                checkBoxPassive.Checked = value.Passive;
                numericUpDownPort.Value = value.Port;
                textBoxServerName.Text = value.ServerName;
                numericUpDownTimeout.Value = value.Timeout;
            }
        }
    }
}
