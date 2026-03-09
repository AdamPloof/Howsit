using Howsit.UI;

namespace Howsit.App;

/// <summary>
/// Main class for managing the lifecycle of Howsit.
/// </summary>
public class App {
    private IRenderer _renderer;

    public App(IRenderer renderer) {
        _renderer = renderer;
    }

    public void Run() {
        
    }
}
