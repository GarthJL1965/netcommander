using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.IO;

namespace netCommander
{
    public class PanelCommandShowAFS : PanelCommandBase
    {

        public PanelCommandShowAFS()
        {
            ToolStripMenuItem menu = new ToolStripMenuItem();
            menu.ShortcutKeys = Keys.Control | Keys.S;
            menu.ShowShortcutKeys = true;
            menu.Text = "Show NTFS file streams";
            CommandMenu = menu;
        }

        protected override void internal_command_proc()
        {
            QueryPanelInfoEventArgs e = new QueryPanelInfoEventArgs();
            OnQueryCurrentPanel(e);

            if (e.FocusedIndex == -1)
            {
                return;
            }

            if (e.ItemCollection == null)
            {
                return;
            }

            DirectoryList dl = (DirectoryList)e.ItemCollection;
            string target_file = Path.Combine(dl.DirectoryPath, dl.GetItemDisplayNameLong(e.FocusedIndex));

            AFSdialog dialog = new AFSdialog(target_file);
            dialog.ShowDialog();
        }
    }
}
