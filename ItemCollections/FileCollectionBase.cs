using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;

namespace netCommander
{
    public abstract class FileCollectionBase : IDisposable
    {
        protected abstract void internal_dispose();
        protected abstract void internal_sort(int criteria_index, bool reverse_order);
        protected abstract void internal_refill();
        public abstract string[] SortCriteriaAvailable { get; }
        public event EventHandler AfterSummaryUpdate;
        public event EventHandler AfterSort;
        public event EventHandler AfterRefill;
        public event ItemUpdateEventHandler ItemUpdate;
        public event ItemUpdateEventHandler ItemRemove;
        public event ItemUpdateEventHandler ItemInsert;
        //public event EventHandler RefillException;
        public abstract string GetItemDisplayName(int index);
        public abstract string GetItemDisplayNameLong(int index);
        public abstract string GetItemDisplaySummaryInfo(int index);
        public abstract string GetSummaryInfo();
        public abstract string GetSummaryInfo(int[] indices);
        public abstract int ItemCount { get; }
        public abstract bool GetItemSelectEnable(int index);
        public abstract bool GetItemIsContainer(int index);
        /// <summary>
        /// create child collection. will be invoke on <enter> on container items
        /// </summary>
        /// <param name="index">target item index</param>
        /// <param name="new_collection">set if new FileCollectionBase needed</param>
        /// <param name="use_new">must use new_collection, else use current collection</param>
        /// <param name="preferred_focused_text">text to set caret</param>
        public abstract void GetChildCollection(int index, ref FileCollectionBase new_collection, ref bool use_new,ref string preferred_focused_text);
        public abstract int FindIndexOfName(string name);
        public abstract int[] FindItems();
        public abstract string GetStatusText();
        public abstract string GetCommandlineTextShort(int index);
        public abstract string GetCommandlineTextLong(int index);

        private List<PanelCommandBase> available_commands = new List<PanelCommandBase>();
        private PanelCommandAddCommandlineShort command_add_short = new PanelCommandAddCommandlineShort();
        private PanelCommandAddCommandlineLong command_add_long = new PanelCommandAddCommandlineLong();

        public FileCollectionBase(int sort_criteria_index, bool sort_reverse)
        {
            sort_current = sort_criteria_index;
            this.sort_reverse = sort_reverse;

            AvailableCommands.Add(command_add_short);
            AvailableCommands.Add(command_add_long);
        }

        #region dragdrop stubs
        public virtual IDataObject GetDragdropObject(int index)
        {
            return null;
        }

        public virtual DragDropEffects GetDragdropEffects(int index)
        {
            return DragDropEffects.All;
        }

        public virtual void DragdropEnterFeedback(DragEventArgs e, out bool accept_dragdrop)
        {
            accept_dragdrop = false;
            e.Effect = DragDropEffects.None;
        }

        public virtual void DragdropOverFeedback(DragEventArgs e)
        {
            e.Effect = DragDropEffects.None;
        }

        public virtual void DragdropDo(DragEventArgs e)
        {
            return;
        }
        #endregion

        /// <summary>
        /// this is stub. must be implemented
        /// </summary>
        /// <param name="index"></param>
        /// <param name="shift"></param>
        protected virtual void DoDefaultAction(int index, bool shift)
        {
            return;
        }
        public void DefautlAction(int index, bool shift)
        {
            DoDefaultAction(index, shift);
        }

        public virtual ItemCategory GetItemCategory(int index)
        {
            return ItemCategory.Default;
        }

        public List<PanelCommandBase> AvailableCommands
        {
            get
            {
                return available_commands;
            }
        }

        public mainForm MainWindow { get; set; }

        private List<MenuItem> internal_sort_menu = null;
        public List<MenuItem> AvailableSortMenu
        {
            get
            {
                if (internal_sort_menu == null)
                {
                    internal_sort_menu = new List<MenuItem>();
                    string[] sort_modes = SortCriteriaAvailable;
                    foreach (string one_mode in sort_modes)
                    {
                        MenuItem sort_menu_item = new MenuItem(one_mode);
                        sort_menu_item.Click += new EventHandler(sort_menu_item_Click);
                        internal_sort_menu.Add(sort_menu_item);
                    }
                }
                return internal_sort_menu;
            }
        }

        void sort_menu_item_Click(object sender, EventArgs e)
        {
            try
            {
                MenuItem src_menu = (MenuItem)sender;
                int ind = internal_sort_menu.IndexOf(src_menu);
                if (ind < 0)
                {
                    return;
                }
                if (ind == SortCriteria)
                {
                    //change sort order
                    SortReverse = !SortReverse;
                }
                else
                {
                    SortCriteria = ind;
                }
            }
            catch (Exception ex)
            {
                Messages.ShowException(ex);
            }
        }

        private int sort_current;
        public int SortCriteria
        {
            get
            {
                return sort_current;
            }
            set
            {
                if (sort_current != value)
                {
                    internal_sort(value, sort_reverse);
                    sort_current = value;
                    OnAfterSort();
                }
            }
        }

        private bool sort_reverse = false;
        public bool SortReverse
        {
            get
            {
                return sort_reverse;
            }
            set
            {
                if (sort_reverse != value)
                {
                    internal_sort(sort_current, value);
                    sort_reverse = value;
                    OnAfterSort();
                }
            }
        }

        public void Refill()
        {
            internal_refill();
            OnAfterRefill();
        }

        protected void OnAfterSummaryUpdate()
        {
            if (AfterSummaryUpdate != null)
            {
                AfterSummaryUpdate(this, new EventArgs());
            }
        }

        protected void OnAfterSort()
        {
            if (AfterSort != null)
            {
                AfterSort(this, new EventArgs());
            }
        }

        protected void OnAfterRefill()
        {
            if (AfterRefill != null)
            {
                AfterRefill(this, new EventArgs());
            }
        }

        protected void OnItemUpdate(int index)
        {
            if (ItemUpdate != null)
            {
                ItemUpdate(this, new ItemUpdateEventArgs(index));
            }
        }

        protected void OnItemRemove(int index)
        {
            if (ItemRemove != null)
            {
                ItemRemove(this, new ItemUpdateEventArgs(index));
            }
        }

        protected void OnItemInsert(int index)
        {
            if (ItemInsert != null)
            {
                ItemInsert(this, new ItemUpdateEventArgs(index));
            }
        }

        protected void OnLongOperaion(string status, bool show_status_window)
        {
            if (MainWindow == null)
            {
                return;
            }

            MainWindow.NotifyLongOperation(status, show_status_window);
        }

        #region IDisposable Members

        public void Dispose()
        {
            internal_dispose();
        }

        #endregion
    }

    public class ItemUpdateEventArgs : EventArgs
    {
        public int Index { get; private set; }

        public ItemUpdateEventArgs(int index)
        {
            Index = index;
        }
    }
    public delegate void ItemUpdateEventHandler(object sender, ItemUpdateEventArgs e);
}
