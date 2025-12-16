namespace Domain.Game;

public class GameConfiguration
{
    /// <summary>
    /// number of troupe spawned at second (round 2 decimal)
    /// </summary>
    public required double SpawnRate { get; set; }
    public required int MaxTroopsForOwnedTerritory { get; set; }
    public required int MaxTroopForNotOwnedTerritory { get; set; }
    /// <summary>
    /// the seconds for waiting a disconnected player to reconnect, after this seconds
    /// the player will be removed by the game
    /// </summary>
    public required int DisconnectionSeconds { get; set; }
}