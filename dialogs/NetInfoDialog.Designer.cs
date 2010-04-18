namespace netCommander
{
    partial class NetInfoDialog
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
            this.panelButton = new System.Windows.Forms.Panel();
            this.buttonClose = new System.Windows.Forms.Button();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPageResourceInfo = new System.Windows.Forms.TabPage();
            this.labelNetworkProviderVersion = new System.Windows.Forms.Label();
            this.textBoxNetworkProviderVersion = new System.Windows.Forms.TextBox();
            this.labelNetworkProviderType = new System.Windows.Forms.Label();
            this.textBoxNetworkProviderType = new System.Windows.Forms.TextBox();
            this.labelProviderName = new System.Windows.Forms.Label();
            this.textBoxProviderName = new System.Windows.Forms.TextBox();
            this.label1ResourceComment = new System.Windows.Forms.Label();
            this.textBoxResourceComment = new System.Windows.Forms.TextBox();
            this.labelResourceType = new System.Windows.Forms.Label();
            this.textBoxResourceType = new System.Windows.Forms.TextBox();
            this.labelDisplayType = new System.Windows.Forms.Label();
            this.textBoxDisplayType = new System.Windows.Forms.TextBox();
            this.labelRemoteName = new System.Windows.Forms.Label();
            this.textBoxRemoteName = new System.Windows.Forms.TextBox();
            this.tabPageServerInfo = new System.Windows.Forms.TabPage();
            this.labelNetserverUptime = new System.Windows.Forms.Label();
            this.labelNetserverDatetime = new System.Windows.Forms.Label();
            this.textBoxNetserverUptime = new System.Windows.Forms.TextBox();
            this.textBoxNetserverDatetime = new System.Windows.Forms.TextBox();
            this.labelNetserverFeatures = new System.Windows.Forms.Label();
            this.textBoxNetserverFeatures = new System.Windows.Forms.TextBox();
            this.labelNetserverUserPath = new System.Windows.Forms.Label();
            this.textBoxNetserverUserPath = new System.Windows.Forms.TextBox();
            this.labelNetserverUsersPerLicense = new System.Windows.Forms.Label();
            this.textBoxNetserverUsersPerLicense = new System.Windows.Forms.TextBox();
            this.labelNetserverAnnounceTime = new System.Windows.Forms.Label();
            this.textBoxNetserverAnnounceTime = new System.Windows.Forms.TextBox();
            this.labelNetserverDisconnectTime = new System.Windows.Forms.Label();
            this.textBoxNetserverDisconnectTime = new System.Windows.Forms.TextBox();
            this.labelNetserverMaximumUsers = new System.Windows.Forms.Label();
            this.textBoxNetserverMaxUsers = new System.Windows.Forms.TextBox();
            this.labelPlatformVersion = new System.Windows.Forms.Label();
            this.textBoxPlatformVersion = new System.Windows.Forms.TextBox();
            this.labelNetworkServerSoftwareType = new System.Windows.Forms.Label();
            this.textBoxNetserverSoftwareType = new System.Windows.Forms.TextBox();
            this.labelNetserverPlatformId = new System.Windows.Forms.Label();
            this.textBoxNetserverPlatformId = new System.Windows.Forms.TextBox();
            this.errorProvider1 = new System.Windows.Forms.ErrorProvider(this.components);
            this.tabPageTransportInfo = new System.Windows.Forms.TabPage();
            this.listViewTransports = new System.Windows.Forms.ListView();
            this.columnHeaderTransportName = new System.Windows.Forms.ColumnHeader();
            this.columnHeaderTransportAddress = new System.Windows.Forms.ColumnHeader();
            this.columnHeaderNetworkAddress = new System.Windows.Forms.ColumnHeader();
            this.columnHeaderDomain = new System.Windows.Forms.ColumnHeader();
            this.columnHeaderNumerOfVcs = new System.Windows.Forms.ColumnHeader();
            this.tabPageSessionInfo = new System.Windows.Forms.TabPage();
            this.listViewSessions = new System.Windows.Forms.ListView();
            this.columnHeaderComputerName = new System.Windows.Forms.ColumnHeader();
            this.columnHeaderUserName = new System.Windows.Forms.ColumnHeader();
            this.columnHeaderNumberOfOpenFiles = new System.Windows.Forms.ColumnHeader();
            this.columnHeaderActivetime = new System.Windows.Forms.ColumnHeader();
            this.columnHeaderIdletime = new System.Windows.Forms.ColumnHeader();
            this.columnHeaderEstablishtype = new System.Windows.Forms.ColumnHeader();
            this.columnHeaderClientType = new System.Windows.Forms.ColumnHeader();
            this.columnHeaderSessionTransportName = new System.Windows.Forms.ColumnHeader();
            this.tabPageFilesInfo = new System.Windows.Forms.TabPage();
            this.listViewFiles = new System.Windows.Forms.ListView();
            this.columnHeaderFileId = new System.Windows.Forms.ColumnHeader();
            this.columnHeaderFilePermissions = new System.Windows.Forms.ColumnHeader();
            this.columnHeaderNumberOfLocks = new System.Windows.Forms.ColumnHeader();
            this.columnHeaderFilePath = new System.Windows.Forms.ColumnHeader();
            this.columnHeaderFileUserName = new System.Windows.Forms.ColumnHeader();
            this.tabPageShareInfo = new System.Windows.Forms.TabPage();
            this.labelShareType = new System.Windows.Forms.Label();
            this.textBoxShareType = new System.Windows.Forms.TextBox();
            this.labelShareComment = new System.Windows.Forms.Label();
            this.textBoxShareComment = new System.Windows.Forms.TextBox();
            this.labelSharePermissions = new System.Windows.Forms.Label();
            this.textBoxSharePermissions = new System.Windows.Forms.TextBox();
            this.labelShareMaxUses = new System.Windows.Forms.Label();
            this.textBoxShareMaxuses = new System.Windows.Forms.TextBox();
            this.labelShareCurrentUses = new System.Windows.Forms.Label();
            this.textBoxShareCurrentUses = new System.Windows.Forms.TextBox();
            this.labelSharePath = new System.Windows.Forms.Label();
            this.textBoxSharePath = new System.Windows.Forms.TextBox();
            this.panelButton.SuspendLayout();
            this.tabControl1.SuspendLayout();
            this.tabPageResourceInfo.SuspendLayout();
            this.tabPageServerInfo.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider1)).BeginInit();
            this.tabPageTransportInfo.SuspendLayout();
            this.tabPageSessionInfo.SuspendLayout();
            this.tabPageFilesInfo.SuspendLayout();
            this.tabPageShareInfo.SuspendLayout();
            this.SuspendLayout();
            // 
            // panelButton
            // 
            this.panelButton.Controls.Add(this.buttonClose);
            this.panelButton.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panelButton.Location = new System.Drawing.Point(0, 315);
            this.panelButton.Name = "panelButton";
            this.panelButton.Size = new System.Drawing.Size(594, 35);
            this.panelButton.TabIndex = 2;
            // 
            // buttonClose
            // 
            this.buttonClose.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.buttonClose.Location = new System.Drawing.Point(507, 6);
            this.buttonClose.Name = "buttonClose";
            this.buttonClose.Size = new System.Drawing.Size(75, 23);
            this.buttonClose.TabIndex = 0;
            this.buttonClose.Text = "Close";
            this.buttonClose.UseVisualStyleBackColor = true;
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabPageResourceInfo);
            this.tabControl1.Controls.Add(this.tabPageServerInfo);
            this.tabControl1.Controls.Add(this.tabPageTransportInfo);
            this.tabControl1.Controls.Add(this.tabPageSessionInfo);
            this.tabControl1.Controls.Add(this.tabPageFilesInfo);
            this.tabControl1.Controls.Add(this.tabPageShareInfo);
            this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl1.Location = new System.Drawing.Point(0, 0);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(594, 315);
            this.tabControl1.TabIndex = 3;
            // 
            // tabPageResourceInfo
            // 
            this.tabPageResourceInfo.Controls.Add(this.labelNetworkProviderVersion);
            this.tabPageResourceInfo.Controls.Add(this.textBoxNetworkProviderVersion);
            this.tabPageResourceInfo.Controls.Add(this.labelNetworkProviderType);
            this.tabPageResourceInfo.Controls.Add(this.textBoxNetworkProviderType);
            this.tabPageResourceInfo.Controls.Add(this.labelProviderName);
            this.tabPageResourceInfo.Controls.Add(this.textBoxProviderName);
            this.tabPageResourceInfo.Controls.Add(this.label1ResourceComment);
            this.tabPageResourceInfo.Controls.Add(this.textBoxResourceComment);
            this.tabPageResourceInfo.Controls.Add(this.labelResourceType);
            this.tabPageResourceInfo.Controls.Add(this.textBoxResourceType);
            this.tabPageResourceInfo.Controls.Add(this.labelDisplayType);
            this.tabPageResourceInfo.Controls.Add(this.textBoxDisplayType);
            this.tabPageResourceInfo.Controls.Add(this.labelRemoteName);
            this.tabPageResourceInfo.Controls.Add(this.textBoxRemoteName);
            this.tabPageResourceInfo.Location = new System.Drawing.Point(4, 22);
            this.tabPageResourceInfo.Name = "tabPageResourceInfo";
            this.tabPageResourceInfo.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageResourceInfo.Size = new System.Drawing.Size(586, 289);
            this.tabPageResourceInfo.TabIndex = 0;
            this.tabPageResourceInfo.Text = "tabPage1";
            this.tabPageResourceInfo.UseVisualStyleBackColor = true;
            // 
            // labelNetworkProviderVersion
            // 
            this.labelNetworkProviderVersion.Location = new System.Drawing.Point(8, 159);
            this.labelNetworkProviderVersion.Name = "labelNetworkProviderVersion";
            this.labelNetworkProviderVersion.Size = new System.Drawing.Size(275, 23);
            this.labelNetworkProviderVersion.TabIndex = 15;
            this.labelNetworkProviderVersion.Text = "Network provider version";
            this.labelNetworkProviderVersion.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // textBoxNetworkProviderVersion
            // 
            this.textBoxNetworkProviderVersion.Location = new System.Drawing.Point(289, 162);
            this.textBoxNetworkProviderVersion.Name = "textBoxNetworkProviderVersion";
            this.textBoxNetworkProviderVersion.ReadOnly = true;
            this.textBoxNetworkProviderVersion.Size = new System.Drawing.Size(288, 20);
            this.textBoxNetworkProviderVersion.TabIndex = 16;
            // 
            // labelNetworkProviderType
            // 
            this.labelNetworkProviderType.Location = new System.Drawing.Point(8, 133);
            this.labelNetworkProviderType.Name = "labelNetworkProviderType";
            this.labelNetworkProviderType.Size = new System.Drawing.Size(275, 23);
            this.labelNetworkProviderType.TabIndex = 13;
            this.labelNetworkProviderType.Text = "Network provider type";
            this.labelNetworkProviderType.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // textBoxNetworkProviderType
            // 
            this.textBoxNetworkProviderType.Location = new System.Drawing.Point(289, 136);
            this.textBoxNetworkProviderType.Name = "textBoxNetworkProviderType";
            this.textBoxNetworkProviderType.ReadOnly = true;
            this.textBoxNetworkProviderType.Size = new System.Drawing.Size(288, 20);
            this.textBoxNetworkProviderType.TabIndex = 14;
            // 
            // labelProviderName
            // 
            this.labelProviderName.Location = new System.Drawing.Point(8, 107);
            this.labelProviderName.Name = "labelProviderName";
            this.labelProviderName.Size = new System.Drawing.Size(275, 23);
            this.labelProviderName.TabIndex = 11;
            this.labelProviderName.Text = "Network provider";
            this.labelProviderName.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // textBoxProviderName
            // 
            this.textBoxProviderName.Location = new System.Drawing.Point(289, 110);
            this.textBoxProviderName.Name = "textBoxProviderName";
            this.textBoxProviderName.ReadOnly = true;
            this.textBoxProviderName.Size = new System.Drawing.Size(288, 20);
            this.textBoxProviderName.TabIndex = 12;
            // 
            // label1ResourceComment
            // 
            this.label1ResourceComment.Location = new System.Drawing.Point(8, 81);
            this.label1ResourceComment.Name = "label1ResourceComment";
            this.label1ResourceComment.Size = new System.Drawing.Size(275, 23);
            this.label1ResourceComment.TabIndex = 9;
            this.label1ResourceComment.Text = "Resource comment";
            this.label1ResourceComment.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // textBoxResourceComment
            // 
            this.textBoxResourceComment.Location = new System.Drawing.Point(289, 84);
            this.textBoxResourceComment.Name = "textBoxResourceComment";
            this.textBoxResourceComment.ReadOnly = true;
            this.textBoxResourceComment.Size = new System.Drawing.Size(288, 20);
            this.textBoxResourceComment.TabIndex = 10;
            // 
            // labelResourceType
            // 
            this.labelResourceType.Location = new System.Drawing.Point(8, 55);
            this.labelResourceType.Name = "labelResourceType";
            this.labelResourceType.Size = new System.Drawing.Size(275, 23);
            this.labelResourceType.TabIndex = 7;
            this.labelResourceType.Text = "Resource type";
            this.labelResourceType.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // textBoxResourceType
            // 
            this.textBoxResourceType.Location = new System.Drawing.Point(289, 58);
            this.textBoxResourceType.Name = "textBoxResourceType";
            this.textBoxResourceType.ReadOnly = true;
            this.textBoxResourceType.Size = new System.Drawing.Size(288, 20);
            this.textBoxResourceType.TabIndex = 8;
            // 
            // labelDisplayType
            // 
            this.labelDisplayType.Location = new System.Drawing.Point(8, 29);
            this.labelDisplayType.Name = "labelDisplayType";
            this.labelDisplayType.Size = new System.Drawing.Size(275, 23);
            this.labelDisplayType.TabIndex = 5;
            this.labelDisplayType.Text = "Display type";
            this.labelDisplayType.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // textBoxDisplayType
            // 
            this.textBoxDisplayType.Location = new System.Drawing.Point(289, 32);
            this.textBoxDisplayType.Name = "textBoxDisplayType";
            this.textBoxDisplayType.ReadOnly = true;
            this.textBoxDisplayType.Size = new System.Drawing.Size(288, 20);
            this.textBoxDisplayType.TabIndex = 6;
            // 
            // labelRemoteName
            // 
            this.labelRemoteName.Location = new System.Drawing.Point(8, 3);
            this.labelRemoteName.Name = "labelRemoteName";
            this.labelRemoteName.Size = new System.Drawing.Size(275, 23);
            this.labelRemoteName.TabIndex = 3;
            this.labelRemoteName.Text = "Remote name";
            this.labelRemoteName.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // textBoxRemoteName
            // 
            this.textBoxRemoteName.Location = new System.Drawing.Point(289, 6);
            this.textBoxRemoteName.Name = "textBoxRemoteName";
            this.textBoxRemoteName.ReadOnly = true;
            this.textBoxRemoteName.Size = new System.Drawing.Size(288, 20);
            this.textBoxRemoteName.TabIndex = 4;
            // 
            // tabPageServerInfo
            // 
            this.tabPageServerInfo.Controls.Add(this.labelNetserverUptime);
            this.tabPageServerInfo.Controls.Add(this.labelNetserverDatetime);
            this.tabPageServerInfo.Controls.Add(this.textBoxNetserverUptime);
            this.tabPageServerInfo.Controls.Add(this.textBoxNetserverDatetime);
            this.tabPageServerInfo.Controls.Add(this.labelNetserverFeatures);
            this.tabPageServerInfo.Controls.Add(this.textBoxNetserverFeatures);
            this.tabPageServerInfo.Controls.Add(this.labelNetserverUserPath);
            this.tabPageServerInfo.Controls.Add(this.textBoxNetserverUserPath);
            this.tabPageServerInfo.Controls.Add(this.labelNetserverUsersPerLicense);
            this.tabPageServerInfo.Controls.Add(this.textBoxNetserverUsersPerLicense);
            this.tabPageServerInfo.Controls.Add(this.labelNetserverAnnounceTime);
            this.tabPageServerInfo.Controls.Add(this.textBoxNetserverAnnounceTime);
            this.tabPageServerInfo.Controls.Add(this.labelNetserverDisconnectTime);
            this.tabPageServerInfo.Controls.Add(this.textBoxNetserverDisconnectTime);
            this.tabPageServerInfo.Controls.Add(this.labelNetserverMaximumUsers);
            this.tabPageServerInfo.Controls.Add(this.textBoxNetserverMaxUsers);
            this.tabPageServerInfo.Controls.Add(this.labelPlatformVersion);
            this.tabPageServerInfo.Controls.Add(this.textBoxPlatformVersion);
            this.tabPageServerInfo.Controls.Add(this.labelNetworkServerSoftwareType);
            this.tabPageServerInfo.Controls.Add(this.textBoxNetserverSoftwareType);
            this.tabPageServerInfo.Controls.Add(this.labelNetserverPlatformId);
            this.tabPageServerInfo.Controls.Add(this.textBoxNetserverPlatformId);
            this.tabPageServerInfo.Location = new System.Drawing.Point(4, 22);
            this.tabPageServerInfo.Name = "tabPageServerInfo";
            this.tabPageServerInfo.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageServerInfo.Size = new System.Drawing.Size(586, 289);
            this.tabPageServerInfo.TabIndex = 1;
            this.tabPageServerInfo.Text = "tabPage2";
            this.tabPageServerInfo.UseVisualStyleBackColor = true;
            // 
            // labelNetserverUptime
            // 
            this.labelNetserverUptime.Location = new System.Drawing.Point(8, 263);
            this.labelNetserverUptime.Name = "labelNetserverUptime";
            this.labelNetserverUptime.Size = new System.Drawing.Size(275, 23);
            this.labelNetserverUptime.TabIndex = 33;
            this.labelNetserverUptime.Text = "Uptime";
            this.labelNetserverUptime.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // labelNetserverDatetime
            // 
            this.labelNetserverDatetime.Location = new System.Drawing.Point(8, 237);
            this.labelNetserverDatetime.Name = "labelNetserverDatetime";
            this.labelNetserverDatetime.Size = new System.Drawing.Size(275, 23);
            this.labelNetserverDatetime.TabIndex = 33;
            this.labelNetserverDatetime.Text = "Server date time";
            this.labelNetserverDatetime.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // textBoxNetserverUptime
            // 
            this.textBoxNetserverUptime.Location = new System.Drawing.Point(289, 265);
            this.textBoxNetserverUptime.Name = "textBoxNetserverUptime";
            this.textBoxNetserverUptime.ReadOnly = true;
            this.textBoxNetserverUptime.Size = new System.Drawing.Size(288, 20);
            this.textBoxNetserverUptime.TabIndex = 34;
            // 
            // textBoxNetserverDatetime
            // 
            this.textBoxNetserverDatetime.Location = new System.Drawing.Point(289, 239);
            this.textBoxNetserverDatetime.Name = "textBoxNetserverDatetime";
            this.textBoxNetserverDatetime.ReadOnly = true;
            this.textBoxNetserverDatetime.Size = new System.Drawing.Size(288, 20);
            this.textBoxNetserverDatetime.TabIndex = 34;
            // 
            // labelNetserverFeatures
            // 
            this.labelNetserverFeatures.Location = new System.Drawing.Point(8, 211);
            this.labelNetserverFeatures.Name = "labelNetserverFeatures";
            this.labelNetserverFeatures.Size = new System.Drawing.Size(275, 23);
            this.labelNetserverFeatures.TabIndex = 31;
            this.labelNetserverFeatures.Text = "Features";
            this.labelNetserverFeatures.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // textBoxNetserverFeatures
            // 
            this.textBoxNetserverFeatures.Location = new System.Drawing.Point(289, 213);
            this.textBoxNetserverFeatures.Name = "textBoxNetserverFeatures";
            this.textBoxNetserverFeatures.ReadOnly = true;
            this.textBoxNetserverFeatures.Size = new System.Drawing.Size(288, 20);
            this.textBoxNetserverFeatures.TabIndex = 32;
            // 
            // labelNetserverUserPath
            // 
            this.labelNetserverUserPath.Location = new System.Drawing.Point(8, 185);
            this.labelNetserverUserPath.Name = "labelNetserverUserPath";
            this.labelNetserverUserPath.Size = new System.Drawing.Size(275, 23);
            this.labelNetserverUserPath.TabIndex = 29;
            this.labelNetserverUserPath.Text = "User path";
            this.labelNetserverUserPath.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // textBoxNetserverUserPath
            // 
            this.textBoxNetserverUserPath.Location = new System.Drawing.Point(289, 188);
            this.textBoxNetserverUserPath.Name = "textBoxNetserverUserPath";
            this.textBoxNetserverUserPath.ReadOnly = true;
            this.textBoxNetserverUserPath.Size = new System.Drawing.Size(288, 20);
            this.textBoxNetserverUserPath.TabIndex = 30;
            // 
            // labelNetserverUsersPerLicense
            // 
            this.labelNetserverUsersPerLicense.Location = new System.Drawing.Point(8, 159);
            this.labelNetserverUsersPerLicense.Name = "labelNetserverUsersPerLicense";
            this.labelNetserverUsersPerLicense.Size = new System.Drawing.Size(275, 23);
            this.labelNetserverUsersPerLicense.TabIndex = 27;
            this.labelNetserverUsersPerLicense.Text = "Users per license";
            this.labelNetserverUsersPerLicense.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // textBoxNetserverUsersPerLicense
            // 
            this.textBoxNetserverUsersPerLicense.Location = new System.Drawing.Point(289, 162);
            this.textBoxNetserverUsersPerLicense.Name = "textBoxNetserverUsersPerLicense";
            this.textBoxNetserverUsersPerLicense.ReadOnly = true;
            this.textBoxNetserverUsersPerLicense.Size = new System.Drawing.Size(288, 20);
            this.textBoxNetserverUsersPerLicense.TabIndex = 28;
            // 
            // labelNetserverAnnounceTime
            // 
            this.labelNetserverAnnounceTime.Location = new System.Drawing.Point(8, 133);
            this.labelNetserverAnnounceTime.Name = "labelNetserverAnnounceTime";
            this.labelNetserverAnnounceTime.Size = new System.Drawing.Size(275, 23);
            this.labelNetserverAnnounceTime.TabIndex = 25;
            this.labelNetserverAnnounceTime.Text = "Announce time (secs)";
            this.labelNetserverAnnounceTime.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // textBoxNetserverAnnounceTime
            // 
            this.textBoxNetserverAnnounceTime.Location = new System.Drawing.Point(289, 136);
            this.textBoxNetserverAnnounceTime.Name = "textBoxNetserverAnnounceTime";
            this.textBoxNetserverAnnounceTime.ReadOnly = true;
            this.textBoxNetserverAnnounceTime.Size = new System.Drawing.Size(288, 20);
            this.textBoxNetserverAnnounceTime.TabIndex = 26;
            // 
            // labelNetserverDisconnectTime
            // 
            this.labelNetserverDisconnectTime.Location = new System.Drawing.Point(8, 107);
            this.labelNetserverDisconnectTime.Name = "labelNetserverDisconnectTime";
            this.labelNetserverDisconnectTime.Size = new System.Drawing.Size(275, 23);
            this.labelNetserverDisconnectTime.TabIndex = 23;
            this.labelNetserverDisconnectTime.Text = "Disconnect time (min.)";
            this.labelNetserverDisconnectTime.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // textBoxNetserverDisconnectTime
            // 
            this.textBoxNetserverDisconnectTime.Location = new System.Drawing.Point(289, 110);
            this.textBoxNetserverDisconnectTime.Name = "textBoxNetserverDisconnectTime";
            this.textBoxNetserverDisconnectTime.ReadOnly = true;
            this.textBoxNetserverDisconnectTime.Size = new System.Drawing.Size(288, 20);
            this.textBoxNetserverDisconnectTime.TabIndex = 24;
            // 
            // labelNetserverMaximumUsers
            // 
            this.labelNetserverMaximumUsers.Location = new System.Drawing.Point(8, 81);
            this.labelNetserverMaximumUsers.Name = "labelNetserverMaximumUsers";
            this.labelNetserverMaximumUsers.Size = new System.Drawing.Size(275, 23);
            this.labelNetserverMaximumUsers.TabIndex = 21;
            this.labelNetserverMaximumUsers.Text = "Maximum users";
            this.labelNetserverMaximumUsers.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // textBoxNetserverMaxUsers
            // 
            this.textBoxNetserverMaxUsers.Location = new System.Drawing.Point(289, 84);
            this.textBoxNetserverMaxUsers.Name = "textBoxNetserverMaxUsers";
            this.textBoxNetserverMaxUsers.ReadOnly = true;
            this.textBoxNetserverMaxUsers.Size = new System.Drawing.Size(288, 20);
            this.textBoxNetserverMaxUsers.TabIndex = 22;
            // 
            // labelPlatformVersion
            // 
            this.labelPlatformVersion.Location = new System.Drawing.Point(8, 55);
            this.labelPlatformVersion.Name = "labelPlatformVersion";
            this.labelPlatformVersion.Size = new System.Drawing.Size(275, 23);
            this.labelPlatformVersion.TabIndex = 19;
            this.labelPlatformVersion.Text = "Platform version";
            this.labelPlatformVersion.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // textBoxPlatformVersion
            // 
            this.textBoxPlatformVersion.Location = new System.Drawing.Point(289, 58);
            this.textBoxPlatformVersion.Name = "textBoxPlatformVersion";
            this.textBoxPlatformVersion.ReadOnly = true;
            this.textBoxPlatformVersion.Size = new System.Drawing.Size(288, 20);
            this.textBoxPlatformVersion.TabIndex = 20;
            // 
            // labelNetworkServerSoftwareType
            // 
            this.labelNetworkServerSoftwareType.Location = new System.Drawing.Point(8, 29);
            this.labelNetworkServerSoftwareType.Name = "labelNetworkServerSoftwareType";
            this.labelNetworkServerSoftwareType.Size = new System.Drawing.Size(275, 23);
            this.labelNetworkServerSoftwareType.TabIndex = 17;
            this.labelNetworkServerSoftwareType.Text = "Software type";
            this.labelNetworkServerSoftwareType.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // textBoxNetserverSoftwareType
            // 
            this.textBoxNetserverSoftwareType.Location = new System.Drawing.Point(289, 32);
            this.textBoxNetserverSoftwareType.Name = "textBoxNetserverSoftwareType";
            this.textBoxNetserverSoftwareType.ReadOnly = true;
            this.textBoxNetserverSoftwareType.Size = new System.Drawing.Size(288, 20);
            this.textBoxNetserverSoftwareType.TabIndex = 18;
            // 
            // labelNetserverPlatformId
            // 
            this.labelNetserverPlatformId.Location = new System.Drawing.Point(8, 3);
            this.labelNetserverPlatformId.Name = "labelNetserverPlatformId";
            this.labelNetserverPlatformId.Size = new System.Drawing.Size(275, 23);
            this.labelNetserverPlatformId.TabIndex = 15;
            this.labelNetserverPlatformId.Text = "Platform";
            this.labelNetserverPlatformId.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // textBoxNetserverPlatformId
            // 
            this.textBoxNetserverPlatformId.Location = new System.Drawing.Point(289, 6);
            this.textBoxNetserverPlatformId.Name = "textBoxNetserverPlatformId";
            this.textBoxNetserverPlatformId.ReadOnly = true;
            this.textBoxNetserverPlatformId.Size = new System.Drawing.Size(288, 20);
            this.textBoxNetserverPlatformId.TabIndex = 16;
            // 
            // errorProvider1
            // 
            this.errorProvider1.BlinkStyle = System.Windows.Forms.ErrorBlinkStyle.NeverBlink;
            this.errorProvider1.ContainerControl = this;
            // 
            // tabPageTransportInfo
            // 
            this.tabPageTransportInfo.Controls.Add(this.listViewTransports);
            this.tabPageTransportInfo.Location = new System.Drawing.Point(4, 22);
            this.tabPageTransportInfo.Name = "tabPageTransportInfo";
            this.tabPageTransportInfo.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageTransportInfo.Size = new System.Drawing.Size(586, 289);
            this.tabPageTransportInfo.TabIndex = 2;
            this.tabPageTransportInfo.Text = "tabPage1";
            this.tabPageTransportInfo.UseVisualStyleBackColor = true;
            // 
            // listViewTransports
            // 
            this.listViewTransports.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeaderTransportName,
            this.columnHeaderTransportAddress,
            this.columnHeaderNetworkAddress,
            this.columnHeaderDomain,
            this.columnHeaderNumerOfVcs});
            this.listViewTransports.Dock = System.Windows.Forms.DockStyle.Right;
            this.listViewTransports.FullRowSelect = true;
            this.listViewTransports.GridLines = true;
            this.listViewTransports.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.Nonclickable;
            this.listViewTransports.Location = new System.Drawing.Point(27, 3);
            this.listViewTransports.MultiSelect = false;
            this.listViewTransports.Name = "listViewTransports";
            this.listViewTransports.ShowGroups = false;
            this.listViewTransports.Size = new System.Drawing.Size(556, 283);
            this.listViewTransports.TabIndex = 36;
            this.listViewTransports.UseCompatibleStateImageBehavior = false;
            this.listViewTransports.View = System.Windows.Forms.View.Details;
            // 
            // columnHeaderTransportName
            // 
            this.columnHeaderTransportName.Text = "Transport name";
            this.columnHeaderTransportName.Width = 238;
            // 
            // columnHeaderTransportAddress
            // 
            this.columnHeaderTransportAddress.Text = "Transport address";
            this.columnHeaderTransportAddress.Width = 108;
            // 
            // columnHeaderNetworkAddress
            // 
            this.columnHeaderNetworkAddress.Text = "Network address";
            this.columnHeaderNetworkAddress.Width = 102;
            // 
            // columnHeaderDomain
            // 
            this.columnHeaderDomain.Text = "Domain";
            this.columnHeaderDomain.Width = 69;
            // 
            // columnHeaderNumerOfVcs
            // 
            this.columnHeaderNumerOfVcs.Text = "Number of connected clients";
            this.columnHeaderNumerOfVcs.Width = 59;
            // 
            // tabPageSessionInfo
            // 
            this.tabPageSessionInfo.Controls.Add(this.listViewSessions);
            this.tabPageSessionInfo.Location = new System.Drawing.Point(4, 22);
            this.tabPageSessionInfo.Name = "tabPageSessionInfo";
            this.tabPageSessionInfo.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageSessionInfo.Size = new System.Drawing.Size(586, 289);
            this.tabPageSessionInfo.TabIndex = 3;
            this.tabPageSessionInfo.Text = "tabPage3";
            this.tabPageSessionInfo.UseVisualStyleBackColor = true;
            // 
            // listViewSessions
            // 
            this.listViewSessions.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeaderComputerName,
            this.columnHeaderUserName,
            this.columnHeaderNumberOfOpenFiles,
            this.columnHeaderActivetime,
            this.columnHeaderIdletime,
            this.columnHeaderEstablishtype,
            this.columnHeaderClientType,
            this.columnHeaderSessionTransportName});
            this.listViewSessions.Dock = System.Windows.Forms.DockStyle.Right;
            this.listViewSessions.FullRowSelect = true;
            this.listViewSessions.GridLines = true;
            this.listViewSessions.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.Nonclickable;
            this.listViewSessions.Location = new System.Drawing.Point(27, 3);
            this.listViewSessions.MultiSelect = false;
            this.listViewSessions.Name = "listViewSessions";
            this.listViewSessions.ShowGroups = false;
            this.listViewSessions.Size = new System.Drawing.Size(556, 283);
            this.listViewSessions.TabIndex = 0;
            this.listViewSessions.UseCompatibleStateImageBehavior = false;
            this.listViewSessions.View = System.Windows.Forms.View.Details;
            // 
            // columnHeaderComputerName
            // 
            this.columnHeaderComputerName.Text = "Computer name";
            this.columnHeaderComputerName.Width = 75;
            // 
            // columnHeaderUserName
            // 
            this.columnHeaderUserName.Text = "User name";
            this.columnHeaderUserName.Width = 72;
            // 
            // columnHeaderNumberOfOpenFiles
            // 
            this.columnHeaderNumberOfOpenFiles.Text = "Open files";
            // 
            // columnHeaderActivetime
            // 
            this.columnHeaderActivetime.Text = "Active time";
            this.columnHeaderActivetime.Width = 65;
            // 
            // columnHeaderIdletime
            // 
            this.columnHeaderIdletime.Text = "Idle time";
            // 
            // columnHeaderEstablishtype
            // 
            this.columnHeaderEstablishtype.Text = "Connection type";
            this.columnHeaderEstablishtype.Width = 65;
            // 
            // columnHeaderClientType
            // 
            this.columnHeaderClientType.Text = "Client type";
            // 
            // columnHeaderSessionTransportName
            // 
            this.columnHeaderSessionTransportName.Text = "Transport name";
            this.columnHeaderSessionTransportName.Width = 86;
            // 
            // tabPageFilesInfo
            // 
            this.tabPageFilesInfo.Controls.Add(this.listViewFiles);
            this.tabPageFilesInfo.Location = new System.Drawing.Point(4, 22);
            this.tabPageFilesInfo.Name = "tabPageFilesInfo";
            this.tabPageFilesInfo.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageFilesInfo.Size = new System.Drawing.Size(586, 289);
            this.tabPageFilesInfo.TabIndex = 4;
            this.tabPageFilesInfo.Text = "tabPage4";
            this.tabPageFilesInfo.UseVisualStyleBackColor = true;
            // 
            // listViewFiles
            // 
            this.listViewFiles.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeaderFileId,
            this.columnHeaderFilePermissions,
            this.columnHeaderNumberOfLocks,
            this.columnHeaderFilePath,
            this.columnHeaderFileUserName});
            this.listViewFiles.Dock = System.Windows.Forms.DockStyle.Right;
            this.listViewFiles.FullRowSelect = true;
            this.listViewFiles.GridLines = true;
            this.listViewFiles.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.Nonclickable;
            this.listViewFiles.Location = new System.Drawing.Point(27, 3);
            this.listViewFiles.MultiSelect = false;
            this.listViewFiles.Name = "listViewFiles";
            this.listViewFiles.ShowGroups = false;
            this.listViewFiles.Size = new System.Drawing.Size(556, 283);
            this.listViewFiles.TabIndex = 37;
            this.listViewFiles.UseCompatibleStateImageBehavior = false;
            this.listViewFiles.View = System.Windows.Forms.View.Details;
            // 
            // columnHeaderFileId
            // 
            this.columnHeaderFileId.Text = "ID";
            this.columnHeaderFileId.Width = 58;
            // 
            // columnHeaderFilePermissions
            // 
            this.columnHeaderFilePermissions.Text = "Permissions";
            this.columnHeaderFilePermissions.Width = 142;
            // 
            // columnHeaderNumberOfLocks
            // 
            this.columnHeaderNumberOfLocks.Text = "Locks";
            this.columnHeaderNumberOfLocks.Width = 58;
            // 
            // columnHeaderFilePath
            // 
            this.columnHeaderFilePath.Text = "Path";
            this.columnHeaderFilePath.Width = 190;
            // 
            // columnHeaderFileUserName
            // 
            this.columnHeaderFileUserName.Text = "User name";
            this.columnHeaderFileUserName.Width = 102;
            // 
            // tabPageShareInfo
            // 
            this.tabPageShareInfo.Controls.Add(this.labelSharePath);
            this.tabPageShareInfo.Controls.Add(this.textBoxSharePath);
            this.tabPageShareInfo.Controls.Add(this.labelShareCurrentUses);
            this.tabPageShareInfo.Controls.Add(this.textBoxShareCurrentUses);
            this.tabPageShareInfo.Controls.Add(this.labelShareMaxUses);
            this.tabPageShareInfo.Controls.Add(this.textBoxShareMaxuses);
            this.tabPageShareInfo.Controls.Add(this.labelSharePermissions);
            this.tabPageShareInfo.Controls.Add(this.textBoxSharePermissions);
            this.tabPageShareInfo.Controls.Add(this.labelShareComment);
            this.tabPageShareInfo.Controls.Add(this.textBoxShareComment);
            this.tabPageShareInfo.Controls.Add(this.labelShareType);
            this.tabPageShareInfo.Controls.Add(this.textBoxShareType);
            this.tabPageShareInfo.Location = new System.Drawing.Point(4, 22);
            this.tabPageShareInfo.Name = "tabPageShareInfo";
            this.tabPageShareInfo.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageShareInfo.Size = new System.Drawing.Size(586, 289);
            this.tabPageShareInfo.TabIndex = 5;
            this.tabPageShareInfo.Text = "tabPage1";
            this.tabPageShareInfo.UseVisualStyleBackColor = true;
            // 
            // labelShareType
            // 
            this.labelShareType.Location = new System.Drawing.Point(8, 3);
            this.labelShareType.Name = "labelShareType";
            this.labelShareType.Size = new System.Drawing.Size(275, 23);
            this.labelShareType.TabIndex = 5;
            this.labelShareType.Text = "Share type";
            this.labelShareType.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // textBoxShareType
            // 
            this.textBoxShareType.Location = new System.Drawing.Point(289, 6);
            this.textBoxShareType.Name = "textBoxShareType";
            this.textBoxShareType.ReadOnly = true;
            this.textBoxShareType.Size = new System.Drawing.Size(288, 20);
            this.textBoxShareType.TabIndex = 6;
            // 
            // labelShareComment
            // 
            this.labelShareComment.Location = new System.Drawing.Point(8, 29);
            this.labelShareComment.Name = "labelShareComment";
            this.labelShareComment.Size = new System.Drawing.Size(275, 23);
            this.labelShareComment.TabIndex = 7;
            this.labelShareComment.Text = "Share remark";
            this.labelShareComment.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // textBoxShareComment
            // 
            this.textBoxShareComment.Location = new System.Drawing.Point(289, 32);
            this.textBoxShareComment.Name = "textBoxShareComment";
            this.textBoxShareComment.ReadOnly = true;
            this.textBoxShareComment.Size = new System.Drawing.Size(288, 20);
            this.textBoxShareComment.TabIndex = 8;
            // 
            // labelSharePermissions
            // 
            this.labelSharePermissions.Location = new System.Drawing.Point(8, 55);
            this.labelSharePermissions.Name = "labelSharePermissions";
            this.labelSharePermissions.Size = new System.Drawing.Size(275, 23);
            this.labelSharePermissions.TabIndex = 9;
            this.labelSharePermissions.Text = "permissions";
            this.labelSharePermissions.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // textBoxSharePermissions
            // 
            this.textBoxSharePermissions.Location = new System.Drawing.Point(289, 58);
            this.textBoxSharePermissions.Name = "textBoxSharePermissions";
            this.textBoxSharePermissions.ReadOnly = true;
            this.textBoxSharePermissions.Size = new System.Drawing.Size(288, 20);
            this.textBoxSharePermissions.TabIndex = 10;
            // 
            // labelShareMaxUses
            // 
            this.labelShareMaxUses.Location = new System.Drawing.Point(8, 81);
            this.labelShareMaxUses.Name = "labelShareMaxUses";
            this.labelShareMaxUses.Size = new System.Drawing.Size(275, 23);
            this.labelShareMaxUses.TabIndex = 11;
            this.labelShareMaxUses.Text = "max users";
            this.labelShareMaxUses.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // textBoxShareMaxuses
            // 
            this.textBoxShareMaxuses.Location = new System.Drawing.Point(289, 84);
            this.textBoxShareMaxuses.Name = "textBoxShareMaxuses";
            this.textBoxShareMaxuses.ReadOnly = true;
            this.textBoxShareMaxuses.Size = new System.Drawing.Size(288, 20);
            this.textBoxShareMaxuses.TabIndex = 12;
            // 
            // labelShareCurrentUses
            // 
            this.labelShareCurrentUses.Location = new System.Drawing.Point(8, 107);
            this.labelShareCurrentUses.Name = "labelShareCurrentUses";
            this.labelShareCurrentUses.Size = new System.Drawing.Size(275, 23);
            this.labelShareCurrentUses.TabIndex = 13;
            this.labelShareCurrentUses.Text = "uses";
            this.labelShareCurrentUses.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // textBoxShareCurrentUses
            // 
            this.textBoxShareCurrentUses.Location = new System.Drawing.Point(289, 110);
            this.textBoxShareCurrentUses.Name = "textBoxShareCurrentUses";
            this.textBoxShareCurrentUses.ReadOnly = true;
            this.textBoxShareCurrentUses.Size = new System.Drawing.Size(288, 20);
            this.textBoxShareCurrentUses.TabIndex = 14;
            // 
            // labelSharePath
            // 
            this.labelSharePath.Location = new System.Drawing.Point(8, 133);
            this.labelSharePath.Name = "labelSharePath";
            this.labelSharePath.Size = new System.Drawing.Size(275, 23);
            this.labelSharePath.TabIndex = 15;
            this.labelSharePath.Text = "local path";
            this.labelSharePath.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // textBoxSharePath
            // 
            this.textBoxSharePath.Location = new System.Drawing.Point(289, 136);
            this.textBoxSharePath.Name = "textBoxSharePath";
            this.textBoxSharePath.ReadOnly = true;
            this.textBoxSharePath.Size = new System.Drawing.Size(288, 20);
            this.textBoxSharePath.TabIndex = 16;
            // 
            // NetInfoDialog
            // 
            this.AcceptButton = this.buttonClose;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.buttonClose;
            this.ClientSize = new System.Drawing.Size(594, 350);
            this.Controls.Add(this.tabControl1);
            this.Controls.Add(this.panelButton);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.Name = "NetInfoDialog";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "ServerInfoDialog";
            this.panelButton.ResumeLayout(false);
            this.tabControl1.ResumeLayout(false);
            this.tabPageResourceInfo.ResumeLayout(false);
            this.tabPageResourceInfo.PerformLayout();
            this.tabPageServerInfo.ResumeLayout(false);
            this.tabPageServerInfo.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider1)).EndInit();
            this.tabPageTransportInfo.ResumeLayout(false);
            this.tabPageSessionInfo.ResumeLayout(false);
            this.tabPageFilesInfo.ResumeLayout(false);
            this.tabPageShareInfo.ResumeLayout(false);
            this.tabPageShareInfo.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panelButton;
        private System.Windows.Forms.Button buttonClose;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPageResourceInfo;
        private System.Windows.Forms.TabPage tabPageServerInfo;
        private System.Windows.Forms.Label labelRemoteName;
        private System.Windows.Forms.TextBox textBoxRemoteName;
        private System.Windows.Forms.Label labelDisplayType;
        private System.Windows.Forms.TextBox textBoxDisplayType;
        private System.Windows.Forms.Label labelResourceType;
        private System.Windows.Forms.TextBox textBoxResourceType;
        private System.Windows.Forms.Label label1ResourceComment;
        private System.Windows.Forms.TextBox textBoxResourceComment;
        private System.Windows.Forms.Label labelProviderName;
        private System.Windows.Forms.TextBox textBoxProviderName;
        private System.Windows.Forms.Label labelNetworkProviderType;
        private System.Windows.Forms.TextBox textBoxNetworkProviderType;
        private System.Windows.Forms.Label labelNetworkProviderVersion;
        private System.Windows.Forms.TextBox textBoxNetworkProviderVersion;
        private System.Windows.Forms.Label labelNetserverPlatformId;
        private System.Windows.Forms.TextBox textBoxNetserverPlatformId;
        private System.Windows.Forms.Label labelNetworkServerSoftwareType;
        private System.Windows.Forms.TextBox textBoxNetserverSoftwareType;
        private System.Windows.Forms.Label labelPlatformVersion;
        private System.Windows.Forms.TextBox textBoxPlatformVersion;
        private System.Windows.Forms.Label labelNetserverMaximumUsers;
        private System.Windows.Forms.TextBox textBoxNetserverMaxUsers;
        private System.Windows.Forms.Label labelNetserverDisconnectTime;
        private System.Windows.Forms.TextBox textBoxNetserverDisconnectTime;
        private System.Windows.Forms.Label labelNetserverAnnounceTime;
        private System.Windows.Forms.TextBox textBoxNetserverAnnounceTime;
        private System.Windows.Forms.Label labelNetserverUsersPerLicense;
        private System.Windows.Forms.TextBox textBoxNetserverUsersPerLicense;
        private System.Windows.Forms.Label labelNetserverUserPath;
        private System.Windows.Forms.TextBox textBoxNetserverUserPath;
        private System.Windows.Forms.Label labelNetserverFeatures;
        private System.Windows.Forms.TextBox textBoxNetserverFeatures;
        private System.Windows.Forms.Label labelNetserverDatetime;
        private System.Windows.Forms.TextBox textBoxNetserverDatetime;
        private System.Windows.Forms.Label labelNetserverUptime;
        private System.Windows.Forms.TextBox textBoxNetserverUptime;
        private System.Windows.Forms.ErrorProvider errorProvider1;
        private System.Windows.Forms.TabPage tabPageTransportInfo;
        private System.Windows.Forms.ListView listViewTransports;
        private System.Windows.Forms.ColumnHeader columnHeaderTransportName;
        private System.Windows.Forms.ColumnHeader columnHeaderTransportAddress;
        private System.Windows.Forms.ColumnHeader columnHeaderNetworkAddress;
        private System.Windows.Forms.ColumnHeader columnHeaderDomain;
        private System.Windows.Forms.ColumnHeader columnHeaderNumerOfVcs;
        private System.Windows.Forms.TabPage tabPageSessionInfo;
        private System.Windows.Forms.ListView listViewSessions;
        private System.Windows.Forms.ColumnHeader columnHeaderComputerName;
        private System.Windows.Forms.ColumnHeader columnHeaderUserName;
        private System.Windows.Forms.ColumnHeader columnHeaderNumberOfOpenFiles;
        private System.Windows.Forms.ColumnHeader columnHeaderActivetime;
        private System.Windows.Forms.ColumnHeader columnHeaderIdletime;
        private System.Windows.Forms.ColumnHeader columnHeaderEstablishtype;
        private System.Windows.Forms.ColumnHeader columnHeaderClientType;
        private System.Windows.Forms.ColumnHeader columnHeaderSessionTransportName;
        private System.Windows.Forms.TabPage tabPageFilesInfo;
        private System.Windows.Forms.ListView listViewFiles;
        private System.Windows.Forms.ColumnHeader columnHeaderFileId;
        private System.Windows.Forms.ColumnHeader columnHeaderFilePermissions;
        private System.Windows.Forms.ColumnHeader columnHeaderNumberOfLocks;
        private System.Windows.Forms.ColumnHeader columnHeaderFilePath;
        private System.Windows.Forms.ColumnHeader columnHeaderFileUserName;
        private System.Windows.Forms.TabPage tabPageShareInfo;
        private System.Windows.Forms.Label labelShareType;
        private System.Windows.Forms.TextBox textBoxShareType;
        private System.Windows.Forms.Label labelShareComment;
        private System.Windows.Forms.TextBox textBoxShareComment;
        private System.Windows.Forms.Label labelSharePermissions;
        private System.Windows.Forms.TextBox textBoxSharePermissions;
        private System.Windows.Forms.Label labelShareMaxUses;
        private System.Windows.Forms.TextBox textBoxShareMaxuses;
        private System.Windows.Forms.Label labelShareCurrentUses;
        private System.Windows.Forms.TextBox textBoxShareCurrentUses;
        private System.Windows.Forms.Label labelSharePath;
        private System.Windows.Forms.TextBox textBoxSharePath;
    }
}