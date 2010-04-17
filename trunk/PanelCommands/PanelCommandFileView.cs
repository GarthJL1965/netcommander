using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;

namespace netCommander
{
    public class PanelCommandFileView : PanelCommandBase
    {
        public PanelCommandFileView()
            : base(Options.GetLiteral(Options.LANG_FILE_VIEW), Shortcut.F3)
        {

        }

        protected override void internal_command_proc()
        {
            try
            {
                QueryPanelInfoEventArgs e = new QueryPanelInfoEventArgs();
                OnQueryCurrentPanel(e);

                if (!(e.ItemCollection is DirectoryList))
                {
                    return;
                }

                DirectoryList dl = (DirectoryList)e.ItemCollection;
                string file_name = dl[e.FocusedIndex].FullName;

                FileViewEditDialog dialog = new FileViewEditDialog();
                dialog.OpenView(file_name);
                dialog.Show();
            }
            catch (Exception ex)
            {
                Messages.ShowException(ex);
            }
        }
    }
}
