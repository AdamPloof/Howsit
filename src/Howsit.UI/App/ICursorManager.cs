using System.Drawing;

using Howsit.UI.Events;
using Howsit.UI.Widgets;

namespace Howsit.UI.App;

/// <summary>
/// CursorManager is responsible for keeping track of which widget owns the cursor
/// and its screen position. 
/// </summary>
public interface ICursorManager {
    /// <summary>
    /// Draws the cursor on the screen if it's visible. The cursor position is determined by
    /// the widget that currently owns the cursor.
    /// </summary>
    public void PlaceCursor();

    /// <summary>
    /// Calculate the absolute position of a cursor on the screen using the relative
    /// position within a widget and the screen dimensions.
    /// </summary>
    /// <param name="cursor"></param>
    /// <param name="cursorOwner"></param>
    /// <param name="screenWidth"></param>
    /// <param name="screenHeight"></param>
    /// <returns></returns>
    public Point CalcScreenPosition(
        Cursor cursor,
        IWidget cursorOwner,
        int screenWidth,
        int screenHeight
    );

    /// <summary>
    /// Keeps track of the currently focused widget in order to calculate
    /// the screen position and style of the cursor.
    /// </summary>
    /// <param name="focusChanged"></param>
    public void HandleFocusChanged(FocusChangedEvent focusChanged);

    /// <summary>
    /// Keep track of current screen dimensions in order to calculate the
    /// absolute cursor position on the screen.
    /// </summary>
    /// <param name="resizeEvent"></param>
    public void HandleResize(ResizeEvent resizeEvent);
}
