using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Drawing;

namespace netCommander.FileView
{
    class mTextViewControl : Control
    {
        #region init & exiting
        public mTextViewControl()
        {
            internal_init();
        }

        private void internal_init()
        {
            SetStyle
                (ControlStyles.AllPaintingInWmPaint |
                ControlStyles.OptimizedDoubleBuffer |
                ControlStyles.UserPaint |
                ControlStyles.Selectable,
                true);

            //test_stream = new FileStream
            //    (@"d:\docs\dyatlow_utf16.txt",
            //    FileMode.Open,
            //    FileAccess.Read);
            //test_provider = new TextProviderFileMultybyte(test_stream, Encoding.Unicode);
            //vScroll.Dock = DockStyle.Right;
            //Controls.Add(vScroll);
            //client_text_rectangle = new Rectangle(ClientRectangle.Location, new Size(ClientSize.Width - vScroll.Width, ClientSize.Height));
        }

        protected override void OnCreateControl()
        {
            text_area.Bounds = ClientRectangle;
            text_area.FetchCharsToScreen = this.fetch_text;
            text_area.FetchSelectionRange = this.fetch_selection;


            base.OnCreateControl();
        }

        protected override void Dispose(bool disposing)
        {
            text_area.Dispose();
            if (text_provider != null)
            {
                text_provider.Dispose();
            }

            base.Dispose(disposing);
        }
        #endregion

        #region scrolling
        private void scroll_to_char(long char_index)
        {
            screen_first_char = char_index;
            Refresh();
            caret_set_position(char_index_at_caret);

            long char_len = text_provider.GetCharsCount();
            ViewPercent = (float)(screen_first_char+text_area.CharsFitted) / (float)char_len;

            OnViewChanged(new EventArgs());
        }

        private void scroll_line_down()
        {
            //к началу строки #1

            if (text_area.LinesCount <= 1)
            {
                return;
            }

            if (is_eof_visible())
            {
                return;
            }

            long target_char = text_area.GetCharacterRangeInLine(1).First + screen_first_char;
            scroll_to_char(target_char);
        }

        private void scroll_line_up()
        {
            //а это уже сложнее 
            //с какого char должна начинаться епредыдуща линия?
            int offset_to_previous_line = text_area.FindPreviousLineStart(CreateGraphics());
            if (offset_to_previous_line >= 0)
            {
                return;
            }
            //офсет должен быть отрицательным
            long abs_previous_line = screen_first_char + offset_to_previous_line;
            scroll_to_char(abs_previous_line);
        }

        private void scroll_page_down()
        {
            int to_line = text_area.LinesCount - 1;
            if (to_line < 0)
            {
                return;
            }
            if (is_eof_visible())
            {
                return;
            }
            long to_char = screen_first_char + text_area.GetCharacterRangeInLine(to_line).First;
            scroll_to_char(to_char);
        }

        private void scroll_page_up()
        {
            int jump_offset = text_area.CharsFitted - text_area.GetCharacterRangeInLine(text_area.LinesCount-1).Length;
            long to_char = screen_first_char - jump_offset;
            if (to_char < 0)
            {
                to_char = 0;
                if (screen_first_char == 0)
                {
                    return;
                }
            }
            scroll_to_char(to_char);
        }

        #endregion

        #region selection
        private long selection_first = 5;
        private long selection_last = 10;

        public long SelectionStart
        {
            get
            {
                return selection_first;
            }
        }

        public long SelectionEnd
        {
            get
            {
                return selection_last;
            }
        }

        private void fetch_selection(out int select_first, out int select_last)
        {
            select_first = (int)(selection_first - screen_first_char);
            select_last = (int)(selection_last - screen_first_char);
        }

        private void selection_set(long sel_first, long sel_last)
        {
            selection_first = sel_first;
            selection_last = sel_last;
            Invalidate(text_area.Bounds);
        }

        private void selection_reset(bool force_paint)
        {
            selection_reset();
            if (force_paint)
            {
                Invalidate(text_area.Bounds);
            }
        }

