using Xunit;

using Howsit.UI;
using Howsit.UI.Drawing;

namespace Howsit.UI.Tests;

public class BorderTests {
    [Fact]
    public void NoBorderIsDrawn() {
        Cell[] buffer = Cell.EmptyCells(12);
        Border border = new Border();
        BorderPainter.ApplyBorder(buffer, 3, 4, border);

        foreach (Cell c in buffer) {
            Assert.True(c.IsEmpty());
        }
    }

    [Fact]
    public void TopBorderIsDrawn() {
        Cell[] buffer = Cell.EmptyCells(12);
        Border border = new Border() { Top = BorderStyle.Solid };
        BorderPainter.ApplyBorder(buffer, 3, 4, border);
        BorderPalette solid = BorderPainter.GetBorderPalette(BorderStyle.Solid);

        for (int i = 0; i < buffer.Length; i++) {
            if (i < 3) {
                Assert.Equal(solid.Horizontal, buffer[i].Value);
            } else {
                Assert.True(buffer[i].IsEmpty());
            }
        }
    }

    [Fact]
    public void BottomBorderIsDrawn() {
        Cell[] buffer = Cell.EmptyCells(12);
        Border border = new Border() { Bottom = BorderStyle.Solid };
        BorderPainter.ApplyBorder(buffer, 3, 4, border);
        BorderPalette solid = BorderPainter.GetBorderPalette(BorderStyle.Solid);

        for (int i = 0; i < buffer.Length; i++) {
            if (i > 8) {
                Assert.Equal(solid.Horizontal, buffer[i].Value);
            } else {
                Assert.True(buffer[i].IsEmpty());
            }
        }
    }

    [Fact]
    public void LeftBorderIsDrawn() {
        Cell[] buffer = Cell.EmptyCells(12);
        Border border = new Border() { Left = BorderStyle.Solid };
        BorderPainter.ApplyBorder(buffer, 3, 4, border);
        BorderPalette solid = BorderPainter.GetBorderPalette(BorderStyle.Solid);

        for (int i = 0; i < buffer.Length; i++) {
            if (i % 3 == 0) {
                Assert.Equal(solid.Vertical, buffer[i].Value);
            } else {
                Assert.True(buffer[i].IsEmpty());
            }
        }
    }

    [Fact]
    public void RightBorderIsDrawn() {
        Cell[] buffer = Cell.EmptyCells(12);
        Border border = new Border() { Right = BorderStyle.Solid };
        BorderPainter.ApplyBorder(buffer, 3, 4, border);
        BorderPalette solid = BorderPainter.GetBorderPalette(BorderStyle.Solid);

        for (int i = 0; i < buffer.Length; i++) {
            if (i % 3 == 2) {
                Assert.Equal(solid.Vertical, buffer[i].Value);
            } else {
                Assert.True(buffer[i].IsEmpty());
            }
        }
    }

    [Fact]
    public void HorizontalBordersAreDrawn() {
        Cell[] buffer = Cell.EmptyCells(12);
        Border border = new Border() { Top = BorderStyle.Solid, Bottom = BorderStyle.Solid };
        BorderPainter.ApplyBorder(buffer, 3, 4, border);
        BorderPalette solid = BorderPainter.GetBorderPalette(BorderStyle.Solid);

        for (int i = 0; i < buffer.Length; i++) {
            if (i < 3 || i > 8) {
                Assert.Equal(solid.Horizontal, buffer[i].Value);
            } else {
                Assert.True(buffer[i].IsEmpty());
            }
        }
    }

    [Fact]
    public void VerticalBordersAreDrawn() {
        Cell[] buffer = Cell.EmptyCells(12);
        Border border = new Border() { Left = BorderStyle.Solid, Right = BorderStyle.Solid };
        BorderPainter.ApplyBorder(buffer, 3, 4, border);
        BorderPalette solid = BorderPainter.GetBorderPalette(BorderStyle.Solid);

        for (int i = 0; i < buffer.Length; i++) {
            if (i % 3 == 0 || i % 3 == 2) {
                Assert.Equal(solid.Vertical, buffer[i].Value);
            } else {
                Assert.True(buffer[i].IsEmpty());
            }
        }
    }

