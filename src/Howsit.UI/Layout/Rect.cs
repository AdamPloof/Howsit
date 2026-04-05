namespace Howsit.UI.Layout;

/// <summary>
/// The coordinates of the top left corner and width/height for
/// a rectangle on the screen.
/// </summary>
public class Rect {
    public int X { get; set; }
    public int Y { get; set; }
    public int Width { get; set; }
    public int Height { get; set; }

    /// <summary>
    /// Constructor for an empty Rect. Useful as a default value.
    /// </summary>
    /// <returns></returns>
    public Rect() {
        X = -1;
        Y = -1;
        Width = 0;
        Height = 0;
    }

    /// <summary>
    /// Standard constructor for a non-empty rect
    /// </summary>
    /// <param name="x"></param>
    /// <param name="y"></param>
    /// <param name="width"></param>
    /// <param name="height"></param>
    public Rect(int x, int y, int width, int height) {
        X = x;
        Y = y;
        Width = width;
        Height = height;
    }

    /// <summary>
    /// Returns true if width or height is 0 or if x or y is negative
    /// </summary>
    /// <returns></returns>
    public bool IsEmpty() {
        if (Width == 0 || Height == 0) {
            return true;
        }

        if (X < 0 || Y < 0) {
            return true;
        }

        return false;
    }
}
