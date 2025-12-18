using Domain.Game;

namespace GameWebApp.Classes.Utilities;

public static class GameSettingsLoader
{
    public static GameSettings Load(IConfiguration configuration)
    {
        var settings = new GameSettings();
        configuration.GetSection("GameSettings").Bind(settings);
        return settings;
    }
}