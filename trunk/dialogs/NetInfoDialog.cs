using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using netCommander.WNet;
using netCommander.NetApi;

namespace netCommander
{
    public partial class NetInfoDialog : Form
    {
        public NetInfoDialog()
        {
            InitializeComponent();
            set_lang();
        }

        private void set_lang()
        {
            if (LicenseManager.UsageMode == LicenseUsageMode.Designtime)
            {
                return;
            }

            tabPageFilesInfo.Text = Options.GetLiteral(Options.LANG_NETWROK_OPEN_FILES_INFO);
            tabPageResourceInfo.Text = Options.GetLiteral(Options.LANG_RESOURCE_INFO);
            tabPageServerInfo.Text = Options.GetLiteral(Options.LANG_SERVER_INFO);
            tabPageSessionInfo.Text = Options.GetLiteral(Options.LANG_SESSION_INFO);
            tabPageTransportInfo.Text = Options.GetLiteral(Options.LANG_TRANSPORT_INFO);
            label1ResourceComment.Text = Options.GetLiteral(Options.LANG_RESOURCE_COMMENT);
            labelDisplayType.Text = Options.GetLiteral(Options.LANG_RESOURCE_DISPLAY_TYPE);
            labelNetserverAnnounceTime.Text = Options.GetLiteral(Options.LANG_SERVER_ANNOUNCE_TIME);
            labelNetserverDatetime.Text = Options.GetLiteral(Options.LANG_SERVER_DATE_TIME);
            labelNetserverDisconnectTime.Text = Options.GetLiteral(Options.LANG_SERVER_DISC_TIME);
            labelNetserverFeatures.Text = Options.GetLiteral(Options.LANG_SERVER_FEATURES);
            labelNetserverMaximumUsers.Text = Options.GetLiteral(Options.LANG_SERVER_MAX_USERS);
            labelNetserverPlatformId.Text = Options.GetLiteral(Options.LANG_SERVER_PLATFORM_ID);
            labelNetserverUptime.Text = Options.GetLiteral(Options.LANG_SERVER_UPTIME_AFTER_RESET);
            labelNetserverUserPath.Text = Options.GetLiteral(Options.LANG_SERVER_USER_PATH);
            labelNetserverUsersPerLicense.Text = Options.GetLiteral(Options.LANG_SERVER_USERS_PER_LICENSE);
            labelNetworkProviderType.Text = Options.GetLiteral(Options.LANG_NETWORK_PROVIDER_TYPE);
            labelNetworkProviderVersion.Text = Options.GetLiteral(Options.LANG_VERSION);
            labelNetworkServerSoftwareType.Text = Options.GetLiteral(Options.LANG_SERVER_SOFTWARE_TYPE);
            labelPlatformVersion.Text = Options.GetLiteral(Options.LANG_VERSION);
            labelProviderName.Text = Options.GetLiteral(Options.LANG_NETWORK_PROVIDER_NAME);
            labelRemoteName.Text = Options.GetLiteral(Options.LANG_RESOURCE_REMOTE_NAME);
            labelResourceType.Text = Options.GetLiteral(Options.LANG_RESOURCE_TYPE);
            columnHeaderActivetime.Text = Options.GetLiteral(Options.LANG_ACTIVE_TIME);
            columnHeaderClientType.Text = Options.GetLiteral(Options.LANG_SESSION_CLIENT_TYPE);
            columnHeaderComputerName.Text = Options.GetLiteral(Options.LANG_COMPUTER_NAME);
            columnHeaderDomain.Text = Options.GetLiteral(Options.LANG_DOMAIN);
            columnHeaderEstablishtype.Text = Options.GetLiteral(Options.LANG_SESSION_ESTABLISH_TYPE);
            columnHeaderFileId.Text = Options.GetLiteral(Options.LANG_NETWORK_FILE_ID);
            columnHeaderFilePath.Text = Options.GetLiteral(Options.LANG_PATH);
            columnHeaderFilePermissions.Text = Options.GetLiteral(Options.LANG_PERMISSIONS);
            columnHeaderFileUserName.Text = Options.GetLiteral(Options.LANG_USER_NAME);
            columnHeaderIdletime.Text = Options.GetLiteral(Options.LANG_IDLE_TIME);
            columnHeaderNetworkAddress.Text = Options.GetLiteral(Options.LANG_TRANSPORT_NETWORK_ADDRESS);
            columnHeaderNumberOfLocks.Text = Options.GetLiteral(Options.LANG_LOCKS);
            columnHeaderNumberOfOpenFiles.Text = Options.GetLiteral(Options.LANG_NUMBER_OF_OPEN_FILES);
            columnHeaderNumerOfVcs.Text = Options.GetLiteral(Options.LANG_NUMBER_OF_CLIENTS);
            columnHeaderSessionTransportName.Text = Options.GetLiteral(Options.LANG_SESSION_TRANSPORT_NAME);
            columnHeaderTransportAddress.Text = Options.GetLiteral(Options.LANG_TRANSPORT_ADDRESS);
            columnHeaderTransportName.Text = Options.GetLiteral(Options.LANG_NAME);
            columnHeaderUserName.Text = Options.GetLiteral(Options.LANG_USER_NAME);

            tabPageShareInfo.Text = Options.GetLiteral(Options.LANG_SHARE_INFO);
            labelShareComment.Text = Options.GetLiteral(Options.LANG_RESOURCE_COMMENT);
            labelShareCurrentUses.Text = Options.GetLiteral(Options.LANG_CURRENT_CONNECTIONS);
            labelShareMaxUses.Text = Options.GetLiteral(Options.LANG_MAX_CONNECTIONS);
            labelSharePath.Text = Options.GetLiteral(Options.LANG_SHARE_PATH);
            labelSharePermissions.Text = Options.GetLiteral(Options.LANG_PERMISSIONS);
            labelShareType.Text = Options.GetLiteral(Options.LANG_TYPE);
        }

