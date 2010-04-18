using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;

namespace netCommander
{
    public interface IPlugin
    {
        void Init();
        event QueryHostInfoEventHandler QueryHostInfo;

        string PluginName { get; }
        string PluginDescription { get; }
        string PluginAuthor { get; }
        string PluginVersion { get; }
    }

    public delegate void QueryHostInfoEventHandler(object sender,QueryHostInfoEventArgs e);
    public class QueryHostInfoEventArgs : EventArgs
    {
        public Form MainWindow { get; set; }
        public MenuItem MainPluginMenuItem { get; set; }
        public string[] SelectedEntries { get; set; }
        public string DirectoryActive { get; set; }
        public string DirectoryPassive { get; set; }
    }
}
