using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Drawing;
using System.ComponentModel;
using System.Drawing.Text;

namespace netCommander.winControls
{
    public class mListControl : Control
    {

        public mListControl()
        {
            init_internal();
        }
        #region focus handling
        protected override void OnGotFocus(EventArgs e)
        {
            UpdateItem(FocusedIndex);
            base.OnGotFocus(e);
        }

        protected override void OnLostFocus(EventArgs e)
        {
            UpdateItem(FocusedIndex);
            base.OnLostFocus(e);
        }
        #endregion

        #region mice handling
        protected override void OnMouseClick(MouseEventArgs e)
        {
            //select item under cursor
            this.Focus();
            int item_index = GetItemAtPoint(e.Location);
            if (item_index >= 0)
            {
                FocusedIndex = item_index;
                if ((e.Button == MouseButtons.Right) || (e.Button == MouseButtons.Left))
                {
                    OnItemClickRight(new ItemClickEventArgs(item_index));
                }
            }

            

            base.OnMouseClick(e);
        }

        protected override void OnMouseDoubleClick(MouseEventArgs e)
        {
            int item_index = GetItemAtPoint(e.Location);
            if (item_index >= 0)
            {
                if(e.Button==MouseButtons.Left)
                {
                    OnItemClickDouble(new ItemClickEventArgs(item_index));
                }
            }

            base.OnMouseDoubleClick(e);
        }

        public event ItemClickEventHandler ItemClickRight;
        public event ItemClickEventHandler ItemClickDouble;

        private void OnItemClickRight(ItemClickEventArgs e)
        {
            if (ItemClickRight != null)
            {
                ItemClickRight(this, e);
            }
        }

        private void OnItemClickDouble(ItemClickEventArgs e)
        {
            if (ItemClickDouble != null)
            {
                ItemClickDouble(this, e);
            }
        }

        #endregion

        #region init & exiting

        private void init_internal()
        {
            SetStyle
                (ControlStyles.AllPaintingInWmPaint | ControlStyles.OptimizedDoubleBuffer | ControlStyles.Selectable | ControlStyles.UserPaint,
                true);
            
            string_format.Alignment = StringAlignment.Near;
            string_format.FormatFlags = StringFormatFlags.LineLimit;
            string_format.LineAlignment = StringAlignment.Center;
            string_format.Trimming = StringTrimming.EllipsisPath;

            //init scroll bars
            scroll_hor = new HScrollBar();
            scroll_hor.Dock = DockStyle.Bottom;
            scroll_hor.Visible = false;
            scroll_hor.Scroll += new ScrollEventHandler(scroll_Scroll);

            scroll_vert = new VScrollBar();
            scroll_vert.Dock = DockStyle.Right;
            scroll_vert.Visible = false;
            scroll_vert.Scroll+=new ScrollEventHandler(scroll_Scroll);

            Controls.Add(scroll_hor);
            Controls.Add(scroll_vert);

            UpdateEnable = true;
        }

        protected override void OnCreateControl()
        {
            
            update_item_height();
            update_cell_rects();
            Invalidate(ClientRectangle);

            base.OnCreateControl();
        }

        protected override void Dispose(bool disposing)
        {
            if (scroll_hor != null)
            {
                scroll_hor.Dispose();
            }
            if (scroll_vert != null)
            {
                scroll_vert.Dispose();
            }

            //brush_background.Dispose();
            //brush_background_focused.Dispose();
            //brush_foreground.Dispose();
            //brush_foreground_selected.Dispose();
            brushes.Dispose();

            base.Dispose(disposing);
        }

        #endregion

        #region scrolling
        private VScrollBar scroll_vert = null;
        private HScrollBar scroll_hor = null;

        private void update_scroll_properties()
        {
            int page_count = get_page_count();
            if (page_count > 1)
            {
                int current_page = get_current_scroll_page();
                if (columns_count == 1)
                {
                    work_area_rectangle = new Rectangle
                    (new Point(0, 0),
                    new Size(ClientSize.Width - scroll_vert.Width, ClientSize.Height));
                    page_count = get_page_count();
                    scroll_vert.Minimum = 0;
                    scroll_vert.Maximum = page_count - 1;
                    scroll_vert.Value = current_page;
                    scroll_vert.LargeChange = 1;
                    scroll_vert.SmallChange = 1;
                    scroll_hor.Visible = false;
                    scroll_vert.Visible = true;
                    
                }
                else
                {
                    work_area_rectangle = new Rectangle
                    (new Point(0, 0),
                    new Size(ClientSize.Width, ClientSize.Height - scroll_hor.Height));
                    page_count = get_page_count();
                    scroll_hor.Minimum = 0;
                    scroll_hor.Maximum = page_count - 1;


                    //workaround
                    //sometimes current_page greater then scroll_hor.Maximum
                    if (current_page <= scroll_hor.Maximum)
                    {
                        scroll_hor.Value = current_page;
                    }
                    else
                    {
                        /* need DEBUG */
                        scroll_hor.Value = 0;
                    }
                    
                    scroll_hor.LargeChange = 1;
                    scroll_hor.SmallChange = 1;
                    scroll_vert.Visible = false;
                    scroll_hor.Visible = true;
                    
                }
            }
            else
            {
                //no scroll needed
                scroll_hor.Visible = false;
                scroll_vert.Visible = false;
                work_area_rectangle = ClientRectangle;
            }
        }

