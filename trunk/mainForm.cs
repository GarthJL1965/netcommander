using System;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using netCommander.winControls;
using netCommander.WNet;
using System.Security.Permissions;

namespace netCommander
{
    public partial class mainForm : Form
    {
        public mainForm()
        {
            InitializeComponent();
            
            set_lang();
            init_panels();
            init_menu();
            init_plugins();
            Options.SetWindowState("main_window", this);
        }

        [SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.UnmanagedCode)]
        protected override void WndProc(ref Message m)
        {
            base.WndProc(ref m);

            if ((browse_menu != null) && (browse_menu.DriveDetector != null))
            {
                browse_menu.DriveDetector.WndProc(ref m);
            }
        }

        private void set_lang()
        {
            if (LicenseManager.UsageMode == LicenseUsageMode.Designtime)
            {
                return;
            }
            

            this.Text = string.Format
                ("netCommander {0}",
                Application.ProductVersion);

            //menuItemFind.Text = Options.GetLiteral(Options.LANG_FIND);
            menuItemBrowse.Text = Options.GetLiteral(Options.LANG_BROWSE);
            //menuItemNetwork.Text = Options.GetLiteral(Options.LANG_NETWORK);
            //menuItemJumpTo.Text = Options.GetLiteral(Options.LANG_JUMP_TO);
            //menuItemFtpServer.Text = Options.GetLiteral(Options.LANG_FTP_SERVER);
            menuItemPanel.Text = Options.GetLiteral(Options.LANG_PANEL);
            menuItemOptions.Text = Options.GetLiteral(Options.LANG_OPTIONS);
            menuItemFont.Text = Options.GetLiteral(Options.LANG_PANEL_FONT);
            menuItemUserCommands.Text = Options.GetLiteral(Options.LANG_USER_MENU);
            //menuItemDrives.Text = Options.GetLiteral(Options.LANG_DRIVES);
            menuItem_plugins.Text = Options.GetLiteral(Options.LANG_PLUGIN_MENU_HEADER);
            menuItemHelp.Text = Options.GetLiteral(Options.LANG_HELP);
            menuItemAbout.Text = Options.GetLiteral(Options.LANG_ABOUT);
        }

        private PluginEngine plugin_engine = null;
        private void init_plugins()
        {
            plugin_engine = new PluginEngine();
            
            plugin_engine.Init(menuItem_plugins);
            plugin_engine.QueryActivePanel += new QueryPanelInfoEventHandler(plugin_engine_QueryActivePanel);
            plugin_engine.QueryPassivePanel += new QueryPanelInfoEventHandler(plugin_engine_QueryPassivePanel);

            if (menuItem_plugins.MenuItems.Count == 0)
            {
                menuItem_plugins.Visible = false;
            }
        }

        void plugin_engine_QueryPassivePanel(object sender, QueryPanelInfoEventArgs e)
        {
            e.FocusedIndex = doublePanel1.PanelPassive.FocusedIndex;
            e.ItemCollection = doublePanel1.PanelPassive.Source;
            e.SelectedIndices = doublePanel1.PanelPassive.SelectionIndices;
        }

        void plugin_engine_QueryActivePanel(object sender, QueryPanelInfoEventArgs e)
        {
            e.FocusedIndex = doublePanel1.PanelActive.FocusedIndex;
            e.ItemCollection = doublePanel1.PanelActive.Source;
            e.SelectedIndices = doublePanel1.PanelActive.SelectionIndices;
        }

        //private DriveMenuItems drive_menu = null;
        private UserMenu user_menu = null;
        private BrowseMenu browse_menu = null;
        private void init_menu()
        {
            //drive_menu = new DriveMenuItems(this,menuItemDrives);
            //drive_menu.DriveClick += new DriveClickEventHandler(drive_menu_DriveClick);

            browse_menu = new BrowseMenu(menuItemBrowse);
            browse_menu.BrowseMenuClick += new BrowseMenuClickEventHandler(browse_menu_BrowseMenuClick);

            user_menu = new UserMenu(menuItemUserCommands);
            user_menu.QueryCurrentPanel += new QueryPanelInfoEventHandler(user_menu_QueryCurrentPanel);
            user_menu.QueryOtherPanel += new QueryPanelInfoEventHandler(user_menu_QueryOtherPanel);
        }

