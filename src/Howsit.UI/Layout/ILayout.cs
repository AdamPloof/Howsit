using System.Collections.Generic;
using Howsit.UI.Widgets;

namespace Howsit.UI.Layout;

/// <summary>
/// Interface for classes responsible for managing the layout for a collection
/// of widgets.
/// </summary>
/// <remarks>
/// Key responsibilities include
/// - Reading the available size and allocating space to each widget
///   in the layout.
/// - Handling window resizes and updating the allocated space
/// </remarks>
public interface ILayout {
    /// <summary>
    /// Calculate the bounding boxes for an collection of widgets in order to
    /// fit within their parent's bounding box.
    /// </summary>
    public void Arrange(IReadOnlyList<IWidget> widgets, Rect bounds);
}
