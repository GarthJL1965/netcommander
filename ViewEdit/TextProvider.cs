using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Runtime.InteropServices;

namespace netCommander.FileView
{
    public interface ITextProvider : IDisposable
    {
        void GetCharParagraph(long char_offset, int max_chars, bool BreakOnParagraphSeparator,out IntPtr char_buffer, out int chars_filled, out bool EOF);
        long GetCharsCount();
        Encoding GetEncoding();
        long FindText(string mask_to_find, long start_position);
        Stream GetBaseStream();
    }

    public interface ITextEditProvider : ITextProvider
    {
        void InsertChars(long char_offset, char[] insertion);
        void DeleteChars(long char_offset, long length);
    }

    #region text edit provider multy byte
    public class TextEditProviderMultyByte : TextEditProviderBase
    {
        public TextEditProviderMultyByte(EditStream stream, Encoding encoding)
            : base(stream, encoding)
        {

        }

        private long calc_chars_count(long byte_offset, long byte_len)
        {
            long chunk_offset = byte_offset;
            long ret = 0;
            int readed = 0;
            int readed_total = 0;
            //если заданное смещение выходит за длину файла, возвращаем 0
            if (byte_offset >= stream_edit.Length - 1)
            {
                return ret;
            }
            //ставим текущую позицию файла на заданное смещение
            stream_edit.Position = byte_offset;
            //и начинаем читать и считать chars кусками длиной в один буфер чтения
            while (true)
            {
                //если буфер больше, чем количество байт, которые еще нужно обсчитать,
                //завершаем цикл
                if (readed_total + bytes_buffer.Length >= byte_len)
                {
                    break;
                }
                //читаем байты в буфер
                readed = stream_edit.Read(bytes_buffer, 0, bytes_buffer.Length);
                //обновляем счетчик прочитанных байт
                readed_total = readed_total + readed;
                //считаем сколько chars получается из буфера и суммируем результат
                ret = ret + encoding.GetCharCount(bytes_buffer, 0, readed);
                //обновляем текущую позицию внутри файла
                chunk_offset = chunk_offset + readed;
                //если текущая позиция внутри файла вылезает за границу файла -> завершаем цикл
                if (chunk_offset >= stream_edit.Length - 1)
                {
                    break;
                }
            }
            //обрабатываем оставшийся хвостик
            //если файл кончился, то readed по идее будет 0

            int tail_len = (int)(byte_len - readed_total);
            //если файл кончился, пропускаем...
            if (tail_len <= bytes_buffer.Length)
            {
                readed = stream_edit.Read(bytes_buffer, 0, tail_len);
                ret = ret + encoding.GetCharCount(bytes_buffer, 0, readed);
            }
            return ret;
        }

        protected override long get_chars_count()
        {
            return calc_chars_count(0, stream_edit.Length);
        }

        protected override long get_byte_offset(long char_offset)
        {
            //если char_offset в пределах декодированного буфера...
            if ((char_offset >= chars_buffer_offset) && (char_offset < chars_buffer_offset + chars_buffer.LengthActual))
            {
                //смещение от начала буфера
                int chars_from_first = (int)(char_offset - chars_buffer_offset);
                //скольео они занимают байт
                int bytes_from_first = encoding.GetByteCount(chars_buffer.CharsManaged, 0, chars_from_first + 1);

                //netCommander.Messages.ShowMessage("from buffer");

                return chars_buffer_byte_offset + bytes_from_first;
            }
            
            //а если нет...
            //long initial_pos = stream_base.Position;
            int readed = 0;
            int intermediate_char_count = 0;
            long char_map_point = 0;
            long byte_map_point = 0;
            //с позиции byte_map_point теперь будем искать внутри файла

            if (char_offset > chars_buffer_offset)
            {
                //если заданное смещение "правее" буфера, сканирование начнем
                //от его конца
                //а если нет, то с начала файла
                //ничего не поделаешь - после редактирования
                //кэшированные map points становятся недействительными
                byte_map_point = chars_buffer_byte_offset + chars_buffer_byte_len;
                char_map_point = chars_buffer_offset + chars_buffer.LengthActual;
            }

            int last_char_count = 0;
            long char_offset_from_point = char_offset - char_map_point;
            bool found = false;
            stream_edit.Position = byte_map_point;

            //netCommander.Messages.ShowMessage
            //    (string.Format("byte_map_point={0}", byte_map_point));


            while (true)
            {
                readed = stream_edit.Read(bytes_buffer, 0, bytes_buffer.Length);
                last_char_count = encoding.GetCharCount(bytes_buffer, 0, readed);
                intermediate_char_count = intermediate_char_count + last_char_count;
                if (intermediate_char_count > char_offset_from_point)
                {
                    //в этом куске и есть то что ищем
                    found = true;
                    break;
                }
                else
                {
                    //продолжаем ццикл
                    byte_map_point = byte_map_point + readed;
                }
                if (byte_map_point > stream_edit.Length - 1)
                {
                    //вышли за границы файла
                    found = false;
                    break;
                }
            }

            if (!found)
            {
                return 0;
            }

            char[] chars_buffer_pos = new char[encoding.GetMaxCharCount(bytes_buffer.Length)];

            int decoded = encoding.GetChars(bytes_buffer, 0, readed, chars_buffer_pos, 0);
            int bytes_needed = encoding.GetByteCount(chars_buffer_pos, 0, (int)(char_offset_from_point - (intermediate_char_count - last_char_count)));
            return byte_map_point + bytes_needed;
        }
    }
    #endregion

