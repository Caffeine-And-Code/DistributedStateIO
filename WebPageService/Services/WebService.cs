using Domain.Services.Auth.DTOs;
using Domain.Services.Store.DTOs;
using WebPageService.Interfaces;

namespace WebPageService.Services;

public class WebService : IWebService
{
    private readonly IAuthRepository _authRepo;
    private readonly IStoreRepository _storeRepo;

    public WebService(IAuthRepository authRepo, IStoreRepository storeRepo)
    {
        _authRepo = authRepo;
        _storeRepo = storeRepo;
    }

    public Task<string> RegisterAsync(UserDto dto) => _authRepo.CreateUserAsync(dto);
    public Task<string?> LoginAsync(UserDto dto) => _authRepo.LoginAsync(dto);
    public Task<bool> DeleteUserAsync(string token) => _authRepo.DeleteUserAsync(token);
    public Task LogoutAsync(string token) => _authRepo.LogoutAsync(token);
    public Task<string?> GetUsernameByTokenAsync(string token) => _authRepo.GetUsernameByTokenAsync(token);
    public Task<string?> GetUsernameByIdAsync(int id) => _authRepo.GetUsernameByIdAsync(id);

    public Task<IEnumerable<LeaderboardUserDto>> GetLeaderboardAsync(int topN, int lastM) => _storeRepo.GetLeaderboardAsync(topN, lastM);
    public Task<IEnumerable<UserMatchDto>> GetUserLastMatchesAsync(int userId, int lastN) => _storeRepo.GetUserLastMatchesAsync(userId, lastN);
    
    public async Task<IEnumerable<LeaderboardEntryDto>> GetLeaderboardViewAsync(int topN, int lastM)
    {
        var raw = await _storeRepo.GetLeaderboardAsync(topN, lastM);

        var result = new List<LeaderboardEntryDto>();
        var position = 1;

        foreach (var entry in raw)
        {
            var username = await _authRepo.GetUsernameByIdAsync(entry.UserId)
                           ?? "Unknown";

            result.Add(new LeaderboardEntryDto
            {
                Position = position++,
                UserId = entry.UserId,
                Username = username,
                Points = entry.Points,
                LastMatches = entry.LastMatches
            });
        }

        return result;
    }
    
    public async Task<IEnumerable<UserMatchDto>> GetMyLastMatchesAsync(string token, int lastN)
    {
        var userId = await _authRepo.GetUserIdByTokenAsync(token);

        if (userId == null)
            throw new UnauthorizedAccessException();

        return await _storeRepo.GetUserLastMatchesAsync(userId.Value, lastN);
    }

}