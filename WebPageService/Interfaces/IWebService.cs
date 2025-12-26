using WebPageService.Entities;
using WebPageService.Entities.DTOs;
using WebPageService.Entities.DTOs.Auth;
using WebPageService.Entities.DTOs.Store;

namespace WebPageService.Interfaces;

public interface IWebService
{
    // Auth
    Task<string> RegisterAsync(UserDto dto);
    Task<string?> LoginAsync(UserDto dto);
    Task<bool> DeleteUserAsync(string token);
    Task LogoutAsync(string token);
    Task<string?> GetUsernameByTokenAsync(string token);
    Task<string?> GetUsernameByIdAsync(int id);

    // Store
    Task<IEnumerable<LeaderboardUserDto>> GetLeaderboardAsync(int topN, int lastM);
    Task<IEnumerable<UserMatchDto>> GetUserLastMatchesAsync(int userId, int lastN);
    
    Task<IEnumerable<LeaderboardEntryDto>> GetLeaderboardViewAsync(int topN, int lastM);
    Task<IEnumerable<UserMatchDto>> GetMyLastMatchesAsync(string token, int lastN);

}