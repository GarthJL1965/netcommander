namespace netCommander
{
    partial class ExtractDialog
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
            this.panelButton = new System.Windows.Forms.Panel();
            this.buttonOK = new System.Windows.Forms.Button();
            this.buttonCancel = new System.Windows.Forms.Button();
            this.textBoxSourceMask = new System.Windows.Forms.TextBox();
            this.labelSourceMask = new System.Windows.Forms.Label();
            this.textBoxDestination = new System.Windows.Forms.TextBox();
            this.labelDestination = new System.Windows.Forms.Label();
            this.checkBoxSupressErrors = new System.Windows.Forms.CheckBox();
            this.checkBoxExtractAttributes = new System.Windows.Forms.CheckBox();
            this.checkBoxCreateEmptyDirs = new System.Windows.Forms.CheckBox();
            this.checkBoxRecursively = new System.Windows.Forms.CheckBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.radioButtonRewriteAll = new System.Windows.Forms.RadioButton();
            this.radioButtonRewriteIfSourceNewer = new System.Windows.Forms.RadioButton();
            this.radioRewriteNo = new System.Windows.Forms.RadioButton();
            this.panelButton.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // panelButton
            // 
            this.panelButton.Controls.Add(this.buttonOK);
            this.panelButton.Controls.Add(this.buttonCancel);
            this.panelButton.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panelButton.Location = new System.Drawing.Point(0, 188);
            this.panelButton.Name = "panelButton";
            this.panelButton.Size = new System.Drawing.Size(372, 35);
            this.panelButton.TabIndex = 9;
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
            // textBoxSourceMask
            // 
            this.textBoxSourceMask.Location = new System.Drawing.Point(96, 12);
            this.textBoxSourceMask.Name = "textBoxSourceMask";
            this.textBoxSourceMask.Size = new System.Drawing.Size(264, 20);
            this.textBoxSourceMask.TabIndex = 1;
            // 
            // labelSourceMask
            // 
            this.labelSourceMask.Location = new System.Drawing.Point(13, 15);
            this.labelSourceMask.Name = "labelSourceMask";
            this.labelSourceMask.Size = new System.Drawing.Size(77, 23);
            this.labelSourceMask.TabIndex = 0;
            this.labelSourceMask.Text = "Source mask:";
            this.labelSourceMask.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // textBoxDestination
            // 
            this.textBoxDestination.Enabled = false;
            this.textBoxDestination.Location = new System.Drawing.Point(96, 38);
            this.textBoxDestination.Name = "textBoxDestination";
            this.textBoxDestination.Size = new System.Drawing.Size(264, 20);
            this.textBoxDestination.TabIndex = 3;
            // 
            // labelDestination
            // 
            this.labelDestination.Location = new System.Drawing.Point(15, 41);
            this.labelDestination.Name = "labelDestination";
            this.labelDestination.Size = new System.Drawing.Size(75, 23);
            this.labelDestination.TabIndex = 2;
            this.labelDestination.Text = "Destination:";
            this.labelDestination.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // checkBoxSupressErrors
            // 
            this.checkBoxSupressErrors.AutoSize = true;
            this.checkBoxSupressErrors.Location = new System.Drawing.Point(18, 68);
            this.checkBoxSupressErrors.Name = "checkBoxSupressErrors";
            this.checkBoxSupressErrors.Size = new System.Drawing.Size(91, 17);
            this.checkBoxSupressErrors.TabIndex = 4;
            this.checkBoxSupressErrors.Text = "supress errors";
            this.checkBoxSupressErrors.UseVisualStyleBackColor = true;
            // 
            // checkBoxExtractAttributes
            // 
            this.checkBoxExtractAttributes.AutoSize = true;
            this.checkBoxExtractAttributes.Enabled = false;
            this.checkBoxExtractAttributes.Location = new System.Drawing.Point(18, 92);
            this.checkBoxExtractAttributes.Name = "checkBoxExtractAttributes";
            this.checkBoxExtractAttributes.Size = new System.Drawing.Size(104, 17);
            this.checkBoxExtractAttributes.TabIndex = 5;
            this.checkBoxExtractAttributes.Text = "extract attributes";
            this.checkBoxExtractAttributes.UseVisualStyleBackColor = true;
            // 
            // checkBoxCreateEmptyDirs
            // 
            this.checkBoxCreateEmptyDirs.AutoSize = true;
            this.checkBoxCreateEmptyDirs.Enabled = false;
            this.checkBoxCreateEmptyDirs.Location = new System.Drawing.Point(196, 68);
            this.checkBoxCreateEmptyDirs.Name = "checkBoxCreateEmptyDirs";
            this.checkBoxCreateEmptyDirs.Size = new System.Drawing.Size(108, 17);
            this.checkBoxCreateEmptyDirs.TabIndex = 6;
            this.checkBoxCreateEmptyDirs.Text = "extract empty dirs";
            this.checkBoxCreateEmptyDirs.UseVisualStyleBackColor = true;
            // 
            // checkBoxRecursively
            // 
            this.checkBoxRecursively.AutoSize = true;
            this.checkBoxRecursively.Enabled = false;
            this.checkBoxRecursively.Location = new System.Drawing.Point(196, 92);
            this.checkBoxRecursively.Name = "checkBoxRecursively";
            this.checkBoxRecursively.Size = new System.Drawing.Size(76, 17);
            this.checkBoxRecursively.TabIndex = 7;
            this.checkBoxRecursively.Text = "recursively";
            this.checkBoxRecursively.UseVisualStyleBackColor = true;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.radioButtonRewriteAll);
            this.groupBox1.Controls.Add(this.radioButtonRewriteIfSourceNewer);
            this.groupBox1.Controls.Add(this.radioRewriteNo);
            this.groupBox1.Location = new System.Drawing.Point(8, 115);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(352, 70);
            this.groupBox1.TabIndex = 8;
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
            // ExtractDialog
            // 
            this.AcceptButton = this.buttonOK;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.buttonCancel;
            this.ClientSize = new System.Drawing.Size(372, 223);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.checkBoxRecursively);
            this.Controls.Add(this.checkBoxCreateEmptyDirs);
            this.Controls.Add(this.checkBoxExtractAttributes);
            this.Controls.Add(this.checkBoxSupressErrors);
            this.Controls.Add(this.textBoxDestination);
            this.Controls.Add(this.labelDestination);
            this.Controls.Add(this.textBoxSourceMask);
            this.Controls.Add(this.labelSourceMask);
            this.Controls.Add(this.panelButton);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.Name = "ExtractDialog";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "ExtractDialog";
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
        public System.Windows.Forms.TextBox textBoxSourceMask;
        private System.Windows.Forms.Label labelSourceMask;
        public System.Windows.Forms.TextBox textBoxDestination;
        private System.Windows.Forms.Label labelDestination;
        private System.Windows.Forms.CheckBox checkBoxSupressErrors;
        private System.Windows.Forms.CheckBox checkBoxExtractAttributes;
        private System.Windows.Forms.CheckBox checkBoxCreateEmptyDirs;
        private System.Windows.Forms.CheckBox checkBoxRecursively;
        private System.Windows.Forms.GroupBox groupBox1;
        public System.Windows.Forms.RadioButton radioButtonRewriteAll;
        public System.Windows.Forms.RadioButton radioButtonRewriteIfSourceNewer;
        public System.Windows.Forms.RadioButton radioRewriteNo;
    }
}