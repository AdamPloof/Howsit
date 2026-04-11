using System;
using System.IO;
using System.Collections.Generic;
using System.Threading;

using Howsit.UI;
using Howsit.UI.App;

namespace Howsit.Demo;

/// <summary>
/// Main class for managing the lifecycle of Howsit.
/// </summary>
public class App {
    private const string LineSeparator = ";;";
    private const int FrameRateMs = 1000;

    public App() {
        
    }

    public void Run() {
        int frameIdx = 0;
        List<string> frames = GetFrames();
        DateTime nextFrameAt = DateTime.UtcNow;

        while (true) {
            DateTime now = DateTime.UtcNow;
            if (now >= nextFrameAt) {
                string frame = frames[frameIdx];

                frameIdx = frameIdx == (frames.Count - 1) ? 0 : frameIdx + 1;
                nextFrameAt = now.AddMilliseconds(FrameRateMs);
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
