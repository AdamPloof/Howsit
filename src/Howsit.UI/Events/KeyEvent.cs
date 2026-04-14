using System;

namespace Howsit.UI.Events;

/// <summary>
/// An event for special key input like ctrl+c, esc, function keys, etc.
/// </summary>
public class KeyEvent : UiEvent {
    public ConsoleKey Key { get; }
    public ConsoleModifiers Modifiers { get; }

    public KeyEvent(ConsoleKey key, ConsoleModifiers modifiers) {
        Key = key;
        Modifiers = modifiers;
    }
}
