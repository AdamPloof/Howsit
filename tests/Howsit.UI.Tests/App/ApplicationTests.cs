using Xunit;
using System;

using Howsit.UI;
using Howsit.UI.App;
using Howsit.UI.Events;
using Howsit.UI.Widgets;
using Howsit.UI.Layout;

namespace Howsit.UI.Tests.App;

public class ApplicationTests {
    [Fact]
    public void TreeNeedsDrawIsDetected() {
        HBoxLayout layout = new HBoxLayout();
        Container root = new Container(null, layout);
        Label child1a = new Label(root, "Label 1A");
        Label child1b = new Label(root, "Label 1B");
        Label child1c = new Label(root, "Label 1C");
        Label child2a = new Label(child1a, "Label 2A");
        Label child2b = new Label(child1a, "Label 2B");
        Label child3a = new Label(child2a, "Label 3A");
        root.IsDirty = false;
        child1a.IsDirty = false;
        child1b.IsDirty = false;
        child1c.IsDirty = false;
        child2a.IsDirty = false;
        child2b.IsDirty = false;
        child3a.IsDirty = true;

        Assert.True(Application.NeedsDraw(root));
    }

    [Fact]
    public void TreeIsCleanIsDetected() {
        HBoxLayout layout = new HBoxLayout();
        Container root = new Container(null, layout);
        Label child1a = new Label(root, "Label 1A");
        Label child1b = new Label(root, "Label 1B");
        Label child1c = new Label(root, "Label 1C");
        Label child2a = new Label(child1a, "Label 2A");
        Label child2b = new Label(child1a, "Label 2B");
        Label child3a = new Label(child2a, "Label 3A");
        root.IsDirty = false;
        child1a.IsDirty = false;
        child1b.IsDirty = false;
        child1c.IsDirty = false;
        child2a.IsDirty = false;
        child2b.IsDirty = false;
        child3a.IsDirty = false;

        Assert.False(Application.NeedsDraw(root));
    }
}
