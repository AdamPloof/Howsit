using System;

namespace Howsit.UI.App;

/// <summary>
/// Standard UI timer with millisecond precision.
/// </summary>
public class Timer : ITimer {
    /// <inheritdoc />
    public TimeSpan Timeout { get; set; }

    /// <inheritdoc />
    public bool Repeat { get; set; }

    private bool _isRunning;
    private DateTimeOffset? _startTime;
    private DateTimeOffset? _stopTime;

    public Timer() {
        _isRunning = false;
        _startTime = null;
        _stopTime = null;
    }

    /// <inheritdoc />
    public void Start() {
        if (_isRunning) {
            throw new InvalidOperationException("Timer already started");
        }

        if (_stopTime is not null) {
            if (_startTime is null) {
                throw new InvalidOperationException("Timer was stopped but start time not preserved");
            }

            // Timer was stopped before it expired. Subtract already elapsed time from timeout.
            TimeSpan diff = (DateTimeOffset)_stopTime - (DateTimeOffset)_startTime;
            _startTime = DateTimeOffset.UtcNow - diff;
        } else {
            _startTime = DateTimeOffset.UtcNow;
        }

        _isRunning = true;
        _stopTime = null;
    }

    /// <inheritdoc />
    public void Stop() {
        _isRunning = false;
        _stopTime = DateTimeOffset.UtcNow;
    }

    /// <inheritdoc />
    public bool IsRunning() {
        return _isRunning;
    }

    /// <inheritdoc />
    public bool Execute() {

        return true;
    }

    /// <inheritdoc />
    public bool IsExpired() {
        if (_startTime is null) {
            // Timer not started
            return false;
        }

        TimeSpan elapsed;
        if (_stopTime is not null) {
            elapsed = (DateTimeOffset)_stopTime - (DateTimeOffset)_startTime;
        } else {
            DateTimeOffset now = DateTimeOffset.UtcNow;
            elapsed = now - (DateTimeOffset)_startTime;
        }

        return elapsed.Milliseconds >= Timeout.Milliseconds;
    }
}
