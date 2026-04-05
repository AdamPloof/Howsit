using Xunit;
using Howsit.UI.Widgets;
using Howsit.UI.Layout;
using System.Collections.Generic;
using System;

namespace Howsit.UI.Tests.Widgets;

public class ContainerTests {
    [Fact]
    public void SingleWidgetPlacedTopLeft() {
        Container c = new Container(null, new MockLayout());
        c.BoundingBox = new Rect(0, 0, 4, 3);

        Label w = new Label(c, new string('a', 9));
        w.BoundingBox = new Rect(0, 0, 3, 3);

        Cell[] buffer = c.Paint();

        Assert.Equal('a', buffer[0].Value);
        Assert.Equal('a', buffer[1].Value);
        Assert.Equal('a', buffer[2].Value);
        Assert.True(buffer[3].IsEmpty());

        Assert.Equal('a', buffer[4].Value);
        Assert.Equal('a', buffer[5].Value);
        Assert.Equal('a', buffer[6].Value);
        Assert.True(buffer[7].IsEmpty());

        Assert.Equal('a', buffer[8].Value);
        Assert.Equal('a', buffer[9].Value);
        Assert.Equal('a', buffer[10].Value);
        Assert.True(buffer[11].IsEmpty());
    }

    [Fact]
    public void SingleWidgetPlacedTopCenter() {
        Container c = new Container(null, new MockLayout());
        c.BoundingBox = new Rect(0, 0, 4, 3);

        Label w = new Label(c, new string('a', 4));
        w.BoundingBox = new Rect(1, 0, 2, 2);

        Cell[] buffer = c.Paint();

        Assert.True(buffer[0].IsEmpty());
        Assert.Equal('a', buffer[1].Value);
        Assert.Equal('a', buffer[2].Value);
        Assert.True(buffer[3].IsEmpty());

        Assert.True(buffer[4].IsEmpty());
        Assert.Equal('a', buffer[5].Value);
        Assert.Equal('a', buffer[6].Value);
        Assert.True(buffer[7].IsEmpty());

        Assert.True(buffer[8].IsEmpty());
        Assert.True(buffer[9].IsEmpty());
        Assert.True(buffer[10].IsEmpty());
        Assert.True(buffer[11].IsEmpty());
    }

    [Fact]
    public void SingleWidgetPlacedTopRight() {
        Container c = new Container(null, new MockLayout());
        c.BoundingBox = new Rect(0, 0, 4, 3);

        Label w = new Label(c, new string('a', 4));
        w.BoundingBox = new Rect(2, 0, 2, 2);

        Cell[] buffer = c.Paint();

        Assert.True(buffer[0].IsEmpty());
        Assert.True(buffer[1].IsEmpty());
        Assert.Equal('a', buffer[2].Value);
        Assert.Equal('a', buffer[3].Value);

        Assert.True(buffer[4].IsEmpty());
        Assert.True(buffer[5].IsEmpty());
        Assert.Equal('a', buffer[6].Value);
        Assert.Equal('a', buffer[7].Value);

        Assert.True(buffer[8].IsEmpty());
        Assert.True(buffer[9].IsEmpty());
        Assert.True(buffer[10].IsEmpty());
        Assert.True(buffer[11].IsEmpty());
    }

    [Fact]
    public void SingleWidgetPlacedCenter() {
        Container c = new Container(null, new MockLayout());
        c.BoundingBox = new Rect(0, 0, 4, 4);

        Label w = new Label(c, new string('a', 4));
        w.BoundingBox = new Rect(1, 1, 2, 2);

        Cell[] buffer = c.Paint();

        Assert.True(buffer[0].IsEmpty());
        Assert.True(buffer[1].IsEmpty());
        Assert.True(buffer[2].IsEmpty());
        Assert.True(buffer[3].IsEmpty());

        Assert.True(buffer[4].IsEmpty());
        Assert.Equal('a', buffer[5].Value);
        Assert.Equal('a', buffer[6].Value);
        Assert.True(buffer[7].IsEmpty());

        Assert.True(buffer[8].IsEmpty());
        Assert.Equal('a', buffer[9].Value);
        Assert.Equal('a', buffer[10].Value);
        Assert.True(buffer[11].IsEmpty());

        Assert.True(buffer[12].IsEmpty());
        Assert.True(buffer[13].IsEmpty());
        Assert.True(buffer[14].IsEmpty());
        Assert.True(buffer[15].IsEmpty());
    }

