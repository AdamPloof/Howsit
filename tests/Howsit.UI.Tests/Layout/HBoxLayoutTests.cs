using Xunit;
using System;
using System.Collections.Generic;

using Howsit.UI.Layout;
using Howsit.UI.Widgets;
using Howsit.UI.Drawing;

namespace Howsit.UI.Tests.Layout;

// TODO: this is a copy of the HBoxLayout tests, update to test width stuff

public class HBoxLayoutTests {
    [Fact]
    public void SingleWidgetNoStretch() {
        Label label = new Label(null, "") { SizeHint = new Size(10, 10) };
        List<IWidget> widgets = [];
        widgets.Add(label);
        HBoxLayout layout = new();
        layout.Arrange(widgets, new Rect(0, 0, 20, 20));

        Rect labelBounds = label.BoundingBox;

        Assert.Equal(0, labelBounds.X);
        Assert.Equal(0, labelBounds.Y);
        Assert.Equal(10, labelBounds.Width);
        Assert.Equal(10, labelBounds.Height);
    }

    [Fact]
    public void MultiWidgetNoStretch() {
        Label labelA = new Label(null, "") { SizeHint = new Size(15, 15) };
        Label labelB = new Label(null, "") { SizeHint = new Size(10, 10) };

        List<IWidget> widgets = [];
        widgets.Add(labelA);
        widgets.Add(labelB);

        HBoxLayout layout = new();
        layout.Arrange(widgets, new Rect(0, 0, 40, 40));

        Rect labelBoundsA = labelA.BoundingBox;
        Assert.Equal(0, labelBoundsA.X);
        Assert.Equal(0, labelBoundsA.Y);
        Assert.Equal(15, labelBoundsA.Width);
        Assert.Equal(15, labelBoundsA.Height);

        Rect labelBoundsB = labelB.BoundingBox;
        Assert.Equal(15, labelBoundsB.X);
        Assert.Equal(0, labelBoundsB.Y);
        Assert.Equal(10, labelBoundsB.Width);
        Assert.Equal(10, labelBoundsB.Height);
    }

    [Fact]
    public void SingleWidgetStretchVertical() {
        Label label = new Label(null, "") {
            SizeHint = new Size(10, 10),
            StretchVertical = 1,
        };
        List<IWidget> widgets = [];
        widgets.Add(label);
        HBoxLayout layout = new();
        layout.Arrange(widgets, new Rect(0, 0, 20, 20));

        Rect labelBounds = label.BoundingBox;

        Assert.Equal(0, labelBounds.X);
        Assert.Equal(0, labelBounds.Y);
        Assert.Equal(10, labelBounds.Width);
        Assert.Equal(20, labelBounds.Height);
    }

    [Fact]
    public void SingleWidgetStretchHorizontal() {
        Label label = new Label(null, "") {
            SizeHint = new Size(10, 10),
            StretchHorizontal = 1,
        };
        List<IWidget> widgets = [];
        widgets.Add(label);
        HBoxLayout layout = new();
        layout.Arrange(widgets, new Rect(0, 0, 20, 20));

        Rect labelBounds = label.BoundingBox;

        Assert.Equal(0, labelBounds.X);
        Assert.Equal(0, labelBounds.Y);
        Assert.Equal(20, labelBounds.Width);
        Assert.Equal(10, labelBounds.Height);
    }

    [Fact]
    public void SingleWidgetStretchAll() {
        Label label = new Label(null, "") {
            SizeHint = new Size(10, 10),
            StretchVertical = 1,
            StretchHorizontal = 1,
        };
        List<IWidget> widgets = [];
        widgets.Add(label);
        HBoxLayout layout = new();
        layout.Arrange(widgets, new Rect(0, 0, 20, 20));

        Rect labelBounds = label.BoundingBox;

        Assert.Equal(0, labelBounds.X);
        Assert.Equal(0, labelBounds.Y);
        Assert.Equal(20, labelBounds.Width);
        Assert.Equal(20, labelBounds.Height);
    }

    [Fact]
    public void MultiWidgetStretchVertical() {
        Label labelA = new Label(null, "") {
            SizeHint = new Size(10, 10),
            StretchVertical = 1,
            StretchHorizontal = 1,
        };
        Label labelB = new Label(null, "") {
            SizeHint = new Size(15, 15),
            StretchVertical = 1,
            StretchHorizontal = 1,
        };
        List<IWidget> widgets = [];
        widgets.Add(labelA);
        widgets.Add(labelB);
        HBoxLayout layout = new();
        layout.Arrange(widgets, new Rect(0, 0, 40, 40));

        Rect labelABounds = labelA.BoundingBox;
        Rect labelBBounds = labelB.BoundingBox;

        Assert.Equal(0, labelABounds.X);
        Assert.Equal(0, labelABounds.Y);
        Assert.Equal(18, labelABounds.Width);
        Assert.Equal(40, labelABounds.Height);

        Assert.Equal(18, labelBBounds.X);
        Assert.Equal(0, labelBBounds.Y);
        Assert.Equal(22, labelBBounds.Width);
        Assert.Equal(40, labelBBounds.Height);
    }

