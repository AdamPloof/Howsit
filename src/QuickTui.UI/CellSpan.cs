using System.Collections.Generic;

using QuickTui.UI.Style;

namespace QuickTui.UI;

/// <summary>
/// A span of cells and its starting position.
/// </summary>
public record CellSpan {
    public int Row { get; init; }
    public int StartColumn { get; init; }
    public CellStyle? Style { get; init; } = null;
    public List<Cell> Cells { get; init; } = [];
}
