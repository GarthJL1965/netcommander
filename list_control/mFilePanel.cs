using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using System.IO;

namespace netCommander.winControls
{
    public partial class mFilePanel : UserControl
    {

        #region init & dispose
        public mFilePanel()
        {
            InitializeComponent();

            inner_ListControl.FocusedIndexChanged += new EventHandler(inner_ListControl_FocusedIndexChanged);
            inner_ListControl.FetchItemsCount += new FetchItemsCountEventHandler(inner_ListControl_FetchItemsCount);
            inner_ListControl.FetchDisplayText += new FetchDisplayTextEventHandler(inner_ListControl_FetchDisplayText);
            inner_InfoPanel.FetchInfoToDislay += new FetchInfoToDislayEventHandler(inner_InfoPanel_FetchInfoToDislay);
            inner_ListControl.MouseClick += new MouseEventHandler(inner_ListControl_MouseClick);
            inner_ListControl.DoubleClick += new EventHandler(inner_ListControl_DoubleClick);
            inner_ListControl.MouseDown += new MouseEventHandler(inner_ListControl_DragMouseDown);
            inner_ListControl.MouseMove+=new MouseEventHandler(inner_ListControl_DragMouseMove);
            inner_ListControl.MouseUp += new MouseEventHandler(inner_ListControl_DragMouseUp);
            inner_ListControl.GiveFeedback += new GiveFeedbackEventHandler(inner_ListControl_GiveFeedback);
            inner_ListControl.DragOver += new DragEventHandler(inner_ListControl_DragOver);
            inner_ListControl.DragEnter += new DragEventHandler(inner_ListControl_DragEnter);
            inner_ListControl.QueryContinueDrag += new QueryContinueDragEventHandler(inner_ListControl_QueryContinueDrag);
            inner_ListControl.DragDrop += new DragEventHandler(inner_ListControl_DragDrop);
            inner_ListControl.DragLeave += new EventHandler(inner_ListControl_DragLeave);
        }

        //private Color color_alt = Options.ForeColorAlt;
        private string preferred_focus_name = string.Empty;
        #endregion

        #region source, list control, infoPanel interaction

        protected override void OnFontChanged(EventArgs e)
        {
            //inner_InfoPanel.Font = Font;
            //inner_ListControl.Font = Font;

            base.OnFontChanged(e);
        }

        public int[] SelectionIndices
        {
            get
            {
                return inner_ListControl.GetSelectionIndices();
            }
        }

        public int FocusedIndex
        {
            get
            {
                return inner_ListControl.FocusedIndex;
            }
            set
            {
                inner_ListControl.FocusedIndex = value;
                inner_ListControl.ScrollToFocusVisible();
            }
        }

        public event EventHandler StatusTextChanged;
        private void OnStatusTextChanged()
        {
            if (StatusTextChanged != null)
            {
                StatusTextChanged(this, new EventArgs());
            }
        }

        public event EventHandler SourceChanged;
        private void OnSourceChanged()
        {
            if (SourceChanged != null)
            {
                SourceChanged(this, new EventArgs());
            }
        }

        void inner_ListControl_FocusedIndexChanged(object sender, EventArgs e)
        {
            inner_InfoPanel.RefreshSafe();
        }

        void inner_InfoPanel_FetchInfoToDislay(object sender, FetchInfoToDislayEventArgs e)
        {
            if (internal_source == null)
            {
                e.DisplayText = string.Empty;
            }
            else
            {
                switch (e.LineIndex)
                {
                    case 0:
                        e.DisplayText =
                            inner_ListControl.FocusedIndex >= internal_source.ItemCount ?
                            string.Empty :
                            internal_source.GetItemDisplayNameLong(inner_ListControl.FocusedIndex);
                        e.DisplayTrimming = StringTrimming.EllipsisCharacter;
                        e.Alignment = StringAlignment.Near;
                        break;

                    case 1:
                        e.DisplayText =
                            inner_ListControl.FocusedIndex >= internal_source.ItemCount ?
                            string.Empty :
                            internal_source.GetItemDisplaySummaryInfo(inner_ListControl.FocusedIndex);
                        e.DisplayTrimming = StringTrimming.EllipsisCharacter;
                        e.Alignment = StringAlignment.Near;
                        break;

                    case 2:
                        if (inner_ListControl.GetSelectionCount() != 0)
                        {
                            e.DisplayText = internal_source.GetSummaryInfo(inner_ListControl.GetSelectionIndices());
                        }
                        else
                        {
                            e.DisplayText = internal_source.GetSummaryInfo();
                        }
                        e.DisplayTrimming = StringTrimming.EllipsisCharacter;
                        e.Alignment = StringAlignment.Near;
                        break;
                }
            }
            e.Colors = Options.GetItemColors(string.Empty, ItemState.None, ItemCategory.Default);
        }

