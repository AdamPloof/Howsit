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
public interface ILayoutManager {
    public void Allocate();
}
