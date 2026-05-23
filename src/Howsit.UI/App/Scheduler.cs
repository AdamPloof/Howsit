using System;
using System.Collections.Generic;

namespace Howsit.UI.App;

/// <inheritdoc />
public class Scheduler : IScheduler {
    private Dictionary<ITimer, List<Action<ITimer>>> _timers;

    public Scheduler() {
        _timers = [];
    }

    /// <inheritdoc />
    public void Register(ITimer timer, Action<ITimer> action) {
        if (!_timers.TryGetValue(timer, out List<Action<ITimer>>? actions)) {
            actions = [];
            _timers[timer] = actions;
        }

        _timers[timer].Add(action);
    }

    /// <inheritdoc />
    public void StartTimers() {
        foreach (ITimer timer in _timers.Keys) {
            if (!timer.IsRunning()) {
                timer.Start();
            }
        }
    }

    /// <inheritdoc />
    public void ExecuteReadyTimers() {
        foreach (ITimer timer in _timers.Keys) {
            if (timer.IsRunning() && timer.IsExpired()) {
                foreach (Action<ITimer> action in _timers[timer]) {
                    action(timer);
                    if (!timer.IsRunning()) {
                        // Timer cancelled by previous action.
                        break;
                    }
                }

                if (timer.IsRunning() && timer.Repeat) {
                    // Start the next cycle of the timer from the end of the previous execution.
                    // This might not be the best way to handle this since it would increase
                    // drift over time if callback exectuion takes a while.
                    timer.Reset();
                }
            }
        }
    }
}
