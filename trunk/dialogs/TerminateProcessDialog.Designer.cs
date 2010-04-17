namespace netCommander
{
    partial class TerminateProcessDialog
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
            this.labelTerminateProcessMessage = new System.Windows.Forms.Label();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.radioButtonSendCloseMainWindow = new System.Windows.Forms.RadioButton();
            this.radioButtonKill = new System.Windows.Forms.RadioButton();
            this.checkBoxUseDebugMode = new System.Windows.Forms.CheckBox();
            this.panelButton.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // panelButton
            // 
            this.panelButton.Controls.Add(this.buttonOK);
            this.panelButton.Controls.Add(this.buttonCancel);
            this.panelButton.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panelButton.Location = new System.Drawing.Point(0, 149);
            this.panelButton.Name = "panelButton";
            this.panelButton.Size = new System.Drawing.Size(372, 35);
            this.panelButton.TabIndex = 14;
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
            // labelTerminateProcessMessage
            // 
            this.labelTerminateProcessMessage.Location = new System.Drawing.Point(12, 9);
            this.labelTerminateProcessMessage.Name = "labelTerminateProcessMessage";
            this.labelTerminateProcessMessage.Size = new System.Drawing.Size(348, 35);
            this.labelTerminateProcessMessage.TabIndex = 15;
            this.labelTerminateProcessMessage.Text = "label1";
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.radioButtonKill);
            this.groupBox1.Controls.Add(this.radioButtonSendCloseMainWindow);
            this.groupBox1.Location = new System.Drawing.Point(12, 47);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(348, 69);
            this.groupBox1.TabIndex = 16;
            this.groupBox1.TabStop = false;
            // 
            // radioButtonSendCloseMainWindow
            // 
            this.radioButtonSendCloseMainWindow.AutoSize = true;
            this.radioButtonSendCloseMainWindow.Location = new System.Drawing.Point(7, 20);
            this.radioButtonSendCloseMainWindow.Name = "radioButtonSendCloseMainWindow";
            this.radioButtonSendCloseMainWindow.Size = new System.Drawing.Size(132, 17);
            this.radioButtonSendCloseMainWindow.TabIndex = 0;
            this.radioButtonSendCloseMainWindow.TabStop = true;
            this.radioButtonSendCloseMainWindow.Text = "send window message";
            this.radioButtonSendCloseMainWindow.UseVisualStyleBackColor = true;
            // 
            // radioButtonKill
            // 
            this.radioButtonKill.AutoSize = true;
            this.radioButtonKill.Location = new System.Drawing.Point(7, 44);
            this.radioButtonKill.Name = "radioButtonKill";
            this.radioButtonKill.Size = new System.Drawing.Size(37, 17);
            this.radioButtonKill.TabIndex = 1;
            this.radioButtonKill.TabStop = true;
            this.radioButtonKill.Text = "kill";
            this.radioButtonKill.UseVisualStyleBackColor = true;
            // 
            // checkBoxUseDebugMode
            // 
            this.checkBoxUseDebugMode.AutoSize = true;
            this.checkBoxUseDebugMode.Location = new System.Drawing.Point(19, 123);
            this.checkBoxUseDebugMode.Name = "checkBoxUseDebugMode";
            this.checkBoxUseDebugMode.Size = new System.Drawing.Size(85, 17);
            this.checkBoxUseDebugMode.TabIndex = 17;
            this.checkBoxUseDebugMode.Text = "debug mode";
            this.checkBoxUseDebugMode.UseVisualStyleBackColor = true;
            // 
            // TerminateProcessDialog
            // 
            this.AcceptButton = this.buttonOK;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.buttonCancel;
            this.ClientSize = new System.Drawing.Size(372, 184);
            this.Controls.Add(this.checkBoxUseDebugMode);
            this.Controls.Add(this.panelButton);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.labelTerminateProcessMessage);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.Name = "TerminateProcessDialog";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "TerminateProcessDialog";
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
        internal System.Windows.Forms.Label labelTerminateProcessMessage;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.RadioButton radioButtonKill;
        private System.Windows.Forms.RadioButton radioButtonSendCloseMainWindow;
        private System.Windows.Forms.CheckBox checkBoxUseDebugMode;
    }
}