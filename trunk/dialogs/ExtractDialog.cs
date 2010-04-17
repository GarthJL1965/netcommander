using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace netCommander
{
    public partial class ExtractDialog : Form
    {
        public ExtractDialog()
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

            labelDestination.Text = Options.GetLiteral(Options.LANG_DESTINATION);
            labelSourceMask.Text = Options.GetLiteral(Options.LANG_MASK);
            checkBoxCreateEmptyDirs.Text = Options.GetLiteral(Options.LANG_PROCESS_EMPTY_DIRS);
            checkBoxExtractAttributes.Text = Options.GetLiteral(Options.LANG_EXTRACT_ATTRIBUTES);
            checkBoxRecursively.Text = Options.GetLiteral(Options.LANG_PROCESS_RECURSIVELY);
            checkBoxSupressErrors.Text = Options.GetLiteral(Options.LANG_SUPRESS_ERRORS);
            radioButtonRewriteAll.Text = Options.GetLiteral(Options.LANG_OVERWITE_ALWAYS);
            radioButtonRewriteIfSourceNewer.Text = Options.GetLiteral(Options.LANG_OVERWRITE_ONLY_IF_SOURCE_NEWER);
            radioRewriteNo.Text = Options.GetLiteral(Options.LANG_NOT_OVERWRITE);
            buttonCancel.Text = Options.GetLiteral(Options.LANG_CANCEL);
            buttonOK.Text = Options.GetLiteral(Options.LANG_OK);
        }

        public ArchiveExtractOptions ArchiveExtractOptions
        {
            get
            {
                ArchiveExtractOptions ret = ArchiveExtractOptions.None;
                ret = checkBoxCreateEmptyDirs.Checked ? ret | ArchiveExtractOptions.CreateEmptyDirectories : ret;
                ret = checkBoxExtractAttributes.Checked ? ret | ArchiveExtractOptions.ExtractAttributes : ret;
                ret = checkBoxRecursively.Checked ? ret | ArchiveExtractOptions.ExtractRecursively : ret;
                ret = checkBoxSupressErrors.Checked ? ret | ArchiveExtractOptions.SupressExceptions : ret;
                ret = radioButtonRewriteAll.Checked ? ret | ArchiveExtractOptions.OverwriteAlways : ret;
                ret = radioButtonRewriteIfSourceNewer.Checked ? ret | ArchiveExtractOptions.OverwriteIfSourceNewer : ret;
                ret = radioRewriteNo.Checked ? ret | ArchiveExtractOptions.NeverOverwite : ret;
                return ret;
            }
            set
            {
                checkBoxCreateEmptyDirs.Checked = (value & ArchiveExtractOptions.CreateEmptyDirectories) == ArchiveExtractOptions.CreateEmptyDirectories;
                checkBoxExtractAttributes.Checked = (value & ArchiveExtractOptions.ExtractAttributes) == ArchiveExtractOptions.ExtractAttributes;
                checkBoxRecursively.Checked = (value & ArchiveExtractOptions.ExtractRecursively) == ArchiveExtractOptions.ExtractRecursively;
                checkBoxSupressErrors.Checked = (value & ArchiveExtractOptions.SupressExceptions) == ArchiveExtractOptions.SupressExceptions;
                radioButtonRewriteAll.Checked = (value & ArchiveExtractOptions.OverwriteAlways) == ArchiveExtractOptions.OverwriteAlways;
                radioButtonRewriteIfSourceNewer.Checked = (value & ArchiveExtractOptions.OverwriteIfSourceNewer) == ArchiveExtractOptions.OverwriteIfSourceNewer;
                radioRewriteNo.Checked = (value & ArchiveExtractOptions.NeverOverwite) == ArchiveExtractOptions.NeverOverwite;
            }
        }
    }
}
