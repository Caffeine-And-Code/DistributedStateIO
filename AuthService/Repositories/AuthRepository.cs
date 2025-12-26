using Microsoft.EntityFrameworkCore;
using AuthService.Data;
using AuthService.Interfaces;
using Domain.Services.Auth.Entities;

namespace AuthService.Repositories;

public class AuthRepository : IAuthRepository
{
    private readonly AuthDbContext _db;

    public AuthRepository(AuthDbContext db)
    {
        _db = db;
    }

    public async Task<User?> GetByUsernameAsync(string username)
    {
        return await _db.Users.FirstOrDefaultAsync(u => u.Username == username);
    }

    public async Task<User?> GetByIdAsync(int id)
    {
        return await _db.Users.FindAsync(id);
    }

    public async Task<User?> GetByTokenAsync(string token)
    {
        if (string.IsNullOrWhiteSpace(token)) return null;
        return await _db.Users.FirstOrDefaultAsync(u => u.Token == token);
    }
    
    public async Task<int?> GetUserIdByTokenAsync(string token)
    {
        var user = await _db.Users
            .FirstOrDefaultAsync(u => u.Token == token);

        return user?.Id;
    }

    public async Task CreateAsync(User user)
    {
        await _db.Users.AddAsync(user);
        await _db.SaveChangesAsync();
    }

    public async Task UpdateAsync(User user)
    {
        _db.Users.Update(user);
        await _db.SaveChangesAsync();
    }

    public async Task DeleteAsync(User user)
    {
        _db.Users.Remove(user);
        await _db.SaveChangesAsync();
    }
}
