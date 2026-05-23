using System;

using Howsit.UI;
using Howsit.UI.Layout;
using Howsit.UI.Widgets;
using Howsit.UI.Drawing;
using Howsit.UI.Style;
using Howsit.UI.Events;
using Howsit.UI.App;
using Howsit.UI.Input;

namespace Howsit.Demo;

internal static class Program {
    private static void Main() {
        Animation animation = new();
        UI ui = new UI(animation);
        ui.Run();
    }
}

public class UI {
    private IApplication _app;
    private Animation _animation;
    private Canvas? _canvas = null;

    public UI(Animation animation) {
        _animation = animation;
        _app = Build();
        ScheduleAnimation();
    }

    public void Run() {
        _app.Run();
    }
    
    private IApplication Build() {
        VBoxLayout layout = new();
        Container root = new Container(null, layout) {
            StretchVertical = 1,
            StretchHorizontal = 1,
            Padding = new Padding(1)
        };
        Label label = new Label(
            root,
            "Howsitgoinnotsobadgoodnyou?",
            new CellStyle(TextFormat.Normal, new Color(255, 0, 0))
        ) {
            StretchHorizontal = 1,
            StretchVertical = 1,
            Border = new Border(BorderStyle.Solid)
        };

        _canvas = new Canvas(
            root,
            // _animation.GetNextFrame(),
            _animation.GetNextFrame(),
            new CellStyle(TextFormat.Normal, new Color(0, 255, 45))
        ) {
            StretchHorizontal = 1,
            StretchVertical = 1,
            Border = new Border(BorderStyle.Solid)
        };

        Renderer renderer = new();
        EventDispatcher dispatcher = new();
        InputParser inputParser = new();
        FocusManager focusManager = new(root);
        Scheduler scheduler = new();
        Application app = new Application(
            root,
            renderer,
            dispatcher,
            inputParser,
            focusManager,
            scheduler
        );

        return app;
    }

    private void ScheduleAnimation() {
        if (_canvas is null) {
            throw new InvalidOperationException("Canvas not initialized");
        }

        TimeSpan timeout = TimeSpan.FromMilliseconds(1000);
        Timer timer = new Timer() { Repeat = true, Timeout = timeout };
        _app.Connect(timer, (ITimer _) => _canvas.SetContent(_animation.GetNextFrame()));
    }
}
