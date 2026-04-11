namespace Howsit.UI.Events;

/// <summary>
/// Resize events are triggered when the window is resized or when the allocated space
/// for a widget or container changes.
/// </summary>
public class ResizeEvent : UiEvent {
    public int Width { get; init; }
    public int Height { get; init; }

    public ResizeEvent(int width, int height) {
        Width = width;
        Height = height;
    }
}
