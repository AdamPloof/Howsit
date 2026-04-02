using System;
using System.Collections.Generic;

using Howsit.UI.Drawing;
using Howsit.UI.Layout;

namespace Howsit.UI.Widgets;

/// <summary>
/// AbstractWidget provides a base implementation for the most common parts of a widget.
/// Most widgets will inherity from this rather than fully implementing IWidget from scratch.
/// </summary>
public abstract class AbstractWidget : IWidget {
    public IWidget? Parent { get; set; }

    /// <inheritdoc />
    public bool Visible { get; set; }

    /// <inheritdoc />
    public Size SizeHint { get; set; }

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

    protected List<IWidget> _children;

    /// <summary>
    /// All Widgets that inherit from AbstractWidget should call the base constructor
    /// to ensure that its Id is set.
    /// </summary>
    public AbstractWidget(IWidget? parent) {
        BoundingBox = new Rect();
        SizeHint = Size.Empty();
        _id = Guid.NewGuid();
        _children = new List<IWidget>();

        // Attaching the parent to the child from the constructor this way is ok for now. If
        // the attach step becomes more involved and depends on a fully initialized child, this
        // might require changing.
        Parent = parent;
        parent?.AddChild(this);
    }

    /// <inheritdoc />
    public Guid GetId() {
        return _id;
    }

    public void AddChild(IWidget child) {
        if (child.Parent != this) {
            child.Parent = this;
        }

        if (_children.Find(c => c.GetId() == child.GetId()) is not null) {
            return;
        }

        _children.Add(child);
    }

    /// <inheritdoc />
    public void SetBounds(Rect rect) {
        BoundingBox = rect;
    }

    /// <inheritdoc />
    public int X() {
        return BoundingBox.X;
    }

    /// <inheritdoc />
    public int Y() {
        return BoundingBox.Y;
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
    public void Nudge(int xAmount, int yAmount) {
        BoundingBox.X += xAmount;
        BoundingBox.Y += yAmount;
    }

    /// <inheritdoc />
    public void Resize(int width, int height) {
        BoundingBox.Width = width;
        BoundingBox.Height = height;
    }

    /// <inheritdoc />
    public Rect ContentArea() {
        int width = BoundingBox.Width - Padding.Left - Padding.Right;
        int x = BoundingBox.X + Padding.Left;
        if (Border.Left != BorderStyle.None) {
            width -= 1;
            x += 1;
        }

        if (Border.Right != BorderStyle.None) {
            width -= 1;
        }

        int height = BoundingBox.Height - Padding.Bottom - Padding.Top;
        int y = BoundingBox.Y + Padding.Top;
        if (Border.Top != BorderStyle.None) {
            height -= 1;
            y += 1;
        }

        if (Border.Bottom != BorderStyle.None) {
            height -= 1;
        }

        return new Rect(x, y, width, height);
    }

    /// <inheritdoc />
    public abstract Cell[] Paint();
}
