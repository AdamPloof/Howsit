using System;
using System.Collections.Generic;

using Howsit.UI.Events;

namespace Howsit.UI.Input;

/// <inheritdoc />
public class InputParser : IInputParser {
    /// <inheritdoc />
    public IEnumerable<UiEvent> ReadAvailable() {
        List<UiEvent> inputEvents = [];
        while (Console.KeyAvailable) {
            ConsoleKeyInfo key = Console.ReadKey(intercept: true);
            UiEvent ev;
            if (IsTextInput(key)) {
                // TODO: dispatch events
                ev = new TextInputEvent($"{key.KeyChar}");
            } else {
                ev = new KeyEvent(key.Key, key.Modifiers);
            }

            inputEvents.Add(ev);
        }

        return inputEvents;
    }

    /// <inheritdoc />
    public bool IsTextInput(ConsoleKeyInfo key) {
        char c = key.KeyChar;
        return c != '\0' && !char.IsControl(c);
    }
}
