using WebPageService.Entities;
using WebPageService.Entities.DTOs;
using WebPageService.Entities.DTOs.Auth;
using WebPageService.Entities.DTOs.Store;

namespace WebPageService.Interfaces;

public interface IWebService
{
    // Auth
    Task<UserResponse> RegisterAsync(UserDto dto);
    Task<string?> LoginAsync(UserDto dto);
    Task<bool> DeleteUserAsync(int id, string token);
    Task LogoutAsync(string token);
    Task<string?> GetUsernameByTokenAsync(string token);
    Task<string?> GetUsernameByIdAsync(int id);

    // Store
    Task<IEnumerable<LeaderboardUserDto>> GetLeaderboardAsync(int topN, int lastM);
    Task<IEnumerable<UserMatchDto>> GetUserLastMatchesAsync(int userId, int lastN);

}