        private void selection_reset()
        {
            selection_first = 1;
            selection_last = 0;
        }
        #endregion

        #region caret
        private long char_index_at_caret = 0;

        private bool is_eof_visible()
        {
            return (text_area.CharsFitted + screen_first_char == text_provider.GetCharsCount());
        }

        private void caret_set_position(long char_index)
        {
            if (char_index < 0)
            {
                //попытка перевести каретку ДО начала текста - ставим каретку на начало
                char_index = 0;
            }

            if ((text_provider != null) && (char_index > text_provider.GetCharsCount()))
            {
                //попытка перевести каретку за границу текста (+1 символ) - пресекаем
                return;
            }

            //вычисляем координаты
            bool outside = false;
            Point caret_pt = text_area.GetCaretPosition((int)(char_index-screen_first_char),out outside);

            if (outside) //выходим за пределы отображаемой области - по идее такого быть не должно
            {
                char_index = screen_first_char;
                caret_pt = text_area.Bounds.Location;
            }

            if (Focused)
            {
                //на самом деле показываем каретку только когда захвачен фокус ввода 
                Caret.SetCaretPos(caret_pt);
            }
            char_index_at_caret = char_index;

            //для обновление статусной области
            CharAtCaret = text_area.GetChar((int)(char_index_at_caret - screen_first_char));
            OnViewChanged(new EventArgs());
        }

        private void caret_set_position(Point pt)
        {
            int char_index_screen = 0;
            Point caret_pt = text_area.GetNearestCaretPosition(pt, out char_index_screen);
            if (Focused)
            {
                Caret.SetCaretPos(caret_pt);
            }
            char_index_at_caret = char_index_screen + screen_first_char;

            CharAtCaret = text_area.GetChar((int)(char_index_at_caret - screen_first_char));
            OnViewChanged(new EventArgs());
        }

        protected override void OnGotFocus(EventArgs e)
        {
            Caret.CreateCaretSolid(Handle, 1, (int)Font.GetHeight());
            //caret_set_position(char_index_at_caret);
            Caret.ShowCaret(Handle);

            base.OnGotFocus(e);
        }

        protected override void OnLostFocus(EventArgs e)
        {
            Caret.DestroyCaret();

            base.OnLostFocus(e);
        }

        

        #endregion

        #region resize

        //private Rectangle client_text_rectangle = new Rectangle();

        protected override void OnClientSizeChanged(EventArgs e)
        {
            //client_text_rectangle =
            //    new Rectangle
            //        (ClientRectangle.Location,
            //        new Size(ClientSize.Width - vScroll.Width, ClientSize.Height));

            Invalidate();   

            //обновим положение каретки
            caret_set_position(char_index_at_caret);

            base.OnClientSizeChanged(e);
        }

        #endregion

        #region chars handling
        public ITextProvider TextProvider
        {
            get
            {
                return text_provider;
            }
            set
            {
                if (text_provider != null)
                {
                    text_provider.Dispose();
                }
                selection_reset();
                text_provider = value;
                //char_index_at_caret = 0;
                Invalidate(ClientRectangle);
                caret_set_position(0);

                if (text_provider is ITextEditProvider)
                {
                    text_edit_provider = (ITextEditProvider)text_provider;
                    EditEnable = true;
                }
                else
                {
                    text_edit_provider = null;
                    EditEnable = false;
                }

                //scroll bar
                
            }
        }