    #region text edit provider single byte
    public class TextEditProviderSingleByte : TextEditProviderBase
    {
        public TextEditProviderSingleByte(EditStream stream, Encoding encoding)
            : base(stream, encoding)
        {

        }

        protected override long get_chars_count()
        {
            return stream_edit.Length;
        }

        protected override long get_byte_offset(long char_offset)
        {
            return char_offset;
        }
    }
    #endregion

    #region text edit provider base
    public abstract class TextEditProviderBase : ITextEditProvider
    {
        protected const int bytes_buffer_size=32768;
        protected CharBuffer chars_buffer = null;
        protected EditStream stream_edit = null;
        protected byte[] bytes_buffer;
        protected Encoding encoding;
        protected long chars_buffer_offset = 0;
        protected long chars_buffer_byte_offset = 0;
        protected int chars_buffer_byte_len = 0;
        protected bool chars_buffer_at_eof = false;
        protected long chars_count_internal = 0;

        public TextEditProviderBase(EditStream stream,Encoding encoding)
        {
            stream_edit = stream;
            chars_buffer = new CharBuffer(encoding.GetMaxCharCount(bytes_buffer_size));
            this.encoding=encoding;
            bytes_buffer = new byte[bytes_buffer_size];
            chars_count_internal = get_chars_count();
        }

        private bool is_in_chars_buffer(long offset, long length)
        {
            bool ret = false;
            if ((offset >= chars_buffer_offset) && (offset + length <= chars_buffer_offset + chars_buffer.LengthActual))
            {
                ret = true;
            }
            return ret;
        }

        private bool is_out_of_chars_buffer(long offset, long length)
        {
            bool ret = false;
            if ((offset + length < chars_buffer_offset) || (offset >= chars_buffer_offset + chars_buffer.LengthActual))
            {
                ret = true;
            }
            return ret;
        }

        protected abstract long get_chars_count();
        protected abstract long get_byte_offset(long char_offset);

        protected void prepare_char_buffer(long char_offset)
        {
            //берем позицию в байтах
            long byte_pos = get_byte_offset(char_offset);
            //читаем байты
            stream_edit.Position = byte_pos;
            int readed = stream_edit.Read(bytes_buffer, 0, bytes_buffer.Length);
            //декодируем
            int decoded = encoding.GetChars(bytes_buffer, 0, readed, chars_buffer.CharsManaged, 0);
            //обновляем внутренние счетчики
            chars_buffer_offset = char_offset;
            chars_buffer.LengthActual = decoded;
            chars_buffer_byte_offset = byte_pos;
            chars_buffer_byte_len = readed;
            chars_buffer_at_eof = (readed < bytes_buffer.Length);
        }

        protected int find_end_of_paragraph(int offset_in_buffer, int max_chars)
        {
            //find first hard line break
            int max_exam_len = Math.Min(max_chars, chars_buffer.LengthActual - offset_in_buffer);
            int first_hard_break = offset_in_buffer + max_exam_len - 1;
            char cur_char = ' ';
            for (int i = offset_in_buffer; i < max_exam_len + offset_in_buffer; i++)
            {
                cur_char = chars_buffer.CharsManaged[i];
                if (cur_char == '\n')
                {
                    first_hard_break = i;
                    break;
                }
                else if (cur_char == '\r')
                {
                    if ((i + 1 < max_exam_len + offset_in_buffer) && (chars_buffer.CharsManaged[i + 1] == '\n'))
                    {
                        //если последовательность \r\n
                        first_hard_break = i + 1;
                        break;
                    }
                    first_hard_break = i;
                    break;
                }
            }
            return first_hard_break;
        }

        #region ITextEditProvider Members

        public Stream GetBaseStream()
        {
            return stream_edit;
        }

        public virtual void InsertChars(long char_offset, char[] insertion)
        {
            byte[] bytes_to_insert = new byte[encoding.GetMaxByteCount(insertion.Length)];
            int encoded_bytes = encoding.GetBytes(insertion, 0, insertion.Length, bytes_to_insert, 0);
            stream_edit.Position = get_byte_offset(char_offset);
            stream_edit.Insert(bytes_to_insert, 0, encoded_bytes);

            //если вставка в char буфере, то изменяем напрямую его
            if (is_in_chars_buffer(char_offset, insertion.Length))
            {
                chars_buffer.InsertChars((int)(char_offset - chars_buffer_offset), insertion);
            }
            //else if (!is_out_of_chars_buffer(char_offset, insertion.Length))
            //{
            //    //если НЕ вне буфера (и не внутри char буфера)
            //    //значит внутри буфера чиастично
            //    //требуется обновление буфера из источника
            //    prepare_char_buffer(chars_buffer_offset);
            //}
            else
            {
                //если не внутри char буфера, то вставка
                //может быть вне экрана, но может быть и на
                //экране, если вставка в конец файла, в последнем случае требуется обновление char буфера
                if (chars_buffer_at_eof)
                {
                    prepare_char_buffer(chars_buffer_offset);
                }
            }


            chars_count_internal += insertion.Length;
        }

