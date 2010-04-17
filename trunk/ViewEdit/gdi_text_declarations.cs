using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;
using System.ComponentModel;
using System.Drawing;

namespace netCommander.FileView
{
    public class NativeGdi
    {
        [DllImport("gdi32.dll", ExactSpelling = true, SetLastError = false)]
        public static extern int GdiFlush();

        /// <summary>
        /// he SelectObject function selects an object into the specified device context (DC).
        /// The new object replaces the previous object of the same type.
        /// This function returns the previously selected object of the specified type.
        /// An application should always replace a new object with the original,
        /// default object after it has finished drawing with the new object.
        /// </summary>
        /// <param name="hdc">Handle to the DC. </param>
        /// <param name="hgdiobj">Handle to the object to be selected.</param>
        /// <returns>If the selected object is not a region and the function succeeds,
        /// the return value is a handle to the object being replaced.
        /// If the selected object is a region and the function succeeds,
        /// the return value is one of the following values: 
        /// SIMPLEREGION 	Region consists of a single rectangle.
        /// COMPLEXREGION 	Region consists of more than one rectangle.
        /// NULLREGION 	Region is empty.
        /// If an error occurs and the selected object is not a region,
        /// the return value is NULL. Otherwise, it is HGDI_ERROR. </returns>
        [DllImport("gdi32.dll", ExactSpelling = true, SetLastError = true)]
        public static extern IntPtr SelectObject(IntPtr hdc, IntPtr hgdiobj);

        [DllImport("gdi32.dll", ExactSpelling = true, SetLastError = true)]
        public static extern int DeleteObject(IntPtr hObject);

        /// <summary>
        /// The SetBkColor function sets the current background color to the specified color value,
        /// or to the nearest physical color if the device cannot represent the specified color value.
        /// </summary>
        /// <param name="hdc"></param>
        /// <param name="color_ref">pecifies the new background color. </param>
        /// <returns>If the function succeeds, the return value specifies
        /// the previous background color as a COLORREF value.
        /// If the function fails, the return value is CLR_INVALID. </returns>
        [DllImport("gdi32.dll", ExactSpelling = true, SetLastError = true)]
        public static extern uint SetBkColor(IntPtr hdc, uint color_ref);

        /// <summary>
        /// The SetBkMode function sets the background mix mode of the specified device context.
        /// The background mix mode is used with text,
        /// hatched brushes, and pen styles that are not solid lines.
        /// </summary>
        /// <param name="hdc"></param>
        /// <param name="iBkMode"></param>
        /// <returns>If the function succeeds, the return value specifies the previous background mode.
        /// If the function fails, the return value is zero.</returns>
        [DllImport("gdi32.dll", ExactSpelling = true, SetLastError = true)]
        public static extern GDI_backgroundMode SetBkMode(IntPtr hdc, GDI_backgroundMode iBkMode);

        /// <summary>
        /// The SetTextAlign function sets the text-alignment flags for the specified device context. 
        /// </summary>
        /// <param name="hdc"></param>
        /// <param name="fMode">combination of GDI_TextAlignment, GDI_LineAlignment and GDI_UpdateCurrentPosition</param>
        /// <returns>If the function succeeds, the return value is the previous text-alignment setting.
        /// If the function fails, the return value is GDI_ERROR.</returns>
        [DllImport("gdi32.dll", ExactSpelling = true, SetLastError = true)]
        public static extern uint SetTextAlign(IntPtr hdc, uint fMode);

        /// <summary>
        /// The SetTextCharacterExtra function sets the intercharacter spacing.
        /// Intercharacter spacing is added to each character, including break characters,
        /// when the system writes a line of text.
        /// </summary>
        /// <param name="hdc"></param>
        /// <param name="nCharExtra">Specifies the amount of extra space, in logical units,
        /// to be added to each character. If the current mapping mode is not MM_TEXT,
        /// the nCharExtra parameter is transformed and rounded to the nearest pixel.</param>
        /// <returns>If the function succeeds, the return value is the previous intercharacter spacing.
        /// If the function fails, the return value is 0x80000000.</returns>
        [DllImport("gdi32.dll", ExactSpelling = true, SetLastError = true)]
        public static extern int SetTextCharacterExtra(IntPtr hdc, int nCharExtra);

        /// <summary>
        /// The SetTextColor function sets the text color for the specified
        /// device context to the specified color.
        /// </summary>
        /// <param name="hdc"></param>
        /// <param name="color_ref"></param>
        /// <returns>If the function succeeds, the return value is a color reference
        /// for the previous text color as a COLORREF value.If the function fails,
        /// the return value is CLR_INVALID.</returns>
        [DllImport("gdi32.dll", ExactSpelling = true, SetLastError = true)]
        public static extern uint SetTextColor(IntPtr hdc, uint color_ref);