        private void fetch_text(int offset_from_top, int max_chars,bool break_on_paragraph, out IntPtr char_buffer, out int chars_filled, out bool EOF)
        {
            if (text_provider == null)
            {
                char_buffer = IntPtr.Zero;
                chars_filled = 0;
                EOF = true;
            }
            else
            {
                long abs_offset = screen_first_char + offset_from_top;
                //если запрашивается текст ДО начала
                //вернем тест от начала
                //но максимоальной длиной max_chars+abs_offset
                //если и max_chars+abs_offset становится отрицательным
                //вернем null
                if (abs_offset < 0)
                {
                    max_chars = (int)(max_chars + abs_offset);
                    if (max_chars <= 0)
                    {
                        char_buffer = IntPtr.Zero;
                        chars_filled = -1;
                        EOF = false;
                    }
                    else
                    {
                        abs_offset = 0;
                        text_provider.GetCharParagraph
                            (abs_offset,
                            max_chars,
                            break_on_paragraph,
                            out char_buffer,
                            out chars_filled,
                            out EOF);
                    }
                }
                else
                {
                    text_provider.GetCharParagraph
                        (abs_offset,
                        max_chars,
                        break_on_paragraph,
                        out char_buffer,
                        out chars_filled,
                        out EOF);
                }
            }
            //string line_s = System.Runtime.InteropServices.Marshal.PtrToStringAuto(char_buffer, chars_filled);
        }

        public bool EditEnable { get; private set; }

        private ITextEditProvider text_edit_provider = null;
        private ITextProvider text_provider = null;
        private long screen_first_char = 0;

        public long FirstChar
        {
            get
            {
                return screen_first_char;
            }
        }
        #endregion

        #region render
        private script_text_area text_area = new script_text_area();
        private bool render_enable = true;
        protected override void OnPaintBackground(PaintEventArgs pevent)
        {
            //base.OnPaintBackground(pevent);
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            e.Graphics.FillRectangle(SystemBrushes.Window, ClientRectangle);
            if ((text_provider != null) && (render_enable))
            {
                text_area.Font = Font;
                text_area.Bounds = ClientRectangle;
                text_area.UpdateArea(e.Graphics);


                //string debug_text =
                //    "first char: " + screen_first_char.ToString();
                //e.Graphics.FillRectangle(SystemBrushes.Window, 20, 20, 200, 20);
                //e.Graphics.DrawString(debug_text, Font, SystemBrushes.Highlight, new PointF(20, 20));

                //caret_set_position(char_index_at_caret);
            }
            //base.OnPaint(e);
        }



        #endregion

        #region keyboard handling