        public virtual void DeleteChars(long char_offset, long length)
        {

            long del_byte_offset = get_byte_offset(char_offset);
            if (del_byte_offset > stream_edit.Length - 1)
            {
                //быстро выходим, если это попытка удалить символ за пределами потока
                return;
            }

            stream_edit.Position=del_byte_offset;
            //
            //нужно вычислить длину для удаления в байтах
            //удаление скорее всего будет в пределах буфера
            //так что вычисление быстрое
            long del_bytes_len =
                get_byte_offset(char_offset + length) - get_byte_offset(char_offset);

            stream_edit.DeleteRange(del_bytes_len);

            //если удаление полностью внутри буфера
            if (is_in_chars_buffer(char_offset, length))
            {
                //а вот здесь уже длина в char'ах
                chars_buffer.DeleteChars((int)(char_offset - chars_buffer_offset), (int)length);
            }
            else if (!is_out_of_chars_buffer(char_offset, length))
            {
                //если НЕ вне буфера (и не внутри char буфера)
                //значит внутри буфера чиастично
                //требуется обновление буфера из источника
                prepare_char_buffer(chars_buffer_offset);
            }

            chars_count_internal -= length;

        }

        #endregion

        #region ITextProvider Members

        public void GetCharParagraph(long char_offset, int max_chars, bool BreakOnParagraphSeparator, out IntPtr char_buffer, out int chars_filled, out bool EOF)
        {
            //если запрошенное в пределах деодированного буфера
            int offset_in_buffer = 0;
            if ((char_offset >= chars_buffer_offset) && (max_chars < chars_buffer.LengthActual - (int)(char_offset - chars_buffer_offset)))
            {
                offset_in_buffer = (int)(char_offset - chars_buffer_offset);
            }
            else if ((char_offset >= chars_buffer_offset) && (chars_buffer_at_eof))
            {
                //декодированнай буфер у конца файла
                offset_in_buffer = (int)(char_offset - chars_buffer_offset);
            }
            else //иначе приготовляем декодированный буфер для char_offset
            {
                prepare_char_buffer(char_offset);
                offset_in_buffer = 0;
            }

            //вычисляем указатель
            char_buffer = chars_buffer.GetAdjustedPointer(offset_in_buffer);

            int end_of_paragraph = offset_in_buffer + max_chars - 1;
            //находим конец параграфа
            if (BreakOnParagraphSeparator)
            {
                end_of_paragraph = find_end_of_paragraph(offset_in_buffer, max_chars);
            }
            chars_filled = end_of_paragraph - offset_in_buffer + 1;
            //chars_filled = end_of_paragraph + 1;

            //проверяем на EOF
            EOF = (chars_buffer_at_eof && (chars_filled >= chars_buffer.LengthActual - offset_in_buffer));
        }

        public long GetCharsCount()
        {
            return chars_count_internal;
        }

        public Encoding GetEncoding()
        {
            return encoding;
        }

        public long FindText(string text_to_find, long start_position)
        {
            char[] readed_chars = new char[encoding.GetMaxCharCount(bytes_buffer.Length)];
            long current_position = get_byte_offset(start_position);
            long stream_len = stream_edit.Length;
            int bytes_readed = 0;
            int chars_decoded = 0;
            int chars_buffer_offset = 0;
            int find_offset = 0;
            char[] chars_find = text_to_find.ToCharArray();
            int chars_find_len = chars_find.Length;
            bool find = false;
            long ret = -1;
            long chars_total = 0;

            stream_edit.Position = current_position;
            while (current_position < stream_len)
            {
                //читаем байты
                bytes_readed = stream_edit.Read(bytes_buffer, 0, bytes_buffer.Length);
                //декодируем
                chars_decoded = encoding.GetChars(bytes_buffer, 0, bytes_readed, readed_chars, 0);
                //ищем
                for (chars_buffer_offset = 0; chars_buffer_offset < chars_decoded; chars_buffer_offset++)
                {
                    for (find_offset = 0; find_offset < chars_find_len; find_offset++)
                    {
                        if ((chars_buffer_offset + find_offset > readed_chars.Length - 1) || (readed_chars[chars_buffer_offset + find_offset] != chars_find[find_offset]))
                        {
                            break;
                        }
                        //если дошло до конца -> нашли
                        find = (find_offset == chars_find_len - 1);
                    }
                    if (find)
                    {
                        break;
                    }
                }
                if (find)
                {
                    ret = start_position + chars_total + chars_buffer_offset;
                    break;
                }

                //если в текущем буфере не нашли, готовим чтение и декодирование следующего куска
                current_position += bytes_readed;
                chars_total += chars_decoded;
            }
            return ret;
        }

