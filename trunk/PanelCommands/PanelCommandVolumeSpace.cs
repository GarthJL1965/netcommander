using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using netCommander.FileSystemEx;

namespace netCommander
{
    public class PanelCommandVolumeSpace : PanelCommandBase
    {
        public PanelCommandVolumeSpace()
            : base(Options.GetLiteral(Options.LANG_VOLUME_SPACE_INFO), Shortcut.CtrlL)
        {

        }

        protected override void internal_command_proc()
        {
            QueryPanelInfoEventArgs e = new QueryPanelInfoEventArgs();
            OnQueryCurrentPanel(e);

            if (!(e.ItemCollection is DirectoryList))
            {
                return;
            }

            try
            {
                DirectoryList dl = (DirectoryList)e.ItemCollection;
                VolumeSpaceInfo info = WinAPiFSwrapper.GetVolumeSpaceInfo(dl.DirectoryPath);

                VolumeSpaceInfoDialog dialog = new VolumeSpaceInfoDialog();
                dialog.textBoxTotalSize.Text = IOhelper.SizeToString(info.TotalNumberOfBytes);
                dialog.textBoxTotalAvailable.Text = IOhelper.SizeToString(info.FreeBytesAvailable);
                dialog.Text = dl.DirectoryPath;
                dialog.ShowDialog();
            }
            catch (Exception ex)
            {
                Messages.ShowException(ex);
            }
        }
    }
}