        void inner_ListControl_FetchDisplayText(object sender, FetchDisplayTextEventArgs e)
        {
            if (internal_source == null)
            {
                e.DisplayText = string.Empty;
            }
            else if (e.ItemIndex >= internal_source.ItemCount)
            {
                e.DisplayText = string.Empty;
            }
            else
            {
                e.DisplayText = e.ItemIndex == -1 ? string.Empty : internal_source.GetItemDisplayName(e.ItemIndex);
                //if (internal_source.GetItemIsContainer(e.ItemIndex))
                //{
                    //if container, use alt text color
                    //e.UseAltColors = true;
                    //e.AltTextColor = color_alt;
                //}
            }
            e.Colors = Options.GetItemColors
                (e.DisplayText,
                e.State,
                internal_source == null ? ItemCategory.Default : internal_source.GetItemCategory(e.ItemIndex));
        }

        void inner_ListControl_FetchItemsCount(object sender, FetchItemsCountEventArgs e)
        {
            if (internal_source == null)
            {
                e.ItemsCount = 0;
            }
            else
            {
                e.ItemsCount = internal_source.ItemCount;
            }
        }

        private FileCollectionBase internal_source = null;
        //private FileCollectionBase internal_source_old = null;

        public FileCollectionBase Source
        {
            get
            {
                return internal_source;
            }
            set
            {
                //save current source
                
                //internal_source_old = internal_source;
                if (internal_source != null)
                {
                    internal_source.Dispose();
                }

                internal_source = value;

                if (value != null)
                {
                    internal_source.AfterRefill += new EventHandler(internal_source_AfterRefill);
                    internal_source.AfterSort += new EventHandler(internal_source_AfterRefill);
                    internal_source.ItemInsert += new ItemUpdateEventHandler(internal_source_ItemInsert);
                    internal_source.ItemRemove += new ItemUpdateEventHandler(internal_source_ItemRemove);
                    internal_source.ItemUpdate += new ItemUpdateEventHandler(internal_source_ItemUpdate);
                    internal_source.AfterSummaryUpdate += new EventHandler(internal_source_AfterSummaryUpdate);
                    //internal_source.RefillException += new EventHandler(internal_source_RefillException);

                    inner_ListControl.ClearSelection(false);

                    foreach (PanelCommandBase cmd in internal_source.AvailableCommands)
                    {
                        cmd.QueryCurrentPanel += new QueryPanelInfoEventHandler(cmd_QueryCurrentPanel);
                        cmd.ItemProcessDone += new ItemEventHandler(cmd_ItemProcessDone);
                        cmd.SetNewSource += new SetNewSourceEventHandler(cmd_SetNewSource);
                    }

                    OnSourceChanged();
                }
            }
        }

        void cmd_SetNewSource(object sender, SetNewSourceEventArgs e)
        {
            if (e.NewSource == null)
            {
                return;
            }

            Source = e.NewSource;
            preferred_focus_name = e.PreferredFocusText;

            if (e.ForceRefillLater)
            {
                Source.Refill();
            }
            else
            {
                //update without refill
                inner_ListControl.UpdateEnable = false;

                //we need update FocusedIndex
                if (e.NewSource.ItemCount == 0)
                {
                    inner_ListControl.FocusedIndex = -1;
                }
                else
                {
                    //preferred_focus_name we set upper
                    if (preferred_focus_name == string.Empty)
                    {
                        inner_ListControl.FocusedIndex = 0;
                    }
                    else
                    {
                        int preferred_focus = Source.FindIndexOfName(preferred_focus_name);
                        if (preferred_focus == -1)
                        {
                            inner_ListControl.FocusedIndex = 0;
                        }
                        else
                        {
                            inner_ListControl.FocusedIndex = preferred_focus;
                        }
                    }
                }

                inner_ListControl.UpdateEnable = true;

                inner_ListControl.UpdateAllItems(true);
                inner_InfoPanel.RefreshSafe();

                inner_ListControl.ScrollToFocusVisible();

                OnStatusTextChanged();
            }
        }

