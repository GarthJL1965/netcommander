using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.IO;
using netCommander.FileSystemEx;
using netCommander.FileView;
using System.Runtime.InteropServices;

namespace netCommander
{
    public partial class FileViewEditDialog : Form
    {
        public FileViewEditDialog()
        {
            InitializeComponent();
            set_lang();
            init_encoding_list();
            build_context_menu();
            Options.SetWindowState("View", this);
        }

        private void set_lang()
        {
            if (LicenseManager.UsageMode == LicenseUsageMode.Designtime)
            {
                return;
            }

            
            TextViewControl.Font = Options.FontView;

            //ItemColors colors = Options.GetItemColors(string.Empty, netCommander.winControls.ItemState.None, ItemCategory.Default);
            //TextViewControl.ForeColor = colors.ForegroundColor;
            //TextViewControl.BackColor = colors.BackgroundColor;

            menuItemFont.Text = Options.GetLiteral(Options.LANG_PANEL_FONT);
            menuItemFind.Text = Options.GetLiteral(Options.LANG_TEXT_FIND);
            menuItemFindNext.Text = Options.GetLiteral(Options.LANG_TEXT_FIND_NEXT);
            menuItemOptions.Text = Options.GetLiteral(Options.LANG_OPTIONS);
            menuItemEdit.Text = Options.GetLiteral(Options.LANG_EDIT);
            menuItemCopyAnsi.Text = Options.GetLiteral(Options.LANG_COPY_AS_ANSI);
            menuItemCopyUTF16.Text = Options.GetLiteral(Options.LANG_COPY_AS_UTF16);
            menuItemEncoding.Text = Options.GetLiteral(Options.LANG_TEXT_ENCODING);
            menuItemEncodingAnsi.Text = Options.GetLiteral(Options.LANG_TEXT_ENCODING_ANSI);
            menuItemEncodingASCII.Text = Options.GetLiteral(Options.LANG_TEXT_ENCODING_ASCII);
            menuItemEncodingOther.Text = Options.GetLiteral(Options.LANG_TEXT_ENCODING_OTHER);
            menuItemEncodingUTF16.Text = Options.GetLiteral(Options.LANG_TEXT_ENCODING_UTF16);
            menuItemEncodingUTF8.Text = Options.GetLiteral(Options.LANG_TEXT_ENCODING_UTF8);
        }

        private string file_name = string.Empty;
        private ITextProvider text_provider = null;

        private void build_context_menu()
        {
            foreach (MenuItem menu_item in menuItemEdit.MenuItems)
            {
                MenuItem clone = menu_item.CloneMenu();
                contextMenu1.MenuItems.Add(clone);
            }

            TextViewControl.ContextMenu = contextMenu1;
        }

        public void OpenView(string FileName)
        {
            open_view(FileName, Encoding.GetEncoding(Options.CodePage));
        }

        //VERY buggy
        //private void open_edit(string file_name)
        //{
        //    this.file_name = file_name;

        //    EditStream fs = new EditStream(file_name);
        //    Encoding enc = ((Encoding_DisplayInfo)toolStripComboBoxEncoding.SelectedItem).EncodingInfo.GetEncoding();
        //    if (text_provider != null)
        //    {
        //        text_provider.Dispose();
        //    }
        //    text_provider = TextProviderFactory.CreateProvider(fs, enc);
        //    TextViewControl.TextProvider = text_provider;
        //    Text = string.Format(Options.GetLiteral(Options.LANG_FILE_EDIT) + " [{0}]", file_name);

        //    //saveAsToolStripMenuItem.Enabled = true;
        //    //saveToolStripMenuItem.Enabled = true;
        //}

        private void open_view(string file_name,Encoding encoding)
        {
            this.file_name = file_name;

            FileStream fs = WinAPiFSwrapper.CreateStreamEx
                (file_name,
                FileAccess.Read,
                FileShare.Read,
                FileMode.Open,
                CreateFileOptions.RANDOM_ACCESS);

            text_provider = TextProviderFactory.CreateProvider(fs, encoding);
            TextViewControl.TextProvider = text_provider;
            Text = string.Format(Options.GetLiteral(Options.LANG_FILE_VIEW) + " [{0}]", file_name);
        }

