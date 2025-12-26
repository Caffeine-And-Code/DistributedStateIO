namespace Domain.Services.Store.DTOs;

public class CreateMatchDto
{
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public List<int> PlayerIds { get; set; } = new();
    public int WinnerId { get; set; }
}