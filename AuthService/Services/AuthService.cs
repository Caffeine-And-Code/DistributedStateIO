using System.Security.Cryptography;
using System.Text;
using AuthService.Entities;
using AuthService.Interfaces;

namespace AuthService.Services;

public class AuthService : IAuthService
{
    private readonly IAuthRepository _repo;

    public AuthService(IAuthRepository repo)
    {
        _repo = repo;
    }

    // SHA256
    private static string HashPassword(string plain)
    {
        using var sha = SHA256.Create();
        var bytes = Encoding.UTF8.GetBytes(plain);
        var hashed = sha.ComputeHash(bytes);
        return Convert.ToHexString(hashed); 
    }

    public async Task<User> CreateUserAsync(string username, string passwordPlain)
    {
        var existing = await _repo.GetByUsernameAsync(username);
        if (existing != null)
            throw new InvalidOperationException("Username already exists.");

        var user = new User
        {
            Username = username,
            Password = HashPassword(passwordPlain),
            Token = null
        };

        return await _repo.CreateUserAsync(user);
    }

    public async Task<bool> DeleteUserAsync(int id, string token)
    {
        var user = await _repo.GetByIdAsync(id);
        if (user == null) return false;
        if (user.Token != token) return false;

        await _repo.DeleteAsync(user);
        return true;
    }

    public async Task<string?> LoginAsync(string username, string passwordPlain)
    {
        var user = await _repo.GetByUsernameAsync(username);
        if (user == null) return null;

        var hash = HashPassword(passwordPlain);
        if (!string.Equals(user.Password, hash, StringComparison.OrdinalIgnoreCase))
            return null;

        user.Token = $"{user.Id}-{Guid.NewGuid()}";
        await _repo.UpdateAsync(user);
        return user.Token;
    }

    public async Task LogoutAsync(string token)
    {
        var user = await _repo.GetByTokenAsync(token);
        if (user == null) return;
        user.Token = null;
        await _repo.UpdateAsync(user);
    }

    public async Task<string?> GetUsernameByTokenAsync(string token)
    {
        var user = await _repo.GetByTokenAsync(token);
        return user?.Username;
    }

    public async Task<string?> GetUsernameByIdAsync(int id)
    {
        var user = await _repo.GetByIdAsync(id);
        return user?.Username;
    }
}