        #region encoding handling
        private void init_encoding_list()
        {
            Encoding_DisplayInfo[] enc_infos = encoding_helper.GetEncodings();
            int def_codepage = Options.CodePage;

            foreach (Encoding_DisplayInfo info in enc_infos)
            {
                MenuItem encoding_menu_item = new MenuItem();
                encoding_menu_item.Text = info.ToString();
                encoding_menu_item.Tag = info;
                encoding_menu_item.Click += new EventHandler(encoding_menu_item_Click);
                if (info.EncodingInfo.CodePage == def_codepage)
                {
                    encoding_menu_item.Checked = true;
                }
                menuItemEncodingOther.MenuItems.Add(encoding_menu_item);
            }
        }

        private void change_encoding(int new_codepage)
        {
            if (text_provider == null)
            {
                return;
            }

            text_provider.Dispose();
            open_view(file_name, Encoding.GetEncoding(new_codepage));

            foreach (MenuItem menu_item in menuItemEncodingOther.MenuItems)
            {
                if (menu_item.Checked)
                {
                    menu_item.Checked = false;
                }

                if (((Encoding_DisplayInfo)menu_item.Tag).EncodingInfo.CodePage == new_codepage)
                {
                    menu_item.Checked = true;
                }
            }

            Options.CodePage = new_codepage;
        }

        void encoding_menu_item_Click(object sender, EventArgs e)
        {
            MenuItem src = (MenuItem)sender;
            Encoding_DisplayInfo d_info = (Encoding_DisplayInfo)src.Tag;
            change_encoding(d_info.EncodingInfo.CodePage);
        }

        private void menuItemEncodingAnsi_Click(object sender, EventArgs e)
        {
            Encoding new_enc = Encoding.Default;
            int new_codepage = new_enc.CodePage;
            change_encoding(new_codepage);
        }

        private void menuItemEncodingOem_Click(object sender, EventArgs e)
        {
            //ASCII
            Encoding new_enc = Encoding.ASCII;
            int new_codepage = new_enc.CodePage;
            change_encoding(new_codepage);
        }

        private void menuItemEncodingUTF8_Click(object sender, EventArgs e)
        {
            Encoding new_enc = Encoding.UTF8;
            int new_codepage = new_enc.CodePage;
            change_encoding(new_codepage);
        }

        private void menuItemEncodingUTF16_Click(object sender, EventArgs e)
        {
            Encoding new_enc = Encoding.Unicode;
            int new_codepage = new_enc.CodePage;
            change_encoding(new_codepage);
        }
 
        #endregion

        #region font handling

        private void change_font()
        {
            fontDialog1.Font = TextViewControl.Font;

            if (fontDialog1.ShowDialog() != DialogResult.OK)
            {
                return;
            }

            if (TextViewControl.Font != fontDialog1.Font)
            {
                TextViewControl.Font = fontDialog1.Font;
                Options.FontView = TextViewControl.Font;
            }
        }

        private void menuItemFont_Click(object sender, EventArgs e)
        {
            change_font();
        }
        #endregion

        #region exiting
        protected override bool ProcessDialogKey(Keys keyData)
        {
            if (keyData == Keys.Escape)
            {
                Close();
            }


            return base.ProcessDialogKey(keyData);
        }

