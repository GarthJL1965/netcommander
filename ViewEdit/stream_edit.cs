using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace netCommander.FileView
{
    public class EditStream : Stream,IDisposable
    {
        /*
         * Идея такая:
         * В связанном списке содержатся дескрипторы кусков (span),
         * каждый дескриптор содержит информацию: ссылка на базовый поток (файл или память), смещение от
         * начала потока, длина.
         * При инициализации дескриптор один, описывающий файловый поток целиком.
         * В процессе редактирования вставки записываются в поток в памяти с созданием соотв.
         * дескрипторов, которые помещаются в связанный список в нужную позицию.
         * Аналогично удаления.
         * Таким образом при редактировании файловый поток не изменяется, а изменяются только дескрипторы,
         * их связанный список и поток в памяти.
         * При вставке символа создается от одного до двух новых дескрипторов (один дескриптор - 20 байт),
         * символ в памяти (от 8 до 32 байт); при удалении создается один новый дескриптор и зписывается
         * 1 байт в память.
         * Чем больше дескртпторов, тем медленнее будет идти поиск.
         */

        private LinkedList<SpanDescriptor> span_list = new LinkedList<SpanDescriptor>();
        private FileStream stream_file = null;
        private string stream_file_name = string.Empty;
        private MemoryStream stream_edits = new MemoryStream();
        private const int save_buffer_len = 32768;

        public EditStream(string file_name)
        {
            init_internal(file_name);
        }

        private LinkedListNode<SpanDescriptor> get_first_span(long position,out long span_offset)
        {
            //просматриваем дескрипторы с начала
            LinkedListNode<SpanDescriptor> node = span_list.First;
            long current_end = 0;
            span_offset = -1;

            while (node != null)
            {
                current_end += node.Value.Length;
                if (position < current_end)
                {
                    //выходим, если конец текущего дескриптора больше, чем position
                    //node и будет искомый
                    //если position вылазиет за пределы, вернется последний span
                    //и sapn_offset будет -1
                    span_offset = position - current_end + node.Value.Length;
                    break;
                }
                node = node.Next;
            }

            return node;
        }

        private void span_split
            (long split_position,
            out LinkedListNode<SpanDescriptor> result_first,
            out LinkedListNode<SpanDescriptor> result_second)
        {
            //найдем span
            long span_offset=0;
            LinkedListNode<SpanDescriptor> span_to_split = get_first_span(split_position, out span_offset);

            //если split_position за логической длиной
            if (span_offset == -1)
            {
                result_first = span_to_split;
                result_second = null;
                return;
            }

            //если split_position - начало span'а
            if (span_offset == 0)
            {
                result_first = null;
                result_second = span_to_split;
                return;
            }

            //иначе создаем span'ы по позиции раздела
            result_first = new LinkedListNode<SpanDescriptor>
                (new SpanDescriptor
                    (span_to_split.Value.DataSource,
                    span_to_split.Value.Offset,
                    span_offset));
            result_second = new LinkedListNode<SpanDescriptor>
                (new SpanDescriptor
                    (span_to_split.Value.DataSource,
                    span_to_split.Value.Offset + span_offset,
                    span_to_split.Value.Length - span_offset));
            //вставляем их последовательно после span_to_split
            span_list.AddAfter(span_to_split, result_first);
            span_list.AddAfter(result_first, result_second);
            // и удаляем исходный span_to_split
            span_list.Remove(span_to_split);
        }

        private SpanDescriptor create_memory_span(byte[] buffer, int offset, int count)
        {
            long write_offset=stream_edits.Length;

            //ставим memory позицию на конец (а может и не надо)
            //stream_edits.Position = stream_edits.Length - 1;

            //записываем байты в поток
            stream_edits.Write(buffer, offset, count);

            //создаем дескриптор
            SpanDescriptor ret = new SpanDescriptor
            (stream_edits,
            write_offset,
            count);

            return ret;
        }

        public bool IsHaveChanges
        {
            get
            {
                return (stream_edits.Length > 0);
            }
        }

        public void SaveEdits(string file_name)
        {
            FileStream stream_save = null;
            try
            {
                Position = 0;
                byte[] save_buffer = new byte[save_buffer_len];
                int bytes_readed = 0;
                long bytes_readed_total = 0;
                stream_save = FileSystemEx.WinAPiFSwrapper.CreateStreamEx
                    (file_name,
                    FileAccess.Write,
                    FileShare.None,
                    FileMode.Create,
                    netCommander.FileSystemEx.CreateFileOptions.SEQUENTIAL_SCAN);
                while (bytes_readed_total < Length)
                {
                    bytes_readed = Read(save_buffer, 0, save_buffer_len);
                    stream_save.Write(save_buffer, 0, bytes_readed);
                    bytes_readed_total += bytes_readed;
                }
            }
            finally
            {
                //теперь можно закрыть save_strem
                if (stream_save != null)
                {
                    stream_save.Flush();
                    stream_save.Close();
                }
            }

            //и открываем файл заново
            init_internal(file_name);
        }

        public void SaveEdits()
        {
            //собираем и сохраняем во временный файл
            string temp_file=string.Empty;
            FileStream stream_temp = null;
            FileStream stream_save = null;

            try
            {
                temp_file = Path.GetTempFileName();
                Position = 0;
                byte[] save_buffer = new byte[save_buffer_len];
                int bytes_readed = 0;
                long bytes_readed_total = 0;
                stream_temp = new FileStream
                (temp_file,
                FileMode.Open,
                FileAccess.ReadWrite,
                FileShare.None);
                while (bytes_readed_total < Length)
                {
                    bytes_readed = Read(save_buffer, 0, save_buffer_len);
                    stream_temp.Write(save_buffer, 0, bytes_readed);
                    bytes_readed_total += bytes_readed;
                }

                stream_temp.Flush();

                //перемещение в оригинадбный файл
                //все было бы просто, но исходный файл может быть и потоком или в нем есть другие потоки
                //простое перемещение временного файла в оригинальный другие поттоки убъет
                //и вообще не сработает перемещение или коприрование в файловый поток
                //поэтому придется открыть исходный файл (или файловый поток) на запись, truncate его 
                //и записывать туда из временного

                stream_file.Close();

                stream_save = FileSystemEx.WinAPiFSwrapper.CreateStreamEx
                    (stream_file_name,
                    FileAccess.Write,
                    FileShare.None,
                    FileMode.Create,
                    netCommander.FileSystemEx.CreateFileOptions.SEQUENTIAL_SCAN);

                stream_temp.Position = 0;
                long temp_len = stream_temp.Length;
                bytes_readed = 0;
                bytes_readed_total = 0;
                while (bytes_readed_total < temp_len)
                {
                    bytes_readed = stream_temp.Read(save_buffer, 0, save_buffer_len);
                    stream_save.Write(save_buffer, 0, bytes_readed);
                    bytes_readed_total += bytes_readed;
                }
            }
            finally
            {
                //теперь можно удалить временный файл и закрыть save_strem
                if (stream_save != null)
                {
                    stream_save.Flush();
                    stream_save.Close();
                }
                if (stream_temp != null)
                {
                    stream_temp.Close();
                }
                if (temp_file != string.Empty)
                {
                    File.Delete(temp_file);
                }
            }
            
            //и открываем файл заново
            init_internal(stream_file_name);
        }

        private void init_internal(string file_name)
        {
            this.stream_file_name = file_name;
            stream_file = FileSystemEx.WinAPiFSwrapper.CreateStreamEx
                (file_name,
                FileAccess.Read,
                FileShare.Read,
                FileMode.Open,
                netCommander.FileSystemEx.CreateFileOptions.RANDOM_ACCESS);

            SpanDescriptor descr = new SpanDescriptor
            (stream_file, 0, stream_file.Length);

            span_list.Clear();
            span_list.AddFirst(descr);

            stream_edits.Close();
            stream_edits = new MemoryStream();

            length_logical = stream_file.Length;
        }

        public void Insert(byte[] buffer, int offset, int count)
        {
            /*
             * где логическая позиция, то есть где начиннается вставка?
             * если точно на начале span'а:
             *      создаем новый span на memory stream и вставляем последний
             *      перед первым
             * если внутри span'а (в том числе и на конце)
             *      этот span разрываем на два
             *      (logical_position-1 - конец первого, logical_position - начало второго),
             *      создаем новый span на memory и вставляем его после разрыва
             * если logical_position==0, то есть в логическом начале
             *      создаем memory span и вставляем его перед самым первым span'ом
             * если logical_position==Length, то есть вставка в логический конец
             *      создаем memory span и вставляем его после самого последнего span'а
             *      
             * в любом случае сначала создаем новый span дескриптор,
             * потом смотрим как его размезщать в span_list
             */

            SpanDescriptor new_descr = create_memory_span(buffer, offset, count);

            if (Position == 0)
            {
                span_list.AddFirst(new_descr);
            }
            else if (Position == Length)
            {
                span_list.AddLast(new_descr);
            }
            else
            {
                LinkedListNode<SpanDescriptor> first_span = null;
                LinkedListNode<SpanDescriptor> second_span = null;
                span_split(Position, out first_span, out second_span);
                if (second_span != null)
                {
                    span_list.AddBefore(second_span, new_descr);
                }
                else
                {
                    throw new ArgumentOutOfRangeException("Position");
                }
            }

            //подправим логическую длину
            length_logical += count;
            //и логическую позицию
            position_logical += count;
        }

        public void DeleteRange(long length)
        {
            /*
             * удаляем от Position (включая) length байт
             * алгоритм примерно как при чтении, но немного другой
             * 
             * Вызываем span_split
             * Удаление начинается с начала второго span'а (их вернет span_split)
             * если длина второго span'а больше, чем скролько байт осталось удалить, снова вызывает span_split 
             * по позиции конца удаления (len) и удаляем первый span;
             * если длина второго span'а меньше или равна, числу оставшихся байт для удаления, удаляем второй span;
             * прибавляем счетчик удаленных байт и повторяем, пока счетчик не достигнет требуемой величины
             */
            long bytes_deleted_total = 0;
            long bytes_deleted = 0;
            long split_position=Position;
            LinkedListNode<SpanDescriptor> first_node = null;
            LinkedListNode<SpanDescriptor> second_node = null;

            while (length > bytes_deleted_total)
            {
                span_split(split_position, out first_node, out second_node);
                if (second_node.Value.Length > length - bytes_deleted_total)
                {
                    split_position = split_position + length;
                    span_split(split_position, out first_node, out second_node);
                    bytes_deleted = first_node.Value.Length;
                    bytes_deleted_total += bytes_deleted;
                    span_list.Remove(first_node);
                    break;
                }
                else
                {
                    bytes_deleted = second_node.Value.Length;
                    bytes_deleted_total += bytes_deleted;
                    span_list.Remove(second_node);
                    split_position = split_position + bytes_deleted;
                }
                //bytes_deleted_total += bytes_deleted;
            }

            if (bytes_deleted_total > 0)
            {
                //записываем нулевой байт, чтоб показать, что было редактирование
                //не развалится память от несколькоих байт
                stream_edits.Write(new byte[] { 0 }, 0, 1);
            }

            //поправим длину
            length_logical -= bytes_deleted_total;
        }

        public override bool CanRead
        {
            get { return true; }
        }

        public override bool CanSeek
        {
            get { return true; }
        }

        public override bool CanWrite
        {
            get { return false; }
        }

        public override void Flush()
        {
            throw new NotImplementedException();
        }

        private long length_logical = -1;
        public override long Length
        {
            get
            {
                if (length_logical == -1)
                {
                    foreach (SpanDescriptor node in span_list)
                    {
                        length_logical += node.Length;
                    }
                }
                return length_logical;
            }
        }

        private long position_logical = 0;
        public override long Position
        {
            get
            {
                return position_logical;
            }
            set
            {
                position_logical = value;
            }
        }

        public override int Read(byte[] buffer, int offset, int count)
        {
            //берем дескриптор для первого куска
            long span_offset = 0;
            LinkedListNode<SpanDescriptor> span = get_first_span(position_logical, out span_offset);

            if ((span_offset == -1)&&(span==null))
            {
                //то есть исходные потоки пусты
                return 0;

                //throw new ArgumentOutOfRangeException("Position");
            }

            int bytes_readed_total = 0;
            int bytes_readed = 0;
            int bytes_to_read = 0;

            while ((span != null) && (bytes_readed_total < count))
            {
                //сколько читаем байт из текущего span?
                if (span.Value.Length - span_offset > count - bytes_readed_total)
                {
                    //если длина span'а больше, чем осталось байт читать
                    bytes_to_read = count - bytes_readed_total;
                }
                else
                {
                    //если span меньше, чем осталось байт заполнить
                    bytes_to_read = (int)(span.Value.Length - span_offset);
                }
                //ставим позицию у потока внутри span'а
                span.Value.DataSource.Position = span.Value.Offset + span_offset;
                //подолжаем (или начинаем) заполнять буфер из потока
                bytes_readed = span.Value.DataSource.Read(buffer, bytes_readed_total + offset, bytes_to_read);
                //обнуляем на случай, если будем читать из следующего span'а
                //следующий (после первого и далее) - всегда с начала span'а
                span_offset = 0;
                //счетчик считанных байт
                bytes_readed_total += bytes_readed;
                //берем следующий span, 
                //есди нет такового, то span будет null и цикл закончится
                span = span.Next;
            }

            position_logical += bytes_readed_total;

            return bytes_readed_total;

        }

        public override long Seek(long offset, SeekOrigin origin)
        {
            switch (origin)
            {
                case SeekOrigin.Begin:
                    position_logical = offset;
                    break;
                case SeekOrigin.Current:
                    position_logical += offset;
                    break;
                case SeekOrigin.End:
                    position_logical = Length + offset;
                    break;
            }

            if (position_logical < 0)
            {
                position_logical = 0;
            }
            else if (position_logical > Length - 1)
            {
                position_logical = Length - 1;
            }

            return position_logical;
        }

        public override void SetLength(long value)
        {
            throw new NotImplementedException();
        }

        public override void Write(byte[] buffer, int offset, int count)
        {
            throw new NotImplementedException();
        }

        public override void Close()
        {
            if (stream_file != null)
            {
                stream_file.Close();
            }
            if (stream_edits != null)
            {
                stream_edits.Close();
            }

            base.Close();
        }

        #region IDisposable Members

        void IDisposable.Dispose()
        {
            if (stream_file != null)
            {
                stream_file.Close();
            }
            if (stream_edits != null)
            {
                stream_edits.Close();
            }
        }

        #endregion
    }

    public class SpanDescriptor
    {
        public Stream DataSource { get; private set; }
        public long Offset { get; private set; }
        public long Length { get; private set; }

        public SpanDescriptor(Stream source,long offset,long length)
        {
            DataSource = source;
            Offset = offset;
            Length = length;
        }
    }
}
