using System;
using System.Threading;

using Howsit.UI.Widgets;
using Howsit.UI.Layout;
using Howsit.UI.Events;

namespace Howsit.UI.App;

/// <inheritdoc />
public class Application : IApplication {
    private const int FRAME_THROTTLE_MS = 25;

    private IContainer _root;
    private IRenderer _renderer;
    private IEventDispatcher _dispatcher;
    private int _winWidth;
    private int _winHeight;
    private bool _isRunning;

    public Application(IContainer root, IRenderer renderer, IEventDispatcher dispatcher) {
        _winWidth = Console.WindowWidth;
        _winHeight = Console.WindowHeight;

        _root = root;
        _root.SetBounds(new Rect(0, 0, _winWidth, _winHeight));
        _renderer = renderer;
        _dispatcher = dispatcher;

        _isRunning = false;
    }

    /// <summary>
    /// Check if any node in the widget tree is dirty.
    /// </summary>
    /// <param name="widget"></param>
    /// <returns></returns>
    public static bool NeedsDraw(IWidget widget) {
        if (widget.IsDirty) {
            return true;
        }

        foreach (IWidget child in widget.GetChildren()) {
            return NeedsDraw(child);
        }

        return false;
    }

    /// <inheritdoc />
    public void Run() {
        _isRunning = true;

        bool previousTreatControlCAsInput = Console.TreatControlCAsInput;
        Console.TreatControlCAsInput = true;
        Console.Out.Write(Ansi.EnterAlternateScreen);
        Console.Out.Write(Ansi.ShowCursor);

        try {
            MainLoop();
        } catch (Exception e) {
            Console.Out.Write(e.Message);
        } finally {
            Console.TreatControlCAsInput = previousTreatControlCAsInput;
            Console.Out.Write(Ansi.ExitAlternateScreen);
            Console.Out.Flush();
        }
    }
    
    private void MainLoop() {
        while (_isRunning) {
            int newWidth = Console.WindowWidth;
            int newHeight = Console.WindowHeight;
            if (newWidth != _winWidth || newHeight != _winHeight) {
                _winWidth = newWidth;
                _winHeight = newHeight;
                _dispatcher.Dispatch(_root, new ResizeEvent(_winWidth, _winHeight));
            }

            if (NeedsDraw(_root)) {
                Cell[] buffer = _root.Paint();
                _renderer.Render(buffer, _winWidth, _winHeight);
            }

            // Exit via Esc or ctrl+c
            while (Console.KeyAvailable) {
                var key = Console.ReadKey(intercept: true);
                var isEscape = key.Key == ConsoleKey.Escape;
                var isCtrlC = key.Key == ConsoleKey.C && key.Modifiers.HasFlag(ConsoleModifiers.Control);
                if (isEscape || isCtrlC) {
                    return;
                }
            }

            Thread.Sleep(FRAME_THROTTLE_MS);
        }
    }
}
