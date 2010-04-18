using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace netCommander
{
    public partial class ArchiveAddDialog : Form
    {
        public ArchiveAddDialog()
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

            checkBoxRecursive.Text = Options.GetLiteral(Options.LANG_PROCESS_RECURSIVELY);
            checkBoxSaveAttributes.Text = Options.GetLiteral(Options.LANG_PRESERVE_ATTRIBUTES);
            checkBoxSupressErrors.Text = Options.GetLiteral(Options.LANG_SUPRESS_ERRORS);
            groupBox1.Text = Options.GetLiteral(Options.LANG_OVERWRITE_EXISTING_ENTRIES);
            radioButtonRewriteAll.Text = Options.GetLiteral(Options.LANG_OVERWITE_ALWAYS);
            radioButtonRewriteIfSourceNewer.Text = Options.GetLiteral(Options.LANG_OVERWRITE_ONLY_IF_SOURCE_NEWER);
            radioRewriteNo.Text = Options.GetLiteral(Options.LANG_NOT_OVERWRITE);
            buttonCancel.Text = Options.GetLiteral(Options.LANG_CANCEL);
            buttonOK.Text = Options.GetLiteral(Options.LANG_OK);
        }

        public ArchiveAddOptions ArchiveAddOptions
        {
            get
            {
                ArchiveAddOptions ret = ArchiveAddOptions.None;
                ret = checkBoxRecursive.Checked ? ret | ArchiveAddOptions.Recursive : ret;
                ret = checkBoxSaveAttributes.Checked ? ret | ArchiveAddOptions.SaveAttributes : ret;
                ret = checkBoxSupressErrors.Checked ? ret | ArchiveAddOptions.SupressErrors : ret;
                ret = radioButtonRewriteAll.Checked ? ret | ArchiveAddOptions.RewriteAlways : ret;
                ret = radioButtonRewriteIfSourceNewer.Checked ? ret | ArchiveAddOptions.RewriteIfSourceNewer : ret;
                ret = radioRewriteNo.Checked ? ret | ArchiveAddOptions.NeverRewrite : ret;
                return ret;
            }
            set
            {
                checkBoxRecursive.Checked = (value & ArchiveAddOptions.Recursive) == ArchiveAddOptions.Recursive;
                checkBoxSaveAttributes.Checked = (value & ArchiveAddOptions.SaveAttributes) == ArchiveAddOptions.SaveAttributes;
                checkBoxSupressErrors.Checked = (value & ArchiveAddOptions.SupressErrors) == ArchiveAddOptions.SupressErrors;
                radioButtonRewriteAll.Checked = (value & ArchiveAddOptions.RewriteAlways) == ArchiveAddOptions.RewriteAlways;
                radioButtonRewriteIfSourceNewer.Checked = (value & ArchiveAddOptions.RewriteIfSourceNewer) == ArchiveAddOptions.RewriteIfSourceNewer;
                radioRewriteNo.Checked = (value & ArchiveAddOptions.NeverRewrite) == ArchiveAddOptions.NeverRewrite;
            }
        }
    }
}