        void browse_menu_BrowseMenuClick(object sender, BrowseMenuClickEventArgs e)
        {

            mFilePanel target_panel = e.Source == null ? doublePanel1.PanelActive : e.Source;

            switch (e.BrowseType)
            {
                case BrowseTypes.Drive:
                    show_directory(target_panel, e.Drive.RootDirectory.Name);
                    break;
                case BrowseTypes.Find:
                    find_files(target_panel);
                    break;
                case BrowseTypes.Network:
                    show_network_root(target_panel);
                    break;
                case BrowseTypes.Jump:
                    jump_to(target_panel);
                    break;
                case BrowseTypes.Ftp:
                    show_ftp_server(target_panel);
                    break;

                case BrowseTypes.SpecialFolder:
                    show_directory(target_panel, Environment.GetFolderPath(e.SpecialFolder));
                    break;

                case BrowseTypes.Processes:
                    show_processes(target_panel);
                    break;
            }
        }

        void user_menu_QueryOtherPanel(object sender, QueryPanelInfoEventArgs e)
        {
            e.FocusedIndex = doublePanel1.PanelPassive.FocusedIndex;
            
            e.ItemCollection = doublePanel1.PanelPassive.Source;
            e.SelectedIndices = doublePanel1.PanelPassive.SelectionIndices;
        }

        void user_menu_QueryCurrentPanel(object sender, QueryPanelInfoEventArgs e)
        {
            e.FocusedIndex = doublePanel1.PanelActive.FocusedIndex;
            e.ItemCollection = doublePanel1.PanelActive.Source;
            e.SelectedIndices = doublePanel1.PanelActive.SelectionIndices;
        }

        private void init_panels()
        {
            //init font
            doublePanel1.Font = Options.FontFilePanel;

            //map callbacks
            doublePanel1.ActivePanelChange += new EventHandler(doublePanel1_ActivePanelChange);
            doublePanel1.PanelActive.SourceChanged += new EventHandler(Panel_SourceChanged);
            doublePanel1.PanelActive.StatusTextChanged += new EventHandler(PanelActive_StatusTextChanged);
            doublePanel1.PanelPassive.SourceChanged += new EventHandler(Panel_SourceChanged);
            doublePanel1.PanelPassive.StatusTextChanged += new EventHandler(PanelActive_StatusTextChanged);

            string last_dir = Options.LastDirectory;
            if (String.IsNullOrEmpty(last_dir))
            {
                last_dir = Directory.GetCurrentDirectory();
            }

            bool dir_exist = false;
            try
            {
                dir_exist = Directory.Exists(last_dir);
            }
            catch (Exception) { }
            if (dir_exist)
            {
                show_directory(doublePanel1.PanelActive, last_dir);
            }
            else
            {
                show_directory(doublePanel1.PanelActive);
            }
            show_directory(doublePanel1.PanelPassive);
            
        }

        void PanelActive_StatusTextChanged(object sender, EventArgs e)
        {
            mFilePanel panel = (mFilePanel)sender;
            if (panel == doublePanel1.PanelActive)
            {
                toolStripStatusLabelCurrentState.Text = panel.Source.GetStatusText();
            }
        }

        void doublePanel1_ActivePanelChange(object sender, EventArgs e)
        {
            update_panel_menu(doublePanel1.PanelActive);
            toolStripStatusLabelCurrentState.Text = doublePanel1.PanelActive.Source.GetStatusText();
            if (doublePanel1.PanelActive.Source is DirectoryList)
            {
                try
                {
                    Directory.SetCurrentDirectory(((DirectoryList)doublePanel1.PanelActive.Source).DirectoryPath);
                }
                catch (Exception) { }
            }
        }

        void Panel_SourceChanged(object sender, EventArgs e)
        {
            mFilePanel panel = (mFilePanel)sender;
            if (doublePanel1.PanelActive == panel)
            {
                update_panel_menu(panel);
                toolStripStatusLabelCurrentState.Text = doublePanel1.PanelActive.Source.GetStatusText();
            }
        }

        public void NotifyLongOperation(string status, bool enable_status_window)
        {
            labelLongOperationStatus.Text = status;

            if (enable_status_window)
            {
                labelLongOperationStatus.Visible = true;
                labelLongOperationStatus.Refresh();
                Cursor = Cursors.WaitCursor;
            }
            else
            {
                labelLongOperationStatus.Visible = false;
                Cursor = Cursors.Default;
            }
        }

