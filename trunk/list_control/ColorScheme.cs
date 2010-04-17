using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using netCommander.winControls;
using System.IO;

namespace netCommander
{

    public class ColorScheme
    {
        private List<ItemColorSchemeEntry> internal_list = new List<ItemColorSchemeEntry>();

        public static ColorScheme SystemDefault
        {
            get
            {
                ColorScheme ret = new ColorScheme();
                ret.internal_list.Add(ItemColorSchemeEntry.SystemDefault);
                return ret;
            }
        }

        public static ColorScheme Parse(string file_name)
        {
            StreamReader reader = null;
            ColorScheme ret = new ColorScheme();

            try
            {
                reader = new StreamReader(file_name, Encoding.UTF8);
                string line = string.Empty;
                while ((line = reader.ReadLine()) != null)
                {
                    line = line.Trim();
                    if (line == string.Empty)
                    {
                        continue;
                    }
                    if (line.StartsWith("#"))
                    {
                        continue;
                    }
                    ItemColorSchemeEntry new_entry = ItemColorSchemeEntry.Parse(line);
                    ret.internal_list.Add(new_entry);
                    //line = reader.ReadLine();
                }
            }
            catch (Exception)
            {
            }
            finally
            {
                if (reader != null)
                {
                    reader.Close();
                }
            }

            if (ret.internal_list.Count == 0)
            {
                ret.internal_list.Add(ItemColorSchemeEntry.SystemDefault);
            }
            return ret;
        }

        public ItemColors GetColors(string item_name, ItemState state, ItemCategory category)
        {
            ItemColorState finded = ItemColorState.SystemDefault;
            bool match = false;

            //Color c = Color.Black;


            foreach (ItemColorSchemeEntry entry in internal_list)
            {
                if ((entry.Category & category) == category)
                {
                    match = false;
                    foreach (string mask in entry.Masks)
                    {
                        if (Wildcard.Match(mask, item_name, false))
                        {
                            finded = entry.ItemColorState;
                            match = true;
                            break;
                        }
                    }
                    if (match)
                    {
                        break;
                    }
                }
            }

            switch (state)
            {
                case ItemState.None:
                    return finded.DefaultState;

                case ItemState.Focused | ItemState.Selected:
                    return finded.SelectedFocusedState;

                case ItemState.Focused:
                    return finded.FocusedState;

                case ItemState.Selected:
                    return finded.SelectedState;

                default:
                    return finded.DefaultState;
            }
        }
    }

    public struct ItemColorSchemeEntry
    {
        public ItemColorState ItemColorState;
        public ItemCategory Category;
        public string[] Masks;

        public static ItemColorSchemeEntry SystemDefault
        {
            get
            {
                ItemColorSchemeEntry ret = new ItemColorSchemeEntry();
                ret.Category = ItemCategory.Default | ItemCategory.Container | ItemCategory.Hidden;
                ret.ItemColorState = ItemColorState.SystemDefault;
                ret.Masks = new string[] { "*" };
                return ret;
            }
        }

        //TODO написать парсер, чтобы читать из файла
        //ColorScheme должен будет передавать строку из файла для распарсивания
        public static ItemColorSchemeEntry Parse(string line)
        {
            ItemColorSchemeEntry ret = ItemColorSchemeEntry.SystemDefault;

            //
            //[category][space][color1][space][color2][space]...[color8][space][mask1][;][mask2][;]...
            //
            try
            {
                string[] items1 = line.Split(new char[] { ' ', '\t' }, StringSplitOptions.RemoveEmptyEntries);
                ret.Category = (ItemCategory)int.Parse(items1[0]);
                ItemColorState i_state = new ItemColorState();

                string hex_prefix="0x";

                i_state.DefaultState = ItemColors.Create(Color.FromArgb(int.Parse(items1[1].Replace(hex_prefix, string.Empty), System.Globalization.NumberStyles.HexNumber)), Color.FromArgb(int.Parse(items1[2].Replace(hex_prefix, string.Empty), System.Globalization.NumberStyles.HexNumber)));
                i_state.FocusedState = ItemColors.Create(Color.FromArgb(int.Parse(items1[3].Replace(hex_prefix, string.Empty), System.Globalization.NumberStyles.HexNumber)), Color.FromArgb(int.Parse(items1[4].Replace(hex_prefix, string.Empty), System.Globalization.NumberStyles.HexNumber)));
                i_state.SelectedState = ItemColors.Create(Color.FromArgb(int.Parse(items1[5].Replace(hex_prefix, string.Empty), System.Globalization.NumberStyles.HexNumber)), Color.FromArgb(int.Parse(items1[6].Replace(hex_prefix, string.Empty), System.Globalization.NumberStyles.HexNumber)));
                i_state.SelectedFocusedState = ItemColors.Create(Color.FromArgb(int.Parse(items1[7].Replace(hex_prefix, string.Empty), System.Globalization.NumberStyles.HexNumber)), Color.FromArgb(int.Parse(items1[8].Replace(hex_prefix, string.Empty), System.Globalization.NumberStyles.HexNumber)));
                string[] masks = items1[9].Split(new char[] { ';' }, StringSplitOptions.RemoveEmptyEntries);
                ret.ItemColorState = i_state;
                ret.Masks = masks;
            }
            catch (Exception)
            {
            }

            return ret;
        }
    }

    public struct ItemColorState
    {
        public ItemColors DefaultState;
        public ItemColors SelectedState;
        public ItemColors FocusedState;
        public ItemColors SelectedFocusedState;

        public static ItemColorState SystemDefault
        {
            get
            {
                ItemColorState ret = new ItemColorState();
                ret.DefaultState = ItemColors.SystemDefault;
                ret.FocusedState = ItemColors.Create(SystemColors.MenuHighlight, SystemColors.WindowText);
                ret.SelectedState = ItemColors.Create(SystemColors.Highlight, SystemColors.HighlightText);
                ret.SelectedFocusedState = ItemColors.Create(SystemColors.MenuHighlight, SystemColors.HighlightText);
                return ret;
            }
        }
    }

    public struct ItemColors
    {
        public Color BackgroundColor;
        public Color ForegroundColor;

        public static ItemColors Create(Color background, Color foreground)
        {
            ItemColors ret = new ItemColors();
            ret.BackgroundColor = background;
            ret.ForegroundColor = foreground;
            return ret;
        }

        public static ItemColors SystemDefault
        {
            get
            {
                ItemColors ret = new ItemColors();
                ret.BackgroundColor = SystemColors.Window;
                ret.ForegroundColor = SystemColors.WindowText;
                return ret;
            }
        }
    }

    public class BrushCache : IDisposable
    {
        private Dictionary<Color, SolidBrush> internal_list = new Dictionary<Color, SolidBrush>();

        public SolidBrush GetBrush(Color color)
        {
            if (internal_list.ContainsKey(color))
            {
                return internal_list[color];
            }
            SolidBrush new_brush = new SolidBrush(color);
            internal_list.Add(color, new_brush);
            return new_brush;
        }

        public void Clear()
        {
            foreach (KeyValuePair<Color, SolidBrush> kvp in internal_list)
            {
                kvp.Value.Dispose();
            }
            internal_list.Clear();
        }

        public void Dispose()
        {
            Clear();
        }
    }

    [Flags()]
    public enum ItemCategory
    {
        Default = 0x1,
        Container =0x2,
        Hidden = 0x4
    }
}
