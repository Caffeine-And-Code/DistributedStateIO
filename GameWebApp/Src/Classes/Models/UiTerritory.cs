using Domain.Game;

namespace GameWebApp.Classes.Models;

public class UiTerritory : Territory
{
    public List<ShapePoint> Shape { get; set; } = [];

    public Territory ToTerritory()
    {
        return this;
    }
}