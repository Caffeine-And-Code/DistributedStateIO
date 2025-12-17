using System.Net.Http.Json;
using WebPageService.Entities.DTOs.Store;
using WebPageService.Interfaces;
using System.Text.Json;

namespace WebPageService.Repositories;

public class StoreRepository : IStoreRepository
{
    private readonly HttpClient _http;
    private readonly JsonSerializerOptions _jsonOpts = new() { PropertyNameCaseInsensitive = true };

    public StoreRepository(HttpClient http) => _http = http;

    public async Task<IEnumerable<LeaderboardUserDto>> GetLeaderboardAsync(int topN, int lastM)
    {
        var res = await _http.GetAsync($"leaderboard?topN={topN}&lastM={lastM}");
        res.EnsureSuccessStatusCode();
        var arr = await res.Content.ReadFromJsonAsync<IEnumerable<LeaderboardUserDto>>(_jsonOpts);
        return arr ?? Enumerable.Empty<LeaderboardUserDto>();
    }

    public async Task<IEnumerable<UserMatchDto>> GetUserLastMatchesAsync(int userId, int lastN)
    {
        var res = await _http.GetAsync($"user/{userId}/last?lastN={lastN}");
        res.EnsureSuccessStatusCode();
        var arr = await res.Content.ReadFromJsonAsync<IEnumerable<UserMatchDto>>(_jsonOpts);
        return arr ?? Enumerable.Empty<UserMatchDto>();
    }
}