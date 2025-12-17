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
    public async Task<UserResponse> Register([FromBody] UserDto dto)
    {
        var user = await _service.RegisterAsync(dto);
        var res = new UserResponse();
        res.Id = user.Id;
        res.Username = user.Username;
        return res;
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

    [HttpDelete("users/{id:int}")]
    public async Task<IActionResult> DeleteUser(int id, [FromHeader(Name = "Authorization")] string? auth)
    {
        if (string.IsNullOrWhiteSpace(auth)) return Unauthorized();
        var token = auth.StartsWith("Bearer ") ? auth.Substring(7) : auth;
        var ok = await _service.DeleteUserAsync(id, token);
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
        var result = await _service.GetLeaderboardAsync(topN, lastM);
        return Ok(result);
    }

    [HttpGet("users/{userId:int}/matches")]
    public async Task<IActionResult> UserMatches(int userId, [FromQuery] int lastN = 5)
    {
        var result = await _service.GetUserLastMatchesAsync(userId, lastN);
        return Ok(result);
    }
}
