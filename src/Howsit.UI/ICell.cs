namespace Howsit.UI;

/// <summary>
/// A single cell of the screen. The TUI equivalent of a pixel.
/// </summary>
/// <remarks>
/// Cells can have a character value, text style (bold, italic, etc.),
/// a foreground color, and a background color. A cell is empty if it does
/// not have a character.
/// </remarks>
public interface ICell {
    /// <summary>
    /// Indicates whether the cell is empty.
    /// </summary>
    /// <returns>True if the cell is empty</returns>
    public bool IsEmpty();

    /// <summary>
    /// Indicates whether the cell contains style information.
    /// </summary>
    /// <returns>True if the cell has no style information</returns>
    public bool StyleIsEmpty();
}