    [Fact]
    public void MultiWidgetStretchAll() {
        Label labelA = new Label(null, "") {
            SizeHint = new Size(10, 10),
            StretchVertical = 1,
            StretchHorizontal = 1,
        };
        Label labelB = new Label(null, "") {
            SizeHint = new Size(15, 15),
            StretchVertical = 1,
            StretchHorizontal = 1,
        };
        List<IWidget> widgets = [];
        widgets.Add(labelA);
        widgets.Add(labelB);
        HBoxLayout layout = new();
        layout.Arrange(widgets, new Rect(0, 0, 40, 40));

        Rect labelABounds = labelA.BoundingBox;
        Rect labelBBounds = labelB.BoundingBox;

        Assert.Equal(0, labelABounds.X);
        Assert.Equal(0, labelABounds.Y);
        Assert.Equal(18, labelABounds.Width);
        Assert.Equal(40, labelABounds.Height);

        Assert.Equal(18, labelBBounds.X);
        Assert.Equal(0, labelBBounds.Y);
        Assert.Equal(22, labelBBounds.Width);
        Assert.Equal(40, labelBBounds.Height);
    }

    [Fact]
    public void MultiWidgetStretchProportional() {
        Label labelA = new Label(null, "") {
            SizeHint = new Size(10, 10),
            StretchVertical = 1,
            StretchHorizontal = 1,
        };
        Label labelB = new Label(null, "") {
            SizeHint = new Size(10, 10),
            StretchVertical = 1,
            StretchHorizontal = 2,
        };
        List<IWidget> widgets = [];
        widgets.Add(labelA);
        widgets.Add(labelB);
        HBoxLayout layout = new();
        layout.Arrange(widgets, new Rect(0, 0, 40, 40));

        Rect labelABounds = labelA.BoundingBox;
        Rect labelBBounds = labelB.BoundingBox;

        Assert.Equal(0, labelABounds.X);
        Assert.Equal(0, labelABounds.Y);
        Assert.Equal(17, labelABounds.Width);
        Assert.Equal(40, labelABounds.Height);

        Assert.Equal(17, labelBBounds.X);
        Assert.Equal(0, labelBBounds.Y);
        Assert.Equal(23, labelBBounds.Width);
        Assert.Equal(40, labelBBounds.Height);
    }

    [Fact]
    public void SingleWidgetHandleOverflow() {
        Label label = new Label(null, "") { SizeHint = new Size(30, 30) };
        List<IWidget> widgets = [];
        widgets.Add(label);
        HBoxLayout layout = new();
        layout.Arrange(widgets, new Rect(0, 0, 20, 20));

        Rect labelBounds = label.BoundingBox;

        Assert.Equal(0, labelBounds.X);
        Assert.Equal(0, labelBounds.Y);
        Assert.Equal(20, labelBounds.Width);
        Assert.Equal(20, labelBounds.Height);
    }

    [Fact]
    public void SingleWidgetHandleOverflowDoesNotStretch() {
        Label label = new Label(null, "") {
            SizeHint = new Size(30, 30),
            StretchHorizontal = 1,
            StretchVertical = 1,
        };
        List<IWidget> widgets = [];
        widgets.Add(label);
        HBoxLayout layout = new();
        layout.Arrange(widgets, new Rect(0, 0, 20, 20));

        Rect labelBounds = label.BoundingBox;

        Assert.Equal(0, labelBounds.X);
        Assert.Equal(0, labelBounds.Y);
        Assert.Equal(20, labelBounds.Width);
        Assert.Equal(20, labelBounds.Height);
    }

    [Fact]
    public void MultiWidgetHandleOverflow() {
        Label labelA = new Label(null, "") { SizeHint = new Size(15, 15) };
        Label labelB = new Label(null, "") { SizeHint = new Size(10, 10) };

        List<IWidget> widgets = [];
        widgets.Add(labelA);
        widgets.Add(labelB);

        HBoxLayout layout = new();
        layout.Arrange(widgets, new Rect(0, 0, 20, 20));

        Rect labelBoundsA = labelA.BoundingBox;
        Assert.Equal(0, labelBoundsA.X);
        Assert.Equal(0, labelBoundsA.Y);
        Assert.Equal(15, labelBoundsA.Width);
        Assert.Equal(15, labelBoundsA.Height);

        Rect labelBoundsB = labelB.BoundingBox;
        Assert.Equal(15, labelBoundsB.X);
        Assert.Equal(0, labelBoundsB.Y);
        Assert.Equal(5, labelBoundsB.Width);
        Assert.Equal(10, labelBoundsB.Height);
    }

