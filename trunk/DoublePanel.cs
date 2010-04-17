using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using netCommander.winControls;

namespace netCommander
{
    public partial class DoublePanel : UserControl
    {
        public DoublePanel()
        {
            InitializeComponent();

            //init font
            Font = Options.FontFilePanel;

            PanelActive = mFilePanelLeft;
            PanelPassive = mFilePanelRight;

            mFilePanelLeft.Enter += new EventHandler(mFilePanel_Enter);
            mFilePanelRight.Enter += new EventHandler(mFilePanel_Enter);
        }

        public event EventHandler ActivePanelChange;

        void mFilePanel_Enter(object sender, EventArgs e)
        {
            mFilePanel panel=(mFilePanel)sender;

            if (panel == PanelActive)
            {
                return;
            }

            mFilePanel other_panel = null;
            if (panel == mFilePanelLeft)
            {
                other_panel = mFilePanelRight;
            }
            else
            {
                other_panel = mFilePanelLeft;
            }

            //panel now active, other panel now passive
            PanelActive = panel;
            PanelPassive = other_panel;

            OnActivePanelChange();
        }

        private void OnActivePanelChange()
        {
            if (ActivePanelChange != null)
            {
                ActivePanelChange(this, new EventArgs());
            }
        }

        public mFilePanel PanelActive { get; private set; }
        public mFilePanel PanelPassive { get; private set; }
    }
}
