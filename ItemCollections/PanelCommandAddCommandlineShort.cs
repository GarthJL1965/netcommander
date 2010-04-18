using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;

namespace netCommander
{
    public class PanelCommandAddCommandlineShort : PanelCommandBase
    {
        public PanelCommandAddCommandlineShort()
        {
            ToolStripMenuItem menu = new ToolStripMenuItem();
            menu.Text = "Add to command line";
            menu.ShortcutKeys = Keys.Enter | Keys.Control;
            CommandMenu = menu;
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
                (e.ItemCollection.GetCommandlineTextShort(e.FocusedIndex));
        }
    }
}