        private NETRESOURCE resource_intern;

        public void FillInfo(NETRESOURCE resource)
        {
            try
            {
                resource_intern = WinApiWNETwrapper.GetResourceInfo(resource.lpRemoteName);

                Text = resource_intern.lpRemoteName;
                textBoxRemoteName.Text = resource_intern.lpRemoteName;
                textBoxDisplayType.Text = resource_intern.dwDisplayType.ToString();
                textBoxResourceType.Text = resource_intern.dwType.ToString();
                textBoxResourceComment.Text = resource_intern.lpComment;
                textBoxProviderName.Text = resource_intern.lpProvider;
            }
            catch (Exception ex)
            {
                errorProvider1.SetIconAlignment(textBoxRemoteName, ErrorIconAlignment.MiddleLeft);
                errorProvider1.SetError(textBoxRemoteName, ex.Message);
                //Messages.ShowException(ex);
            }

            NETINFOSTRUCT netinfo = new NETINFOSTRUCT();
            try
            {
                netinfo = WinApiWNETwrapper.GetNetworkInfo(resource_intern.lpProvider);

                textBoxNetworkProviderType.Text = netinfo.wNetType.ToString();
                textBoxNetworkProviderVersion.Text = string.Format("0x{0:X}", netinfo.dwProviderVersion);
            }
            catch (Exception ex)
            {
                errorProvider1.SetIconAlignment(textBoxNetworkProviderType, ErrorIconAlignment.MiddleLeft);
                errorProvider1.SetError(textBoxNetworkProviderType, ex.Message);
            }

            if ((resource_intern.dwDisplayType == ResourceDisplayType.SERVER) && (netinfo.wNetType == NetworkType.LANMAN))
            {
                fill_netserver_page();
                tabControl1.TabPages.Remove(tabPageShareInfo);
            }
            else if ((resource_intern.dwDisplayType == ResourceDisplayType.SHARE) || (resource_intern.dwDisplayType == ResourceDisplayType.SHAREADMIN))
            {
                fill_share_page();
                tabControl1.TabPages.Remove(tabPageFilesInfo);
                tabControl1.TabPages.Remove(tabPageServerInfo);
                tabControl1.TabPages.Remove(tabPageSessionInfo);
                tabControl1.TabPages.Remove(tabPageTransportInfo);
            }
            else
            {
                tabControl1.TabPages.Remove(tabPageShareInfo);
                tabControl1.TabPages.Remove(tabPageFilesInfo);
                tabControl1.TabPages.Remove(tabPageServerInfo);
                tabControl1.TabPages.Remove(tabPageSessionInfo);
                tabControl1.TabPages.Remove(tabPageTransportInfo);
            }

        }