        private int get_page_count()
        {
            int ret = 0;
            int item_count = get_items_count();
            int cells_in_page = columns_count * (int)Math.Floor((float)work_area_rectangle.Height / (float)item_height);
            ret = (int)Math.Ceiling((float)item_count / (float)cells_in_page);
            return ret;
        }

        private int get_current_scroll_page()
        {
            int item_count = get_items_count();
            int cells_in_page = columns_count * (int)Math.Floor((float)work_area_rectangle.Height / (float)item_height);
            return (int)Math.Floor((float)first_visible_index / (float)cells_in_page);
        }

        void scroll_Scroll(object sender, ScrollEventArgs e)
        {
            ScrollToPage(e.NewValue);
        }

        public void ScrollToPage(int page)
        {
            int item_count = get_items_count();
            int page_count = get_page_count();
            int to_page = page;
            if (to_page > (page_count-1))
            {
                to_page = page_count - 1;
            }

            int cells_in_page = columns_count * (int)Math.Floor((float)work_area_rectangle.Height / (float)item_height);
            int to_item = to_page * cells_in_page;
            if (to_item > (item_count - 1))
            {
                to_item = item_count - 1;
            }

            first_visible_index = to_item;
            UpdateAllItems();
        }

        public void ScrollToFocusVisible()
        {
            ScrollToItem(FocusedIndex);
        }

        public void ScrollToItem(int item_index)
        {
            if (item_index_2_cell_index(item_index) == -1)
            {
                //really not visible
                if (FocusedIndex < first_visible_index)
                {
                    //focus under top
                    first_visible_index = item_index;
                }
                else
                {
                    //focus under bottom
                    int cells_count = columns_count * (int)Math.Floor((float)work_area_rectangle.Height / (float)item_height);
                    first_visible_index = Math.Max(0, item_index - cells_count + 1);
                }
                update_scroll_value();   
                UpdateAllItems();
            }
        }

        private void update_scroll_value()
        {
            int scroll_page = get_current_scroll_page();
            if (scroll_hor.Visible)
            {
                scroll_hor.Value = scroll_page;
            }
            if (scroll_vert.Visible)
            {
                scroll_vert.Value = scroll_page;
            }
        }

        #endregion

        #region callbacks - fetch items
        public event FetchItemsCountEventHandler FetchItemsCount;
        public event FetchDisplayTextEventHandler FetchDisplayText;

        private void OnFetchItemsCount(FetchItemsCountEventArgs e)
        {
            if (FetchItemsCount != null)
            {
                FetchItemsCount(this, e);
            }
        }
        private int get_items_count()
        {
            FetchItemsCountEventArgs e = new FetchItemsCountEventArgs();
            OnFetchItemsCount(e);
            return e.ItemsCount;
        }
        private void OnFetchDisplayInfo(FetchDisplayTextEventArgs e)
        {
            if (FetchDisplayText != null)
            {
                FetchDisplayText(this, e);
            }
        }
        #endregion

        #region selection & focus handling

        private List<int> internal_selection = new List<int>();
        private int focused_index = 0;

        public ItemState GetItemState(int item_index)
        {
            ItemState ret = ItemState.None;
            if (internal_selection.Contains(item_index))
            {
                ret = ret | ItemState.Selected;
            }
            if ((item_index == FocusedIndex) && (this.Focused))
            {
                ret = ret | ItemState.Focused;
            }
            return ret;
        }

        public int[] GetSelectionIndices()
        {
            return internal_selection.ToArray();
        }

        public int GetSelectionCount()
        {
            return internal_selection.Count;
        }

        public void SetSelectionIndices(IEnumerable<int> indices)
        {
            ClearSelection(true);
            foreach(int ind in indices)
            {
                internal_selection.Add(ind);
                UpdateItem(ind);
            }
        }

