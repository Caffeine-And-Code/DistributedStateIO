namespace StoreService.Entities.DTOs;

public class UserMatchDto
{
    public int MatchId { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public bool IsWinner { get; set; }
    public int Points { get; set; }
}