        void cmd_ItemProcessDone(object sender, ItemEventArs e)
        {
            if (e.Index == -1)
            {
                //find by name
                if (e.ItemText == string.Empty)
                {
                    return;
                }
                int ind = Source.FindIndexOfName(e.ItemText);
                if (ind == -1)
                {
                    return;
                }
                inner_ListControl.SetItemSelection(ind, false);
            }
            else
            {
                //use index e.Index
                inner_ListControl.SetItemSelection(e.Index, false);
            }
        }

        void cmd_QueryCurrentPanel(object sender, QueryPanelInfoEventArgs e)
        {
            e.FocusedIndex = inner_ListControl.FocusedIndex;
            e.ItemCollection = Source;
            e.SelectedIndices = inner_ListControl.GetSelectionIndices();
        }

        //void internal_source_RefillException(object sender, EventArgs e)
        //{
            //exception while refilling source
            //try to load old source
            //Source = internal_source_old;
            //inner_ListControl.UpdateAllItems(true);
            //inner_InfoPanel.RefreshSafe();
        //}

        void internal_source_AfterSummaryUpdate(object sender, EventArgs e)
        {
            inner_InfoPanel.RefreshSafe();
        }

        void internal_source_ItemUpdate(object sender, ItemUpdateEventArgs e)
        {
            if (e.Index == inner_ListControl.FocusedIndex)
            {
                inner_InfoPanel.RefreshSafe();
            }

            inner_ListControl.UpdateItem(e.Index);
        }

        void internal_source_ItemRemove(object sender, ItemUpdateEventArgs e)
        {
            inner_ListControl.RemoveItem(e.Index);
        }

        void internal_source_ItemInsert(object sender, ItemUpdateEventArgs e)
        {
            inner_ListControl.InsertItem(e.Index);
        }

        void internal_source_AfterRefill(object sender, EventArgs e)
        {
            inner_ListControl.UpdateEnable = false;

            //we need update FocusedIndex
            FileCollectionBase src = (FileCollectionBase)sender;
            if (src.ItemCount == 0)
            {
                inner_ListControl.FocusedIndex = -1;
            }
            else
            {
                //preferred_focus_name we get at GetChildCollection call
                if (preferred_focus_name == string.Empty)
                {
                    inner_ListControl.FocusedIndex = 0;

                    // need DEBUG
                    // иначе при запуске программы курсор получается за пределами окна
                    inner_ListControl.ScrollToFocusVisible();
                    //
                }
                else
                {
                    int preferred_focus = src.FindIndexOfName(preferred_focus_name);
                    if (preferred_focus == -1)
                    {
                        inner_ListControl.FocusedIndex = 0;
                    }
                    else
                    {
                        inner_ListControl.FocusedIndex = preferred_focus;
                    }
                }
            }

            //added later
            inner_ListControl.ClearSelection();

            inner_ListControl.UpdateEnable = true;

            inner_ListControl.UpdateAllItems(true);
            inner_InfoPanel.RefreshSafe();

            inner_ListControl.ScrollToFocusVisible();

            OnStatusTextChanged();
        }
        #endregion

        #region keyboard handling

        private void focus_down()
        {
            if (Source == null)
            {
                return;
            }

            int desired_index = inner_ListControl.FocusedIndex + 1;
            if ((desired_index >= Source.ItemCount) || (desired_index < 0))
            {
                return;
            }

            inner_ListControl.FocusedIndex = desired_index;
            inner_ListControl.ScrollToFocusVisible();
        }

        private void focus_up()
        {
            if (Source == null)
            {
                return;
            }

            int desired_index = inner_ListControl.FocusedIndex - 1;
            if ((desired_index < 0) || (desired_index >= Source.ItemCount))
            {
                return;
            }

            inner_ListControl.FocusedIndex = desired_index;
            inner_ListControl.ScrollToFocusVisible();
        }

