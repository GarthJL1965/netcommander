using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace netCommander
{
    public partial class CopyFileProgressDialog : Form
    {
        public CopyFileProgressDialog()
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

            checkBoxCloseOnFinish.Text = Options.GetLiteral(Options.LANG_CLOSE_ON_FINISH);
            buttonCancel.Text = Options.GetLiteral(Options.LANG_CANCEL);
        }

        public void SetProgress(ulong value, ulong maximum)
        {
            if (maximum < value)
            {
                progressBarCurrentStream.Style = ProgressBarStyle.Marquee;
            }
            else
            {
                progressBarCurrentStream.Style = ProgressBarStyle.Blocks;

                double progress_koeff = 1;

                if (maximum < (ulong)int.MaxValue)
                {
                    progress_koeff = 1;
                }
                else
                {
                    progress_koeff = (double)maximum / (double)int.MaxValue;
                }

                progressBarCurrentStream.Maximum = (int)((double)maximum / progress_koeff);
                progressBarCurrentStream.Value = (int)((double)value / progress_koeff);
            }
        }

        private object cancel_pressed_lock = new object();
        private bool cancel_pressed_unsafe = false;
        public bool CancelPressedSafe
        {
            get
            {
                bool ret = false;
                lock (cancel_pressed_lock)
                {
                    ret = cancel_pressed_unsafe;
                }
                return ret;
            }
            set
            {
                lock (cancel_pressed_lock)
                {
                    cancel_pressed_unsafe = value;
                }
            }
        }

        private void buttonCancel_Click(object sender, EventArgs e)
        {
            CancelPressedSafe = true;
        }

        private void CopyFileProgressDialog_FormClosing(object sender, FormClosingEventArgs e)
        {

        }
    }
}
