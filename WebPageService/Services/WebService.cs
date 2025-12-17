using WebPageService.Entities;
using WebPageService.Entities.DTOs;
using WebPageService.Entities.DTOs.Auth;
using WebPageService.Entities.DTOs.Store;
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

    public Task<UserResponse> RegisterAsync(UserDto dto) => _authRepo.CreateUserAsync(dto);
    public Task<string?> LoginAsync(UserDto dto) => _authRepo.LoginAsync(dto);
    public Task<bool> DeleteUserAsync(int id, string token) => _authRepo.DeleteUserAsync(id, token);
    public Task LogoutAsync(string token) => _authRepo.LogoutAsync(token);
    public Task<string?> GetUsernameByTokenAsync(string token) => _authRepo.GetUsernameByTokenAsync(token);
    public Task<string?> GetUsernameByIdAsync(int id) => _authRepo.GetUsernameByIdAsync(id);

    public Task<IEnumerable<LeaderboardUserDto>> GetLeaderboardAsync(int topN, int lastM) => _storeRepo.GetLeaderboardAsync(topN, lastM);
    public Task<IEnumerable<UserMatchDto>> GetUserLastMatchesAsync(int userId, int lastN) => _storeRepo.GetUserLastMatchesAsync(userId, lastN);
}