    [Fact]
    public void SingleWidgetPlacedBottomCenter() {
        Container c = new Container(null, new MockLayout());
        c.BoundingBox = new Rect(0, 0, 4, 3);

        Label w = new Label(c, new string('a', 4));
        w.BoundingBox = new Rect(1, 1, 2, 2);

        Cell[] buffer = c.Paint();

        Assert.True(buffer[0].IsEmpty());
        Assert.True(buffer[1].IsEmpty());
        Assert.True(buffer[2].IsEmpty());
        Assert.True(buffer[3].IsEmpty());

        Assert.True(buffer[4].IsEmpty());
        Assert.Equal('a', buffer[5].Value);
        Assert.Equal('a', buffer[6].Value);
        Assert.True(buffer[7].IsEmpty());

        Assert.True(buffer[8].IsEmpty());
        Assert.Equal('a', buffer[9].Value);
        Assert.Equal('a', buffer[10].Value);
        Assert.True(buffer[11].IsEmpty());
    }

    [Fact]
    public void VerticalAlignWidgetsPlaced() {
        Container c = new Container(null, new MockLayout());
        c.BoundingBox = new Rect(0, 0, 4, 4);

        Label wA = new Label(c, new string('a', 4));
        wA.BoundingBox = new Rect(0, 0, 2, 2);
        Label wB = new Label(c, new string('b', 4));
        wB.BoundingBox = new Rect(0, 2, 2, 2);
        Cell[] buffer = c.Paint();

        Assert.Equal('a', buffer[0].Value);
        Assert.Equal('a', buffer[1].Value);
        Assert.True(buffer[2].IsEmpty());
        Assert.True(buffer[3].IsEmpty());

        Assert.Equal('a', buffer[4].Value);
        Assert.Equal('a', buffer[5].Value);
        Assert.True(buffer[6].IsEmpty());
        Assert.True(buffer[7].IsEmpty());

        Assert.Equal('b', buffer[8].Value);
        Assert.Equal('b', buffer[9].Value);
        Assert.True(buffer[10].IsEmpty());
        Assert.True(buffer[11].IsEmpty());

        Assert.Equal('b', buffer[12].Value);
        Assert.Equal('b', buffer[13].Value);
        Assert.True(buffer[14].IsEmpty());
        Assert.True(buffer[15].IsEmpty());
    }

    [Fact]
    public void HorizontalAlignWidgetsPlaced() {
        Container c = new Container(null, new MockLayout());
        c.BoundingBox = new Rect(0, 0, 4, 3);

        Label wA = new Label(c, new string('a', 4));
        wA.BoundingBox = new Rect(0, 0, 2, 2);
        Label wB = new Label(c, new string('b', 4));
        wB.BoundingBox = new Rect(2, 0, 2, 2);
        Cell[] buffer = c.Paint();

        Assert.Equal('a', buffer[0].Value);
        Assert.Equal('a', buffer[1].Value);
        Assert.Equal('b', buffer[2].Value);
        Assert.Equal('b', buffer[3].Value);

        Assert.Equal('a', buffer[4].Value);
        Assert.Equal('a', buffer[5].Value);
        Assert.Equal('b', buffer[6].Value);
        Assert.Equal('b', buffer[7].Value);

        Assert.True(buffer[8].IsEmpty());
        Assert.True(buffer[9].IsEmpty());
        Assert.True(buffer[10].IsEmpty());
        Assert.True(buffer[11].IsEmpty());
    }

    [Fact]
    public void DiagonalOffsetWidgetsPlaced() {
        Container c = new Container(null, new MockLayout());
        c.BoundingBox = new Rect(0, 0, 4, 4);

        Label wA = new Label(c, new string('a', 4));
        wA.BoundingBox = new Rect(0, 0, 2, 2);
        Label wB = new Label(c, new string('b', 4));
        wB.BoundingBox = new Rect(2, 2, 2, 2);
        Cell[] buffer = c.Paint();

        Assert.Equal('a', buffer[0].Value);
        Assert.Equal('a', buffer[1].Value);
        Assert.True(buffer[2].IsEmpty());
        Assert.True(buffer[3].IsEmpty());

        Assert.Equal('a', buffer[4].Value);
        Assert.Equal('a', buffer[5].Value);
        Assert.True(buffer[6].IsEmpty());
        Assert.True(buffer[7].IsEmpty());

        Assert.True(buffer[8].IsEmpty());
        Assert.True(buffer[9].IsEmpty());
        Assert.Equal('b', buffer[10].Value);
        Assert.Equal('b', buffer[11].Value);

        Assert.True(buffer[12].IsEmpty());
        Assert.True(buffer[13].IsEmpty());
        Assert.Equal('b', buffer[14].Value);
        Assert.Equal('b', buffer[15].Value);
    }