        #endregion

        #region IDisposable Members

        public void Dispose()
        {
            chars_buffer.Dispose();
            stream_edit.Close();
        }

        #endregion
    }
    #endregion

    #region factory
    public class TextProviderFactory
    {
        private TextProviderFactory()
        {

        }

        public static ITextProvider CreateProvider(Stream file_stream, Encoding enc)
        {
            if (enc.IsSingleByte)
            {
                //netCommander.Messages.ShowMessage("Use single byte");
                if (file_stream is EditStream)
                {
                    return new TextEditProviderSingleByte((EditStream)file_stream, enc);
                }
                else
                {
                    return new TextProviderFileSingleByte((FileStream)file_stream, enc);
                }
            }
            else
            {
                //netCommander.Messages.ShowMessage("Use multy byte");
                if (file_stream is EditStream)
                {
                    return new TextEditProviderMultyByte((EditStream)file_stream, enc);
                }
                else
                {
                    return new TextProviderFileMultybyte((FileStream)file_stream, enc);
                }
            }
        }
    }
    #endregion

    #region single byte

    public class TextProviderFileSingleByte : ITextProvider
    {
        private const int bytes_buffer_len_default = 32768;
        private int chars_buffer_len = 0;
        private int bytes_buffer_len = 0;
        private char[] chars_buffer = new char[] { };
        private GCHandle chars_buffer_gch = new GCHandle();
        private byte[] bytes_buffer = new byte[] { };
        private Encoding encoding = null;
        private Decoder decoder = null;
        private FileStream stream_base = null;

        private bool char_buffer_at_eof = false;
        private long first_decoded_byte = 0;
        private int decoded_bytes_len = 0;

        public TextProviderFileSingleByte(FileStream file_stream,Encoding enc)
        {
            bytes_buffer_len = bytes_buffer_len_default;
            chars_buffer_len = enc.GetMaxCharCount(bytes_buffer_len);
            chars_buffer = new char[chars_buffer_len];
            bytes_buffer = new byte[bytes_buffer_len];
            chars_buffer_gch = GCHandle.Alloc(chars_buffer, GCHandleType.Pinned);
            encoding = enc;
            decoder = encoding.GetDecoder();
            stream_base = file_stream;
        }

        private void prepare_char_buffer(long byte_offset)
        {
            //читаем байты
            stream_base.Position = byte_offset;
            int readed = stream_base.Read(bytes_buffer, 0, bytes_buffer_len);
            //декодируем
            int decoded = decoder.GetChars(bytes_buffer, 0, readed, chars_buffer, 0);
            //обновляем внутренние счетчики
            first_decoded_byte = byte_offset;
            decoded_bytes_len = readed;
            char_buffer_at_eof = (readed < bytes_buffer_len);
        }

        private IntPtr adjust_char_pointer(int offset_from_first)
        {
            int bytes_from_first = 0;
            bytes_from_first = Encoding.Unicode.GetByteCount(chars_buffer, 0, offset_from_first);

            //почему-то gchandle иногда не инициализируется
            if (!chars_buffer_gch.IsAllocated)
            {
                chars_buffer_gch = GCHandle.Alloc(chars_buffer, GCHandleType.Pinned);
            }

            IntPtr ret = new IntPtr(chars_buffer_gch.AddrOfPinnedObject().ToInt64() + bytes_from_first);
            return ret;
        }

        private int find_end_of_paragraph(int offset_from_first, int max_chars)
        {
            //find first hard line break
            int max_exam_len = Math.Min(max_chars, decoded_bytes_len - offset_from_first);
            int first_hard_break = offset_from_first + max_exam_len - 1;
            char cur_char = ' ';
            for (int i = offset_from_first; i < max_exam_len + offset_from_first; i++)
            {
                cur_char = chars_buffer[i];
                if (cur_char == '\n')
                {
                    first_hard_break = i;
                    break;
                }
                else if (cur_char == '\r')
                {
                    if ((i + 1 < max_exam_len + offset_from_first) && (chars_buffer[i + 1] == '\n'))
                    {
                        //если последовательность \r\n
                        first_hard_break = i + 1;
                        break;
                    }
                    first_hard_break = i;
                    break;
                }
            }
            return first_hard_break;
        }

        #region ITextProvider Members

