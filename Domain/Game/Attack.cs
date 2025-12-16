namespace Domain.Game;

public class Attack
{
    public required Guid AttackerTerritoryId { get; set; }
    public required Guid DefenderTerritoryId { get; set; }
    public required int Troops { get; set; }
    /// <summary>
    /// Percentual, managed like moneys: the real percentual is Progress / 100, so if Progress value is 5052, the percentual is 50.52%
    /// </summary>
    public int Progress { get; set; } = 0;
}