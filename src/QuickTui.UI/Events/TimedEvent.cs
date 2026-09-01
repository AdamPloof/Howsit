using System;

namespace QuickTui.UI.Events;

/// <summary>
/// An event for special key input like ctrl+c, esc, function keys, etc.
/// </summary>
public class TimedEvent : UiEvent {
    private TimeSpan _timeout;
    private Action _cb;

    public TimedEvent(TimeSpan timeout, Action cb) {
        _timeout = timeout;
        _cb = cb;
    }
    
    /// <summary>
    /// Call the event's callback.
    /// </summary>
    public void Execute() {
        _cb();
    }
}
