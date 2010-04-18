using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Windows.Forms;
using netCommander.winControls;
using Dolinay;
using System.Drawing;

namespace netCommander
{
    internal class BrowseMenu
    {
        private DriveDetector notifier = null;
        private List<InternalBrowseItem> menu_items;
        private MenuItem parent_main_menu_item;
        private ContextMenu context_menu;
        private const int context_pad = 10;

        #region shortcuts
        private static Shortcut[] drive_shortcuts = new Shortcut[]
            {Shortcut.CtrlShiftA,
            Shortcut.CtrlShiftB,
            Shortcut.CtrlShiftC,
            Shortcut.CtrlShiftD,
            Shortcut.CtrlShiftE,
            Shortcut.CtrlShiftF,
            Shortcut.CtrlShiftG,
            Shortcut.CtrlShiftH,
            Shortcut.CtrlShiftI,
            Shortcut.CtrlShiftJ,
            Shortcut.CtrlShiftK,
            Shortcut.CtrlShiftL,
            Shortcut.CtrlShiftM,
            Shortcut.CtrlShiftN,
            Shortcut.CtrlShiftO,
            Shortcut.CtrlShiftP,
            Shortcut.CtrlShiftQ,
            Shortcut.CtrlShiftR,
            Shortcut.CtrlShiftS,
            Shortcut.CtrlShiftT,
            Shortcut.CtrlShiftU,
            Shortcut.CtrlShiftV,
            Shortcut.CtrlShiftW,
            Shortcut.CtrlShiftX,
            Shortcut.CtrlShiftY,
            Shortcut.CtrlShiftZ};

