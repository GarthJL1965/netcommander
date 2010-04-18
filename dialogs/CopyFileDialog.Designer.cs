namespace netCommander
{
    partial class CopyFileDialog
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
            this.panelButton = new System.Windows.Forms.Panel();
            this.buttonOK = new System.Windows.Forms.Button();
            this.buttonCancel = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.textBoxDestination = new System.Windows.Forms.TextBox();
            this.checkBoxAllowDecryptedDestination = new System.Windows.Forms.CheckBox();
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.checkBoxCopySymlink = new System.Windows.Forms.CheckBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.radioButtonRewriteAll = new System.Windows.Forms.RadioButton();
            this.radioButtonRewriteIfSourceNewer = new System.Windows.Forms.RadioButton();
            this.radioRewriteNo = new System.Windows.Forms.RadioButton();
            this.checkBoxClearAttributes = new System.Windows.Forms.CheckBox();
            this.checkBoxSupressErrors = new System.Windows.Forms.CheckBox();
            this.checkBoxCopySecurityAttributes = new System.Windows.Forms.CheckBox();
            this.checkBoxShowTotalProgress = new System.Windows.Forms.CheckBox();
            this.textBoxSourceMask = new System.Windows.Forms.TextBox();
            this.checkBoxCopyRecursively = new System.Windows.Forms.CheckBox();
            this.checkBoxCopyEmptyDirs = new System.Windows.Forms.CheckBox();
            this.panelButton.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // panelButton
            // 
            this.panelButton.Controls.Add(this.buttonOK);
            this.panelButton.Controls.Add(this.buttonCancel);
            this.panelButton.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panelButton.Location = new System.Drawing.Point(0, 254);
            this.panelButton.Name = "panelButton";
            this.panelButton.Size = new System.Drawing.Size(372, 35);
            this.panelButton.TabIndex = 13;
            // 
            // buttonOK
            // 
            this.buttonOK.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.buttonOK.Location = new System.Drawing.Point(208, 6);
            this.buttonOK.Name = "buttonOK";
            this.buttonOK.Size = new System.Drawing.Size(75, 23);
            this.buttonOK.TabIndex = 0;
            this.buttonOK.Text = "OK";
            this.buttonOK.UseVisualStyleBackColor = true;
            // 
            // buttonCancel
            // 
            this.buttonCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.buttonCancel.Location = new System.Drawing.Point(289, 6);
            this.buttonCancel.Name = "buttonCancel";
            this.buttonCancel.Size = new System.Drawing.Size(75, 23);
            this.buttonCancel.TabIndex = 1;
            this.buttonCancel.Text = "Cancel";
            this.buttonCancel.UseVisualStyleBackColor = true;
            // 
            // label2
            // 
            this.label2.Location = new System.Drawing.Point(13, 13);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(77, 23);
            this.label2.TabIndex = 0;
            this.label2.Text = "Source mask:";
            this.label2.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // label1
            // 
            this.label1.Location = new System.Drawing.Point(15, 39);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(75, 23);
            this.label1.TabIndex = 2;
            this.label1.Text = "Destination:";
            this.label1.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // textBoxDestination
            // 
            this.textBoxDestination.Location = new System.Drawing.Point(96, 36);
            this.textBoxDestination.Name = "textBoxDestination";
            this.textBoxDestination.Size = new System.Drawing.Size(264, 20);
            this.textBoxDestination.TabIndex = 3;
            // 
            // checkBoxAllowDecryptedDestination
            // 
            this.checkBoxAllowDecryptedDestination.AutoSize = true;
            this.checkBoxAllowDecryptedDestination.Location = new System.Drawing.Point(16, 65);
            this.checkBoxAllowDecryptedDestination.Name = "checkBoxAllowDecryptedDestination";
            this.checkBoxAllowDecryptedDestination.Size = new System.Drawing.Size(89, 17);
            this.checkBoxAllowDecryptedDestination.TabIndex = 4;
            this.checkBoxAllowDecryptedDestination.Text = "Allow decrypt";
            this.toolTip1.SetToolTip(this.checkBoxAllowDecryptedDestination, "An attempt to copy an encrypted file will succeed\r\neven if the destination copy c" +
                    "annot be encrypted.");
            this.checkBoxAllowDecryptedDestination.UseVisualStyleBackColor = true;
            // 
            // checkBoxCopySymlink
            // 
            this.checkBoxCopySymlink.AutoSize = true;
            this.checkBoxCopySymlink.Location = new System.Drawing.Point(187, 65);
            this.checkBoxCopySymlink.Name = "checkBoxCopySymlink";
            this.checkBoxCopySymlink.Size = new System.Drawing.Size(138, 17);
            this.checkBoxCopySymlink.TabIndex = 9;
            this.checkBoxCopySymlink.Text = "Copy symlink as symlink";
            this.toolTip1.SetToolTip(this.checkBoxCopySymlink, "If the source file is a symbolic link, the destination file is also a symbolic li" +
                    "nk\r\npointing to the same file that the source symbolic link is pointing to.\r\nNot" +
                    " supported in Windows 2000/XP/2003 ");
            this.checkBoxCopySymlink.UseVisualStyleBackColor = true;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.radioButtonRewriteAll);
            this.groupBox1.Controls.Add(this.radioButtonRewriteIfSourceNewer);
            this.groupBox1.Controls.Add(this.radioRewriteNo);
            this.groupBox1.Location = new System.Drawing.Point(12, 181);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(352, 70);
            this.groupBox1.TabIndex = 12;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Rewrite existing files";
            // 
            // radioButtonRewriteAll
            // 
            this.radioButtonRewriteAll.AutoSize = true;
            this.radioButtonRewriteAll.Location = new System.Drawing.Point(187, 20);
            this.radioButtonRewriteAll.Name = "radioButtonRewriteAll";
            this.radioButtonRewriteAll.Size = new System.Drawing.Size(95, 17);
            this.radioButtonRewriteAll.TabIndex = 2;
            this.radioButtonRewriteAll.TabStop = true;
            this.radioButtonRewriteAll.Text = "Rewrite all files";
            this.radioButtonRewriteAll.UseVisualStyleBackColor = true;
            // 
            // radioButtonRewriteIfSourceNewer
            // 
            this.radioButtonRewriteIfSourceNewer.AutoSize = true;
            this.radioButtonRewriteIfSourceNewer.Location = new System.Drawing.Point(16, 44);
            this.radioButtonRewriteIfSourceNewer.Name = "radioButtonRewriteIfSourceNewer";
            this.radioButtonRewriteIfSourceNewer.Size = new System.Drawing.Size(136, 17);
            this.radioButtonRewriteIfSourceNewer.TabIndex = 1;
            this.radioButtonRewriteIfSourceNewer.TabStop = true;
            this.radioButtonRewriteIfSourceNewer.Text = "Rewrite if source newer";
            this.radioButtonRewriteIfSourceNewer.UseVisualStyleBackColor = true;
            // 
            // radioRewriteNo
            // 
            this.radioRewriteNo.AutoSize = true;
            this.radioRewriteNo.Location = new System.Drawing.Point(16, 20);
            this.radioRewriteNo.Name = "radioRewriteNo";
            this.radioRewriteNo.Size = new System.Drawing.Size(73, 17);
            this.radioRewriteNo.TabIndex = 0;
            this.radioRewriteNo.TabStop = true;
            this.radioRewriteNo.Text = "No rewrite";
            this.radioRewriteNo.UseVisualStyleBackColor = true;
            // 
            // checkBoxClearAttributes
            // 
            this.checkBoxClearAttributes.AutoSize = true;
            this.checkBoxClearAttributes.Location = new System.Drawing.Point(16, 158);
            this.checkBoxClearAttributes.Name = "checkBoxClearAttributes";
            this.checkBoxClearAttributes.Size = new System.Drawing.Size(264, 17);
            this.checkBoxClearAttributes.TabIndex = 8;
            this.checkBoxClearAttributes.Text = "Clear readonly and hidden attributes while rewriting";
            this.checkBoxClearAttributes.UseVisualStyleBackColor = true;
            // 
            // checkBoxSupressErrors
            // 
            this.checkBoxSupressErrors.AutoSize = true;
            this.checkBoxSupressErrors.Location = new System.Drawing.Point(16, 89);
            this.checkBoxSupressErrors.Name = "checkBoxSupressErrors";
            this.checkBoxSupressErrors.Size = new System.Drawing.Size(93, 17);
            this.checkBoxSupressErrors.TabIndex = 5;
            this.checkBoxSupressErrors.Text = "Supress errors";
            this.checkBoxSupressErrors.UseVisualStyleBackColor = true;
            // 
            // checkBoxCopySecurityAttributes
            // 
            this.checkBoxCopySecurityAttributes.AutoSize = true;
            this.checkBoxCopySecurityAttributes.Location = new System.Drawing.Point(187, 89);
            this.checkBoxCopySecurityAttributes.Name = "checkBoxCopySecurityAttributes";
            this.checkBoxCopySecurityAttributes.Size = new System.Drawing.Size(135, 17);
            this.checkBoxCopySecurityAttributes.TabIndex = 10;
            this.checkBoxCopySecurityAttributes.Text = "Copy security attributes";
            this.checkBoxCopySecurityAttributes.UseVisualStyleBackColor = true;
            // 
            // checkBoxShowTotalProgress
            // 
            this.checkBoxShowTotalProgress.AutoSize = true;
            this.checkBoxShowTotalProgress.Location = new System.Drawing.Point(16, 135);
            this.checkBoxShowTotalProgress.Name = "checkBoxShowTotalProgress";
            this.checkBoxShowTotalProgress.Size = new System.Drawing.Size(119, 17);
            this.checkBoxShowTotalProgress.TabIndex = 7;
            this.checkBoxShowTotalProgress.Text = "Show total progress";
            this.checkBoxShowTotalProgress.UseVisualStyleBackColor = true;
            // 
            // textBoxSourceMask
            // 
            this.textBoxSourceMask.Location = new System.Drawing.Point(96, 10);
            this.textBoxSourceMask.Name = "textBoxSourceMask";
            this.textBoxSourceMask.Size = new System.Drawing.Size(264, 20);
            this.textBoxSourceMask.TabIndex = 1;
            // 
            // checkBoxCopyRecursively
            // 
            this.checkBoxCopyRecursively.AutoSize = true;
            this.checkBoxCopyRecursively.Location = new System.Drawing.Point(16, 112);
            this.checkBoxCopyRecursively.Name = "checkBoxCopyRecursively";
            this.checkBoxCopyRecursively.Size = new System.Drawing.Size(124, 17);
            this.checkBoxCopyRecursively.TabIndex = 6;
            this.checkBoxCopyRecursively.Text = "Copy files recursively";
            this.checkBoxCopyRecursively.UseVisualStyleBackColor = true;
            // 
            // checkBoxCopyEmptyDirs
            // 
            this.checkBoxCopyEmptyDirs.AutoSize = true;
            this.checkBoxCopyEmptyDirs.Location = new System.Drawing.Point(187, 113);
            this.checkBoxCopyEmptyDirs.Name = "checkBoxCopyEmptyDirs";
            this.checkBoxCopyEmptyDirs.Size = new System.Drawing.Size(132, 17);
            this.checkBoxCopyEmptyDirs.TabIndex = 11;
            this.checkBoxCopyEmptyDirs.Text = "Copy empty directories";
            this.checkBoxCopyEmptyDirs.UseVisualStyleBackColor = true;
            // 
            // CopyFileDialog
            // 
            this.AcceptButton = this.buttonOK;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.buttonCancel;
            this.ClientSize = new System.Drawing.Size(372, 289);
            this.Controls.Add(this.checkBoxCopyEmptyDirs);
            this.Controls.Add(this.checkBoxCopyRecursively);
            this.Controls.Add(this.textBoxSourceMask);
            this.Controls.Add(this.checkBoxShowTotalProgress);
            this.Controls.Add(this.checkBoxCopySecurityAttributes);
            this.Controls.Add(this.checkBoxSupressErrors);
            this.Controls.Add(this.checkBoxClearAttributes);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.checkBoxCopySymlink);
            this.Controls.Add(this.checkBoxAllowDecryptedDestination);
            this.Controls.Add(this.textBoxDestination);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.panelButton);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.Name = "CopyFileDialog";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "CopyFileDialog";
            this.panelButton.ResumeLayout(false);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Panel panelButton;
        public System.Windows.Forms.Button buttonOK;
        private System.Windows.Forms.Button buttonCancel;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        public System.Windows.Forms.TextBox textBoxDestination;
        private System.Windows.Forms.ToolTip toolTip1;
        public System.Windows.Forms.CheckBox checkBoxAllowDecryptedDestination;
        public System.Windows.Forms.CheckBox checkBoxCopySymlink;
        private System.Windows.Forms.GroupBox groupBox1;
        public System.Windows.Forms.RadioButton radioRewriteNo;
        public System.Windows.Forms.RadioButton radioButtonRewriteIfSourceNewer;
        public System.Windows.Forms.RadioButton radioButtonRewriteAll;
        public System.Windows.Forms.CheckBox checkBoxClearAttributes;
        public System.Windows.Forms.CheckBox checkBoxSupressErrors;
        public System.Windows.Forms.CheckBox checkBoxCopySecurityAttributes;
        private System.Windows.Forms.CheckBox checkBoxShowTotalProgress;
        public System.Windows.Forms.TextBox textBoxSourceMask;
        private System.Windows.Forms.CheckBox checkBoxCopyRecursively;
        private System.Windows.Forms.CheckBox checkBoxCopyEmptyDirs;
    }
}