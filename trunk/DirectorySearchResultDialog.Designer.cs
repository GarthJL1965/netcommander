namespace netCommander
{
    partial class DirectorySearchResultDialog
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
            this.listViewResult = new System.Windows.Forms.ListView();
            this.columnHeaderName = new System.Windows.Forms.ColumnHeader();
            this.columnHeaderSize = new System.Windows.Forms.ColumnHeader();
            this.columnHeaderCreate = new System.Windows.Forms.ColumnHeader();
            this.columnHeaderModification = new System.Windows.Forms.ColumnHeader();
            this.columnHeaderAccess = new System.Windows.Forms.ColumnHeader();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.toolStripStatusLabelCurrentDir = new System.Windows.Forms.ToolStripStatusLabel();
            this.columnHeaderAttr = new System.Windows.Forms.ColumnHeader();
            this.buttonStop = new System.Windows.Forms.Button();
            this.toolStripStatusLabelCount = new System.Windows.Forms.ToolStripStatusLabel();
            this.statusStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // listViewResult
            // 
            this.listViewResult.AllowColumnReorder = true;
            this.listViewResult.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeaderName,
            this.columnHeaderSize,
            this.columnHeaderAttr,
            this.columnHeaderCreate,
            this.columnHeaderModification,
            this.columnHeaderAccess});
            this.listViewResult.Dock = System.Windows.Forms.DockStyle.Top;
            this.listViewResult.FullRowSelect = true;
            this.listViewResult.GridLines = true;
            this.listViewResult.HideSelection = false;
            this.listViewResult.Location = new System.Drawing.Point(0, 0);
            this.listViewResult.MultiSelect = false;
            this.listViewResult.Name = "listViewResult";
            this.listViewResult.ShowGroups = false;
            this.listViewResult.Size = new System.Drawing.Size(634, 224);
            this.listViewResult.TabIndex = 0;
            this.listViewResult.UseCompatibleStateImageBehavior = false;
            this.listViewResult.View = System.Windows.Forms.View.Details;
            this.listViewResult.ColumnClick += new System.Windows.Forms.ColumnClickEventHandler(this.listViewResult_ColumnClick);
            // 
            // columnHeaderName
            // 
            this.columnHeaderName.Text = "Path and name";
            this.columnHeaderName.Width = 350;
            // 
            // columnHeaderSize
            // 
            this.columnHeaderSize.Text = "Size";
            this.columnHeaderSize.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.columnHeaderSize.Width = 100;
            // 
            // columnHeaderCreate
            // 
            this.columnHeaderCreate.Text = "Create";
            this.columnHeaderCreate.Width = 120;
            // 
            // columnHeaderModification
            // 
            this.columnHeaderModification.Text = "Modification";
            this.columnHeaderModification.Width = 120;
            // 
            // columnHeaderAccess
            // 
            this.columnHeaderAccess.Text = "Access";
            this.columnHeaderAccess.Width = 120;
            // 
            // statusStrip1
            // 
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripStatusLabelCount,
            this.toolStripStatusLabelCurrentDir});
            this.statusStrip1.Location = new System.Drawing.Point(0, 263);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(634, 22);
            this.statusStrip1.TabIndex = 1;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // toolStripStatusLabelCurrentDir
            // 
            this.toolStripStatusLabelCurrentDir.AutoSize = false;
            this.toolStripStatusLabelCurrentDir.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.toolStripStatusLabelCurrentDir.Name = "toolStripStatusLabelCurrentDir";
            this.toolStripStatusLabelCurrentDir.Size = new System.Drawing.Size(479, 17);
            this.toolStripStatusLabelCurrentDir.Spring = true;
            this.toolStripStatusLabelCurrentDir.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // columnHeaderAttr
            // 
            this.columnHeaderAttr.Text = "Attributes";
            this.columnHeaderAttr.Width = 80;
            // 
            // buttonStop
            // 
            this.buttonStop.Location = new System.Drawing.Point(13, 231);
            this.buttonStop.Name = "buttonStop";
            this.buttonStop.Size = new System.Drawing.Size(75, 23);
            this.buttonStop.TabIndex = 2;
            this.buttonStop.Text = "Stop";
            this.buttonStop.UseVisualStyleBackColor = true;
            this.buttonStop.Click += new System.EventHandler(this.buttonStop_Click);
            // 
            // toolStripStatusLabelCount
            // 
            this.toolStripStatusLabelCount.AutoSize = false;
            this.toolStripStatusLabelCount.BorderSides = System.Windows.Forms.ToolStripStatusLabelBorderSides.Right;
            this.toolStripStatusLabelCount.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.toolStripStatusLabelCount.Name = "toolStripStatusLabelCount";
            this.toolStripStatusLabelCount.Size = new System.Drawing.Size(140, 17);
            this.toolStripStatusLabelCount.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // DirectorySearchResultDialog
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(634, 285);
            this.Controls.Add(this.buttonStop);
            this.Controls.Add(this.statusStrip1);
            this.Controls.Add(this.listViewResult);
            this.Name = "DirectorySearchResultDialog";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "DirectorySearchResultDialog";
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ListView listViewResult;
        private System.Windows.Forms.ColumnHeader columnHeaderName;
        private System.Windows.Forms.ColumnHeader columnHeaderSize;
        private System.Windows.Forms.ColumnHeader columnHeaderCreate;
        private System.Windows.Forms.ColumnHeader columnHeaderModification;
        private System.Windows.Forms.ColumnHeader columnHeaderAccess;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabelCurrentDir;
        private System.Windows.Forms.ColumnHeader columnHeaderAttr;
        private System.Windows.Forms.Button buttonStop;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabelCount;
    }
}