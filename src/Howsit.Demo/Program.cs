using Howsit.UI;
using Howsit.UI.Layout;
using Howsit.UI.Widgets;
using Howsit.UI.Drawing;
using Howsit.UI.Events;
using Howsit.UI.App;

VBoxLayout layout = new();
Container root = new Container(null, layout) {
    StretchVertical = 1,
    StretchHorizontal = 1,
    Padding = new Padding(1)
};
Label label = new Label(root, "Howsitgoinnotsobadgoodnyou?") {
    StretchHorizontal = 1,
    StretchVertical = 1,
    Border = new Border(BorderStyle.Solid)
};

Renderer renderer = new();
EventDispatcher dispatcher = new();
Application app = new Application(root, renderer, dispatcher);
app.Run();
