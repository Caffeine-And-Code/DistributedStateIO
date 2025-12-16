namespace Domain.Game;

public class GameState
{
    public required GameConfiguration Configuration { get; set; }
    public required ICollection<Player> Players { get; set; }
    public required ICollection<Territory>  Territories { get; set; }
    public ICollection<Attack> Attacks { get; set; } = new List<Attack>();
    public bool IsGameWaitingForReconnection { get; set; } = false;
}