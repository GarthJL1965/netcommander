namespace netCommander
{
    partial class mainForm
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
            this.components = new System.ComponentModel.Container();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.toolStripStatusLabelCurrentState = new System.Windows.Forms.ToolStripStatusLabel();
            this.labelLongOperationStatus = new System.Windows.Forms.Label();
            this.fontDialog1 = new System.Windows.Forms.FontDialog();
            this.mainMenu1 = new System.Windows.Forms.MainMenu(this.components);
            this.menuItemBrowse = new System.Windows.Forms.MenuItem();
            this.menuItemPanel = new System.Windows.Forms.MenuItem();
            this.menuItemUserCommands = new System.Windows.Forms.MenuItem();
            this.menuItem_plugins = new System.Windows.Forms.MenuItem();
            this.menuItemOptions = new System.Windows.Forms.MenuItem();
            this.menuItemFont = new System.Windows.Forms.MenuItem();
            this.menuItem1 = new System.Windows.Forms.MenuItem();
            this.menuItemHelp = new System.Windows.Forms.MenuItem();
            this.menuItemAbout = new System.Windows.Forms.MenuItem();
            this.doublePanel1 = new netCommander.DoublePanel();
            this.commandPrompt = new netCommander.CommandPrompt();
            this.statusStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // statusStrip1
            // 
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripStatusLabelCurrentState});
            this.statusStrip1.Location = new System.Drawing.Point(0, -22);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(602, 22);
            this.statusStrip1.TabIndex = 2;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // toolStripStatusLabelCurrentState
            // 
            this.toolStripStatusLabelCurrentState.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.toolStripStatusLabelCurrentState.Name = "toolStripStatusLabelCurrentState";
            this.toolStripStatusLabelCurrentState.Size = new System.Drawing.Size(587, 17);
            this.toolStripStatusLabelCurrentState.Spring = true;
            this.toolStripStatusLabelCurrentState.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // labelLongOperationStatus
            // 
            this.labelLongOperationStatus.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.labelLongOperationStatus.AutoEllipsis = true;
            this.labelLongOperationStatus.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.labelLongOperationStatus.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.labelLongOperationStatus.Location = new System.Drawing.Point(167, -69);
            this.labelLongOperationStatus.Name = "labelLongOperationStatus";
            this.labelLongOperationStatus.Size = new System.Drawing.Size(254, 62);
            this.labelLongOperationStatus.TabIndex = 3;
            this.labelLongOperationStatus.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.labelLongOperationStatus.Visible = false;
            // 
            // fontDialog1
            // 
            this.fontDialog1.AllowScriptChange = false;
            this.fontDialog1.AllowVerticalFonts = false;
            this.fontDialog1.FixedPitchOnly = true;
            this.fontDialog1.FontMustExist = true;
            this.fontDialog1.ScriptsOnly = true;
            this.fontDialog1.ShowEffects = false;
            // 
            // mainMenu1
            // 
            this.mainMenu1.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.menuItemBrowse,
            this.menuItemPanel,
            this.menuItemUserCommands,
            this.menuItem_plugins,
            this.menuItemOptions,
            this.menuItemHelp});
            // 
            // menuItemBrowse
            // 
            this.menuItemBrowse.Index = 0;
            this.menuItemBrowse.Text = "browse";
            // 
            // menuItemPanel
            // 
            this.menuItemPanel.Index = 1;
            this.menuItemPanel.Text = "panel";
            // 
            // menuItemUserCommands
            // 
            this.menuItemUserCommands.Index = 2;
            this.menuItemUserCommands.Text = "user_menu";
            // 
            // menuItem_plugins
            // 
            this.menuItem_plugins.Index = 3;
            this.menuItem_plugins.Text = "plugins";
            // 
            // menuItemOptions
            // 
            this.menuItemOptions.Index = 4;
            this.menuItemOptions.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.menuItemFont,
            this.menuItem1});
            this.menuItemOptions.Text = "options";
            // 
            // menuItemFont
            // 
            this.menuItemFont.Index = 0;
            this.menuItemFont.Text = "font";
            this.menuItemFont.Click += new System.EventHandler(this.menuItemFont_Click);
            // 
            // menuItem1
            // 
            this.menuItem1.Index = 1;
            this.menuItem1.Text = "test";
            this.menuItem1.Visible = false;
            this.menuItem1.Click += new System.EventHandler(this.menuItem1_Click);
            // 
            // menuItemHelp
            // 
            this.menuItemHelp.Index = 5;
            this.menuItemHelp.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.menuItemAbout});
            this.menuItemHelp.Text = "help";
            // 
            // menuItemAbout
            // 
            this.menuItemAbout.Index = 0;
            this.menuItemAbout.Text = "about";
            this.menuItemAbout.Click += new System.EventHandler(this.menuItem3_Click);
            // 
            // doublePanel1
            // 
            this.doublePanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.doublePanel1.Font = new System.Drawing.Font("Courier New", 10F);
            this.doublePanel1.Location = new System.Drawing.Point(0, 0);
            this.doublePanel1.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.doublePanel1.Name = "doublePanel1";
            this.doublePanel1.Size = new System.Drawing.Size(602, 0);
            this.doublePanel1.TabIndex = 1;
            // 
            // commandPrompt
            // 
            this.commandPrompt.CommandText = "";
            this.commandPrompt.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.commandPrompt.Location = new System.Drawing.Point(0, -46);
            this.commandPrompt.Name = "commandPrompt";
            this.commandPrompt.Size = new System.Drawing.Size(602, 24);
            this.commandPrompt.TabIndex = 4;
            // 
            // mainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(602, 0);
            this.Controls.Add(this.labelLongOperationStatus);
            this.Controls.Add(this.doublePanel1);
            this.Controls.Add(this.commandPrompt);
            this.Controls.Add(this.statusStrip1);
            this.Menu = this.mainMenu1;
            this.Name = "mainForm";
            this.Text = "Form1";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.mainForm_FormClosing);
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private DoublePanel doublePanel1;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabelCurrentState;
        private System.Windows.Forms.Label labelLongOperationStatus;
        internal CommandPrompt commandPrompt;
        private System.Windows.Forms.FontDialog fontDialog1;
        private System.Windows.Forms.MainMenu mainMenu1;
        private System.Windows.Forms.MenuItem menuItemBrowse;
        private System.Windows.Forms.MenuItem menuItemPanel;
        private System.Windows.Forms.MenuItem menuItemUserCommands;
        private System.Windows.Forms.MenuItem menuItemOptions;
        private System.Windows.Forms.MenuItem menuItemFont;
        private System.Windows.Forms.MenuItem menuItem1;
        private System.Windows.Forms.MenuItem menuItem_plugins;
        private System.Windows.Forms.MenuItem menuItemHelp;
        private System.Windows.Forms.MenuItem menuItemAbout;
    }
}