        public void ClearSelection()
        {
            ClearSelection(false);
        }

        public void ClearSelection(bool force_update)
        {
            int[] sel_clone = new int[] { };
            if (force_update)
            {
                sel_clone = GetSelectionIndices();
            }
            //foreach (int ind in sel_clone)
            //{
            //    internal_selection.Remove(ind);
            //}
            internal_selection.Clear();
            foreach (int ind in sel_clone)
            {
                UpdateItem(ind);
            }
        }

        public void SetItemSelection(int item_index,bool select)
        {
            if(select)
            {
                if(!internal_selection.Contains(item_index))
                {
                    internal_selection.Add(item_index);
                    UpdateItem(item_index);
                }
            }
            else
            {
                if(internal_selection.Contains(item_index))
                {
                    internal_selection.Remove(item_index);
                    UpdateItem(item_index);
                }
            }
        }

        public void InvertItemSelection(int item_index)
        {
            if(internal_selection.Contains(item_index))
            {
                internal_selection.Remove(item_index);
            }
            else
            {
                internal_selection.Add(item_index);
            }
            UpdateItem(item_index);
        }

        public event EventHandler FocusedIndexChanged;

        private void OnFocusedIndexChanged()
        {
            if (FocusedIndexChanged != null)
            {
                FocusedIndexChanged(this, new EventArgs());
            }
        }

        public int FocusedIndex
        {
            get
            {
                return focused_index;
            }
            set
            {
                if (value < 0)
                {
                    value = 0;
                }
                if (value == focused_index)
                {
                    return;
                }

                int old_index = focused_index;
                focused_index = value;
                UpdateItem(old_index);
                UpdateItem(focused_index);
                OnFocusedIndexChanged();
            }
        }
        #endregion

        #region publics - update list items

        //TODO обновить focused_index и internal_selection
        public void UpdateItem(int item_index)
        {
            int cell_index = item_index_2_cell_index(item_index);
            if (cell_index == -1)
            {
                return; //item is ouside of visible
            }
            Invalidate(cell_rects[cell_index]);
        }

        public void UpdateAllItems()
        {
            Invalidate(work_area_rectangle);
        }

        public void UpdateAllItems(bool force_recalculate_grid)
        {
            if (force_recalculate_grid)
            {
                update_cell_rects();
            }
            Invalidate(ClientRectangle);
        }

        public void RemoveItem(int item_index)
        {
            internal_selection.Remove(item_index);

            int[] sels_copy = GetSelectionIndices();
            foreach (int ind in sels_copy)
            {
                if (ind > item_index)
                {
                    internal_selection.Remove(ind);
                    internal_selection.Add(ind - 1);
                }
            }

            //focus:
            //what if get_item_count becomes 0?
            if (item_index <= FocusedIndex)
            {
                FocusedIndex = FocusedIndex - 1;
            }
            

            if (item_index < first_visible_index)
            {
                first_visible_index--;
                return;
            }
            if (item_index < first_visible_index + cell_rects.Count)
            {
                //removing in visible area
                //update items AFTER item_index TO first_visible_index + cell_rects.Count;
                for (int i = item_index; i < first_visible_index + cell_rects.Count - 1; i++)
                {
                    UpdateItem(i);
                }
                return;
            }
            //if item_index is greater than visible area indices -> do nothing
        }

        public void InsertItem(int inserted_index)
        {
            int[] sels_copy = GetSelectionIndices();
            foreach (int ind in sels_copy)
            {
                if (ind > inserted_index)
                {
                    internal_selection.Remove(ind);
                    internal_selection.Add(ind + 1);
                    UpdateItem(ind);
                    UpdateItem(ind + 1);
                }
            }

            if (FocusedIndex >= inserted_index)
            {
                FocusedIndex = FocusedIndex + 1;
            }

            if (inserted_index < first_visible_index)
            {
                //index is less than first visible
                first_visible_index++;
                UpdateAllItems();
                return;
            }
            if (inserted_index < first_visible_index + cell_rects.Count)
            {
                //insert in visible area
                //update items AFTER insertred item
                for(int i=inserted_index;i<first_visible_index+cell_rects.Count-1;i++)
                {
                    UpdateItem(i);
                }
                return;
            }
            //if insert_index is greater than vivble indices do nothing
        }
        #endregion

        #region sizing & font changing

        protected override void OnClientSizeChanged(EventArgs e)
        {
            update_cell_rects();
            Invalidate(ClientRectangle);
            base.OnClientSizeChanged(e);
        }

        protected override void OnFontChanged(EventArgs e)
        {
            update_item_height();
            update_cell_rects();
            Invalidate(ClientRectangle);
            base.OnFontChanged(e);
        }

