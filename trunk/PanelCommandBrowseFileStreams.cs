using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using netCommander.FileSystemEx;
using System.IO;

namespace netCommander
{
    public class PanelCommandBrowseFileStreams : PanelCommandBase
    {
        public PanelCommandBrowseFileStreams()
        {
            ToolStripMenuItem menu = new ToolStripMenuItem();
            menu.ShortcutKeys = Keys.Control | Keys.D;
            menu.ShowShortcutKeys = true;
            menu.Text = "File streams";
            CommandMenu = menu;

        }

        protected override void internal_command_proc()
        {
            QueryPanelInfoEventArgs e = new QueryPanelInfoEventArgs();
            OnQueryCurrentPanel(e);

            if (e.FocusedIndex < 0)
            {
                return;
            }

            DirectoryList dl_list = (DirectoryList)e.ItemCollection;

            if (dl_list.GetItemDisplayNameLong(e.FocusedIndex) == "..")
            {
                return;
            }

            //check volume caps
            string root_path = Path.GetPathRoot(dl_list.DirectoryPath);
            VolumeInfo vi = new VolumeInfo(root_path);
            if ((vi.FileSystemFlags & VolumeCaps.NamedStreams) != VolumeCaps.NamedStreams)
            {
                Messages.ShowMessage
                    (string.Format
                    ("File system on drive {0} not support named streams",
                    root_path));
                return;
            }

            StreamList new_source = new StreamList
            (Path.Combine(dl_list.DirectoryPath, dl_list.GetItemDisplayNameLong(e.FocusedIndex)),
            false,
            0,
            false);

            try
            {
                new_source.Refill();
            }
            catch (Exception ex)
            {
                Messages.ShowException(ex);
                return;
            }

            SetNewSourceEventArgs e_new_source = new SetNewSourceEventArgs
            (new_source,
            false,
            string.Empty);
            OnSetNewSource(e_new_source);

        }
    }
}
