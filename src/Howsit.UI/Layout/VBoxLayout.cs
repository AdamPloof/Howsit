using System;
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
    /// <remarks>
    /// This method has be written to minimize the number of passes required to calculate
    /// the final bounds of the widgets. The result is kind of hard to follow.
    /// 
    /// The gist of what's going on though is the first pass figures out the total space
    /// required for all of the widgets and their stretch preferences.
    /// 
    /// The second pass sets the bounds for each widget and factors in stretch/overlow.
    /// </remarks>
    /// <param name="widgets"></param>
    /// <param name="bounds"></param>
    public void Arrange(IReadOnlyList<IWidget> widgets, Rect bounds) {
        int stretchCount = 0;
        int totalStretchFactors = 0;
        int totalMinHeight = 0;
        int totalPreferredHeight = 0;
        foreach (IWidget w in widgets) {
            // Pass 1. Figure how much space is required and get stretch preferences.
            totalMinHeight += w.MinSize.Height;
            totalPreferredHeight += w.PreferredSize.Height;

            if (w.StretchVertical > 0) {
                stretchCount++;
                totalStretchFactors += w.StretchVertical;
            }
        }

        // Function used to get the height for a widget
        Func<IWidget, int> getHeight;
        if (totalPreferredHeight <= bounds.Height) {
            getHeight = w => w.PreferredSize.Height;
        } else if (totalMinHeight <= bounds.Height) {
            getHeight = w => w.MinSize.Height;
        } else {
            getHeight = w => w.MinSize.Height;
        }

        int extraHeight = bounds.Height - totalMinHeight;
        int currentY = bounds.Y;
        foreach (IWidget w in widgets) {
            // Pass 2. Set the bounds for each widget
            int width = bounds.Width;
            if (w.StretchHorizontal < 1) {
                // Don't stretch, go with max, min, or bounds width -- whatever fits.
                width = w.MaxSize.Width < bounds.Width
                    ? w.MaxSize.Width
                    : w.MinSize.Width < bounds.Width ? w.MinSize.Width : bounds.Width;
            }
            Rect r = new Rect(bounds.X, currentY, width, getHeight(w));
            w.SetBounds(r);
            currentY = w.MinSize.Height + 1;

            if (extraHeight == 0) {
                continue;
            } else if (extraHeight > 0) {
                // Extra space available, handle stretch    
                if (w.StretchVertical > 0) {
                    int stretch = (int)(((double)w.StretchVertical / stretchCount) * extraHeight);
                    w.Resize(w.BoundingBox.Width, w.BoundingBox.Height + stretch);
                    currentY =+ stretch;
                }
            } else {
                // Handle overflow                
            }
        }
    }
}
