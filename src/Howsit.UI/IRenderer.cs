using System;

namespace Howsit.UI;

/// <summary>
/// Renderers are responsible for taking a screen buffer and writing
/// it to the output.
/// </summary>
public interface IRenderer {
    /// <summary>
    /// Write the screen buffer to the output
    /// </summary>
    /// <param name="buffer"></param>
    public void Render(ReadOnlySpan<Cell> buffer, int width, int height);

    /// <summary>
    /// Write a string to output. Rows split on new lines.
    /// Content that overflows the width/height is truncated.
    /// </summary>
    /// <param name="content"></param>
    public void Render(string content, int width, int height);
}
