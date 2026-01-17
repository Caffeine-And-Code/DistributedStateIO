using Domain.Game;
using GameWebApp.Classes.Models;

namespace GameWebApp.Classes.Utilities.TerritoriesShape;

public sealed class TerritoryShapeProvider
{
    private static readonly Lock Padlock = new();

    private TerritoryShapeProvider()
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
            var uiTerritory = _territories[territory.Id];
            uiTerritory.Troops = territory.Troops;
            uiTerritory.OwnerId = territory.OwnerId;
        }

        return _territories.Values;
    }
}