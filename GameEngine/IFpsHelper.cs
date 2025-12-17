namespace GameEngine;

public interface IFpsHelper
{
    /// <summary>
    /// save as previous cycle time the current milliseconds
    /// </summary>
    /// <param name="fps">the max FPS of the game (like 30 or 60)</param>
    public void Initialize(long fps);
    /// <summary>
    /// return the elapsed time between previous cycle time and now in milliseconds,
    /// save as previous cycle time the actual cycle time (datetime now)
    /// </summary>
    /// <returns>the elapsed time between previous cycle time and actual cycle time</returns>
    public long GetElapsedTime();
    /// <summary>
    /// wait for the amount of time of the difference between FPS and the now milliseconds minus current cycle time
    /// (dt = now_millis - actual_cycle_time, if dt is less than fps then wait for (fps - dt) milliseconds)
    /// </summary>
    /// <returns>wait until next frame ready for not exceed max FPS</returns>
    public Task WaitForNextFrame();
}