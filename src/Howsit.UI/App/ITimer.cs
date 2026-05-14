using System;

namespace Howsit.UI.App;

/// <summary>
/// Timers are used to schedule actions. Actions may be one-shot or repeatable such as
/// advancing the frames of an animation.
/// </summary>
public interface ITimer {
    /// <summary>
    /// The length of time before the timer's callback can be called.
    /// </summary>
    public TimeSpan Timeout { get; set; }

    /// <summary>
    /// If true, the timeout will reset when Execute() is called.
    /// </summary>
    public bool Repeat { get; set; }

    /// <summary>
    /// Start the timer.
    /// </summary>
    public void Start();

    /// <summary>
    /// Stop the timer.
    /// </summary>
    public void Stop();

    /// <summary>
    /// Indicates whether this timer is still alive. Once a timer has expired and its
    /// callback executed it becomes inactive unless it is set to repeat.
    /// </summary>
    public bool IsRunning();

    /// <summary>
    /// Returns true if the timer is not stopped and now is greater than the Timeout duration
    /// from the start time.
    /// </summary>
    /// <returns></returns>
    public bool IsExpired();

    /// <summary>
    /// Execute the Timer's callback. Returns true if the timer should repeat.
    /// </summary>
    /// <remarks>
    /// Throws an InvalidOperationException if the timer's timeout has not expired.
    /// </remarks>
    public bool Execute();
}
