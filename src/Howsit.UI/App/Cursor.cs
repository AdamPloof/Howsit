using System.Drawing;

namespace Howsit.UI.App;

/// <summary>
/// The position and style of the cursor within a widget (relative) or screen (absolute).
/// </summary>
public record Cursor {
    /// <summary>
    /// The cursor coordinates.
    /// </summary>
    public Point Position { get; set; } = new Point(0, 0);

    /// <summary>
    /// Indicates whether the cursor should be shown/hidden.
    /// </summary>
    public bool Visible { get; set; } = true;
}