        /// <summary>
        /// The GetTextExtentPoint32 function computes the width and height of the specified string of text. 
        /// </summary>
        /// <param name="hdc"></param>
        /// <param name="lpString">Pointer to a buffer that specifies the text string. The string does not need
        /// to be null-terminated, because the c parameter specifies the length of the string. </param>
        /// <param name="c">Specifies the length of the string pointed to by lpString.</param>
        /// <param name="lpSize">Pointer to a SIZE structure that receives
        /// the dimensions of the string, in logical units. </param>
        /// <returns>If the function succeeds, the return value is nonzero.</returns>
        [DllImport("gdi32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        public static extern int GetTextExtentPoint32
          (IntPtr hdc,
          [MarshalAs(UnmanagedType.LPTStr)]
          string lpString,
          int c,
          ref SIZE lpSize);

        /// <summary>
        /// The GetTextExtentExPoint function retrieves the number of characters
        /// in a specified string that will fit within a specified space and fills
        /// an array with the text extent for each of those characters.
        /// (A text extent is the distance between the beginning of the space
        /// and a character that will fit in the space.)
        /// This information is useful for word-wrapping calculations.
        /// </summary>
        /// <param name="hdc"></param>
        /// <param name="lpszStr"></param>
        /// <param name="cchString"></param>
        /// <param name="nMaxExtent"></param>
        /// <param name="lpnFit"></param>
        /// <param name="alpDx"></param>
        /// <param name="lpSize"></param>
        /// <returns>If the function succeeds, the return value is nonzero.</returns>
        [DllImport("gdi32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        public static extern int GetTextExtentExPoint
            (IntPtr hdc,         // handle to DC
            [MarshalAs(UnmanagedType.LPTStr)]
            string lpszStr, // character string
            int cchString,   // number of characters
            int nMaxExtent,  // maximum width of formatted string
            ref int lpnFit,    // maximum number of characters
            ref int alpDx,     // pointer to first element of array of partial string widths
            ref SIZE lpSize    // string dimensions
            );

        /// <summary>
        /// The GetTextExtentExPoint function retrieves the number of characters
        /// in a specified string that will fit within a specified space and fills
        /// an array with the text extent for each of those characters.
        /// (A text extent is the distance between the beginning of the space
        /// and a character that will fit in the space.)
        /// This information is useful for word-wrapping calculations.
        /// </summary>
        /// <param name="hdc"></param>
        /// <param name="lpszStr"></param>
        /// <param name="cchString"></param>
        /// <param name="nMaxExtent"></param>
        /// <param name="lpnFit"></param>
        /// <param name="alpDx"></param>
        /// <param name="lpSize"></param>
        /// <returns>If the function succeeds, the return value is nonzero.</returns>
        [DllImport("gdi32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        public static extern int GetTextExtentExPoint
            (IntPtr hdc,         // handle to DC
            IntPtr lpszStr, // character string
            int cchString,   // number of characters
            int nMaxExtent,  // maximum width of formatted string
            ref int lpnFit,    // maximum number of characters
            IntPtr alpDx,     // pointer to first element of array of partial string widths
            ref SIZE lpSize    // string dimensions
            );

        /// <summary>
        /// The GetTabbedTextExtent function computes the width and height of a character string.
        /// If the string contains one or more tab characters, the width of the string
        /// is based upon the specified tab stops. The GetTabbedTextExtent function
        /// uses the currently selected font to compute the dimensions of the string.
        /// </summary>
        /// <param name="hDC"></param>
        /// <param name="lpString"></param>
        /// <param name="nCount">Specifies the length of the text string. For the ANSI function
        /// it is a BYTE count and for the Unicode function it is a WORD count. Note that
        /// for the ANSI function, characters in SBCS code pages take one byte each,
        /// while most characters in DBCS code pages take two bytes; for the Unicode function,
        /// most currently defined Unicode characters (those in the Basic Multilingual Plane (BMP))
        /// are one WORD while Unicode surrogates are two WORDs. </param>
        /// <param name="nTabPositions">Specifies the number of tab-stop positions in the array pointed
        /// to by the lpnTabStopPositions parameter.</param>
        /// <param name="lpnTabStopPositions">Pointer to an array containing the tab-stop positions,
        /// in device units. The tab stops must be sorted in increasing order; the smallest x-value
        /// should be the first item in the array.</param>
        /// <returns>If the function succeeds, the return value is the dimensions
        /// of the string in logical units. The height is in the high-order
        /// word and the width is in the low-order word.If the function fails,
        /// the return value is 0.</returns>
        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        public static extern uint GetTabbedTextExtent
            (IntPtr hDC,                        // handle to DC
            [MarshalAs(UnmanagedType.LPTStr)]
            string lpString,               // character string
            int nCount,                     // number of characters
            int nTabPositions,              // number of tab positions
            ref int lpnTabStopPositions // array of tab positions
            );

        /// <summary>
        /// The GetTabbedTextExtent function computes the width and height of a character string.
        /// If the string contains one or more tab characters, the width of the string
        /// is based upon the specified tab stops. The GetTabbedTextExtent function
        /// uses the currently selected font to compute the dimensions of the string.
        /// </summary>
        /// <param name="hDC"></param>
        /// <param name="lpString"></param>
        /// <param name="nCount">Specifies the length of the text string. For the ANSI function
        /// it is a BYTE count and for the Unicode function it is a WORD count. Note that
        /// for the ANSI function, characters in SBCS code pages take one byte each,
        /// while most characters in DBCS code pages take two bytes; for the Unicode function,
        /// most currently defined Unicode characters (those in the Basic Multilingual Plane (BMP))
        /// are one WORD while Unicode surrogates are two WORDs. </param>
        /// <param name="nTabPositions">Specifies the number of tab-stop positions in the array pointed
        /// to by the lpnTabStopPositions parameter.</param>
        /// <param name="lpnTabStopPositions">Pointer to an array containing the tab-stop positions,
        /// in device units. The tab stops must be sorted in increasing order; the smallest x-value
        /// should be the first item in the array.</param>
        /// <returns>If the function succeeds, the return value is the dimensions
        /// of the string in logical units. The height is in the high-order
        /// word and the width is in the low-order word.If the function fails,
        /// the return value is 0.</returns>
        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        public static extern uint GetTabbedTextExtent
            (IntPtr hDC,                        // handle to DC
            IntPtr lpString,               // character string
            int nCount,                     // number of characters
            int nTabPositions,              // number of tab positions
            ref int lpnTabStopPositions // array of tab positions
            );

        /// <summary>
        /// The GetCharABCWidths function retrieves the widths, in logical units,
        /// of consecutive characters in a specified range from the current TrueType font.
        /// This function succeeds only with TrueType fonts.
        /// </summary>
        /// <param name="hdc"></param>
        /// <param name="uFirstChar"></param>
        /// <param name="uLastChar"></param>
        /// <param name="lpabc">pointer to first element of ABC[]</param>
        /// <returns>If the function succeeds, the return value is nonzero</returns>
        [DllImport("gdi32.dll", ExactSpelling = true, SetLastError = true)]
        public static extern int GetCharABCWidths
            (IntPtr hdc,
            uint uFirstChar,
            uint uLastChar,
            ref ABC lpabc);

        /// <summary>
        /// The GetCharABCWidthsFloat function retrieves the widths, in logical units,
        /// of consecutive characters in a specified range from the current font.
        /// </summary>
        /// <param name="hdc"></param>
        /// <param name="iFirstChar"></param>
        /// <param name="iLastChar"></param>
        /// <param name="lpABCF">pointer to first element of ABCFLOAT[]</param>
        /// <returns>If the function succeeds, the return value is nonzero.</returns>
        [DllImport("gdi32.dll", ExactSpelling = true, SetLastError = true)]
        public static extern int GetCharABCWidthsFloat
            (IntPtr hdc,
            uint iFirstChar,
            uint iLastChar,
            ref ABCFLOAT lpABCF);

        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        public static extern int DrawTextEx
            (IntPtr hdc,        // handle to DC
            [MarshalAs(UnmanagedType.LPTStr)]
            string lpchText,    // text to draw
            int cchText,                 // length of text to draw
            ref RECT lprc,                 // rectangle coordinates
            GDI_DrawTextFormat dwDTFormat,             // formatting options
            ref DRAWTEXTPARAMS lpDTParams  // more formatting options
            );

        /// <summary>
        /// The ExtTextOut function draws text using the currently selected font, background color, and text color.
        /// You can optionally provide dimensions to be used for clipping, opaquing, or both.
        /// By default, the current position is not used or updated by this function.
        /// However, an application can call the SetTextAlign function with the fMode parameter set
        /// to TA_UPDATECP to permit the system to use and update the current position each time
        /// the application calls ExtTextOut for a specified device context.
        /// When this flag is set, the system ignores the X and Y parameters on subsequent ExtTextOut calls. 
        /// </summary>
        /// <param name="hdc"></param>
        /// <param name="X">Specifies the x-coordinate, in logical coordinates,
        /// of the reference point used to position the string.</param>
        /// <param name="Y">Specifies the y-coordinate, in logical coordinates,
        /// of the reference point used to position the string.</param>
        /// <param name="fuOptions">The ETO_GLYPH_INDEX and ETO_RTLREADING values cannot be used together.
        /// Because ETO_GLYPH_INDEX implies that all language processing has been completed,
        /// the function ignores the ETO_RTLREADING flag if also specified. </param>
        /// <param name="lprc">Pointer to an optional RECT structure that specifies the dimensions,
        /// in logical coordinates, of a rectangle that is used for clipping, opaquing, or both.</param>
        /// <param name="lpString">Pointer to a string that specifies the text to be drawn.
        /// The string does not need to be zero-terminated, since cbCount specifies the length of the string. </param>
        /// <param name="cbCount">Specifies the length of the string pointed to by lpString</param>
        /// <param name="lpDx">Pointer to an optional array of values that indicate the distance
        /// between origins of adjacent character cells. For example, lpDx[i] logical units separate
        /// the origins of character cell i and character cell i + 1. </param>
        /// <returns>If the string is drawn, the return value is nonzero.</returns>
        [DllImport("gdi32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        public static extern int ExtTextOut
            (IntPtr hdc,
            int X,
            int Y,
            GDI_ExtTextOutOption fuOptions,
            ref RECT lprc,
            [MarshalAs(UnmanagedType.LPTStr)]
            string lpString,
            int cbCount,
            ref int lpDx);

        /// <summary>
        /// The ExtTextOut function draws text using the currently selected font, background color, and text color.
        /// You can optionally provide dimensions to be used for clipping, opaquing, or both.
        /// By default, the current position is not used or updated by this function.
        /// However, an application can call the SetTextAlign function with the fMode parameter set
        /// to TA_UPDATECP to permit the system to use and update the current position each time
        /// the application calls ExtTextOut for a specified device context.
        /// When this flag is set, the system ignores the X and Y parameters on subsequent ExtTextOut calls. 
        /// </summary>
        /// <param name="hdc"></param>
        /// <param name="X">Specifies the x-coordinate, in logical coordinates,
        /// of the reference point used to position the string.</param>
        /// <param name="Y">Specifies the y-coordinate, in logical coordinates,
        /// of the reference point used to position the string.</param>
        /// <param name="fuOptions">The ETO_GLYPH_INDEX and ETO_RTLREADING values cannot be used together.
        /// Because ETO_GLYPH_INDEX implies that all language processing has been completed,
        /// the function ignores the ETO_RTLREADING flag if also specified. </param>
        /// <param name="lprc">Pointer to an optional RECT structure that specifies the dimensions,
        /// in logical coordinates, of a rectangle that is used for clipping, opaquing, or both.</param>
        /// <param name="lpString">Pointer to a string that specifies the text to be drawn.
        /// The string does not need to be zero-terminated, since cbCount specifies the length of the string. </param>
        /// <param name="cbCount">Specifies the length of the string pointed to by lpString</param>
        /// <param name="lpDx">Pointer to an optional array of values that indicate the distance
        /// between origins of adjacent character cells. For example, lpDx[i] logical units separate
        /// the origins of character cell i and character cell i + 1. </param>
        /// <returns>If the string is drawn, the return value is nonzero.</returns>
        [DllImport("gdi32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        public static extern int ExtTextOut
            (IntPtr hdc,
            int X,
            int Y,
            GDI_ExtTextOutOption fuOptions,
            IntPtr lprc,
            IntPtr lpString,
            int cbCount,
            IntPtr lpDx);


        /// <summary>
        /// The ExtTextOut function draws text using the currently selected font, background color, and text color.
        /// You can optionally provide dimensions to be used for clipping, opaquing, or both.
        /// By default, the current position is not used or updated by this function.
        /// However, an application can call the SetTextAlign function with the fMode parameter set
        /// to TA_UPDATECP to permit the system to use and update the current position each time
        /// the application calls ExtTextOut for a specified device context.
        /// When this flag is set, the system ignores the X and Y parameters on subsequent ExtTextOut calls. 
        /// </summary>
        /// <param name="hdc"></param>
        /// <param name="X">Specifies the x-coordinate, in logical coordinates,
        /// of the reference point used to position the string.</param>
        /// <param name="Y">Specifies the y-coordinate, in logical coordinates,
        /// of the reference point used to position the string.</param>
        /// <param name="fuOptions">The ETO_GLYPH_INDEX and ETO_RTLREADING values cannot be used together.
        /// Because ETO_GLYPH_INDEX implies that all language processing has been completed,
        /// the function ignores the ETO_RTLREADING flag if also specified. </param>
        /// <param name="lprc">Pointer to an optional RECT structure that specifies the dimensions,
        /// in logical coordinates, of a rectangle that is used for clipping, opaquing, or both.</param>
        /// <param name="lpString">Pointer to a string that specifies the text to be drawn.
        /// The string does not need to be zero-terminated, since cbCount specifies the length of the string. </param>
        /// <param name="cbCount">Specifies the length of the string pointed to by lpString</param>
        /// <param name="lpDx">Pointer to an optional array of values that indicate the distance
        /// between origins of adjacent character cells. For example, lpDx[i] logical units separate
        /// the origins of character cell i and character cell i + 1. </param>
        /// <returns>If the string is drawn, the return value is nonzero.</returns>
        [DllImport("gdi32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        public static extern int ExtTextOut
            (IntPtr hdc,
            int X,
            int Y,
            GDI_ExtTextOutOption fuOptions,
            IntPtr lprc,
            [MarshalAs(UnmanagedType.LPTStr)]
            string lpString,
            int cbCount,
            IntPtr lpDx);

        /// <summary>
        /// The ExtTextOut function draws text using the currently selected font, background color, and text color.
        /// You can optionally provide dimensions to be used for clipping, opaquing, or both.
        /// By default, the current position is not used or updated by this function.
        /// However, an application can call the SetTextAlign function with the fMode parameter set
        /// to TA_UPDATECP to permit the system to use and update the current position each time
        /// the application calls ExtTextOut for a specified device context.
        /// When this flag is set, the system ignores the X and Y parameters on subsequent ExtTextOut calls. 
        /// </summary>
        /// <param name="hdc"></param>
        /// <param name="X">Specifies the x-coordinate, in logical coordinates,
        /// of the reference point used to position the string.</param>
        /// <param name="Y">Specifies the y-coordinate, in logical coordinates,
        /// of the reference point used to position the string.</param>
        /// <param name="fuOptions">The ETO_GLYPH_INDEX and ETO_RTLREADING values cannot be used together.
        /// Because ETO_GLYPH_INDEX implies that all language processing has been completed,
        /// the function ignores the ETO_RTLREADING flag if also specified. </param>
        /// <param name="lprc">Pointer to an optional RECT structure that specifies the dimensions,
        /// in logical coordinates, of a rectangle that is used for clipping, opaquing, or both.</param>
        /// <param name="lpString">Pointer to a string that specifies the text to be drawn.
        /// The string does not need to be zero-terminated, since cbCount specifies the length of the string. </param>
        /// <param name="cbCount">Specifies the length of the string pointed to by lpString</param>
        /// <param name="lpDx">Pointer to an optional array of values that indicate the distance
        /// between origins of adjacent character cells. For example, lpDx[i] logical units separate
        /// the origins of character cell i and character cell i + 1. </param>
        /// <returns>If the string is drawn, the return value is nonzero.</returns>
        [DllImport("gdi32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        public static extern int ExtTextOut
            (IntPtr hdc,
            int X,
            int Y,
            GDI_ExtTextOutOption fuOptions,
            ref RECT lprc,
            IntPtr lpString,
            int cbCount,
            IntPtr lpDx);

        /// <summary>
        /// The GetCharacterPlacement function retrieves information about a character string,
        /// such as character widths, caret positioning, ordering within the string, and glyph
        /// rendering. The type of information returned depends on the dwFlags parameter and 
        /// is based on the currently selected font in the specified display context.
        /// The function copies the information to the specified GCP_RESULTS structure
        /// or to one or more arrays specified by the structure.
        /// </summary>
        /// <param name="hdc"></param>
        /// <param name="lpString">Pointer to the character string to process. The string does 
        /// not need to be zero-terminated, since nCount specifies the length of the string. </param>
        /// <param name="nCount">Specifies the length of the string pointed to by lpString.</param>
        /// <param name="nMaxExtent">Specifies the maximum extent (in logical units) to which the string
        /// is processed. Characters that, if processed, would exceed this extent are ignored.
        /// Computations for any required ordering or glyph arrays apply only to the included characters.
        /// This parameter is used only if the GCP_MAXEXTENT value is specified in the dwFlags parameter.
        /// As the function processes the input string, each character and its extent is added to the output,
        /// extent, and other arrays only if the total extent has not yet exceeded the maximum.
        /// Once the limit is reached, processing will stop.</param>
        /// <param name="lpResults"></param>
        /// <param name="dwFlags"></param>
        /// <returns>If the function succeeds, the return value is the same as the return value
        /// is the width and height of the string in logical units. The width is the low-order
        /// word and the height is the high-order word. If the function fails, the return value
        /// is zero.</returns>
        [DllImport("gdi32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        public static extern uint GetCharacterPlacement
            (IntPtr hdc,
            [MarshalAs(UnmanagedType.LPTStr)]
            string lpString,
            int nCount,
            int nMaxExtent,
            ref GCP_RESULTS lpResults,
            GDI_GetCharacterPlacementOption dwFlags);

        /// <summary>
        /// The GetCharacterPlacement function retrieves information about a character string,
        /// such as character widths, caret positioning, ordering within the string, and glyph
        /// rendering. The type of information returned depends on the dwFlags parameter and 
        /// is based on the currently selected font in the specified display context.
        /// The function copies the information to the specified GCP_RESULTS structure
        /// or to one or more arrays specified by the structure.
        /// </summary>
        /// <param name="hdc"></param>
        /// <param name="lpString">Pointer to the character string to process. The string does 
        /// not need to be zero-terminated, since nCount specifies the length of the string. </param>
        /// <param name="nCount">Specifies the length of the string pointed to by lpString.</param>
        /// <param name="nMaxExtent">Specifies the maximum extent (in logical units) to which the string
        /// is processed. Characters that, if processed, would exceed this extent are ignored.
        /// Computations for any required ordering or glyph arrays apply only to the included characters.
        /// This parameter is used only if the GCP_MAXEXTENT value is specified in the dwFlags parameter.
        /// As the function processes the input string, each character and its extent is added to the output,
        /// extent, and other arrays only if the total extent has not yet exceeded the maximum.
        /// Once the limit is reached, processing will stop.</param>
        /// <param name="lpResults"></param>
        /// <param name="dwFlags"></param>
        /// <returns>If the function succeeds, the return value is the same as the return value
        /// is the width and height of the string in logical units. The width is the low-order
        /// word and the height is the high-order word. If the function fails, the return value
        /// is zero.</returns>
        [DllImport("gdi32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        public static extern uint GetCharacterPlacement
            (IntPtr hdc,
            IntPtr lpString,
            int nCount,
            int nMaxExtent,
            ref GCP_RESULTS lpResults,
            GDI_GetCharacterPlacementOption dwFlags);

        /// <summary>
        /// The return value, when masked with FLI_MASK,
        /// can be passed directly to the GetCharacterPlacement function. 
        /// </summary>
        /// <param name="hdc"></param>
        /// <returns></returns>
        [DllImport("gdi32.dll", ExactSpelling = true, SetLastError = true)]
        public static extern uint GetFontLanguageInfo(IntPtr hdc);

        [DllImport("gdi32.dll", ExactSpelling = true, SetLastError = true)]
        public static extern MappingModes GetMapMode(IntPtr hdc);

        public const int HGDI_ERROR = -1;
        public const uint CLR_INVALID = 0xFFFFFFFF;
        public const uint GDI_ERROR = 0xFFFFFFFF;

        /* Region Flags */
        public const int ERROR = 0;
        public const int NULLREGION = 1;
        public const int SIMPLEREGION = 2;
        public const int COMPLEXREGION = 3;

        /* Background Modes */
        public const int TRANSPARENT = 1;
        public const int OPAQUE = 2;

        /* Text Alignment Options */
        public const int TA_NOUPDATECP = 0;
        public const int TA_UPDATECP = 1;

        public const int TA_LEFT = 0;
        public const int TA_RIGHT = 2;
        public const int TA_CENTER = 6;

        public const int TA_TOP = 0;
        public const int TA_BOTTOM = 8;
        public const int TA_BASELINE = 24;

        public const int TA_RTLREADING = 256;
        /*******************************/

        /*
        * DrawText() Format Flags
        */
        public const int DT_TOP = 0x00000000;
        public const int DT_LEFT = 0x00000000;
        public const int DT_CENTER = 0x00000001;
        public const int DT_RIGHT = 0x00000002;
        public const int DT_VCENTER = 0x00000004;
        public const int DT_BOTTOM = 0x00000008;
        public const int DT_WORDBREAK = 0x00000010;
        public const int DT_SINGLELINE = 0x00000020;
        public const int DT_EXPANDTABS = 0x00000040;
        public const int DT_TABSTOP = 0x00000080;
        public const int DT_NOCLIP = 0x00000100;
        public const int DT_EXTERNALLEADING = 0x00000200;
        public const int DT_CALCRECT = 0x00000400;
        public const int DT_NOPREFIX = 0x00000800;
        public const int DT_INTERNAL = 0x00001000;
        public const int DT_EDITCONTROL = 0x00002000;
        public const int DT_PATH_ELLIPSIS = 0x00004000;
        public const int DT_END_ELLIPSIS = 0x00008000;
        public const int DT_MODIFYSTRING = 0x00010000;
        public const int DT_RTLREADING = 0x00020000;
        public const int DT_WORD_ELLIPSIS = 0x00040000;
        public const int DT_NOFULLWIDTHCHARBREAK = 0x00080000;
        public const int DT_HIDEPREFIX = 0x00100000;
        public const int DT_PREFIXONLY = 0x00200000;

        /*
         * ExtTextOut options
         */
        public const int ETO_OPAQUE = 0x0002;
        public const int ETO_CLIPPED = 0x0004;
        public const int ETO_GLYPH_INDEX = 0x0010;
        public const int ETO_RTLREADING = 0x0080;
        public const int ETO_NUMERICSLOCAL = 0x0400;
        public const int ETO_NUMERICSLATIN = 0x0800;
        public const int ETO_IGNORELANGUAGE = 0x1000;
        public const int ETO_PDY = 0x2000;
        public const int ETO_REVERSE_INDEX_MAP = 0x10000;

        public const uint GCP_DBCS = 0x0001;
        public const uint GCP_REORDER = 0x0002;
        public const uint GCP_USEKERNING = 0x0008;
        public const uint GCP_GLYPHSHAPE = 0x0010;
        public const uint GCP_LIGATE = 0x0020;
        public const uint GCP_DIACRITIC = 0x0100;
        public const uint GCP_KASHIDA = 0x0400;
        public const uint GCP_ERROR = 0x8000;
        public const uint FLI_MASK = 0x103B;
        public const uint GCP_JUSTIFY = 0x00010000;
        public const uint FLI_GLYPHS = 0x00040000;
        public const uint GCP_CLASSIN = 0x00080000;
        public const uint GCP_MAXEXTENT = 0x00100000;
        public const uint GCP_JUSTIFYIN = 0x00200000;
        public const uint GCP_DISPLAYZWG = 0x00400000;
        public const uint GCP_SYMSWAPOFF = 0x00800000;
        public const uint GCP_NUMERICOVERRIDE = 0x01000000;
        public const uint GCP_NEUTRALOVERRIDE = 0x02000000;
        public const uint GCP_NUMERICSLATIN = 0x04000000;
        public const uint GCP_NUMERICSLOCAL = 0x08000000;

        public const int GCPCLASS_LATIN = 1;
        public const int GCPCLASS_HEBREW = 2;
        public const int GCPCLASS_ARABIC = 2;
        public const int GCPCLASS_NEUTRAL = 3;
        public const int GCPCLASS_LOCALNUMBER = 4;
        public const int GCPCLASS_LATINNUMBER = 5;
        public const int GCPCLASS_LATINNUMERICTERMINATOR = 6;
        public const int GCPCLASS_LATINNUMERICSEPARATOR = 7;
        public const int GCPCLASS_NUMERICSEPARATOR = 8;
        public const int GCPCLASS_PREBOUNDLTR = 0x80;
        public const int GCPCLASS_PREBOUNDRTL = 0x40;
        public const int GCPCLASS_POSTBOUNDLTR = 0x20;
        public const int GCPCLASS_POSTBOUNDRTL = 0x10;
        public const int GCPGLYPH_LINKBEFORE = 0x8000;
        public const int GCPGLYPH_LINKAFTER = 0x4000;

        
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct COLORREF
    {
        public uint color_ref;

        public COLORREF(Color color)
        {
            color_ref =
                (uint)color.R +
                (((uint)color.G) << 8) +
                (((uint)color.B) << 16);
        }

        public Color ToColor()
        {
            return Color.FromArgb
                ((int)(0x000000FFU & color_ref),
                (int)(0x0000FF00U & color_ref) >> 8,
                (int)(0x00FF0000U & color_ref) >> 16);
        }

        public void SetColor(Color color)
        {
            color_ref = (uint)color.R + (((uint)color.G) << 8) + (((uint)color.B) << 16);
        }
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct ABC
    {
        public int A;
        public int B;
        public int C;

        public int TotalWidth()
        {
            return A + B + C;
        }
    }

    public struct ABCFLOAT
    {
        public float A;
        public float B;
        public float C;

        public float TotalWidth()
        {
            return A + B + C;
        }
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct SIZE
    {
        public int cx;
        public int cy;

        public SIZE(int cx, int cy)
        {
            this.cx = cx;
            this.cy = cy;
        }
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct DRAWTEXTPARAMS
    {
        public uint cbSize;
        public int iTabLength;
        public int iLeftMargin;
        public int iRightMargin;
        public uint uiLengthDrawn;

        public void Init()
        {
            cbSize = (uint)Marshal.SizeOf(this);
        }
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct RECT
    {
        public int Left;
        public int Top;
        public int Right;
        public int Bottom;

        public RECT(int left_, int top_, int right_, int bottom_)
        {
            Left = left_;
            Top = top_;
            Right = right_;
            Bottom = bottom_;
        }

        public int Height { get { return Bottom - Top; } }
        public int Width { get { return Right - Left; } }
        public Size Size { get { return new Size(Width, Height); } }

        public Point Location { get { return new Point(Left, Top); } }

        // Handy method for converting to a System.Drawing.Rectangle
        public Rectangle ToRectangle()
        { return Rectangle.FromLTRB(Left, Top, Right, Bottom); }

        public static RECT FromRectangle(Rectangle rectangle)
        {
            return new RECT(rectangle.Left, rectangle.Top, rectangle.Right, rectangle.Bottom);
        }

        public override int GetHashCode()
        {
            return Left ^ ((Top << 13) | (Top >> 0x13))
              ^ ((Width << 0x1a) | (Width >> 6))
              ^ ((Height << 7) | (Height >> 0x19));
        }

        #region Operator overloads

        public static implicit operator Rectangle(RECT rect)
        {
            return rect.ToRectangle();
        }

        public static implicit operator RECT(Rectangle rect)
        {
            return FromRectangle(rect);
        }

        #endregion
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct GCP_RESULTS
    {
        public int lStructSize;
        /// <summary>
        /// Pointer to the buffer that receives the output string or is NULL if the output string
        /// is not needed. The output string is a version of the original string that is in the order
        /// that will be displayed on a specified device. Typically the output string is identical
        /// to the original string, but may be different if the string needs reordering and
        /// the GCP_REORDER flag is set or if the original string exceeds the maximum extent
        /// and the GCP_MAXEXTENT flag is set.
        /// </summary>
        public IntPtr lpOutString;
        /// <summary>
        /// Pointer to the array that receives ordering indexes or is NULL if the ordering indexes
        /// are not needed. However, its meaning depends on the other elements of GCP_RESULTS.
        /// If glyph indexes are to be returned, the indexes are for the lpGlyphs array;
        /// if glyphs indexes are not returned and lpOrder is requested, the indexes are for lpOutString.
        /// For example, in the latter case the value of lpOrder[i] is the position of lpString[i]
        /// in the output string lpOutString. This is typically used when GetFontLanguageInfo returns
        /// the GCP_REORDER flag, which indicates that the original string needs reordering.
        /// For example, in Hebrew, in which the text runs from right to left, the lpOrder array
        /// gives the exact locations of each element in the original string. 
        /// </summary>
        public IntPtr lpOrder;
        /// <summary>
        /// Pointer to the array that receives the distances between adjacent character cells
        /// or is NULL if these distances are not needed. If glyph rendering is done, the distances
        /// are for the glyphs not the characters, so the resulting array can be used with the
        /// ExtTextOut function. The distances in this array are in display order.
        /// To find the distance for the ith character in the original string,
        /// use the lpOrder array as follows: width = lpDx[lpOrder[i]];
        /// </summary>
        public IntPtr lpDx;
        /// <summary>
        /// Pointer to the array that receives the caret position values or is NULL if caret
        /// positions are not needed. Each value specifies the caret position immediately before
        /// the corresponding character. In some languages the position of the caret for each
        /// character may not be immediately to the left of the character. For example, in Hebrew,
        /// in which the text runs from right to left, the caret position is to the right of the character.
        /// If glyph ordering is done, lpCaretPos matches the original string not the output string.
        /// This means that some adjacent values may be the same. The values in this array are in input order.
        /// To find the caret position value for the ith character in the original string,
        /// use the array as follows: position = lpCaretPos[i];
        /// </summary>
        public IntPtr lpCaretPos;
        /// <summary>
        /// Pointer to the array that contains and/or receives character classifications.
        /// The values indicate how to lay out characters in the string and are similar
        /// (but not identical) to the CT_CTYPE2 values returned by the GetStringTypeEx function. 
        /// </summary>
        public IntPtr lpClass;
        /// <summary>
        /// Pointer to the array that receives the values identifying the glyphs used for rendering
        /// the string or is NULL if glyph rendering is not needed. The number of glyphs in the array
        /// may be less than the number of characters in the original string if the string contains
        /// ligated glyphs. Also if reordering is required, the order of the glyphs may not be sequential.
        /// This array is useful if more than one operation is being done on a string which has any form
        /// of ligation, kerning or order-switching. Using the values in this array for subsequent
        /// operations saves the time otherwise required to generate the glyph indices each time.
        /// This array always contains glyph indices and the ETO_GLYPH_INDEX value must always be
        /// used when this array is used with the ExtTextOut function. When GCP_LIGATE is used,
        /// you can limit the number of characters that will be ligated together. (In Arabic for example,
        /// three-character ligations are common). This is done by setting the maximum required
        /// in lpGcpResults->lpGlyphs[0]. If no maximum is required, you should set this field
        /// to zero. For languages such as Arabic, where GetFontLanguageInfo returns the GCP_GLYPHSHAPE flag,
        /// the glyphs for a character will be different depending on whether the character
        /// is at the beginning, middle, or end of a word. Typically, the first character in the input
        /// string will also be the first character in a word, and the last character in the input
        /// string will be treated as the last character in a word. However, if the displayed string
        /// is a subset of the complete string, such as when displaying a section of scrolled text,
        /// this may not be true. In these cases, it is desirable to force the first or last characters
        /// to be shaped as not being initial or final forms. To do this, again, the first location
        /// in the lpGlyphs array is used by performing an OR operation of the ligation value above
        /// with the values GCPGLYPH_LINKBEFORE and/or GCPGLYPH_LINKAFTER. For example,
        /// a value of GCPGLYPH_LINKBEFORE | 2 means that two-character ligatures are the maximum
        /// required, and the first character in the string should be treated as if it is in the middle of a word. 
        /// </summary>
        public IntPtr lpGlyphs;
        /// <summary>
        /// On input, this member must be set to the size of the arrays pointed to by the array
        /// pointer members. On output, this is set to the number of glyphs filled in,
        /// in the output arrays. If glyph substitution is not required 
        /// (that is, each input character maps to exactly one glyph), this member is the same as it is on input.
        /// </summary>
        public int nGlyphs;
        /// <summary>
        /// Number of characters that fit within the extents specified by the nMaxExtent parameter
        /// of the GetCharacterPlacement function. If the GCP_MAXEXTENT or GCP_JUSTIFY value is set,
        /// this value may be less than the number of characters in the original string.
        /// This member is set regardless of whether the GCP_MAXEXTENT or GCP_JUSTIFY value is specified.
        /// Unlike nGlyphs, which specifies the number of output glyphs, nMaxFit refers to the number
        /// of characters from the input string. For Latin SBCS languages, this will be the same. 
        /// </summary>
        public int nMaxFit;
    }

    public enum GDI_backgroundMode
    {
        TRANSPARENT = NativeGdi.TRANSPARENT,
        OPAQUE = NativeGdi.OPAQUE,
        ERROR = 0
    }

    public enum GDI_TextAlignment
    {
        LEFT = NativeGdi.TA_LEFT,
        RIGHT = NativeGdi.TA_RIGHT,
        CENTER = NativeGdi.TA_CENTER
    }

    public enum GDI_LineAlignment
    {
        TOP = NativeGdi.TA_TOP,
        BOTTOM = NativeGdi.TA_BOTTOM,
        BASELINE = NativeGdi.TA_BASELINE
    }

    public enum GDI_UpdateCurrentPosition
    {
        NOUPDATE = NativeGdi.TA_NOUPDATECP,
        UPDATE = NativeGdi.TA_UPDATECP
    }

    [Flags()]
    public enum GDI_DrawTextFormat
    {
        /// <summary>
        /// ustifies the text to the top of the rectangle.
        /// </summary>
        TOP = NativeGdi.DT_TOP,
        /// <summary>
        /// Aligns text to the left.
        /// </summary>
        LEFT = NativeGdi.DT_LEFT,
        /// <summary>
        /// Centers text horizontally in the rectangle.
        /// </summary>
        CENTER = NativeGdi.DT_CENTER,
        /// <summary>
        /// Aligns text to the right.
        /// </summary>
        RIGHT = NativeGdi.DT_RIGHT,
        /// <summary>
        /// Centers text vertically. This value is used only with the DT_SINGLELINE value.
        /// </summary>
        VCENTER = NativeGdi.DT_VCENTER,
        /// <summary>
        /// Justifies the text to the bottom of the rectangle.
        /// This value is used only with the DT_SINGLELINE value.
        /// </summary>
        BOTTOM = NativeGdi.DT_BOTTOM,
        /// <summary>
        /// Breaks words. Lines are automatically broken between words if a word extends past
        /// the edge of the rectangle specified by the lprc parameter. A carriage return-line
        /// feed sequence also breaks the line.
        /// </summary>
        WORDBREAK = NativeGdi.DT_WORDBREAK,
        /// <summary>
        /// Displays text on a single line only. Carriage returns and line feeds do not break the line.
        /// </summary>
        SINGLELINE = NativeGdi.DT_SINGLELINE,
        /// <summary>
        /// Expands tab characters. The default number of characters per tab is eight.
        /// </summary>
        EXPANDTABS = NativeGdi.DT_EXPANDTABS,
        /// <summary>
        /// Sets tab stops. The DRAWTEXTPARAMS structure pointed to by the lpDTParams parameter
        /// specifies the number of average character widths per tab stop.
        /// </summary>
        TABSTOP = NativeGdi.DT_TABSTOP,
        /// <summary>
        /// Draws without clipping. DrawTextEx is somewhat faster when DT_NOCLIP is used
        /// </summary>
        NOCLIP = NativeGdi.DT_NOCLIP,
        /// <summary>
        /// Includes the font external leading in line height.
        /// Normally, external leading is not included in the height of a line of text.
        /// </summary>
        EXTERNALLEADING = NativeGdi.DT_EXTERNALLEADING,
        /// <summary>
        /// Determines the width and height of the rectangle. If there are multiple lines of text,
        /// DrawTextEx uses the width of the rectangle pointed to by the lprc parameter
        /// and extends the base of the rectangle to bound the last line of text.
        /// If there is only one line of text, DrawTextEx modifies the right side
        /// of the rectangle so that it bounds the last character in the line.
        /// In either case, DrawTextEx returns the height of the formatted text, but does not draw the text.
        /// </summary>
        CALCRECT = NativeGdi.DT_CALCRECT,
        /// <summary>
        /// Turns off processing of prefix characters. Normally, DrawTextEx interprets
        /// the ampersand (&) mnemonic-prefix character as a directive to underscore
        /// the character that follows, and the double-ampersand (&&) mnemonic-prefix
        /// characters as a directive to print a single ampersand. By specifying DT_NOPREFIX,
        /// this processing is turned off. Compare with DT_HIDEPREFIX and DT_PREFIXONLY
        /// </summary>
        NOPREFIX = NativeGdi.DT_NOPREFIX,
        /// <summary>
        /// Uses the system font to calculate text metrics.
        /// </summary>
        INTERNAL = NativeGdi.DT_INTERNAL,
        /// <summary>
        /// Duplicates the text-displaying characteristics of a multiline edit control.
        /// Specifically, the average character width is calculated in the same manner
        /// as for an edit control, and the function does not display a partially visible last line.
        /// </summary>
        EDITCONTROL = NativeGdi.DT_EDITCONTROL,
        PATH_ELLIPSIS = NativeGdi.DT_PATH_ELLIPSIS,
        END_ELLIPSIS = NativeGdi.DT_END_ELLIPSIS,
        /// <summary>
        /// Modifies the specified string to match the displayed text.
        /// This value has no effect unless DT_END_ELLIPSIS or DT_PATH_ELLIPSIS is specified.
        /// </summary>
        MODIFYSTRING = NativeGdi.DT_MODIFYSTRING,
        /// <summary>
        /// Layout in right-to-left reading order for bi-directional text when the font selected 
        /// into the hdc is a Hebrew or Arabic font. 
        /// The default reading order for all text is left-to-right.
        /// </summary>
        RTLREADING = NativeGdi.DT_RTLREADING,
        WORD_ELLIPSIS = NativeGdi.DT_WORD_ELLIPSIS,
        /// <summary>
        /// Prevents a line break at a DBCS (double-wide character string),
        /// so that the line-breaking rule is equivalent to SBCS strings.
        /// For example, this can be used in Korean windows, for more readability of icon labels.
        /// This value has no effect unless DT_WORDBREAK is specified.
        /// </summary>
        NOFULLWIDTHCHARBREAK = NativeGdi.DT_NOFULLWIDTHCHARBREAK,
        /// <summary>
        /// Ignores the ampersand (&) prefix character in the text. The letter that follows will not be underlined,
        /// but other mnemonic-prefix characters are still processed. 
        /// </summary>
        HIDEPREFIX = NativeGdi.DT_HIDEPREFIX,
        /// <summary>
        /// Draws only an underline at the position of the character following the ampersand (&) prefix character.
        /// Does not draw any character in the string. 
        /// </summary>
        PREFIXONLY = NativeGdi.DT_PREFIXONLY
    }

    [Flags()]
    public enum GDI_ExtTextOutOption
    {
        None=0,
        /// <summary>
        /// The current background color should be used to fill the rectangle.
        /// </summary>
        OPAQUE = NativeGdi.ETO_OPAQUE,
        /// <summary>
        /// The text will be clipped to the rectangle.
        /// </summary>
        CLIPPED = NativeGdi.ETO_CLIPPED,
        /// <summary>
        /// The lpString array refers to an array returned from GetCharacterPlacement
        /// and should be parsed directly by GDI as no further language-specific processing
        /// is required. Glyph indexing only applies to TrueType fonts, but the flag can be
        /// used for bitmap and vector fonts to indicate that no further language processing
        /// is necessary and GDI should process the string directly. Note that all glyph indexes
        /// are 16-bit values even though the string is assumed to be an array of 8-bit values
        /// for raster fonts.For ExtTextOutW, the glyph indexes are saved to a metafile.
        /// However, to display the correct characters the metafile must be played back using
        /// the same font. For ExtTextOutA, the glyph indexes are not saved.
        /// </summary>
        GLYPH_INDEX = NativeGdi.ETO_GLYPH_INDEX,
        /// <summary>
        /// If this value is specified and a Hebrew or Arabic font is selected into the device context,
        /// the string is output using right-to-left reading order. If this value is not specified,
        /// the string is output in left-to-right order. The same effect can be achieved by setting
        /// the TA_RTLREADING value in SetTextAlign. This value is preserved for backward compatibility.
        /// </summary>
        RTLREADING = NativeGdi.ETO_RTLREADING,
        NUMERICSLOCAL = NativeGdi.ETO_NUMERICSLOCAL,
        NUMERICSLATIN = NativeGdi.ETO_NUMERICSLATIN,
        /// <summary>
        /// Reserved for system use. If an application sets this flag, it loses international
        /// scripting support and in some cases it may display no text at all.
        /// </summary>
        IGNORELANGUAGE = NativeGdi.ETO_IGNORELANGUAGE,
        /// <summary>
        /// When this is set, the array pointed to by lpDx contains pairs of values.
        /// The first value of each pair is, as usual, the distance between origins of
        /// adjacent character cells, but the second value is the displacement along the vertical
        /// direction of the font.
        /// </summary>
        PDY = NativeGdi.ETO_PDY,
        REVERSE_INDEX_MAP = NativeGdi.ETO_REVERSE_INDEX_MAP
    }

    [Flags()]
    public enum GDI_GetCharacterPlacementOption : uint
    {
        /// <summary>
        /// Specifies that the lpClass array contains preset classifications for characters.
        /// The classifications may be the same as on output. If the particular classification
        /// for a character is not known, the corresponding location in the array must be set to zero.
        /// for more information about the classifications, see GCP_RESULTS.
        /// This is useful only if GetFontLanguageInfo returned the GCP_REORDER flag.
        /// </summary>
        CLASSIN = NativeGdi.GCP_CLASSIN,
        /// <summary>
        /// Determines how diacritics in the string are handled. If this value is not set,
        /// diacritics are treated as zero-width characters. For example, a Hebrew string may
        /// contain diacritics, but you may not want to display them.Use GetFontLanguageInfo
        /// to determine whether a font supports diacritics. If it does, you can use or not use
        /// the GCP_DIACRITIC flag in the call to GetCharacterPlacement, depending on the needs
        /// of your application.
        /// </summary>
        DIACRITIC = NativeGdi.GCP_DIACRITIC,
        /// <summary>
        /// For languages that need reordering or different glyph shapes depending on the positions
        /// of the characters within a word, nondisplayable characters often appear in the code page.
        /// For example, in the Hebrew code page, there are Left-To-Right and Right-To-Left markers,
        /// to help determine the final positioning of characters within the output strings.
        /// Normally these are not displayed and are removed from the lpGlyphs and lpDx arrays.
        /// You can use the GCP_DISPLAYZWG flag to display these characters.
        /// </summary>
        DISPLAYZWG = NativeGdi.GCP_DISPLAYZWG,
        /// <summary>
        /// Specifies that some or all characters in the string are to be displayed using shapes other
        /// than the standard shapes defined in the currently selected font for the current code page.
        /// Some languages, such as Arabic, cannot support glyph creation unless this value is specified.
        /// As a general rule, if GetFontLanguageInfo returns this value for a string, this value must be
        /// used with GetCharacterPlacement.
        /// </summary>
        GLYPHSHAPE = NativeGdi.GCP_GLYPHSHAPE,
        /// <summary>
        /// Adjusts the extents in the lpDx array so that the string length is the same as nMaxExtent.
        /// GCP_JUSTIFY may only be used in conjunction with GCP_MAXEXTENT.
        /// </summary>
        JUSTIFY = NativeGdi.GCP_JUSTIFYIN,
        /// <summary>
        /// Use Kashidas as well as, or instead of, adjusted extents to modify the length of the
        /// string so that it is equal to the value specified by nMaxExtent. In the lpDx array,
        /// a Kashida is indicated by a negative justification index. GCP_KASHIDA may be used only
        /// in conjunction with GCP_JUSTIFY and only if the font (and language) support Kashidas.
        /// Use GetFontLanguageInfo to determine whether the current font supports Kashidas.Using
        /// Kashidas to justify the string can result in the number of glyphs required being greater
        /// than the number of characters in the input string. Because of this, when Kashidas are used,
        /// the application cannot assume that setting the arrays to be the size of the input string
        /// will be sufficient. (The maximum possible will be approximately dxPageWidth/dxAveCharWidth,
        /// where dxPageWidth is the width of the document and dxAveCharWidth is the average character
        /// width as returned from a GetTextMetrics call).Note that just because GetFontLanguageInfo returns
        /// the GCP_KASHIDA flag does not mean that it has to be used in the call to GetCharacterPlacement,
        /// just that the option is available.
        /// </summary>
        KASHIDA = NativeGdi.GCP_KASHIDA,
        /// <summary>
        /// Use ligations wherever characters ligate. A ligation occurs where one glyph is used 
        /// for two or more characters. For example, the letters a and e can ligate to.
        /// For this to be used, however , both the language support and the font must support
        /// the required glyphs (the example will not be processed by default in English).
        /// Use GetFontLanguageInfo to determine whether the current font supports ligation.
        /// If it does and a specific maximum is required for the number of characters that will
        /// ligate, set the number in the first element of the lpGlyphs array. If normal ligation
        /// is required, set this value to zero. If GCP_LIGATE is not specified, no ligation 
        /// will take place. See GCP_RESULTS for more information.If the GCP_REORDER value
        /// is usually required for the character set but is not specified, the output will be
        /// meaningless unless the string being passed in is already in visual ordering 
        /// (that is, the result that gets put into lpGcpResults->lpOutString in one call
        /// to GetCharacterPlacement is the input string of a second call).Note that just because
        /// GetFontLanguageInfo returns the GCP_LIGATE flag does not mean that it has to be
        /// used in the call to GetCharacterPlacement, just that the option is available.
        /// </summary>
        LIGATE = NativeGdi.GCP_LIGATE,
        /// <summary>
        /// Compute extents of the string only as long as the resulting extent,
        /// in logical units, does not exceed the values specified by the nMaxExtent parameter.
        /// </summary>
        MAXEXTENT = NativeGdi.GCP_MAXEXTENT,
        /// <summary>
        /// Certain languages only. Override the normal handling of neutrals and treat them
        /// as strong characters that match the strings reading order. Useful only with the GCP_REORDER flag.
        /// </summary>
        NEUTRALOVERRIDE = NativeGdi.GCP_NEUTRALOVERRIDE,
        /// <summary>
        /// Certain languages only. Override the normal handling of numerics and treat them as strong characters
        /// that match the strings reading order. Useful only with the GCP_REORDER flag.
        /// </summary>
        NUMERICOVERRIDE = NativeGdi.GCP_NUMERICOVERRIDE,
        /// <summary>
        /// Arabic/Thai only. Use standard Latin glyphs for numbers and override the system default.
        /// To determine if this option is available in the language of the font, use GetStringTypeEx
        /// to see if the language supports more than one number format.
        /// </summary>
        NUMERICSLATIN = NativeGdi.GCP_NUMERICSLATIN,
        /// <summary>
        /// Arabic/Thai only. Use local glyphs for numeric characters and override the system default.
        /// To determine if this option is available in the language of the font, use GetStringTypeEx
        /// to see if the language supports more than one number format.
        /// </summary>
        NUMERICSLOCAL = NativeGdi.GCP_NUMERICSLOCAL,
        /// <summary>
        /// Reorder the string. Use for languages that are not SBCS and left-to-right reading order.
        /// If this value is not specified, the string is assumed to be in display order already.
        /// If this flag is set for Semitic languages and the lpClass array is used, the first two
        /// elements of the array are used to specify the reading order beyond the bounds of the string.
        /// GCP_CLASS_PREBOUNDRTL and GCP_CLASS_PREBOUNDLTR can be used to set the order.
        /// If no preset order is required, set the values to zero. These values can be combined
        /// with other values if the GCPCLASSIN flag is set. If the GCP_REORDER value is not specified,
        /// the lpString parameter is taken to be visual ordered for languages where this is used,
        /// and the lpOutString and lpOrder fields are ignored. Use GetFontLanguageInfo to determine
        /// whether the current font supports reordering.
        /// </summary>
        REORDER = NativeGdi.GCP_REORDER,
        /// <summary>
        /// Semitic languages only. Specifies that swappable characters are not reset. For example,
        /// in a right-to-left string, the '(' and ')' are not reversed.
        /// </summary>
        SYMSWAPOFF = NativeGdi.GCP_SYMSWAPOFF,
        /// <summary>
        /// se kerning pairs in the font (if any) when creating the widths arrays.
        /// Use GetFontLanguageInfo to determine whether the current font supports kerning pairs.
        /// Note that just because GetFontLanguageInfo returns the GCP_USEKERNING flag does not mean
        /// that it has to be used in the call to GetCharacterPlacement, just that the option is available.
        /// Most TrueType fonts have a kerning table, but you do not have to use it.
        /// </summary>
        USEKERNING = NativeGdi.GCP_USEKERNING
    }

    public enum MappingModes
    {
        MM_TEXT = 1,
        MM_LOMETRIC = 2,
        MM_HIMETRIC = 3,
        MM_LOENGLISH = 4,
        MM_HIENGLISH = 5,
        MM_TWIPS = 6,
        MM_ISOTROPIC = 7,
        MM_ANISOTROPIC = 8
    }
}
