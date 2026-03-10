using System;

using Howsit.UI;

namespace Howsit.App;

/// <summary>
/// Main class for managing the lifecycle of Howsit.
/// </summary>
public class App {
    private const string EnterAlternateScreen = "\u001b[?1049h";
    private const string ExitAlternateScreen = "\u001b[?1049l";
    private const string ClearScreenAndHome = "\u001b[2J\u001b[H";
    private const string ClearToEndOfLine = "\u001b[K";
    private const string ShowCursor = "\u001b[?25h";

    private IRenderer _renderer;

    public App(IRenderer renderer) {
        _renderer = renderer;
    }

    public void Run() {
        bool previousTreatControlCAsInput = Console.TreatControlCAsInput;

        Console.TreatControlCAsInput = true;
        Console.Out.Write(EnterAlternateScreen);
        Console.Out.Write(ShowCursor);
        DemoRunner demo = new(_renderer);

        try {
            demo.Run();
        } catch (Exception e) {
            Console.Out.Write(e.Message);
        } finally {
            Console.TreatControlCAsInput = previousTreatControlCAsInput;
            Console.Out.Write(ExitAlternateScreen);
            Console.Out.Flush();
        }
    }
}
