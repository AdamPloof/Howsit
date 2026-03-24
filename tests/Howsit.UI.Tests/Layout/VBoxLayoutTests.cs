using Xunit;
using System.Collections.Generic;

using Howsit.UI.Layout;
using Howsit.UI.Widgets;
using Howsit.UI.Drawing;

namespace Howsit.UI.Tests.Layout;

public class VBoxLayoutTests {
    [Fact]
    public void SingleWidgetMinSize() {
        Label label = new Label("Test") { MinSize = new Size(10, 10) };
        List<IWidget> widgets = [];
        widgets.Add(label);
        VBoxLayout layout = new();
        layout.Arrange(widgets, new Rect(0, 0, 20, 20));

        Rect labelBounds = label.BoundingBox;

        Assert.Equal(0, labelBounds.X);
        Assert.Equal(0, labelBounds.Y);
        Assert.Equal(10, labelBounds.Width);
        Assert.Equal(10, labelBounds.Height);
    }

    [Fact]
    public void SingleWidgetPreferredSize() {
        Label label = new Label("Test") {
            MinSize = new Size(10, 10),
            PreferredSize = new Size(15, 15)
        };
        List<IWidget> widgets = [];
        widgets.Add(label);
        VBoxLayout layout = new();
        layout.Arrange(widgets, new Rect(0, 0, 20, 20));

        Rect labelBounds = label.BoundingBox;

        Assert.Equal(0, labelBounds.X);
        Assert.Equal(0, labelBounds.Y);
        Assert.Equal(15, labelBounds.Width);
        Assert.Equal(15, labelBounds.Height);
    }
    
    [Fact]
    public void MultiWidgetMinSize() {
        Label labelA = new Label("Test A") { MinSize = new Size(15, 15) };
        Label labelB = new Label("Test B") { MinSize = new Size(10, 10) };

        List<IWidget> widgets = [];
        widgets.Add(labelA);
        widgets.Add(labelB);

        VBoxLayout layout = new();
        layout.Arrange(widgets, new Rect(0, 0, 40, 40));

        Rect labelBoundsA = labelA.BoundingBox;
        Assert.Equal(0, labelBoundsA.X);
        Assert.Equal(0, labelBoundsA.Y);
        Assert.Equal(15, labelBoundsA.Width);
        Assert.Equal(15, labelBoundsA.Height);

        Rect labelBoundsB = labelB.BoundingBox;
        Assert.Equal(0, labelBoundsB.X);
        Assert.Equal(15, labelBoundsB.Y);
        Assert.Equal(10, labelBoundsB.Width);
        Assert.Equal(10, labelBoundsB.Height);
    }

    [Fact]
    public void MultiWidgetPreferredSize() {
        Label labelA = new Label("Test A") {
            MinSize = new Size(15, 15),
            PreferredSize = new Size(20, 20),
        };
        Label labelB = new Label("Test B") {
            MinSize = new Size(10, 10),
            PreferredSize = new Size(15, 15)
        };

        List<IWidget> widgets = [];
        widgets.Add(labelA);
        widgets.Add(labelB);

        VBoxLayout layout = new();
        layout.Arrange(widgets, new Rect(0, 0, 40, 40));

        Rect labelBoundsA = labelA.BoundingBox;
        Assert.Equal(0, labelBoundsA.X);
        Assert.Equal(0, labelBoundsA.Y);
        Assert.Equal(20, labelBoundsA.Width);
        Assert.Equal(20, labelBoundsA.Height);

        Rect labelBoundsB = labelB.BoundingBox;
        Assert.Equal(0, labelBoundsB.X);
        Assert.Equal(20, labelBoundsB.Y);
        Assert.Equal(15, labelBoundsB.Width);
        Assert.Equal(15, labelBoundsB.Height);
    }

    [Fact]
    public void MultiWidgetPreferredFallbackToMin() {
        Label labelA = new Label("Test A") {
            MinSize = new Size(15, 15),
            PreferredSize = new Size(20, 20),
        };
        Label labelB = new Label("Test B") {
            MinSize = new Size(10, 10),
            PreferredSize = new Size(15, 15)
        };

        List<IWidget> widgets = [];
        widgets.Add(labelA);
        widgets.Add(labelB);

        VBoxLayout layout = new();
        layout.Arrange(widgets, new Rect(0, 0, 30, 30));

        Rect labelBoundsA = labelA.BoundingBox;
        Assert.Equal(0, labelBoundsA.X);
        Assert.Equal(0, labelBoundsA.Y);
        Assert.Equal(20, labelBoundsA.Width);
        Assert.Equal(20, labelBoundsA.Height);

        Rect labelBoundsB = labelB.BoundingBox;
        Assert.Equal(0, labelBoundsB.X);
        Assert.Equal(20, labelBoundsB.Y);
        Assert.Equal(10, labelBoundsB.Width);
        Assert.Equal(10, labelBoundsB.Height);
    }

    [Fact]
    public void SingleWidgetStretchVertical() {
        Label label = new Label("Test") {
            MinSize = new Size(10, 10),
            StretchVertical = 1,
        };
        List<IWidget> widgets = [];
        widgets.Add(label);
        VBoxLayout layout = new();
        layout.Arrange(widgets, new Rect(0, 0, 20, 20));

        Rect labelBounds = label.BoundingBox;

        Assert.Equal(0, labelBounds.X);
        Assert.Equal(0, labelBounds.Y);
        Assert.Equal(10, labelBounds.Width);
        Assert.Equal(20, labelBounds.Height);
    }

    [Fact]
    public void SingleWidgetStretchHorizontal() {
        Label label = new Label("Test") {
            MinSize = new Size(10, 10),
            StretchHorizontal = 1,
        };
        List<IWidget> widgets = [];
        widgets.Add(label);
        VBoxLayout layout = new();
        layout.Arrange(widgets, new Rect(0, 0, 20, 20));

        Rect labelBounds = label.BoundingBox;

        Assert.Equal(0, labelBounds.X);
        Assert.Equal(0, labelBounds.Y);
        Assert.Equal(20, labelBounds.Width);
        Assert.Equal(10, labelBounds.Height);
    }

    [Fact]
    public void SingleWidgetStretchBoth() {
        Label label = new Label("Test") {
            MinSize = new Size(10, 10),
            StretchVertical = 1,
        };
        List<IWidget> widgets = [];
        widgets.Add(label);
        VBoxLayout layout = new();
        layout.Arrange(widgets, new Rect(0, 0, 20, 20));

        Rect labelBounds = label.BoundingBox;

        Assert.Equal(0, labelBounds.X);
        Assert.Equal(0, labelBounds.Y);
        Assert.Equal(20, labelBounds.Width);
        Assert.Equal(20, labelBounds.Height);
    }

    [Fact]
    public void MultiWidgetStretch() {

    }
    
    [Fact]
    public void SingleWidgetHandleOverflow() {

    }

    [Fact]
    public void MultiWidgetHandleOverflow() {

    }

    [Fact]
    public void MultiWidgetAllSizePrefs() {

    }
    
    [Fact]
    public void SingleWidgetNoMinSizeStretches() {

    }

    [Fact]
    public void MultiWidgetNoMinSizeStretch() {

    }

    [Fact]
    public void EmptyBoundsThrows() {

    }

    [Fact]
    public void InvalidBoundsPositionThrows() {

    }

    [Fact]
    public void WidgetWithEmptyMinSizeAnNoStretchThrows() {

    }
}
