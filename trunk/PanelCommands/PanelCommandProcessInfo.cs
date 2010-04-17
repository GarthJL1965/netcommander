using System;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;

namespace netCommander
{
    class PanelCommandProcessInfo : PanelCommandBase
    {

        public PanelCommandProcessInfo()
            : base(Options.GetLiteral(Options.LANG_PROPERTIES), System.Windows.Forms.Shortcut.F11)
        {

        }

        protected override void internal_command_proc()
        {
            try
            {
                QueryPanelInfoEventArgs e_current = new QueryPanelInfoEventArgs();
                OnQueryCurrentPanel(e_current);

                ProcessList pl = (ProcessList)e_current.ItemCollection;
                Process p = pl[e_current.FocusedIndex];

                ProcessInfoDialog dialog = new ProcessInfoDialog();
                //p.Refresh();
                dialog.Fill(p);
                dialog.ShowDialog();
            }
            catch (Exception ex)
            {
                Messages.ShowException(ex);
            }
        }
    }
}
