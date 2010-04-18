using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;

namespace netCommander
{
    public abstract class PanelCommandBase
    {
        public event QueryPanelInfoEventHandler QueryCurrentPanel;
        public event QueryPanelInfoEventHandler QueryOtherPanel;
        public event ItemEventHandler ItemProcessDone;
        public event SetNewSourceEventHandler SetNewSource;

        protected abstract void internal_command_proc();

        private ToolStripMenuItem internal_menu_item = null;
        public ToolStripMenuItem CommandMenu
        {
            get
            {
                return internal_menu_item;
            }
            set
            {
                internal_menu_item = value;
                internal_menu_item.Click += new EventHandler(internal_menu_item_Click);
            }
        }

        private void internal_menu_item_Click(object sender, EventArgs e)
        {
            internal_command_proc();
        }

        protected void OnQueryCurrentPanel(QueryPanelInfoEventArgs e)
        {
            if (QueryCurrentPanel != null)
            {
                QueryCurrentPanel(this, e);
            }
        }

        protected void OnQueryOtherPanel(QueryPanelInfoEventArgs e)
        {
            if (QueryOtherPanel != null)
            {
                QueryOtherPanel(this, e);
            }
        }

        protected void OnItemProcessDone(ItemEventArs e)
        {
            if (ItemProcessDone != null)
            {
                ItemProcessDone(this, e);
            }
        }

        protected void OnSetNewSource(SetNewSourceEventArgs e)
        {
            if (SetNewSource != null)
            {
                SetNewSource(this, e);
            }
        }
    }

    public delegate void ItemEventHandler(object sender, ItemEventArs e);
    public class ItemEventArs : EventArgs
    {
        public int Index { get; private set; }
        public string ItemText { get; private set; }

        public ItemEventArs(int index)
        {
            Index = index;
            ItemText = string.Empty;
        }

        public ItemEventArs(string item_text)
        {
            Index = -1;
            ItemText = item_text;
        }

        public ItemEventArs(int index, string item_text)
        {
            Index = index;
            ItemText = item_text;
        }
    }

    public delegate void QueryPanelInfoEventHandler(object sender, QueryPanelInfoEventArgs e);
    public class QueryPanelInfoEventArgs : EventArgs
    {
        public QueryPanelInfoEventArgs()
        {
            ItemCollection = null;
            FocusedIndex = -1;
            SelectedIndices = new int[] { };
        }

        public FileCollectionBase ItemCollection { get; set; }
        public int FocusedIndex { get; set; }
        public int[] SelectedIndices { get; set; }
    }

    public delegate void SetNewSourceEventHandler(object sender, SetNewSourceEventArgs e);
    public class SetNewSourceEventArgs : EventArgs
    {
        public SetNewSourceEventArgs
            (FileCollectionBase new_source, bool force_refill_later, string prefrred_focus_text)
        {
            NewSource = new_source;
            ForceRefillLater = force_refill_later;
            PreferredFocusText = prefrred_focus_text;
        }

        public FileCollectionBase NewSource { get; private set; }
        public bool ForceRefillLater { get; private set; }
        public string PreferredFocusText { get; private set; }
    }
}
