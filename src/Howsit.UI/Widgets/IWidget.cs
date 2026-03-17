using Howsit.UI;
using Howsit.UI.Drawing;
using Howsit.UI.Layout;

namespace Howsit.UI.Widgets;

/// <summary>
/// Widgets are elements that can be placed on the screen. They have their own
/// way of handling inputs behavior.
/// </summary>
/// <remarks>
/// Note that only the Height and Width properties refer to actual dimensions. Which the
/// layout manager is responsible for setting. All other dimension props are used to request
/// size from the layout manager.
/// </remarks>
public interface IWidget {
    /// <summary>
    /// Indicates whether the widget should be painted or not. Hidden widgets
    /// do not take up any space in the layout.
    /// </summary>
    public bool Visible { get; set; }

    /// <summary>
    /// Minimum size requested for the widget
    /// </summary>
    public Size MinSize { get; set; }

    /// <summary>
    /// Maximum size requested for the widget
    /// </summary>
    public Size MaxSize { get; set; }

    /// <summary>
    /// Preferred size requested for the widget
    /// </summary>
    public Size PreferredSize { get; set; }

    /// <summary>
    /// Amount that the widget should stretch horizontally relative to its adjacent siblings.
    /// A value of 0 means no stretch, greater than 0 means grow, less than 0 means shrink.
    /// </summary>
    /// <example>
    /// Widgets A and B are side by side. A.StretchHorizontal = 1 and B.StretchHorizontal = 2
    /// --------------------------------
    /// | A      | B                    |
    /// |        | B gets 2x the space  |
    /// |        |                      |
    /// |_______________________________|
    /// 
    /// When A.StretchHorizontal = 2 and B.StretchHorizontal = 2
    /// --------------------------------
    /// | A            | B              |
    /// |              | Equal space    |
    /// |              |                |
    /// |_______________________________|
    /// </example>
    public int StretchHorizontal { get; set; }

    /// <summary>
    /// Amount that the widget should stretch vertically relative to its adjacent siblings
    /// A value of 0 means no stretch, greater than 0 means grow, less than 0 means shrink.
    /// </summary>
    public int StretchVertical { get; set; }

    /// <summary>
    /// Padding for an element.
    /// </summary>
    public Padding Padding { get; set; }

    /// <summary>
    /// Border for an element.
    /// </summary>
    public Border Border { get; set; }

    /// <summary>
    /// The z-index of the widget. Used to resolve which cell gets rendered when
    /// there is overlapping content.
    /// </summary>
    public int Zindex { get; set; }

    /// <summary>
    /// Actual size of widget. This is set by the layout manager when allocating space
    /// via SetBounds.
    /// </summary>
    protected Size Size { get; set; }

    /// <summary>
    /// Set the bounds of the widget. Used by layouts to allocate space.
    /// </summary>
    /// <param name="size"></param>
    public void SetBounds(Size size);

    /// <summary>
    /// Render the cell to a cell buffer.
    /// </summary>
    /// <returns></returns>
    public Cell[] Paint();
}
