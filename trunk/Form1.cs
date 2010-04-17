using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using netCommander.FileSystemEx;
using System.IO;

namespace netCommander
{
    public partial class Form1 : Form
    {
        

        public Form1()
        {
            InitializeComponent();

            mFilePanel1.SourceChanged += new EventHandler(mFilePanel1_SourceChanged);
        }

        void mFilePanel1_SourceChanged(object sender, EventArgs e)
        {
            //update menu
            leftPanelToolStripMenuItem.DropDownItems.Clear();
            netCommander.winControls.mFilePanel panel = (netCommander.winControls.mFilePanel)sender;

            foreach (PanelCommandBase cmd in panel.Source.AvailableCommands)
            {
                leftPanelToolStripMenuItem.DropDownItems.Add(cmd.CommandMenu);
            }

        }

     

        private void button1_Click(object sender, EventArgs e)
        {
            //DriveList dl = new DriveList(1, false);
            

            //mFilePanel1.Source = dl;
            //dl.Refill();

            //DirectoryList dirL = new DirectoryList(4, false, @"c:\temp");
            //dirL.Refill();

            DirectoryList dirL = new DirectoryList(0, false, @"d:\temp");
            mFilePanel1.Source = dirL;
            dirL.Refill();

            //DirectoryListFilterDialog f = new DirectoryListFilterDialog();
            //f.ShowDialog();
        }

        private void findToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DirectoryListFilterDialog filter_dialog = new DirectoryListFilterDialog(true);
            if (filter_dialog.ShowDialog() == DialogResult.OK)
            {
                DirectorySearchResultDialog result_dialog = new DirectorySearchResultDialog();
                result_dialog.Show();
                result_dialog.BeginSearch(filter_dialog.DirectoryListFilter);
            }
        }
    }
}
