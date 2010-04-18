namespace netCommander
{
    partial class FileViewEditDialog
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.toolStripStatusLabelCharAtCaret = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolStripStatusLabelBytes = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolStripStatusLabelPosition = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolStripStatusLabelProcent = new System.Windows.Forms.ToolStripStatusLabel();
            this.fontDialog1 = new System.Windows.Forms.FontDialog();
            this.saveFileDialog1 = new System.Windows.Forms.SaveFileDialog();
            this.mainMenu1 = new System.Windows.Forms.MainMenu(this.components);
            this.menuItemOptions = new System.Windows.Forms.MenuItem();
            this.menuItemFont = new System.Windows.Forms.MenuItem();
            this.menuItemEncoding = new System.Windows.Forms.MenuItem();
            this.menuItemEncodingAnsi = new System.Windows.Forms.MenuItem();
            this.menuItemEncodingASCII = new System.Windows.Forms.MenuItem();
            this.menuItemEncodingUTF8 = new System.Windows.Forms.MenuItem();
            this.menuItemEncodingUTF16 = new System.Windows.Forms.MenuItem();
            this.menuItemEncodingOther = new System.Windows.Forms.MenuItem();
            this.menuItemEdit = new System.Windows.Forms.MenuItem();
            this.menuItemCopyAnsi = new System.Windows.Forms.MenuItem();
            this.menuItemCopyUTF16 = new System.Windows.Forms.MenuItem();
            this.menuItemFind = new System.Windows.Forms.MenuItem();
            this.contextMenu1 = new System.Windows.Forms.ContextMenu();
            this.menuItemFindNext = new System.Windows.Forms.MenuItem();
            this.TextViewControl = new netCommander.FileView.mTextViewControl();
            this.statusStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // statusStrip1
            // 
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripStatusLabelCharAtCaret,
            this.toolStripStatusLabelBytes,
            this.toolStripStatusLabelPosition,
            this.toolStripStatusLabelProcent});
            this.statusStrip1.Location = new System.Drawing.Point(0, 167);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(637, 24);
            this.statusStrip1.TabIndex = 0;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // toolStripStatusLabelCharAtCaret
            // 
            this.toolStripStatusLabelCharAtCaret.BorderSides = System.Windows.Forms.ToolStripStatusLabelBorderSides.Right;
            this.toolStripStatusLabelCharAtCaret.Name = "toolStripStatusLabelCharAtCaret";
            this.toolStripStatusLabelCharAtCaret.Size = new System.Drawing.Size(62, 19);
            this.toolStripStatusLabelCharAtCaret.Text = "label char";
            // 
            // toolStripStatusLabelBytes
            // 
            this.toolStripStatusLabelBytes.BorderSides = System.Windows.Forms.ToolStripStatusLabelBorderSides.Right;
            this.toolStripStatusLabelBytes.Name = "toolStripStatusLabelBytes";
            this.toolStripStatusLabelBytes.Size = new System.Drawing.Size(39, 19);
            this.toolStripStatusLabelBytes.Text = "bytes";
            // 
            // toolStripStatusLabelPosition
            // 
            this.toolStripStatusLabelPosition.BorderSides = System.Windows.Forms.ToolStripStatusLabelBorderSides.Right;
            this.toolStripStatusLabelPosition.Name = "toolStripStatusLabelPosition";
            this.toolStripStatusLabelPosition.Size = new System.Drawing.Size(54, 19);
            this.toolStripStatusLabelPosition.Text = "position";
            // 
            // toolStripStatusLabelProcent
            // 
            this.toolStripStatusLabelProcent.Name = "toolStripStatusLabelProcent";
            this.toolStripStatusLabelProcent.Size = new System.Drawing.Size(48, 19);
            this.toolStripStatusLabelProcent.Text = "procent";
            // 
            // mainMenu1
            // 
            this.mainMenu1.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.menuItemOptions,
            this.menuItemEdit});
            // 
            // menuItemOptions
            // 
            this.menuItemOptions.Index = 0;
            this.menuItemOptions.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.menuItemFont,
            this.menuItemEncoding});
            this.menuItemOptions.Text = "opts";
            // 
            // menuItemFont
            // 
            this.menuItemFont.Index = 0;
            this.menuItemFont.Shortcut = System.Windows.Forms.Shortcut.CtrlF;
            this.menuItemFont.Text = "font";
            this.menuItemFont.Click += new System.EventHandler(this.menuItemFont_Click);
            // 
            // menuItemEncoding
            // 
            this.menuItemEncoding.Index = 1;
            this.menuItemEncoding.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.menuItemEncodingAnsi,
            this.menuItemEncodingASCII,
            this.menuItemEncodingUTF8,
            this.menuItemEncodingUTF16,
            this.menuItemEncodingOther});
            this.menuItemEncoding.Text = "encoding";
            // 
            // menuItemEncodingAnsi
            // 
            this.menuItemEncodingAnsi.Index = 0;
            this.menuItemEncodingAnsi.Text = "sys ansi";
            this.menuItemEncodingAnsi.Click += new System.EventHandler(this.menuItemEncodingAnsi_Click);
            // 
            // menuItemEncodingASCII
            // 
            this.menuItemEncodingASCII.Index = 1;
            this.menuItemEncodingASCII.Text = "sys oem";
            this.menuItemEncodingASCII.Click += new System.EventHandler(this.menuItemEncodingOem_Click);
            // 
            // menuItemEncodingUTF8
            // 
            this.menuItemEncodingUTF8.Index = 2;
            this.menuItemEncodingUTF8.Text = "utf8";
            this.menuItemEncodingUTF8.Click += new System.EventHandler(this.menuItemEncodingUTF8_Click);
            // 
            // menuItemEncodingUTF16
            // 
            this.menuItemEncodingUTF16.Index = 3;
            this.menuItemEncodingUTF16.Text = "utf16";
            this.menuItemEncodingUTF16.Click += new System.EventHandler(this.menuItemEncodingUTF16_Click);
            // 
            // menuItemEncodingOther
            // 
            this.menuItemEncodingOther.Index = 4;
            this.menuItemEncodingOther.Text = "other";
            // 
            // menuItemEdit
            // 
            this.menuItemEdit.Index = 1;
            this.menuItemEdit.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.menuItemCopyAnsi,
            this.menuItemCopyUTF16,
            this.menuItemFind,
            this.menuItemFindNext});
            this.menuItemEdit.Text = "edit";
            // 
            // menuItemCopyAnsi
            // 
            this.menuItemCopyAnsi.Index = 0;
            this.menuItemCopyAnsi.Shortcut = System.Windows.Forms.Shortcut.CtrlC;
            this.menuItemCopyAnsi.Text = "copy ansi";
            this.menuItemCopyAnsi.Click += new System.EventHandler(this.menuItemCopyAnsi_Click);
            // 
            // menuItemCopyUTF16
            // 
            this.menuItemCopyUTF16.Index = 1;
            this.menuItemCopyUTF16.Shortcut = System.Windows.Forms.Shortcut.CtrlShiftC;
            this.menuItemCopyUTF16.Text = "copy utf16";
            this.menuItemCopyUTF16.Click += new System.EventHandler(this.menuItemCopyUTF16_Click);
            // 
            // menuItemFind
            // 
            this.menuItemFind.Index = 2;
            this.menuItemFind.Shortcut = System.Windows.Forms.Shortcut.CtrlF3;
            this.menuItemFind.Text = "find";
            this.menuItemFind.Click += new System.EventHandler(this.menuItemFind_Click);
            // 
            // menuItemFindNext
            // 
            this.menuItemFindNext.Index = 3;
            this.menuItemFindNext.Shortcut = System.Windows.Forms.Shortcut.F3;
            this.menuItemFindNext.Text = "find next";
            this.menuItemFindNext.Click += new System.EventHandler(this.menuItemFindNext_Click);
            // 
            // TextViewControl
            // 
            this.TextViewControl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.TextViewControl.Location = new System.Drawing.Point(0, 0);
            this.TextViewControl.Name = "TextViewControl";
            this.TextViewControl.Size = new System.Drawing.Size(637, 167);
            this.TextViewControl.TabIndex = 2;
            this.TextViewControl.Text = "mTextViewControl1";
            this.TextViewControl.TextProvider = null;
            this.TextViewControl.ViewChanged += new System.EventHandler(this.TextViewControl_ViewChanged);
            // 
            // FileViewEditDialog
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(637, 191);
            this.Controls.Add(this.TextViewControl);
            this.Controls.Add(this.statusStrip1);
            this.Menu = this.mainMenu1;
            this.Name = "FileViewEditDialog";
            this.Text = "FileViewEditDialog";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FileViewEditDialog_FormClosing);
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.StatusStrip statusStrip1;
        private netCommander.FileView.mTextViewControl TextViewControl;
        private System.Windows.Forms.FontDialog fontDialog1;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabelProcent;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabelCharAtCaret;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabelPosition;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabelBytes;
        private System.Windows.Forms.SaveFileDialog saveFileDialog1;
        private System.Windows.Forms.MainMenu mainMenu1;
        private System.Windows.Forms.MenuItem menuItemOptions;
        private System.Windows.Forms.MenuItem menuItemFont;
        private System.Windows.Forms.MenuItem menuItemEdit;
        private System.Windows.Forms.MenuItem menuItemCopyAnsi;
        private System.Windows.Forms.MenuItem menuItemCopyUTF16;
        private System.Windows.Forms.MenuItem menuItemFind;
        private System.Windows.Forms.MenuItem menuItemEncoding;
        private System.Windows.Forms.MenuItem menuItemEncodingAnsi;
        private System.Windows.Forms.MenuItem menuItemEncodingASCII;
        private System.Windows.Forms.MenuItem menuItemEncodingUTF8;
        private System.Windows.Forms.MenuItem menuItemEncodingUTF16;
        private System.Windows.Forms.MenuItem menuItemEncodingOther;
        private System.Windows.Forms.ContextMenu contextMenu1;
        private System.Windows.Forms.MenuItem menuItemFindNext;
    }
}