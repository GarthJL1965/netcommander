namespace netCommander
{
    partial class CreateDirectoryDialog
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
            this.textBoxDirectoryName = new System.Windows.Forms.TextBox();
            this.textBoxTemplateDirectory = new System.Windows.Forms.TextBox();
            this.checkBoxUseTemplate = new System.Windows.Forms.CheckBox();
            this.labelParentDir = new System.Windows.Forms.Label();
            this.panelButton.SuspendLayout();
            this.SuspendLayout();
            // 
            // panelButton
            // 
            this.panelButton.Controls.Add(this.buttonOK);
            this.panelButton.Controls.Add(this.buttonCancel);
            this.panelButton.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panelButton.Location = new System.Drawing.Point(0, 69);
            this.panelButton.Name = "panelButton";
            this.panelButton.Size = new System.Drawing.Size(372, 35);
            this.panelButton.TabIndex = 4;
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
            // textBoxDirectoryName
            // 
            this.textBoxDirectoryName.Location = new System.Drawing.Point(166, 9);
            this.textBoxDirectoryName.Name = "textBoxDirectoryName";
            this.textBoxDirectoryName.Size = new System.Drawing.Size(194, 20);
            this.textBoxDirectoryName.TabIndex = 1;
            // 
            // textBoxTemplateDirectory
            // 
            this.textBoxTemplateDirectory.Location = new System.Drawing.Point(166, 40);
            this.textBoxTemplateDirectory.Name = "textBoxTemplateDirectory";
            this.textBoxTemplateDirectory.Size = new System.Drawing.Size(194, 20);
            this.textBoxTemplateDirectory.TabIndex = 3;
            // 
            // checkBoxUseTemplate
            // 
            this.checkBoxUseTemplate.AutoEllipsis = true;
            this.checkBoxUseTemplate.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.checkBoxUseTemplate.Location = new System.Drawing.Point(13, 42);
            this.checkBoxUseTemplate.Name = "checkBoxUseTemplate";
            this.checkBoxUseTemplate.Size = new System.Drawing.Size(147, 17);
            this.checkBoxUseTemplate.TabIndex = 2;
            this.checkBoxUseTemplate.Text = "Use as template";
            this.checkBoxUseTemplate.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.checkBoxUseTemplate.UseVisualStyleBackColor = true;
            this.checkBoxUseTemplate.CheckedChanged += new System.EventHandler(this.checkBoxUseTemplate_CheckedChanged);
            // 
            // labelParentDir
            // 
            this.labelParentDir.AutoEllipsis = true;
            this.labelParentDir.Location = new System.Drawing.Point(10, 12);
            this.labelParentDir.Name = "labelParentDir";
            this.labelParentDir.Size = new System.Drawing.Size(150, 23);
            this.labelParentDir.TabIndex = 0;
            this.labelParentDir.Text = "one";
            this.labelParentDir.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // CreateDirectoryDialog
            // 
            this.AcceptButton = this.buttonOK;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.buttonCancel;
            this.ClientSize = new System.Drawing.Size(372, 104);
            this.Controls.Add(this.labelParentDir);
            this.Controls.Add(this.checkBoxUseTemplate);
            this.Controls.Add(this.textBoxTemplateDirectory);
            this.Controls.Add(this.textBoxDirectoryName);
            this.Controls.Add(this.panelButton);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.Name = "CreateDirectoryDialog";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "CreateDirectoryDialog";
            this.panelButton.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Panel panelButton;
        public System.Windows.Forms.Button buttonOK;
        private System.Windows.Forms.Button buttonCancel;
        public System.Windows.Forms.TextBox textBoxDirectoryName;
        public System.Windows.Forms.CheckBox checkBoxUseTemplate;
        public System.Windows.Forms.TextBox textBoxTemplateDirectory;
        public System.Windows.Forms.Label labelParentDir;
    }
}