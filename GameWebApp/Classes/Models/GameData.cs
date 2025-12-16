using GameWebApp.Classes.Utilities;

namespace GameWebApp.Classes.Models;

public static class GameData
{
    public static readonly List<Territory> Territories =
    [
        new() { Id = "1", X = 150, Y = 200, Owner = 0, Troops = 20, Neighbors = ["2", "4"] },
        new() { Id = "2", X = 320, Y = 120, Owner = null, Troops = 10, Neighbors = ["1", "3", "5"] },
        new() { Id = "3", X = 500, Y = 150, Owner = 1, Troops = 20, Neighbors = ["2", "5", "6"] },
        new() { Id = "4", X = 200, Y = 380, Owner = null, Troops = 15, Neighbors = ["1", "7", "11"] },
        new() { Id = "5", X = 500, Y = 320, Owner = null, Troops = 8, Neighbors = ["2", "3", "6", "8"] },
        new() { Id = "6", X = 700, Y = 180, Owner = 2, Troops = 20, Neighbors = ["3", "5", "9", "15"] },
        new() { Id = "7", X = 350, Y = 500, Owner = null, Troops = 12, Neighbors = ["4", "11", "12"] },
        new() { Id = "8", X = 650, Y = 400, Owner = null, Troops = 10, Neighbors = ["5", "9", "12", "13"] },
        new() { Id = "9", X = 880, Y = 300, Owner = 3, Troops = 20, Neighbors = ["6", "8", "10", "13"] },
        new() { Id = "10", X = 1050, Y = 250, Owner = null, Troops = 15, Neighbors = ["9", "14", "16"] },
        new() { Id = "11", X = 150, Y = 550, Owner = 4, Troops = 20, Neighbors = ["4", "7"] },
        new() { Id = "12", X = 550, Y = 550, Owner = null, Troops = 12, Neighbors = ["7", "8", "13"] },
        new() { Id = "13", X = 800, Y = 520, Owner = 5, Troops = 20, Neighbors = ["8", "9", "12", "14"] },
        new() { Id = "14", X = 1050, Y = 450, Owner = null, Troops = 10, Neighbors = ["10", "13", "16"] },
        new() { Id = "15", X = 850, Y = 100, Owner = 6, Troops = 20, Neighbors = ["6", "16"] },
        new() { Id = "16", X = 1050, Y = 100, Owner = 7, Troops = 20, Neighbors = ["10", "14", "15"] }
    ];

    public static void InitializeShapes()
    {
        foreach (var t in Territories)
        {
            t.Shape = TerritoryShapeGenerator.Generate(t.X, t.Y);
        }
    }

    public static List<Attack> Attacks = new(); // vuoto all’avvio
}