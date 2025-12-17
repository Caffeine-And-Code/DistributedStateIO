namespace WebPageService.Entities;

public class MatchUser
{
    public int Id { get; set; }

    public int MatchId { get; set; }
    public Match Match { get; set; } = null!;

    public int UserId { get; set; }

    public bool IsWinner { get; set; }
}