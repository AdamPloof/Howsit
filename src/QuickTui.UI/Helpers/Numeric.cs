namespace QuickTui.UI.Helpers;

/// <summary>
/// Helper of numeric functions. Often used in place of modern C# functions
/// to provide compatibility with older versions.
/// </summary>
internal static class Numeric {
    public static double Clamp(double value, double min, double max) {
        if (value < min) {
            return min;
        }

        if (value > max) {
            return max;
        }

        return value;
    }

    public static int Clamp(int value, int min, int max) {
        if (value < min) {
            return min;
        }

        if (value > max) {
            return max;
        }

        return value;
    }
}
