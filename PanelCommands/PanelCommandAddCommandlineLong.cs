using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;

namespace netCommander
{
    public class PanelCommandAddCommandlineLong : PanelCommandBase
    {
        public PanelCommandAddCommandlineLong()
            : base(Options.GetLiteral(Options.LANG_ADD_TO_COMMAND_LINE_WITH_PATH), Shortcut.CtrlF)
        {

        }

        protected override void internal_command_proc()
        {
            QueryPanelInfoEventArgs e = new QueryPanelInfoEventArgs();
            OnQueryCurrentPanel(e);

            if (e.ItemCollection.MainWindow == null)
            {
                return;
            }

            e.ItemCollection.MainWindow.commandPrompt.AddCommandChunk
                (e.ItemCollection.GetCommandlineTextLong(e.FocusedIndex));
        }
    }
}
