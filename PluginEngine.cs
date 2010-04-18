using System;
using System.Windows.Forms;
using System.IO;
using System.Reflection;
using System.Collections;
using System.Collections.Generic;

namespace netCommander
{
    class PluginEngine
    {
        public event QueryPanelInfoEventHandler QueryActivePanel;
        public event QueryPanelInfoEventHandler QueryPassivePanel;

        private List<IPlugin> plugins_loaded = new List<IPlugin>();
        private MenuItem parent_plugin_menu = null;

        public void Init(MenuItem parent_menu)
        {
            parent_plugin_menu = parent_menu;
            plugins_loaded.Clear();

            //first get plugins dll
            string[] plugins_dll = Directory.GetFiles
                (Path.GetDirectoryName(Application.ExecutablePath),
                "plugin_*.dll",
                SearchOption.TopDirectoryOnly);

            //iterate throw dlls
            foreach (string plugin_dll in plugins_dll)
            {
                try
                {
                    //load assembly
                    Assembly plugin_assembly = null;
                    plugin_assembly = Assembly.LoadFile(plugin_dll);
                    if (plugin_assembly == null)
                    {
                        continue;
                    }

                    Type[] plugin_types = plugin_assembly.GetTypes();
                    //iterate throw assembly types
                    foreach (Type one_type in plugin_types)
                    {

                        //continue if type is not class and not public public
                        if (!(one_type.IsClass && one_type.IsPublic))
                        {
                            continue;
                        }

                        //check if type implements IPlugin interface
                        Type plugin_interface_type = one_type.GetInterface("IPlugin", true);
                        if (plugin_interface_type == null)
                        {
                            continue;
                        }

                        //create plugin instance
                        IPlugin one_plugin = (IPlugin)plugin_assembly.CreateInstance(one_type.FullName);

                        //add to list
                        plugins_loaded.Add(one_plugin);

                        //bind to query callback
                        one_plugin.QueryHostInfo += new QueryHostInfoEventHandler(one_plugin_QueryHostInfo);

                        //call init
                        one_plugin.Init();

                        
                    }

                }//to next type
                catch (Exception ex)
                {
                    Messages.ShowException(ex, string.Format(Options.GetLiteral(Options.LANG_FAILED_TO_LOAD_PLUGIN_FROM_0), plugin_dll));
                }
            }//to next dll

        }

        void one_plugin_QueryHostInfo(object sender, QueryHostInfoEventArgs e)
        {
            //query panels
            QueryPanelInfoEventArgs e_active = new QueryPanelInfoEventArgs();
            QueryPanelInfoEventArgs e_passive = new QueryPanelInfoEventArgs();
            on_query_active_panel(e_active);
            on_query_passive_panel(e_passive);

            //create args
            string[] selected_files = new string[0];
            if ((e_active.SelectedIndices.Length == 0) && (e_active.FocusedIndex >= 0))
            {
                selected_files = new string[] { e_active.ItemCollection.GetCommandlineTextLong(e_active.FocusedIndex) };
            }
            else
            {
                selected_files = new string[e_active.SelectedIndices.Length];
                for (int i = 0; i < e_active.SelectedIndices.Length; i++)
                {
                    selected_files[i] = e_active.ItemCollection.GetCommandlineTextLong(e_active.SelectedIndices[i]);
                }
            }
            string active_directory = string.Empty;
            if (e_active.ItemCollection is DirectoryList)
            {
                active_directory = ((DirectoryList)e_active.ItemCollection).DirectoryPath;
            }
            string passive_directory = string.Empty;
            if (e_passive.ItemCollection is DirectoryList)
            {
                passive_directory = ((DirectoryList)e_passive.ItemCollection).DirectoryPath;
            }
            Form main_form = Program.MainWindow;
            
            //and set event arg props
            e.DirectoryActive = active_directory;
            e.DirectoryPassive = passive_directory;
            e.MainPluginMenuItem = parent_plugin_menu;
            e.MainWindow = main_form;
            e.SelectedEntries = selected_files;
        }

        private void on_query_active_panel(QueryPanelInfoEventArgs e)
        {
            if (QueryActivePanel == null)
            {
                return;
            }

            QueryActivePanel(this, e);
        }

        private void on_query_passive_panel(QueryPanelInfoEventArgs e)
        {
            if (QueryPassivePanel == null)
            {
                return;
            }

            QueryPassivePanel(this, e);
        }

    }


}
