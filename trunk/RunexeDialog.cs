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
