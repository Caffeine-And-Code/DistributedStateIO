using AuthService.Entities;

namespace AuthService.Interfaces;

public interface IAuthRepository
{
    Task<User?> GetByUsernameAsync(string username);
    Task<User?> GetByIdAsync(int id);
    Task<User?> GetByTokenAsync(string token);
    Task<int?> GetUserIdByTokenAsync(string token);
    Task CreateAsync(User user);
    Task UpdateAsync(User user);
    Task DeleteAsync(User user);
}