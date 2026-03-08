using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;

namespace Howsit.UI;

public class Renderer : IRenderer {
    private enum State {
        StartSpan,
        InSpan
    }

    private Cell[]? _prevBuffer;

    public Renderer() {
        _prevBuffer = null;
    }

    /// <summary>
    /// Compare two buffers and return a collection of the spans of cells that have changed.
    /// </summary>
    /// <param name="buffer"></param>
    /// <param name="prevBuffer"></param>
    /// <param name="width"></param>
    /// <returns></returns>
    /// <exception cref="Exception"></exception>
    public static IEnumerable<CellSpan> Diff(
        ReadOnlySpan<Cell> buffer,
        ReadOnlySpan<Cell> prevBuffer,
        int width
    ) {
        State state = State.StartSpan;
        List<CellSpan> diff = [];
        for (int i = 0; i < buffer.Length; i++) {
            int row = i / width;
            int column = i % width;

            if (column == 0) {
                state = State.StartSpan;
            }

            if (buffer[i] != prevBuffer[i]) {
                // Start new CellSpan if StartSpan, otherwise add to current
                switch (state) {
                    case State.StartSpan:
                        diff.Add(new CellSpan() {
                            Row = row,
                            StartColumn = column,
                            Cells = new List<Cell>() { buffer[i] }
                        });
                        state = State.InSpan;
                        break;
                    case State.InSpan:
                        diff.Last().Cells.Add(buffer[i]);
                        break;
                    default:
                        throw new Exception("Invalid state when parsing screen buffer diff");
                }
            } else {
                // End of diff span
                state = State.StartSpan;
            }
        }

        return diff;
    }

    public void Render(ReadOnlySpan<Cell> buffer, int width, int height) {
        // TODO: validate dimensions of buffer, height, and width
        if (_prevBuffer is null) {
            _prevBuffer = Cell.EmptyCells(width * height);
        }

        

        buffer.CopyTo(_prevBuffer);
    }

    /// <summary>
    /// A span of cells and its starting position.
    /// </summary>
    public record CellSpan {
        public int Row { get; init; }
        public int StartColumn { get; init; }
        public List<Cell> Cells { get; init; } = [];
    }
}
