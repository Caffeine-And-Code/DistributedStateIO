namespace Domain.Game.Events;

public class PlayerDisconnectedEvent
{
    public required int PlayerId { get; set; }
}