namespace Domain.Game.Commands;

public class PlayerDisconnectedEvent
{
    public required int PlayerId { get; set; }
}