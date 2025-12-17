using WebPageService.Entities.DTOs.Store;

namespace WebPageService.Interfaces;

public interface IStoreRepository
{
    Task<IEnumerable<LeaderboardUserDto>> GetLeaderboardAsync(int topN, int lastM);
    Task<IEnumerable<UserMatchDto>> GetUserLastMatchesAsync(int userId, int lastN);
}