        #endregion

        #region render

        public bool UpdateEnable { get; set; }

        [ReadOnly(true)]
        public int FirstVisibleItem
        {
            get
            {
                return first_visible_index;
            }
            set
            {
                if (value < 0)
                {
                    value = 0;
                }

                if (value != first_visible_index)
                {
                    first_visible_index = value;
                    update_scroll_value();
                    UpdateAllItems();
                }
            }
        }

        [ReadOnly(true)]
        public int LastVisibleItem
        {
            get
            {
                int last_item_cell = FirstVisibleItem + MaximumVisibleItems-1;
                int item_count=get_items_count();
                if (last_item_cell > item_count - 1)
                {
                    last_item_cell = item_count - 1;
                }
                return last_item_cell;
            }
        }

        [ReadOnly(true)]
        public int MaximumVisibleItems
        {
            get
            {
                return columns_count * (int)Math.Floor((float)work_area_rectangle.Height / (float)item_height);
            }
        }

        [ReadOnly(true)]
        public int ItemsPerColumn
        {
            get
            {
                return (int)Math.Floor((float)work_area_rectangle.Height / (float)item_height);
            }
        }

        [ReadOnly(true)]
        public int ColumnsCount
        {
            get
            {
                return columns_count;
            }
            set
            {
                if (value == columns_count)
                {
                    return;
                }
                if (value < 1)
                {
                    return;
                }
                columns_count = value;
                UpdateAllItems(true);
            }
        }

        public bool IsItemVisible(int item_index)
        {
            int cell_ind = item_index_2_cell_index(item_index);
            return (cell_ind >= 0);
        }

        public int GetItemAtPoint(Point pt)
        {
            int ret = -1;
            int cell_at_point = -1;
            int len = cell_rects.Count;
            for (int i = 0; i < len; i++)
            {
                Rectangle rect = cell_rects[i];
                if (rect.Contains(pt))
                {
                    cell_at_point = i;
                    break;
                }
            }
            if (cell_at_point == -1)
            {
                return ret;
            }
            ret = cell_index_2_item_index(cell_at_point);
            return ret;
        }

        public TextRenderingHint TextRenderingHint { get; set; }

        private List<Rectangle> cell_rects = new List<Rectangle>();
        private int first_visible_index = 0;
        private StringFormat string_format = new StringFormat();
        private int item_height = 16;
        private int columns_count = 2;
        private Rectangle work_area_rectangle = new Rectangle();

        //private Brush brush_background = new SolidBrush(Options.BackColor);
        //private Brush brush_foreground = new SolidBrush(Options.ForeColor);
        //private Brush brush_background_focused = new SolidBrush(Options.BackColorFocused);
        //private Brush brush_foreground_selected = new SolidBrush(Options.ForeColorSelected);
        //private Color color_focused_back = Options.BackColorFocused;
        //private Color color_selected_fore = Options.ForeColorSelected;
        //private Color color_fore = Options.ForeColor;
        private BrushCache brushes = new BrushCache();

        private void update_item_height()
        {
            Graphics g = CreateGraphics();
            item_height = (int)Math.Ceiling(Font.GetHeight(g)) + 4;
            g.Dispose();
        }

        private void update_cell_rects()
        {
            update_scroll_properties();
            int vert_cells_count = (int)Math.Floor((float)work_area_rectangle.Height / (float)item_height);

            //which size of one cell?
            Size cell_size = new Size
            ((int)Math.Floor((float)work_area_rectangle.Width / (float)columns_count),
            item_height);

            //clear cell list
            cell_rects.Clear();

            //fill with new values
            for (int i = 0; i <columns_count; i++)
            {
                for (int j = 0; j < vert_cells_count; j++)
                {
                    cell_rects.Add
                        (new Rectangle(new Point(i * cell_size.Width, j * cell_size.Height), cell_size));
                }
            }
        }

        private int cell_index_2_item_index(int cell_index)
        {
            int items_count = get_items_count();

            int ret= cell_index += first_visible_index;
            if (ret > (items_count - 1))
            {
                return -1; //no item for given cell
            }
            else
            {
                return ret;
            }
        }

        private int item_index_2_cell_index(int item_index)
        {
            if (item_index < first_visible_index)
            {
                return -1; //item is not visible
            }
            if (item_index > (first_visible_index + cell_rects.Count - 1))
            {
                return -1; //item is not visible
            }
            return item_index -= first_visible_index;
        }