    [Fact]
    public void SingleWidgetNoMinSizeStretches() {
        Label label = new Label(null, "") { StretchHorizontal = 1, StretchVertical = 1 };
        List<IWidget> widgets = [];
        widgets.Add(label);
        HBoxLayout layout = new();
        layout.Arrange(widgets, new Rect(0, 0, 20, 20));

        Rect labelBounds = label.BoundingBox;

        Assert.Equal(0, labelBounds.X);
        Assert.Equal(0, labelBounds.Y);
        Assert.Equal(20, labelBounds.Width);
        Assert.Equal(20, labelBounds.Height);
    }

    [Fact]
    public void MultiWidgetNoMinSizeStretch() {
        Label labelA = new Label(null, "") { StretchHorizontal = 1, StretchVertical = 1 };
        Label labelB = new Label(null, "") { StretchHorizontal = 2, StretchVertical = 2 };
        List<IWidget> widgets = [];
        widgets.Add(labelA);
        widgets.Add(labelB);

        HBoxLayout layout = new();
        layout.Arrange(widgets, new Rect(0, 0, 30, 30));

        Rect labelABounds = labelA.BoundingBox;
        Rect labelBBounds = labelB.BoundingBox;

        Assert.Equal(0, labelABounds.X);
        Assert.Equal(0, labelABounds.Y);
        Assert.Equal(10, labelABounds.Width);
        Assert.Equal(30, labelABounds.Height);

        Assert.Equal(10, labelBBounds.X);
        Assert.Equal(0, labelBBounds.Y);
        Assert.Equal(20, labelBBounds.Width);
        Assert.Equal(30, labelBBounds.Height);
    }

    [Fact]
    public void ManyWidgetsStackedCorrectly() {
        Label labelA = new Label(null, "") { SizeHint = new Size(20, 20) };
        Label labelB = new Label(null, "") { SizeHint = new Size(15, 15) };
        Label labelC = new Label(null, "") { SizeHint = new Size(10, 10) };
        Label labelD = new Label(null, "") { SizeHint = new Size(5, 5) };

        List<IWidget> widgets = [];
        widgets.Add(labelA);
        widgets.Add(labelB);
        widgets.Add(labelC);
        widgets.Add(labelD);

        HBoxLayout layout = new();
        layout.Arrange(widgets, new Rect(0, 0, 100, 100));

        Rect labelBoundsA = labelA.BoundingBox;
        Assert.Equal(0, labelBoundsA.X);
        Assert.Equal(0, labelBoundsA.Y);
        Assert.Equal(20, labelBoundsA.Width);
        Assert.Equal(20, labelBoundsA.Height);

        Rect labelBoundsB = labelB.BoundingBox;
        Assert.Equal(20, labelBoundsB.X);
        Assert.Equal(0, labelBoundsB.Y);
        Assert.Equal(15, labelBoundsB.Width);
        Assert.Equal(15, labelBoundsB.Height);

        Rect labelBoundsC = labelC.BoundingBox;
        Assert.Equal(35, labelBoundsC.X);
        Assert.Equal(0, labelBoundsC.Y);
        Assert.Equal(10, labelBoundsC.Width);
        Assert.Equal(10, labelBoundsC.Height);

        Rect labelBoundsD = labelD.BoundingBox;
        Assert.Equal(45, labelBoundsD.X);
        Assert.Equal(0, labelBoundsD.Y);
        Assert.Equal(5, labelBoundsD.Width);
        Assert.Equal(5, labelBoundsD.Height);
    }

    [Fact]
    public void EmptyBoundsThrows() {
        Label label = new Label(null, "") { SizeHint = new Size(10, 10) };
        List<IWidget> widgets = [];
        widgets.Add(label);
        HBoxLayout layout = new();

        Assert.Throws<ArgumentException>(() => layout.Arrange(widgets, new Rect()));
    }

    [Fact]
    public void InvalidBoundsPositionThrows() {
        Label label = new Label(null, "") { SizeHint = new Size(10, 10) };
        List<IWidget> widgets = [];
        widgets.Add(label);
        HBoxLayout layout = new();

        Assert.Throws<ArgumentException>(() => layout.Arrange(widgets, new Rect(5, 5, 40, 40)));
    }

    [Fact]
    public void WidgetWithEmptySizeHintNoStretchThrows() {
        Label label = new Label(null, "");
        List<IWidget> widgets = [];
        widgets.Add(label);
        HBoxLayout layout = new();

        Assert.Throws<Exception>(() => layout.Arrange(widgets, new Rect(0, 0, 40, 40)));
    }
}
