namespace AuthService.Interfaces;

public interface IAuthService
{
    Task CreateUserAsync(string username, string passwordPlain);
    Task<bool> DeleteByTokenAsync(string token);
    Task<string?> LoginAsync(string username, string passwordPlain);
    Task LogoutAsync(string token);
    Task<string?> GetUsernameByTokenAsync(string token);
    Task<int?> GetUserIdByTokenAsync(string token);
    Task<string?> GetUsernameByIdAsync(int id);
}