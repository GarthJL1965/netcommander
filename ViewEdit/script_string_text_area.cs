using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;
using System.Drawing;

namespace netCommander.FileView
{
    public class script_text_area:IDisposable
    {
        public Rectangle Bounds { get; set; }
        public Font Font { get; set; }

        public int CharsFitted
        {
            get
            {
                int ret = 0;
                foreach (LineInfoSS line in lines_info)
                {
                    ret = ret + line.CharLength;
                }
                return ret;
            }
        }

        public int MaximumCharsInLine
        {
            get
            {
                return max_chars_in_line;
            }
            set
            {
                max_chars_in_line = value;
            }
        }

        public int LinesCount
        {
            get
            {
                return lines_info.Count;
            }
        }

        public Rectangle GetLineBounds(int line_index)
        {
            Size s = lines_info[line_index].Extent;
            return new Rectangle(lines_info[line_index].ReferensePoint, s);
        }

        public char GetChar(int char_index)
        {
            if (FetchCharsToScreen == null)
            {
                return '\0';
            }

            char ret = new char();
            IntPtr buffer=IntPtr.Zero;
            int chars_filled=0;
            bool eof=false;

            FetchCharsToScreen(char_index, 1, true,out buffer, out chars_filled, out eof);
            if (chars_filled > 0)
            {
                ret = Marshal.PtrToStringUni(buffer, chars_filled)[0];
            }
            return ret;
        }

        public LogicalCharacterAttribute GetLogicalCharacterAttribute(int char_index)
        {
            int line_index = GetLineIndexFromCharacterIndex(char_index);
            if (line_index == -1)
            {
                return LogicalCharacterAttribute.None;
            }

            int char_in_line = char_index - lines_info[line_index].CharFirst;
            LogicalCharacterAttribute ret=lines_info[line_index].GetLogicalAttribute(char_in_line);
            return ret;
        }

        public Rectangle GetCharExtent(int char_index)
        {
            int line_index = GetLineIndexFromCharacterIndex(char_index);
            if (line_index == -1)
            {
                return Rectangle.Empty;
            }

            int char_in_line = char_index - lines_info[line_index].CharFirst;
            Rectangle ret = new Rectangle();
            ret.X = lines_info[line_index].ReferensePoint.X + lines_info[line_index].GetX_AtCharacterIndex(char_in_line, false).Xoffset;
            ret.Y = lines_info[line_index].ReferensePoint.Y;
            ret.Height = lines_info[line_index].Extent.Height;
            ret.Width = lines_info[line_index].GetX_AtCharacterIndex(char_in_line, true).Xoffset - lines_info[line_index].GetX_AtCharacterIndex(char_in_line, false).Xoffset;
            return ret;
        }

        public Region TextRegion { get; private set; }

 
        public Point GetNearestCaretPosition(Point pt, out int char_index)
        {
            if (LinesCount == 0)
            {
                char_index = 0;
                return new Point();
            }

            //найдем линию
            int line_index = GetLineIndexFromPoint(pt);

            if (line_index == -1)
            {
                //если point не попадает в строку, считаем ближайшей последнюю
                line_index = LinesCount - 1;
            }

            //найдем char index
            CharacterPositionInfo char_info = lines_info[line_index].GetCharacterIndexAtX(pt.X);

            switch (char_info.PositionType)
            {
                case CharacterPositionType.LeadingEdge:
                    char_index = char_info.CharIndex + lines_info[line_index].CharFirst;
                    break;
                case CharacterPositionType.TrailingEdge:
                    char_index = char_info.CharIndex + lines_info[line_index].CharFirst + 1;
                    break;
                case CharacterPositionType.ClusterPart:
                    char_index = char_info.CharIndex + lines_info[line_index].CharFirst + 1;
                    break;
                case CharacterPositionType.BeforeBeginning:
                    char_index = lines_info[line_index].CharFirst;
                    break;
                case CharacterPositionType.AfterEnd:
                    char_index = lines_info[line_index].CharFirst + lines_info[line_index].CharLength - 1;
                    break;
                default:
                    char_index = lines_info[line_index].CharFirst;
                    break;
            }
            bool outside = false;

            return GetCaretPosition(char_index, out outside);
        }

