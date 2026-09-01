using System;
using QuickTui.UI;
using QuickTui.UI.Drawing;
using QuickTui.UI.Layout;
using QuickTui.UI.Style;

namespace QuickTui.UI.Widgets;

/// <summary>
/// A basic widget for displaying static content. 
/// </summary>
/// <todo>Could this just be replaced by a ReadOnly Textbox?</todo>
public class Canvas : Widget {
    /// <inheritdoc />
    public override bool AcceptsFocus { get; protected set; } = false;

    private string? _content;
    private CellStyle _style;
    private Cell[] _cachedBuffer;

    public Canvas(IWidget? parent, string? content, CellStyle? style) : base(parent) {
        _content = content;
        _style = style ?? new CellStyle();
        _cachedBuffer = [];
    }

    /// <summary>
    /// Set the text content.
    /// </summary>
    /// <param name="content"></param>
    public void SetContent(string? content) {
        _content = content;
        IsDirty = true;
    }

    public void SetStyle(CellStyle style) {
        _style = style;
    }

    /// <inheritdoc />
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
            _content ?? "",
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
    /// Does not accept focus.
    /// </summary>
    /// <returns></returns>
    public override bool SetFocus() {
        return false;
    }

    /// <summary>
    /// A Canvas cannot be focused on to begin with, they always respond to clear focus
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
