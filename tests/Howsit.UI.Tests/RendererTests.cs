using Xunit;
using System;
using System.Linq;
using System.Collections.Generic;

using Howsit.UI;

namespace Howsit.UI.Tests;

public class RendererTests {
    [Fact]
    public void IdenticalBuffersDiffIsEmpty() {
        Cell[] cells = new Cell[9] {
            new Cell('a'),
            new Cell('b'),
            new Cell('c'),
            new Cell('d'),
            new Cell('e'),
            new Cell('f'),
            new Cell('g'),
            new Cell('h'),
            new Cell('i'),
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

        IEnumerable<Renderer.CellSpan> diff = Renderer.Diff(buffer, prevBuffer, 3);

        Assert.Equal(3, diff.Count());
        Renderer.CellSpan? firstRow = diff.FirstOrDefault(d => d.Row == 0);
        if (firstRow is null) {
            return;
        }

        Assert.Equal(2, firstRow.Cells.Count);
        Assert.Equal(0, firstRow.Row);
        Assert.Equal(1, firstRow.StartColumn);
        Assert.Equal('B', firstRow.Cells[0].Value);
        Assert.Equal('c', firstRow.Cells[1].Value);

        Renderer.CellSpan? secondRow = diff.FirstOrDefault(d => d.Row == 1);
        Assert.NotNull(secondRow);
        if (secondRow is null) {
            return;
        }

        Assert.Equal(3, secondRow.Cells.Count);
        Assert.Equal(1, secondRow.Row);
        Assert.Equal(0, secondRow.StartColumn);
        Assert.Equal('D', secondRow.Cells[0].Value);
        Assert.Equal('E', secondRow.Cells[1].Value);
        Assert.Equal('f', secondRow.Cells[2].Value);

        Renderer.CellSpan? thirdRow = diff.FirstOrDefault(d => d.Row == 1);
        Assert.NotNull(thirdRow);
        if (thirdRow is null) {
            return;
        }

        Assert.Single(thirdRow.Cells);
        Assert.Equal(2, thirdRow.Row);
        Assert.Equal(2, thirdRow.StartColumn);
        Assert.Equal('i', thirdRow.Cells[0].Value);
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
        ReadOnlySpan<Cell> prevBuffer = prev.AsSpan();
        ReadOnlySpan<Cell> buffer = next.AsSpan();

        IEnumerable<Renderer.CellSpan> diff = Renderer.Diff(buffer, prevBuffer, 2);

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
        ReadOnlySpan<Cell> prevBuffer = prev.AsSpan();
        ReadOnlySpan<Cell> buffer = next.AsSpan();

        IEnumerable<Renderer.CellSpan> diff = Renderer.Diff(buffer, prevBuffer, 3);

        Assert.Equal(3, diff.Count());
        Renderer.CellSpan? firstRow = diff.FirstOrDefault(d => d.Row == 0);
        if (firstRow is null) {
            return;
        }

        Assert.Equal(2, firstRow.Cells.Count);
        Assert.Equal(0, firstRow.Row);
        Assert.Equal(0, firstRow.StartColumn);
        Assert.Equal('A', firstRow.Cells[0].Value);
        Assert.Equal('B', firstRow.Cells[1].Value);

        Renderer.CellSpan? secondRow = diff.FirstOrDefault(d => d.Row == 1);
        Assert.NotNull(secondRow);
        if (secondRow is null) {
            return;
        }

        Assert.Single(secondRow.Cells);
        Assert.Equal(1, secondRow.Row);
        Assert.Equal(1, secondRow.StartColumn);
        Assert.Equal('E', secondRow.Cells[0].Value);

        Renderer.CellSpan? thirdRow = diff.FirstOrDefault(d => d.Row == 1);
        Assert.NotNull(thirdRow);
        if (thirdRow is null) {
            return;
        }

        Assert.Equal(3, thirdRow.Cells.Count);
        Assert.Equal(3, thirdRow.Row);
        Assert.Equal(0, thirdRow.StartColumn);
        Assert.Equal('j', thirdRow.Cells[0].Value);
        Assert.Equal('k', thirdRow.Cells[1].Value);
        Assert.Equal('l', thirdRow.Cells[2].Value);
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
        ReadOnlySpan<Cell> prevBuffer = prev.AsSpan();
        ReadOnlySpan<Cell> buffer = next.AsSpan();

        IEnumerable<Renderer.CellSpan> diff = Renderer.Diff(buffer, prevBuffer, 3);

        Assert.Equal(2, diff.Count());
        Renderer.CellSpan? firstRow = diff.FirstOrDefault(d => d.Row == 0);
        if (firstRow is null) {
            return;
        }

        Assert.Equal(2, firstRow.Cells.Count);
        Assert.Equal(0, firstRow.Row);
        Assert.Equal(0, firstRow.StartColumn);
        Assert.Equal('a', firstRow.Cells[0].Value);
        Assert.Equal('b', firstRow.Cells[1].Value);

        Renderer.CellSpan? secondRow = diff.FirstOrDefault(d => d.Row == 1);
        Assert.NotNull(secondRow);
        if (secondRow is null) {
            return;
        }

        Assert.Single(secondRow.Cells);
        Assert.Equal(1, secondRow.Row);
        Assert.Equal(1, secondRow.StartColumn);
        Assert.Equal('e', secondRow.Cells[0].Value);
    }
}
