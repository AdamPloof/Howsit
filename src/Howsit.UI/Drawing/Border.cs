namespace Howsit.UI.Drawing;

/// <summary>
/// Prop for element border styles.
/// </summary>
public enum BorderType {
    None,
    Solid,
    Dash,
    Wide,
    Emphasis
}

/// <summary>
/// Border props for an element.
/// </summary>
public record struct Border {
    /// <summary>
    /// Default constructor, all borders set to None
    /// </summary>
    public Border() {
        Top = BorderType.None;
        Bottom = BorderType.None;
        Left = BorderType.None;
        Right = BorderType.None;
    }

    /// <summary>
    /// Set all borders to b
    /// </summary>
    /// <param name="b"></param>
    public Border(BorderType b) {
        Top = b;
        Bottom = b;
        Left = b;
        Right = b;
    }

    /// <summary>
    /// Set top + bottom and left + right borders together.
    /// </summary>
    /// <param name="topBottom"></param>
    /// <param name="leftRight"></param>
    public Border(BorderType topBottom, BorderType leftRight) {
        Top = topBottom;
        Bottom = topBottom;
        Left = leftRight;
        Right = leftRight;
    }

    /// <summary>
    /// Set top, right, bottom, left borders explicitly.
    /// </summary>
    /// <param name="top"></param>
    /// <param name="right"></param>
    /// <param name="bottom"></param>
    /// <param name="left"></param>
    public Border(BorderType top, BorderType right, BorderType bottom, BorderType left) {
        Top = top;
        Bottom = bottom;
        Left = left;
        Right = right;
    }

    public BorderType Top { get; set; }
    public BorderType Bottom { get; set; }
    public BorderType Left { get; set; }
    public BorderType Right { get; set; }
}
