using System.Collections.Generic;

namespace Howsit.UI.Widgets;

public interface IContainer {
    /// <summary>
    /// Add a child widget to the container
    /// </summary>
    /// <param name="child"></param>
    public void AddChild(IWidget child);

    /// <summary>
    /// Get and set the bouding boxes for the container's children. 
    /// </summary>
    public void PerformLayout();
}
