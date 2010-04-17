using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace netCommander
{
    public partial class UserMenuEditDialog : Form
    {
        public UserMenuEditDialog()
        {
            InitializeComponent();
            set_lang();
        }

        private void set_lang()
        {
            if (LicenseManager.UsageMode == LicenseUsageMode.Designtime)
            {
                return;
            }

            columnHeaderCommand.Text = Options.GetLiteral(Options.LANG_COMMAND_TEXT);
            columnHeaderMenuText.Text = Options.GetLiteral(Options.LANG_MENU_TEXT);
            buttonAdd.Text = Options.GetLiteral(Options.LANG_ADD);
            buttonEdit.Text = Options.GetLiteral(Options.LANG_EDIT);
            buttonOK.Text = Options.GetLiteral(Options.LANG_OK);
            buttonRemove.Text = Options.GetLiteral(Options.LANG_DELETE);
            Text = Options.GetLiteral(Options.LANG_USER_MENU);
        }

        UserMenu internal_menu;

        public void SetMenu(UserMenu menu)
        {
            internal_menu = menu;

            for (int i = 0; i < internal_menu.Count; i++)
            {
                listViewInternal.Items.Add(new InternalItem(internal_menu[i]));
            }
        }

        private void edit_entry(InternalItem  item)
        {
            UserMenuEntryEditDialog dialog = new UserMenuEntryEditDialog();
            dialog.SetEntry(item.internalData);
            
            if (dialog.ShowDialog() != DialogResult.OK)
            {
                return;
            }

            dialog.Apply();
            item.UpdateView();
        }

        private void new_entry()
        {
            UserMenuEntry new_entry = new UserMenuEntry(string.Empty, string.Empty);
            UserMenuEntryEditDialog dialog = new UserMenuEntryEditDialog();
            dialog.SetEntry(new_entry);

            if (dialog.ShowDialog() != DialogResult.OK)
            {
                return;
            }
            dialog.Apply();

            internal_menu.Add(new_entry);
            listViewInternal.Items.Add(new InternalItem(new_entry));
        }

        private void remove_entry(int index)
        {
            listViewInternal.Items.RemoveAt(index);
            internal_menu.RemoveAt(index);
        }

        private class InternalItem : ListViewItem
        {
            public InternalItem(UserMenuEntry entry)
            :base()
            {
                internalData = entry;
                Text = internalData.Text;
                SubItems.Add(internalData.CommandText);
            }

            public void UpdateView()
            {
                Text = internalData.Text;
                SubItems[1].Text = internalData.CommandText;
            }

            public UserMenuEntry internalData
            {
                get;
                set;
            }
        }

        private void buttonAdd_Click(object sender, EventArgs e)
        {
            new_entry();
        }

        private void buttonRemove_Click(object sender, EventArgs e)
        {
            if (listViewInternal.SelectedItems.Count == 0)
            {
                return;
            }

            remove_entry(listViewInternal.SelectedIndices[0]);
        }

        private void buttonEdit_Click(object sender, EventArgs e)
        {
            if (listViewInternal.SelectedItems.Count == 0)
            {
                return;
            }

            edit_entry((InternalItem)listViewInternal.SelectedItems[0]);
        }

        private void listViewInternal_DoubleClick(object sender, EventArgs e)
        {
            ListViewHitTestInfo hit_info = listViewInternal.HitTest(listViewInternal.PointToClient(MousePosition));

            if (hit_info.Item == null)
            {
                return;
            }

            edit_entry((InternalItem)hit_info.Item);
        }

    }
}
