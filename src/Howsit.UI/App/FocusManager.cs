using Howsit.UI.Widgets;

namespace Howsit.UI.App;

/// <inheritdoc />
public class FocusManager : IFocusManager {
    private IWidget _focused;

    public FocusManager(IWidget root) {
        _focused = root;
    }

    /// <inheritdoc />
    public IWidget GetFocusedWidget() {
        return _focused;
    }

    /// <inheritdoc />
    public bool ChangeFocus(IWidget widget) {
        if (!widget.AcceptsFocus) {
            return false;
        }

        if (!_focused.ClearFocus()) {
            // focused widget refused to give up focus.
            return false;
        }

        return widget.SetFocus();
    }
}
