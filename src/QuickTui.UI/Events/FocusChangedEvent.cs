using QuickTui.UI.Widgets;

namespace QuickTui.UI.Events;

/// <summary>
/// Indicates the focused widget has changed. Provides a reference the new focused
/// widget as well as the previously focused widget. Previously focused widget may
/// be null on initial render.
/// </summary>
public class FocusChangedEvent : UiEvent {
    public IWidget? PreviousFocus { get; }
    public IWidget Focused { get; }

    public FocusChangedEvent(IWidget? previousFocus, IWidget focused) {
        PreviousFocus = previousFocus;
        Focused = focused;
    }
}
