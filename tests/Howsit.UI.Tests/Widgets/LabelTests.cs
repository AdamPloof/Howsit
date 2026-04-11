using Xunit;
using System;

using Howsit.UI.Widgets;
using Howsit.UI.Layout;
using Howsit.UI.Style;
using Howsit.UI.Drawing;

namespace Howsit.UI.Tests.Widgets;

public class LabelTests {
    [Fact]
    public void LabelNoBorderNoPaddingIsPainted() {
        Label label = new Label(null, "Test Label A");
        label.SetBounds(new Rect(1, 1, 140, 25));
        Cell[] buffer = label.Paint();

        Assert.Equal(3500, buffer.Length);
        Assert.Equal('T', buffer[0].Value);
        Assert.Equal('e', buffer[1].Value);
        Assert.Equal('s', buffer[2].Value);
        Assert.Equal('t', buffer[3].Value);
        Assert.Equal(' ', buffer[4].Value);
        Assert.Equal('L', buffer[5].Value);
        Assert.Equal('a', buffer[6].Value);
        Assert.Equal('b', buffer[7].Value);
        Assert.Equal('e', buffer[8].Value);
        Assert.Equal('l', buffer[9].Value);
        Assert.Equal(' ', buffer[10].Value);
        Assert.Equal('A', buffer[11].Value);
    }

    [Fact]
    public void LabelBorderNoPaddingIsPainted() {
        Label label = new Label(null, "Test Label A") {
            Border = new Border(BorderStyle.Solid)
        };
        label.SetBounds(new Rect(1, 1, 140, 25));
        Cell[] buffer = label.Paint();

        Assert.Equal(3500, buffer.Length);
        Assert.Equal('T', buffer[141].Value);
        Assert.Equal('e', buffer[142].Value);
        Assert.Equal('s', buffer[143].Value);
        Assert.Equal('t', buffer[144].Value);
        Assert.Equal(' ', buffer[145].Value);
        Assert.Equal('L', buffer[146].Value);
        Assert.Equal('a', buffer[147].Value);
        Assert.Equal('b', buffer[148].Value);
        Assert.Equal('e', buffer[149].Value);
        Assert.Equal('l', buffer[150].Value);
        Assert.Equal(' ', buffer[151].Value);
        Assert.Equal('A', buffer[152].Value);
    }
    
    [Fact]
    public void LabelBorderWithPaddingIsPainted() {
        Label label = new Label(null, "Test Label A") {
            Border = new Border(BorderStyle.Solid),
            Padding = new Padding(2)
        };
        label.SetBounds(new Rect(1, 1, 140, 25));
        Cell[] buffer = label.Paint();

        Assert.Equal(3500, buffer.Length);
        Assert.Equal('T', buffer[423].Value);
        Assert.Equal('e', buffer[424].Value);
        Assert.Equal('s', buffer[425].Value);
        Assert.Equal('t', buffer[426].Value);
        Assert.Equal(' ', buffer[427].Value);
        Assert.Equal('L', buffer[428].Value);
        Assert.Equal('a', buffer[429].Value);
        Assert.Equal('b', buffer[430].Value);
        Assert.Equal('e', buffer[431].Value);
        Assert.Equal('l', buffer[432].Value);
        Assert.Equal(' ', buffer[433].Value);
        Assert.Equal('A', buffer[434].Value);
    }
}
