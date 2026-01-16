namespace Domain.Game;

public class Attack
{
    public required Guid AttackerTerritoryId { get; init; }
    public required Guid DefenderTerritoryId { get; init; }
    public required int Troops { get; set; }

    /// <summary>
    /// Percentual, managed like moneys: the real percentual is Progress / 100, so if Progress value is 5052, the percentual is 50.52%
    /// </summary>
    public int Progress { get; set; }

    /// <summary>
    /// Checks if the passed attack go to the opposite direction of this attack,
    /// so if the attacker and defender id are reversed
    /// </summary>
    /// <param name="other">other attack</param>
    /// <returns>true if other goes to the opposite direction, false otherwise</returns>
    public bool GoesToTheOppositeDirection(Attack other)
    {
        return other.AttackerTerritoryId == DefenderTerritoryId &&
               other.DefenderTerritoryId == AttackerTerritoryId;
    }
}