        public Point GetCaretPosition(int char_index,out bool outside)
        {
            Point ret = new Point();

            //найдем линию
            int line_index = GetLineIndexFromCharacterIndex(char_index);
            if (line_index == -1)
            {
                outside = true;
                return ret;
            }

            //позиция внутри линии
            int char_index_in_line = char_index - lines_info[line_index].CharFirst;

            //а что если запрашивается позиция в коце текста?
            //тогда придется вернуть trailing edge предыдущего символа
            //при этом если предыдущий символ \n, то каретку надо бы поставить на следующую строку
            //и проверить не вылазиет ли она за пределы экрана;
            //а если предыдущего нет, то есть текст пуст вернем вехний левый угол
            bool is_trailing_edge = false;
            if (char_index_in_line >= lines_info[line_index].CharLength)
            {
                if (char_index_in_line > 0)
                {
                    char_index_in_line = char_index_in_line - 1;
                    is_trailing_edge = true;
                    char pre_char = GetChar(lines_info[line_index].CharFirst + char_index_in_line);
                    if (pre_char == '\n')
                    {
                        ret = lines_info[line_index].ReferensePoint;
                        ret.Y = ret.Y + lines_info[line_index].Extent.Height;
                        outside = (ret.Y > Bounds.Bottom);
                        return ret;
                    }
                }
                else
                {
                    ret = lines_info[line_index].ReferensePoint;
                    outside = false;
                    return ret;
                }
            }

            CharacterPositionInfo char_info = lines_info[line_index].GetX_AtCharacterIndex
                (char_index_in_line, is_trailing_edge);

            

            ret = new Point(char_info.Xoffset + Bounds.X, lines_info[line_index].ReferensePoint.Y);

            //System.Windows.Forms.MessageBox.Show
            //    (ret.ToString() +
            //    " line #" + line_index.ToString() +
            //    "; char #" + char_index_in_line.ToString() +
            //    "; line first char #" + lines_info[line_index].CharFirst.ToString() +
            //    "; line char length " + lines_info[line_index].CharLength.ToString() +
            //    "; char: " + char.ToString(GetChar(char_index)));
            outside = false;
            return ret;
        }


        public CharacterRange GetCharacterRangeInLine(int line_index)
        {
            return new CharacterRange(lines_info[line_index].CharFirst, lines_info[line_index].CharLength);
        }

        public HitTextResult HitTest(Point pt)
        {
            HitTextResult ret = new HitTextResult();

            if (LinesCount == 0)
            {
                ret.CharacterIndex = -1;
                ret.CharacterIndexInLine = -1;
                ret.LineIndex = -1;
                ret.HitTextType = HitTextType.BeforeText;
                return ret;
            }

            if (pt.Y < lines_info[0].ReferensePoint.Y)
            {
                ret.CharacterIndex = -1;
                ret.CharacterIndexInLine = -1;
                ret.LineIndex = -1;
                ret.HitTextType = HitTextType.BeforeText;
                return ret;
            }

            if (pt.Y > lines_info[LinesCount - 1].ReferensePoint.Y + lines_info[LinesCount - 1].Extent.Height)
            {
                ret.CharacterIndex = -1;
                ret.CharacterIndexInLine = -1;
                ret.LineIndex = -1;
                ret.HitTextType = HitTextType.AfterText;
                return ret;
            }

            int line_index = GetLineIndexFromPoint(pt);
            if (line_index == -1)
            {
                ret.CharacterIndex = -1;
                ret.CharacterIndexInLine = -1;
                ret.LineIndex = -1;
                ret.HitTextType = HitTextType.Undefined;
                return ret;
            }

            CharacterPositionInfo cpi = lines_info[line_index].GetCharacterIndexAtX(pt.X - lines_info[line_index].ReferensePoint.X);

            ret.CharacterIndex = cpi.CharIndex + lines_info[line_index].CharFirst;
            ret.CharacterIndexInLine = cpi.CharIndex;
            ret.LineIndex = line_index;

            switch (cpi.PositionType)
            {
                case CharacterPositionType.LeadingEdge:
                case CharacterPositionType.TrailingEdge:
                case CharacterPositionType.ClusterPart:
                    ret.HitTextType = HitTextType.Glyph;
                    break;
                case CharacterPositionType.BeforeBeginning:
                    ret.HitTextType = HitTextType.BeforeOfLine;
                    break;
                case CharacterPositionType.AfterEnd:
                    ret.HitTextType = HitTextType.AfterOfLine;
                    break;
            }
            return ret;
        }