        protected override bool ProcessDialogKey(Keys keyData)
        {
            switch (keyData)
            {
                case Keys.Right:
                    if ((int)(char_index_at_caret - screen_first_char) == text_area.CharsFitted - 1)
                    {

                        //нужен скролл на строку вниз
                        scroll_line_down();

                    }
                    caret_set_position(char_index_at_caret + 1);
                    selection_reset(true);
                    return true;

                case Keys.Left:
                    if (char_index_at_caret == screen_first_char)
                    {
                        //скролл вверх
                        scroll_line_up();
                    }
                    selection_reset(true);
                    caret_set_position(char_index_at_caret - 1);
                    return true;

                case Keys.Down:
                    Point target_down_point = Caret.GetCaretPos();
                    target_down_point.Y = target_down_point.Y + (int)Math.Ceiling(text_area.Font.GetHeight());
                    if (text_area.GetLineBounds(text_area.LinesCount - 1).Y < target_down_point.Y)
                    {
                        //скролл вниз
                        scroll_line_down();
                        //target_down_point = Caret.GetCaretPos();
                    }
                    caret_set_position(target_down_point);
                    selection_reset(true);
                    return true;

                case Keys.Up:
                    Point target_up_point_1 = Caret.GetCaretPos();
                    Point target_up_point_2 = Caret.GetCaretPos();
                    target_up_point_2.Y = target_up_point_1.Y - (int)Math.Ceiling(text_area.Font.GetHeight());
                    if (target_up_point_2.Y < text_area.Bounds.Y)
                    {
                        scroll_line_up();
                        target_up_point_2 = target_up_point_1;
                    }
                    caret_set_position(target_up_point_2);
                    selection_reset(true);
                    return true;

                case Keys.PageUp:
                    selection_reset();
                    scroll_page_up();
                    caret_set_position(screen_first_char);
                    return true;

                case Keys.PageDown:
                    selection_reset();
                    scroll_page_down();
                    caret_set_position(screen_first_char);
                    return true;

                case Keys.Home:
                    selection_reset();
                    scroll_to_char(0);
                    caret_set_position(screen_first_char);
                    return true;

                case Keys.End:
                    selection_reset();
                    long target_char = text_provider.GetCharsCount();
                    target_char = target_char - 16;
                    if (target_char < 0)
                    {
                        target_char = 0;
                    }
                    scroll_to_char(target_char);
                    caret_set_position(target_char);
                    return true;

                case Keys.Shift|Keys.Right:
                    if (selection_last == char_index_at_caret)
                    {
                        //продолжаем выделение
                        selection_set(selection_first, char_index_at_caret + 1);
                    }
                    else
                    {
                        //начинаем новое
                        selection_set(char_index_at_caret, char_index_at_caret + 1);
                    }

                    if ((int)(char_index_at_caret - screen_first_char) == text_area.CharsFitted - 1)
                    {
                        //нужен скролл на строку вниз
                        scroll_line_down();
                    }
                    caret_set_position(char_index_at_caret + 1);
                    return true;

                case Keys.Shift|Keys.Left:
                    if (selection_first == char_index_at_caret)
                    {
                        //продолжаем выделение влево
                        selection_set(char_index_at_caret-1, selection_last);
                    }
                    else
                    {
                        //начианем новое
                        selection_set(char_index_at_caret - 1, char_index_at_caret);
                    }
                    if (char_index_at_caret == screen_first_char)
                    {
                        //скролл вверх
                        scroll_line_up();
                    }
                    caret_set_position(char_index_at_caret - 1);
                    return true;

                case Keys.Shift|Keys.Down:

                    long old_char_caret = char_index_at_caret;

                    Point target_down_point_shift = Caret.GetCaretPos();
                    target_down_point_shift.Y = target_down_point_shift.Y + (int)Math.Ceiling(text_area.Font.GetHeight());
                    if (text_area.GetLineBounds(text_area.LinesCount - 1).Y < target_down_point_shift.Y)
                    {
                        //скролл вниз
                        scroll_line_down();
                        //target_down_point = Caret.GetCaretPos();
                    }
                    caret_set_position(target_down_point_shift);

                    if (selection_last == old_char_caret)
                    {
                        //продолжаем выделение
                        selection_set(selection_first, char_index_at_caret);
                    }
                    else
                    {
                        //начинаем новое
                        selection_set(old_char_caret, char_index_at_caret );
                    }
                    return true;

                case Keys.Shift|Keys.Up:
                    long old_char_caret_2 = char_index_at_caret;

                    Point target_up_point_1_shift = Caret.GetCaretPos();
                    Point target_up_point_2_shift = Caret.GetCaretPos();
                    target_up_point_2_shift.Y = target_up_point_1_shift.Y - (int)Math.Ceiling(text_area.Font.GetHeight());
                    if (target_up_point_2_shift.Y < text_area.Bounds.Y)
                    {
                        scroll_line_up();
                        target_up_point_2_shift = target_up_point_1_shift;
                    }
                    caret_set_position(target_up_point_2_shift);

                    if (selection_first == old_char_caret_2)
                    {
                        //продолжаем выделение
                        selection_set(char_index_at_caret, selection_last);
                    }
                    else
                    {
                        //начинаем новое
                        selection_set(char_index_at_caret, old_char_caret_2);
                    }
                    return true;

                //case Keys.S:
                //    scroll_line_down();
                //    return true;

                //case Keys.W:
                //    scroll_line_up();
                //    return true;

                //case Keys.D:
                //    scroll_page_down();
                //    return true;

                //case Keys.E:
                //    scroll_page_up();
                //    return true;

                //case Keys.T:
                //    text_area.FindPreviousLineStart(CreateGraphics());
                //    return true;

                    /*
                     * editing
                     */
                case Keys.Delete:
                    if (EditEnable)
                    {
                        text_edit_provider.DeleteChars(char_index_at_caret, 1);
                        Invalidate();
                    }
                    return true;

                case Keys.Back:
                    if ((EditEnable)&&(char_index_at_caret>=1))
                    {
                        text_edit_provider.DeleteChars(char_index_at_caret - 1,1);
                        Refresh();
                        caret_set_position(char_index_at_caret - 1);
                    }
                    return true;

                case Keys.Control|Keys.Delete:
                    if ((EditEnable) && (selection_last - selection_first > 0))
                    {
                        text_edit_provider.DeleteChars(selection_first,selection_last-selection_first);
                        Invalidate();
                    }
                    return true;

            }


            return base.ProcessDialogKey(keyData);
        }

