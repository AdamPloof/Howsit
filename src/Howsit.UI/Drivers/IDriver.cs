using System.Drawing;

namespace Howsit.UI.Drivers;

/// <summary>
/// Interface for classes responsible for rendering text buffer to the screen
/// </summary>
public interface IDriver {
    /// <summary>
    /// Write the current output buffer to the screen.
    /// </summary>
    void Refresh();

    /// <summary>
    /// Append <paramref name="str"/> to the output buffer.
    /// </summary>
    /// <param name="str"></param>
    void WriteStr(string str);
}
