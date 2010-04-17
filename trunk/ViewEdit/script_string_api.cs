using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;
using System.ComponentModel;
using System.Drawing;

namespace netCommander.FileView
{
    public class NativeScript
    {
        [DllImport("usp10.dll", SetLastError = true)]
        public static extern int ScriptStringAnalyse
            (IntPtr hdc,
            [MarshalAs(UnmanagedType.LPTStr)]
            string pString,
            int cString,
            int cGlyphs,
            int iCharset,
            ScripStringAnalyseOptions dwFlags,
            int iReqWidth,
            IntPtr psControl, //SCRIPT_CONTROL* 
            IntPtr psState, //SCRIPT_STATE* 
            IntPtr piDx, // const int* 
            ref SCRIPT_TABDEF pTabdef,
            IntPtr pbInClass, //const BYTE* 
            ref IntPtr pssa //SCRIPT_STRING_ANALYSIS* 
            );

        //returns HRESULT
        [DllImport("usp10.dll", SetLastError = true)]
        public static extern int ScriptStringAnalyse
            (IntPtr hdc,
            IntPtr pString,
            int cString,
            int cGlyphs,
            int iCharset,
            ScripStringAnalyseOptions dwFlags,
            int iReqWidth,
            IntPtr psControl, //SCRIPT_CONTROL* 
            IntPtr psState, //SCRIPT_STATE* 
            IntPtr piDx, // const int* 
            ref SCRIPT_TABDEF pTabdef,
            IntPtr pbInClass, //const BYTE* 
            ref IntPtr pssa //SCRIPT_STRING_ANALYSIS* 
            );

        /// <summary>
        /// Converts an x coordinate to a character position.
        /// If the x coordinate corresponds to the leading edge of the character,
        /// the value of piTrailing is 0. If the x coordinate corresponds to the trailing
        /// edge of the character, the value of piTrailing is a positive integer.
        /// As for ScriptXtoCP, the value is 1 for a character that can be rendered on its own.
        /// The value is greater than 1 if the character is part of a cluster in a script
        /// for which cursors are not placed within a cluster, to indicate the offset
        /// to the next legitimate logical cursor position.
        /// If the x coordinate is before the beginning of the line, the function
        /// retrieves -1 for piCh and 1 for piTrailing, indicating the
        /// "trailing edge of the nonexistent character before the line."
        /// If the x coordinate is after the end of the line, the function retrieves for
        /// piCh the first index beyond the length of the line and 0 for piTrailing.
        /// The 0 value indicates the "leading edge of the nonexistent character after the line." 
        /// </summary>
        /// <param name="ssa">A SCRIPT_STRING_ANALYSIS structure for the string</param>
        /// <param name="iX">The x coordinate</param>
        /// <param name="piCh">Pointer to a variable in which this function
        /// retrieves the character position corresponding to the x coordinate. </param>
        /// <param name="piTrailing">Pointer to a variable in which this function
        /// retrieves a value indicating if the x coordinate is for the leading edge
        /// or the trailing edge of the character position.</param>
        /// <returns>HRESULT</returns>
        [DllImport("usp10.dll", SetLastError = true)]
        public static extern int ScriptStringXtoCP
            (IntPtr ssa,
            int iX,
            ref int piCh,
            ref int piTrailing);

        [DllImport("usp10.dll", SetLastError = true)]
        public static extern int ScriptStringCPtoX
            (IntPtr ssa,
            int icp,
            [MarshalAs(UnmanagedType.Bool)]
            bool fTrailing,
            ref int pX);

        [DllImport("usp10.dll", SetLastError = true)]
        public static extern int ScriptStringFree(ref IntPtr pssa);

        [DllImport("Usp10.dll",  SetLastError = true)]
        public static extern IntPtr ScriptString_pcOutChars(IntPtr ssa);

        [DllImport("Usp10.dll",  SetLastError = true)]
        public static extern IntPtr ScriptString_pLogAttr(IntPtr ssa);