        private CharacterPositionInfo GetCharacterAtPoint(Point pt)
        {
            CharacterPositionInfo ret = new CharacterPositionInfo();
            //find line
            int line_index = GetLineIndexFromPoint(pt);
            //если pt не соответсвует ни одной линии...
            if (line_index == -1)
            {
                ret.CharIndex = -1;
                ret.OffsetToNextCaretPosition = 0;
                ret.PositionType = CharacterPositionType.AfterEnd;
                ret.Xoffset = pt.X;
                return ret;
            }

            //если линия найдена...
            ret = lines_info[line_index].GetCharacterIndexAtX(pt.X - Bounds.X);
            //поправляем, чтобы ret.CharIndex был бы относительно экрана
            ret.CharIndex = ret.CharIndex + lines_info[line_index].CharFirst;
            return ret;
        }

        public int GetLineIndexFromPoint(Point pt)
        {
            int ret = -1;
            int current_top = 0;
            int current_bottom = 0;
            for (int i = 0; i < LinesCount; i++)
            {
                current_bottom = lines_info[i].ReferensePoint.Y + lines_info[i].Extent.Height;
                current_top = lines_info[i].ReferensePoint.Y;
                if (current_top != 0)
                {
                    current_top--;
                }
                if ((pt.Y >= current_top) && (pt.Y < current_bottom))
                {
                    ret = i;
                    break;
                }
            }
            return ret;
        }

        public int GetLineIndexFromCharacterIndex(int char_index)
        {
            int ret = -1;
            for (int i = 0; i < LinesCount; i++)
            {
                if ((char_index >= lines_info[i].CharFirst) && (char_index <= lines_info[i].CharFirst + lines_info[i].CharLength))
                {
                    ret = i;
                    break;
                }
            }
            
            return ret;
        }

        private int max_chars_in_line = 256;
        private List<LineInfoSS> lines_info = new List<LineInfoSS>();

        public FetchSelectionRange FetchSelectionRange;
        private void OnFetchSelectionRange(out int select_first, out int select_last)
        {
            if (FetchSelectionRange != null)
            {
                FetchSelectionRange(out select_first, out select_last);
            }
            else
            {
                select_first = 1;
                select_last = 0;
            }
        }

        public FetchCharsToScreen FetchCharsToScreen;
        private void OnFetchCharsToScreen
            (int offset_from_top,
            bool break_on_paragraph,
            out IntPtr char_buffer,
            out int chars_filled,
            out bool EOF)
        {
            if (FetchCharsToScreen != null)
            {
                FetchCharsToScreen(offset_from_top, max_chars_in_line, break_on_paragraph, out char_buffer, out chars_filled, out EOF);
            }
            else
            {
                char_buffer = IntPtr.Zero;
                chars_filled = 0;
                EOF = true;
            }
        }

        public int FindPreviousLineStart(IDeviceContext dev)
        {
            //для обсчета строки будет нужен контекст
            IntPtr hdc = dev.GetHdc();
            IntPtr font_ptr = Font.ToHfont();
            IntPtr old_font = NativeGdi.SelectObject(hdc, font_ptr);
            uint old_align = NativeGdi.SetTextAlign(hdc, 0);
            int line_height = (int)Math.Ceiling(Font.GetHeight());
            IntPtr char_buffer = IntPtr.Zero;
            bool EOF = false;
            int chars_filled = 0;
            int ret = 0;

            //получаем буфер символов начиная от -max_chars_in_line 
            OnFetchCharsToScreen
                (-max_chars_in_line,
                false,
                out char_buffer,
                out chars_filled,
                out EOF);

            if (chars_filled <= 0)
            {
                //то есть и так находимся в начале
                ret = 0;
            }
            else
            {
                //получаем ssa_sctruct
                IntPtr ssa_scruct = NativeScript.ScriptStringAnalyseCall
                    (hdc,
                    char_buffer,
                    chars_filled,
                    SCRIPT_TABDEF.GetDefault());
                
                //берем logical wodths
                int[] widths = NativeScript.GetLogicalWidths(ssa_scruct);
                //освобождаем ssa_scruct
                NativeScript.ScriptStringFree(ref ssa_scruct);

                //суммируем полученные ширины символов с конца, пока не превысим
                //максимальную ширину или не встретим второй hard break
                char current_char = '\0';
                int current_width = 0;
                int finding_ind = -1;
                for (int i = chars_filled - 1; i >= 0; i--)
                {
                    current_char = get_char_from_buffer(char_buffer, i);
                    current_width = current_width + widths[i];
                    //проверяем ширину
                    if (current_width > Bounds.Width)
                    {
                        finding_ind = i;
                        break;
                    }

                    //проверяем hard break (\n   \r     \r\n) начиная со второго шага
                    if (i != chars_filled - 1)
                    {
                        if (current_char == '\n')
                        {
                            if (((i - 1) >= 0) && (get_char_from_buffer(char_buffer, i - 1) == '\r'))
                            {
                                finding_ind = i;
                                break;
                            }
                            finding_ind = i;
                            break;
                        }
                        else if ((current_char == '\r') && (i < chars_filled - 2))
                        {
                            finding_ind = i;
                            break;
                        }
                    }
                }

                //finding_ind теперь соответствует концу предыщей строки
                ret = -(chars_filled - finding_ind-1);

            }

            font_ptr = NativeGdi.SelectObject(hdc, old_font);
            NativeGdi.DeleteObject(font_ptr);
            NativeGdi.SetTextAlign(hdc, old_align);
            dev.ReleaseHdc();

            return ret;
        }

