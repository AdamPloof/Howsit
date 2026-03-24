using System;

using Howsit.UI.Drawing;
using Howsit.UI.Layout;

namespace Howsit.UI.Widgets;

public abstract class AbstractWidget : IWidget {
    /// <inheritdoc />
    public bool Visible { get; set; }

    /// <inheritdoc />
    public Size MinSize { get; set; }

    /// <inheritdoc />
    public Size MaxSize { get; set; }

    /// <inheritdoc />
    public Size PreferredSize { get; set; }

    /// <inheritdoc />
    public int StretchHorizontal { get; set; }

    /// <inheritdoc />
    public int StretchVertical { get; set; }

    /// <inheritdoc />
    public Padding Padding { get; set; }

    /// <inheritdoc />
    public Border Border { get; set; }

    /// <inheritdoc />
    public int Zindex { get; set; }

    /// <inheritdoc />
    public Rect BoundingBox { get; set; }

    protected Guid _id;

    /// <summary>
    /// All Widgets that inherit from AbstractWidget should call the base constructor
    /// to ensure that its Id is set.
    /// </summary>
    public AbstractWidget() {
        _id = Guid.NewGuid();
        BoundingBox = new Rect();
        MinSize = Size.Empty();
        PreferredSize = Size.Empty();
        MaxSize = Size.Empty();
    }

    /// <inheritdoc />
    public Guid GetId() {
        return _id;
    }

    /// <inheritdoc />
    public void SetBounds(Rect rect) {
        BoundingBox = rect;
    }

    /// <inheritdoc />
    public int GetHeight() {
        return BoundingBox.Height;
    }

    /// <inheritdoc />
    public int GetWidth() {
        return BoundingBox.Width;
    }

    /// <inheritdoc />
    public void Move(int x, int y) {
        BoundingBox.X = x;
        BoundingBox.Y = y;
    }
    
    /// <inheritdoc />
    public void Resize(int width, int height) {
        BoundingBox.Width = width;
        BoundingBox.Height = height;
    }

    /// <inheritdoc />
    public abstract Cell[] Paint();
}