        protected override void OnPaintBackground(PaintEventArgs e)
        {
            FetchDisplayTextEventArgs eArgs = new FetchDisplayTextEventArgs(-1, ItemState.None);
            OnFetchDisplayInfo(eArgs);

            Brush b_brush = brushes.GetBrush(eArgs.Colors.BackgroundColor);
            e.Graphics.FillRectangle(b_brush, e.ClipRectangle);
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            //base.OnPaint(e);

            if (FetchDisplayText==null)
            {
                return;
            }

            if (!UpdateEnable)
            {
                return;
            }

            //which items need repoint?
            for (int i = 0; i < cell_rects.Count; i++)
            {
                if (cell_rects[i].IntersectsWith(e.ClipRectangle))
                {
                    //cell [i] need repaints
                    //set render options
                    e.Graphics.TextRenderingHint = TextRenderingHint;

                    render_cell(i, e.Graphics, e.ClipRectangle);
                }
            }
        }

        protected void render_cell(int cell_index,Graphics g,Rectangle clip_rectangle)
        {
            int item_index=cell_index_2_item_index(cell_index);
            if (item_index == -1)
            {
                return; //empty cell
            }
            ItemState state = GetItemState(item_index);
            FetchDisplayTextEventArgs e = new FetchDisplayTextEventArgs(item_index, state);
            OnFetchDisplayInfo(e);
            //ColorSchemeForItem scheme = e.ColorScheme;

            Brush b_brush = brushes.GetBrush(e.Colors.BackgroundColor);
            Brush f_brush = brushes.GetBrush(e.Colors.ForegroundColor);

            //highlights focus only on focused list
            //if (((state & ItemState.Focused) == ItemState.Focused)&&(this.Focused))
            //{
            //    b_brush = brush_background_focused;
            //}
            //else
            //{
            //    b_brush = brush_background;
            //}

            //if ((state & ItemState.Selected) == ItemState.Selected)
            //{
            //    f_brush = brush_foreground_selected;
            //}
            //else
            //{
            //    if (e.UseAltColors)
            //    {
            //        f_brush = new SolidBrush(e.AltTextColor);
            //    }
            //    else
            //    {
            //        f_brush = brush_foreground;
            //    }
            //}

            g.FillRectangle(b_brush, cell_rects[cell_index]);
            g.DrawString
                (e.DisplayText,
                this.Font,
                f_brush,
                cell_rects[cell_index],
                string_format);

            //focus rectangle
            if (item_index==FocusedIndex)
            {
                ControlPaint.DrawFocusRectangle(g, cell_rects[cell_index], e.Colors.ForegroundColor, e.Colors.BackgroundColor);
                //if ((state & ItemState.Selected) == ItemState.Selected)
                //{
                //    ControlPaint.DrawFocusRectangle(g, cell_rects[cell_index], color_selected_fore, color_focused_back);
                //}
                //else
                //{
                //    if (e.UseAltColors)
                //    {
                //        ControlPaint.DrawFocusRectangle(g, cell_rects[cell_index], e.AltTextColor, color_focused_back);
                //    }
                //    else
                //    {
                //        ControlPaint.DrawFocusRectangle(g, cell_rects[cell_index], color_fore, color_focused_back);
                //    }
                //}
            }
        }

        #endregion
    }

    #region callback declare

    public delegate void FetchItemsCountEventHandler(object sender,FetchItemsCountEventArgs e);
    public class FetchItemsCountEventArgs : EventArgs
    {
        public int ItemsCount { get; set; }
    }

    public delegate void FetchDisplayTextEventHandler(object sender,FetchDisplayTextEventArgs e);
    public class FetchDisplayTextEventArgs : EventArgs
    {
        //public Color ForeColor { get; set; }
        //public Color BackColor { get; set; }
        public string DisplayText { get; set; }
        public ItemState State { get; private set; }
        public int ItemIndex { get; private set; }
        //public Color AltTextColor { get; set; }
        //public bool UseAltColors { get; set; }

        public ItemColors Colors { get; set; }

        public FetchDisplayTextEventArgs(int item_index,ItemState state)
        {
            ItemIndex = item_index;
            //UseAltColors = false;
            State = state;
            //ForeColor = SystemColors.WindowText;
            //BackColor = SystemColors.Window;
        }
    }

    public delegate void ItemClickEventHandler(object sender,ItemClickEventArgs e);
    public class ItemClickEventArgs : EventArgs
    {
        public int ItemIndex { get; private set; }

        public ItemClickEventArgs(int item_index)
        {
            ItemIndex = item_index;
        }
    }
    #endregion

    [Flags]
    public enum ItemState
    {
        None = 0,
        Focused = 1,
        Selected = 2
    }

    #region colors
    
    #endregion

}
