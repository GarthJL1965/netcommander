using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using netCommander.WNet;
using netCommander.NetApi;

namespace netCommander
{
    class PanelCommandServerInfo : PanelCommandBase
    {
        public PanelCommandServerInfo()
            : base(Options.GetLiteral(Options.LANG_PROPERTIES), Shortcut.F11)
        {

        }

        protected override void internal_command_proc()
        {
            QueryPanelInfoEventArgs e = new QueryPanelInfoEventArgs();
            OnQueryCurrentPanel(e);

            WnetResourceList rl = (WnetResourceList)e.ItemCollection;

            if (rl.GetItemDisplayNameLong(e.FocusedIndex) == "..")
            {
                return;
            }

            NETRESOURCE res_info = rl[e.FocusedIndex];

            NetInfoDialog dialog = new NetInfoDialog();
            dialog.FillInfo(res_info);
            dialog.ShowDialog();
            
             
        }
    }
}
