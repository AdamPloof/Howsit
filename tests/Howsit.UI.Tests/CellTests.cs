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
        Cell c1 = new Cell('a', new CellStyle() { Format = TextFormat.Bold });
        Cell c2 = new Cell('a', new CellStyle() { Format = TextFormat.Bold | TextFormat.Italic });
        Cell c3 = new Cell(
            'a',
            new CellStyle() {
                Format = TextFormat.Normal,
                FgColor = new Color(255, 0, 0)
            }
        );
        Cell c4 = new Cell(
            'a',
            new CellStyle() {
                Format = TextFormat.Normal,
                FgColor = new Color(255, 0, 0),
                BgColor = new Color(0, 255, 0)
            }
        );

        Assert.False(c1.Style?.IsEmpty());
        Assert.False(c2.Style?.IsEmpty());
        Assert.False(c3.Style?.IsEmpty());
        Assert.False(c4.Style?.IsEmpty());
    }

    [Fact]
    public void CellHasNoStyle() {
        Cell c1 = Cell.Empty();
        Cell c2 = new Cell(' ');
        Cell c3 = new Cell('a');

        Assert.True(c1.Style is null);
        Assert.True(c2.Style is null);
        Assert.True(c3.Style is null);
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
                new Cell('a', new CellStyle() { Format = TextFormat.Italic }),
                new Cell('a', new CellStyle() { Format = TextFormat.Italic })
            },
            new Cell[] {
                new Cell('a', new CellStyle() { Format = TextFormat.Italic | TextFormat.Bold }),
                new Cell('a', new CellStyle() { Format = TextFormat.Italic | TextFormat.Bold })
            },
            new Cell[] {
                new Cell(
                    'a',
                    new CellStyle() {
                        Format = TextFormat.Italic | TextFormat.Bold,
                        FgColor = red
                    }
                ),
                new Cell(
                    'a',
                    new CellStyle() {
                        Format = TextFormat.Italic | TextFormat.Bold,
                        FgColor = red
                    }
                )
            },
            new Cell[] {
                new Cell(
                    'a',
                    new CellStyle() {
                        Format = TextFormat.Italic | TextFormat.Bold,
                        FgColor = red,
                        BgColor = green
                    }
                ),
                new Cell(
                    'a',
                    new CellStyle() {
                        Format = TextFormat.Italic | TextFormat.Bold,
                        FgColor = red,
                        BgColor = green
                    }
                )
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
                new Cell('a', new CellStyle()  {Format = TextFormat.Italic }),
                new Cell('a', new CellStyle()  {Format = TextFormat.Bold })
            },
            new Cell[] {
                new Cell('a', new CellStyle() { Format = TextFormat.Italic | TextFormat.Bold }),
                new Cell('a', new CellStyle() { Format = TextFormat.Italic | TextFormat.Underline })
            },
            new Cell[] {
                new Cell('a', new CellStyle() {
                        Format = TextFormat.Italic | TextFormat.Bold,
                        FgColor = red
                    }
                ),
                new Cell('a', new CellStyle() {
                        Format = TextFormat.Italic | TextFormat.Bold,
                        FgColor = green
                    }
                )
            },
            new Cell[] {
                new Cell('a', new CellStyle() {
                        Format = TextFormat.Italic | TextFormat.Bold,
                        FgColor = red,
                        BgColor = green
                    }
                ),
                new Cell('a', new CellStyle() {
                        Format = TextFormat.Italic | TextFormat.Bold,
                        FgColor = red,
                        BgColor = blue
                    }
                )
            },
        };

        return cells;
    }
}
