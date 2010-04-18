namespace netCommander
{
    partial class DeleteFileDialog
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
            this.checkBoxForceReadonly = new System.Windows.Forms.CheckBox();
            this.textBoxMask = new System.Windows.Forms.TextBox();
            this.labelMask = new System.Windows.Forms.Label();
            this.checkBoxRemoveEmptyDirs = new System.Windows.Forms.CheckBox();
            this.checkBoxRecursive = new System.Windows.Forms.CheckBox();
            this.panelButton.SuspendLayout();
            this.SuspendLayout();
            // 
            // panelButton
            // 
            this.panelButton.Controls.Add(this.buttonOK);
            this.panelButton.Controls.Add(this.buttonCancel);
            this.panelButton.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panelButton.Location = new System.Drawing.Point(0, 106);
            this.panelButton.Name = "panelButton";
            this.panelButton.Size = new System.Drawing.Size(372, 35);
            this.panelButton.TabIndex = 5;
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
            // checkBoxForceReadonly
            // 
            this.checkBoxForceReadonly.AutoSize = true;
            this.checkBoxForceReadonly.Location = new System.Drawing.Point(12, 87);
            this.checkBoxForceReadonly.Name = "checkBoxForceReadonly";
            this.checkBoxForceReadonly.Size = new System.Drawing.Size(239, 17);
            this.checkBoxForceReadonly.TabIndex = 4;
            this.checkBoxForceReadonly.Text = "Delete files or directory with readonly attribute";
            this.checkBoxForceReadonly.UseVisualStyleBackColor = true;
            // 
            // textBoxMask
            // 
            this.textBoxMask.Location = new System.Drawing.Point(100, 12);
            this.textBoxMask.Name = "textBoxMask";
            this.textBoxMask.Size = new System.Drawing.Size(264, 20);
            this.textBoxMask.TabIndex = 1;
            // 
            // labelMask
            // 
            this.labelMask.Location = new System.Drawing.Point(12, 15);
            this.labelMask.Name = "labelMask";
            this.labelMask.Size = new System.Drawing.Size(82, 23);
            this.labelMask.TabIndex = 0;
            this.labelMask.Text = "Mask:";
            this.labelMask.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // checkBoxRemoveEmptyDirs
            // 
            this.checkBoxRemoveEmptyDirs.AutoSize = true;
            this.checkBoxRemoveEmptyDirs.Location = new System.Drawing.Point(12, 41);
            this.checkBoxRemoveEmptyDirs.Name = "checkBoxRemoveEmptyDirs";
            this.checkBoxRemoveEmptyDirs.Size = new System.Drawing.Size(148, 17);
            this.checkBoxRemoveEmptyDirs.TabIndex = 2;
            this.checkBoxRemoveEmptyDirs.Text = "Remove empty directories";
            this.checkBoxRemoveEmptyDirs.UseVisualStyleBackColor = true;
            // 
            // checkBoxRecursive
            // 
            this.checkBoxRecursive.AutoSize = true;
            this.checkBoxRecursive.Location = new System.Drawing.Point(12, 64);
            this.checkBoxRecursive.Name = "checkBoxRecursive";
            this.checkBoxRecursive.Size = new System.Drawing.Size(131, 17);
            this.checkBoxRecursive.TabIndex = 3;
            this.checkBoxRecursive.Text = "Delete files recursively";
            this.checkBoxRecursive.UseVisualStyleBackColor = true;
            // 
            // DeleteFileDialog
            // 
            this.AcceptButton = this.buttonOK;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.buttonCancel;
            this.ClientSize = new System.Drawing.Size(372, 141);
            this.Controls.Add(this.checkBoxRecursive);
            this.Controls.Add(this.checkBoxRemoveEmptyDirs);
            this.Controls.Add(this.textBoxMask);
            this.Controls.Add(this.labelMask);
            this.Controls.Add(this.checkBoxForceReadonly);
            this.Controls.Add(this.panelButton);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.Name = "DeleteFileDialog";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "DeleteFileDialog";
            this.panelButton.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Panel panelButton;
        public System.Windows.Forms.Button buttonOK;
        private System.Windows.Forms.Button buttonCancel;
        public System.Windows.Forms.TextBox textBoxMask;
        public System.Windows.Forms.Label labelMask;
        public System.Windows.Forms.CheckBox checkBoxForceReadonly;
        public System.Windows.Forms.CheckBox checkBoxRemoveEmptyDirs;
        public System.Windows.Forms.CheckBox checkBoxRecursive;
    }
}