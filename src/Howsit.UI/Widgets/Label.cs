using System;
using Howsit.UI;

namespace Howsit.UI.Widgets;

/// <summary>
/// Basic Label widget for displaying static text
/// </summary>
public class Label : AbstractWidget {
    private string _content;

    public Label(string content) : base() {
        _content = content;
    }

    public override Cell[] Paint() {
        if (BoundingBox.IsEmpty()) {
            return new Cell[0];
        }
        Cell[] cells = Cell.EmptyCells(BoundingBox.Width * BoundingBox.Height);
        int idx = 0;
        foreach (char c in _content) {
            if (idx >= cells.Length) {
                break;
            }

            cells[idx] = new Cell(c);
            idx++;
        }

        return cells;
    }
}
