using Domain.Services.Store.Entities;
using Microsoft.EntityFrameworkCore;
using StoreService.Data;
using StoreService.Interfaces;

namespace StoreService.Repositories;

public class MatchRepository : IMatchRepository
{
    private readonly StoreDbContext _context;
    public MatchRepository(StoreDbContext context) => _context = context;

    public async Task AddAsync(Match match)
    {
        await _context.Matches.AddAsync(match);
    }

    public async Task SaveChangesAsync() => await _context.SaveChangesAsync();

    public async Task<List<Match>> GetAllMatchesWithPlayersAsync()
    {
        return await _context.Matches
            .Include(m => m.Players)
            .AsNoTracking()
            .ToListAsync();
    }

    public async Task<List<Match>> GetMatchesByUserAsync(int userId)
    {
        return await _context.Matches
            .Where(m => m.Players.Any(p => p.UserId == userId))
            .Include(m => m.Players)
            .AsNoTracking()
            .ToListAsync();
    }
}