        [DllImport("Usp10.dll",  SetLastError = true)]
        public static extern int ScriptStringGetLogicalWidths
            (IntPtr ssa,
            IntPtr piDx     //ref int[] 
            );

        [DllImport("Usp10.dll",  SetLastError = true)]
        public static extern IntPtr ScriptString_pSize(IntPtr ssa);

        [DllImport("Usp10.dll", SetLastError = true)]
        public static extern int ScriptStringOut
            (IntPtr ssa,
            int iX,
            int iY,
            GDI_ExtTextOutOption uOptions,
            ref RECT prc,
            int iMinSel,
            int iMaxSel,
            bool fDisabled);

        [DllImport("Usp10.dll", SetLastError = true)]
        public static extern int ScriptStringOut
            (IntPtr ssa,
            int iX,
            int iY,
            GDI_ExtTextOutOption uOptions,
            IntPtr prc,
            int iMinSel,
            int iMaxSel,
            bool fDisabled);

        public const int S_OK = 0;
        public const int S_FALSE = 1;

        public const int SSA_PASSWORD = 0x00000001;  // Input string contains a single character to be duplicated iLength times
        public const int SSA_TAB = 0x00000002;  // Expand tabs
        public const int SSA_CLIP = 0x00000004;  // Clip string at iReqWidth
        public const int SSA_FIT = 0x00000008;  // Justify string to iReqWidth
        public const int SSA_DZWG = 0x00000010;  // Provide representation glyphs for control characters
        public const int SSA_FALLBACK = 0x00000020;  // Use fallback fonts
        public const int SSA_BREAK = 0x00000040;  // Return break flags (character and word stops)
        public const int SSA_GLYPHS = 0x00000080;  // Generate glyphs, positions and attributes
        public const int SSA_RTL = 0x00000100;  // Base embedding level 1
        public const int SSA_GCP = 0x00000200;  // Return missing glyphs and LogCLust with GetCharacterPlacement conventions
        public const int SSA_HOTKEY = 0x00000400;  // Replace '&' with underline on subsequent codepoint
        public const int SSA_METAFILE = 0x00000800;  // Write items with ExtTextOutW Unicode calls, not glyphs
        public const int SSA_LINK = 0x00001000;  // Apply FE font linking/association to non-complex text
        public const int SSA_HIDEHOTKEY = 0x00002000;  // Remove first '&' from displayed string
        public const int SSA_HOTKEYONLY = 0x00002400;  // Display underline only.

        public static int[] GetLogicalWidths(IntPtr ssa_struct)
        {
            int ssa_chars = GetPcOutChars(ssa_struct);
            int[] ret = new int[ssa_chars];
            GCHandle gch = GCHandle.Alloc(ret, GCHandleType.Pinned);
            IntPtr ret_ptr = gch.AddrOfPinnedObject();
            try
            {
                int res = ScriptStringGetLogicalWidths(ssa_struct, ret_ptr);
                if (res != S_OK)
                {
                    Marshal.ThrowExceptionForHR(res);
                }
            }
            finally
            {
                if ((gch != null) && (gch.IsAllocated))
                {
                    gch.Free();
                }
            }
            return ret;
        }

        public static LogicalCharacterAttribute GetCharacterAttribute(IntPtr buffer, int index)
        {
            byte retB = Marshal.ReadByte(buffer, index);
            return (LogicalCharacterAttribute)retB;
        }

        public static int GetPcOutChars(IntPtr ssa_sctruct_ptr)
        {
            int ret = 0;
            IntPtr ret_ptr = ScriptString_pcOutChars(ssa_sctruct_ptr);
            if (ret_ptr == IntPtr.Zero)
            {
                return ret;
            }
            ret = Marshal.ReadInt32(ret_ptr);
            return ret;
        }

