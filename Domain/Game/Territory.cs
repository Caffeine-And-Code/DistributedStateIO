namespace Domain.Game;

public class Territory
{
    public required Guid Id { get; set; }
    public required int X { get; set; }
    public required int Y { get; set; }
    public required int Troops { get; set; }
    public int? OwnerId { get; set; }

    /// <summary>
    /// Checks if another territory is an ally of this territory
    /// </summary>
    /// <param name="other"></param>
    /// <returns></returns>
    public bool IsAlly(Territory other)
    {
        return other.OwnerId == OwnerId;
    }
}