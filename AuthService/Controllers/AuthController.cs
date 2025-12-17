using AuthService.Entities;
using AuthService.Interfaces;
using Microsoft.AspNetCore.Mvc;
using AuthService.Entities.DTOs;

namespace AuthService.Controllers;

[ApiController]
[Route("api/auth")]
public class AuthController : ControllerBase
{
    private readonly IAuthService _service;

    public AuthController(IAuthService service)
    {
        _service = service;
    }

    [HttpPost("create")]
    public async Task<ActionResult<UserResponse>> Create([FromBody] UserDto dto)
    {
        var user = await _service.CreateUserAsync(dto.Username, dto.Password);

        var response = new UserResponse
        {
            Id = user.Id,
            Username = user.Username
        };

        // Se hai GetById implementato, usa CreatedAtAction
        return CreatedAtAction(nameof(GetById), new { id = response.Id }, response);
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id, [FromHeader(Name = "Authorization")] string? token)
    {
        if (string.IsNullOrWhiteSpace(token))
            return Unauthorized(new { message = "Missing token in Authorization header" });

        var ok = await _service.DeleteUserAsync(id, token);
        if (!ok) return Unauthorized(new { message = "Invalid token or user not found" });
        return NoContent();
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] UserDto dto)
    {
        var token = await _service.LoginAsync(dto.Username, dto.Password);
        if (token == null) return Unauthorized(new { message = "Invalid credentials" });
        return Ok(new { token });
    }

    [HttpPost("logout")]
    public async Task<IActionResult> Logout([FromHeader(Name = "Authorization")] string? token)
    {
        if (string.IsNullOrWhiteSpace(token)) return BadRequest(new { message = "Missing token in Authorization header" });
        await _service.LogoutAsync(token);
        return Ok();
    }

    [HttpGet("bytoken")]
    public async Task<IActionResult> GetByToken([FromHeader(Name = "Authorization")] string? token)
    {
        if (string.IsNullOrWhiteSpace(token)) return BadRequest(new { message = "Missing token in Authorization header" });

        var username = await _service.GetUsernameByTokenAsync(token);
        if (username == null) return Unauthorized(new { message = "Invalid token" });
        return Ok(new { username });
    }

    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetById(int id)
    {
        var username = await _service.GetUsernameByIdAsync(id);
        if (username == null) return NotFound(new { message = "User not found" });
        return Ok(new { username });
    }
}
