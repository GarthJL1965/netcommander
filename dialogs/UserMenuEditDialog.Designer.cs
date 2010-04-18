namespace netCommander
{
    partial class UserMenuEditDialog
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
            this.buttonEdit = new System.Windows.Forms.Button();
            this.buttonRemove = new System.Windows.Forms.Button();
            this.buttonAdd = new System.Windows.Forms.Button();
            this.buttonOK = new System.Windows.Forms.Button();
            this.listViewInternal = new System.Windows.Forms.ListView();
            this.columnHeaderMenuText = new System.Windows.Forms.ColumnHeader();
            this.columnHeaderCommand = new System.Windows.Forms.ColumnHeader();
            this.panelButton.SuspendLayout();
            this.SuspendLayout();
            // 
            // panelButton
            // 
            this.panelButton.Controls.Add(this.buttonEdit);
            this.panelButton.Controls.Add(this.buttonRemove);
            this.panelButton.Controls.Add(this.buttonAdd);
            this.panelButton.Controls.Add(this.buttonOK);
            this.panelButton.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panelButton.Location = new System.Drawing.Point(0, 229);
            this.panelButton.Name = "panelButton";
            this.panelButton.Size = new System.Drawing.Size(372, 35);
            this.panelButton.TabIndex = 1;
            // 
            // buttonEdit
            // 
            this.buttonEdit.Location = new System.Drawing.Point(175, 6);
            this.buttonEdit.Name = "buttonEdit";
            this.buttonEdit.Size = new System.Drawing.Size(75, 23);
            this.buttonEdit.TabIndex = 2;
            this.buttonEdit.Text = "button1";
            this.buttonEdit.UseVisualStyleBackColor = true;
            this.buttonEdit.Click += new System.EventHandler(this.buttonEdit_Click);
            // 
            // buttonRemove
            // 
            this.buttonRemove.Location = new System.Drawing.Point(94, 6);
            this.buttonRemove.Name = "buttonRemove";
            this.buttonRemove.Size = new System.Drawing.Size(75, 23);
            this.buttonRemove.TabIndex = 1;
            this.buttonRemove.Text = "button1";
            this.buttonRemove.UseVisualStyleBackColor = true;
            this.buttonRemove.Click += new System.EventHandler(this.buttonRemove_Click);
            // 
            // buttonAdd
            // 
            this.buttonAdd.Location = new System.Drawing.Point(12, 6);
            this.buttonAdd.Name = "buttonAdd";
            this.buttonAdd.Size = new System.Drawing.Size(75, 23);
            this.buttonAdd.TabIndex = 0;
            this.buttonAdd.Text = "button1";
            this.buttonAdd.UseVisualStyleBackColor = true;
            this.buttonAdd.Click += new System.EventHandler(this.buttonAdd_Click);
            // 
            // buttonOK
            // 
            this.buttonOK.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.buttonOK.Location = new System.Drawing.Point(285, 6);
            this.buttonOK.Name = "buttonOK";
            this.buttonOK.Size = new System.Drawing.Size(75, 23);
            this.buttonOK.TabIndex = 3;
            this.buttonOK.Text = "OK";
            this.buttonOK.UseVisualStyleBackColor = true;
            // 
            // listViewInternal
            // 
            this.listViewInternal.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeaderMenuText,
            this.columnHeaderCommand});
            this.listViewInternal.Dock = System.Windows.Forms.DockStyle.Fill;
            this.listViewInternal.FullRowSelect = true;
            this.listViewInternal.GridLines = true;
            this.listViewInternal.Location = new System.Drawing.Point(0, 0);
            this.listViewInternal.MultiSelect = false;
            this.listViewInternal.Name = "listViewInternal";
            this.listViewInternal.Size = new System.Drawing.Size(372, 229);
            this.listViewInternal.TabIndex = 0;
            this.listViewInternal.UseCompatibleStateImageBehavior = false;
            this.listViewInternal.View = System.Windows.Forms.View.Details;
            this.listViewInternal.DoubleClick += new System.EventHandler(this.listViewInternal_DoubleClick);
            // 
            // columnHeaderMenuText
            // 
            this.columnHeaderMenuText.Text = "Menu";
            this.columnHeaderMenuText.Width = 120;
            // 
            // columnHeaderCommand
            // 
            this.columnHeaderCommand.Text = "Command";
            this.columnHeaderCommand.Width = 240;
            // 
            // UserMenuEditDialog
            // 
            this.AcceptButton = this.buttonOK;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(372, 264);
            this.Controls.Add(this.listViewInternal);
            this.Controls.Add(this.panelButton);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.Name = "UserMenuEditDialog";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "UserMenuEditDialog";
            this.panelButton.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panelButton;
        public System.Windows.Forms.Button buttonOK;
        private System.Windows.Forms.ListView listViewInternal;
        private System.Windows.Forms.ColumnHeader columnHeaderMenuText;
        private System.Windows.Forms.ColumnHeader columnHeaderCommand;
        private System.Windows.Forms.Button buttonRemove;
        private System.Windows.Forms.Button buttonAdd;
        private System.Windows.Forms.Button buttonEdit;
    }
}