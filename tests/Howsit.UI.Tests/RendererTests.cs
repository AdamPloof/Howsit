using Xunit;
using System;
using System.Linq;
using System.Collections.Generic;

using Howsit.UI;
using Howsit.UI.Exceptions;

namespace Howsit.UI.Tests;

// TODO: test diffs where cells are empty
public class RendererTests {
    [Fact]
    public void IdenticalBuffersDiffIsEmpty() {
        Cell[] cells = new Cell[9] {
            new Cell('a'), new Cell('b'), new Cell('c'),
            new Cell('d'), new Cell('e'), new Cell('f'),
            new Cell('g'), new Cell('h'), new Cell('i'),
        };
        ReadOnlySpan<Cell> buffer = cells.AsSpan();
        ReadOnlySpan<Cell> prevBuffer = cells.AsSpan();

        IEnumerable<Renderer.CellSpan> diff = Renderer.Diff(buffer, prevBuffer, 3);

        Assert.Empty(diff);
    }

    [Fact]
    public void OppositeBuffersDiffEqualsBufferLength() {
        Cell[] prev = new Cell[9] {
            new Cell('a'), new Cell('b'), new Cell('c'),
            new Cell('d'), new Cell('e'), new Cell('f'),
            new Cell('g'), new Cell('h'), new Cell('i'),
        };
        Cell[] next = new Cell[9] {
            new Cell('A'), new Cell('B'), new Cell('C'),
            new Cell('D'), new Cell('E'), new Cell('F'),
            new Cell('G'), new Cell('H'), new Cell('I'),
        };
        ReadOnlySpan<Cell> prevBuffer = prev.AsSpan();
        ReadOnlySpan<Cell> buffer = next.AsSpan();

        IEnumerable<Renderer.CellSpan> diff = Renderer.Diff(buffer, prevBuffer, 3);

        Assert.Equal(3, diff.Count());
        foreach (Renderer.CellSpan span in diff) {
            Assert.Equal(3, span.Cells.Count());
        }
    }

    [Fact]
    public void PartialBuffersDiffFound() {
        Cell[] prev = new Cell[9] {
            new Cell('a'), new Cell('b'), new Cell('c'),
            new Cell('d'), new Cell('e'), new Cell('f'),
            new Cell('g'), new Cell('h'), new Cell('i'),
        };
        Cell[] next = new Cell[9] {
            new Cell('a'), new Cell('B'), new Cell('c'),
            new Cell('D'), new Cell('E'), new Cell('f'),
            new Cell('g'), new Cell('h'), new Cell('i'),
        };
        ReadOnlySpan<Cell> prevBuffer = prev.AsSpan();
        ReadOnlySpan<Cell> buffer = next.AsSpan();

        IEnumerable<Renderer.CellSpan> diff = Renderer.Diff(buffer, prevBuffer, 3);

        Assert.Equal(2, diff.Count());
        Renderer.CellSpan? firstRow = diff.FirstOrDefault(d => d.Row == 0);
        if (firstRow is null) {
            return;
        }

        Assert.Single(firstRow.Cells);
        Assert.Equal(0, firstRow.Row);
        Assert.Equal(1, firstRow.StartColumn);
        Assert.Equal('B', firstRow.Cells[0].Value);
        Renderer.CellSpan? secondRow = diff.FirstOrDefault(d => d.Row == 1);
        Assert.NotNull(secondRow);
        if (secondRow is null) {
            return;
        }

        Assert.Equal(2, secondRow.Cells.Count);
        Assert.Equal(1, secondRow.Row);
        Assert.Equal(0, secondRow.StartColumn);
        Assert.Equal('D', secondRow.Cells[0].Value);
        Assert.Equal('E', secondRow.Cells[1].Value);
    }

    [Fact]
    public void EmptyCellsOverwrite() {
        Cell[] prev = new Cell[9] {
            new Cell('a'), new Cell('b'), new Cell('c'),
            Cell.Empty(), new Cell('e'), new Cell('f'),
            new Cell('g'), Cell.Empty(), Cell.Empty(),
        };
        Cell[] next = new Cell[9] {
            new Cell('a'), new Cell('b'), new Cell('c'),
            new Cell('e'), new Cell('e'), new Cell('f'),
            new Cell('g'), new Cell('h'), new Cell('i'),
        };
        ReadOnlySpan<Cell> prevBuffer = prev.AsSpan();
        ReadOnlySpan<Cell> buffer = next.AsSpan();

        IEnumerable<Renderer.CellSpan> diff = Renderer.Diff(buffer, prevBuffer, 3);

        Assert.Equal(2, diff.Count());
        Renderer.CellSpan? firstRow = diff.FirstOrDefault(d => d.Row == 0);
        if (firstRow is null) {
            return;
        }

        Assert.Single(firstRow.Cells);
        Assert.Equal(1, firstRow.Row);
        Assert.Equal(0, firstRow.StartColumn);
        Assert.Equal('e', firstRow.Cells[0].Value);
        Renderer.CellSpan? secondRow = diff.FirstOrDefault(d => d.Row == 1);
        Assert.NotNull(secondRow);
        if (secondRow is null) {
            return;
        }

        Assert.Equal(2, secondRow.Cells.Count);
        Assert.Equal(2, secondRow.Row);
        Assert.Equal(1, secondRow.StartColumn);
        Assert.Equal('h', secondRow.Cells[0].Value);
        Assert.Equal('i', secondRow.Cells[1].Value);
    }

