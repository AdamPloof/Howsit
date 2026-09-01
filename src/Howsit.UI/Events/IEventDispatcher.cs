using Howsit.UI.Widgets;

namespace Howsit.UI.Events;

/// <summary>
/// IEventDispatcher is responsible for passing events to the widget tree
/// or to specific target widgets. Events can propagate down the widget hierarchy.
/// </summary>
public interface IEventDispatcher {
    /// <summary>
    /// Forward the event to a target widget and its children.
    /// </summary>
    /// <remarks>
    /// If an event should propagate through the entire widget tree, the
    /// target should be the top level window.
    /// </remarks>
    /// <param name="target"></param>
    /// <param name="uiEvent"></param>
    public void Dispatch(IWidget target, UiEvent uiEvent);
}