        private void FileViewEditDialog_FormClosing(object sender, FormClosingEventArgs e)
        {
            Options.SaveWindowState("View", this);

            if ((e.CloseReason == CloseReason.TaskManagerClosing) || (e.CloseReason == CloseReason.WindowsShutDown))
            {
                return;
            }

            if (!(TextViewControl.EditEnable))
            {
                return;
            }


            EditStream edit_stream = (EditStream)text_provider.GetBaseStream();
            if (edit_stream.IsHaveChanges)
            {
                if (Messages.ShowQuestionYesNo
                    (Options.GetLiteral(Options.LANG_HAVE_UNSAVED_CHANGES),
                    Options.GetLiteral(Options.LANG_FILE_EDIT)) == DialogResult.No)
                {
                    e.Cancel = true;
                }
            }

        }
        #endregion

        #region view control intract
        private void TextViewControl_ViewChanged(object sender, EventArgs e)
        {
            //set status info
            try
            {
                char char_at_caret = TextViewControl.CharAtCaret;

                toolStripStatusLabelProcent.Text = string.Format("{0:P}", TextViewControl.ViewPercent);
                toolStripStatusLabelCharAtCaret.Text = string.Format("[{0}] [U+{1:X4}]", char.GetUnicodeCategory(char_at_caret), char.ConvertToUtf32(char_at_caret.ToString(), 0));
                toolStripStatusLabelPosition.Text = string.Format("{0:N0} / {1:N0}", TextViewControl.CharIndexAtCaret, TextViewControl.CharsTotal);

                Encoding enc = text_provider.GetEncoding();
                byte[] char_bytes = enc.GetBytes(new char[] { char_at_caret });
                string bytes_string = string.Empty;
                for (int i = 0; i < char_bytes.Length; i++)
                {
                    bytes_string = bytes_string + string.Format("{0:X2} ", char_bytes[i]);
                }
                toolStripStatusLabelBytes.Text = bytes_string;
            }
            catch (Exception ex)
            {
                //and fallback
                toolStripStatusLabelCharAtCaret.Text = ex.Message;
            }
        }
        #endregion

        #region find text
        string find_string = string.Empty;

        private void find_next()
        {
            if (find_string == string.Empty)
            {
                return;
            }

            if (text_provider == null)
            {
                return;
            }

            TextViewControl.GotoText(find_string);
        }

        private void find()
        {
            TextFindDialog dialog = new TextFindDialog();
            dialog.textBoxTextToFind.Text = find_string;

            if (dialog.ShowDialog() != DialogResult.OK)
            {
                return;
            }

            find_string = dialog.textBoxTextToFind.Text;
            find_next();
        }

        private void menuItemFind_Click(object sender, EventArgs e)
        {
            find();
        }

        private void menuItemFindNext_Click(object sender, EventArgs e)
        {
            find_next();
        }

        #endregion

        #region copy
        private void menuItemCopyAnsi_Click(object sender, EventArgs e)
        {
            copy_selection(false);
        }

        private void menuItemCopyUTF16_Click(object sender, EventArgs e)
        {
            copy_selection(true);
        }

        private void copy_selection(bool unicode)
        {
            try
            {
                int selection_len = (int)(TextViewControl.SelectionEnd - TextViewControl.SelectionStart);

                if (selection_len <= 0)
                {
                    return;
                }

                IntPtr char_buffer = IntPtr.Zero;
                int chars_filled = 0;
                bool eof = false;

                try
                {
                    text_provider.GetCharParagraph
                        (TextViewControl.SelectionStart,
                        selection_len,
                        false,
                        out char_buffer,
                        out chars_filled,
                        out eof);

                    string sel_string = string.Empty;
                    sel_string = Marshal.PtrToStringUni(char_buffer, chars_filled);
                    if (unicode)
                    {
                        Clipboard.SetText(sel_string, TextDataFormat.UnicodeText);
                    }
                    else
                    {
                        Clipboard.SetText(sel_string, TextDataFormat.Text);
                    }
                }
                finally
                {
                    if (char_buffer != IntPtr.Zero)
                    {
                        //Marshal.FreeHGlobal(char_buffer);
                        //буфер не надо освобождать
                    }
                }
            }
            catch (Exception ex)
            {
                Messages.ShowException(ex);
            }
        }
        #endregion

        



        

        

        

       


    }
}