        private void update_panel_menu(mFilePanel panel)
        {
            //clear current
            menuItemPanel.MenuItems.Clear();

            //and add sort options
            if ((panel.Source.AvailableSortMenu != null) && (panel.Source.AvailableSortMenu.Count > 0))
            {
                MenuItem sort_header = new MenuItem();
                sort_header.Text = Options.GetLiteral(Options.LANG_SORT);
                menuItemPanel.MenuItems.Add(sort_header);
                foreach (MenuItem item in panel.Source.AvailableSortMenu)
                {
                    sort_header.MenuItems.Add(item);
                }
            }

            //and add from panel
            foreach (PanelCommandBase cmd in panel.Source.AvailableCommands)
            {
                menuItemPanel.MenuItems.Add(cmd.CommandMenu);
                cmd.QueryOtherPanel += new QueryPanelInfoEventHandler(cmd_QueryOtherPanel);
            }

        }

        void cmd_QueryOtherPanel(object sender, QueryPanelInfoEventArgs e)
        {
            //passive panel info
            e.FocusedIndex = doublePanel1.PanelPassive.FocusedIndex;
            e.ItemCollection = doublePanel1.PanelPassive.Source;
            e.SelectedIndices = doublePanel1.PanelPassive.SelectionIndices;
        }

        private void show_directory(mFilePanel target_panel)
        {
            string def_dir = Directory.GetCurrentDirectory();
            show_directory(target_panel, def_dir);
        }

        private void show_directory(mFilePanel target_panel, string dir_path)
        {
            try
            {
                DirectoryList old_dl = null;

                if ((target_panel.Source != null) && (target_panel.Source is DirectoryList))
                {
                    old_dl = (DirectoryList)target_panel.Source;
                }

                DirectoryList dl = null;

                if (old_dl == null)
                {
                    dl = new DirectoryList
                    (0,
                    false,
                    dir_path);
                }
                else
                {
                    dl = new DirectoryList
                    (old_dl.SortCriteria,
                    old_dl.SortReverse,
                    dir_path);
                }

                dl.MainWindow = this;
                target_panel.Source = dl;
                try
                {
                    dl.Refill();
                }
                catch (Exception ex_inner)
                {
                    if (old_dl != null)
                    {
                        target_panel.Source = old_dl;
                    }
                    throw ex_inner;
                }
            }
            catch (Exception ex)
            {
                Messages.ShowException(ex);
            }
        }

        private void show_processes(mFilePanel target_panel)
        {
            ProcessList pl = new ProcessList(0, false, string.Empty);
            pl.MainWindow = this;
            target_panel.Source = pl;
            pl.Refill();
        }

        

        #region DEBUG
        //test menu
        private void menuItem1_Click(object sender, EventArgs e)
        {
            //System.Diagnostics.Process[] processes = System.Diagnostics.Process.GetProcesses();

            //BassPlayer.BassCore bass_core = new BassPlayer.BassCore();

            //bass_core.Dispose();
        }

        private void testToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //string tt = Path.GetDirectoryName(@"c:\dir1\dir2\dir3\dir4\file.name");
            //tt = Path.GetDirectoryName(tt);
            //tt = Path.GetPathRoot(tt);
            //tt = Path.GetDirectoryName(tt);

            //bool res = Wrapper.IsSameVolume(@"c:\temp\cons.chm", @"c:\rk6\endday");
            //try
            //{
            //    Wrapper.CreateDirectoryTree(@"d:\dir0\dir1\dir2\dir3\dir4", string.Empty);
            //}
            //catch (Exception ex)
            //{
            //    Messages.ShowException(ex);
            //}

            //long res = Wrapper.GetFileSize(@"c:\temp\test_copy\test.txt:hidden");

            //NT_FILE_STANDARD_INFORMATION si = WrapperNT.GetFileInfo_standard(@"c:\temp\test_copy");
            //NT_FILE_BASIC_INFO bi = WrapperNT.GetFileInfo_basic(@"c:\temp\test_copy");
            //WrapperNT.FileStream_enum stream_enum = new WrapperNT.FileStream_enum(@"c:\temp\test_copy\test.txt");
            //foreach (NT_FILE_STREAM_INFORMATION info in stream_enum)
            //{
            //    long s = info.StreamSize;
            //}

