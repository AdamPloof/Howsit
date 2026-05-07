using System;
using System.Collections.Generic;
using System.Threading;

using Howsit.UI.Widgets;
using Howsit.UI.Layout;
using Howsit.UI.Events;
using Howsit.UI.Input;

namespace Howsit.UI.App;

/// <inheritdoc />
public class Application : IApplication {
    private const int FRAME_THROTTLE_MS = 25;

    private IContainer _root;
    private IRenderer _renderer;
    private IEventDispatcher _dispatcher;
    private IInputParser _inputParser;
    private IFocusManager _focusManager;
    private int _winWidth;
    private int _winHeight;
    private bool _isRunning;

    /// <summary>
    /// Registered event handlers.
    /// </summary>
    private readonly Dictionary<Type, List<Action<UiEvent>>> _handlers = [];

    public Application(
        IContainer root,
        IRenderer renderer,
        IEventDispatcher dispatcher,
        IInputParser inputParser,
        IFocusManager focusManager
    ) {
        _winWidth = Console.WindowWidth;
        _winHeight = Console.WindowHeight;

        _root = root;
        _root.SetBounds(new Rect(0, 0, _winWidth, _winHeight));
        _renderer = renderer;
        _dispatcher = dispatcher;
        _inputParser = inputParser;
        _focusManager = focusManager;

        _isRunning = false;

        InitializeHandlers();
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

        string? exitMessage = null;
        try {
            MainLoop();
        } catch (Exception e) {
            exitMessage = e.Message;
        } finally {
            Console.TreatControlCAsInput = previousTreatControlCAsInput;
            Console.Out.Write(Ansi.ExitAlternateScreen);
            Console.Out.Flush();
            if (exitMessage is not null) {
                Console.Out.Write(exitMessage);
            }
        }
    }

    private void MainLoop() {
        while (_isRunning) {
            CheckForWindowResize();
            IEnumerable<UiEvent> inputEvents = _inputParser.ReadAvailable();
            foreach (UiEvent inputEvent in inputEvents) {
                HandleApplicationEvent(inputEvent);
                if (inputEvent.Handled) {
                    continue;
                }

                _dispatcher.Dispatch(_focusManager.GetFocusedWidget(), inputEvent);
            }

            if (!_isRunning) {
                // Input could have caused an early exit so let's break out now
                // to avoid drawing an extra frame and waiting for the throttle timeout.
                break;
            }

            if (NeedsDraw(_root)) {
                Cell[] buffer = _root.Paint();
                _renderer.Render(buffer, _winWidth, _winHeight);
            }

            Thread.Sleep(FRAME_THROTTLE_MS);
        }
    }

    private void CheckForWindowResize() {
        int newWidth = Console.WindowWidth;
        int newHeight = Console.WindowHeight;
        if (newWidth != _winWidth || newHeight != _winHeight) {
            _winWidth = newWidth;
            _winHeight = newHeight;
            _dispatcher.Dispatch(_root, new ResizeEvent(_winWidth, _winHeight));
        }
    }

    private void InitializeHandlers() {
        List<Action<UiEvent>> keyHandlers = [];
        keyHandlers.Add(e => HandleKeyEvent((KeyEvent)e));
        _handlers.Add(typeof(KeyEvent), keyHandlers);
    }

    private void HandleApplicationEvent(UiEvent uiEvent) {
        Type eventType = uiEvent.GetType();
        if (_handlers.TryGetValue(eventType, out List<Action<UiEvent>>? handlers)) {
            foreach (Action<UiEvent> handler in handlers) {
                handler(uiEvent);
                if (uiEvent.Handled) {
                    return;
                }
            }
        }
    }
    
    private void HandleKeyEvent(KeyEvent keyEvent) {
        if (keyEvent.Key == ConsoleKey.Escape) {
            _focusManager.ChangeFocus(_root);
        }

        if (keyEvent.Key == ConsoleKey.Tab) {
            if (_focusManager.FocusedWidgetCapturesTab()) {
                return;
            }

            if (keyEvent.Modifiers.HasFlag(ConsoleModifiers.Shift)) {
                _focusManager.FocusPrevious();
            } else {            
                _focusManager.FocusNext();
            }
        }

        bool isCtrlC = keyEvent.Key == ConsoleKey.C
            && keyEvent.Modifiers.HasFlag(ConsoleModifiers.Control);

        // TODO: should make shutdown configurable.
        if (isCtrlC) {
            _isRunning = false;
        }
    }
}
