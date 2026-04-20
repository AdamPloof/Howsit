using Howsit.UI;
using Howsit.UI.Layout;
using Howsit.UI.Widgets;
using Howsit.UI.Drawing;
using Howsit.UI.Style;
using Howsit.UI.Events;
using Howsit.UI.App;
using Howsit.UI.Input;

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

Label label2 = new Label(
    root,
    "Okapp,appokwhat?",
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
Application app = new Application(root, renderer, dispatcher, inputParser, focusManager);
app.Run();
