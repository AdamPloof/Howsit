namespace Howsit.UI.Drawing;

/// <summary>
/// The dimensions of an object in the UI
/// </summary>
/// <todo>
/// Validate that dimensions are never less than 0
/// </todo>
public class Size {
    public int Width { get; set; }
    public int Height { get; set; }

    public Size(int width, int height) {
        Width = width;
        Height = height;
    }

    public static Size Empty() {
        return new Size(0, 0);
    }

    public bool IsEmpty() {
        return Width < 1 || Height < 1;
    }
}
