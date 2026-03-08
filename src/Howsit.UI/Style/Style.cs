using System;

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
    Normal = 0b_0000_0000,  // 0
    Italic = 0b_0000_0001,  // 1
    Bold = 0b_0000_0010,  // 2
    Underline = 0b_0000_0100,  // 4
    Muted = 0b_0000_1000,  // 8
    Blink = 0b_0001_0000,  // 16
    Strikethrough = 0b_0010_0000,  // 32
}
