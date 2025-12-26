using Domain.Services.Store.Entities;

namespace StoreService.Interfaces;

public interface IMatchRepository
{
    Task AddAsync(Match match);
    Task<List<Match>> GetAllMatchesWithPlayersAsync();
    Task<List<Match>> GetMatchesByUserAsync(int userId);
    Task SaveChangesAsync();
}