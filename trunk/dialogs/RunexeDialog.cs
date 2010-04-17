using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace netCommander
{
    public partial class RunexeDialog : Form
    {
        public RunexeDialog()
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

            label2.Text = Options.GetLiteral(Options.LANG_FILE_NAME);
            label1.Text = Options.GetLiteral(Options.LANG_WORKING_DIRECTORY);
            label3.Text = Options.GetLiteral(Options.LANG_ARGUMENTS);
            label4.Text = Options.GetLiteral(Options.LANG_VERB);
            checkBoxShellexecute.Text = Options.GetLiteral(Options.LANG_USE_SHELLEXECUTE);
            checkBoxRunas.Text = Options.GetLiteral(Options.LANG_RUN_AS_ANOTHER_USER);
            checkBoxRunInConsole.Text = Options.GetLiteral(Options.LANG_RUN_IN_NEW_CONSOLE_WINDOW);
            checkBoxLoadProfile.Text = Options.GetLiteral(Options.LANG_LOAD_PROFILE);
            buttonCancel.Text = Options.GetLiteral(Options.LANG_CANCEL);
            buttonOK.Text = Options.GetLiteral(Options.LANG_OK);

        }

        public RunExeOptions RunExeOptions
        {
            get
            {
                RunExeOptions ret = RunExeOptions.None;
                if (checkBoxLoadProfile.Checked)
                {
                    ret = ret | RunExeOptions.LoadEnvironment;
                }
                if (checkBoxRunas.Checked)
                {
                    ret = ret | RunExeOptions.UseRunas;
                }
                if (checkBoxRunInConsole.Checked)
                {
                    ret = ret | RunExeOptions.RunInConsole;
                }
                if (checkBoxShellexecute.Checked)
                {
                    ret = ret | RunExeOptions.UseShellExecute;
                }
                return ret;
            }
            set
            {
                checkBoxLoadProfile.Checked = (value & RunExeOptions.LoadEnvironment) == RunExeOptions.LoadEnvironment;
                checkBoxRunas.Checked = (value & RunExeOptions.UseRunas) == RunExeOptions.UseRunas;
                checkBoxRunInConsole.Checked = (value & RunExeOptions.RunInConsole) == RunExeOptions.RunInConsole;
                checkBoxShellexecute.Checked = (value & RunExeOptions.UseShellExecute) == RunExeOptions.UseShellExecute;
            }
        }
    }
}
