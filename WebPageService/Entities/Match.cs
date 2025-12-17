namespace WebPageService.Entities;

public class Match
{
    public int Id { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }

    public ICollection<MatchUser> Players { get; set; } = new List<MatchUser>();
}