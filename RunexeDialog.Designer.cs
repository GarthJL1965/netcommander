namespace netCommander
{
    partial class RunexeDialog
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
            this.textBoxFilename = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.textBoxWorkingDirectory = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.textBoxArguments = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.comboBoxVerb = new System.Windows.Forms.ComboBox();
            this.label4 = new System.Windows.Forms.Label();
            this.checkBoxShellexecute = new System.Windows.Forms.CheckBox();
            this.checkBoxRunInConsole = new System.Windows.Forms.CheckBox();
            this.checkBoxRunas = new System.Windows.Forms.CheckBox();
            this.checkBoxLoadProfile = new System.Windows.Forms.CheckBox();
            this.panelButton.SuspendLayout();
            this.SuspendLayout();
            // 
            // panelButton
            // 
            this.panelButton.Controls.Add(this.buttonOK);
            this.panelButton.Controls.Add(this.buttonCancel);
            this.panelButton.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panelButton.Location = new System.Drawing.Point(0, 165);
            this.panelButton.Name = "panelButton";
            this.panelButton.Size = new System.Drawing.Size(372, 35);
            this.panelButton.TabIndex = 12;
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
            // textBoxFilename
            // 
            this.textBoxFilename.Location = new System.Drawing.Point(130, 12);
            this.textBoxFilename.Name = "textBoxFilename";
            this.textBoxFilename.ReadOnly = true;
            this.textBoxFilename.Size = new System.Drawing.Size(230, 20);
            this.textBoxFilename.TabIndex = 1;
            // 
            // label2
            // 
            this.label2.Location = new System.Drawing.Point(13, 15);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(111, 23);
            this.label2.TabIndex = 0;
            this.label2.Text = "File to run:";
            this.label2.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // textBoxWorkingDirectory
            // 
            this.textBoxWorkingDirectory.Location = new System.Drawing.Point(130, 38);
            this.textBoxWorkingDirectory.Name = "textBoxWorkingDirectory";
            this.textBoxWorkingDirectory.Size = new System.Drawing.Size(230, 20);
            this.textBoxWorkingDirectory.TabIndex = 3;
            // 
            // label1
            // 
            this.label1.Location = new System.Drawing.Point(13, 41);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(111, 23);
            this.label1.TabIndex = 2;
            this.label1.Text = "Working directory:";
            this.label1.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // textBoxArguments
            // 
            this.textBoxArguments.Location = new System.Drawing.Point(130, 64);
            this.textBoxArguments.Name = "textBoxArguments";
            this.textBoxArguments.Size = new System.Drawing.Size(230, 20);
            this.textBoxArguments.TabIndex = 5;
            // 
            // label3
            // 
            this.label3.Location = new System.Drawing.Point(13, 67);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(111, 23);
            this.label3.TabIndex = 4;
            this.label3.Text = "Arguments:";
            this.label3.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // comboBoxVerb
            // 
            this.comboBoxVerb.FormattingEnabled = true;
            this.comboBoxVerb.Location = new System.Drawing.Point(130, 91);
            this.comboBoxVerb.Name = "comboBoxVerb";
            this.comboBoxVerb.Size = new System.Drawing.Size(230, 21);
            this.comboBoxVerb.TabIndex = 7;
            // 
            // label4
            // 
            this.label4.Location = new System.Drawing.Point(13, 94);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(111, 23);
            this.label4.TabIndex = 6;
            this.label4.Text = "Verb";
            this.label4.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // checkBoxShellexecute
            // 
            this.checkBoxShellexecute.AutoSize = true;
            this.checkBoxShellexecute.Location = new System.Drawing.Point(16, 121);
            this.checkBoxShellexecute.Name = "checkBoxShellexecute";
            this.checkBoxShellexecute.Size = new System.Drawing.Size(110, 17);
            this.checkBoxShellexecute.TabIndex = 8;
            this.checkBoxShellexecute.Text = "Use shell execute";
            this.checkBoxShellexecute.UseVisualStyleBackColor = true;
            // 
            // checkBoxRunInConsole
            // 
            this.checkBoxRunInConsole.AutoSize = true;
            this.checkBoxRunInConsole.Location = new System.Drawing.Point(185, 121);
            this.checkBoxRunInConsole.Name = "checkBoxRunInConsole";
            this.checkBoxRunInConsole.Size = new System.Drawing.Size(159, 17);
            this.checkBoxRunInConsole.TabIndex = 9;
            this.checkBoxRunInConsole.Text = "Run in new console window";
            this.checkBoxRunInConsole.UseVisualStyleBackColor = true;
            // 
            // checkBoxRunas
            // 
            this.checkBoxRunas.AutoSize = true;
            this.checkBoxRunas.Location = new System.Drawing.Point(16, 145);
            this.checkBoxRunas.Name = "checkBoxRunas";
            this.checkBoxRunas.Size = new System.Drawing.Size(122, 17);
            this.checkBoxRunas.TabIndex = 10;
            this.checkBoxRunas.Text = "Run as another user";
            this.checkBoxRunas.UseVisualStyleBackColor = true;
            // 
            // checkBoxLoadProfile
            // 
            this.checkBoxLoadProfile.AutoSize = true;
            this.checkBoxLoadProfile.Location = new System.Drawing.Point(185, 145);
            this.checkBoxLoadProfile.Name = "checkBoxLoadProfile";
            this.checkBoxLoadProfile.Size = new System.Drawing.Size(98, 17);
            this.checkBoxLoadProfile.TabIndex = 11;
            this.checkBoxLoadProfile.Text = "Load usr profile";
            this.checkBoxLoadProfile.UseVisualStyleBackColor = true;
            // 
            // RunexeDialog
            // 
            this.AcceptButton = this.buttonOK;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.buttonCancel;
            this.ClientSize = new System.Drawing.Size(372, 200);
            this.Controls.Add(this.checkBoxLoadProfile);
            this.Controls.Add(this.checkBoxRunas);
            this.Controls.Add(this.checkBoxRunInConsole);
            this.Controls.Add(this.checkBoxShellexecute);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.comboBoxVerb);
            this.Controls.Add(this.textBoxArguments);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.textBoxWorkingDirectory);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.textBoxFilename);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.panelButton);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Name = "RunexeDialog";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "RunexeDialog";
            this.panelButton.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Panel panelButton;
        public System.Windows.Forms.Button buttonOK;
        private System.Windows.Forms.Button buttonCancel;
        public System.Windows.Forms.TextBox textBoxFilename;
        private System.Windows.Forms.Label label2;
        public System.Windows.Forms.TextBox textBoxWorkingDirectory;
        private System.Windows.Forms.Label label1;
        public System.Windows.Forms.TextBox textBoxArguments;
        private System.Windows.Forms.Label label3;
        public System.Windows.Forms.ComboBox comboBoxVerb;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.CheckBox checkBoxShellexecute;
        private System.Windows.Forms.CheckBox checkBoxRunInConsole;
        private System.Windows.Forms.CheckBox checkBoxRunas;
        private System.Windows.Forms.CheckBox checkBoxLoadProfile;
    }
}