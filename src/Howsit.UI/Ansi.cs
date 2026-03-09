using Howsit.UI.Style;

namespace Howsit.UI;

public static class Ansi {
    public const string ESC = "\x1B";

    public const string Reset = ESC + "[0m";

    public const string Bold = ESC + "[1m";
    public const string Muted = ESC + "[2m";
    public const string Italic = ESC + "[3m";
    public const string Underline = ESC + "[4m";
    public const string SlowBlink = ESC + "[5m";
    public const string RapidBlink = ESC + "[6m";
    public const string StrikeThrough = ESC + "[9m";

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
    /// Set the foreground color.
    /// </summary>
    /// <param name="color"></param>
    /// <remarks>
    /// Example 24-bit foreground color with text: \x1b[38;2;17;57;66mHello World\x1b[0m
    /// 
    /// Note that you need to remember to reset the color when using this method to
    /// manually set the foreground color.
    /// 
    /// Use ColorizeText() to wrap text in a color without having to manually reset.
    /// </remarks>
    /// <returns></returns>
    public static string SetForegroundColor(Color color) {
        return $"{ESC}[38;2;{color.Red};{color.Green};{color.Blue}m";
    }

    /// <summary>
    /// Set the foreground color.
    /// </summary>
    /// <param name="color"></param>
    /// <remarks>
    /// Example 24-bit background color with text: \x1b[48;2;17;57;66mHello World\x1b[0m
    /// 
    /// Note that you need to remember to reset the color when using this method to
    /// manually set the background color.
    /// 
    /// Use ColorizeText() to wrap text in a color without having to manually reset.
    /// </remarks>
    /// <returns></returns>
    public static string SetBackgroundColor(Color color) {
        return $"{ESC}[48;2;{color.Red};{color.Green};{color.Blue}m";
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

            return $"{SetBackgroundColor((Color)bgColor)}{content}{ESC}{Reset}";
        }

        if (bgColor is null) {
            return $"{SetForegroundColor((Color)fgColor)}{content}{ESC}{Reset}";
        }

        Color fg = (Color)fgColor;
        Color bg = (Color)bgColor;
        string colorSeq = $"{ESC}[38;2{fg.Red};{fg.Green};{fg.Blue};48;2{bg.Red};{bg.Green};{bg.Blue}m";

        return $"{colorSeq}{content}{ESC}{Reset}";
    }
}