            //FILE_FS_ATTRIBUTE_INFORMATION vol_attr = WrapperNT.GetFileVolumeAttributeInfo(@"z:\clerks.avi");
            //FILE_FS_CONTROL_INFORMATION vol_ctrl = WrapperNT.GetFileVolumeControlInfo(@"z:\clerks.avi");
            //FILE_FS_FULLSIZE_INFORMATION vol_fs = WrapperNT.GetFileVolumeFullsizeInfo(@"z:\clerks.avi");
            //FILE_FS_VOLUME_INFORMATION vol_info = WrapperNT.GetFileVolumeInfo(@"z:\clerks.avi");
            //FILE_FS_DEVICE_INFORMATION vol_dev = WrapperNT.GetFileVolumeDeviceInfo(@"z:\clerks.avi");

            //FileSystemAccessRuleEditor d = new FileSystemAccessRuleEditor();
            //d.ShowDialog();

            //int res = WinApiFS.CopyFile
            //    (@"c:\temp\test_copy\test.txt:hidden",
            //    @"c:\temp\test_copy\test_hidden_copyfile.txt",
            //    true);
            //int win_err = System.Runtime.InteropServices.Marshal.GetLastWin32Error();
            //Win32Exception win_ex = new Win32Exception(win_err);

            //netCommander.NetApi.SERVER_INFO_101[] si = netCommander.NetApi.WinApiNETwrapper.GetServerInfos_101(netCommander.NetApi.NetserverEnumType.ALL);

            //UseWaitCursor = true;
            //Refresh();
            ////FileCollectionBase old = doublePanel1.PanelActive.Source;
            //try
            //{
            //    NetServerList nl = new NetServerList(0, false);
            //    nl.Refill();
            //    doublePanel1.PanelActive.Source = nl;
            //}
            //catch (Exception ex)
            //{
            //    UseWaitCursor = false;
            //    Messages.ShowException(ex);
            //    //doublePanel1.PanelActive.Source = old;
            //}
            //UseWaitCursor = false;

            //netCommander.NetApi.NET_DISPLAY_USER[] du =
            //    netCommander.NetApi.WinApiNETwrapper.QueryDisplayInfoUser("nt03");
            //netCommander.NetApi.NET_DISPLAY_MACHINE[] dm =
            //    netCommander.NetApi.WinApiNETwrapper.QueryDisplayInfoMachine("nt03");
            //netCommander.NetApi.NET_DISPLAY_GROUP[] dg =
            //    netCommander.NetApi.WinApiNETwrapper.QueryDisplayInfoGroup("nt03");
            //netCommander.NetApi.SHARE_INFO_1[] si2 =
            //    netCommander.NetApi.WinApiNETwrapper.GetShareInfos_1("jumboss1");

            //netCommander.WNet.NETRESOURCE root_res = new netCommander.WNet.NETRESOURCE();
            //root_res.dwDisplayType = netCommander.WNet.ResourceDisplayType.ROOT;
            //root_res.dwScope = netCommander.WNet.ResourceScope.GLOBALNET;
            //root_res.dwType = netCommander.WNet.ResourceType.ANY;
            //root_res.dwUsage = netCommander.WNet.ResourceUsage.NONE;
            //root_res.lpLocalName = null;
            //root_res.lpProvider = null;
            //root_res.lpRemoteName = null;
            //netCommander.WNet.WnetResourceEnumerator res_enum = new netCommander.WNet.WnetResourceEnumerator
            //(root_res,
            //netCommander.WNet.ResourceScope.GLOBALNET,
            //netCommander.WNet.ResourceType.ANY,
            //netCommander.WNet.ResourceUsage.NONE);
            //foreach (netCommander.WNet.NETRESOURCE resource in res_enum)
            //{
            //    string n = resource.lpRemoteName;

            //    netCommander.WNet.WnetResourceEnumerator cur_enum = new netCommander.WNet.WnetResourceEnumerator
            //    (resource,
            //    netCommander.WNet.ResourceScope.GLOBALNET,
            //    netCommander.WNet.ResourceType.ANY,
            //    netCommander.WNet.ResourceUsage.NONE);
            //    foreach (netCommander.WNet.NETRESOURCE ch_resource in cur_enum)
            //    {
            //        string ch_n = ch_resource.lpRemoteName;

