using Microsoft.EntityFrameworkCore;
using AuthService.Data;
using AuthService.Entities;
using AuthService.Interfaces;

namespace AuthService.Repositories;

public class AuthRepository : IAuthRepository
{
    private readonly AuthDbContext _db;

    public AuthRepository(AuthDbContext db)
    {
        _db = db;
    }

    public async Task<User> CreateUserAsync(User user)
    {
        _db.Users.Add(user);
        await _db.SaveChangesAsync();
        return user;
    }

    public async Task<User?> GetByIdAsync(int id)
    {
        return await _db.Users.FindAsync(id);
    }

    public async Task<User?> GetByUsernameAsync(string username)
    {
        return await _db.Users.SingleOrDefaultAsync(u => u.Username == username);
    }

    public async Task<User?> GetByTokenAsync(string token)
    {
        return await _db.Users.SingleOrDefaultAsync(u => u.Token == token);
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
