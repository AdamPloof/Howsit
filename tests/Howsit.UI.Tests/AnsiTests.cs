using Xunit;
using Howsit.UI;
using Howsit.UI.Style;

namespace Howsit.UI.Tests;

public class AnsiTests {
    public void NoColorColorizeReturnsText() {
        string t = "Hello, Test";
        string output = Ansi.Colorize(t, null, null);

        Assert.Equal(t, output);
    }

    public void FgColorIsSet() {
        string t = "Hello, Test";
        Color fg = new Color(255, 155, 15);

        string output = Ansi.Colorize(t, fg, null);
        string expected = $"\x1B[38;2;255;155;15mHello, Test\x1B[0m";

        Assert.Equal(expected, output);
    }

    public void BgColorIsSet() {
        string t = "Hello, Test";
        Color bg = new Color(255, 155, 15);

        string output = Ansi.Colorize(t, bg, null);
        string expected = $"\x1B[48;2;255;155;15mHello, Test\x1B[0m";

        Assert.Equal(expected, output);
    }

    public void FgAndBgColorAreSet() {
        string t = "Hello, Test";
        Color fg = new Color(80, 85, 212);
        Color bg = new Color(255, 155, 15);

        string output = Ansi.Colorize(t, bg, null);
        string expected = $"\x1B[38;2;80;85;212;48;2;255;155;15mHello, Test\x1B[0m";

        Assert.Equal(expected, output);
    }
}