            //        netCommander.WNet.WnetResourceEnumerator cur2_enum = new netCommander.WNet.WnetResourceEnumerator
            //        (ch_resource,
            //        netCommander.WNet.ResourceScope.GLOBALNET,
            //        netCommander.WNet.ResourceType.ANY,
            //        netCommander.WNet.ResourceUsage.NONE);
            //        foreach (netCommander.WNet.NETRESOURCE ch2_resource in cur2_enum)
            //        {
            //            string ch2_n = ch2_resource.lpRemoteName;
            //        }
            //    }
            //}

            //netCommander.WNet.NETRESOURCE hid_res = new netCommander.WNet.NETRESOURCE();
            //hid_res.dwDisplayType = netCommander.WNet.ResourceDisplayType.SHAREADMIN;
            //hid_res.lpProvider = "Microsoft Windows Network";
            //hid_res.lpRemoteName = @"\\nt03\D$";

            //netCommander.WNet.NETRESOURCE up_res = netCommander.WNet.WinApiWNETwrapper.GetParentResource(hid_res);


            //netCommander.WNet.NETRESOURCE root_res = new netCommander.WNet.NETRESOURCE();
            //root_res.dwDisplayType = netCommander.WNet.ResourceDisplayType.ROOT;
            //root_res.dwScope = netCommander.WNet.ResourceScope.GLOBALNET;
            //root_res.dwType = netCommander.WNet.ResourceType.ANY;
            //root_res.dwUsage = netCommander.WNet.ResourceUsage.NONE;
            //root_res.lpLocalName = null;
            //root_res.lpProvider = null;
            //root_res.lpRemoteName = null;
            //netCommander.WNet.WnetResourceEnumerator res_enum = new netCommander.WNet.WnetResourceEnumerator
            //(root_res,
            //netCommander.WNet.ResourceScope.GLOBALNET,
            //netCommander.WNet.ResourceType.ANY,
            //netCommander.WNet.ResourceUsage.NONE);
            //foreach (netCommander.WNet.NETRESOURCE resource in res_enum)
            //{
            //    netCommander.WNet.NETINFOSTRUCT inf = netCommander.WNet.WinApiWNETwrapper.GetNetworkInfo(resource.lpProvider);
            //}

            //netCommander.WNet.NETINFOSTRUCT inf = netCommander.WNet.WinApiWNETwrapper.GetNetworkInfo("Microsoft Windows Network");

            //try
            //{
            //    WnetResourceList rl = new WnetResourceList(0, false);

            //    doublePanel1.PanelActive.Source = rl;
            //    rl.Refill();
            //}
            //catch (Exception ex)
            //{
            //    UseWaitCursor = false;
            //    Messages.ShowException(ex);
            //    //doublePanel1.PanelActive.Source = old;
            //}
            //UseWaitCursor = false;

            

            //netCommander.WNet.NETRESOURCE res = netCommander.WNet.WinApiWNETwrapper.GetResourceInfo(@"\\nt03\temp\test_copy");
            //netCommander.WNet.NETRESOURCE p_res = netCommander.WNet.WinApiWNETwrapper.GetParentResource(res);

            //FtpOptions f_opt = FtpOptions.Default;

            //int len1 = System.Runtime.InteropServices.Marshal.SizeOf(typeof(REPARSE_DATA_BUFFER_SYMLINK));
            //int len2 = System.Runtime.InteropServices.Marshal.SizeOf(typeof(REPARSE_DATA_BUFFER_MOUNTPOINT));

            //FileInfoExEnumerable fie_enum = new FileInfoExEnumerable(@"c:\temp\*",true,true,true,true);
            //foreach (FileInfoEx info in fie_enum)
            //{
            //    ulong s = info.Size;
            //}

            //string[] f1 = Directory.GetFiles(@"c:\temp", "*", SearchOption.TopDirectoryOnly);
            //string[] d1 = Directory.GetDirectories(@"c:\temp", "*", SearchOption.TopDirectoryOnly);
            //DirectoryInfo di = new DirectoryInfo(@"c:\temp");
            //FileSystemInfo[] fsi = di.GetFileSystemInfos();