        public static Size GetScriptStringExtent(IntPtr ssa_struct_ptr)
        {
            SIZE retSIZE = new SIZE();
            IntPtr size_ptr = ScriptString_pSize(ssa_struct_ptr);
            if (size_ptr == IntPtr.Zero)
            {
                return new Size();
            }
            retSIZE=(SIZE)Marshal.PtrToStructure(size_ptr,typeof(SIZE));
            return new Size(retSIZE.cx, retSIZE.cy);
        }

        public static IntPtr ScriptStringAnalyseCall
            (IntPtr hdc,
            IntPtr char_buffer,
            int max_char_len,
            int max_width,
            SCRIPT_TABDEF tab_def)
        {
            IntPtr ret=IntPtr.Zero;
            IntPtr actual_buf = char_buffer;
            ScripStringAnalyseOptions opts =
                    ScripStringAnalyseOptions.BREAK |
                    ScripStringAnalyseOptions.CLIP |
                    //ScripStringAnalyseOptions.FALLBACK | -> это вызывает утечку, не освобождаются объекты gdi font
                    ScripStringAnalyseOptions.GLYPHS |
                    //ScripStringAnalyseOptions.LINK |
                    ScripStringAnalyseOptions.TAB;
            int res0 = ScriptStringAnalyse
                (hdc,
                actual_buf,
                max_char_len,
                (int)((max_char_len * 1.5) + 16),
                -1,
                opts,
                max_width,
                IntPtr.Zero,
                IntPtr.Zero,
                IntPtr.Zero,
                ref tab_def,
                IntPtr.Zero,
                ref ret);
            if (res0 != S_OK)
            {
                Marshal.ThrowExceptionForHR(res0);
            }
            return ret;
        }

        public static IntPtr ScriptStringAnalyseCall
            (IntPtr hdc,
            IntPtr char_buffer,
            int max_char_len,
            SCRIPT_TABDEF tab_def)
        {
            IntPtr ret = IntPtr.Zero;
            IntPtr actual_buf = char_buffer;
            ScripStringAnalyseOptions opts =
                    ScripStringAnalyseOptions.BREAK |
                    //ScripStringAnalyseOptions.FALLBACK |
                    ScripStringAnalyseOptions.GLYPHS |
                    //ScripStringAnalyseOptions.LINK |
                    ScripStringAnalyseOptions.TAB;
            int res0 = ScriptStringAnalyse
                (hdc,
                actual_buf,
                max_char_len,
                (int)((max_char_len * 1.5) + 16),
                -1,
                opts,
                0,
                IntPtr.Zero,
                IntPtr.Zero,
                IntPtr.Zero,
                ref tab_def,
                IntPtr.Zero,
                ref ret);
            if (res0 != S_OK)
            {
                Marshal.ThrowExceptionForHR(res0);
            }
            return ret;
        }
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct SCRIPT_TABDEF
    {
        public int cTabStops;
        public int iScale;
        public IntPtr pTabStops;
        public int iTabOrigin;

        public static SCRIPT_TABDEF GetDefault()
        {
            SCRIPT_TABDEF ret = new SCRIPT_TABDEF();
            ret.cTabStops = 0;
            ret.iScale = 0;
            ret.pTabStops = IntPtr.Zero;
            ret.iTabOrigin = 4;
            return ret;
        }
    }

    public struct CharacterPositionInfo
    {
        public int Xoffset;
        public int CharIndex;
        public CharacterPositionType PositionType;
        public int OffsetToNextCaretPosition;

        public static CharacterPositionInfo FromCharIndex(IntPtr ssa_struct, int char_index, CharacterPositionType pos_type)
        {
            if ((pos_type == CharacterPositionType.AfterEnd) ||
                (pos_type == CharacterPositionType.BeforeBeginning) ||
                (pos_type == CharacterPositionType.ClusterPart))
            {
                throw new ArgumentException("Illegal parameter.", "pos_type");
            }

            bool trailing = (pos_type == CharacterPositionType.TrailingEdge);
            int x_offset = 0;
            int res = NativeScript.ScriptStringCPtoX
                (ssa_struct,
                char_index,
                trailing,
                ref x_offset);
            if (res != NativeScript.S_OK)
            {
                Marshal.ThrowExceptionForHR(res);
            }

            CharacterPositionInfo ret = new CharacterPositionInfo();
            ret.CharIndex = char_index;
            ret.PositionType = pos_type;
            ret.Xoffset=x_offset;

            return ret;
        }
                

