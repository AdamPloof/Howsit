using Howsit.UI.Widgets;

namespace Howsit.UI.App;

/// <summary>
/// FocusManagers are responsbile for keeping track of the currently focused widget.
/// There should only ever be one focused widget at a time.
/// </summary>
public interface IFocusManager {
    /// <summary>
    /// Returns the currently focused widget. If nothing has been focused on
    /// yet, returns the root widget.
    /// </summary>
    /// <returns></returns>
    public IWidget GetFocusedWidget();

    /// <summary>
    /// Set the focus to the target widget. Returns true if the widget accepted
    /// the focuse change.
    /// </summary>
    /// <param name="widget"></param>
    public bool ChangeFocus(IWidget widget);
}
