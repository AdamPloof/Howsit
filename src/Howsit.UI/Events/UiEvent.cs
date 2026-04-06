namespace Howsit.UI.Events;

/// <summary>
/// Base class for all Events
/// </summary>
public abstract class UiEvent {
    public bool Handled { get; private set; }

    /// <summary>
    /// Mark the event as handled. This stops the propagation/bubbling of the event.
    /// </summary>
    public void MarkHandled() {
        Handled = true;
    }
}
