using System;

namespace Howsit.UI.App;

/// <summary>
/// Schedulers are responsible for managing timed events and controls.
/// </summary>
public interface IScheduler {
    /// <summary>
    /// Add a timer for the scheduler to manage.
    /// </summary>
    /// <param name="timer"></param>
    public void Register(ITimer timer, Action<ITimer> action);

    /// <summary>
    /// Start all registered timers.
    /// </summary>
    public void StartTimers();

    /// <summary>
    /// Execute the callbacks of any timers whose timeouts have expired.
    /// </summary>
    public void ExecuteReadyTimers();
}
