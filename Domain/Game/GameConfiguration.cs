namespace Domain.Game;

public class GameConfiguration
{
    /// <summary>
    /// number of troupe spawned at second (round 2 decimal)
    /// TODO: fatto's comment: ???? what is this exactly ???? sorry don't remember ;(
    /// </summary>
    public required double SpawnRate { get; set; }
    public required int MaxTroopsForOwnedTerritory { get; init; }
    public required int MaxTroopForNotOwnedTerritory { get; init; }
    /// <summary>
    /// the seconds for waiting a disconnected player to reconnect, after this seconds
    /// the player will be removed by the game
    /// </summary>
    public required int DisconnectionSeconds { get; set; }
    /// <summary>
    /// The Progress at which an attack can be considered ended
    /// </summary>
    public required int MaxAttackProgress { get; init; }
    /// <summary>
    /// At each update what is the step to add at the Progress of each attacks 
    /// </summary>
    public required int AttackPercentageOfMovement  { get; init; }
}