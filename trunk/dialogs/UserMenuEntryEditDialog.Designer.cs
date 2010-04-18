namespace netCommander
{
    partial class UserMenuEntryEditDialog
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
            this.textBoxMenuText = new System.Windows.Forms.TextBox();
            this.labelMenuText = new System.Windows.Forms.Label();
            this.textBoxCommandText = new System.Windows.Forms.TextBox();
            this.labelCommandText = new System.Windows.Forms.Label();
            this.checkBoxNoWindow = new System.Windows.Forms.CheckBox();
            this.checkBoxUseShellexecute = new System.Windows.Forms.CheckBox();
            this.textBoxHelp = new System.Windows.Forms.TextBox();
            this.panelButton.SuspendLayout();
            this.SuspendLayout();
            // 
            // panelButton
            // 
            this.panelButton.Controls.Add(this.buttonOK);
            this.panelButton.Controls.Add(this.buttonCancel);
            this.panelButton.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panelButton.Location = new System.Drawing.Point(0, 251);
            this.panelButton.Name = "panelButton";
            this.panelButton.Size = new System.Drawing.Size(372, 35);
            this.panelButton.TabIndex = 7;
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
            // textBoxMenuText
            // 
            this.textBoxMenuText.Location = new System.Drawing.Point(146, 12);
            this.textBoxMenuText.Name = "textBoxMenuText";
            this.textBoxMenuText.Size = new System.Drawing.Size(214, 20);
            this.textBoxMenuText.TabIndex = 1;
            // 
            // labelMenuText
            // 
            this.labelMenuText.Location = new System.Drawing.Point(13, 15);
            this.labelMenuText.Name = "labelMenuText";
            this.labelMenuText.Size = new System.Drawing.Size(127, 23);
            this.labelMenuText.TabIndex = 0;
            this.labelMenuText.Text = "Source mask:";
            this.labelMenuText.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // textBoxCommandText
            // 
            this.textBoxCommandText.Location = new System.Drawing.Point(16, 58);
            this.textBoxCommandText.Multiline = true;
            this.textBoxCommandText.Name = "textBoxCommandText";
            this.textBoxCommandText.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.textBoxCommandText.Size = new System.Drawing.Size(344, 64);
            this.textBoxCommandText.TabIndex = 3;
            // 
            // labelCommandText
            // 
            this.labelCommandText.Location = new System.Drawing.Point(13, 41);
            this.labelCommandText.Name = "labelCommandText";
            this.labelCommandText.Size = new System.Drawing.Size(127, 23);
            this.labelCommandText.TabIndex = 2;
            this.labelCommandText.Text = "Source mask:";
            // 
            // checkBoxNoWindow
            // 
            this.checkBoxNoWindow.AutoSize = true;
            this.checkBoxNoWindow.Location = new System.Drawing.Point(16, 129);
            this.checkBoxNoWindow.Name = "checkBoxNoWindow";
            this.checkBoxNoWindow.Size = new System.Drawing.Size(80, 17);
            this.checkBoxNoWindow.TabIndex = 4;
            this.checkBoxNoWindow.Text = "checkBox1";
            this.checkBoxNoWindow.UseVisualStyleBackColor = true;
            // 
            // checkBoxUseShellexecute
            // 
            this.checkBoxUseShellexecute.AutoSize = true;
            this.checkBoxUseShellexecute.Location = new System.Drawing.Point(16, 153);
            this.checkBoxUseShellexecute.Name = "checkBoxUseShellexecute";
            this.checkBoxUseShellexecute.Size = new System.Drawing.Size(80, 17);
            this.checkBoxUseShellexecute.TabIndex = 5;
            this.checkBoxUseShellexecute.Text = "checkBox1";
            this.checkBoxUseShellexecute.UseVisualStyleBackColor = true;
            // 
            // textBoxHelp
            // 
            this.textBoxHelp.Location = new System.Drawing.Point(12, 176);
            this.textBoxHelp.Multiline = true;
            this.textBoxHelp.Name = "textBoxHelp";
            this.textBoxHelp.ReadOnly = true;
            this.textBoxHelp.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.textBoxHelp.Size = new System.Drawing.Size(344, 64);
            this.textBoxHelp.TabIndex = 6;
            // 
            // UserMenuEntryEditDialog
            // 
            this.AcceptButton = this.buttonOK;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.buttonCancel;
            this.ClientSize = new System.Drawing.Size(372, 286);
            this.Controls.Add(this.textBoxHelp);
            this.Controls.Add(this.checkBoxUseShellexecute);
            this.Controls.Add(this.checkBoxNoWindow);
            this.Controls.Add(this.textBoxCommandText);
            this.Controls.Add(this.labelCommandText);
            this.Controls.Add(this.textBoxMenuText);
            this.Controls.Add(this.labelMenuText);
            this.Controls.Add(this.panelButton);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.Name = "UserMenuEntryEditDialog";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "UserMenuEntryEditDialog";
            this.panelButton.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Panel panelButton;
        public System.Windows.Forms.Button buttonOK;
        private System.Windows.Forms.Button buttonCancel;
        public System.Windows.Forms.TextBox textBoxMenuText;
        private System.Windows.Forms.Label labelMenuText;
        public System.Windows.Forms.TextBox textBoxCommandText;
        private System.Windows.Forms.Label labelCommandText;
        private System.Windows.Forms.CheckBox checkBoxNoWindow;
        private System.Windows.Forms.CheckBox checkBoxUseShellexecute;
        public System.Windows.Forms.TextBox textBoxHelp;
    }
}