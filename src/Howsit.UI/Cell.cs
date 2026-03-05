namespace Howsit.UI;

using System.Collections.Generic;
using Howsit.UI.Style;

/// <summary>
/// A single cell of the screen. The TUI equivalent of a pixel.
/// </summary>
public class Cell : ICell, IEquatable<Cell> {
    public char? Value { get; set; }
    public TextFormat Format { get; set; } = TextFormat.Normal;
    public Color? FontColor { get; set; }
    public Color? BgColor { get; set; }

    public Cell(char? value) {
        Value = value;
    }

    public Cell(
        char value,
        TextFormat format,
        Color? fontColor = null,
        Color? bgColor = null
    ) {
        Value = value;
        Format = format;
        FontColor = fontColor;
        BgColor = bgColor;
    }

    public static Cell Empty() {
        return new Cell(null);
    }

    public bool IsEmpty() {
        return Value is null;
    }

    public bool StyleIsEmpty() {
        return IsEmpty() || (
            Format == TextFormat.Normal
            && FontColor is null
            && BgColor is null
        );
    }

    public override bool Equals(object? obj) {
        return obj is Cell c && Equals(c);
    }

    public bool Equals(Cell? other) {
        return other is not null
            && Value == other.Value
            && Format == other.Format
            && FontColor == other.FontColor
            && BgColor == other.BgColor;
    }

    public static bool operator == (Cell c1, Cell c2) {
        return Equals(c1, c2);
    }
    
    public static bool operator != (Cell c1, Cell c2) {
        return !(c1 == c2);
    }

    public override int GetHashCode() {
        return HashCode.Combine(
            Value,
            Format,
            FontColor.GetHashCode(),
            BgColor.GetHashCode()
        );
    }
}
