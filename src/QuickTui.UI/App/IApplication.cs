using System;
using QuickTui.UI.Widgets;

namespace QuickTui.UI.App;

/// <summary>
/// The main orchestrator of a QuickTui app. Stores a reference to the root node, sets up
/// event dispatching/handling, and runs the main loop.
/// </summary>
public interface IApplication {
    /// <summary>
    /// Register an ITimer instance with an action to be called when the timer expires.
    /// The callback takes in the timer instance in order to provide a way for the callee to
    /// modify if/when the timer should repeat.
    /// </summary>
    /// <param name="timer"></param>
    /// <param name="callback"></param>
    public void Connect(ITimer timer, Action<ITimer> action);

    /// <summary>
    /// Start the application's mainloop
    /// </summary>
    public void Run();
}
