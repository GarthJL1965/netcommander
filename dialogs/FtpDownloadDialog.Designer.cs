namespace netCommander
{
    partial class FtpTransferDialog
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FtpTransferDialog));
            this.panelButton = new System.Windows.Forms.Panel();
            this.buttonOK = new System.Windows.Forms.Button();
            this.buttonCancel = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.labelSourceFile = new System.Windows.Forms.Label();
            this.textBoxDestination = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.checkBoxShowTotal = new System.Windows.Forms.CheckBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.radioButtonOverwriteAlways = new System.Windows.Forms.RadioButton();
            this.radioButtonOvewriteSOurceNewer = new System.Windows.Forms.RadioButton();
            this.radioButtonOverwriteNo = new System.Windows.Forms.RadioButton();
            this.checkBoxSupressErrors = new System.Windows.Forms.CheckBox();
            this.label3 = new System.Windows.Forms.Label();
            this.textBoxBufferSize = new System.Windows.Forms.TextBox();
            this.panelButton.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // panelButton
            // 
            this.panelButton.Controls.Add(this.buttonOK);
            this.panelButton.Controls.Add(this.buttonCancel);
            resources.ApplyResources(this.panelButton, "panelButton");
            this.panelButton.Name = "panelButton";
            // 
            // buttonOK
            // 
            this.buttonOK.DialogResult = System.Windows.Forms.DialogResult.OK;
            resources.ApplyResources(this.buttonOK, "buttonOK");
            this.buttonOK.Name = "buttonOK";
            this.buttonOK.UseVisualStyleBackColor = true;
            // 
            // buttonCancel
            // 
            this.buttonCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            resources.ApplyResources(this.buttonCancel, "buttonCancel");
            this.buttonCancel.Name = "buttonCancel";
            this.buttonCancel.UseVisualStyleBackColor = true;
            // 
            // label2
            // 
            resources.ApplyResources(this.label2, "label2");
            this.label2.Name = "label2";
            // 
            // labelSourceFile
            // 
            this.labelSourceFile.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            resources.ApplyResources(this.labelSourceFile, "labelSourceFile");
            this.labelSourceFile.Name = "labelSourceFile";
            // 
            // textBoxDestination
            // 
            resources.ApplyResources(this.textBoxDestination, "textBoxDestination");
            this.textBoxDestination.Name = "textBoxDestination";
            // 
            // label1
            // 
            resources.ApplyResources(this.label1, "label1");
            this.label1.Name = "label1";
            // 
            // checkBoxShowTotal
            // 
            resources.ApplyResources(this.checkBoxShowTotal, "checkBoxShowTotal");
            this.checkBoxShowTotal.Name = "checkBoxShowTotal";
            this.checkBoxShowTotal.UseVisualStyleBackColor = true;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.radioButtonOverwriteAlways);
            this.groupBox1.Controls.Add(this.radioButtonOvewriteSOurceNewer);
            this.groupBox1.Controls.Add(this.radioButtonOverwriteNo);
            resources.ApplyResources(this.groupBox1, "groupBox1");
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.TabStop = false;
            // 
            // radioButtonOverwriteAlways
            // 
            resources.ApplyResources(this.radioButtonOverwriteAlways, "radioButtonOverwriteAlways");
            this.radioButtonOverwriteAlways.Name = "radioButtonOverwriteAlways";
            this.radioButtonOverwriteAlways.TabStop = true;
            this.radioButtonOverwriteAlways.UseVisualStyleBackColor = true;
            // 
            // radioButtonOvewriteSOurceNewer
            // 
            resources.ApplyResources(this.radioButtonOvewriteSOurceNewer, "radioButtonOvewriteSOurceNewer");
            this.radioButtonOvewriteSOurceNewer.Name = "radioButtonOvewriteSOurceNewer";
            this.radioButtonOvewriteSOurceNewer.TabStop = true;
            this.radioButtonOvewriteSOurceNewer.UseVisualStyleBackColor = true;
            // 
            // radioButtonOverwriteNo
            // 
            resources.ApplyResources(this.radioButtonOverwriteNo, "radioButtonOverwriteNo");
            this.radioButtonOverwriteNo.Name = "radioButtonOverwriteNo";
            this.radioButtonOverwriteNo.TabStop = true;
            this.radioButtonOverwriteNo.UseVisualStyleBackColor = true;
            // 
            // checkBoxSupressErrors
            // 
            resources.ApplyResources(this.checkBoxSupressErrors, "checkBoxSupressErrors");
            this.checkBoxSupressErrors.Name = "checkBoxSupressErrors";
            this.checkBoxSupressErrors.UseVisualStyleBackColor = true;
            // 
            // label3
            // 
            resources.ApplyResources(this.label3, "label3");
            this.label3.Name = "label3";
            // 
            // textBoxBufferSize
            // 
            resources.ApplyResources(this.textBoxBufferSize, "textBoxBufferSize");
            this.textBoxBufferSize.Name = "textBoxBufferSize";
            // 
            // FtpTransferDialog
            // 
            this.AcceptButton = this.buttonOK;
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.buttonCancel;
            this.Controls.Add(this.checkBoxSupressErrors);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.textBoxBufferSize);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.checkBoxShowTotal);
            this.Controls.Add(this.textBoxDestination);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.labelSourceFile);
            this.Controls.Add(this.panelButton);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.Name = "FtpTransferDialog";
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
        private System.Windows.Forms.Label label2;
        public System.Windows.Forms.Label labelSourceFile;
        public System.Windows.Forms.TextBox textBoxDestination;
        private System.Windows.Forms.Label label1;
        public System.Windows.Forms.CheckBox checkBoxShowTotal;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.RadioButton radioButtonOvewriteSOurceNewer;
        public System.Windows.Forms.RadioButton radioButtonOverwriteNo;
        private System.Windows.Forms.RadioButton radioButtonOverwriteAlways;
        private System.Windows.Forms.CheckBox checkBoxSupressErrors;
        private System.Windows.Forms.Label label3;
        public System.Windows.Forms.TextBox textBoxBufferSize;
    }
}