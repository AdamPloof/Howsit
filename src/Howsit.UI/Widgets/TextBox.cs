using System;
using Howsit.UI;
using Howsit.UI.Drawing;
using Howsit.UI.Layout;
using Howsit.UI.Style;

namespace Howsit.UI.Widgets;

/// <summary>
/// A standard text box for displaying text. 
/// </summary>
public class TextBox : Widget {
    /// <summary>
    /// Indicates whether the text box should accept input events to modify
    /// its content.
    /// </summary>
    public bool ReadOnly { get; set; } = false;

    /// <inheritdoc />
    public override bool AcceptsFocus { get; protected set; } = true;

    private string? _content;
    private CellStyle _style;
    private Cell[] _cachedBuffer;

    public TextBox(IWidget? parent, string? content, CellStyle? style) : base(parent) {
        _content = content;
        _style = style ?? new CellStyle();
        _cachedBuffer = [];
    }

    /// <summary>
    /// Set the text content.
    /// </summary>
    /// <remarks>
    /// Even if the text box is ReadOnly, it is still possible to set the content
    /// via this method. ReadOnly prevents text content from being edited via
    /// input events.
    /// </remarks>
    /// <param name="content"></param>
    public void SetContent(string? content) {
        _content = content;
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
    /// Accepts focus when ReadOnly is false.
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