        private char get_char_from_buffer(IntPtr buffer, int offset)
        {
            byte[] char_b = new byte[2];
            char_b[0]=Marshal.ReadByte(buffer, offset * 2);
            char_b[1] = Marshal.ReadByte(buffer, offset * 2 + 1);
            return BitConverter.ToChar(char_b, 0);
        }

        /// <summary>
        /// without render, only calc lines layout
        /// </summary>
        /// <param name="dev"></param>
        public void CalcLines(IDeviceContext dev,int max_lines_to_calc)
        {
            //освобождаем имеющиеся строки
            free_lines();
            if (TextRegion != null)
            {
                TextRegion.Dispose();
            }
            TextRegion = new Region();

            IntPtr hdc = dev.GetHdc();
            IntPtr font_ptr = Font.ToHfont();
            IntPtr old_font = NativeGdi.SelectObject(hdc, font_ptr);
            uint old_align = NativeGdi.SetTextAlign(hdc, 0);
            int line_height = (int)Math.Ceiling(Font.GetHeight());
            IntPtr char_buffer = IntPtr.Zero;
            bool EOF = false;
            int chars_filled = 0;
            int current_char_offset = 0;
            int current_Y = 0;
            int current_line_index = 0;

            while (current_line_index<max_lines_to_calc)
            {
                OnFetchCharsToScreen
                    (current_char_offset,
                    true,
                    out char_buffer,
                    out chars_filled,
                    out EOF);
                LineInfoSS new_line = new LineInfoSS();
                new_line.ReferensePoint = new Point(Bounds.X, Bounds.Y + current_Y);
                new_line.CharFirst = current_char_offset;

                //обсчитываем
                if (chars_filled != 0)
                {
                    new_line.PrepareLayout
                        (hdc,
                        char_buffer,
                        chars_filled,
                        Bounds.Width);
                }

                //прибавляем счетчики
                current_char_offset = current_char_offset + new_line.CharLength;
                current_Y = current_Y + line_height;
                current_line_index++;

                lines_info.Add(new_line);
                TextRegion.Union(new Rectangle(new_line.ReferensePoint, new_line.Extent));

                //если стоит признак конца файла, выходим
                if (EOF)
                {
                    break;
                }
            }

            font_ptr = NativeGdi.SelectObject(hdc, old_font);
            NativeGdi.DeleteObject(font_ptr);
            NativeGdi.SetTextAlign(hdc, old_align);

            dev.ReleaseHdc();
        }

