namespace netCommander
{
    partial class StreamViewer
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
            this.panelButton = new System.Windows.Forms.Panel();
            this.buttonDelete = new System.Windows.Forms.Button();
            this.buttonCopyTo = new System.Windows.Forms.Button();
            this.listViewStreams = new System.Windows.Forms.ListView();
            this.columnHeaderName = new System.Windows.Forms.ColumnHeader();
            this.columnHeaderSize = new System.Windows.Forms.ColumnHeader();
            this.columnHeaderAllocationSize = new System.Windows.Forms.ColumnHeader();
            this.folderBrowserDialog1 = new System.Windows.Forms.FolderBrowserDialog();
            this.saveFileDialog1 = new System.Windows.Forms.SaveFileDialog();
            this.panelButton.SuspendLayout();
            this.SuspendLayout();
            // 
            // panelButton
            // 
            this.panelButton.Controls.Add(this.buttonDelete);
            this.panelButton.Controls.Add(this.buttonCopyTo);
            this.panelButton.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panelButton.Location = new System.Drawing.Point(0, 267);
            this.panelButton.Name = "panelButton";
            this.panelButton.Size = new System.Drawing.Size(374, 35);
            this.panelButton.TabIndex = 1;
            // 
            // buttonDelete
            // 
            this.buttonDelete.Enabled = false;
            this.buttonDelete.Location = new System.Drawing.Point(87, 6);
            this.buttonDelete.Name = "buttonDelete";
            this.buttonDelete.Size = new System.Drawing.Size(75, 23);
            this.buttonDelete.TabIndex = 1;
            this.buttonDelete.Text = "Delete";
            this.buttonDelete.UseVisualStyleBackColor = true;
            this.buttonDelete.Click += new System.EventHandler(this.buttonDelete_Click);
            // 
            // buttonCopyTo
            // 
            this.buttonCopyTo.Enabled = false;
            this.buttonCopyTo.Location = new System.Drawing.Point(6, 6);
            this.buttonCopyTo.Name = "buttonCopyTo";
            this.buttonCopyTo.Size = new System.Drawing.Size(75, 23);
            this.buttonCopyTo.TabIndex = 0;
            this.buttonCopyTo.Text = "Copy to file";
            this.buttonCopyTo.UseVisualStyleBackColor = true;
            this.buttonCopyTo.Click += new System.EventHandler(this.buttonCopyTo_Click);
            // 
            // listViewStreams
            // 
            this.listViewStreams.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeaderName,
            this.columnHeaderSize,
            this.columnHeaderAllocationSize});
            this.listViewStreams.Dock = System.Windows.Forms.DockStyle.Fill;
            this.listViewStreams.FullRowSelect = true;
            this.listViewStreams.GridLines = true;
            this.listViewStreams.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.Nonclickable;
            this.listViewStreams.HideSelection = false;
            this.listViewStreams.Location = new System.Drawing.Point(0, 0);
            this.listViewStreams.Name = "listViewStreams";
            this.listViewStreams.Size = new System.Drawing.Size(374, 267);
            this.listViewStreams.TabIndex = 0;
            this.listViewStreams.UseCompatibleStateImageBehavior = false;
            this.listViewStreams.View = System.Windows.Forms.View.Details;
            // 
            // columnHeaderName
            // 
            this.columnHeaderName.Text = "Name";
            this.columnHeaderName.Width = 150;
            // 
            // columnHeaderSize
            // 
            this.columnHeaderSize.Text = "Data size";
            this.columnHeaderSize.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.columnHeaderSize.Width = 100;
            // 
            // columnHeaderAllocationSize
            // 
            this.columnHeaderAllocationSize.Text = "Allocation size";
            this.columnHeaderAllocationSize.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.columnHeaderAllocationSize.Width = 100;
            // 
            // folderBrowserDialog1
            // 
            this.folderBrowserDialog1.Description = "Select destination directory";
            // 
            // saveFileDialog1
            // 
            this.saveFileDialog1.AddExtension = false;
            this.saveFileDialog1.SupportMultiDottedExtensions = true;
            // 
            // StreamViewer
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.listViewStreams);
            this.Controls.Add(this.panelButton);
            this.Name = "StreamViewer";
            this.Size = new System.Drawing.Size(374, 302);
            this.panelButton.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panelButton;
        private System.Windows.Forms.Button buttonCopyTo;
        private System.Windows.Forms.Button buttonDelete;
        private System.Windows.Forms.ListView listViewStreams;
        private System.Windows.Forms.ColumnHeader columnHeaderName;
        private System.Windows.Forms.ColumnHeader columnHeaderSize;
        private System.Windows.Forms.ColumnHeader columnHeaderAllocationSize;
        private System.Windows.Forms.FolderBrowserDialog folderBrowserDialog1;
        private System.Windows.Forms.SaveFileDialog saveFileDialog1;
    }
}
