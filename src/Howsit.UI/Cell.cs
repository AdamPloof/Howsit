using System;
using System.Collections.Generic;
using Howsit.UI.Style;

namespace Howsit.UI;

/// <summary>
/// A single cell of the screen. The TUI equivalent of a pixel.
/// </summary>
public readonly record struct Cell {
    public readonly char Value { get; init; }
    public readonly TextFormat Format { get; init; } = TextFormat.Normal;
    public readonly Color? FontColor { get; init; }
    public readonly Color? BgColor { get; init; }

    public Cell(char value) {
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
        return new Cell(' ');
    }

    /// <summary>
    /// Helper for creating a buffer of empty cells
    /// </summary>
    /// <param name="num">The number of empty cells to create</param>
    /// <returns></returns>
    public static Cell[] EmptyCells(int num) {
        if (num == 0) {
            return [];
        }

        Cell[] cells = new Cell[num];
        for (int i = 0; i < num; i++) {
            cells[i] = Cell.Empty();
        }

        return cells;
    }

    public bool IsEmpty() {
        return Value == ' ';
    }

    public bool StyleIsEmpty() {
        return IsEmpty() || (
            Format == TextFormat.Normal
            && FontColor is null
            && BgColor is null
        );
    }
}
