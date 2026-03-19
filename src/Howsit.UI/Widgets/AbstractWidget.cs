using Howsit.UI.Drawing;
using Howsit.UI.Layout;

namespace Howsit.UI.Widgets;

public abstract class AbstractWidget : IWidget {
    /// <inheritdoc />
    public bool Visible { get; set; }

    /// <inheritdoc />
    public Size MinSize { get; set; }

    /// <inheritdoc />
    public Size? MaxSize { get; set; }

    /// <inheritdoc />
    public Size? PreferredSize { get; set; }

    /// <inheritdoc />
    public int StretchHorizontal { get; set; }

    /// <inheritdoc />
    public int StretchVertical { get; set; }

    /// <inheritdoc />
    public Padding Padding { get; set; }

    /// <inheritdoc />
    public Border Border { get; set; }

    /// <inheritdoc />
    public int Zindex { get; set; }

    /// <inheritdoc />
    protected Size Size { get; set; }

    /// <inheritdoc />
    public Rect? BoundingBox { get; set; } = null;

    /// <inheritdoc />
    public void SetBounds(Rect? rect) {
        BoundingBox = rect;
    }

    /// <inheritdoc />
    public abstract Cell[] Paint();
}
