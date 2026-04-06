using System;
using Howsit.UI.Widgets;

namespace Howsit.UI.Events;

/// <inheritdoc />
public class EventDispatcher : IEventDispatcher {
    /// <summary>
    /// Forward the event to the widget's handler. Broadcast the event downward through
    /// the widget tree unless a handler stops propagation.
    /// </summary>
    /// <param name="target"></param>
    /// <param name="uiEvent"></param>
    public void Dispatch(IWidget target, UiEvent uiEvent) {
        target.HandleEvent(uiEvent);
        if (uiEvent.Handled) {
            return;
        }

        foreach (IWidget child in target.GetChildren()) {
            Dispatch(child, uiEvent);

            if (uiEvent.Handled) {
                return;
            }
        }
    }
}
