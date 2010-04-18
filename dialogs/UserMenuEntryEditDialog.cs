using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace netCommander
{
    public partial class UserMenuEntryEditDialog : Form
    {
        public UserMenuEntryEditDialog()
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

            labelCommandText.Text = Options.GetLiteral(Options.LANG_COMMAND_TEXT);
            labelMenuText.Text = Options.GetLiteral(Options.LANG_MENU_TEXT);
            textBoxHelp.Text = Options.GetLiteral(Options.LANG_COMMAND_TEXT_HELP);
            buttonCancel.Text = Options.GetLiteral(Options.LANG_CANCEL);
            buttonOK.Text = Options.GetLiteral(Options.LANG_OK);
            checkBoxNoWindow.Text = Options.GetLiteral(Options.LANG_PROCESS_NO_WINDOW);
            checkBoxUseShellexecute.Text = Options.GetLiteral(Options.LANG_USE_SHELLEXECUTE);
        }

        private UserMenuEntry internal_entry = new UserMenuEntry(string.Empty, string.Empty);

        public void Apply()
        {
            internal_entry.Text = textBoxMenuText.Text;
            internal_entry.CommandText = textBoxCommandText.Text;

            ProcessStartFlags opts = ProcessStartFlags.None;
            if (checkBoxNoWindow.Checked)
            {
                opts = opts | ProcessStartFlags.NoWindow;
            }
            if (checkBoxUseShellexecute.Checked)
            {
                opts = opts | ProcessStartFlags.UseShellexecute;
            }
            internal_entry.Options = opts;
        }

        public void SetEntry(UserMenuEntry entry)
        {
            internal_entry = entry;
            textBoxCommandText.Text = internal_entry.CommandText;
            textBoxMenuText.Text = internal_entry.Text;
            checkBoxNoWindow.Checked = (internal_entry.Options & ProcessStartFlags.NoWindow) == ProcessStartFlags.NoWindow;
            checkBoxUseShellexecute.Checked = (internal_entry.Options & ProcessStartFlags.UseShellexecute) == ProcessStartFlags.UseShellexecute;
            Text = internal_entry.Text;
        }
    }
}
