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
            this.buttonClose = new System.Windows.Forms.Button();
            this.listViewResult = new System.Windows.Forms.ListView();
            this.columnHeaderStreamName = new System.Windows.Forms.ColumnHeader();
            this.columnHeaderStreamSize = new System.Windows.Forms.ColumnHeader();
            this.columnHeaderStreamID = new System.Windows.Forms.ColumnHeader();
            this.columnHeaderStreamAttributes = new System.Windows.Forms.ColumnHeader();
            this.buttonCopyToFile = new System.Windows.Forms.Button();
            this.saveFileDialog1 = new System.Windows.Forms.SaveFileDialog();
            this.panelButtons.SuspendLayout();
            this.SuspendLayout();
            // 
            // panelButtons
            // 
            this.panelButtons.Controls.Add(this.buttonCopyToFile);
            this.panelButtons.Controls.Add(this.buttonClose);
            this.panelButtons.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panelButtons.Location = new System.Drawing.Point(0, 251);
            this.panelButtons.Name = "panelButtons";
            this.panelButtons.Size = new System.Drawing.Size(632, 35);
            this.panelButtons.TabIndex = 1;
            // 
            // buttonClose
            // 
            this.buttonClose.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonClose.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.buttonClose.Location = new System.Drawing.Point(545, 6);
            this.buttonClose.Name = "buttonClose";
            this.buttonClose.Size = new System.Drawing.Size(75, 23);
            this.buttonClose.TabIndex = 0;
            this.buttonClose.Text = "Close";
            this.buttonClose.UseVisualStyleBackColor = true;
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
            this.listViewResult.Size = new System.Drawing.Size(632, 251);
            this.listViewResult.TabIndex = 0;
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
            // buttonCopyToFile
            // 
            this.buttonCopyToFile.Location = new System.Drawing.Point(13, 5);
            this.buttonCopyToFile.Name = "buttonCopyToFile";
            this.buttonCopyToFile.Size = new System.Drawing.Size(75, 23);
            this.buttonCopyToFile.TabIndex = 1;
            this.buttonCopyToFile.Text = "Copy to file";
            this.buttonCopyToFile.UseVisualStyleBackColor = true;
            this.buttonCopyToFile.Click += new System.EventHandler(this.buttonCopyToFile_Click);
            // 
            // AFSdialog
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.buttonClose;
            this.ClientSize = new System.Drawing.Size(632, 286);
            this.Controls.Add(this.listViewResult);
            this.Controls.Add(this.panelButtons);
            this.MinimumSize = new System.Drawing.Size(300, 34);
            this.Name = "AFSdialog";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "AFSdialog";
            this.panelButtons.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panelButtons;
        private System.Windows.Forms.ListView listViewResult;
        private System.Windows.Forms.ColumnHeader columnHeaderStreamName;
        private System.Windows.Forms.ColumnHeader columnHeaderStreamSize;
        private System.Windows.Forms.ColumnHeader columnHeaderStreamID;
        private System.Windows.Forms.ColumnHeader columnHeaderStreamAttributes;
        private System.Windows.Forms.Button buttonClose;
        private System.Windows.Forms.Button buttonCopyToFile;
        private System.Windows.Forms.SaveFileDialog saveFileDialog1;
    }
}