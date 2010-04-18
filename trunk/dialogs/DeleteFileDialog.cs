using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace netCommander
{
    public partial class DeleteFileDialog : Form
    {
        public DeleteFileDialog()
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
            labelMask.Text = Options.GetLiteral(Options.LANG_MASK);
            checkBoxRemoveEmptyDirs.Text = Options.GetLiteral(Options.LANG_PROCESS_EMPTY_DIRS);
            checkBoxRecursive.Text = Options.GetLiteral(Options.LANG_PROCESS_RECURSIVELY);
            checkBoxForceReadonly.Text = Options.GetLiteral(Options.LANG_DELETE_READONLY);
            buttonCancel.Text = Options.GetLiteral(Options.LANG_CANCEL);
            buttonOK.Text = Options.GetLiteral(Options.LANG_OK);
        }

        public DeleteFileOptions DeleteFileOptions
        {
            get
            {
                DeleteFileOptions ret = DeleteFileOptions.None;
                if (checkBoxForceReadonly.Checked)
                {
                    ret = ret | DeleteFileOptions.DeleteReadonly;
                }
                if (checkBoxRecursive.Checked)
                {
                    ret = ret | DeleteFileOptions.RecursiveDeleteFiles;
                }
                if (checkBoxRemoveEmptyDirs.Checked)
                {
                    ret = ret | DeleteFileOptions.DeleteEmptyDirectories;
                }
                return ret;
            }
            set
            {
                checkBoxRemoveEmptyDirs.Checked = (value & DeleteFileOptions.DeleteEmptyDirectories) == DeleteFileOptions.DeleteEmptyDirectories;
                checkBoxForceReadonly.Checked = (value & DeleteFileOptions.DeleteReadonly) == DeleteFileOptions.DeleteReadonly;
                checkBoxRecursive.Checked = (value & DeleteFileOptions.RecursiveDeleteFiles) == DeleteFileOptions.RecursiveDeleteFiles;
            }
        }
    }
}
