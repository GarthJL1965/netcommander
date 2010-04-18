namespace netCommander
{
    partial class ArchiveAddDialog
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
        	this.label2 = new System.Windows.Forms.Label();
        	this.groupBox1 = new System.Windows.Forms.GroupBox();
        	this.radioButtonRewriteAll = new System.Windows.Forms.RadioButton();
        	this.radioButtonRewriteIfSourceNewer = new System.Windows.Forms.RadioButton();
        	this.radioRewriteNo = new System.Windows.Forms.RadioButton();
        	this.checkBoxSupressErrors = new System.Windows.Forms.CheckBox();
        	this.checkBoxRecursive = new System.Windows.Forms.CheckBox();
        	this.checkBoxSaveAttributes = new System.Windows.Forms.CheckBox();
        	this.panelButton.SuspendLayout();
        	this.groupBox1.SuspendLayout();
        	this.SuspendLayout();
        	// 
        	// panelButton
        	// 
        	this.panelButton.Controls.Add(this.buttonOK);
        	this.panelButton.Controls.Add(this.buttonCancel);
        	this.panelButton.Dock = System.Windows.Forms.DockStyle.Bottom;
        	this.panelButton.Location = new System.Drawing.Point(0, 159);
        	this.panelButton.Name = "panelButton";
        	this.panelButton.Size = new System.Drawing.Size(372, 35);
        	this.panelButton.TabIndex = 6;
        	// 
        	// buttonOK
        	// 
        	this.buttonOK.DialogResult = System.Windows.Forms.DialogResult.OK;
        	this.buttonOK.Location = new System.Drawing.Point(204, 6);
        	this.buttonOK.Name = "buttonOK";
        	this.buttonOK.Size = new System.Drawing.Size(75, 23);
        	this.buttonOK.TabIndex = 0;
        	this.buttonOK.Text = "OK";
        	this.buttonOK.UseVisualStyleBackColor = true;
        	// 
        	// buttonCancel
        	// 
        	this.buttonCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
        	this.buttonCancel.Location = new System.Drawing.Point(285, 6);
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
        	// label2
        	// 
        	this.label2.Location = new System.Drawing.Point(13, 15);
        	this.label2.Name = "label2";
        	this.label2.Size = new System.Drawing.Size(77, 23);
        	this.label2.TabIndex = 0;
        	this.label2.Text = "Source mask:";
        	this.label2.TextAlign = System.Drawing.ContentAlignment.TopRight;
        	// 
        	// groupBox1
        	// 
        	this.groupBox1.Controls.Add(this.radioButtonRewriteAll);
        	this.groupBox1.Controls.Add(this.radioButtonRewriteIfSourceNewer);
        	this.groupBox1.Controls.Add(this.radioRewriteNo);
        	this.groupBox1.Location = new System.Drawing.Point(12, 88);
        	this.groupBox1.Name = "groupBox1";
        	this.groupBox1.Size = new System.Drawing.Size(348, 70);
        	this.groupBox1.TabIndex = 5;
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
        	// checkBoxSupressErrors
        	// 
        	this.checkBoxSupressErrors.AutoSize = true;
        	this.checkBoxSupressErrors.Location = new System.Drawing.Point(12, 41);
        	this.checkBoxSupressErrors.Name = "checkBoxSupressErrors";
        	this.checkBoxSupressErrors.Size = new System.Drawing.Size(91, 17);
        	this.checkBoxSupressErrors.TabIndex = 2;
        	this.checkBoxSupressErrors.Text = "supress errors";
        	this.checkBoxSupressErrors.UseVisualStyleBackColor = true;
        	// 
        	// checkBoxRecursive
        	// 
        	this.checkBoxRecursive.AutoSize = true;
        	this.checkBoxRecursive.Location = new System.Drawing.Point(12, 65);
        	this.checkBoxRecursive.Name = "checkBoxRecursive";
        	this.checkBoxRecursive.Size = new System.Drawing.Size(69, 17);
        	this.checkBoxRecursive.TabIndex = 3;
        	this.checkBoxRecursive.Text = "recursive";
        	this.checkBoxRecursive.UseVisualStyleBackColor = true;
        	// 
        	// checkBoxSaveAttributes
        	// 
        	this.checkBoxSaveAttributes.AutoSize = true;
        	this.checkBoxSaveAttributes.Enabled = false;
        	this.checkBoxSaveAttributes.Location = new System.Drawing.Point(199, 41);
        	this.checkBoxSaveAttributes.Name = "checkBoxSaveAttributes";
        	this.checkBoxSaveAttributes.Size = new System.Drawing.Size(97, 17);
        	this.checkBoxSaveAttributes.TabIndex = 4;
        	this.checkBoxSaveAttributes.Text = "Save attributes";
        	this.checkBoxSaveAttributes.UseVisualStyleBackColor = true;
        	// 
        	// ArchiveAddDialog
        	// 
        	this.AcceptButton = this.buttonOK;
        	this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
        	this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
        	this.CancelButton = this.buttonCancel;
        	this.ClientSize = new System.Drawing.Size(372, 194);
        	this.Controls.Add(this.checkBoxSaveAttributes);
        	this.Controls.Add(this.checkBoxRecursive);
        	this.Controls.Add(this.checkBoxSupressErrors);
        	this.Controls.Add(this.groupBox1);
        	this.Controls.Add(this.textBoxSourceMask);
        	this.Controls.Add(this.label2);
        	this.Controls.Add(this.panelButton);
        	this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
        	this.MaximizeBox = false;
        	this.Name = "ArchiveAddDialog";
        	this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
        	this.Text = "ArchiveAddDialog";
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
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.GroupBox groupBox1;
        public System.Windows.Forms.RadioButton radioButtonRewriteAll;
        public System.Windows.Forms.RadioButton radioButtonRewriteIfSourceNewer;
        public System.Windows.Forms.RadioButton radioRewriteNo;
        private System.Windows.Forms.CheckBox checkBoxSupressErrors;
        private System.Windows.Forms.CheckBox checkBoxRecursive;
        private System.Windows.Forms.CheckBox checkBoxSaveAttributes;
    }
}