using System;

namespace Howsit.UI.App;

/// <summary>
/// The main orchestrator of a HowsitUI app. Stores a reference to the root node, sets up
/// event dispatching/handling, and runs the main loop.
/// </summary>
public interface IApplication {
    /// <summary>
    /// Start the application's mainloop
    /// </summary>
    public void Run();
}
