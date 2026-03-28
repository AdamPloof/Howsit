using Xunit;
using System;

using Howsit.UI;

namespace Howsit.UI.Tests;

public class TextBufferTests {
    [Fact]
    public void StringIsConvertedToBuffer() {
        string content = """
            123
            456
            789
            """.Trim();
        Cell[] buffer = TextBuffer.FromString(content, 3, 3);

        Assert.Equal(9, buffer.Length);
        Assert.Equal('1', buffer[0].Value);
        Assert.Equal('2', buffer[1].Value);
        Assert.Equal('3', buffer[2].Value);
        Assert.Equal('4', buffer[3].Value);
        Assert.Equal('5', buffer[4].Value);
        Assert.Equal('6', buffer[5].Value);
        Assert.Equal('7', buffer[6].Value);
        Assert.Equal('8', buffer[7].Value);
        Assert.Equal('9', buffer[8].Value);
    }

    [Fact]
    public void StringOverflowsWidthWrapped() {
        string content = """
            abcd
            efgh
            ijkl
            """.Trim();
        Cell[] buffer = TextBuffer.FromString(content, 3, 3);

        Assert.Equal(9, buffer.Length);
        Assert.Equal('a', buffer[0].Value);
        Assert.Equal('b', buffer[1].Value);
        Assert.Equal('c', buffer[2].Value);
        Assert.Equal('d', buffer[3].Value);
        Assert.True(buffer[5].IsEmpty());
        Assert.True(buffer[4].IsEmpty());
        Assert.Equal('e', buffer[6].Value);
        Assert.Equal('f', buffer[7].Value);
        Assert.Equal('g', buffer[8].Value);
    }

    [Fact]
    public void StringOverflowsHeightTruncated() {
        string content = """
            abc
            def
            ghi
            """.Trim();
        Cell[] buffer = TextBuffer.FromString(content, 3, 2);

        Assert.Equal(6, buffer.Length);
        Assert.Equal('a', buffer[0].Value);
        Assert.Equal('b', buffer[1].Value);
        Assert.Equal('c', buffer[2].Value);
        Assert.Equal('d', buffer[3].Value);
        Assert.Equal('e', buffer[4].Value);
        Assert.Equal('f', buffer[5].Value);
    }

    [Fact]
    public void StringOverflowsAllDimensionsWrappedAndTruncated() {
        string content = """
            abcd
            efgh
            ijkl
            """.Trim();
        Cell[] buffer = TextBuffer.FromString(content, 3, 2);

        Assert.Equal(6, buffer.Length);
        Assert.Equal('a', buffer[0].Value);
        Assert.Equal('b', buffer[1].Value);
        Assert.Equal('c', buffer[2].Value);
        Assert.Equal('d', buffer[3].Value);
        Assert.True(buffer[5].IsEmpty());
        Assert.True(buffer[4].IsEmpty());
    }
}
