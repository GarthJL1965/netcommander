using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace netCommander
{
    public partial class FileAttributesEditDialog : Form
    {
        public FileAttributesEditDialog()
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

            checkBoxArchive.Text = Options.GetLiteral(Options.LANG_FILE_ATTRIBUTE_ARCHIVE);
            toolTip1.SetToolTip(checkBoxArchive, Options.GetLiteral(Options.LANG_FILE_ATTRIBUTE_ARCHIVE_TOOLTIP));
            checkBoxCompressed.Text = Options.GetLiteral(Options.LANG_FILE_ATTRIBUTE_COMPRESSED);
            toolTip1.SetToolTip(checkBoxCompressed, Options.GetLiteral(Options.LANG_FILE_ATTRIBUTE_COMPRESSED_TOOLTIP));
            checkBoxDirectory.Text = Options.GetLiteral(Options.LANG_FILE_ATTRIBUTE_DIRECTORY);
            toolTip1.SetToolTip(checkBoxDirectory, Options.GetLiteral(Options.LANG_FILE_ATTRIBUTE_DIRECTORY_TOOLTIP));
            checkBoxEncrypted.Text = Options.GetLiteral(Options.LANG_FILE_ATTRIBUTE_ENCRYPTED);
            toolTip1.SetToolTip(checkBoxEncrypted, Options.GetLiteral(Options.LANG_FILE_ATTRIBUTE_ENCRYPTED_TOOLTIP));
            checkBoxHidden.Text = Options.GetLiteral(Options.LANG_FILE_ATTRIBUTE_HIDDEN);
            toolTip1.SetToolTip(checkBoxHidden, Options.GetLiteral(Options.LANG_FILE_ATTRIBUTE_HIDDEN_TOOLTIP));
            checkBoxNormal.Text = Options.GetLiteral(Options.LANG_FILE_ATTRIBUTE_NORMAL);
            toolTip1.SetToolTip(checkBoxNormal, Options.GetLiteral(Options.LANG_FILE_ATTRIBUTE_NORMAL_TOOLTIP));
            checkBoxNotcontentIndexed.Text = Options.GetLiteral(Options.LANG_FILE_ATTRIBUTE_NOT_CONTENT_INDEXED);
            toolTip1.SetToolTip(checkBoxNotcontentIndexed, Options.GetLiteral(Options.LANG_FILE_ATTRIBUTE_NOT_CONTENT_INDEXED_TOOLTIP));
            checkBoxOffline.Text = Options.GetLiteral(Options.LANG_FILE_ATTRIBUTE_OFFLINE);
            toolTip1.SetToolTip(checkBoxOffline, Options.GetLiteral(Options.LANG_FILE_ATTRIBUTE_OFFLINE_TOOLTIP));
            checkBoxReadonly.Text = Options.GetLiteral(Options.LANG_FILE_ATTRIBUTE_READONLY);
            toolTip1.SetToolTip(checkBoxReadonly, Options.GetLiteral(Options.LANG_FILE_ATTRIBUTE_READONLY_TOOLTIP));
            checkBoxReparsePoint.Text = Options.GetLiteral(Options.LANG_FILE_ATTRIBUTE_REPARSE_POINT);
            toolTip1.SetToolTip(checkBoxReparsePoint, Options.GetLiteral(Options.LANG_FILE_ATTRIBUTE_REPARSE_POINT_TOOLTIP));
            checkBoxSparseFile.Text = Options.GetLiteral(Options.LANG_FILE_ATTRIBUTE_SPARSE);
            toolTip1.SetToolTip(checkBoxSparseFile, Options.GetLiteral(Options.LANG_FILE_ATTRIBUTE_SPARSE_TOOLTIP));
            checkBoxSystem.Text = Options.GetLiteral(Options.LANG_FILE_ATTRIBUTE_SYSTEM);
            toolTip1.SetToolTip(checkBoxSystem, Options.GetLiteral(Options.LANG_FILE_ATTRIBUTE_SYSTEM_TOOLTIP));
            checkBoxTemporary.Text = Options.GetLiteral(Options.LANG_FILE_ATTRIBUTE_TEMPORARY);
            toolTip1.SetToolTip(checkBoxTemporary, Options.GetLiteral(Options.LANG_FILE_ATTRIBUTE_TEMPORARY_TOOLTIP));
            buttonOK.Text = Options.GetLiteral(Options.LANG_OK);
            buttonClear.Text = Options.GetLiteral(Options.LANG_CLEAR);
            buttonCancel.Text = Options.GetLiteral(Options.LANG_CANCEL);
        }
    }
}
