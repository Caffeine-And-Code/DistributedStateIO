namespace Domain.Game;

public class Player
{
    public required int Id { get; set; }
    public required bool IsInGame { get; set; }
    public required string Username { get; set; }
    /// <summary>
    /// represent if a player has been disconnected due to too much seconds of disconnection absence
    /// </summary>
    public bool IsDisconnected { get; set; } = false;
    /// <summary>
    /// If the value is "-1" the player is connected to the game,
    /// else if the value is greater than 0, the player is in waiting for
    /// reconnection and those are the seconds left for attempting the reconnection 
    /// </summary>
    public int RemainingSecondsForReconnection { get; set; } = -1;
}