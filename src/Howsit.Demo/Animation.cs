using System;
using System.IO;
using System.Collections.Generic;

namespace Howsit.Demo;

/// <summary>
/// Main class for managing the lifecycle of Howsit.
/// </summary>
public class Animation {
    private const string LineSeparator = ";;";

    private List<string> _frames;
    int _frameIdx;

    public Animation() {
        _frames = GetFrames();
        _frameIdx = 0;
    }

    public string GetNextFrame() {
        string frame = _frames[_frameIdx];
        _frameIdx = _frameIdx == (_frames.Count - 1) ? 0 : _frameIdx + 1;

        return frame;
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
