using Domain.Game;
using GameWebApp.Classes.Models;

namespace GameWebApp.Classes.Utilities;

public sealed class TerritoryShapeProvider
{
    private static readonly Lock Padlock = new();

    TerritoryShapeProvider()
    {
    }

    public static TerritoryShapeProvider Instance
    {
        get
        {
            lock (Padlock)
            {
                if (field == null)
                {
                    field = new TerritoryShapeProvider();
                }

                return field;
            }
        }
    } = null;

    private Dictionary<Guid, UiTerritory>? _territories;

    public void Initialize(ICollection<UiTerritory> territories)
    {
        _territories = new Dictionary<Guid, UiTerritory>();
        foreach (var territory in territories)
        {
            _territories.Add(territory.Id, territory);
        }
    }

    public ICollection<UiTerritory> GetTerritories(ICollection<Territory> updatedTerritories)
    {
        if (_territories == null)
        {
            throw new InvalidOperationException("should not be called before initialization");
        }

        foreach (var territory in updatedTerritories)
        {
            _territories[territory.Id].Troops = territory.Troops;
        }

        return _territories.Values;
    }

    public ICollection<ShapePoint> GetShapesOfTerritory(Guid territoryId)
    {
        if (!_territories!.ContainsKey(territoryId))
        {
            return [];
        }

        return _territories[territoryId].Shape;
    }
}