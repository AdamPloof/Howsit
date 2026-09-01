namespace Howsit.UI.Events;

/// <summary>
/// An event for text input by the user.
/// </summary>
public class TextInputEvent : UiEvent {
    public string Text { get; }

    public TextInputEvent(string text) {
        Text = text;
    }
}
