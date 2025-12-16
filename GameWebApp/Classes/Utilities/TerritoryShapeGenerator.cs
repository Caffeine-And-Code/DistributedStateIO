using GameWebApp.Classes.Models;

namespace GameWebApp.Classes.Utilities;

public static class TerritoryShapeGenerator
{
    /// <summary>
    /// Genera una forma poligonale organica intorno al punto centrale.
    /// </summary>
    /// <param name="x">Coordinate X del centro</param>
    /// <param name="y">Coordinate Y del centro</param>
    /// <param name="radius">Raggio medio</param>
    /// <param name="irregularity">Random sulla forma</param>
    /// <returns>Lista di punti che descrivono il poligono</returns>
    public static List<ShapePoint> Generate(int x, int y, int radius = 55, int irregularity = 10)
    {
        var points = new List<ShapePoint>();
        const int steps = 12; // come nel JS

        var rand = new Random();

        for (var i = 0; i < steps; i++)
        {
            var angle = (Math.PI * 2 / steps) * i;

            var r = radius + (rand.NextDouble() * irregularity * 2 - irregularity);

            var px = (int)Math.Round(x + Math.Cos(angle) * r);
            var py = (int)Math.Round(y + Math.Sin(angle) * r);

            points.Add(new ShapePoint { X = px, Y = py });
        }

        return points;
    }
}