        public static CharacterPositionInfo FromXoffset(IntPtr ssa_struct, int x_offset)
        {
            int char_pos=0;
            int char_trailing=0;
            if (ssa_struct == IntPtr.Zero)
            {
                x_offset = 0;
                return new CharacterPositionInfo();
            }
            int res = NativeScript.ScriptStringXtoCP
                (ssa_struct,
                x_offset,
                ref char_pos,
                ref char_trailing);
            if (res != NativeScript.S_OK)
            {
                Marshal.ThrowExceptionForHR(res);
            }
            int chars_count = NativeScript.GetPcOutChars(ssa_struct);
            CharacterPositionInfo ret = new CharacterPositionInfo();
            ret.CharIndex = char_pos;
            ret.Xoffset = x_offset;
            if (char_trailing == 0)
            {
                if (char_pos == chars_count)
                {
                    ret.PositionType = CharacterPositionType.AfterEnd;
                }
                else
                {
                    ret.PositionType = CharacterPositionType.LeadingEdge;
                }
            }
            else if (char_trailing == 1)
            {
                if (char_pos == -1)
                {
                    ret.PositionType = CharacterPositionType.BeforeBeginning;
                }
                else
                {
                    ret.PositionType = CharacterPositionType.TrailingEdge;
                }
            }
            else
            {
                ret.PositionType = CharacterPositionType.ClusterPart;
                ret.OffsetToNextCaretPosition = char_trailing;
            }
            return ret;
        }
    }

    public enum CharacterPositionType
    {
        LeadingEdge,
        TrailingEdge,
        ClusterPart,
        BeforeBeginning,
        AfterEnd
    }

    [Flags()]
    public enum ScripStringAnalyseOptions
    {
        PASSWORD = NativeScript.SSA_PASSWORD,  // Input string contains a single character to be duplicated iLength times
        TAB = NativeScript.SSA_TAB,  // Expand tabs
        CLIP = NativeScript.SSA_CLIP,  // Clip string at iReqWidth
        FIT = NativeScript.SSA_FIT,  // Justify string to iReqWidth
        DZWG = NativeScript.SSA_DZWG,  // Provide representation glyphs for control characters
        FALLBACK = NativeScript.SSA_FALLBACK,  // Use fallback fonts
        BREAK = NativeScript.SSA_BREAK,  // Return break flags (character and word stops)
        GLYPHS = NativeScript.SSA_GLYPHS,  // Generate glyphs, positions and attributes
        RTL = NativeScript.SSA_RTL,  // Base embedding level 1
        GCP = NativeScript.SSA_GCP,  // Return missing glyphs and LogCLust with GetCharacterPlacement conventions
        HOTKEY = NativeScript.SSA_HOTKEY,  // Replace '&' with underline on subsequent codepoint
        METAFILE = NativeScript.SSA_METAFILE,  // Write items with ExtTextOutW Unicode calls, not glyphs
        LINK = NativeScript.SSA_LINK,  // Apply FE font linking/association to non-complex text
        HIDEHOTKEY = NativeScript.SSA_HIDEHOTKEY,  // Remove first '&' from displayed string
        HOTKEYONLY = NativeScript.SSA_HOTKEYONLY  // Display underline only.
    }

    [Flags()]
    public enum LogicalCharacterAttribute : byte
    {
        SoftBreak = 0x1,
        WhiteSpace = 0x2,
        CharStop = 0x4,
        WordStop = 0x8,
        Invalid = 0x10,
        None = 0
    }
}
