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
    /// 
    /// If the available space is not enough to meet the min size requested by each widget,
    /// the space will be distributed on a "first come, first served" basis.
    /// </remarks>
    /// <param name="widgets"></param>
    /// <param name="bounds"></param>
    public void Arrange(IReadOnlyList<IWidget> widgets, Rect bounds) {
        if (bounds.IsEmpty()) {
            throw new ArgumentException("Cannot layout widgets in empty bounds");
        }

        if (bounds.X != 0 || bounds.Y != 0) {
            // Bounds are relative to the parent so it would make no sense to pass bounds
            // with coordinates other than the 0, 0.
            throw new ArgumentException(
                "Bounds passd to layout should always have a position of top left"
            );
        }

        int stretchCount = 0;
        int totalStretchFactors = 0;
        int totalMinHeight = 0;
        int totalPreferredHeight = 0;
        foreach (IWidget w in widgets) {
            // Pass 1. Figure how much space is required and get stretch preferences.
            if (w.MinSize.IsEmpty() && w.StretchVertical < 1) {
                throw new Exception(
                    "Unable to layout widget. Widget must either have a min size or stretch"
                );
            }

            totalMinHeight += w.MinSize.Height;
            totalPreferredHeight += w.PreferredSize.IsEmpty() ? w.MinSize.Height : w.PreferredSize.Height;

            if (w.StretchVertical > 0) {
                stretchCount++;
                totalStretchFactors += w.StretchVertical;
            }
        }

        // Function used to get the height for a widget
        Func<IWidget, int> getHeight;
        if (totalPreferredHeight <= bounds.Height) {
            getHeight = w => w.PreferredSize.IsEmpty() ? w.MinSize.Height : w.PreferredSize.Height;
        } else if (totalMinHeight <= bounds.Height) {
            getHeight = w => w.MinSize.Height;
        } else {
            getHeight = w => w.MinSize.Height;
        }

        // TODO: I'm pretty sure a mix of min and preferred height is going to cause a problem
        int extraHeight = bounds.Height - totalMinHeight;
        int currentY = bounds.Y;
        int remainingHeight = bounds.Height;
        foreach (IWidget w in widgets) {
            // Pass 2. Set the bounds for each widget
            if (remainingHeight < 1) {
                break;
            }

            int width = bounds.Width;
            if (w.StretchHorizontal < 1) {
                // Don't stretch, go with max, min, or bounds width -- whatever fits.
                width = w.MaxSize.Width < bounds.Width
                    ? w.MaxSize.Width
                    : w.MinSize.Width < bounds.Width ? w.MinSize.Width : bounds.Width;
            }

            int height = getHeight(w);
            height = height > remainingHeight ? height : remainingHeight;
            if (extraHeight > 0) {
                // Extra space available, handle stretch    
                if (w.StretchVertical > 0) {
                    int stretch = (int)(((double)w.StretchVertical / stretchCount) * extraHeight);
                    height += stretch;
                }
            }
            
            Rect r = new Rect(bounds.X, currentY, width, height);
            w.SetBounds(r);
            currentY = w.GetHeight() + 1;
        }
    }
}