        /// <summary>
        /// 
        /// </summary>
        /// <param name="text_to_find"></param>
        /// <param name="start_position"></param>
        /// <returns>-1 if not find</returns>
        public long FindText(string text_to_find, long start_position)
        {
            char[] readed_chars = new char[encoding.GetMaxCharCount(bytes_buffer_len)];
            long current_position = start_position;
            long stream_len = stream_base.Length;
            int bytes_readed = 0;
            int chars_decoded = 0;
            int chars_buffer_offset = 0;
            int find_offset = 0;
            char[] chars_find = text_to_find.ToCharArray();
            int chars_find_len = chars_find.Length;
            bool find = false;
            long ret = -1;

            stream_base.Position = current_position;
            while (current_position < stream_len)
            {
                //читаем байты
                bytes_readed = stream_base.Read(bytes_buffer, 0, bytes_buffer_len);
                //декодируем
                chars_decoded= decoder.GetChars(bytes_buffer, 0, bytes_readed, readed_chars, 0);
                //ищем
                for (chars_buffer_offset = 0; chars_buffer_offset < chars_decoded; chars_buffer_offset++)
                {
                    for (find_offset = 0; find_offset < chars_find_len; find_offset++)
                    {
                        if ((chars_buffer_offset + find_offset > readed_chars.Length-1) || (readed_chars[chars_buffer_offset + find_offset] != chars_find[find_offset]))
                        {
                            break;
                        }
                        //если дошло до конца -> нашли
                        find = (find_offset == chars_find_len - 1);
                    }
                    if (find)
                    {
                        break;
                    }
                }
                if (find)
                {
                    ret = current_position + encoding.GetByteCount(readed_chars, 0, chars_buffer_offset);
                    break;
                }
                
                //если в текущем буфере не нашли, готовим чтение и декодирование следующего куска
                current_position += bytes_readed;
            }

            return ret;

        }

        public void GetCharParagraph(long char_offset, int max_chars, bool UseParagraphBreak, out IntPtr char_buffer, out int chars_filled, out bool EOF)
        {
            //если запрошенное в пределах деодированного буфера
            int offset_from_first = 0;
            if ((char_offset >= first_decoded_byte) && (max_chars < decoded_bytes_len - (int)(char_offset - first_decoded_byte)))
            {
                offset_from_first = (int)(char_offset - first_decoded_byte);
            }
            else if ((char_offset >= first_decoded_byte) && (char_buffer_at_eof))
            {
                //декодированнай буфер у конца файла
                offset_from_first = (int)(char_offset - first_decoded_byte);
            }
            else //иначе приготовляем декодированный буфер для char_offset
            {
                prepare_char_buffer(char_offset);
                offset_from_first = 0;
            }

            //вычисляем указатель
            char_buffer = adjust_char_pointer(offset_from_first);

            int end_of_paragraph = offset_from_first + max_chars - 1;
            //находим конец параграфа
            if (UseParagraphBreak)
            {
                end_of_paragraph = find_end_of_paragraph(offset_from_first, max_chars);
            }
            chars_filled = end_of_paragraph - offset_from_first + 1;
            //chars_filled = end_of_paragraph + 1;

            //проверяем на EOF
            EOF = (char_buffer_at_eof && (chars_filled > decoded_bytes_len - offset_from_first));
        }

        public long GetCharsCount()
        {
            return stream_base.Length;
        }

        public Encoding GetEncoding()
        {
            return encoding;
        }

        public Stream GetBaseStream()
        {
            return stream_base;
        }

        #endregion

        #region IDisposable Members

        public void Dispose()
        {
            if ((chars_buffer_gch != null) && (chars_buffer_gch.IsAllocated))
            {
                chars_buffer_gch.Free();
            }
            if (stream_base != null)
            {
                stream_base.Close();
            }
        }

        #endregion
    }

    #endregion

    #region multybyte
    public class TextProviderFileMultybyte : ITextProvider
    {
        private const int bytes_buffer_len_default = 32768;
        private int chars_buffer_len = 0;
        private int bytes_buffer_len = 0;
        private int max_map_points = 128;
        private char[] chars_buffer = new char[] { };
        private GCHandle chars_buffer_gch = new GCHandle();
        private char[] chars_buffer_pos = new char[] { };
        private byte[] bytes_buffer = new byte[] { };
        private Encoding encoding = null;
        private Decoder decoder = null;
        private FileStream stream_base = null;

        private bool char_buffer_at_eof = false;
        private long first_decoded_byte = 0;
        private int decoded_bytes_len = 0;
        private long first_buffered_char = 0;
        private int buffered_chars_len = 0;


        /*********************
         * реализовано
         * *******************
         */
        /// <summary>
        /// key is byte offset, value is char offset
        /// </summary>
        private SortedDictionary<long, long> map_points_table = new SortedDictionary<long, long>();

        public TextProviderFileMultybyte(FileStream file_stream,Encoding enc)
        {
            bytes_buffer_len = bytes_buffer_len_default;
            chars_buffer_len = enc.GetMaxCharCount(bytes_buffer_len);
            //max_map_points = 8096;
            chars_buffer = new char[chars_buffer_len];
            chars_buffer_pos = new char[chars_buffer_len];
            bytes_buffer = new byte[bytes_buffer_len];
            chars_buffer_gch = GCHandle.Alloc(chars_buffer, GCHandleType.Pinned);
            encoding = enc;
            decoder = encoding.GetDecoder();
            stream_base = file_stream;
            init_map();
        }

