namespace Domain.Game.Commands;

public class AttackEvent
{
    public required Guid AttackerTerritoryId { get; set; }
    public required Guid DefenderTerritoryId { get; set; }
    public required int Troops { get; set; }
}