        private void focus_right()
        {
            if (Source == null)
            {
                return;
            }

            if (inner_ListControl.ColumnsCount == 1)
            {
                return;
            }

            int desired_index = inner_ListControl.FocusedIndex + inner_ListControl.ItemsPerColumn;
            if (desired_index < 0)
            {
                desired_index = 0;
            }
            if (desired_index > Source.ItemCount - 1)
            {
                desired_index = Source.ItemCount - 1;
            }

            inner_ListControl.FocusedIndex = desired_index;
            if (!inner_ListControl.IsItemVisible(desired_index))
            {
                inner_ListControl.FirstVisibleItem = inner_ListControl.FirstVisibleItem + inner_ListControl.ItemsPerColumn;
            }
        }

        private void focus_left()
        {
            if (Source == null)
            {
                return;
            }

            if (inner_ListControl.ColumnsCount == 1)
            {
                return;
            }

            int desired_index = inner_ListControl.FocusedIndex - inner_ListControl.ItemsPerColumn;
            if (desired_index < 0)
            {
                desired_index = 0;
            }
            if (desired_index > Source.ItemCount - 1)
            {
                desired_index = Source.ItemCount - 1;
            }

            inner_ListControl.FocusedIndex = desired_index;
            if (!inner_ListControl.IsItemVisible(desired_index))
            {
                inner_ListControl.FirstVisibleItem = inner_ListControl.FirstVisibleItem - inner_ListControl.ItemsPerColumn;
            }
        }

        private void focus_page_down()
        {
            if (Source == null)
            {
                return;
            }

            int desired_index = inner_ListControl.FocusedIndex + inner_ListControl.MaximumVisibleItems;
            if (desired_index < 0)
            {
                desired_index = 0;
            }
            if (desired_index > Source.ItemCount - 1)
            {
                desired_index = Source.ItemCount - 1;
            }

            if (!inner_ListControl.IsItemVisible(desired_index))
            {
                inner_ListControl.FirstVisibleItem = inner_ListControl.LastVisibleItem;
                if (!inner_ListControl.IsItemVisible(desired_index))
                {
                    desired_index = inner_ListControl.LastVisibleItem;
                }
            }

            inner_ListControl.FocusedIndex = desired_index;
        }

        private void focus_page_up()
        {
            if (Source == null)
            {
                return;
            }

            int desired_index = inner_ListControl.FocusedIndex - inner_ListControl.MaximumVisibleItems;
            if (desired_index < 0)
            {
                desired_index = 0;
            }
            if (desired_index > Source.ItemCount - 1)
            {
                desired_index = Source.ItemCount - 1;
            }

            if (!inner_ListControl.IsItemVisible(desired_index))
            {
                int to_first = inner_ListControl.FirstVisibleItem - inner_ListControl.MaximumVisibleItems + 1;
                if (to_first < 0)
                {
                    to_first = 0;
                }
                inner_ListControl.FirstVisibleItem = to_first;
                if (!inner_ListControl.IsItemVisible(desired_index))
                {
                    desired_index = to_first;
                }
            }

            inner_ListControl.FocusedIndex = desired_index;
        }

        private void focus_home()
        {
            if (Source == null)
            {
                return;
            }

            inner_ListControl.FocusedIndex = 0;
            inner_ListControl.ScrollToFocusVisible();
        }

        private void focus_end()
        {
            if (Source == null)
            {
                return;
            }

            inner_ListControl.FocusedIndex = Source.ItemCount - 1;
            inner_ListControl.ScrollToFocusVisible();
        }

        private bool action_enter_shift()
        {
            if (Source == null)
            {
                return true;
            }

            if (!Source.GetItemIsContainer(FocusedIndex))
            {
                Source.DefautlAction(FocusedIndex, true);
                return true;
            }

            return false;
        }

        private bool action_enter()
        {
            if (Source == null)
            {
                return true;
            }

            //if not container, do default action of Source
            if (!Source.GetItemIsContainer(inner_ListControl.FocusedIndex))
            {
                Source.DefautlAction(FocusedIndex, false);
                return true;
            }

            //try to get child collection
            //FileCollectionBase new_source = Source.GetChildCollection(inner_ListControl.FocusedIndex,ref preferred_focus_name);
            inner_ListControl.ClearSelection(false);
            FileCollectionBase new_source = null;
            bool use_new=false;
            Source.GetChildCollection(inner_ListControl.FocusedIndex, ref new_source, ref use_new, ref preferred_focus_name);

            if (use_new)
            {
                //refilling MUST be already done in GetChildCollection
                //source completely new
                if (new_source == null)
                {
                    return false;
                }
                //change Source
                Source = new_source;
                internal_source_AfterRefill(Source, new EventArgs());
            }
            

            //if use_new==false
            //Refill() must be call from FileCollectionBase
            //Source.Refill();

            //return true - supress subsequent key processing
            return true;
        }