        #region position handling
        private void init_map()
        {

            if (stream_base.Length < 1048576)
            {
                return;
            }

            //выравниваем map_step по границе char'ов
            long map_step = stream_base.Length / max_map_points;
            map_step = (map_step / encoding.GetMaxByteCount(1)) * encoding.GetMaxByteCount(1);
            long stream_len=stream_base.Length;
            long current_offset = 0;
            long current_chars_count = 0;
            long current_char_offset=0;

            while (current_offset <= stream_len)
            {
                current_chars_count = calc_chars_count(current_offset, map_step);
                current_char_offset += current_chars_count;
                map_points_table.Add(current_offset + map_step, current_char_offset);
                current_offset += map_step;
            }

            //в последнем map_point позиция в байтах будет за пределами файла

            chars_count_internal = current_char_offset;
        }

        private long calc_chars_count(long offset, long byte_len)
        {
            long initial_position = stream_base.Position;
            long chunk_offset = offset;
            long ret = 0;
            int readed = 0;
            int readed_total = 0;
            //если заданное смещение выходит за длину файла, возвращаем 0
            if (offset >= stream_base.Length - 1)
            {
                return ret;
            }
            //ставим текущую позицию файла на заданное смещение
            stream_base.Position = offset;
            //и начинаем читать и считать chars кусками длиной в один буфер чтения
            while (true)
            {
                //если буфер больше, чем количество байт, которые еще нужно обсчитать,
                //завершаем цикл
                if (readed_total + bytes_buffer_len >= byte_len)
                {
                    break;
                }
                //читаем байты в буфер
                readed = stream_base.Read(bytes_buffer, 0, bytes_buffer_len);
                //обновляем счетчик прочитанных байт
                readed_total = readed_total + readed;
                //считаем сколько chars получается из буфера и суммируем результат
                ret = ret + decoder.GetCharCount(bytes_buffer, 0, readed,false);
                //обновляем текущую позицию внутри файла
                chunk_offset = chunk_offset + readed;
                //если текущая позиция внутри файла вылезает за границу файла -> завершаем цикл
                if (chunk_offset >= stream_base.Length - 1)
                {
                    break;
                }
            }
            //обрабатываем оставшийся хвостик
            //если файл кончился, то readed по идее будет 0

            int tail_len = (int)(byte_len - readed_total);
            //если файл кончился, пропускаем...
            if (tail_len <= bytes_buffer.Length)
            {
                readed = stream_base.Read(bytes_buffer, 0, tail_len);
                ret = ret + decoder.GetCharCount(bytes_buffer, 0, readed, true);
            }
            //восстанавливаем текущую позицию
            stream_base.Position = initial_position;
            return ret;
        } 

        private long calc_byte_offset(long char_offset)
        {
            //если char_offset в пределах декодированного буфера...
            if ((char_offset >= first_buffered_char) && (char_offset < first_buffered_char + buffered_chars_len))
            {
                //смещение от начала буфера
                int chars_from_first = (int)(char_offset - first_buffered_char);
                //скольео они занимают байт
                int bytes_from_first = encoding.GetByteCount(chars_buffer, 0, chars_from_first + 1);

                //netCommander.Messages.ShowMessage("from buffer");

                return first_decoded_byte + bytes_from_first;
            }

            //а если нет...
            long initial_pos = stream_base.Position;
            int readed = 0;
            long byte_map_point = 0;
            long char_map_point = 0;
            long char_offset_from_point = 0;
            int intermediate_char_count = 0;
            KeyValuePair<long, long> pre_map_point = new KeyValuePair<long, long>(0, 0);
            //сканируем сохраненные map points
            foreach (KeyValuePair<long, long> map_point_pair in map_points_table)
            {
                //если у очередного map_point смещение в chars больше заданного,
                //значит предыдущий map_point - и есть тот который ищем
                //то есть с которого будем искать точную позицию внутри файла
                if (map_point_pair.Value > char_offset)
                {
                    byte_map_point = pre_map_point.Key;
                    char_map_point = pre_map_point.Value;
                    break;
                }
                pre_map_point = map_point_pair;
            }

            //с позиции byte_map_point теперь будем искать внутри файла
            int last_char_count = 0;
            char_offset_from_point = char_offset - char_map_point;
            bool found = false;
            stream_base.Position = byte_map_point;

            //netCommander.Messages.ShowMessage
            //    (string.Format("byte_map_point={0}", byte_map_point));


            while (true)
            {
                readed = stream_base.Read(bytes_buffer, 0, bytes_buffer_len);
                last_char_count = decoder.GetCharCount(bytes_buffer, 0, readed);
                intermediate_char_count = intermediate_char_count + last_char_count;
                if (intermediate_char_count > char_offset_from_point)
                {
                    //в этом куске и есть то что ищем
                    found = true;
                    break;
                }
                else
                {
                    //продолжаем ццикл
                    byte_map_point = byte_map_point + readed;
                }
                if (byte_map_point > stream_base.Length - 1)
                {
                    //вышли за границы файла
                    found = false;
                    break;
                }
            }

            if (!found)
            {
                stream_base.Position = initial_pos;
                return 0;
            }

            int decoded = decoder.GetChars(bytes_buffer, 0, readed,chars_buffer_pos, 0);
            int bytes_needed = encoding.GetByteCount(chars_buffer_pos, 0, (int)(char_offset_from_point - (intermediate_char_count - last_char_count)));
            stream_base.Position = initial_pos;
            return byte_map_point + bytes_needed;
        } 
        #endregion

