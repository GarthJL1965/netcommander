using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.Drawing;
using System.Drawing.Text;
using System.ComponentModel;

namespace netCommander.winControls
{
    class mInfoPanel : Control
    {

        public mInfoPanel()
        {
            init_internal();
        }

        public void RefreshSafe()
        {
            if (InvokeRequired)
            {
                Invoke(new MethodInvoker(Refresh));
            }
            else
            {
                Refresh();
            }
        }

        private void init_internal()
        {
            SetStyle
                (ControlStyles.AllPaintingInWmPaint |
                ControlStyles.OptimizedDoubleBuffer |
                ControlStyles.UserPaint,
                true);

            string_format.Alignment = StringAlignment.Near;
            string_format.FormatFlags = StringFormatFlags.LineLimit;
            string_format.LineAlignment = StringAlignment.Center;
        }

        protected override void OnCreateControl()
        {
            update_line_height();
            update_line_rects();
            Invalidate();

            base.OnCreateControl();
        }

        [ReadOnly(true)]
        public int LineCount
        {
            get
            {
                return line_count;
            }
            set
            {
                line_count = value;
                update_line_rects();
                Invalidate(ClientRectangle);
            }
        }

        public bool ShowBorder
        {
            get
            {
                return show_border;
            }
            set
            {
                show_border = value;
                if (value)
                {
                    border_size = SystemInformation.Border3DSize;
                }
                else
                {
                    border_size = Size.Empty;
                }
            }
        }

        private int line_count=3;
        private int line_height = 16;
        private List<Rectangle> line_rects = new List<Rectangle>();
        private StringFormat string_format = new StringFormat();
        private Size border_size = new Size();
        private bool show_border = false;

        public TextRenderingHint TextRenderingHint { get; set; }

        //private Brush brush_background = new SolidBrush(Options.BackColor);
        //private Brush brush_foreground = new SolidBrush(Options.ForeColor);
        private BrushCache brushes = new BrushCache();

        public event FetchInfoToDislayEventHandler FetchInfoToDislay;
        private void OnFetchInfoToDislay(FetchInfoToDislayEventArgs e)
        {
            if (FetchInfoToDislay != null)
            {
                FetchInfoToDislay(this,e);
            }
        }

        private void update_line_height()
        {
            Graphics g = CreateGraphics();
            line_height = (int)Math.Ceiling(Font.GetHeight(g)) + 4;
            g.Dispose();
        }

        private void update_line_rects()
        {
            line_rects.Clear();
            Size line_size = new Size();

            
            line_size=new Size
            (Size.Width-2*border_size.Width,
            line_height);
            for (int i = 0; i < line_count; i++)
            {
                line_rects.Add(new Rectangle
                (new Point(border_size.Width, i * line_height+border_size.Height),
                line_size));
            }
            this.Size = new Size(Size.Width, line_height * line_count+border_size.Height*2);
        }

        protected override void OnFontChanged(EventArgs e)
        {
            update_line_height();
            update_line_rects();
            Invalidate(ClientRectangle);

            base.OnFontChanged(e);
        }

        protected override void OnSizeChanged(EventArgs e)
        {
            update_line_rects();
            Invalidate(ClientRectangle);

            base.OnSizeChanged(e);
        }

        protected override void OnPaintBackground(PaintEventArgs e)
        {
            FetchInfoToDislayEventArgs eArgs = new FetchInfoToDislayEventArgs(-1);
            OnFetchInfoToDislay(eArgs);

            e.Graphics.FillRectangle(brushes.GetBrush(eArgs.Colors.BackgroundColor), ClientRectangle);
      
            if (show_border)
            {
                ControlPaint.DrawBorder3D
               (e.Graphics,
               ClientRectangle);
            }
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            //base.OnPaint(e);

            e.Graphics.TextRenderingHint = TextRenderingHint;
            for (int i = 0; i < line_count; i++)
            {
                FetchInfoToDislayEventArgs eArgs = new FetchInfoToDislayEventArgs(i);
                OnFetchInfoToDislay(eArgs);
                string_format.Trimming = eArgs.DisplayTrimming;
                string_format.Alignment = eArgs.Alignment;
                Brush fore_brush = null;
                Brush back_brush = null;
                fore_brush = brushes.GetBrush(eArgs.Colors.ForegroundColor);
                back_brush = brushes.GetBrush(eArgs.Colors.BackgroundColor);

                e.Graphics.FillRectangle(back_brush, line_rects[i]);
                e.Graphics.DrawString
                    (eArgs.DisplayText,
                    Font,
                    fore_brush,
                    line_rects[i],
                    string_format);
            }
        }


    }

    public delegate void FetchInfoToDislayEventHandler(object sender,FetchInfoToDislayEventArgs e);
    public class FetchInfoToDislayEventArgs : EventArgs
    {
        public String DisplayText { get; set; }
        public StringTrimming DisplayTrimming { get; set; }
        public int LineIndex { get; private set; }
        public StringAlignment Alignment { get; set; }
        public ItemColors Colors { get; set; }

        public FetchInfoToDislayEventArgs(int line_index)
        {
            LineIndex = line_index;
            DisplayText = string.Empty;
            DisplayTrimming = StringTrimming.EllipsisPath;
            Alignment = StringAlignment.Near;
        }
    }
}
