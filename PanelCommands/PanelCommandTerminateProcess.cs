using System;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;

namespace netCommander
{
    class PanelCommandTerminateProcess : PanelCommandBase
    {
        public PanelCommandTerminateProcess()
            : base(Options.GetLiteral(Options.LANG_TERMINATE), System.Windows.Forms.Shortcut.F8)
        {

        }

        protected override void internal_command_proc()
        {
            QueryPanelInfoEventArgs e_current = new QueryPanelInfoEventArgs();
            OnQueryCurrentPanel(e_current);

            ProcessList pl = (ProcessList)e_current.ItemCollection;
            Process p = pl[e_current.FocusedIndex];

            //prepare dialog
            TerminateProcessOptions opts = Options.TerminateProcessOptions;
            TerminateProcessDialog dialog = new TerminateProcessDialog();
            dialog.Text = Options.GetLiteral(Options.LANG_TERMINATE);
            dialog.labelTerminateProcessMessage.Text = string.Format
                (Options.GetLiteral(Options.LANG_TERMINATE_PROCESS_ID_NAME),
                p.ProcessName,
                p.Id);
            dialog.Options = opts;

            if (dialog.ShowDialog() != System.Windows.Forms.DialogResult.OK)
            {
                return;
            }

            opts = dialog.Options;
            Options.TerminateProcessOptions = opts;
            bool res = false;

            try
            {
                if ((opts & TerminateProcessOptions.DebugMode) == TerminateProcessOptions.DebugMode)
                {
                    Process.EnterDebugMode();
                }

                if ((opts & TerminateProcessOptions.CloseMainWindow) == TerminateProcessOptions.CloseMainWindow)
                {
                    res = p.CloseMainWindow();

                    if (!res)
                    {
                        Messages.ShowMessage(Options.GetLiteral(Options.LANG_TERMINATE_NO_MAIN_WINDOW));
                    }
                    else
                    {
                        res = p.WaitForExit(30000);
                    }
                }
                else if ((opts & TerminateProcessOptions.Kill) == TerminateProcessOptions.Kill)
                {
                    p.Kill();
                    res = p.WaitForExit(30000);
                }

                if (!res)
                {
                    Messages.ShowMessage(Options.GetLiteral(Options.LANG_TERMINATE_FAIL));
                }
            }
            catch (Exception ex)
            {
                Messages.ShowException(ex);
            }
            finally
            {
                if ((opts & TerminateProcessOptions.DebugMode) == TerminateProcessOptions.DebugMode)
                {
                    try
                    {
                        Process.LeaveDebugMode();
                    }
                    catch (Exception) { }
                }
            }
        }
    }
}