            //WinAPiFSwrapper.SetMountpoint(@"c:\temp_l", @"c:\temp");
            //WinAPiFSwrapper.DeleteMountpoint(@"c:\temp_l");
            //WinAPiFSwrapper.SetMountPoint(@"c:\temp_l", @"c:\temp");

            //WinAPiFSwrapper.SetMountpoint(@"c:\users\max\temp_l", @"c:\users\max");
            //WinAPiFSwrapper.SetSymbolicLink(@"c:\users\max\temp_s", @"c:\temp", false);

            //REPARSE_DATA_BUFFER_MOUNTPOINT res1 = WinAPiFSwrapper.GetMountpointInfo(@"c:\users\max\temp_l");
            //REPARSE_DATA_BUFFER_SYMLINK res2 = WinAPiFSwrapper.GetSymboliclinkInfo(@"c:\users\max\temp_s");

            //FtpConnectionOptions opts = new FtpConnectionOptions();
            //opts.Passive = false;
            //opts.Password = "turrentine";
            //opts.ServerName = @"ftp://nt03";
            //opts.UserName = "max_ftp_admin";
            //opts.Anonymous = false;

            //FtpConnection f_con = new FtpConnection(opts);

            //f_con.CreateDirectory("utils/first_dir/second_dir/third_dir");
            //f_con.CreateDirectoryTree("utils/first_dir/second_dir/third_dir");

            //FtpEntryInfo info = new FtpEntryInfo();
            //bool exists = f_con.GetEntryInfo("utils/xml", ref info, false);

            //List<FtpEntryInfo> res_list = f_con.GetDirectoryDetailsList("utils/not_existent_file");
            //List<string> res_list_2 = f_con.GetDirectoryList("utils/not_existent_file");
            //System.Net.FtpStatusCode res = f_con.DeleteDirectory("temp");

            //long s = f_con.GetSize("utils/gawk.tgz");
            //DateTime dt = f_con.GetStamp("utils/gawk.tgz");
            //List<string> fl = f_con.GetDirectoryList("video2/");
            //List<string> fl_2=f_con.GetDirectoryList("video2");
            ////List<FtpEntryInfo> fl_d = f_con.GetDirectoryDetailsList("video2/");
            //List<FtpEntryInfo> fl_d = f_con.GetDirectoryDetailsList("utils/");

            //FtpDownloadEngine fde = new FtpDownloadEngine
            //(fl_d.ToArray(),
            //"",
            //f_con,
            //FtpTransferOptions.Default(),
            //null);
            //fde.Run();

            //FtpEntryInfo info = f_con.GetEntryInfo("utils/xml");

            //System.Net.FtpStatusCode res = f_con.DeleteFile("antivir/eicar.com");

            //int res = netCommander.FileSystemEx.WinApiFS.RemoveDirectory(@"c:\temp_l");
            //int win_err = System.Runtime.InteropServices.Marshal.GetLastWin32Error();


            //KeysConverter k_conv = new KeysConverter();
            //object o = k_conv.ConvertFromString("a")

            //DriveInfo[] dis = DriveInfo.GetDrives();
            //VolumeInfo vi = new VolumeInfo(@"z:\temp");
            //VolumeSpaceInfo vsi1 = WinAPiFSwrapper.GetVolumeSpaceInfo(@"z:\");
            //VolumeSpaceInfo vsi2 = WinAPiFSwrapper.GetVolumeSpaceInfo(@"z:\temp");
            //VolumeSpaceInfo vsi3 = WinAPiFSwrapper.GetVolumeSpaceInfo(@"\\nt03\d\vid");
            //VolumeSpaceInfo vsi4 = WinAPiFSwrapper.GetVolumeSpaceInfo(@"c:\");
            //VolumeSpaceInfo vsi5 = WinAPiFSwrapper.GetVolumeSpaceInfo(@"c:\temp");

            //VolumeSpaceInfo vsi6 = WinAPiFSwrapper.GetVolumeSpaceInfo(@"\\nt03\d$");