        private void action_insert()
        {
            //that is change selection state

            if (Source == null)
            {
                return;
            }

            if (!Source.GetItemSelectEnable(inner_ListControl.FocusedIndex))
            {
                return;
            }

            inner_ListControl.InvertItemSelection(inner_ListControl.FocusedIndex);

            if (Source.ItemCount - 1 != inner_ListControl.FocusedIndex)
            {
                //not last item
                focus_down();
            }
            else
            {
                //force update info panel
                inner_InfoPanel.RefreshSafe();
            }
        }

        private void action_ctrl_a()
        {
            //select all without containers
            if (Source == null)
            {
                return;
            }

            //clear current selection
            inner_ListControl.ClearSelection(true);

            for (int i = 0; i < Source.ItemCount; i++)
            {
                if ((Source.GetItemSelectEnable(i)) && (!Source.GetItemIsContainer(i)))
                {
                    inner_ListControl.SetItemSelection(i, true);
                }
            }

            inner_InfoPanel.RefreshSafe();
        }

        private void action_ctrl_shift_a()
        {
            //select all include conrainers
            if (Source == null)
            {
                return;
            }

            //clear current selection
            inner_ListControl.ClearSelection(true);

            for (int i = 0; i < Source.ItemCount; i++)
            {
                if (Source.GetItemSelectEnable(i))
                {
                    inner_ListControl.SetItemSelection(i, true);
                }
            }

            inner_InfoPanel.RefreshSafe();
        }

        private void action_multiply()
        {
            //invert selection
            if (Source == null)
            {
                return;
            }

            bool sel_state = false;
            for (int i = 0; i < Source.ItemCount; i++)
            {
                if (Source.GetItemSelectEnable(i))
                {
                    sel_state = (inner_ListControl.GetItemState(i) & ItemState.Selected) == ItemState.Selected;
                    inner_ListControl.SetItemSelection(i, !sel_state);
                }
            }

            inner_InfoPanel.RefreshSafe();
        }

        private void action_add()
        {
            //select on filter
            if (Source == null)
            {
                return;
            }

            int[] new_sel = Source.FindItems();
            if (new_sel.Length > 0)
            {
                inner_ListControl.ClearSelection(true);

                for (int i = 0; i < new_sel.Length; i++)
                {
                    if (Source.GetItemSelectEnable(new_sel[i]))
                    {
                        inner_ListControl.SetItemSelection(new_sel[i], true);
                    }
                }
            }

            inner_InfoPanel.RefreshSafe();

        }

        protected override bool ProcessDialogKey(Keys keyData)
        {
            switch (keyData)
            {
                case Keys.Down:
                    focus_down();
                    return true;

                case Keys.Up:
                    focus_up();
                    return true;

                case Keys.Right:
                    focus_right();
                    return true;

                case Keys.Left:
                    focus_left();
                    return true;

                case Keys.PageDown:
                    focus_page_down();
                    return true;

                case Keys.PageUp:
                    focus_page_up();
                    return true;

                case Keys.Home:
                    focus_home();
                    return true;

                case Keys.End:
                    focus_end();
                    return true;

                case Keys.Enter|Keys.Shift:
                    return action_enter_shift();

                case Keys.Enter:
                    return action_enter();

                case Keys.Insert:
                    action_insert();
                    return true;

                case Keys.A | Keys.Control:
                    action_ctrl_a();
                    return true;

                case Keys.A | Keys.Control | Keys.Shift:
                    action_ctrl_shift_a();
                    return true;

                case Keys.Multiply:
                    action_multiply();
                    return true;

                case Keys.Add:
                    action_add();
                    return true;
            }

            return base.ProcessDialogKey(keyData);
        }


        #endregion

