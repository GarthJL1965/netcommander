namespace netCommander
{
    partial class FtpConnectionDialog
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
            this.label1 = new System.Windows.Forms.Label();
            this.textBoxServerName = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.numericUpDownPort = new System.Windows.Forms.NumericUpDown();
            this.numericUpDownTimeout = new System.Windows.Forms.NumericUpDown();
            this.label3 = new System.Windows.Forms.Label();
            this.checkBoxPassive = new System.Windows.Forms.CheckBox();
            this.checkBoxKeepAlive = new System.Windows.Forms.CheckBox();
            this.checkBoxEnableSsl = new System.Windows.Forms.CheckBox();
            this.checkBoxEnableProxy = new System.Windows.Forms.CheckBox();
            this.checkBoxAnonymous = new System.Windows.Forms.CheckBox();
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.panelButton.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownPort)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownTimeout)).BeginInit();
            this.SuspendLayout();
            // 
            // panelButton
            // 
            this.panelButton.Controls.Add(this.buttonOK);
            this.panelButton.Controls.Add(this.buttonCancel);
            this.panelButton.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panelButton.Location = new System.Drawing.Point(0, 176);
            this.panelButton.Name = "panelButton";
            this.panelButton.Size = new System.Drawing.Size(372, 35);
            this.panelButton.TabIndex = 11;
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
            // label1
            // 
            this.label1.Location = new System.Drawing.Point(13, 13);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(115, 23);
            this.label1.TabIndex = 0;
            this.label1.Text = "Server host name";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // textBoxServerName
            // 
            this.textBoxServerName.Location = new System.Drawing.Point(134, 15);
            this.textBoxServerName.Name = "textBoxServerName";
            this.textBoxServerName.Size = new System.Drawing.Size(226, 20);
            this.textBoxServerName.TabIndex = 1;
            // 
            // label2
            // 
            this.label2.Location = new System.Drawing.Point(12, 36);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(115, 23);
            this.label2.TabIndex = 2;
            this.label2.Text = "Port";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // numericUpDownPort
            // 
            this.numericUpDownPort.Location = new System.Drawing.Point(134, 39);
            this.numericUpDownPort.Maximum = new decimal(new int[] {
            65000,
            0,
            0,
            0});
            this.numericUpDownPort.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numericUpDownPort.Name = "numericUpDownPort";
            this.numericUpDownPort.Size = new System.Drawing.Size(83, 20);
            this.numericUpDownPort.TabIndex = 3;
            this.numericUpDownPort.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // numericUpDownTimeout
            // 
            this.numericUpDownTimeout.Location = new System.Drawing.Point(134, 65);
            this.numericUpDownTimeout.Maximum = new decimal(new int[] {
            30000000,
            0,
            0,
            0});
            this.numericUpDownTimeout.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numericUpDownTimeout.Name = "numericUpDownTimeout";
            this.numericUpDownTimeout.Size = new System.Drawing.Size(83, 20);
            this.numericUpDownTimeout.TabIndex = 5;
            this.numericUpDownTimeout.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // label3
            // 
            this.label3.Location = new System.Drawing.Point(12, 62);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(115, 23);
            this.label3.TabIndex = 4;
            this.label3.Text = "Timeout";
            this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // checkBoxPassive
            // 
            this.checkBoxPassive.Location = new System.Drawing.Point(12, 88);
            this.checkBoxPassive.Name = "checkBoxPassive";
            this.checkBoxPassive.Size = new System.Drawing.Size(161, 24);
            this.checkBoxPassive.TabIndex = 6;
            this.checkBoxPassive.Text = "Passive mode";
            this.checkBoxPassive.UseVisualStyleBackColor = true;
            // 
            // checkBoxKeepAlive
            // 
            this.checkBoxKeepAlive.Location = new System.Drawing.Point(12, 148);
            this.checkBoxKeepAlive.Name = "checkBoxKeepAlive";
            this.checkBoxKeepAlive.Size = new System.Drawing.Size(161, 24);
            this.checkBoxKeepAlive.TabIndex = 7;
            this.checkBoxKeepAlive.Text = "Keep alive";
            this.checkBoxKeepAlive.UseVisualStyleBackColor = true;
            // 
            // checkBoxEnableSsl
            // 
            this.checkBoxEnableSsl.Location = new System.Drawing.Point(179, 88);
            this.checkBoxEnableSsl.Name = "checkBoxEnableSsl";
            this.checkBoxEnableSsl.Size = new System.Drawing.Size(181, 24);
            this.checkBoxEnableSsl.TabIndex = 8;
            this.checkBoxEnableSsl.Text = "Enable SSL";
            this.checkBoxEnableSsl.UseVisualStyleBackColor = true;
            // 
            // checkBoxEnableProxy
            // 
            this.checkBoxEnableProxy.Location = new System.Drawing.Point(12, 118);
            this.checkBoxEnableProxy.Name = "checkBoxEnableProxy";
            this.checkBoxEnableProxy.Size = new System.Drawing.Size(161, 24);
            this.checkBoxEnableProxy.TabIndex = 9;
            this.checkBoxEnableProxy.Text = "Enable proxy";
            this.checkBoxEnableProxy.UseVisualStyleBackColor = true;
            // 
            // checkBoxAnonymous
            // 
            this.checkBoxAnonymous.Location = new System.Drawing.Point(179, 118);
            this.checkBoxAnonymous.Name = "checkBoxAnonymous";
            this.checkBoxAnonymous.Size = new System.Drawing.Size(181, 24);
            this.checkBoxAnonymous.TabIndex = 10;
            this.checkBoxAnonymous.Text = "Anonymous logon";
            this.checkBoxAnonymous.UseVisualStyleBackColor = true;
            // 
            // FtpConnectionDialog
            // 
            this.AcceptButton = this.buttonOK;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.buttonCancel;
            this.ClientSize = new System.Drawing.Size(372, 211);
            this.Controls.Add(this.checkBoxAnonymous);
            this.Controls.Add(this.checkBoxEnableProxy);
            this.Controls.Add(this.checkBoxEnableSsl);
            this.Controls.Add(this.checkBoxKeepAlive);
            this.Controls.Add(this.checkBoxPassive);
            this.Controls.Add(this.numericUpDownTimeout);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.numericUpDownPort);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.textBoxServerName);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.panelButton);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.Name = "FtpConnectionDialog";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "FtpConnectionDialog";
            this.panelButton.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownPort)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownTimeout)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Panel panelButton;
        public System.Windows.Forms.Button buttonOK;
        private System.Windows.Forms.Button buttonCancel;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox textBoxServerName;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.NumericUpDown numericUpDownPort;
        private System.Windows.Forms.NumericUpDown numericUpDownTimeout;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.CheckBox checkBoxPassive;
        private System.Windows.Forms.CheckBox checkBoxKeepAlive;
        private System.Windows.Forms.CheckBox checkBoxEnableSsl;
        private System.Windows.Forms.CheckBox checkBoxEnableProxy;
        private System.Windows.Forms.CheckBox checkBoxAnonymous;
        private System.Windows.Forms.ToolTip toolTip1;
    }
}