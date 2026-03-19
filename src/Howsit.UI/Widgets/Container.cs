using System.Collections.Generic;

using Howsit.UI;
using Howsit.UI.Layout;

namespace Howsit.UI.Widgets;

/// <summary>
/// Base container type. Containers are used to group widgets into a layout section.
/// Their layout style/orientation is determined by the ILayout type provided.
/// </summary>
public class Container : AbstractWidget, IContainer {
    private ILayout _layout;
    private List<IWidget> _children;

    public Container(ILayout layout) {
        _layout = layout;
        _children = new List<IWidget>();
    }

    /// <inheritdoc />
    public void AddChild(IWidget child) {
        _children.Add(child);
    }

    /// <inheritdoc />
    public void PerformLayout() {
        if (BoundingBox is not Rect bbox) {
            // TODO: throw error?
            // TODO: validate bounding box is at least 1x1?
            return;
        }

        _layout.Arrange(_children, bbox);
    }

    /// <summary>
    /// Loops through children to get requested space which is passed to the layout.
    /// The layout will return the actual space available to assign to each child.
    /// Finally, each widget will be painted and added to this containers buffer.
    /// </summary>
    /// <returns></returns>
    public override Cell[] Paint() {
        Cell[] cells = [];

        return cells;
    }
}
