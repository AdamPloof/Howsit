using System;
using Howsit.UI.Style;

namespace Howsit.UI;

/// <summary>
/// Helper class for converting text content into a Cell buffer.
/// </summary>
public static class TextBuffer {
    /// <summary>
    /// Convert a string to a screen buffer of Cells. The string is split into
    /// rows on newlines. Any content that overflows the width or height is truncated.
    /// </summary>
    /// <param name="content"></param>
    /// <param name="width"></param>
    /// <param name="height"></param>
    /// <returns></returns>
    public static Cell[] FromString(string content, int width, int height) {
        return FromString(content, width, height, new CellStyle());
    }

    /// <summary>
    /// Overload that takes in a CellStyle that will be used to format the text content
    /// </summary>
    /// <remarks>
    /// Eventually this method should accept some kind of formated text content that allows
    /// for embedding style content within a string. For now, style is all or nothing.
    /// </remarks>
    /// <param name="content"></param>
    /// <param name="width"></param>
    /// <param name="height"></param>
    /// <param name="style"></param>
    /// <returns></returns>
    public static Cell[] FromString(string content, int width, int height, CellStyle style) {
        Cell[] buffer = Cell.EmptyCells(width * height);
        int row = 0;
        int col = 0;
        foreach (char c in content) {
            if (row >= height) {
                break;
            }

            if (c == '\r') {
                // Break on \n, ignore \r in case of \r\n
                continue;
            }

            if (c == '\n') {
                row++;
                col = 0;
                continue;
            }

            if (col >= width) {
                // Wrap text onto the next line if space allows
                row++;
                col = 0;
            }

            if (row >= height) {
                break;
            }

            if (style.IsEmpty()) {
                buffer[col + (row * width)] = new Cell(c);
            } else {
                buffer[col + (row * width)] = new Cell(c, style);
            }

            col++;
        }

        return buffer;
    }
}
