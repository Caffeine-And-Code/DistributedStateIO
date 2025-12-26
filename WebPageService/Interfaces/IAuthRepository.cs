using Domain.Services.Auth.DTOs;

namespace WebPageService.Interfaces;

public interface IAuthRepository
{
    Task<string> CreateUserAsync(UserDto dto);
    Task<bool> DeleteUserAsync(string token);
    Task<string?> LoginAsync(UserDto dto);
    Task LogoutAsync(string token);
    Task<string?> GetUsernameByTokenAsync(string token);
    Task<int?> GetUserIdByTokenAsync(string token);
    Task<string?> GetUsernameByIdAsync(int id);
}