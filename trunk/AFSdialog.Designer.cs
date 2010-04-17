namespace netCommander
{
    partial class AFSdialog
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
            this.panelButtons = new System.Windows.Forms.Panel();
            this.listViewResult = new System.Windows.Forms.ListView();
            this.columnHeaderStreamName = new System.Windows.Forms.ColumnHeader();
            this.columnHeaderStreamSize = new System.Windows.Forms.ColumnHeader();
            this.columnHeaderStreamID = new System.Windows.Forms.ColumnHeader();
            this.columnHeaderStreamAttributes = new System.Windows.Forms.ColumnHeader();
            this.SuspendLayout();
            // 
            // panelButtons
            // 
            this.panelButtons.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panelButtons.Location = new System.Drawing.Point(0, 249);
            this.panelButtons.Name = "panelButtons";
            this.panelButtons.Size = new System.Drawing.Size(520, 49);
            this.panelButtons.TabIndex = 0;
            // 
            // listViewResult
            // 
            this.listViewResult.AllowColumnReorder = true;
            this.listViewResult.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeaderStreamName,
            this.columnHeaderStreamSize,
            this.columnHeaderStreamID,
            this.columnHeaderStreamAttributes});
            this.listViewResult.Dock = System.Windows.Forms.DockStyle.Fill;
            this.listViewResult.FullRowSelect = true;
            this.listViewResult.GridLines = true;
            this.listViewResult.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.Nonclickable;
            this.listViewResult.HideSelection = false;
            this.listViewResult.Location = new System.Drawing.Point(0, 0);
            this.listViewResult.MultiSelect = false;
            this.listViewResult.Name = "listViewResult";
            this.listViewResult.Size = new System.Drawing.Size(520, 249);
            this.listViewResult.TabIndex = 1;
            this.listViewResult.UseCompatibleStateImageBehavior = false;
            this.listViewResult.View = System.Windows.Forms.View.Details;
            // 
            // columnHeaderStreamName
            // 
            this.columnHeaderStreamName.Text = "Name";
            this.columnHeaderStreamName.Width = 200;
            // 
            // columnHeaderStreamSize
            // 
            this.columnHeaderStreamSize.Text = "Size";
            this.columnHeaderStreamSize.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.columnHeaderStreamSize.Width = 100;
            // 
            // columnHeaderStreamID
            // 
            this.columnHeaderStreamID.Text = "ID";
            this.columnHeaderStreamID.Width = 100;
            // 
            // columnHeaderStreamAttributes
            // 
            this.columnHeaderStreamAttributes.Text = "Attributes";
            this.columnHeaderStreamAttributes.Width = 100;
            // 
            // AFSdialog
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(520, 298);
            this.Controls.Add(this.listViewResult);
            this.Controls.Add(this.panelButtons);
            this.Name = "AFSdialog";
            this.Text = "AFSdialog";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panelButtons;
        private System.Windows.Forms.ListView listViewResult;
        private System.Windows.Forms.ColumnHeader columnHeaderStreamName;
        private System.Windows.Forms.ColumnHeader columnHeaderStreamSize;
        private System.Windows.Forms.ColumnHeader columnHeaderStreamID;
        private System.Windows.Forms.ColumnHeader columnHeaderStreamAttributes;
    }
}