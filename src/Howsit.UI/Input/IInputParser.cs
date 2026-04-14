using System;
using System.Collections.Generic;
using Howsit.UI.Events;

namespace Howsit.UI.Input;

/// <summary>
/// Input parsers are responsbile for reading input from the user and passing them to
/// the application via events.
/// </summary>
public interface IInputParser {
    /// <summary>
    /// Read input from Console and dispatch control/text input events
    /// </summary>
    public IEnumerable<UiEvent> ReadAvailable();

    /// <summary>
    /// Returns true if the input should be parsed as text.
    /// </summary>
    /// <returns></returns>
    public bool IsTextInput(ConsoleKeyInfo key);
}
