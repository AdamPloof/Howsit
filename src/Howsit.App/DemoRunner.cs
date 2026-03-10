using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using Howsit.UI;

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
                _renderer.Render(frame, Console.WindowWidth, Console.WindowHeight);
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
}
