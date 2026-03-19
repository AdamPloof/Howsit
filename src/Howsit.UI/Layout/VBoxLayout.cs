using System.Collections.Generic;
using System.Drawing;
using Howsit.UI.Widgets;

namespace Howsit.UI.Layout;

/// <summary>
/// Lines up widgets vertically
/// </summary>
public class VBoxLayout : ILayout {
    /// <summary>
    /// Arrange widgets in a vertically stacked box.
    /// </summary>
    /// <param name="widgets"></param>
    /// <param name="bounds"></param>
    public void Arrange(IEnumerable<IWidget> widgets, Rect bounds) {
        // A vertical layout is most concerned with allocating height, so we start there.
        int totalMinHeight = 0;
        int totalPreferredHeight = 0;
        foreach (IWidget w in widgets) {
            totalMinHeight += w.MinSize.Height;
            totalPreferredHeight += w.PreferredSize?.Height ?? w.MinSize.Height;
        }

        if (totalPreferredHeight <= bounds.Height) {
            // We can comfortably fit our widgets in the available space
        } else if (totalMinHeight <= bounds.Height) {
            // We can fit our widgets within the available space, but not at the preferred height
        } else {
            // We can't fit everything, time to deal with overflow.
        }
    }
}
