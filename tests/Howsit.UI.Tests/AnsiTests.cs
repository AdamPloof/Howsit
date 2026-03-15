using Xunit;
using Howsit.UI;
using Howsit.UI.Style;

namespace Howsit.UI.Tests;

public class AnsiTests {
    [Fact]
    public void NoColorColorizeReturnsText() {
        string t = "Hello, Test";
        string output = Ansi.Colorize(t, null, null);

        Assert.Equal(t, output);
    }

    [Fact]
    public void FgColorIsSet() {
        string t = "Hello, Test";
        Color fg = new Color(255, 155, 15);

        string output = Ansi.Colorize(t, fg, null);
        string expected = $"\x1B[38;2;255;155;15mHello, Test\x1B[0m";

        Assert.Equal(expected, output);
    }

    [Fact]
    public void BgColorIsSet() {
        string t = "Hello, Test";
        Color bg = new Color(255, 155, 15);

        string output = Ansi.Colorize(t, null, bg);
        string expected = $"\x1B[48;2;255;155;15mHello, Test\x1B[0m";

        Assert.Equal(expected, output);
    }

    [Fact]
    public void FgAndBgColorAreSet() {
        string t = "Hello, Test";
        Color fg = new Color(80, 85, 212);
        Color bg = new Color(255, 155, 15);

        string output = Ansi.Colorize(t, fg, bg);
        string expected = $"\x1B[38;2;80;85;212;48;2;255;155;15mHello, Test\x1B[0m";

        Assert.Equal(expected, output);
    }

    [Fact]
    public void EmptyStyleReturnsEmptyEscapeSequence() {
        string output = Ansi.EscapeSequence(new CellStyle());

        Assert.Equal("", output);
    }

    [Theory]
    [InlineData(TextFormat.Bold, "\x1B[1m")]
    [InlineData(TextFormat.Muted, "\x1B[2m")]
    [InlineData(TextFormat.Italic, "\x1B[3m")]
    [InlineData(TextFormat.Underline, "\x1B[4m")]
    [InlineData(TextFormat.SlowBlink, "\x1B[5m")]
    [InlineData(TextFormat.RapidBlink, "\x1B[6m")]
    [InlineData(TextFormat.Strikethrough, "\x1B[9m")]
    public void EscapeSequenceReturnsIndividualFormatCodes(TextFormat format, string expected) {
        CellStyle style = new CellStyle(format);

        string output = Ansi.EscapeSequence(style);

        Assert.Equal(expected, output);
    }

    [Fact]
    public void EscapeSequenceCombinesMultipleFormatCodes() {
        CellStyle style = new CellStyle(
            TextFormat.Italic | TextFormat.Bold | TextFormat.Underline | TextFormat.Strikethrough
        );

        string output = Ansi.EscapeSequence(style);

        Assert.Equal("\x1B[3;1;4;9m", output);
    }

    [Fact]
    public void EscapeSequenceIncludesForegroundColor() {
        CellStyle style = new CellStyle(
            TextFormat.Normal,
            fgColor: new Color(12, 34, 56)
        );

        string output = Ansi.EscapeSequence(style);

        Assert.Equal("\x1B[38;2;12;34;56m", output);
    }

    [Fact]
    public void EscapeSequenceIncludesBackgroundColor() {
        CellStyle style = new CellStyle(
            TextFormat.Normal,
            bgColor: new Color(78, 90, 123)
        );

        string output = Ansi.EscapeSequence(style);

        Assert.Equal("\x1B[48;2;78;90;123m", output);
    }

    [Fact]
    public void EscapeSequenceCombinesColorsAndFormats() {
        CellStyle style = new CellStyle(
            TextFormat.Bold | TextFormat.Underline,
            fgColor: new Color(10, 20, 30),
            bgColor: new Color(40, 50, 60)
        );

        string output = Ansi.EscapeSequence(style);

        Assert.Equal("\x1B[38;2;10;20;30;48;2;40;50;60;1;4m", output);
    }
}
