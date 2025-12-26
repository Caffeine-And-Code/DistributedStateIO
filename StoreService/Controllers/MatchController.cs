using Domain.Services.Store.DTOs;
using Microsoft.AspNetCore.Mvc;
using StoreService.Interfaces;

namespace StoreService.Controllers;

[ApiController]
[Route("api/matches")]
public class MatchController : ControllerBase
{
    private readonly IMatchService _service;
    private readonly string _token;

    public MatchController(IMatchService service, IConfiguration configuration)
    {
        _service = service;
        _token = configuration["Security:MatchCreationToken"]
                ?? throw new InvalidOperationException("MatchCreationToken not configured");
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateMatchDto dto, [FromHeader(Name = "Authorization")] string? token)
    {
        if (string.IsNullOrWhiteSpace(token) || token != _token)
        {
            return BadRequest(new { message = "Missing token in Authorization header" });
        }
        var match = await _service.CreateMatchAsync(dto.StartDate, dto.EndDate, dto.PlayerIds, dto.WinnerId);
        return Ok(new { match.Id });
    }

    [HttpGet("leaderboard")]
    public async Task<IActionResult> Leaderboard([FromQuery] int topN = 10, [FromQuery] int lastM = 5)
    {
        var result = await _service.GetLeaderboardAsync(topN, lastM);
        return Ok(result);
    }

    [HttpGet("user/{userId}/last")]
    public async Task<IActionResult> UserLastMatches(int userId, [FromQuery] int lastN = 5)
    {
        var result = await _service.GetUserLastMatchesAsync(userId, lastN);
        return Ok(result);
    }
}