namespace Howsit.UI.Layout;

/// <summary>
/// The coordinates of the top left corner and width/height for
/// a rectangle on the screen.
/// </summary>
/// <param name="X">Horizontal coordinate</param>
/// <param name="Y">Vertical coordinate</param>
/// <param name="Width">Width of the rectangle</param>
/// <param name="Height">Height of the rectangle</param>
public readonly record struct Rect(int X, int Y, int Width, int Height);
