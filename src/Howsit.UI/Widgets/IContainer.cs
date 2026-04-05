namespace Howsit.UI.Widgets;

/// <summary>
/// Containers are a special type of widget that are responsible for managing
/// the layout of their children.
/// </summary>
public interface IContainer {
    /// <summary>
    /// Get and set the bouding boxes for the container's children. 
    /// </summary>
    public void PerformLayout();
}
