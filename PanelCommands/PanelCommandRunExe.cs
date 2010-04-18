using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.IO;
using netCommander.FileSystemEx;
using System.Diagnostics;

namespace netCommander
{
    public class PanelCommandRunExe : PanelCommandBase
    {
        public PanelCommandRunExe()
            : base(Options.GetLiteral(Options.LANG_EXECUTE), Shortcut.CtrlR)
        {
        }

        protected override void internal_command_proc()
        {
            QueryPanelInfoEventArgs e = new QueryPanelInfoEventArgs();
            OnQueryCurrentPanel(e);

            if (e.FocusedIndex == -1)
            {
                return;
            }

            DirectoryList dl = (DirectoryList)e.ItemCollection;

            if (dl.GetItemDisplayNameLong(e.FocusedIndex) == "..")
            {
                return;
            }

            //show dialog
            ProcessStartInfo psi = new ProcessStartInfo();
            string file_name = dl[e.FocusedIndex].FullName;
            psi.FileName = file_name;
            RunExeOptions opts = Options.RunExeOptions;
            RunexeDialog dialog = new RunexeDialog();
            dialog.Text = Options.GetLiteral(Options.LANG_EXECUTE);
            dialog.RunExeOptions = opts;
            dialog.textBoxArguments.Text = string.Empty;
            dialog.textBoxFilename.Text = dl[e.FocusedIndex].FileName;
            dialog.textBoxWorkingDirectory.Text = dl.DirectoryPath;
            if (psi.Verbs != null)
            {
                foreach (string verb in psi.Verbs)
                {
                    dialog.comboBoxVerb.Items.Add(verb);
                }
            }

            if (dialog.ShowDialog() != DialogResult.OK)
            {
                return;
            }

            opts = dialog.RunExeOptions;
            Options.RunExeOptions = opts;

            psi.LoadUserProfile = (opts & RunExeOptions.LoadEnvironment) == RunExeOptions.LoadEnvironment;
            if ((opts & RunExeOptions.RunInConsole) == RunExeOptions.RunInConsole)
            {
                if (file_name.Contains(" "))
                {
                    file_name = '"' + file_name + '"';
                }
                psi.FileName = "cmd";
                psi.Arguments = "/K "+file_name;
            }
            else
            {
                psi.FileName = file_name;
            }

            if ((opts & RunExeOptions.UseRunas) == RunExeOptions.UseRunas)
            {
                string user = string.Empty;
                string pass = string.Empty;

                if (Messages.AskCredentials(Options.GetLiteral(Options.LANG_ACCOUNT), Options.GetLiteral(Options.LANG_EXECUTE) + " '" + file_name + "'", ref user, ref pass) == DialogResult.OK)
                {
                    psi.UserName = user;
                    System.Security.SecureString sec = new System.Security.SecureString();
                    foreach (char c in pass)
                    {
                        sec.AppendChar(c);
                    }
                    psi.Password = sec;
                }
            }

            psi.UseShellExecute = (opts & RunExeOptions.UseShellExecute) == RunExeOptions.UseShellExecute;

            psi.WorkingDirectory = dialog.textBoxWorkingDirectory.Text;
            psi.Verb = dialog.comboBoxVerb.Text;
            psi.Arguments = psi.Arguments + " " + dialog.textBoxArguments.Text;

            try
            {
                Process.Start(psi);
            }
            catch (Exception ex)
            {
                
                Messages.ShowException
                    (ex,
                    string.Format(Options.GetLiteral(Options.LANG_CANNOT_EXCUTE_0_1), psi.FileName, psi.Arguments));
            }
        }
    }

    [Flags()]
    public enum RunExeOptions
    {
        None = 0,
        UseShellExecute = 0x1,
        RunInConsole = 0x2,
        UseRunas = 0x4,
        LoadEnvironment = 0x8,
    }
}
