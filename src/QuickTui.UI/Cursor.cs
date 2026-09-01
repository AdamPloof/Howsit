using System.Drawing;

namespace QuickTui.UI;

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

    /// <summary>
    /// The cursor's style.
    /// </summary>
    public CursorStyle Style { get; set; } = CursorStyle.Default;
}

public enum CursorStyle {
    BlinkingBlock,
    Default,
    SteadyBlock,
    BlinkingUnderline,
    SteadyUnderline,
    BlinkingBar,
    SteadyBar
}
