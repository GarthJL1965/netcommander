namespace netCommander.winControls
{
    partial class mFilePanel
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
            this.inner_ListControl = new netCommander.winControls.mListControl();
            this.inner_InfoPanel = new netCommander.winControls.mInfoPanel();
            this.SuspendLayout();
            // 
            // inner_ListControl
            // 
            this.inner_ListControl.AllowDrop = true;
            this.inner_ListControl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.inner_ListControl.FocusedIndex = 0;
            this.inner_ListControl.Location = new System.Drawing.Point(0, 0);
            this.inner_ListControl.Name = "inner_ListControl";
            this.inner_ListControl.Size = new System.Drawing.Size(249, 221);
            this.inner_ListControl.TabIndex = 1;
            this.inner_ListControl.Text = "mListControl1";
            this.inner_ListControl.TextRenderingHint = System.Drawing.Text.TextRenderingHint.SystemDefault;
            this.inner_ListControl.UpdateEnable = true;
            // 
            // inner_InfoPanel
            // 
            this.inner_InfoPanel.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.inner_InfoPanel.Location = new System.Drawing.Point(0, 221);
            this.inner_InfoPanel.Name = "inner_InfoPanel";
            this.inner_InfoPanel.ShowBorder = true;
            this.inner_InfoPanel.Size = new System.Drawing.Size(249, 55);
            this.inner_InfoPanel.TabIndex = 0;
            this.inner_InfoPanel.TabStop = false;
            this.inner_InfoPanel.Text = "mInfoPanel1";
            this.inner_InfoPanel.TextRenderingHint = System.Drawing.Text.TextRenderingHint.SystemDefault;
            // 
            // mFilePanel
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.inner_ListControl);
            this.Controls.Add(this.inner_InfoPanel);
            this.Name = "mFilePanel";
            this.Size = new System.Drawing.Size(249, 276);
            this.ResumeLayout(false);

        }

        #endregion

        private netCommander.winControls.mInfoPanel inner_InfoPanel;
        private netCommander.winControls.mListControl inner_ListControl;
    }
}
