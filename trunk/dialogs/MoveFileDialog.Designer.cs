namespace netCommander
{
    partial class MoveFileDialog
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
            this.textBoxDestination = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.checkBoxSupressErrors = new System.Windows.Forms.CheckBox();
            this.checkBoxMoveSecirityAttributes = new System.Windows.Forms.CheckBox();
            this.checkBoxReplaceExisting = new System.Windows.Forms.CheckBox();
            this.textBoxMask = new System.Windows.Forms.TextBox();
            this.panelButton.SuspendLayout();
            this.SuspendLayout();
            // 
            // panelButton
            // 
            this.panelButton.Controls.Add(this.buttonOK);
            this.panelButton.Controls.Add(this.buttonCancel);
            this.panelButton.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panelButton.Location = new System.Drawing.Point(0, 131);
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
            // textBoxDestination
            // 
            this.textBoxDestination.Location = new System.Drawing.Point(96, 32);
            this.textBoxDestination.Name = "textBoxDestination";
            this.textBoxDestination.Size = new System.Drawing.Size(264, 20);
            this.textBoxDestination.TabIndex = 3;
            // 
            // label1
            // 
            this.label1.Location = new System.Drawing.Point(15, 35);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(75, 23);
            this.label1.TabIndex = 2;
            this.label1.Text = "Destination:";
            this.label1.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // label2
            // 
            this.label2.Location = new System.Drawing.Point(15, 9);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(75, 23);
            this.label2.TabIndex = 0;
            this.label2.Text = "Mask:";
            this.label2.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // checkBoxSupressErrors
            // 
            this.checkBoxSupressErrors.AutoSize = true;
            this.checkBoxSupressErrors.Location = new System.Drawing.Point(13, 62);
            this.checkBoxSupressErrors.Name = "checkBoxSupressErrors";
            this.checkBoxSupressErrors.Size = new System.Drawing.Size(93, 17);
            this.checkBoxSupressErrors.TabIndex = 4;
            this.checkBoxSupressErrors.Text = "Supress errors";
            this.checkBoxSupressErrors.UseVisualStyleBackColor = true;
            // 
            // checkBoxMoveSecirityAttributes
            // 
            this.checkBoxMoveSecirityAttributes.AutoSize = true;
            this.checkBoxMoveSecirityAttributes.Enabled = false;
            this.checkBoxMoveSecirityAttributes.Location = new System.Drawing.Point(13, 86);
            this.checkBoxMoveSecirityAttributes.Name = "checkBoxMoveSecirityAttributes";
            this.checkBoxMoveSecirityAttributes.Size = new System.Drawing.Size(302, 17);
            this.checkBoxMoveSecirityAttributes.TabIndex = 5;
            this.checkBoxMoveSecirityAttributes.Text = "Move security attributes while moving files across volumes ";
            this.checkBoxMoveSecirityAttributes.UseVisualStyleBackColor = true;
            // 
            // checkBoxReplaceExisting
            // 
            this.checkBoxReplaceExisting.AutoSize = true;
            this.checkBoxReplaceExisting.Location = new System.Drawing.Point(13, 110);
            this.checkBoxReplaceExisting.Name = "checkBoxReplaceExisting";
            this.checkBoxReplaceExisting.Size = new System.Drawing.Size(125, 17);
            this.checkBoxReplaceExisting.TabIndex = 6;
            this.checkBoxReplaceExisting.Text = "Replace existing files";
            this.checkBoxReplaceExisting.UseVisualStyleBackColor = true;
            // 
            // textBoxMask
            // 
            this.textBoxMask.Location = new System.Drawing.Point(96, 6);
            this.textBoxMask.Name = "textBoxMask";
            this.textBoxMask.Size = new System.Drawing.Size(264, 20);
            this.textBoxMask.TabIndex = 1;
            // 
            // MoveFileDialog
            // 
            this.AcceptButton = this.buttonOK;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.buttonCancel;
            this.ClientSize = new System.Drawing.Size(372, 166);
            this.Controls.Add(this.textBoxMask);
            this.Controls.Add(this.checkBoxReplaceExisting);
            this.Controls.Add(this.checkBoxMoveSecirityAttributes);
            this.Controls.Add(this.checkBoxSupressErrors);
            this.Controls.Add(this.textBoxDestination);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.panelButton);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.Name = "MoveFileDialog";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "MoveFileDialog";
            this.panelButton.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Panel panelButton;
        public System.Windows.Forms.Button buttonOK;
        private System.Windows.Forms.Button buttonCancel;
        public System.Windows.Forms.TextBox textBoxDestination;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.CheckBox checkBoxSupressErrors;
        private System.Windows.Forms.CheckBox checkBoxMoveSecirityAttributes;
        private System.Windows.Forms.CheckBox checkBoxReplaceExisting;
        public System.Windows.Forms.TextBox textBoxMask;
    }
}