        private static string[] drive_letters = new string[]
            {@"a:\",
            @"b:\",
            @"c:\",
            @"d:\",
            @"e:\",
            @"f:\",
            @"g:\",
            @"h:\",
            @"i:\",
            @"j:\",
            @"k:\",
            @"l:\",
            @"m:\",
            @"n:\",
            @"o:\",
            @"p:\",
            @"q:\",
            @"r:\",
            @"s:\",
            @"t:\",
            @"u:\",
            @"v:\",
            @"w:\",
            @"x:\",
            @"y:\",
            @"z:\"};
        #endregion

        public BrowseMenu(MenuItem parent_menu_item)
        {
            parent_main_menu_item = parent_menu_item;

            intern_build();
            notifier = new DriveDetector(parent_menu_item.GetMainMenu().GetForm());
            notifier.DeviceArrived += new DriveDetectorEventHandler(notifier_DeviceArrived);
            notifier.DeviceRemoved += new DriveDetectorEventHandler(notifier_DeviceRemoved);
        }

        internal void ShowAsContextMenu(mFilePanel source_panel)
        {
            //calc coords:
            Point target_pt = new Point(context_pad, context_pad);
            context_menu.Show(source_panel, target_pt);
            

        }

        internal DriveDetector DriveDetector
        {
            get
            {
                return notifier;
            }
        }

        internal event BrowseMenuClickEventHandler BrowseMenuClick;

        private void intern_build()
        {
            //clear
            parent_main_menu_item.MenuItems.Clear();
            if (menu_items == null)
            {
                menu_items = new List<InternalBrowseItem>();
            }
            menu_items.Clear();

            //add drive refs
            DriveInfo[] dis = DriveInfo.GetDrives();

            foreach (DriveInfo di in dis)
            {
                InternalBrowseItem drive_menu = new InternalBrowseItem(di);
                drive_menu.Click += new EventHandler(browse_menu_Click);
                menu_items.Add(drive_menu);
            }

            //add line
            InternalBrowseItem menu_break_1 = new InternalBrowseItem(BrowseTypes.Break);
            menu_items.Add(menu_break_1);


            //add special folders
            string[] spec_folder_enum_names = Enum.GetNames(typeof(Environment.SpecialFolder));
            SortedList<string, object> spec_folder_enum_names_sorted =
                new SortedList<string, object>(StringComparer.InvariantCulture);
            foreach (string one_spec_folder_name in spec_folder_enum_names)
            {
                spec_folder_enum_names_sorted.Add(one_spec_folder_name, null);
            }
            InternalBrowseItem spec_folder_header_menu =
                new InternalBrowseItem(Options.GetLiteral(Options.LANG_SPECIAL_FOLDERS));
            foreach(KeyValuePair<string,object> kvp in spec_folder_enum_names_sorted)
            {
                InternalBrowseItem spec_folder_menu = new InternalBrowseItem
                    ((Environment.SpecialFolder)Enum.Parse(typeof(Environment.SpecialFolder), kvp.Key),
                    kvp.Key);
                spec_folder_header_menu.MenuItems.Add(spec_folder_menu);
                spec_folder_menu.Click += new EventHandler(browse_menu_Click);
                
            }
            menu_items.Add(spec_folder_header_menu);

            //add some browse items
            InternalBrowseItem find_menu = new InternalBrowseItem(BrowseTypes.Find);
            menu_items.Add(find_menu);
            find_menu.Click += new EventHandler(browse_menu_Click);

            InternalBrowseItem network_menu = new InternalBrowseItem(BrowseTypes.Network);
            menu_items.Add(network_menu);
            network_menu.Click += new EventHandler(browse_menu_Click);

            InternalBrowseItem jump_menu = new InternalBrowseItem(BrowseTypes.Jump);
            menu_items.Add(jump_menu);
            jump_menu.Click += new EventHandler(browse_menu_Click);

            InternalBrowseItem ftp_menu = new InternalBrowseItem(BrowseTypes.Ftp);
            menu_items.Add(ftp_menu);
            ftp_menu.Click += new EventHandler(browse_menu_Click);

            InternalBrowseItem processes_menu = new InternalBrowseItem(BrowseTypes.Processes);
            menu_items.Add(processes_menu);
            processes_menu.Click += new EventHandler(browse_menu_Click);

            //add to main menu
            parent_main_menu_item.MenuItems.AddRange(menu_items.ToArray());

            //add to context menu
            if (context_menu == null)
            {
                context_menu = new ContextMenu();
            }
            context_menu.MenuItems.Clear();
            int added_ind = 0;
            foreach (InternalBrowseItem main_item in menu_items)
            {
                added_ind = context_menu.MenuItems.Add(main_item.CloneBrowseMenu());
                context_menu.MenuItems[added_ind].Click += new EventHandler(browse_menu_Click);
                if (context_menu.MenuItems[added_ind].MenuItems.Count > 0)
                {
                    foreach (InternalBrowseItem child_item in context_menu.MenuItems[added_ind].MenuItems)
                    {
                        child_item.Click += new EventHandler(browse_menu_Click);
                    }
                }
            }

            //add line (only main menu)
            InternalBrowseItem menu_break_2 = new InternalBrowseItem(BrowseTypes.Break);
            parent_main_menu_item.MenuItems.Add(menu_break_2);

            //add refresh (only main menu)
            MenuItem refresh_menu = new MenuItem(Options.GetLiteral(Options.LANG_REFRESH));
            parent_main_menu_item.MenuItems.Add(refresh_menu);
            refresh_menu.Click += new EventHandler(refresh_menu_Click);

        }

        void refresh_menu_Click(object sender, EventArgs e)
        {
            intern_build();
        }

        void browse_menu_Click(object sender, EventArgs e)
        {
            if (BrowseMenuClick == null)
            {
                return;
            }

            InternalBrowseItem menu = (InternalBrowseItem)sender;

            ContextMenu cont_menu = menu.GetContextMenu();
            BrowseMenuClick
                (this,
                new BrowseMenuClickEventArgs
                    (menu.BrowseType,
                    cont_menu == null ? null : (mFilePanel)cont_menu.SourceControl,
                    menu.DriveInfo,
                    menu.SpecialFolder));
        }

        

        private static void map_shortcut(DriveInfo drive_info, InternalBrowseItem menu_item)
        {
            string drive_letter = drive_info.Name.ToLower(System.Globalization.CultureInfo.InvariantCulture);
            for (int i = 0; i < drive_letters.Length; i++)
            {
                if (drive_letters[i] != drive_letter)
                {
                    continue;
                }

                menu_item.Shortcut = drive_shortcuts[i];
                break;
            }
        }

        private void notifier_DeviceRemoved(object sender, DriveDetectorEventArgs e)
        {
            intern_build();
        }

        private void notifier_DeviceArrived(object sender, DriveDetectorEventArgs e)
        {
            intern_build();
        }

        private class InternalBrowseItem : MenuItem
        {
            public InternalBrowseItem CloneBrowseMenu()
            {
                InternalBrowseItem ret = new InternalBrowseItem();

                ret.BarBreak = BarBreak;
                ret.Break = Break;
                ret.BrowseType = BrowseType;
                ret.DriveInfo = DriveInfo;
                ret.Enabled = Enabled;
                ret.OwnerDraw = OwnerDraw;
                ret.Shortcut = Shortcut;
                ret.ShowShortcut = ShowShortcut;
                ret.Text = Text;
                ret.Visible = Visible;
                ret.SpecialFolder = SpecialFolder;

                if (MenuItems.Count > 0)
                {
                    foreach (MenuItem child in MenuItems)
                    {
                        ret.MenuItems.Add(((InternalBrowseItem)child).CloneBrowseMenu());
                    }
                }

                //ret.Events.AddHandlers(this.Events);

                return ret;
            }

            private InternalBrowseItem()
            {

            }

            public InternalBrowseItem(DriveInfo info)
            {
                DriveInfo = info;

                this.Text = string.Format
                    ("{0} [{1}] {2}",
                    info.Name,
                    info.DriveType,
                    info.IsReady ? info.VolumeLabel : string.Empty);
                this.BrowseType = BrowseTypes.Drive;
                BrowseMenu.map_shortcut(info, this);
            }

            public InternalBrowseItem(Environment.SpecialFolder special_folder, string spec_folder_enum_name)
            {
                Text = Options.GetLiteral(spec_folder_enum_name);
                DriveInfo = null;
                BrowseType = BrowseTypes.SpecialFolder;
                SpecialFolder = special_folder;
            }

            public InternalBrowseItem(string text)
            {
                Text = text;
                DriveInfo = null;
                BrowseType = BrowseTypes.Parent;
            }

            public InternalBrowseItem(BrowseTypes type)
            {
                DriveInfo = null;
                BrowseType = type;

                switch (type)
                {
                    case BrowseTypes.Find:
                        Text = Options.GetLiteral(Options.LANG_FIND);
                        Shortcut = Shortcut.AltF7;
                        break;

                    case BrowseTypes.Ftp:
                        Text = Options.GetLiteral(Options.LANG_FTP_SERVER);
                        break;

                    case BrowseTypes.Jump:
                        Text = Options.GetLiteral(Options.LANG_JUMP_TO);
                        Shortcut = Shortcut.AltF8;
                        break;

                    case BrowseTypes.Network:
                        Text = Options.GetLiteral(Options.LANG_NETWORK);
                        break;

                    case BrowseTypes.Break:
                        Text = "-";
                        break;

                    case BrowseTypes.Processes:
                        Text = Options.GetLiteral(Options.LANG_PROCESSES);
                        break;
                }
            }

            public Environment.SpecialFolder SpecialFolder { get; private set; }
            public DriveInfo DriveInfo { get; private set; }
            public BrowseTypes BrowseType { get; private set; }
        }
    }

