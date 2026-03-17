using System.Collections.Generic;

namespace Howsit.UI.Widgets;

public interface IContainer : IWidget {
    /// <summary>
    /// Add a child widget to the container
    /// </summary>
    /// <param name="child"></param>
    public void AddChild(IWidget child);

    /// <summary>
    /// Arrange children by using a layout to allocate space
    /// and position of the container's children.
    /// </summary>
    public void Arrange();
}
