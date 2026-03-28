using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;

using Howsit.UI.Exceptions;
using Howsit.UI.Style;

namespace Howsit.UI;

public class Renderer : IRenderer {
    private enum State {
        StartSpan,
        InSpan
    }

    private Cell[]? _prevBuffer;
    private CellStyle? _activeStyle;

    public Renderer() {
        _prevBuffer = null;
        _activeStyle = null;
    }

    /// <summary>
    /// Compare two buffers and return a collection of the spans of cells that have changed.
    /// </summary>
    /// <param name="buffer"></param>
    /// <param name="prevBuffer"></param>
    /// <param name="width"></param>
    /// <returns></returns>
    /// <exception cref="RenderException"></exception>
    public static IEnumerable<CellSpan> Diff(
        ReadOnlySpan<Cell> buffer,
        ReadOnlySpan<Cell> prevBuffer,
        int width
    ) {
        if (buffer.Length == 0) {
            throw new RenderException("Screen buffer cannot be empty");
        }

        if (buffer.Length != prevBuffer.Length) {
            throw new RenderException("Unable to diff buffers. Buffers must be of equal length.");
        }

        if (buffer.Length % width != 0) {
            throw new RenderException("Buffer length must be evenly divisible by screen width");
        }

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
                            Style = buffer[i].Style,
                            Cells = new List<Cell>() { buffer[i] }
                        });
                        state = State.InSpan;
                        break;
                    case State.InSpan:
                        if (buffer[i].Style != diff.Last().Cells.Last().Style) {
                            // Style is different from previous cell, start a new span.
                            diff.Add(new CellSpan() {
                                Row = row,
                                StartColumn = column,
                                Style = buffer[i].Style,
                                Cells = new List<Cell>()
                            });
                        }
                        diff.Last().Cells.Add(buffer[i]);
                        break;
                    default:
                        throw new RenderException("Invalid state when parsing screen buffer diff");
                }
            } else {
                // End of diff span
                state = State.StartSpan;
            }
        }

        return diff;
    }

    /// <inheritdoc />
    public void Render(ReadOnlySpan<Cell> buffer, int width, int height) {
        if (buffer.Length != width * height) {
            throw new RenderException("Buffer length must match screen dimensions (width * height)");
        }

        if (_prevBuffer is not null && _prevBuffer.Length != buffer.Length) {
            // Redraw the screen from scratch if the window was resized.
            DrawScreen(buffer, width, height);
            Console.Out.Flush();

            return;
        }

        if (_prevBuffer is null) {
            _prevBuffer = Cell.EmptyCells(width * height);
        }

        IEnumerable<CellSpan> diff = Diff(buffer, _prevBuffer, width);
        foreach (CellSpan span in diff) {
            DrawSpan(span);
        }

        Console.Out.Flush();
        buffer.CopyTo(_prevBuffer);
    }

    /// <inheritdoc>
    public void Render(string content, int width, int height) {
        Cell[] buffer = TextBuffer.FromString(content, width, height);
        Render(buffer, width, height);
    }

    /// <summary>
    /// Completely re-draw the screen with a new buffer.
    /// </summary>
    /// <remarks>
    /// It's assumed that the buffer.Length == width * height
    /// </remarks>
    /// <param name="buffer"></param>
    /// <param name="width"></param>
    /// <param name="height"></param>
    private void DrawScreen(ReadOnlySpan<Cell> buffer, int width, int height) {
        if (buffer.Length == 0) {
            throw new RenderException("Screen buffer cannot be empty");
        }

        int bufferOffset = 0;
        for (int row = 0; row < height; row++) {
            // Split the row into spans of consecutive styled cells.
            List<CellSpan> lineSpans = [];
            ReadOnlySpan<Cell> line = buffer.Slice(bufferOffset, bufferOffset + width - 1);
            CellStyle? prevStyle = line[0].Style;

            int currentSpanIdx = 0;
            lineSpans.Add(
                new CellSpan() { Row = row, StartColumn = 0, Cells = new List<Cell>(), Style = prevStyle }
            );
            for (int i = 0; i < line.Length; i++) {
                if (line[i].Style == prevStyle) {
                    lineSpans[currentSpanIdx].Cells.Add(line[i]);
                } else {
                    lineSpans.Add(
                        new CellSpan() {
                            Row = row,
                            StartColumn = i,
                            Cells = new List<Cell>() { line[i] },
                            Style = line[i].Style
                        }
                    );
                    prevStyle = line[i].Style;
                    currentSpanIdx++;
                }
            }
            lineSpans.ForEach(DrawSpan);

            bufferOffset += width;
        }
    }

    /// <summary>
    /// Draw a span of cells to the screen. Used to selectively updates
    /// a portion of the screen.
    /// </summary>
    /// <remarks>
    /// The width of a span + its starting column should not exceed the width of the screen.
    /// </remarks>
    /// <param name="span"></param>
    private void DrawSpan(CellSpan span) {
        Console.Out.Write(Ansi.MoveCursorTo(span.Row, span.StartColumn));
        string content;
        if (span.Style is not null) {
            content = Ansi.EscapeSequence(
                (CellStyle)span.Style
            ) + new string(span.Cells.Select(c => c.Value).ToArray());
        } else if (_activeStyle is not null) {
            content = Ansi.Reset + new string(span.Cells.Select(c => c.Value).ToArray());
        } else {
            content = new string(span.Cells.Select(c => c.Value).ToArray());
        }

        _activeStyle = span.Style;

        Console.Out.Write(content);
    }
}