        public Encoding GetEncoding()
        {
            return encoding;
        }

        private void prepare_char_buffer(long char_offset)
        {
            //берем позицию в байтах
            long byte_pos = calc_byte_offset(char_offset);
            //читаем байты
            stream_base.Position = byte_pos;
            int readed = stream_base.Read(bytes_buffer, 0, bytes_buffer_len);
            //декодируем
            int decoded = decoder.GetChars(bytes_buffer, 0, readed, chars_buffer, 0);
            //обновляем внутренние счетчики
            first_buffered_char = char_offset;
            buffered_chars_len = decoded;
            first_decoded_byte = byte_pos;
            decoded_bytes_len = readed;
            char_buffer_at_eof = (readed < bytes_buffer_len);
        }

        private IntPtr adjust_char_pointer(int offset_from_first)
        {
            int bytes_from_first = 0;
            //можно попробовать взять CharSize из System.Text.UnicodeEncoding
            //тогда не неужно будет каждый раз рассчитывать кодировку
            bytes_from_first = Encoding.Unicode.GetByteCount(chars_buffer, 0, offset_from_first);

            //почему-то gchandle иногда не инициализируется
            if (!chars_buffer_gch.IsAllocated)
            {
                chars_buffer_gch = GCHandle.Alloc(chars_buffer, GCHandleType.Pinned);
            }

            IntPtr ret = new IntPtr(chars_buffer_gch.AddrOfPinnedObject().ToInt64() + bytes_from_first);
            return ret;
        }

        private int find_end_of_paragraph(int offset_from_first, int max_chars)
        {
            //find first hard line break
            int max_exam_len = Math.Min(max_chars, buffered_chars_len - offset_from_first);
            int first_hard_break = offset_from_first + max_exam_len - 1;
            char cur_char = ' ';
            for (int i = offset_from_first; i < max_exam_len + offset_from_first; i++)
            {
                cur_char = chars_buffer[i];
                if (cur_char == '\n')
                {
                    first_hard_break = i;
                    break;
                }
                else if (cur_char == '\r')
                {
                    if ((i + 1 < max_exam_len + offset_from_first) && (chars_buffer[i + 1] == '\n'))
                    {
                        //если последовательность \r\n
                        first_hard_break = i + 1;
                        break;
                    }
                    first_hard_break = i;
                    break;
                }
            }
            return first_hard_break;
        }

        #region ITextProvider Members

        /// <summary>
        /// char position
        /// </summary>
        /// <param name="text_to_find"></param>
        /// <param name="start_position"></param>
        /// <returns>-1 if not find</returns>
        public long FindText(string text_to_find, long start_position)
        {
            char[] readed_chars = new char[encoding.GetMaxCharCount(bytes_buffer_len)];
            long current_position = calc_byte_offset(start_position);
            long stream_len = stream_base.Length;
            int bytes_readed = 0;
            int chars_decoded = 0;
            int chars_buffer_offset = 0;
            int find_offset = 0;
            char[] chars_find = text_to_find.ToCharArray();
            int chars_find_len = chars_find.Length;
            bool find = false;
            long ret = -1;
            long chars_total = 0;

            stream_base.Position = current_position;
            while (current_position < stream_len)
            {
                //читаем байты
                bytes_readed = stream_base.Read(bytes_buffer, 0, bytes_buffer_len);
                //декодируем
                chars_decoded = decoder.GetChars(bytes_buffer, 0, bytes_readed, readed_chars, 0);
                //ищем
                for (chars_buffer_offset = 0; chars_buffer_offset < chars_decoded; chars_buffer_offset++)
                {
                    for (find_offset = 0; find_offset < chars_find_len; find_offset++)
                    {
                        if ((chars_buffer_offset + find_offset > readed_chars.Length - 1) || (readed_chars[chars_buffer_offset + find_offset] != chars_find[find_offset]))
                        {
                            break;
                        }
                        //если дошло до конца -> нашли
                        find = (find_offset == chars_find_len - 1);
                    }
                    if (find)
                    {
                        break;
                    }
                }
                if (find)
                {
                    ret = start_position + chars_total + chars_buffer_offset;
                    break;
                }

                //если в текущем буфере не нашли, готовим чтение и декодирование следующего куска
                current_position += bytes_readed;
                chars_total += chars_decoded;
            }

           

            return ret;

        }

