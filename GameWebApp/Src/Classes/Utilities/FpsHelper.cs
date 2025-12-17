using GameEngine;

namespace GameWebApp.Classes.Utilities;

public class FpsHelper : IFpsHelper
{
    private long PreviousCycleTime { get; set; }
    private int Fps { get; set; }

    private static long GetCurrentMilliseconds() => DateTimeOffset.Now.ToUnixTimeMilliseconds();

    public void Initialize(int fps)
    {
        Fps = fps;
        PreviousCycleTime = GetCurrentMilliseconds();
    }

    public long GetElapsedTime()
    {
        var currentCycleTime = GetCurrentMilliseconds() - PreviousCycleTime;
        PreviousCycleTime = GetCurrentMilliseconds();
        return currentCycleTime;
    }

    public async Task WaitForNextFrame()
    {
        var dt = GetCurrentMilliseconds() - PreviousCycleTime;
        var expectedDt = 1000 / Fps;

        // exceeded frame window
        if (dt >= expectedDt)
        {
            return;
        }

        var millisToWait = expectedDt - dt;
        await Task.Delay(TimeSpan.FromMilliseconds(millisToWait));
    }
}