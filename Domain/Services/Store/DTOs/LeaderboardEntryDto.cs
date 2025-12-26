namespace Domain.Services.Store.DTOs;

public class LeaderboardEntryDto
{
    public int Position { get; set; }
    public int UserId { get; set; }
    public string Username { get; set; } = string.Empty;
    public int Points { get; set; }
    public IEnumerable<UserMatchDto> LastMatches { get; set; } = [];
}