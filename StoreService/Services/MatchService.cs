using StoreService.Entities;
using StoreService.Entities.DTOs;
using StoreService.Interfaces;

namespace StoreService.Services;

public class MatchService : IMatchService
{
    private readonly IMatchRepository _repo;

    public MatchService(IMatchRepository repo) => _repo = repo;

    public async Task<Match> CreateMatchAsync(DateTime start, DateTime end, List<int> playerIds, int winnerId)
    {
        var match = new Match
        {
            StartDate = start,
            EndDate = end
        };

        foreach (var id in playerIds)
        {
            match.Players.Add(new MatchUser
            {
                UserId = id,
                IsWinner = (id == winnerId)
            });
        }

        if (!playerIds.Contains(winnerId))
        {
            match.Players.Add(new MatchUser
            {
                UserId = winnerId,
                IsWinner = true
            });
        }

        await _repo.AddAsync(match);
        await _repo.SaveChangesAsync();

        return match;
    }

    public async Task<List<LeaderboardUserDto>> GetLeaderboardAsync(int topN, int lastMMatches)
    {
        var allMatches = await _repo.GetAllMatchesWithPlayersAsync();

        var users = allMatches
            .SelectMany(m => m.Players.Select(p => new { p.UserId, p.IsWinner, Match = m }))
            .GroupBy(x => x.UserId)
            .Select(g => new LeaderboardUserDto
            {
                UserId = g.Key,
                Points = g.Sum(x => x.IsWinner ? 2 : -1),
                LastMatches = g
                    .OrderByDescending(x => x.Match.EndDate)
                    .Take(lastMMatches)
                    .Select(x => new UserMatchDto
                    {
                        MatchId = x.Match.Id,
                        StartDate = x.Match.StartDate,
                        EndDate = x.Match.EndDate,
                        IsWinner = x.IsWinner,
                        Points = x.IsWinner ? 2 : -1
                    })
                    .ToList()
            })
            .OrderByDescending(x => x.Points)
            .Take(topN)
            .ToList();

        return users;
    }

    public async Task<List<UserMatchDto>> GetUserLastMatchesAsync(int userId, int lastN)
    {
        var matches = await _repo.GetMatchesByUserAsync(userId);

        var result = matches
            .OrderByDescending(m => m.EndDate)
            .Take(lastN)
            .Select(m =>
            {
                var pu = m.Players.First(p => p.UserId == userId);
                return new UserMatchDto
                {
                    MatchId = m.Id,
                    StartDate = m.StartDate,
                    EndDate = m.EndDate,
                    IsWinner = pu.IsWinner,
                    Points = pu.IsWinner ? 2 : -1
                };
            })
            .ToList();

        return result;
    }
}
