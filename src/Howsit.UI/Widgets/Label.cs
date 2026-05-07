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
    public override bool AcceptsFocus { get; protected set; } = false;

    private string _content;
    private CellStyle _style;
    private Cell[] _cachedBuffer;

    public Label(IWidget? parent, string content) : base(parent) {
        _content = content;
        _style = new CellStyle();
        _cachedBuffer = [];
        HandleContentChanged();
    }

    public Label(IWidget? parent, string content, CellStyle style) : base(parent) {
        _content = content;
        _style = style;
        _cachedBuffer = [];
        HandleContentChanged();
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

        int col = contentRect.X;
        int row = contentRect.Y;
        for (int i = 0; i < contentCells.Length; i++) {
            if (col - contentRect.X >= contentRect.Width) {
                col = contentRect.X;
                row++;
            }

            buffer[col + (row * BoundingBox.Width)] = contentCells[i];
            col++;
        }

        _cachedBuffer = buffer;
        IsDirty = false;

        return buffer;
    }

    /// <summary>
    /// Calculate a new size hint to fit the content. If original size hint
    /// has enough space then use the original.
    /// </summary>
    /// <param name="content"></param>
    private void HandleContentChanged() {
        Size newSize = Size.Empty();
        foreach (char c in _content) {
            if (c == '\r') {
                // Break on \n, ignore \r in case of \r\n
                continue;
            }

            if (c == '\n') {
                newSize.Height += 1;
            } else {
                newSize.Width += 1;
            }
        }

        newSize.Width += Padding.Left + Padding.Right;
        newSize.Height += Padding.Top + Padding.Bottom;
        if (Border.Left != BorderStyle.None) {
            newSize.Width += 1;
        }

        if (Border.Right != BorderStyle.None) {
            newSize.Width += 1;
        }

        if (Border.Top != BorderStyle.None) {
            newSize.Height += 1;
        }

        if (Border.Bottom != BorderStyle.None) {
            newSize.Height += 1;
        }

        if (newSize.Width > SizeHint.Width || newSize.Height > SizeHint.Height) {
            SizeHint = newSize;
        }
    }

    /// <summary>
    /// Labels are not eligible for focus.
    /// </summary>
    /// <returns></returns>
    public override bool SetFocus() {
        return false;
    }

    /// <summary>
    /// Since labels cannot be focused on to begin with, they always respond to clear focus
    /// requests with true.
    /// </summary>
    /// <returns></returns>
    public override bool ClearFocus() {
        return true;
    }

    /// <inheritdoc />
    public override bool CaptureTabKey() {
        return false;
    }
}
