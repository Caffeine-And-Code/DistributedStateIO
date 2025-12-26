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
    public async Task<IActionResult> Create([FromBody] UserDto dto)
    {
        if (string.IsNullOrWhiteSpace(dto.Username) || string.IsNullOrWhiteSpace(dto.Password))
            return BadRequest(new { message = "Missing username or password" });

        try
        {
            await _service.CreateUserAsync(dto.Username, dto.Password);
            return Created(string.Empty, new { message = "User created successfully" });
        }
        catch (InvalidOperationException ex)
        {
            return Conflict(new { message = ex.Message });
        }
    }
    
    [HttpDelete]
    public async Task<IActionResult> DeleteByToken([FromHeader(Name = "Authorization")] string? token)
    {
        if (string.IsNullOrWhiteSpace(token))
            return BadRequest(new { message = "Missing token in Authorization header" });

        var deleted = await _service.DeleteByTokenAsync(token);
        if (!deleted) return Unauthorized(new { message = "Invalid token or user not found" });

        return Ok(new { message = "User deleted" });
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
    
    [HttpGet("bytoken/id")]
    public async Task<IActionResult> GetUserIdByToken(
        [FromHeader(Name = "Authorization")] string? auth)
    {
        if (string.IsNullOrWhiteSpace(auth))
            return Unauthorized();

        var token = auth.StartsWith("Bearer ")
            ? auth.Substring(7)
            : auth;

        var userId = await _service.GetUserIdByTokenAsync(token);

        if (userId == null)
            return Unauthorized();

        return Ok(new { userId });
    }

    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetById(int id)
    {
        var username = await _service.GetUsernameByIdAsync(id);
        if (username == null) return NotFound(new { message = "User not found" });
        return Ok(new { username });
    }
}
