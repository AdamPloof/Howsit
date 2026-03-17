namespace Howsit.UI.Drawing;

/// <summary>
/// Padding prop for a widget. Follows CSS shorthand via constructors
/// for setting top+bottom and left+right.
/// </summary>
public record struct Padding {
    /// <summary>
    /// All padding set to 0
    /// </summary>
    public Padding() {
        Top = 0;
        Bottom = 0;
        Left = 0;
        Right = 0;
    }

    /// <summary>
    /// Set padding for all sides.
    /// </summary>
    /// <param name="p"></param>
    public Padding(int p) {
        Top = p;
        Bottom = p;
        Left = p;
        Right = p;
    }

    /// <summary>
    /// Set top + bottom padding and left + right padding together.
    /// </summary>
    /// <param name="topBottom"></param>
    /// <param name="leftRight"></param>
    public Padding(int topBottom, int leftRight) {
        Top = topBottom;
        Bottom = topBottom;
        Left = leftRight;
        Right = leftRight;
    }

    /// <summary>
    /// Set each side's padding explicitly.
    /// </summary>
    /// <param name="top"></param>
    /// <param name="right"></param>
    /// <param name="bottom"></param>
    /// <param name="left"></param>
    public Padding(int top, int right, int bottom, int left) {
        Top = top;
        Right = right;
        Bottom = bottom;
        Left = left;
    }

    public int Top { get; set; }
    public int Bottom { get; set; }
    public int Left { get; set; }
    public int Right { get; set; }
}
