using System.Diagnostics;
using GameEngine;

namespace GameWebApp.Classes.Utilities;

public class FpsHelper : IFpsHelper
{
    private readonly Stopwatch _stopwatch = new();
    private long _lastFrameTime;
    private int _fps;

    public void Initialize(int fps)
    {
        _fps = fps;
        _stopwatch.Restart();
        _lastFrameTime = 0;
    }

    /// <summary>
    /// Ritorna il delta time in millisecondi dall’ultimo frame
    /// </summary>
    public long GetElapsedTime()
    {
        var now = _stopwatch.ElapsedMilliseconds;
        var elapsed = now - _lastFrameTime;
        _lastFrameTime = now;
        return elapsed;
    }

    /// <summary>
    /// Attende fino al prossimo frame per mantenere FPS costanti
    /// </summary>
    public async Task WaitForNextFrame()
    {
        var frameTime = 1000.0 / _fps;
        var elapsed = _stopwatch.ElapsedMilliseconds - _lastFrameTime;

        var wait = frameTime - elapsed;
        if (wait > 0)
        {
            await Task.Delay(TimeSpan.FromMilliseconds(wait));
        }
    }
}