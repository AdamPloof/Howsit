using System;
using System.Collections.Generic;

using System.Linq;
using Howsit.UI;

namespace Howsit.UI.Style;

/// <summary>
/// Text formatting options.
/// </summary>
/// <remarks>
/// Can be combined by using bitwise logical operators | or &.
/// </remarks>
/// <see href="https://learn.microsoft.com/en-us/dotnet/csharp/language-reference/builtin-types/enum" />
/// <see href="https://learn.microsoft.com/en-us/dotnet/api/system.flagsattribute?view=net-10.0" />
[Flags]
public enum TextFormat {
    Normal        = 0b_0000_0000,  // 0
    Italic        = 0b_0000_0001,  // 1
    Bold          = 0b_0000_0010,  // 2
    Underline     = 0b_0000_0100,  // 4
    Muted         = 0b_0000_1000,  // 8
    SlowBlink     = 0b_0001_0000,  // 16
    RapidBlink    = 0b_0010_0000,  // 32
    Strikethrough = 0b_0100_0000,  // 64
}

/// <summary>
/// Components of style formatting.
/// </summary>
public readonly record struct CellStyle {
    public readonly TextFormat Format { get; init; } = TextFormat.Normal;
    public readonly Color? FgColor { get; init; }
    public readonly Color? BgColor { get; init; }

    public CellStyle(
        TextFormat format,
        Color? fgColor = null,
        Color? bgColor = null
    ) {
        Format = format;
        FgColor = fgColor;
        BgColor = bgColor;
    }

    /// <summary>
    /// Returns true if the style has no text format or color values.
    /// </summary>
    /// <returns></returns>
    public bool IsEmpty() {
        return Format == TextFormat.Normal && FgColor is null && BgColor is null;
    }

    /// <summary>
    /// Return the style as an ANSI escape code sequence.
    /// </summary>
    /// <returns></returns>
    public string EscapeSequence() {
        if (IsEmpty()) {
            return "";
        }

        List<string> codes = [];
        if (FgColor is not null) {
            codes.Add(Ansi.ForegroundColor((Color)FgColor));
        }

        if (BgColor is not null) {
            codes.Add(Ansi.BackgroundColor((Color)BgColor));
        }

        foreach (int formatCode in Ansi.TextFormatToControlCodes(Format)) {
            codes.Add($"{formatCode}");
        }

        return $"{Ansi.ESC}[{string.Join(';', codes)}m";
    }
}
