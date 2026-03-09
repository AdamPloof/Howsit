using System;

namespace Howsit.UI.Exceptions;

/// <summary>
/// A error that occured during rendering
/// </summary>
public class RenderException : Exception {
    public RenderException() { }

    public RenderException(string message) : base(message) {

    }

    public RenderException(string message, Exception innerException)
        : base(message, innerException) {

    }
}
