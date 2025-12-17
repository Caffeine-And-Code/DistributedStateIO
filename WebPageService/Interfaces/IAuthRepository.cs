using Microsoft.AspNetCore.Mvc;
using WebPageService.Entities;
using WebPageService.Entities.DTOs;
using WebPageService.Entities.DTOs.Auth;

namespace WebPageService.Interfaces;

public interface IAuthRepository
{
    Task<UserResponse> CreateUserAsync(UserDto dto);
    Task<bool> DeleteUserAsync(int id, string token);
    Task<string?> LoginAsync(UserDto dto);
    Task LogoutAsync(string token);
    Task<string?> GetUsernameByTokenAsync(string token);
    Task<string?> GetUsernameByIdAsync(int id);
}