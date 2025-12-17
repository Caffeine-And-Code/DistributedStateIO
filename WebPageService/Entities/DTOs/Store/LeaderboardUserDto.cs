namespace WebPageService.Entities.DTOs.Store;

public class LeaderboardUserDto
{
    public int UserId { get; set; }
    public int Points { get; set; }
    public List<UserMatchDto> LastMatches { get; set; } = new();
}