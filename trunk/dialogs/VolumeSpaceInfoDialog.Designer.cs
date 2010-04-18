namespace netCommander
{
    partial class VolumeSpaceInfoDialog
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
            this.textBoxTotalSize = new System.Windows.Forms.TextBox();
            this.labelTotalSize = new System.Windows.Forms.Label();
            this.textBoxTotalAvailable = new System.Windows.Forms.TextBox();
            this.labelTotalAvailable = new System.Windows.Forms.Label();
            this.panelButton.SuspendLayout();
            this.SuspendLayout();
            // 
            // panelButton
            // 
            this.panelButton.Controls.Add(this.buttonOK);
            this.panelButton.Controls.Add(this.buttonCancel);
            this.panelButton.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panelButton.Location = new System.Drawing.Point(0, 67);
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
            // textBoxTotalSize
            // 
            this.textBoxTotalSize.Location = new System.Drawing.Point(187, 12);
            this.textBoxTotalSize.Name = "textBoxTotalSize";
            this.textBoxTotalSize.ReadOnly = true;
            this.textBoxTotalSize.Size = new System.Drawing.Size(173, 20);
            this.textBoxTotalSize.TabIndex = 14;
            // 
            // labelTotalSize
            // 
            this.labelTotalSize.Location = new System.Drawing.Point(13, 15);
            this.labelTotalSize.Name = "labelTotalSize";
            this.labelTotalSize.Size = new System.Drawing.Size(168, 23);
            this.labelTotalSize.TabIndex = 13;
            this.labelTotalSize.Text = "Source mask:";
            this.labelTotalSize.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // textBoxTotalAvailable
            // 
            this.textBoxTotalAvailable.Location = new System.Drawing.Point(187, 38);
            this.textBoxTotalAvailable.Name = "textBoxTotalAvailable";
            this.textBoxTotalAvailable.ReadOnly = true;
            this.textBoxTotalAvailable.Size = new System.Drawing.Size(173, 20);
            this.textBoxTotalAvailable.TabIndex = 16;
            // 
            // labelTotalAvailable
            // 
            this.labelTotalAvailable.Location = new System.Drawing.Point(13, 41);
            this.labelTotalAvailable.Name = "labelTotalAvailable";
            this.labelTotalAvailable.Size = new System.Drawing.Size(168, 23);
            this.labelTotalAvailable.TabIndex = 15;
            this.labelTotalAvailable.Text = "Source mask:";
            this.labelTotalAvailable.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // VolumeSpaceInfoDialog
            // 
            this.AcceptButton = this.buttonOK;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.buttonCancel;
            this.ClientSize = new System.Drawing.Size(372, 102);
            this.Controls.Add(this.textBoxTotalAvailable);
            this.Controls.Add(this.labelTotalAvailable);
            this.Controls.Add(this.textBoxTotalSize);
            this.Controls.Add(this.labelTotalSize);
            this.Controls.Add(this.panelButton);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.Name = "VolumeSpaceInfoDialog";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "VolumeSpaceInfoDialog";
            this.panelButton.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Panel panelButton;
        public System.Windows.Forms.Button buttonOK;
        private System.Windows.Forms.Button buttonCancel;
        public System.Windows.Forms.TextBox textBoxTotalSize;
        private System.Windows.Forms.Label labelTotalSize;
        public System.Windows.Forms.TextBox textBoxTotalAvailable;
        private System.Windows.Forms.Label labelTotalAvailable;
    }
}