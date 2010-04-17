namespace netCommander
{
    partial class DoublePanel
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.mFilePanelLeft = new netCommander.winControls.mFilePanel();
            this.mFilePanelRight = new netCommander.winControls.mFilePanel();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.SuspendLayout();
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.Location = new System.Drawing.Point(0, 0);
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.mFilePanelLeft);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.mFilePanelRight);
            this.splitContainer1.Size = new System.Drawing.Size(534, 294);
            this.splitContainer1.SplitterDistance = 260;
            this.splitContainer1.TabIndex = 0;
            this.splitContainer1.TabStop = false;
            // 
            // mFilePanelLeft
            // 
            this.mFilePanelLeft.Dock = System.Windows.Forms.DockStyle.Fill;
            this.mFilePanelLeft.Location = new System.Drawing.Point(0, 0);
            this.mFilePanelLeft.Name = "mFilePanelLeft";
            this.mFilePanelLeft.Size = new System.Drawing.Size(260, 294);
            this.mFilePanelLeft.Source = null;
            this.mFilePanelLeft.TabIndex = 0;
            // 
            // mFilePanelRight
            // 
            this.mFilePanelRight.Dock = System.Windows.Forms.DockStyle.Fill;
            this.mFilePanelRight.Location = new System.Drawing.Point(0, 0);
            this.mFilePanelRight.Name = "mFilePanelRight";
            this.mFilePanelRight.Size = new System.Drawing.Size(270, 294);
            this.mFilePanelRight.Source = null;
            this.mFilePanelRight.TabIndex = 0;
            // 
            // DoublePanel
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.splitContainer1);
            this.Name = "DoublePanel";
            this.Size = new System.Drawing.Size(534, 294);
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            this.splitContainer1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.SplitContainer splitContainer1;
        private netCommander.winControls.mFilePanel mFilePanelLeft;
        private netCommander.winControls.mFilePanel mFilePanelRight;
    }
}