            //string reg_pattern = @"""(?:(?<="")([^""]+)""\s*)|\s*([^""\s]+)";
            //System.Text.RegularExpressions.Regex rex = new System.Text.RegularExpressions.Regex(reg_pattern, System.Text.RegularExpressions.RegexOptions.IgnoreCase | System.Text.RegularExpressions.RegexOptions.Singleline);
            //string[] split1 = rex.Split(@"""long command with space"" -arg1 arg2 /arg3 ""OPtiona with space"" arg4=100", 2);
            //string[] split2 = rex.Split(@"longCommand -arg1 arg2 /arg3 arg4=100", 2);
            //string[] split3 = rex.Split(@"""c:\temp\long command without args""", 2);

            //TextViewDialog t_dialog = new TextViewDialog();
            //t_dialog.Show();

            //TextViewForm t_form = new TextViewForm();
            //t_form.Show();

            //switch (Options.MenuRenderMode)
            //{
            //    case ToolStripRenderMode.ManagerRenderMode:
            //        menuStrip1.RenderMode = ToolStripRenderMode.Professional;
            //        Options.MenuRenderMode = ToolStripRenderMode.Professional;
            //        break;

            //    case ToolStripRenderMode.Professional:
            //        menuStrip1.RenderMode = ToolStripRenderMode.System;
            //        Options.MenuRenderMode = ToolStripRenderMode.System;
            //        break;

