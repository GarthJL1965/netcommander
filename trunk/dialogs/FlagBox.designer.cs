namespace netCommander
{
    partial class FlagBox
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
            this.checkedListBoxInner = new System.Windows.Forms.CheckedListBox();
            this.SuspendLayout();
            // 
            // checkedListBoxInner
            // 
            this.checkedListBoxInner.CheckOnClick = true;
            this.checkedListBoxInner.Dock = System.Windows.Forms.DockStyle.Fill;
            this.checkedListBoxInner.FormattingEnabled = true;
            this.checkedListBoxInner.Location = new System.Drawing.Point(0, 0);
            this.checkedListBoxInner.Name = "checkedListBoxInner";
            this.checkedListBoxInner.Size = new System.Drawing.Size(231, 229);
            this.checkedListBoxInner.TabIndex = 0;
            this.checkedListBoxInner.ItemCheck += new System.Windows.Forms.ItemCheckEventHandler(this.checkedListBoxInner_ItemCheck);
            // 
            // FlagBox
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.checkedListBoxInner);
            this.Name = "FlagBox";
            this.Size = new System.Drawing.Size(231, 232);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.CheckedListBox checkedListBoxInner;
    }
}
