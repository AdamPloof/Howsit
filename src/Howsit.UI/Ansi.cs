using System;
using System.Collections.Generic;
using Howsit.UI.Style;

namespace Howsit.UI;

public static class Ansi {
    public const string ESC = "\x1B";

    public const string Reset = ESC + "[0m";

    public const string EnterAlternateScreen = "\u001b[?1049h";
    public const string ExitAlternateScreen = "\u001b[?1049l";
    public const string ClearScreenAndHome = "\u001b[2J\u001b[H";
    public const string ClearToEndOfLine = "\u001b[K";
    public const string ShowCursor = "\u001b[?25h";

    // SGR control codes
    public const int Bold          = 1;
    public const int Muted         = 2;
    public const int Italic        = 3;
    public const int Underline     = 4;
    public const int SlowBlink     = 5;
    public const int RapidBlink    = 6;
    public const int Strikethrough = 9;

    public static Dictionary<TextFormat, int> FormatControlMap = new Dictionary<TextFormat, int>() {
        {TextFormat.Bold, Bold},
        {TextFormat.Muted, Muted},
        {TextFormat.Italic, Italic},
        {TextFormat.Underline, Underline},
        {TextFormat.SlowBlink, SlowBlink},
        {TextFormat.RapidBlink, RapidBlink},
        {TextFormat.Strikethrough, Strikethrough},
    };

    public static List<int> TextFormatToControlCodes(TextFormat format) {
        List<int> flags = [];
        foreach (TextFormat flag in Enum.GetValues<TextFormat>()) {
            if (flag == TextFormat.Normal) {
                continue;
            }

            if ((format & flag) != 0) {
                flags.Add(FormatControlMap[flag]);
            }
        }

        return flags;
    }

    /// <summary>
    /// Move the cursor to the screen position.
    /// </summary>
    /// <param name="row"></param>
    /// <param name="col"></param>
    /// <returns></returns>
    public static string MoveCursorTo(int row, int col) {
        return $"{ESC}[{row};{col}H";
    }

    /// <summary>
    /// Returns the control codes for setting the foreground color to the Color provided.
    /// </summary>
    /// <param name="color"></param>
    /// <returns></returns>
    public static string ForegroundColor(Color color) {
        return $"38;2;{color.Red};{color.Green};{color.Blue}";
    }

    /// <summary>
    /// Returns the control codes for setting the background color to the Color provided.
    /// </summary>
    /// <param name="color"></param>
    /// <returns></returns>
    public static string BackgroundColor(Color color) {
        return $"48;2;{color.Red};{color.Green};{color.Blue}";
    }

    /// <summary>
    /// Wrap text in color sequence(s). Resets the color mode at the end of the text.
    /// </summary>
    /// <param name="text"></param>
    /// <param name="fgColor"></param>
    /// <param name="bgColor"></param>
    /// <remarks>
    /// Example using both foreground and background colors:
    /// \x1b[38;2;17;57;66;48;2;214;248;255mHello World\x1b[0m
    /// </remarks>
    /// <returns></returns>
    public static string Colorize(string content, Color? fgColor, Color? bgColor) {
        if (fgColor is null) {
            if (bgColor is null) {
                return content;
            }

            return $"{ESC}[{BackgroundColor((Color)bgColor)}m{content}{Reset}";
        }

        if (bgColor is null) {
            return $"{ESC}[{ForegroundColor((Color)fgColor)}m{content}{Reset}";
        }

        Color fg = (Color)fgColor;
        Color bg = (Color)bgColor;
        string colorSeq = $"{ESC}[38;2;{fg.Red};{fg.Green};{fg.Blue};48;2;{bg.Red};{bg.Green};{bg.Blue}m";

        return $"{colorSeq}{content}{Reset}";
    }

    /// <summary>
    /// Return the style as an ANSI escape code sequence.
    /// </summary>
    /// <returns></returns>
    public static string EscapeSequence(CellStyle style) {
        if (style.IsEmpty()) {
            return "";
        }

        List<string> codes = [];
        if (style.FgColor is not null) {
            codes.Add(Ansi.ForegroundColor((Color)style.FgColor));
        }

        if (style.BgColor is not null) {
            codes.Add(Ansi.BackgroundColor((Color)style.BgColor));
        }

        foreach (int formatCode in Ansi.TextFormatToControlCodes(style.Format)) {
            codes.Add($"{formatCode}");
        }

        return $"{Ansi.ESC}[{string.Join(';', codes)}m";
    }
}