            //    case ToolStripRenderMode.System:
            //        menuStrip1.RenderMode = ToolStripRenderMode.ManagerRenderMode;
            //        Options.MenuRenderMode = ToolStripRenderMode.ManagerRenderMode;
            //        break;
            //}
        }
        #endregion

        #region closing
        private void mainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            //save state
            Options.SaveWindowState("main_window", this);

            //save last dir
            if ((doublePanel1 != null) && (doublePanel1.PanelActive.Source is DirectoryList))
            {
                Options.LastDirectory = ((DirectoryList)doublePanel1.PanelActive.Source).DirectoryPath;
            }
        }
        #endregion

        #region find

        private void find_files(mFilePanel target_panel)
        {
            if (target_panel == null)
            {
                target_panel = doublePanel1.PanelActive;
            }

            DirectoryListFilterDialog filter_dialog = null;
            DirectoryListFilter filter = new DirectoryListFilter();
            filter.Load();

            if (doublePanel1.PanelActive.Source is DirectoryList)
            {
                filter.InCurrentDirectory = true;
                filter.InCurrentDirectoryWithSubdirs = true;
                filter.CurrentDirectory = ((DirectoryList)doublePanel1.PanelActive.Source).DirectoryPath;
                filter.CurrentDrive = Path.GetPathRoot(filter.CurrentDirectory);
            }

            filter_dialog = new DirectoryListFilterDialog(true, filter);
            filter_dialog.Text = Options.GetLiteral(Options.LANG_FIND);
            if (filter_dialog.ShowDialog() == DialogResult.OK)
            {
                DirectorySearchResultDialog result_dialog = new DirectorySearchResultDialog(filter_dialog.DirectoryListFilter);
                //result_dialog.BeginSearch(filter_dialog.DirectoryListFilter);
                if (result_dialog.ShowDialog() == DialogResult.OK)
                {
                    //go to...
                    string sel_file_name = result_dialog.SelectedResult;
                    if (string.IsNullOrEmpty(sel_file_name))
                    {
                        return;
                    }
                    string sel_dir = Path.GetDirectoryName(sel_file_name);
                    string sel_file = Path.GetFileName(sel_file_name);
                    DirectoryList dl = new DirectoryList(target_panel.Source.SortCriteria, target_panel.Source.SortReverse, sel_dir);
                    target_panel.Source = dl;
                    dl.Refill();
                    int foc_index = dl.FindIndexOfName(sel_file);
                    if (foc_index != -1)
                    {
                        doublePanel1.PanelActive.FocusedIndex = foc_index;
                    }
                }
            }
        }

        private void menuItemFind_Click(object sender, EventArgs e)
        {
            find_files(null);
        }

        #endregion

        #region network
        private void menuItemNetwork_Click(object sender, EventArgs e)
        {
            show_network_root(doublePanel1.PanelActive);   
        }

        private void show_network_root(mFilePanel target_panel)
        {
            WnetResourceList nl = new WnetResourceList
            (0, false, netCommander.WNet.WinApiWNETwrapper.RootNetresource);
            nl.MainWindow = this;
            target_panel.Source = nl;
            nl.Refill();
        }
        #endregion

        #region jump
        private void menuItemJumpTo_Click(object sender, EventArgs e)
        {
            jump_to(doublePanel1.PanelActive);
        }

        private void jump_to(mFilePanel target_panel)
        {
            JumpToDialog dial = new JumpToDialog();
            dial.Text = Options.GetLiteral(Options.LANG_WHERE_TO_JUMP);

            if (dial.ShowDialog() != DialogResult.OK)
            {
                return;
            }

            string path = dial.textBoxPath.Text;
            FileCollectionBase new_source = null;

            try
            {
                //parse path
                if (path.StartsWith(@"\\"))
                {
                    //assume smb path

                    NETRESOURCE target_res = WinApiWNETwrapper.GetResourceInfo(path);
                    if (target_res.dwDisplayType == netCommander.WNet.ResourceDisplayType.SHARE)
                    {
                        //it is network share - show directory list
                        new_source = new DirectoryList(0, false, path);
                    }
                    else if ((target_res.dwUsage & ResourceUsage.CONTAINER) != ResourceUsage.CONTAINER)
                    {
                        //target_res is not container - try show parent
                        target_res = WinApiWNETwrapper.GetParentResource(target_res);
                        new_source = new WnetResourceList(0, false, target_res);
                    }
                    else
                    {
                        //show target_resource
                        new_source = new WnetResourceList(0, false, target_res);
                    }

                }//end of stratsWith("\\") brunch
                else
                {
                    new_source = new DirectoryList(0, false, path);
                }

                //now refill source
                new_source.MainWindow = this;
                new_source.Refill();
                target_panel.Source = new_source;
                target_panel.Refresh();
            }
            catch (Exception ex)
            {
                Messages.ShowException(ex, string.Format(Options.GetLiteral(Options.LANG_CANNOT_SHOW_0), path));
            }
        }
        #endregion

        #region ftp
        private void menuItemFtpServer_Click(object sender, EventArgs e)
        {
            show_ftp_server(doublePanel1.PanelActive);
        }

        private void show_ftp_server(mFilePanel target_panel)
        {
            FtpConnectionOptions opts = Options.FtpOptions;
            FtpConnectionDialog dialog = new FtpConnectionDialog();
            dialog.FtpConnectionOptions = opts;
            dialog.Text = Options.GetLiteral(Options.LANG_CONNECT_TO_FTP_SERVER);

            if (dialog.ShowDialog() != DialogResult.OK)
            {
                return;
            }

            opts = dialog.FtpConnectionOptions;
            Options.FtpOptions = opts;

            if (!opts.Anonymous)
            {
                string user = opts.UserName;
                string pass = opts.Password;
                Messages.AskCredentials
                    (Options.GetLiteral(Options.LANG_LOGIN_TO_FTP_SERVER), opts.ServerName, ref user, ref pass);
                opts.UserName = user;
                opts.Password = pass;
            }

            FtpConnection conn = new FtpConnection(opts);
            FtpDirectoryList fl = new FtpDirectoryList(0, false, conn);
            try
            {
                fl.MainWindow = this;
                fl.Refill();
                target_panel.Source = fl;
                target_panel.Refresh();
            }
            catch (Exception ex)
            {
                Messages.ShowException(ex);
            }
        }

        #endregion

        #region font
        private void menuItemFont_Click(object sender, EventArgs e)
        {
            fontDialog1.Font = doublePanel1.Font;

            if (fontDialog1.ShowDialog() == DialogResult.OK)
            {
                Font new_font = fontDialog1.Font;
                doublePanel1.Font = new_font;
                Options.FontFilePanel = new_font;

            }
        }

        #endregion

        protected override bool ProcessDialogKey(Keys keyData)
        {
            switch (keyData)
            {
                case Keys.Alt | Keys.F1:
                    browse_menu.ShowAsContextMenu(doublePanel1.PanelLeft);
                    return true;

                case Keys.Alt | Keys.F2:
                    browse_menu.ShowAsContextMenu(doublePanel1.PanelRight);
                    return true;
            }

            return base.ProcessDialogKey(keyData);
        }

        //about
        private void menuItem3_Click(object sender, EventArgs e)
        {
            AboutBox ab = new AboutBox();
            ab.ShowDialog();
        }

        
    }
}