        #region context menu & mice handling
        private Peter.ShellContextMenu context_menu = new Peter.ShellContextMenu();
        void inner_ListControl_MouseClick(object sender, MouseEventArgs e)
        {
            
            if (e.Button == MouseButtons.Left)
            {
                return;
            }
            else if (e.Button == MouseButtons.Right)
            {
                //check source, process only DirectoryList
                DirectoryList dl = null;
                if (!(Source is DirectoryList))
                {
                    return;
                }


                int index = inner_ListControl.GetItemAtPoint(e.Location);
                if (index < 0)
                {
                    return;
                }

                try
                {
                    dl = (DirectoryList)Source;
                    FileInfoEx info = dl[index];
                    if (info.Directory)
                    {
                        DirectoryInfo dir_info = new DirectoryInfo(info.FullName);
                        context_menu.ShowContextMenu(new DirectoryInfo[] { dir_info }, PointToScreen(e.Location));
                    }
                    else
                    {
                        FileInfo file_info = new FileInfo(info.FullName);
                        context_menu.ShowContextMenu(new FileInfo[] { file_info }, PointToScreen(e.Location));
                    }
                }
                catch (Exception ex)
                {
                    Messages.ShowException(ex);
                }
            }//end right button
        }

        void inner_ListControl_DoubleClick(object sender, EventArgs e)
        {
            //treat as <ENTER> press
            action_enter();
        }
        #endregion

        #region drag-n-drop handling
        Size drag_size = SystemInformation.DragSize;
        Point drag_point = Point.Empty;
        int drag_item_index = -1;
        bool drag_state = false;
        bool drag_accept = false;

        void inner_ListControl_DragMouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button != MouseButtons.Left)
            {
                return;
            }

            drag_point = e.Location;
            drag_item_index = inner_ListControl.GetItemAtPoint(e.Location);
        }

        void inner_ListControl_DragMouseUp(object sender, MouseEventArgs e)
        {
            //reset drag_point
            drag_point = Point.Empty;
            drag_item_index = -1;
            drag_state = false;
        }

        private void inner_ListControl_DragMouseMove(object sender, MouseEventArgs e)
        {
            //Program.MainWindow.Text = e.Location.ToString() + " " + drag_point.ToString() + " " + drag_item_index.ToString();

            if (drag_state)
            {
                return;
            }

            if (!check_drag_size(e.Location))
            {
                return;
            }

            if (drag_item_index == -1)
            {
                return;
            }

            drag_state = true;

            object drag_object = Source.GetDragdropObject(drag_item_index);
            if (drag_object == null)
            {
                return;
            }
            else
            {
                inner_ListControl.DoDragDrop(drag_object, Source.GetDragdropEffects(drag_item_index));
            }
        }

        void inner_ListControl_GiveFeedback(object sender, GiveFeedbackEventArgs e)
        {
            
        }

        void inner_ListControl_DragLeave(object sender, EventArgs e)
        {
            drag_state = false;
            drag_point = Point.Empty;
            drag_item_index = -1;
            drag_accept = false;
        }

        void inner_ListControl_DragOver(object sender, DragEventArgs e)
        {
            if (drag_accept)
            {
                Source.DragdropOverFeedback(e);
            }
        }

        void inner_ListControl_DragEnter(object sender, DragEventArgs e)
        {
            Source.DragdropEnterFeedback(e, out drag_accept);
            if (drag_accept)
            {
                drag_state = true;
            }

            //string[] formats = e.Data.GetFormats();
            //object[] datas = new object[formats.Length];
            //for (int i = 0; i < formats.Length; i++)
            //{
            //    datas[i] = e.Data.GetData(formats[i]);
            //}
            //int j = 0;
        }

        void inner_ListControl_DragDrop(object sender, DragEventArgs e)
        {
            if (!drag_state)
            {
                return;
            }
            
            drag_state = false;
            drag_point = Point.Empty;
            drag_item_index = -1;
            drag_accept = false;
            Source.DragdropDo(e);
        }

        void inner_ListControl_QueryContinueDrag(object sender, QueryContinueDragEventArgs e)
        {
            if (e.EscapePressed)
            {
                drag_state = false;
                e.Action = DragAction.Cancel;
            }
        }

        private bool check_drag_size(Point current_cursor_location)
        {
            if (drag_point == Point.Empty)
            {
                return false;
            }

            int delta = Math.Abs(current_cursor_location.X - drag_point.X);
            if (delta > drag_size.Width)
            {
                return true;
            }
            delta = Math.Abs(current_cursor_location.Y - drag_point.Y);
            if (delta > drag_size.Height)
            {
                return true;
            }
            return false;
        }
        #endregion

       

        
    }
}
