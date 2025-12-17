using AuthService.Entities;

namespace AuthService.Interfaces;

public interface IAuthService
{
    Task<User> CreateUserAsync(string username, string passwordPlain);
    Task<bool> DeleteUserAsync(int id, string token);
    Task<string?> LoginAsync(string username, string passwordPlain);
    Task LogoutAsync(string token);
    Task<string?> GetUsernameByTokenAsync(string token);
    Task<string?> GetUsernameByIdAsync(int id);
}