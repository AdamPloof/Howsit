using System;
using Howsit.UI;
using Howsit.UI.Style;

namespace Howsit.UI.Widgets;

/// <summary>
/// Basic Label widget for displaying static text
/// </summary>
public class Label : AbstractWidget {
    private string _content;
    private CellStyle? _style;

    public Label(IWidget? parent, string content) : base(parent) {
        _content = content;
        _style = null;
    }

    public Label(IWidget? parent, string content, CellStyle style) : base(parent) {
        _content = content;
        _style = style;
    }

    public void SetContent(string content) {
        _content = content;
    }

    public void SetStyle(CellStyle? style) {
        _style = style;
    }

    public override Cell[] Paint() {
        if (BoundingBox.IsEmpty()) {
            return new Cell[0];
        }

        if (_style is CellStyle s) {
            return TextBuffer.FromString(_content, BoundingBox.Width, BoundingBox.Height, s);
        } else {
            return TextBuffer.FromString(_content, BoundingBox.Width, BoundingBox.Height);
        }
    }
}