    [Fact]
    public void EmptyCellsWritten() {
        Cell[] prev = new Cell[9] {
            new Cell('a'), new Cell('b'), new Cell('c'),
            new Cell('e'), new Cell('e'), new Cell('f'),
            new Cell('g'), new Cell('h'), new Cell('i'),
        };
        Cell[] next = new Cell[9] {
            new Cell('a'), new Cell('b'), new Cell('c'),
            Cell.Empty(), new Cell('e'), new Cell('f'),
            new Cell('g'), Cell.Empty(), Cell.Empty(),
        };
        ReadOnlySpan<Cell> prevBuffer = prev.AsSpan();
        ReadOnlySpan<Cell> buffer = next.AsSpan();

        IEnumerable<Renderer.CellSpan> diff = Renderer.Diff(buffer, prevBuffer, 3);

        Assert.Equal(2, diff.Count());
        Renderer.CellSpan? firstRow = diff.FirstOrDefault(d => d.Row == 0);
        if (firstRow is null) {
            return;
        }

        Assert.Single(firstRow.Cells);
        Assert.Equal(1, firstRow.Row);
        Assert.Equal(0, firstRow.StartColumn);
        Assert.True(firstRow.Cells[0].IsEmpty());
        Renderer.CellSpan? secondRow = diff.FirstOrDefault(d => d.Row == 1);
        Assert.NotNull(secondRow);
        if (secondRow is null) {
            return;
        }

        Assert.Equal(2, secondRow.Cells.Count);
        Assert.Equal(2, secondRow.Row);
        Assert.Equal(1, secondRow.StartColumn);
        Assert.True(secondRow.Cells[0].IsEmpty());
        Assert.True(secondRow.Cells[1].IsEmpty());
    }

    [Fact]
    public void WidthChangeUpDiffFound() {
        Cell[] prev = new Cell[6] {
            new Cell('a'), new Cell('b'),
            new Cell('d'), new Cell('e'),
            new Cell('g'), new Cell('h'),
        };
        Cell[] next = new Cell[9] {
            new Cell('a'), new Cell('B'), new Cell('c'),
            new Cell('D'), new Cell('E'), new Cell('f'),
            new Cell('g'), new Cell('h'), new Cell('i'),
        };
        ReadOnlySpan<Cell> prevBuffer = prev.AsSpan();
        ReadOnlySpan<Cell> buffer = next.AsSpan();

        Assert.Throws<RenderException>(() => Renderer.Diff(next.AsSpan(), prev.AsSpan(), 3));
    }

    [Fact]
    public void WidthChangeDownDiffFound() {
        Cell[] prev = new Cell[9] {
            new Cell('a'), new Cell('B'), new Cell('c'),
            new Cell('D'), new Cell('E'), new Cell('f'),
            new Cell('g'), new Cell('h'), new Cell('i'),
        };
        Cell[] next = new Cell[6] {
            new Cell('a'), new Cell('b'),
            new Cell('d'), new Cell('e'),
            new Cell('g'), new Cell('h'),
        };

        Assert.Throws<RenderException>(() => Renderer.Diff(next.AsSpan(), prev.AsSpan(), 2));
    }

    [Fact]
    public void HeightChangeUpDiffFound() {
        Cell[] prev = new Cell[9] {
            new Cell('a'), new Cell('b'), new Cell('c'),
            new Cell('d'), new Cell('e'), new Cell('f'),
            new Cell('g'), new Cell('h'), new Cell('i'),
        };
        Cell[] next = new Cell[12] {
            new Cell('A'), new Cell('B'), new Cell('c'),
            new Cell('d'), new Cell('E'), new Cell('f'),
            new Cell('g'), new Cell('h'), new Cell('i'),
            new Cell('j'), new Cell('k'), new Cell('l'),
        };

        Assert.Throws<RenderException>(() => Renderer.Diff(next.AsSpan(), prev.AsSpan(), 3));
    }

    [Fact]
    public void HeightChangeDownDiffFound() {
        Cell[] prev = new Cell[12] {
            new Cell('A'), new Cell('B'), new Cell('c'),
            new Cell('d'), new Cell('E'), new Cell('f'),
            new Cell('g'), new Cell('h'), new Cell('i'),
            new Cell('j'), new Cell('k'), new Cell('l'),
        };
        Cell[] next = new Cell[9] {
            new Cell('a'), new Cell('b'), new Cell('c'),
            new Cell('d'), new Cell('e'), new Cell('f'),
            new Cell('g'), new Cell('h'), new Cell('i'),
        };

        Assert.Throws<RenderException>(() => Renderer.Diff(next.AsSpan(), prev.AsSpan(), 3));
    }

    [Fact]
    public void BufferNotDivisibleByWidthThrows() {
        Cell[] prev = new Cell[9] {
            new Cell('A'), new Cell('B'), new Cell('c'),
            new Cell('d'), new Cell('E'), new Cell('f'),
            new Cell('g'), new Cell('h'), new Cell('i'),
        };
        Cell[] next = new Cell[9] {
            new Cell('a'), new Cell('b'), new Cell('c'),
            new Cell('d'), new Cell('e'), new Cell('f'),
            new Cell('g'), new Cell('h'), new Cell('i'),
        };

        Assert.Throws<RenderException>(() => Renderer.Diff(next.AsSpan(), prev.AsSpan(), 5));
    }
}