        /// <summary>
        /// calc layout and render
        /// </summary>
        /// <param name="dev"></param>
        public void UpdateArea(IDeviceContext dev)
        //public void UpdateArea(Graphics g)
        {
            free_lines();
            if (TextRegion != null)
            {
                TextRegion.Dispose();
            }
            TextRegion = new Region();

            IntPtr hdc = dev.GetHdc();
            //IntPtr hdc = g.GetHdc();

            

            IntPtr font_ptr = Font.ToHfont();
            //IntPtr font_ptr = font_ptr_cached;
            IntPtr old_font = NativeGdi.SelectObject(hdc, font_ptr);

            //Font hdc_font = Font.FromHfont(old_font);
            
            uint old_align = NativeGdi.SetTextAlign(hdc, 0);
            int line_height = (int)Math.Ceiling(Font.GetHeight());
            IntPtr char_buffer = IntPtr.Zero;

            int select_first = 0;
            int select_last = 0;
            int select_line_first = 0;
            int select_line_last = 0;
            OnFetchSelectionRange(out select_first, out select_last);

            bool EOF=false;
            int chars_filled=0;
            int current_char_offset = 0;
            int current_Y = 0;
            while (current_Y + line_height < Bounds.Height)
            {
                select_line_first = 0;
                select_line_last = 0;

                OnFetchCharsToScreen
                    (current_char_offset,
                    true,
                    out char_buffer,
                    out chars_filled,
                    out EOF);
                LineInfoSS new_line = new LineInfoSS();
                new_line.ReferensePoint = new Point(Bounds.X, Bounds.Y + current_Y);
                new_line.CharFirst = current_char_offset;

                //если буфер не пуст рисуем строку
                if (chars_filled != 0)
                {
                    new_line.PrepareLayout
                        (hdc,
                        char_buffer,
                        chars_filled,
                        Bounds.Width);

                    //пересчитываем selection
                    if ((select_first >= current_char_offset) && (select_first <= current_char_offset + new_line.CharLength - 1))
                    {
                        //начало selection попадает в строку
                        select_line_first = select_first - current_char_offset;
                        select_line_last = new_line.CharLength - 1;
                    }
                    if ((select_last >= current_char_offset) && (select_last <= current_char_offset + new_line.CharLength - 1))
                    {
                        //конец selection попадает в строку
                        select_line_last = select_last - current_char_offset;
                    }
                    if ((select_first <= current_char_offset) && (select_last >= current_char_offset + new_line.CharLength - 1))
                    {
                        //весь selection попадает в строку
                        select_line_first = select_first - current_char_offset;
                        select_line_last = new_line.CharLength - 1;
                    }

                    //и рисуем
                    new_line.Render(select_line_first, select_line_last);
                }

                //прибавляем счетчики
                current_char_offset = current_char_offset + new_line.CharLength;
                current_Y = current_Y + line_height;

                lines_info.Add(new_line);
                TextRegion.Union(new Rectangle(new_line.ReferensePoint, new_line.Extent));

                //если стоит признак конца файла, выходим
                if (EOF)
                {
                    break;
                }
            }



            font_ptr = NativeGdi.SelectObject(hdc, old_font);
            NativeGdi.DeleteObject(font_ptr);
            //NativeGdi.DeleteObject(old_font);
            NativeGdi.SetTextAlign(hdc,old_align);

            NativeGdi.GdiFlush(); //возможно не обязательно

            dev.ReleaseHdc();
            //g.ReleaseHdc(hdc);
        }



        private void free_lines()
        {
            foreach (LineInfoSS line in lines_info)
            {
                line.Dispose();
            }
            lines_info.Clear();
        }



        #region line info
        class LineInfoSS : IDisposable
        {
            public Point ReferensePoint { get; set; }
            public int CharFirst { get; set; }
            public int CharLength { get; private set; }
            public Size Extent { get; private set; }

            public LogicalCharacterAttribute GetLogicalAttribute(int char_index)
            {
                IntPtr log_attr_buffer = NativeScript.ScriptString_pLogAttr(ssa_struct_ptr);
                return NativeScript.GetCharacterAttribute(log_attr_buffer, char_index);
            }

            public CharacterPositionInfo GetCharacterIndexAtX(int xOffset)
            {
                return CharacterPositionInfo.FromXoffset(ssa_struct_ptr, xOffset);
            }

            public CharacterPositionInfo GetX_AtCharacterIndex(int char_index, bool trailingEdge)
            {
                if (char_index > CharLength - 1)
                {
                    throw new ArgumentOutOfRangeException
                        ("char_index", char_index, "char_index greater than line length.");
                }
                CharacterPositionType pos_type=CharacterPositionType.LeadingEdge;
                if(trailingEdge)
                {
                    pos_type=CharacterPositionType.TrailingEdge;
                }
                return CharacterPositionInfo.FromCharIndex(ssa_struct_ptr, char_index, pos_type);
            }

