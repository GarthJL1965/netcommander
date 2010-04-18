namespace netCommander
{
    partial class CreateLinkDialog
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
            this.textBoxLinkname = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.textBoxLinktarget = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.radioButtonSymlink = new System.Windows.Forms.RadioButton();
            this.radioButtonMountpoint = new System.Windows.Forms.RadioButton();
            this.radioButtonHardlink = new System.Windows.Forms.RadioButton();
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.panelButton.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // panelButton
            // 
            this.panelButton.Controls.Add(this.buttonOK);
            this.panelButton.Controls.Add(this.buttonCancel);
            this.panelButton.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panelButton.Location = new System.Drawing.Point(0, 166);
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
            // textBoxLinkname
            // 
            this.textBoxLinkname.Location = new System.Drawing.Point(96, 12);
            this.textBoxLinkname.Name = "textBoxLinkname";
            this.textBoxLinkname.Size = new System.Drawing.Size(268, 20);
            this.textBoxLinkname.TabIndex = 1;
            // 
            // label1
            // 
            this.label1.Location = new System.Drawing.Point(15, 15);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(75, 23);
            this.label1.TabIndex = 0;
            this.label1.Text = "Link name:";
            this.label1.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // textBoxLinktarget
            // 
            this.textBoxLinktarget.Location = new System.Drawing.Point(96, 38);
            this.textBoxLinktarget.Name = "textBoxLinktarget";
            this.textBoxLinktarget.ReadOnly = true;
            this.textBoxLinktarget.Size = new System.Drawing.Size(268, 20);
            this.textBoxLinktarget.TabIndex = 3;
            // 
            // label2
            // 
            this.label2.Location = new System.Drawing.Point(15, 41);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(75, 23);
            this.label2.TabIndex = 2;
            this.label2.Text = "Link target:";
            this.label2.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.radioButtonSymlink);
            this.groupBox1.Controls.Add(this.radioButtonMountpoint);
            this.groupBox1.Controls.Add(this.radioButtonHardlink);
            this.groupBox1.Location = new System.Drawing.Point(8, 68);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(356, 98);
            this.groupBox1.TabIndex = 4;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Link type";
            // 
            // radioButtonSymlink
            // 
            this.radioButtonSymlink.AutoSize = true;
            this.radioButtonSymlink.Location = new System.Drawing.Point(10, 68);
            this.radioButtonSymlink.Name = "radioButtonSymlink";
            this.radioButtonSymlink.Size = new System.Drawing.Size(86, 17);
            this.radioButtonSymlink.TabIndex = 2;
            this.radioButtonSymlink.TabStop = true;
            this.radioButtonSymlink.Text = "Symbolic link";
            this.radioButtonSymlink.UseVisualStyleBackColor = true;
            // 
            // radioButtonMountpoint
            // 
            this.radioButtonMountpoint.AutoSize = true;
            this.radioButtonMountpoint.Location = new System.Drawing.Point(10, 44);
            this.radioButtonMountpoint.Name = "radioButtonMountpoint";
            this.radioButtonMountpoint.Size = new System.Drawing.Size(129, 17);
            this.radioButtonMountpoint.TabIndex = 1;
            this.radioButtonMountpoint.TabStop = true;
            this.radioButtonMountpoint.Text = "Junction (mount point)";
            this.radioButtonMountpoint.UseVisualStyleBackColor = true;
            // 
            // radioButtonHardlink
            // 
            this.radioButtonHardlink.AutoSize = true;
            this.radioButtonHardlink.Location = new System.Drawing.Point(10, 20);
            this.radioButtonHardlink.Name = "radioButtonHardlink";
            this.radioButtonHardlink.Size = new System.Drawing.Size(67, 17);
            this.radioButtonHardlink.TabIndex = 0;
            this.radioButtonHardlink.TabStop = true;
            this.radioButtonHardlink.Text = "Hard link";
            this.radioButtonHardlink.UseVisualStyleBackColor = true;
            // 
            // CreateLinkDialog
            // 
            this.AcceptButton = this.buttonOK;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.buttonCancel;
            this.ClientSize = new System.Drawing.Size(372, 201);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.textBoxLinktarget);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.textBoxLinkname);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.panelButton);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.Name = "CreateLinkDialog";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "CreateLinkDialog";
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
        public System.Windows.Forms.TextBox textBoxLinkname;
        private System.Windows.Forms.Label label1;
        public System.Windows.Forms.TextBox textBoxLinktarget;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.RadioButton radioButtonMountpoint;
        private System.Windows.Forms.RadioButton radioButtonHardlink;
        private System.Windows.Forms.RadioButton radioButtonSymlink;
        private System.Windows.Forms.ToolTip toolTip1;
    }
}