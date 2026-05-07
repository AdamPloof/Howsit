using System;
using System.Collections.Generic;

using Howsit.UI.Drawing;
using Howsit.UI.Layout;
using Howsit.UI.Events;

namespace Howsit.UI.Widgets;

/// <summary>
/// AbstractWidget provides a base implementation for the most common parts of a widget.
/// Most widgets will inherity from this rather than fully implementing IWidget from scratch.
/// </summary>
public abstract class Widget : IWidget {
    /// <inheritdoc />
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

    /// <inheritdoc />
    public bool IsDirty { get; set; }

    /// <inheritdoc />
    public bool HasFocus { get; protected set; }

    /// <inheritdoc />
    public abstract bool AcceptsFocus { get; protected set; }

    /// <summary>
    /// Unique identifier for this Widget
    /// </summary>
    protected Guid _id;

    /// <summary>
    /// Collection of child widgets.
    /// </summary>
    protected List<IWidget> _children;

    /// <summary>
    /// Registered event handlers.
    /// </summary>
    protected readonly Dictionary<Type, List<Action<UiEvent>>> _handlers = [];

    /// <summary>
    /// All Widgets that inherit from AbstractWidget should call the base constructor
    /// to ensure that its Id is set.
    /// </summary>
    public Widget(IWidget? parent) {
        SizeHint = Size.Empty();
        BoundingBox = new Rect();
        IsDirty = true;

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

    /// <inheritdoc />
    public IEnumerable<IWidget> GetChildren() {
        return _children;
    }

    /// <inheritdoc />
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
        if (rect == BoundingBox) {
            return;
        }

        BoundingBox = rect;
        IsDirty = true;
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
        int x = Padding.Left;
        if (Border.Left != BorderStyle.None) {
            width -= 1;
            x += 1;
        }

        if (Border.Right != BorderStyle.None) {
            width -= 1;
        }

        int height = BoundingBox.Height - Padding.Bottom - Padding.Top;
        int y = Padding.Top;
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

    /// <inheritdoc />
    public abstract bool SetFocus();

    /// <inheritdoc />
    public abstract bool ClearFocus();

    /// <inheritdoc />
    public abstract bool CaptureTabKey();

    /// <inheritdoc />
    public void AddHandler<TEvent>(Action<TEvent> handler) where TEvent : UiEvent {
        Type eventType = typeof(TEvent);
        if (!_handlers.TryGetValue(eventType, out List<Action<UiEvent>>? handlers)) {
            handlers = [];
            _handlers[eventType] = handlers;
        }

        handlers.Add((e) => handler((TEvent)e));
    }

    public void HandleEvent(UiEvent uiEvent) {
        Type eventType = uiEvent.GetType();
        if (_handlers.TryGetValue(eventType, out List<Action<UiEvent>>? handlers)) {
            foreach (Action<UiEvent> handler in handlers) {
                handler(uiEvent);
                if (uiEvent.Handled) {
                    // Propagation stopped.
                    return;
                }
            }
        }
    }
}
