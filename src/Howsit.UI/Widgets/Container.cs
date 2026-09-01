using System;

using Howsit.UI;
using Howsit.UI.Layout;
using Howsit.UI.Events;

namespace Howsit.UI.Widgets;

/// <summary>
/// Base container type. Containers are used to group widgets into a layout section.
/// Their layout style/orientation is determined by the ILayout type provided.
/// </summary>
public class Container : Widget, IContainer {
    public override bool AcceptsFocus { get; protected set; } = false;

    private ILayout _layout;

    public Container(IWidget? parent, ILayout layout) : base(parent) {
        LayoutIsDirty = true;
        _layout = layout;

        if (parent is null) {
            // Accept focus if container is the root widget
            AcceptsFocus = true;
        }

        AddHandler<ResizeEvent>(HandleResize);
    }

    /// <inheritdoc />
    public bool LayoutIsDirty { get; set; }

    /// <inheritdoc />
    public void PerformLayout() {
        if (BoundingBox.IsEmpty()) {
            throw new Exception("Unable to layout widget. Bounds is empty");
        }

        _layout.Arrange(_children, BoundingBox);
        LayoutIsDirty = false;
    }

    /// <summary>
    /// Paint each widget and combine into the container's buffer.
    /// </summary>
    /// <remarks>
    /// Preconditions:
    /// - Layout has been performed for child widgets
    /// - BoundingBox of Container is not empty
    /// </remarks>
    /// <returns></returns>
    public override Cell[] Paint() {
        if (BoundingBox.IsEmpty()) {
            throw new Exception("Unable to paint widget. Bounds is empty");
        }

        if (LayoutIsDirty) {
            PerformLayout();
        }

        Cell[] buffer = Cell.EmptyCells(BoundingBox.Width * BoundingBox.Height);
        foreach (IWidget w in _children) {
            int row = w.Y();
            int col = w.X();
            Cell[] widgetBuffer = w.Paint();
            foreach (Cell c in widgetBuffer) {
                if (col - w.X() >= w.GetWidth()) {
                    row++;
                    col = w.X();
                }

                int pos = col + (row * BoundingBox.Width);
                if (pos >= buffer.Length) {
                    // This indicates a bug in the Layout
                    throw new Exception("Paint position exceeded buffer size");
                }

                buffer[pos] = c;
                col++;
            }
        }

        IsDirty = false;

        return buffer;
    }

    public void HandleResize(ResizeEvent resizeEvent) {
        if (Parent is null) {
            // This is the root widget, reset bounds to window dimensions
            BoundingBox.Width = resizeEvent.Width;
            BoundingBox.Height = resizeEvent.Height;
        }

        LayoutIsDirty = true;
        IsDirty = true;
    }

    /// <summary>
    /// Generally containers are not eligible for focus. The exception is
    /// the root widget.
    /// </summary>
    /// <returns></returns>
    public override bool SetFocus() {
        if (Parent is null) {
            HasFocus = true;

            return true;
        }

        return false;
    }

    /// <summary>
    /// Containers always accept clear focus requests.
    /// </summary>
    /// <returns></returns>
    public override bool ClearFocus() {
        HasFocus = false;

        return true;
    }

    /// <inheritdoc />
    public override bool CaptureTabKey() {
        return false;
    }
}
