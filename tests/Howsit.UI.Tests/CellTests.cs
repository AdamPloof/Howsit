using Xunit;

using Howsit.UI;
using Howsit.UI.Style;

namespace Howsit.UI.Tests;

public class CellTests {
    [Theory]
    [MemberData(nameof(EquivalentData))]
    public void CellsEqualByOperator(Cell[] cells) {
        Assert.True(cells[0] == cells[1]);
    }

    [Theory]
    [MemberData(nameof(NonEquivalentData))]
    public void CellsNotEqualByOperator(Cell[] cells) {
        Assert.True(cells[0] != cells[1]);
    }

    [Theory]
    [MemberData(nameof(EquivalentData))]
    public void CellsEqualByMethod(Cell[] cells) {
        Assert.True(cells[0].Equals(cells[1]));
    }

    [Theory]
    [MemberData(nameof(NonEquivalentData))]
    public void CellsNotEqualByMethod(Cell[] cells) {
        Assert.False(cells[0].Equals(cells[1]));
    }

    [Fact]
    public void CellIsEmpty() {
        Cell empty1 = Cell.Empty();
        Cell empty2 = new Cell(' ');

        Assert.True(empty1.IsEmpty());
        Assert.True(empty2.IsEmpty());
    }

    [Fact]
    public void CellIsNotEmpty() {
        Cell c = new Cell('a');

        Assert.False(c.IsEmpty());
    }

    [Fact]
    public void CellHasStyle() {
        Cell c1 = new Cell('a', TextFormat.Bold);
        Cell c2 = new Cell('a', TextFormat.Bold | TextFormat.Italic);
        Cell c3 = new Cell('a', TextFormat.Normal, new Color(255, 0, 0));
        Cell c4 = new Cell('a', TextFormat.Normal, new Color(255, 0, 0), new Color(0, 255, 0));

        Assert.False(c1.StyleIsEmpty());
        Assert.False(c2.StyleIsEmpty());
        Assert.False(c3.StyleIsEmpty());
        Assert.False(c4.StyleIsEmpty());
    }

    [Fact]
    public void CellHasNoStyle() {
        Cell c1 = Cell.Empty();
        Cell c2 = new Cell(' ');
        Cell c3 = new Cell('a');

        Assert.True(c1.StyleIsEmpty());
        Assert.True(c2.StyleIsEmpty());
        Assert.True(c3.StyleIsEmpty());
    }

    [Fact]
    public void InitializeCellBufferIsValid() {
        Cell[] cells = Cell.EmptyCells(2 * 2);
        foreach (Cell c in cells) {
            Assert.True(c.IsEmpty());
        }
    }

    public static TheoryData<Cell[]> EquivalentData() {
        Color red = new Color(255, 0, 0);
        Color green = new Color(0, 255, 0);
        Color blue = new Color(0, 0, 255);

        TheoryData<Cell[]> cells = new() {
            new Cell[] {
                new Cell('a'),
                new Cell('a')
            },
            new Cell[] {
                new Cell('a', TextFormat.Italic),
                new Cell('a', TextFormat.Italic)
            },
            new Cell[] {
                new Cell('a', TextFormat.Italic | TextFormat.Bold),
                new Cell('a', TextFormat.Italic | TextFormat.Bold)
            },
            new Cell[] {
                new Cell('a', TextFormat.Italic | TextFormat.Bold, red),
                new Cell('a', TextFormat.Italic | TextFormat.Bold, red)
            },
            new Cell[] {
                new Cell('a', TextFormat.Italic | TextFormat.Bold, red, green),
                new Cell('a', TextFormat.Italic | TextFormat.Bold, red, green)
            },
        };

        return cells;
    }

    public static TheoryData<Cell[]> NonEquivalentData() {
        Color red = new Color(255, 0, 0);
        Color green = new Color(0, 255, 0);
        Color blue = new Color(0, 0, 255);

        TheoryData<Cell[]> cells = new() {
            new Cell[] {
                new Cell('a'),
                new Cell('b')
            },
            new Cell[] {
                new Cell('a', TextFormat.Italic),
                new Cell('a', TextFormat.Bold)
            },
            new Cell[] {
                new Cell('a', TextFormat.Italic | TextFormat.Bold),
                new Cell('a', TextFormat.Italic | TextFormat.Underline)
            },
            new Cell[] {
                new Cell('a', TextFormat.Italic | TextFormat.Bold, red),
                new Cell('a', TextFormat.Italic | TextFormat.Bold, green)
            },
            new Cell[] {
                new Cell('a', TextFormat.Italic | TextFormat.Bold, red, green),
                new Cell('a', TextFormat.Italic | TextFormat.Bold, red, blue)
            },
        };

        return cells;
    }
}
