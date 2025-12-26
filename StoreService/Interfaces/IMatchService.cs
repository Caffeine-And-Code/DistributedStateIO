using Domain.Services.Store.DTOs;
using Domain.Services.Store.Entities;

namespace StoreService.Interfaces;

public interface IMatchService
{
    Task<Match> CreateMatchAsync(DateTime start, DateTime end, List<int> playerIds, int winnerId);
    Task<List<LeaderboardUserDto>> GetLeaderboardAsync(int topN, int lastMMatches);
    Task<List<UserMatchDto>> GetUserLastMatchesAsync(int userId, int lastN);
}