            private IntPtr ssa_struct_ptr = IntPtr.Zero;
            private SCRIPT_TABDEF tab_def = SCRIPT_TABDEF.GetDefault();

            public void Render(int select_first, int select_last)
            {
                int res0 = NativeScript.ScriptStringOut
                    (ssa_struct_ptr,
                    ReferensePoint.X,
                    ReferensePoint.Y,
                    GDI_ExtTextOutOption.None,
                    IntPtr.Zero,
                    select_first,
                    select_last,
                    false);
                if (res0 != NativeScript.S_OK)
                {
                    Marshal.ThrowExceptionForHR(res0);
                }
            }

            public void PrepareLayout(IntPtr hdc, IntPtr char_buffer, int max_char_len, int max_width)
            {
                ssa_struct_ptr = NativeScript.ScriptStringAnalyseCall
                    (hdc,
                    char_buffer,
                    max_char_len,
                    max_width,
                    tab_def);

                //сколько поместилось
                int chars_clipped = NativeScript.GetPcOutChars(ssa_struct_ptr);

                //просматриваем с конца для поиска разрыва линии
                IntPtr attr_buffer = NativeScript.ScriptString_pLogAttr(ssa_struct_ptr);
                int line_break_ind = -1;

                //если вся строка уместилась в линию
                if (chars_clipped == max_char_len)
                {
                    line_break_ind = chars_clipped - 1;
                }

                //если вся строка не уместилась, ищем крайний с конца SoftBreak
                if (line_break_ind == -1)
                {
                    LogicalCharacterAttribute attr = LogicalCharacterAttribute.None;
                    for (int i = chars_clipped - 1; i > 0; i--)
                    {
                        attr = NativeScript.GetCharacterAttribute(attr_buffer, i);
                        if ((attr & LogicalCharacterAttribute.SoftBreak) == LogicalCharacterAttribute.SoftBreak)
                        {
                            line_break_ind = i-1;
                            break;
                        }
                    }
                }

                //если и softBreak не найден, рвем по умещению
                if (line_break_ind == -1)
                {
                    line_break_ind = chars_clipped - 1;
                }

                //если разрываем, вторично вызываем обссчет с новой длиной строки
                if (line_break_ind + 1 != max_char_len)
                {
                    //освобождаем предыдущий ssa_sctuct_ptr
                    int res3 = NativeScript.ScriptStringFree(ref ssa_struct_ptr);
                    if (res3 != 0)
                    {
                        Marshal.ThrowExceptionForHR(res3);
                    }

                    //и обновляем
                    ssa_struct_ptr = NativeScript.ScriptStringAnalyseCall
                        (hdc,
                        char_buffer,
                        line_break_ind + 1,
                        tab_def);
                }

                //debug
                //string actual_string = Marshal.PtrToStringAuto(char_buffer, line_break_ind + 1);
                //char[] actual_chars = actual_string.ToCharArray();

                //заполняем свойства
                Extent = NativeScript.GetScriptStringExtent(ssa_struct_ptr);
                CharLength = line_break_ind + 1;

                //debug
                //if (Extent.Width == 0)
                //{
                //    int debug = 0;
                //}
            }

            /// <summary>
            /// dispose obligatory!!
            /// </summary>
            public void Dispose()
            {
                if (ssa_struct_ptr != IntPtr.Zero)
                {
                    int res=NativeScript.ScriptStringFree(ref ssa_struct_ptr);
                    if (res != 0)
                    {
                        Marshal.ThrowExceptionForHR(res);
                    }
                }
            }
        }
        #endregion

        #region IDisposable Members

        public void Dispose()
        {
            free_lines();
            if (TextRegion != null)
            {
                TextRegion.Dispose();
            }
        }
        
        #endregion
    }

    public delegate void FetchCharsToScreen(int offset_from_top, int max_chars, bool BreakOnParagraph,out IntPtr char_buffer, out int chars_filled, out bool EOF);
    public delegate void FetchSelectionRange(out int select_first, out int select_last);

    public enum HitTextType
    {
        Glyph,
        BeforeText,
        AfterText,
        AfterOfLine,
        BeforeOfLine,
        Undefined
    }

    public struct HitTextResult
    {
        public int CharacterIndex;
        public int LineIndex;
        public int CharacterIndexInLine;
        public HitTextType HitTextType;
    }
}
