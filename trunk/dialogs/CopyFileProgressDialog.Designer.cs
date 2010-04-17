namespace netCommander
{
    partial class CopyFileProgressDialog
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
            this.buttonCancel = new System.Windows.Forms.Button();
            this.progressBarCurrentStream = new System.Windows.Forms.ProgressBar();
            this.labelStatus = new System.Windows.Forms.Label();
            this.labelStatusTotal = new System.Windows.Forms.Label();
            this.labelSpeed = new System.Windows.Forms.Label();
            this.checkBoxCloseOnFinish = new System.Windows.Forms.CheckBox();
            this.labelError = new System.Windows.Forms.Label();
            this.panelButton.SuspendLayout();
            this.SuspendLayout();
            // 
            // panelButton
            // 
            this.panelButton.Controls.Add(this.buttonCancel);
            this.panelButton.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panelButton.Location = new System.Drawing.Point(0, 187);
            this.panelButton.Name = "panelButton";
            this.panelButton.Size = new System.Drawing.Size(372, 35);
            this.panelButton.TabIndex = 7;
            // 
            // buttonCancel
            // 
            this.buttonCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.buttonCancel.Location = new System.Drawing.Point(289, 6);
            this.buttonCancel.Name = "buttonCancel";
            this.buttonCancel.Size = new System.Drawing.Size(75, 23);
            this.buttonCancel.TabIndex = 0;
            this.buttonCancel.Text = "Cancel";
            this.buttonCancel.UseVisualStyleBackColor = true;
            this.buttonCancel.Click += new System.EventHandler(this.buttonCancel_Click);
            // 
            // progressBarCurrentStream
            // 
            this.progressBarCurrentStream.Location = new System.Drawing.Point(12, 83);
            this.progressBarCurrentStream.MarqueeAnimationSpeed = 5000;
            this.progressBarCurrentStream.Name = "progressBarCurrentStream";
            this.progressBarCurrentStream.Size = new System.Drawing.Size(347, 17);
            this.progressBarCurrentStream.Style = System.Windows.Forms.ProgressBarStyle.Continuous;
            this.progressBarCurrentStream.TabIndex = 1;
            // 
            // labelStatus
            // 
            this.labelStatus.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.labelStatus.Location = new System.Drawing.Point(13, 13);
            this.labelStatus.Name = "labelStatus";
            this.labelStatus.Size = new System.Drawing.Size(346, 67);
            this.labelStatus.TabIndex = 0;
            this.labelStatus.Text = "label1";
            // 
            // labelStatusTotal
            // 
            this.labelStatusTotal.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.labelStatusTotal.Location = new System.Drawing.Point(13, 103);
            this.labelStatusTotal.Name = "labelStatusTotal";
            this.labelStatusTotal.Size = new System.Drawing.Size(346, 22);
            this.labelStatusTotal.TabIndex = 2;
            this.labelStatusTotal.Text = "label1";
            this.labelStatusTotal.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // labelSpeed
            // 
            this.labelSpeed.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.labelSpeed.Location = new System.Drawing.Point(12, 134);
            this.labelSpeed.Name = "labelSpeed";
            this.labelSpeed.Size = new System.Drawing.Size(346, 24);
            this.labelSpeed.TabIndex = 4;
            this.labelSpeed.Text = "label1";
            this.labelSpeed.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // checkBoxCloseOnFinish
            // 
            this.checkBoxCloseOnFinish.AutoSize = true;
            this.checkBoxCloseOnFinish.Location = new System.Drawing.Point(13, 162);
            this.checkBoxCloseOnFinish.Name = "checkBoxCloseOnFinish";
            this.checkBoxCloseOnFinish.Size = new System.Drawing.Size(94, 17);
            this.checkBoxCloseOnFinish.TabIndex = 5;
            this.checkBoxCloseOnFinish.Text = "Close on finish";
            this.checkBoxCloseOnFinish.UseVisualStyleBackColor = true;
            // 
            // labelError
            // 
            this.labelError.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.labelError.ForeColor = System.Drawing.Color.Red;
            this.labelError.Location = new System.Drawing.Point(149, 162);
            this.labelError.Name = "labelError";
            this.labelError.Size = new System.Drawing.Size(209, 23);
            this.labelError.TabIndex = 6;
            this.labelError.Text = "One or more files were not copied";
            this.labelError.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // CopyFileProgressDialog
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(372, 222);
            this.Controls.Add(this.labelError);
            this.Controls.Add(this.checkBoxCloseOnFinish);
            this.Controls.Add(this.labelSpeed);
            this.Controls.Add(this.labelStatusTotal);
            this.Controls.Add(this.labelStatus);
            this.Controls.Add(this.progressBarCurrentStream);
            this.Controls.Add(this.panelButton);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.Name = "CopyFileProgressDialog";
            this.Text = "CopyFileProgressDialog";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.CopyFileProgressDialog_FormClosing);
            this.panelButton.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Panel panelButton;
        private System.Windows.Forms.Button buttonCancel;
        public System.Windows.Forms.Label labelStatus;
        public System.Windows.Forms.ProgressBar progressBarCurrentStream;
        public System.Windows.Forms.Label labelStatusTotal;
        public System.Windows.Forms.Label labelSpeed;
        public System.Windows.Forms.CheckBox checkBoxCloseOnFinish;
        public System.Windows.Forms.Label labelError;
    }
}