namespace Domain.Game;

public class Territory
{
    public required Guid Id { get; set; }
    public required int X { get; set; }
    public required int Y { get; set; }
    public required int Troops { get; set; }
    public int? OwnerId { get; set; }
}