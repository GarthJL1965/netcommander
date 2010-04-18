namespace netCommander
{
    partial class TouchFileDialog
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
            this.textBoxFileName = new System.Windows.Forms.TextBox();
            this.dateTimePickerCreation = new System.Windows.Forms.DateTimePicker();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.dateTimePickerModification = new System.Windows.Forms.DateTimePicker();
            this.label3 = new System.Windows.Forms.Label();
            this.dateTimePickerAccess = new System.Windows.Forms.DateTimePicker();
            this.panelButton.SuspendLayout();
            this.SuspendLayout();
            // 
            // panelButton
            // 
            this.panelButton.Controls.Add(this.buttonOK);
            this.panelButton.Controls.Add(this.buttonCancel);
            this.panelButton.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panelButton.Location = new System.Drawing.Point(0, 120);
            this.panelButton.Name = "panelButton";
            this.panelButton.Size = new System.Drawing.Size(376, 35);
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
            // textBoxFileName
            // 
            this.textBoxFileName.Location = new System.Drawing.Point(13, 13);
            this.textBoxFileName.Name = "textBoxFileName";
            this.textBoxFileName.Size = new System.Drawing.Size(351, 20);
            this.textBoxFileName.TabIndex = 0;
            // 
            // dateTimePickerCreation
            // 
            this.dateTimePickerCreation.CustomFormat = "dd.MM.yyyy HH:mm:ss";
            this.dateTimePickerCreation.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dateTimePickerCreation.Location = new System.Drawing.Point(217, 39);
            this.dateTimePickerCreation.Name = "dateTimePickerCreation";
            this.dateTimePickerCreation.ShowCheckBox = true;
            this.dateTimePickerCreation.Size = new System.Drawing.Size(147, 20);
            this.dateTimePickerCreation.TabIndex = 2;
            // 
            // label1
            // 
            this.label1.Location = new System.Drawing.Point(13, 38);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(198, 23);
            this.label1.TabIndex = 1;
            this.label1.Text = "Creation time";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label2
            // 
            this.label2.Location = new System.Drawing.Point(13, 64);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(198, 23);
            this.label2.TabIndex = 3;
            this.label2.Text = "Last modification time";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // dateTimePickerModification
            // 
            this.dateTimePickerModification.CustomFormat = "dd.MM.yyyy HH:mm:ss";
            this.dateTimePickerModification.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dateTimePickerModification.Location = new System.Drawing.Point(217, 65);
            this.dateTimePickerModification.Name = "dateTimePickerModification";
            this.dateTimePickerModification.ShowCheckBox = true;
            this.dateTimePickerModification.Size = new System.Drawing.Size(147, 20);
            this.dateTimePickerModification.TabIndex = 4;
            // 
            // label3
            // 
            this.label3.Location = new System.Drawing.Point(13, 90);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(198, 23);
            this.label3.TabIndex = 5;
            this.label3.Text = "Last access time";
            this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // dateTimePickerAccess
            // 
            this.dateTimePickerAccess.CustomFormat = "dd.MM.yyyy HH:mm:ss";
            this.dateTimePickerAccess.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dateTimePickerAccess.Location = new System.Drawing.Point(217, 91);
            this.dateTimePickerAccess.Name = "dateTimePickerAccess";
            this.dateTimePickerAccess.ShowCheckBox = true;
            this.dateTimePickerAccess.Size = new System.Drawing.Size(147, 20);
            this.dateTimePickerAccess.TabIndex = 6;
            // 
            // TouchFileDialog
            // 
            this.AcceptButton = this.buttonOK;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.buttonCancel;
            this.ClientSize = new System.Drawing.Size(376, 155);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.dateTimePickerAccess);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.dateTimePickerModification);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.dateTimePickerCreation);
            this.Controls.Add(this.textBoxFileName);
            this.Controls.Add(this.panelButton);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.Name = "TouchFileDialog";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "TouchFileDialog";
            this.Activated += new System.EventHandler(this.TouchFileDialog_Activated);
            this.panelButton.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Panel panelButton;
        private System.Windows.Forms.Button buttonCancel;
        private System.Windows.Forms.Button buttonOK;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        public System.Windows.Forms.TextBox textBoxFileName;
        public System.Windows.Forms.DateTimePicker dateTimePickerCreation;
        public System.Windows.Forms.DateTimePicker dateTimePickerModification;
        public System.Windows.Forms.DateTimePicker dateTimePickerAccess;
    }
}