        protected override bool ProcessDialogChar(char charCode)
        {
            //вставляем char
            int caret_offset = 0;
            if (EditEnable)
            {
                if (charCode == '\r')
                {
                    //обрабатываем \r - перевод строки
                    text_edit_provider.InsertChars(char_index_at_caret, Environment.NewLine.ToCharArray());
                    caret_offset = Environment.NewLine.Length;
                }
                else
                {
                    //остальные символы
                    text_edit_provider.InsertChars(char_index_at_caret, new char[] { charCode });
                    caret_offset = 1;
                }
                Refresh();
                caret_set_position(char_index_at_caret + caret_offset);

                return true;
            }

            return base.ProcessDialogChar(charCode);
        }
        #endregion

        #region publics

        public void GotoText(string text)
        {
            //ищем от каретки
            long to_pos = text_provider.FindText(text, char_index_at_caret);

            if (to_pos == -1)
            {
                netCommander.Messages.ShowMessage("Not found.");
                return;
            }

            scroll_to_char(to_pos);
            caret_set_position(to_pos);

            selection_set(to_pos, to_pos + text.Length);
        }

        public event EventHandler ViewChanged;
        private void OnViewChanged(EventArgs e)
        {
            if (ViewChanged == null)
            {
                return;
            }
            ViewChanged(this, e);
        }

        public float ViewPercent { get; private set; }
        public char CharAtCaret { get; private set; }
        public byte[] BytesAtCaret { get; private set; }

        public long CharIndexAtCaret
        {
            get
            {
                return char_index_at_caret;
            }
        }

        public long CharsTotal
        {
            get
            {
                return text_provider.GetCharsCount();
            }
        }

        #endregion

        #region mice handling
        //private bool mouse_selection = false;
        private Size drag_size = System.Windows.Forms.SystemInformation.DragSize;
        private Rectangle mouse_test_rect = Rectangle.Empty;

        protected override void OnMouseDown(MouseEventArgs e)
        {
            if (!Focused)
            {
                Focus();
            }

            if (e.Button == MouseButtons.Left)
            {
                selection_reset(true);
                mouse_test_rect = new Rectangle(e.Location.X + drag_size.Width / 2, e.Location.Y + drag_size.Height / 2, drag_size.Width, drag_size.Height);
            }

            base.OnMouseDown(e);
        }

        protected override void OnMouseUp(MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                mouse_test_rect = Rectangle.Empty;
            }

            base.OnMouseUp(e);
        }

