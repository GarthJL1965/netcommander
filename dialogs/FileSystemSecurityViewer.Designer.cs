namespace netCommander
{
    partial class FileSystemSecurityViewer
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
            this.textBoxSddl = new System.Windows.Forms.TextBox();
            this.textBoxAcl = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // textBoxSddl
            // 
            this.textBoxSddl.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.textBoxSddl.Location = new System.Drawing.Point(0, 197);
            this.textBoxSddl.Multiline = true;
            this.textBoxSddl.Name = "textBoxSddl";
            this.textBoxSddl.ReadOnly = true;
            this.textBoxSddl.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.textBoxSddl.Size = new System.Drawing.Size(365, 48);
            this.textBoxSddl.TabIndex = 0;
            // 
            // textBoxAcl
            // 
            this.textBoxAcl.AcceptsReturn = true;
            this.textBoxAcl.AcceptsTab = true;
            this.textBoxAcl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.textBoxAcl.Location = new System.Drawing.Point(0, 0);
            this.textBoxAcl.Multiline = true;
            this.textBoxAcl.Name = "textBoxAcl";
            this.textBoxAcl.ReadOnly = true;
            this.textBoxAcl.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.textBoxAcl.Size = new System.Drawing.Size(365, 197);
            this.textBoxAcl.TabIndex = 1;
            // 
            // FileSystemSecurityViewer
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.textBoxAcl);
            this.Controls.Add(this.textBoxSddl);
            this.Name = "FileSystemSecurityViewer";
            this.Size = new System.Drawing.Size(365, 245);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox textBoxSddl;
        private System.Windows.Forms.TextBox textBoxAcl;

    }
}