        private void fill_netserver_page()
        {
            try
            {
                SERVER_INFO_102 serv_info = WinApiNETwrapper.GetServerInfo_102(resource_intern.lpRemoteName);
                textBoxNetserverPlatformId.Text = serv_info.sv102_platform_id.ToString();
                textBoxNetserverSoftwareType.Text = IOhelper.NetserverTypeToString(serv_info.sv102_type);
                textBoxPlatformVersion.Text = string.Format("{0}.{1}", serv_info.GetVersionMajor(), serv_info.sv102_version_minor);
                textBoxNetserverMaxUsers.Text = serv_info.sv102_users.ToString();
                textBoxNetserverDisconnectTime.Text = serv_info.sv102_disc.ToString();
                textBoxNetserverAnnounceTime.Text = serv_info.sv102_announce.ToString();
                textBoxNetserverUsersPerLicense.Text = serv_info.sv102_licenses.ToString();
                textBoxNetserverUserPath.Text = serv_info.sv102_userpath;
            }
            catch (Exception ex)
            {
                errorProvider1.SetIconAlignment(textBoxNetserverPlatformId, ErrorIconAlignment.MiddleLeft);
                errorProvider1.SetError(textBoxNetserverPlatformId, ex.Message);
            }

            try
            {
                NetRemoteComputerSupportsFeatures features = WinApiNETwrapper.GetComputerSupports(resource_intern.lpRemoteName);
                textBoxNetserverFeatures.Text = features.ToString();
            }
            catch (Exception ex)
            {
                errorProvider1.SetIconAlignment(textBoxNetserverFeatures, ErrorIconAlignment.MiddleLeft);
                errorProvider1.SetError(textBoxNetserverFeatures, ex.Message);
            }

            try
            {
                TIME_OF_DAY_INFO dt = WinApiNETwrapper.GetServerTime(resource_intern.lpRemoteName);
                textBoxNetserverDatetime.Text = string.Format("{0} {1},{2}", dt.GetCurrentDatetime().ToLongDateString(), dt.GetCurrentDatetime().ToLongTimeString(), dt.GetCurrentDatetime().Millisecond);
                textBoxNetserverUptime.Text = dt.GetUptime().ToString();
            }
            catch (Exception ex)
            {
                errorProvider1.SetIconAlignment(textBoxNetserverDatetime, ErrorIconAlignment.MiddleLeft);
                errorProvider1.SetError(textBoxNetserverDatetime, ex.Message);
            }

            //NET_DISPLAY_GROUP[] groups = WinApiNETwrapper.QueryDisplayInfoGroup(resource_intern.lpRemoteName);
            //NET_DISPLAY_MACHINE[] machines = WinApiNETwrapper.QueryDisplayInfoMachine(resource_intern.lpRemoteName);
            //NET_DISPLAY_USER[] users = WinApiNETwrapper.QueryDisplayInfoUser(resource_intern.lpRemoteName);

            try
            {
                SERVER_TRANSPORT_INFO_1[] transports = WinApiNETwrapper.ServerTransportEnum_1(resource_intern.lpRemoteName);
                for (int i = 0; i < transports.Length; i++)
                {
                    ListViewItem lvi = new ListViewItem();
                    lvi.Text = transports[i].svti1_transportname;
                    lvi.SubItems.Add(transports[i].TransportAddress);
                    lvi.SubItems.Add(transports[i].svti1_networkaddress);
                    lvi.SubItems.Add(transports[i].svti1_domain);
                    lvi.SubItems.Add(transports[i].svti1_numberofvcs.ToString());
                    listViewTransports.Items.Add(lvi);
                }
                listViewTransports.Dock = DockStyle.Fill;
            }
            catch (Exception ex)
            {
                errorProvider1.SetIconAlignment(listViewTransports, ErrorIconAlignment.MiddleLeft);
                errorProvider1.SetError(listViewTransports, ex.Message);

            }

            /* net sessions */
            errorProvider1.SetIconAlignment(listViewSessions, ErrorIconAlignment.MiddleLeft);
            Array sessions = null;
            NetSessionEnumLevel session_level = NetSessionEnumLevel.INFO_502;
            try
            {
                //try level 502
                sessions = WinApiNETwrapper.NetSessionEnum(resource_intern.lpRemoteName, null, null, session_level);
            }
            catch (Exception)
            {
                //if exception try level 10
                session_level = NetSessionEnumLevel.INFO_10;
                try
                {
                    sessions = WinApiNETwrapper.NetSessionEnum(resource_intern.lpRemoteName, null, null, session_level);
                }
                catch (Exception ex_10)
                {
                    errorProvider1.SetError(listViewSessions, ex_10.Message);
                }
            }
            if (sessions != null)
            {
                listViewSessions.Dock = DockStyle.Fill;
                for (int i = 0; i < sessions.Length; i++)
                {
                    ListViewItem lvi = new ListViewItem();
                    switch (session_level)
                    {
                        case NetSessionEnumLevel.INFO_502:
                            SESSION_INFO_502 info_502 = (SESSION_INFO_502)sessions.GetValue(i);
                            lvi.Text = info_502.sesi502_cname;
                            lvi.SubItems.Add(info_502.sesi502_username);
                            lvi.SubItems.Add(info_502.sesi502_num_opens.ToString());
                            lvi.SubItems.Add(info_502.TimeActive.ToString());
                            lvi.SubItems.Add(info_502.TimeIdle.ToString());
                            lvi.SubItems.Add(info_502.sesi502_user_flags.ToString());
                            lvi.SubItems.Add(info_502.sesi502_cltype_name);
                            lvi.SubItems.Add(info_502.sesi502_transport);
                            break;
                        case NetSessionEnumLevel.INFO_10:
                            SESSION_INFO_10 info_10 = (SESSION_INFO_10)sessions.GetValue(i);
                            lvi.Text = info_10.sesi10_cname;
                            lvi.SubItems.Add(string.Empty);
                            lvi.SubItems.Add(string.Empty);
                            lvi.SubItems.Add(info_10.TimeActive.ToString());
                            lvi.SubItems.Add(info_10.TimeIdle.ToString());
                            lvi.SubItems.Add(string.Empty);
                            lvi.SubItems.Add(string.Empty);
                            lvi.SubItems.Add(string.Empty);
                            break;
                    }
                    listViewSessions.Items.Add(lvi);
                }
            }
            /* end of net sessions */

            /* open files */
            try
            {
                Array files = WinApiNETwrapper.NetFileEnum(resource_intern.lpRemoteName, null, null, NetFileEnumLevel.INFO_3);
                listViewFiles.Dock = DockStyle.Fill;
                for (int i = 0; i < files.Length; i++)
                {
                    FILE_INFO_3 f_info = (FILE_INFO_3)files.GetValue(i);
                    ListViewItem lvi = new ListViewItem();
                    lvi.Text = string.Format("0x{0:X}", f_info.fi3_id);
                    lvi.SubItems.Add(f_info.fi3_permission.ToString());
                    lvi.SubItems.Add(f_info.fi3_num_locks.ToString());
                    lvi.SubItems.Add(f_info.fi3_pathname);
                    lvi.SubItems.Add(f_info.fi3_username);
                    listViewFiles.Items.Add(lvi);
                }
            }
            catch (Exception ex_files)
            {
                errorProvider1.SetIconAlignment(listViewFiles, ErrorIconAlignment.MiddleLeft);
                errorProvider1.SetError(listViewFiles, ex_files.Message);
            }
            /* end of open files */

        }

        private void fill_share_page()
        {
            try
            {
                string[] share_splitted = resource_intern.lpRemoteName.Split
                    (new char[] { System.IO.Path.DirectorySeparatorChar }, StringSplitOptions.RemoveEmptyEntries);
                SHARE_INFO_2 share_info = WinApiNETwrapper.GetShareInfo_2(share_splitted[0], share_splitted[1]);

                textBoxShareType.Text = share_info.shi2_type.ToString();
                textBoxShareComment.Text = share_info.shi2_remark;
                textBoxSharePermissions.Text = share_info.shi2_permissions.ToString();
                textBoxShareMaxuses.Text = share_info.shi2_max_uses.ToString();
                textBoxShareCurrentUses.Text = share_info.shi2_current_uses.ToString();
                textBoxSharePath.Text = share_info.shi2_path;
            }
            catch (Exception ex)
            {
                errorProvider1.SetIconAlignment(textBoxShareType, ErrorIconAlignment.MiddleLeft);
                errorProvider1.SetError(textBoxShareType, ex.Message);
            }
        }
    }
}