        /*
         * TODO
         * добавить случай, когда требуется scroll и, соответственно, поправить для 
         * этого GetNearestCaretPosition, чтобы возвращалось что-то осмысленное, если точка за пределами текста
         * выделение влево и вправо - работает
         * выделение вверх и вниз - какая-то херь с индексами
         */
        private void mouse_select_do(Point mice_location)
        {
            int ind = 0;
            HitTextResult hit_test = text_area.HitTest(mice_location);
            ind = hit_test.CharacterIndex;
            long char_under_mice = ind + screen_first_char;

            if ((hit_test.HitTextType == HitTextType.Glyph) && (char_under_mice >= selection_first) && (char_under_mice < selection_last))
            {
                //выходим, если char под указателем уже выделен
                return;
            }

            //если char под указаьелем не выделен
            //смторим как он располагается относительно существующего выделения

            long pre_char = -1;
            //левый char выделен?
            pre_char = char_under_mice - 1;
            if ((pre_char >= selection_first) && (pre_char < selection_last))
            {
                //добавляем к выделению, char под указателем - конец выделения
                selection_set(selection_first, char_under_mice + 1);
                return;
            }

            //правый char выделен?
            pre_char = char_under_mice + 1;
            if ((pre_char >= selection_first) && (pre_char < selection_last))
            {
                //char под указателем - начало выделения
                selection_set(char_under_mice, selection_last);
                return;
            }

            //верхний char выделен?
            int line_height = (int)Math.Ceiling(Font.GetHeight());
            Point pre_point = new Point();
            pre_point.X = mice_location.X;
            pre_point.Y = mice_location.Y - line_height;
            HitTextResult neibo_hit = text_area.HitTest(pre_point);
            ind = neibo_hit.CharacterIndex;
            pre_char = ind + screen_first_char;
            if ((hit_test.HitTextType == HitTextType.Glyph) && (pre_char >= selection_first) && (pre_char < selection_last))
            {
                //char под указателем - конец выделения
                selection_set(selection_first, char_under_mice + 1);
                return;
            }
            if ((hit_test.HitTextType == HitTextType.AfterText) && (pre_char >= selection_first) && (pre_char < selection_last))
            {
                //указатель ниже текста - прокрутка
                scroll_line_down();
                //теперь char под указателем - конец выделения
                hit_test = text_area.HitTest(mice_location);
                char_under_mice = hit_test.CharacterIndex + screen_first_char;
                //если и после прокрутки под указатедем - пусто, выходим
                if (hit_test.HitTextType == HitTextType.Glyph)
                {
                    selection_set(selection_first, char_under_mice + 1);
                    return;
                }
                else
                {
                    return;
                }
            }

            //нижний char выдеден?
            pre_point.Y = mice_location.Y + line_height;
            text_area.GetNearestCaretPosition(pre_point, out ind);
            pre_char = ind + screen_first_char;
            if ((hit_test.HitTextType==HitTextType.Glyph)&&(pre_char >= selection_first) && (pre_char < selection_last))
            {
                //char под указателем - начало выделения
                selection_set(char_under_mice, selection_last);
                return;
            }
            if ((hit_test.HitTextType == HitTextType.BeforeText) && (pre_char >= selection_first) && (pre_char < selection_last))
            {
                //нужна прокрутка вверх
                scroll_line_up();
                //теперь char под указателем - насчало выделения
                hit_test = text_area.HitTest(mice_location);
                char_under_mice = hit_test.CharacterIndex + screen_first_char;
                //если и после прокрутки под указатедем - пусто, выходим
                if (hit_test.HitTextType == HitTextType.Glyph)
                {
                    selection_set(char_under_mice, selection_last);
                    return;
                }
                else
                {
                    return;
                }
            }

            //иначе начинаем новое выделение
            if (hit_test.HitTextType == HitTextType.Glyph)
            {
                selection_set(char_under_mice, char_under_mice + 1);
            }
        }

        protected override void OnMouseMove(MouseEventArgs e)
        {
            if (text_area.Bounds.Contains(e.Location))
            {
                Cursor = Cursors.IBeam;

                if ((mouse_test_rect != Rectangle.Empty) && (!mouse_test_rect.Contains(e.Location)))
                {
                    mouse_select_do(e.Location);

                }

            }
            else
            {
                Cursor = Cursors.Default;
            }



            base.OnMouseMove(e);
        }

        protected override void OnMouseClick(MouseEventArgs e)
        {
            //клик:
            //ставим каретку на клик и снимаем выделение
            caret_set_position(e.Location);

            //if ((e.Button == MouseButtons.Left)&&(mouse_test_rect==Rectangle.Empty))
            //{
            //    selection_reset(true);
            //}

            base.OnMouseClick(e);
        }
        #endregion
    }


}
