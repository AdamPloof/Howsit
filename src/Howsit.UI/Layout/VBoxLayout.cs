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

        int totalStretchFactors = 0;
        int totalRequestedHeight = 0;
        foreach (IWidget w in widgets) {
            // Pass 1. Figure how much space is required and get stretch preferences.
            if (w.SizeHint.IsEmpty() && w.StretchVertical < 1) {
                throw new Exception(
                    "Unable to layout widget. Widget must either have a size hint or stretch"
                );
            }

            totalRequestedHeight += w.SizeHint.Height;

            if (w.StretchVertical > 0) {
                totalStretchFactors += w.StretchVertical;
            }
        }

        int currentY = bounds.Y;
        int extraHeight = bounds.Height - totalRequestedHeight;
        int remainingHeight = bounds.Height;
        foreach (IWidget w in widgets) {
            // Pass 2. Set the bounds for each widget
            if (remainingHeight < 1) {
                // TODO: rather than first come first serve on space, consider rationing space
                // among all widgets
                w.SetBounds(new Rect(bounds.X, currentY, 0, 0));
                continue;
            }

            int width;
            if (w.StretchHorizontal > 0) {
                width = bounds.Width;
            } else {
                width = w.SizeHint.Width < bounds.Width ? w.SizeHint.Width : bounds.Width;
            }

            int height = w.SizeHint.Height < remainingHeight ? w.SizeHint.Height : remainingHeight;
            if (extraHeight > 0 && w.StretchVertical > 0) {
                // Extra space available, handle stretch    
                int stretch = (int)(((double)w.StretchVertical / totalStretchFactors) * extraHeight);
                height += stretch;
            }

            Rect r = new Rect(bounds.X, currentY, width, height);
            w.SetBounds(r);
            remainingHeight -= w.GetHeight();
            currentY += w.GetHeight();
        }

        if (totalStretchFactors > 0) {
            // Pass 3. distribute remaining height evenly among stretch widgets
            // This a very dumb way of doing this, but good enough for now since there should never
            // more than a few units of left over height.
            int i = 0;
            while (remainingHeight > 0) {
                int idx = i % widgets.Count;
                if (widgets[idx].StretchVertical > 0) {
                    widgets[idx].Resize(widgets[idx].GetWidth(), widgets[idx].GetHeight() + 1);
                    remainingHeight -= 1;
                    
                    if (idx + 1 < widgets.Count) {
                        widgets[idx + 1].Nudge(0, 1);
                    }
                }

                i++;
            }
        }
    }
}