    [Fact]
    public void GridAlignWidgetsPlaced() {
        Container c = new Container(null, new MockLayout());
        c.BoundingBox = new Rect(0, 0, 4, 4);

        Label wA = new Label(c, new string('a', 4));
        wA.BoundingBox = new Rect(0, 0, 2, 2);
        Label wB = new Label(c, new string('b', 4));
        wB.BoundingBox = new Rect(2, 0, 2, 2);
        Label wC = new Label(c, new string('c', 8));
        wC.BoundingBox = new Rect(0, 2, 4, 2);
        Cell[] buffer = c.Paint();

        Assert.Equal('a', buffer[0].Value);
        Assert.Equal('a', buffer[1].Value);
        Assert.Equal('b', buffer[2].Value);
        Assert.Equal('b', buffer[3].Value);

        Assert.Equal('a', buffer[4].Value);
        Assert.Equal('a', buffer[5].Value);
        Assert.Equal('b', buffer[6].Value);
        Assert.Equal('b', buffer[7].Value);

        Assert.Equal('c', buffer[8].Value);
        Assert.Equal('c', buffer[9].Value);
        Assert.Equal('c', buffer[10].Value);
        Assert.Equal('c', buffer[11].Value);

        Assert.Equal('c', buffer[12].Value);
        Assert.Equal('c', buffer[13].Value);
        Assert.Equal('c', buffer[14].Value);
        Assert.Equal('c', buffer[15].Value);
    }

    [Fact]
    public void GridOffsetWidgetsPlaced() {
        Container c = new Container(null, new MockLayout());
        c.BoundingBox = new Rect(0, 0, 4, 4);

        Label wA = new Label(c, new string('a', 4));
        wA.BoundingBox = new Rect(0, 0, 2, 2);
        Label wB = new Label(c, new string('b', 4));
        wB.BoundingBox = new Rect(2, 0, 2, 2);
        Label wC = new Label(c, new string('c', 4));
        wC.BoundingBox = new Rect(2, 2, 2, 2);
        Cell[] buffer = c.Paint();

        Assert.Equal('a', buffer[0].Value);
        Assert.Equal('a', buffer[1].Value);
        Assert.Equal('b', buffer[2].Value);
        Assert.Equal('b', buffer[3].Value);

        Assert.Equal('a', buffer[4].Value);
        Assert.Equal('a', buffer[5].Value);
        Assert.Equal('b', buffer[6].Value);
        Assert.Equal('b', buffer[7].Value);

        Assert.True(buffer[8].IsEmpty());
        Assert.True(buffer[9].IsEmpty());
        Assert.Equal('c', buffer[10].Value);
        Assert.Equal('c', buffer[11].Value);

        Assert.True(buffer[12].IsEmpty());
        Assert.True(buffer[13].IsEmpty());
        Assert.Equal('c', buffer[14].Value);
        Assert.Equal('c', buffer[15].Value);
    }
    
    [Fact]
    public void EmptyDimensionsThrows() {
        Container c = new Container(null, new MockLayout());
        Label w = new Label(c, new string('a', 9));
        w.BoundingBox = new Rect(0, 0, 3, 3);

        Assert.Throws<Exception>(() => c.Paint());
    }

    [Fact]
    public void WidgetPositionOverflowsBufferThrows() {
        Container c = new Container(null, new MockLayout());
        c.BoundingBox = new Rect(0, 0, 4, 3);
        Label w = new Label(c, new string('a', 9));
        w.BoundingBox = new Rect(3, 0, 3, 3);

        Assert.Throws<Exception>(() => c.Paint());
    }

    public class MockLayout : ILayout {
        public void Arrange(IReadOnlyList<IWidget> widgets, Rect bounds) {}
    }
}
