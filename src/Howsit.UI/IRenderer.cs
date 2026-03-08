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
}
