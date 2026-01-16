using Domain.Game.Exceptions;

namespace Domain.Game;

public class GameState
{
    public required Guid GameId { get; set; }
    public required GameConfiguration Configuration { get; set; }
    public required List<Player> Players { get; set; }
    public required List<Territory> Territories { get; set; }
    public List<Attack> Attacks { get; set; } = [];
    public bool IsGameWaitingForReconnection { get; set; }

    /// <summary>
    /// Get the first Territory that has the given id
    /// </summary>
    /// <param name="idToFind">id of the territory to find</param>
    /// <returns>The founded territory</returns>
    /// <exception cref="TerritoryMissException">if the id is not present in the list</exception>
    public Territory FindTerritoryById(Guid idToFind)
    {
        var founded = Territories.FirstOrDefault(territory => territory.Id == idToFind);

        return founded ?? throw new TerritoryMissException(idToFind);
    }

    public Attack? GetCounterAttackOfAttack(Attack attack)
    {
        var oppositeDirectionAttacks = Attacks.FirstOrDefault(attack.GoesToTheOppositeDirection);

        if (oppositeDirectionAttacks is null)
        {
            return null;
        }

        var attackerTerritory = FindTerritoryById(attack.AttackerTerritoryId);
        var defenderTerritory = FindTerritoryById(attack.DefenderTerritoryId);

        return attackerTerritory.IsAlly(defenderTerritory)
            ? null
            : oppositeDirectionAttacks;
    }
}