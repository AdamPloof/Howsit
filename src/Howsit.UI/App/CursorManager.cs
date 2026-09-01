using System;
using System.Drawing;

using Howsit.UI;
using Howsit.UI.Widgets;
using Howsit.UI.Events;
using Howsit.UI.Layout;

namespace Howsit.UI.App;

/// <inheritdoc />
public class CursorManager : ICursorManager {
    private IWidget? _cursorOwner;
    private int _screenWidth;
    private int _screenHeight;

    public CursorManager() {
        _cursorOwner = null;
        _screenWidth = 0;
        _screenHeight = 0;
    }

    /// <inheritdoc />
    public void PlaceCursor() {
        if (_cursorOwner is null) {
            return;
        }

        Point screenPos = CalcScreenPosition(
            _cursorOwner.GetCursor(),
            _cursorOwner,
            _screenWidth,
            _screenHeight
        );

        // TODO: writing should be managed by a driver
        // TODO: use cursor style
        Console.Out.Write(Ansi.MoveCursorTo(screenPos.Y, screenPos.X));
    }

    /// <inheritdoc /> 
    public Point CalcScreenPosition(
        Cursor cursor,
        IWidget cursorOwner,
        int screenWidth,
        int screenHeight
    ) {
        Rect bounds = cursorOwner.BoundingBox;

        return new Point() {
            X = bounds.X + cursor.Position.X,
            Y = bounds.Y + cursor.Position.Y
        };
    }

    /// <inheritdoc />    
    public void HandleFocusChanged(FocusChangedEvent focusChanged) {
        // TODO: the cursor owner isn't necessarily the focused widget.
        // Need to figure out whether a cursor owns the widget.
        // Or maybe the focused widget does always own the cursor, but just
        // sometimes it's not visible (e.g. buttons)
        _cursorOwner = focusChanged.Focused;
    }

    public void HandleResize(ResizeEvent resizeEvent) {
        _screenWidth = resizeEvent.Width;
        _screenHeight = resizeEvent.Height;
    }
}
