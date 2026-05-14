using System;
using System.Collections.Generic;

namespace Howsit.UI.App;

/// <inheritdoc />
public class Scheduler : IScheduler {
    private IList<ITimer> _timers;

    public Scheduler() {
        _timers = [];
    }

    /// <inheritdoc />
    public void Register(ITimer timer) {
        _timers.Add(timer);
    }

    /// <inheritdoc />
    public void StartTimers() {
        foreach (ITimer timer in _timers) {
            if (timer.IsRunning()) {
                timer.Start();
            }
        }
    }

    /// <inheritdoc />
    public void ExecuteReadyTimers() {
        foreach (ITimer timer in _timers) {
            if (timer.IsRunning() && timer.IsExpired()) {
                timer.Execute();
            }
        }
    }
}
