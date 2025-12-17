using AuthService.Entities;

namespace AuthService.Interfaces;

public interface IAuthRepository
{
    Task<User> CreateUserAsync(User user);
    Task<User?> GetByIdAsync(int id);
    Task<User?> GetByUsernameAsync(string username);
    Task<User?> GetByTokenAsync(string token);
    Task UpdateAsync(User user);
    Task DeleteAsync(User user);
}