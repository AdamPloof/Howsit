using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;

using Howsit.UI;
using Howsit.UI.Style;

namespace Howsit.App;

public class DemoRunner {
    private const string LineSeparator = ";;";
    private const int FrameRateMs = 1000;

    private IRenderer _renderer;

    public DemoRunner(IRenderer renderer) {
        _renderer = renderer;
    }

    public void Run() {
        int frameIdx = 0;
        List<string> frames = GetFrames();
        DateTime nextFrameAt = DateTime.UtcNow;

        while (true) {
            DateTime now = DateTime.UtcNow;
            if (now >= nextFrameAt) {
                string frame = frames[frameIdx];
                Cell[] buffer = ColorizeBuffer(
                    Renderer.StringToBuffer(frame, Console.WindowWidth, Console.WindowHeight)
                );
                _renderer.Render(buffer, Console.WindowWidth, Console.WindowHeight);
                frameIdx = frameIdx == (frames.Count - 1) ? 0 : frameIdx + 1;
                nextFrameAt = now.AddMilliseconds(FrameRateMs);
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

            Thread.Sleep(25);
        }
    }

    private Cell[] ColorizeBuffer(Cell[] buffer) {
        Color[] colors = GetColors();
        Cell[] colorized = new Cell[buffer.Length];
        for (int i = 0; i < buffer.Length; i++) {
            colorized[i] = new Cell() {
                Value = buffer[i].Value,
                Style = new CellStyle() { FgColor = colors[i % colors.Length] }
            };
        }

        return colorized;
    }

    private List<string> GetFrames() {
        string path = Path.Combine(AppContext.BaseDirectory, "var", "demo.txt");
        string[] lines = File.ReadAllLines(path);
        List<string> frames = [""];
        List<string> currentFrameLines = [];
        foreach (string line in lines) {
            if (line.Trim() == LineSeparator) {
                if (currentFrameLines.Count > 0) {
                    frames.Add(string.Join(Environment.NewLine, currentFrameLines));
                    currentFrameLines.Clear();
                }

                continue;
            }

            currentFrameLines.Add(line);
        }

        // Clear last frame
        if (currentFrameLines.Count > 0) {
            frames.Add(string.Join(Environment.NewLine, currentFrameLines));
            currentFrameLines.Clear();
        }

        return frames;
    }

    private Color[] GetColors() {
        Color[] colors = new Color[8];
        colors[0] = new Color(252, 235, 250);
        colors[1] = new Color(245, 235, 252);
        colors[2] = new Color(235, 239, 252);
        colors[3] = new Color(235, 250, 252);
        colors[4] = new Color(235, 252, 242);
        colors[5] = new Color(246, 252, 235);
        colors[6] = new Color(252, 249, 235);
        colors[7] = new Color(252, 241, 235);

        return colors;
    }
}
