using System;
using Howsit.UI;
using Howsit.UI.Drawing;
using Howsit.UI.Layout;
using Howsit.UI.Style;

namespace Howsit.UI.Widgets;

/// <summary>
/// Basic Label widget for displaying static text
/// </summary>
public class Label : Widget {
    private string _content;
    private CellStyle _style;
    private Cell[] _cachedBuffer;

    public Label(IWidget? parent, string content) : base(parent) {
        _content = content;
        _style = new CellStyle();
        _cachedBuffer = [];
    }

    public Label(IWidget? parent, string content, CellStyle style) : base(parent) {
        _content = content;
        _style = style;
        _cachedBuffer = [];
    }

    public void SetContent(string content) {
        _content = content;
    }

    public void SetStyle(CellStyle style) {
        _style = style;
    }

    public override Cell[] Paint() {
        if (!IsDirty) {
            return _cachedBuffer;
        }

        if (BoundingBox.IsEmpty()) {
            return new Cell[0];
        }

        Cell[] buffer = Cell.EmptyCells(BoundingBox.Width * BoundingBox.Height);
        if (!Border.IsNone()) {
            BorderPainter.ApplyBorder(buffer, BoundingBox.Width, BoundingBox.Height, Border);
        }

        Rect contentRect = ContentArea();
        Cell[] contentCells = TextBuffer.FromString(
            _content,
            contentRect.Width,
            contentRect.Height,
            _style
        );
        int contentStart = contentRect.X + (contentRect.Y * contentRect.Width);
        Array.Copy(contentCells, 0, buffer, contentStart, contentCells.Length);

        return buffer;
    }
}
