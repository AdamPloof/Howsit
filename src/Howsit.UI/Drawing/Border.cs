using System;

namespace Howsit.UI.Drawing;

/// <summary>
/// Prop for element border styles.
/// </summary>
public enum BorderStyle {
    None,
    Solid,
    Dash,
    Wide,
    Emphasis
}

/// <summary>
/// Struct for the characters used for a border type.
/// </summary>
/// <param name="Horizontal"></param>
/// <param name="Vertical"></param>
/// <param name="TopLeft"></param>
/// <param name="TopRight"></param>
/// <param name="BottomLeft"></param>
/// <param name="BottomRight"></param>
public readonly record struct BorderPalette(
    char Horizontal,
    char Vertical,
    char TopLeft,
    char TopRight,
    char BottomLeft,
    char BottomRight
);

/// <summary>
/// Helper for converting a Border to characters.
/// </summary>
public static class BorderPainter {
    public static readonly BorderPalette None = new(' ', ' ', ' ', ' ', ' ', ' ' );
    public static readonly BorderPalette Solid = new('─', '│', '┌', '┐', '└', '┘' );
    public static readonly BorderPalette Dash = new('-', '|', '+', '+', '+', '+');
    public static readonly BorderPalette Wide = new('═', '║', '╔', '╗', '╚', '╝');
    public static readonly BorderPalette Emphasis = new('━', '┃', '┏', '┓', '┗', '┛');

    /// <summary>
    /// Add a border to a buffer of cells.
    /// </summary>
    /// <param name="border"></param>
    /// <param name="buffer"></param>
    /// <param name="height"></param>
    /// <param name="width"></param>
    /// <returns></returns>
    public static void ApplyBorder(Cell[] buffer, int width, int height, Border border) {
        if (buffer.Length != width * height) {
            throw new Exception("Buffer size does not match dimensions (width * height)");
        }

        // TODO: what about color?
        ApplyHorizontalBorders(buffer, width, height, border);
        ApplyVerticalBorders(buffer, width, height, border);
    }

    /// <summary>
    /// Apply border to the top and bottom sides. If there is a left/right border, the edges will
    /// use corner characters.
    /// </summary>
    /// <param name="buffer"></param>
    /// <param name="width"></param>
    /// <param name="height"></param>
    /// <param name="border"></param>
    public static void ApplyHorizontalBorders(Cell[] buffer, int width, int height, Border border) {
        if (border.Top == BorderStyle.None && border.Bottom == BorderStyle.None) {
            return;
        }

        int lastRowStart = width * (height - 1);
        BorderPalette topPalette = GetBorderPalette(border.Top);
        BorderPalette bottomPalette = GetBorderPalette(border.Bottom);
        for (int x = 0; x < width; x++) {
            if (x == 0 && border.Left != BorderStyle.None) {
                // Has left side border, use corner
                buffer[x] = new Cell(topPalette.TopLeft);
                buffer[lastRowStart + x] = new Cell(bottomPalette.BottomLeft);
            } else if (x == width - 1 && border.Right != BorderStyle.None) {
                // Has right side border, use corner
                buffer[x] = new Cell(topPalette.TopRight);
                buffer[lastRowStart + x] = new Cell(bottomPalette.BottomRight);
            } else {
                buffer[x] = new Cell(topPalette.Horizontal);
                buffer[lastRowStart + x] = new Cell(bottomPalette.Horizontal);
            }
        }
    }
    
    /// <summary>
    /// Apply border to the left and right sides. If there is no top/bottom border the border
    /// will extend all the way to top/bottom.
    /// </summary>
    /// <param name="buffer"></param>
    /// <param name="width"></param>
    /// <param name="height"></param>
    /// <param name="border"></param>
    public static void ApplyVerticalBorders(Cell[] buffer, int width, int height, Border border) {
        if (border.Left == BorderStyle.None && border.Right == BorderStyle.None) {
            return;
        }

        BorderPalette leftPalette = GetBorderPalette(border.Left);
        BorderPalette rightPalette = GetBorderPalette(border.Right);
        for (int y = 0; y < height; y++) {
            if (y == 0 && border.Top != BorderStyle.None) {
                // Has top border, skip the top row
            } else if (y == height - 1 && border.Bottom != BorderStyle.None) {
                // Has bottom border, skip the bottom row
            } else {
                int rowStart = y * width;
                buffer[rowStart] = new Cell(leftPalette.Vertical);
                buffer[rowStart + width - 1] = new Cell(rightPalette.Vertical);
            }
        }

    }
    
    public static BorderPalette GetBorderPalette(BorderStyle style) {
        switch (style) {
            case BorderStyle.Solid:
                return Solid;
            case BorderStyle.Dash:
                return Dash;
            case BorderStyle.Wide:
                return Wide;
            case BorderStyle.Emphasis:
                return Emphasis;
            default:
                return None;
        }
    }
}

/// <summary>
/// Border props for an element.
/// </summary>
/// <remarks>
/// Borders are always a single character wide. If you need additional space between widgets,
/// use padding.
/// </remarks>
public record struct Border {
    /// <summary>
    /// Default constructor, all borders set to None
    /// </summary>
    public Border() {
        Top = BorderStyle.None;
        Bottom = BorderStyle.None;
        Left = BorderStyle.None;
        Right = BorderStyle.None;
    }

    /// <summary>
    /// Set all borders to b
    /// </summary>
    /// <param name="b"></param>
    public Border(BorderStyle b) {
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
    public Border(BorderStyle topBottom, BorderStyle leftRight) {
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
    public Border(BorderStyle top, BorderStyle right, BorderStyle bottom, BorderStyle left) {
        Top = top;
        Bottom = bottom;
        Left = left;
        Right = right;
    }

    public BorderStyle Top { get; set; }
    public BorderStyle Bottom { get; set; }
    public BorderStyle Left { get; set; }
    public BorderStyle Right { get; set; }

    /// <summary>
    /// Returns true if all sides are None
    /// </summary>
    /// <returns></returns>
    public bool IsNone() {
        if (Top != BorderStyle.None) return true;
        if (Bottom != BorderStyle.None) return true;
        if (Left != BorderStyle.None) return true;
        if (Right != BorderStyle.None) return true;

        return false;
    }
}
