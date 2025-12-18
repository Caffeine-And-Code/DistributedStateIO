namespace Domain.Game;

public class GameState
{
    public required Guid GameId { get; set; }
    public required GameConfiguration Configuration { get; set; }
    public required List<Player> Players { get; set; }
    public required List<Territory>  Territories { get; set; }
    public List<Attack> Attacks { get; set; } = [];
    public bool IsGameWaitingForReconnection { get; set; }
}