using System;
using System.Collections.Generic;
using Howsit.UI.Style;

namespace Howsit.UI;

/// <summary>
/// A single cell of the screen. The TUI equivalent of a pixel.
/// </summary>
public readonly record struct Cell {
    public readonly char Value { get; init; }
    public readonly CellStyle? Style { get; init; }

    public Cell(char value) {
        Value = value;
        Style = null;
    }

    public Cell(
        char value,
        CellStyle style
    ) {
        Value = value;
        Style = style;
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
}
