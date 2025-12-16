namespace GameWebApp.Classes.Models;

public class Territory
{
    public string Id { get; set; } = "";
    public int X { get; set; }
    public int Y { get; set; }
    public int? Owner { get; set; }   // null = neutral
    public int Troops { get; set; }
    public List<string> Neighbors { get; set; } = new();
    public List<ShapePoint> Shape { get; set; } = new(); // verrà riempito via JS o utilità C#
}