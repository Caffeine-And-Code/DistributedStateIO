using Microsoft.AspNetCore.Mvc;
using WebPageService.Entities;
using WebPageService.Entities.DTOs;
using WebPageService.Entities.DTOs.Auth;
using WebPageService.Entities.DTOs.Store;
using WebPageService.Interfaces;

namespace WebPageService.Controllers;

[ApiController]
[Route("api")]
public class WebController : ControllerBase
{
    private readonly IWebService _service;

    public WebController(IWebService service) => _service = service;

    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] UserDto dto)
    {
        try
        {
            var message = await _service.RegisterAsync(dto);
            return Ok(new { message });
        }
        catch (InvalidOperationException ex)
        {
            return Conflict(new { message = ex.Message });
        }
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] UserDto dto)
    {
        var token = await _service.LoginAsync(dto);
        if (token == null) return Unauthorized();
        return Ok(new { token });
    }

    [HttpPost("logout")]
    public async Task<IActionResult> Logout([FromHeader(Name = "Authorization")] string? auth)
    {
        if (string.IsNullOrWhiteSpace(auth)) return BadRequest();
        var token = auth.StartsWith("Bearer ") ? auth.Substring(7) : auth;
        await _service.LogoutAsync(token);
        return Ok();
    }

    [HttpDelete("users")]
    public async Task<IActionResult> DeleteUser([FromHeader(Name = "Authorization")] string? auth)
    {
        if (string.IsNullOrWhiteSpace(auth)) return Unauthorized();
        var token = auth.StartsWith("Bearer ") ? auth.Substring(7) : auth;
        var ok = await _service.DeleteUserAsync(token);
        if (!ok) return Unauthorized();
        return NoContent();
    }

    [HttpGet("me")]
    public async Task<IActionResult> Me([FromHeader(Name = "Authorization")] string? auth)
    {
        if (string.IsNullOrWhiteSpace(auth)) return BadRequest();
        var token = auth.StartsWith("Bearer ") ? auth.Substring(7) : auth;
        var username = await _service.GetUsernameByTokenAsync(token);
        if (username == null) return Unauthorized();
        return Ok(new { username });
    }

    [HttpGet("users/{id:int}")]
    public async Task<IActionResult> GetUserById(int id)
    {
        var username = await _service.GetUsernameByIdAsync(id);
        if (username == null) return NotFound();
        return Ok(new { id, username });
    }

    [HttpGet("leaderboard")]
    public async Task<IActionResult> Leaderboard([FromQuery] int topN = 10, [FromQuery] int lastM = 5)
    {
        var result = await _service.GetLeaderboardViewAsync(topN, lastM);
        return Ok(result);
    }

    [HttpGet("me/matches")]
    public async Task<IActionResult> MyMatches(
        [FromHeader(Name = "Authorization")] string? auth,
        [FromQuery] int lastN = 5)
    {
        if (string.IsNullOrWhiteSpace(auth))
            return Unauthorized();

        var token = auth.StartsWith("Bearer ")
            ? auth.Substring(7)
            : auth;

        var matches = await _service.GetMyLastMatchesAsync(token, lastN);
        return Ok(matches);
    }
}