    internal enum BrowseTypes
    {
        Drive,
        Find,
        Network,
        Jump,
        Ftp,
        Break,
        SpecialFolder,
        Parent,
        Processes
    }

    internal class BrowseMenuClickEventArgs : EventArgs
    {
        public BrowseTypes BrowseType { get; private set; }
        public mFilePanel Source { get; private set; }
        public DriveInfo Drive { get; private set; }
        public Environment.SpecialFolder SpecialFolder { get; private set; }

        public BrowseMenuClickEventArgs(BrowseTypes type)
        {
            BrowseType = type;
        }

        public BrowseMenuClickEventArgs(BrowseTypes type,mFilePanel source)
        {
            BrowseType = type;
            Source = source;
        }

        public BrowseMenuClickEventArgs(BrowseTypes type,mFilePanel source,DriveInfo drive,Environment.SpecialFolder spec_folder)
        {
            BrowseType = type;
            Source = source;
            Drive = drive;
            SpecialFolder = spec_folder;
        }

        public BrowseMenuClickEventArgs(Environment.SpecialFolder special_folder)
        {
            BrowseType = BrowseTypes.SpecialFolder;
            SpecialFolder = special_folder;
        }
    }

    internal delegate void BrowseMenuClickEventHandler(object sender,BrowseMenuClickEventArgs e);
}
