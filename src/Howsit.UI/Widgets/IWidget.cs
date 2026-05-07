using System;
using System.Collections.Generic;

using Howsit.UI;
using Howsit.UI.Drawing;
using Howsit.UI.Layout;
using Howsit.UI.Events;

namespace Howsit.UI.Widgets;

/// <summary>
/// Widgets are elements that can be placed on the screen. They have their own
/// way of handling inputs behavior.
/// </summary>
/// <remarks>
/// Note that only the Height and Width properties refer to actual dimensions. Which the
/// layout manager is responsible for setting. All other dimension props are used to request
/// size from the layout manager.
/// </remarks>
public interface IWidget {
    /// <summary>
    /// The widget's parent. A null parent indicates the widget is unattached to the widget tree
    /// or the widget is the root node.
    /// </summary>
    /// <remarks>
    /// It is very unlikely that would need to update a widgets parent directly after
    /// it has been created.
    /// </remarks>
    public IWidget? Parent { get; set; }

    /// <summary>
    /// Indicates whether the widget should be painted or not. Hidden widgets
    /// do not take up any space in the layout.
    /// </summary>
    public bool Visible { get; set; }

    /// <summary>
    /// Requested for the widget
    /// </summary>
    public Size SizeHint { get; set; }

    /// <summary>
    /// Amount that the widget should stretch horizontally relative to its adjacent siblings.
    /// A value of 0 means no stretch, greater than 0 means grow, less than 0 means shrink.
    /// </summary>
    /// <example>
    /// Widgets A and B are side by side. A.StretchHorizontal = 1 and B.StretchHorizontal = 2
    /// --------------------------------
    /// | A       | B                   |
    /// |         | Gets 2x the space   |
    /// |         |                     |
    /// |_______________________________|
    /// 
    /// When A.StretchHorizontal = 2 and B.StretchHorizontal = 2
    /// --------------------------------
    /// | A            | B              |
    /// |              | Equal space    |
    /// |              |                |
    /// |_______________________________|
    /// </example>
    public int StretchHorizontal { get; set; }

    /// <summary>
    /// Amount that the widget should stretch vertically relative to its adjacent siblings
    /// A value of 0 means no stretch, greater than 0 means grow, less than 0 means shrink.
    /// </summary>
    public int StretchVertical { get; set; }

    /// <summary>
    /// Padding for an element.
    /// </summary>
    public Padding Padding { get; set; }

    /// <summary>
    /// Border for an element.
    /// </summary>
    public Border Border { get; set; }

    /// <summary>
    /// The z-index of the widget. Used to resolve which cell gets rendered when
    /// there is overlapping content.
    /// </summary>
    public int Zindex { get; set; }

    /// <summary>
    /// Bounding box of the widget. This is set by the layout manager when allocating space
    /// via SetBounds.
    /// </summary>
    public Rect BoundingBox { get; set; }

    /// <summary>
    /// Indicates whether the widget requires repainting.
    /// </summary>
    public bool IsDirty { get; set; }

    /// <summary>
    /// Returns true if the widget has the focus for input and other events.
    /// </summary>
    public bool HasFocus { get; }

    /// <summary>
    /// Indicates whether the widget accepts focus for input events.
    /// </summary>
    /// <remarks>
    /// If this is true, the widget must abide by that when SetFocus() is called.
    /// </remarks>
    public bool AcceptsFocus { get; }

    /// <summary>
    /// Get the widgets unique ID.
    /// </summary>
    /// <returns></returns>
    public Guid GetId();

    /// <summary>
    /// The child widgets.
    /// </summary>
    /// <returns></returns>
    public IEnumerable<IWidget> GetChildren();

    /// <summary>
    /// Adds a child widget.
    /// </summary>
    /// <remarks>
    /// You should not need to manually add child widgets. The widget tree will be built by
    /// passing the parent to new widgets.
    /// </remarks>
    /// <param name="widget"></param>
    public void AddChild(IWidget widget);

    /// <summary>
    /// Set the bounding box of the widget. Used by layouts to allocate space.
    /// </summary>
    /// <param name="rect"></param>
    public void SetBounds(Rect rect);

    /// <summary>
    /// Helper for getting the x coordinate of the widget's bounding box.
    /// </summary>
    /// <returns></returns>
    public int X();

    /// <summary>
    /// Helper for getting the y coordinate of the widget's bounding box.
    /// </summary>
    /// <returns></returns>
    public int Y();

    /// <summary>
    /// Helper for getting the height of the widget's bounding box.
    /// </summary>
    /// <returns></returns>
    public int GetHeight();

    /// <summary>
    /// Helper for getting the width of the widget's bounding box.
    /// </summary>
    /// <returns></returns>
    public int GetWidth();

    /// <summary>
    /// Move the widget to the coordinates provided.
    /// </summary>
    /// <summary>
    /// This method is called performing the layout for the widget. Attempts to
    /// call this directly will have no effect. 
    /// </summary>
    /// <param name="x"></param>
    /// <param name="y"></param>
    public void Move(int x, int y);

    /// <summary>
    /// Similiar to Move but shifts the widget the specified amount along an axis.
    /// </summary>
    /// <summary>
    /// This method is called performing the layout for the widget. Attempts to
    /// call this directly will have no effect. 
    /// </summary>
    /// <param name="xAmount"></param>
    /// <param name="yAmount"></param>
    public void Nudge(int xAmount, int yAmount);

    /// <summary>
    /// Resize the widget to the new dimensions.
    /// </summary>
    /// <summary>
    /// This method is called performing the layout for the widget. Attempts to
    /// call this directly will have no effect. 
    /// </summary>
    /// <param name="x"></param>
    /// <param name="y"></param>
    public void Resize(int width, int height);

    /// <summary>
    /// Returns the inner rect inside the widgets border and padding. 
    /// </summary>
    /// <returns></returns>
    public Rect ContentArea();

    /// <summary>
    /// Render the cell to a cell buffer.
    /// </summary>
    /// <returns></returns>
    public Cell[] Paint();

    /// <summary>
    /// Give the keyboard focus to this widget. Returns true if the widget accepts focus.
    /// </summary>
    public bool SetFocus();

    /// <summary>
    /// Removes the keyboard focus from this widget. Returns true if the widget allows focus
    /// to be cleared.
    /// </summary>
    public bool ClearFocus();

    /// <summary>
    /// Indicates that the widget should capture tab key events and prevent tab from
    /// changing the focused widget.
    /// </summary>
    /// <returns></returns>
    public bool CaptureTabKey();

    /// <summary>
    /// Register an event handler for a specific event type. Multiple handlers can be registered
    /// for the same event.
    /// </summary>
    /// <typeparam name="TEvent"></typeparam>
    /// <param name="handler"></param>
    public void AddHandler<TEvent>(Action<TEvent> handler) where TEvent : UiEvent;

    /// <summary>
    /// Forward an event along to a registered handler. If no handler is registered for
    /// the event type, it is ignored.
    /// </summary>
    /// <remarks>
    /// When multiple handlers are registered for the same event, they will be called in the order
    /// that they were added.
    /// </remarks>
    /// <param name="uiEvent"></param>
    public void HandleEvent(UiEvent uiEvent);
}