        private long chars_count_internal=-1;
        public long GetCharsCount()
        {
            if (chars_count_internal == -1)
            {
                chars_count_internal = calc_chars_count(0, stream_base.Length);
            }
            return chars_count_internal;
        }

        public void GetCharParagraph(long char_offset, int max_chars, bool UseParagraphBreak,out IntPtr char_buffer, out int chars_filled, out bool EOF)
        {
            
            //если запрошенное в пределах деодированного буфера
            int offset_from_first = 0;
            if ((char_offset >= first_buffered_char) && (max_chars < buffered_chars_len - (int)(char_offset - first_buffered_char)))
            {
                offset_from_first = (int)(char_offset - first_buffered_char);
            }
            else if ((char_offset >= first_buffered_char) && (char_buffer_at_eof))
            {
                //декодированнай буфер у конца файла
                offset_from_first = (int)(char_offset - first_buffered_char);
            }
            else //иначе приготовляем декодированный буфер для char_offset
            {
                prepare_char_buffer(char_offset);
                offset_from_first = 0;
            }

            //вычисляем указатель
            char_buffer = adjust_char_pointer(offset_from_first);

            int end_of_paragraph = offset_from_first + max_chars - 1;
            //находим конец параграфа
            if (UseParagraphBreak)
            {
                end_of_paragraph = find_end_of_paragraph(offset_from_first, max_chars);
            }
             chars_filled = end_of_paragraph - offset_from_first + 1;
            //chars_filled = end_of_paragraph + 1;

            //проверяем на EOF
            EOF = (char_buffer_at_eof && (chars_filled > buffered_chars_len - offset_from_first));

            //if (EOF)
            //{
            //    int debug_point = 0;
            //}

            //вроде все
        }

        public Stream GetBaseStream()
        {
            return stream_base;
        }
        
        #endregion

        #region IDisposable Members

        public void Dispose()
        {
            if ((chars_buffer_gch != null) && (chars_buffer_gch.IsAllocated))
            {
                chars_buffer_gch.Free();
            }
            if (stream_base != null)
            {
                stream_base.Close();
            }
        }

        #endregion
    }
    #endregion

    #region fixed buffer

    public class CharBuffer : IDisposable
    {
        [DllImport("Kernel32.dll", EntryPoint = "RtlMoveMemory", SetLastError = false)]
        private static extern void MoveMemory(IntPtr dest, IntPtr src, int size);

        private int char_len = System.Text.UnicodeEncoding.CharSize;
        private char[] chars_managed;
        private GCHandle chars_gch = new GCHandle();

        public CharBuffer(int buffer_len)
        {
            chars_managed = new char[buffer_len];
            chars_gch = GCHandle.Alloc(chars_managed, GCHandleType.Pinned);
        }

        public int LengthActual { get; set; }
        public int Length
        {
            get
            {
                return chars_managed.Length;
            }
        }

        public char[] CharsManaged
        {
            get
            {
                return chars_managed;
            }
        }

        public IntPtr GetAdjustedPointer(int offset)
        {
            return new IntPtr
                (chars_gch.AddrOfPinnedObject().ToInt64() +
                char_len * offset);
        }

        public void InsertChars(int start_insert, char[] insertion)
        {
            if (insertion.Length < chars_managed.Length - start_insert + 1)
            {
                IntPtr dest = GetAdjustedPointer(start_insert + insertion.Length);
                IntPtr src = GetAdjustedPointer(start_insert);
                int size = char_len * (chars_managed.Length - start_insert - insertion.Length+1);
                MoveMemory(dest, src, size);
            }

            //иначе двигать память не надо - просто перезаписываем весь хвост
            IntPtr destination=GetAdjustedPointer(start_insert);
            int insert_actual_len = Math.Min(insertion.Length, chars_managed.Length - start_insert + 1);
            Marshal.Copy
                (insertion,
                0,
                destination,
                insert_actual_len);

            //поправим актуальную длину - это важно, когда вставка рядом с концом файла
            //в этом случае LengthActual меньше максимальной длины буфера
            int new_actual_len = LengthActual + insertion.Length;
            if (new_actual_len < chars_managed.Length)
            {
                LengthActual = new_actual_len;
            }
        }

        public void DeleteChars(int start_delete, int length_delete)
        {
            if (length_delete < chars_managed.Length - start_delete + 1)
            {
                IntPtr dest = GetAdjustedPointer(start_delete);
                IntPtr src = GetAdjustedPointer(start_delete + length_delete);
                int size = char_len * (chars_managed.Length - start_delete - length_delete + 1);
                MoveMemory(dest, src, size);
                LengthActual -= length_delete;
            }
            else
            {
                LengthActual = start_delete;
            }
        }

        #region IDisposable Members

        public void Dispose()
        {
            if ((chars_gch != null) && (chars_gch.IsAllocated))
            {
                chars_gch.Free();
            }
        }

        #endregion
    }

    #endregion
}