    [Fact]
    public void AllBordersAreDrawn() {
        Cell[] buffer = Cell.EmptyCells(12);
        Border border = new Border() {
            Top = BorderStyle.Solid,
            Bottom = BorderStyle.Dash,
            Left = BorderStyle.Emphasis,
            Right = BorderStyle.Wide
        };
        BorderPainter.ApplyBorder(buffer, 3, 4, border);
        BorderPalette solid = BorderPainter.GetBorderPalette(BorderStyle.Solid);
        BorderPalette dash = BorderPainter.GetBorderPalette(BorderStyle.Dash);
        BorderPalette emphasis = BorderPainter.GetBorderPalette(BorderStyle.Emphasis);
        BorderPalette wide = BorderPainter.GetBorderPalette(BorderStyle.Wide);

        Assert.Equal(solid.TopLeft, buffer[0].Value);
        Assert.Equal(solid.Horizontal, buffer[1].Value);
        Assert.Equal(solid.TopRight, buffer[2].Value);
        Assert.Equal(emphasis.Vertical, buffer[3].Value);
        Assert.True(buffer[4].IsEmpty());
        Assert.Equal(wide.Vertical, buffer[5].Value);
        Assert.Equal(emphasis.Vertical, buffer[6].Value);
        Assert.True(buffer[7].IsEmpty());
        Assert.Equal(wide.Vertical, buffer[8].Value);
        Assert.Equal(dash.BottomLeft, buffer[9].Value);
        Assert.Equal(dash.Horizontal, buffer[10].Value);
        Assert.Equal(dash.BottomRight, buffer[11].Value);
    }

    [Fact]
    public void TopAndLeftBordersAreDrawn() {
        Cell[] buffer = Cell.EmptyCells(12);
        Border border = new Border() {
            Top = BorderStyle.Solid,
            Left = BorderStyle.Emphasis,
        };
        BorderPainter.ApplyBorder(buffer, 3, 4, border);
        BorderPalette solid = BorderPainter.GetBorderPalette(BorderStyle.Solid);
        BorderPalette emphasis = BorderPainter.GetBorderPalette(BorderStyle.Emphasis);

        Assert.Equal(solid.TopLeft, buffer[0].Value);
        Assert.Equal(solid.Horizontal, buffer[1].Value);
        Assert.Equal(solid.Horizontal, buffer[2].Value);
        Assert.Equal(emphasis.Vertical, buffer[3].Value);
        Assert.True(buffer[4].IsEmpty());
        Assert.True(buffer[5].IsEmpty());
        Assert.Equal(emphasis.Vertical, buffer[6].Value);
        Assert.True(buffer[7].IsEmpty());
        Assert.True(buffer[8].IsEmpty());
        Assert.Equal(emphasis.Vertical, buffer[9].Value);
        Assert.True(buffer[10].IsEmpty());
        Assert.True(buffer[11].IsEmpty());
    }

    [Fact]
    public void BottomAndRightBordersAreDrawn() {
        Cell[] buffer = Cell.EmptyCells(12);
        Border border = new Border() {
            Bottom = BorderStyle.Dash,
            Right = BorderStyle.Wide
        };
        BorderPainter.ApplyBorder(buffer, 3, 4, border);
        BorderPalette dash = BorderPainter.GetBorderPalette(BorderStyle.Dash);
        BorderPalette wide = BorderPainter.GetBorderPalette(BorderStyle.Wide);

        Assert.True(buffer[0].IsEmpty());
        Assert.True(buffer[1].IsEmpty());
        Assert.Equal(wide.Vertical, buffer[2].Value);
        Assert.True(buffer[3].IsEmpty());
        Assert.True(buffer[4].IsEmpty());
        Assert.Equal(wide.Vertical, buffer[5].Value);
        Assert.True(buffer[6].IsEmpty());
        Assert.True(buffer[7].IsEmpty());
        Assert.Equal(wide.Vertical, buffer[8].Value);
        Assert.Equal(dash.Horizontal, buffer[9].Value);
        Assert.Equal(dash.Horizontal, buffer[10].Value);
        Assert.Equal(dash.BottomRight, buffer[11].Value);
    }
}
