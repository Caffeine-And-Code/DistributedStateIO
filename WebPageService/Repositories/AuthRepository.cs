using System.Net.Http.Json;
using WebPageService.Entities.DTOs.Auth;
using WebPageService.Interfaces;
using System.Text.Json;
using Microsoft.AspNetCore.Mvc;
using WebPageService.Entities;
using WebPageService.Entities.DTOs;

namespace WebPageService.Repositories;

public class AuthRepository : IAuthRepository
{
    private readonly HttpClient _http;
    private readonly JsonSerializerOptions _jsonOpts = new() { PropertyNameCaseInsensitive = true };

    public AuthRepository(HttpClient http) => _http = http;

    public async Task<UserResponse> CreateUserAsync(UserDto dto)
    {
        var res = await _http.PostAsJsonAsync("create", dto);
        res.EnsureSuccessStatusCode();
        var created = await res.Content.ReadFromJsonAsync<UserResponse>(_jsonOpts);

        if (created == null)
            throw new InvalidOperationException("Auth service returned empty response.");

        return created;
    }

    public async Task<bool> DeleteUserAsync(int id, string token)
    {
        var req = new HttpRequestMessage(HttpMethod.Delete, $"{id}");
        req.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
        var res = await _http.SendAsync(req);
        return res.IsSuccessStatusCode;
    }

    public async Task<string?> LoginAsync(UserDto dto)
    {
        var res = await _http.PostAsJsonAsync("login", dto);
        if (!res.IsSuccessStatusCode) return null;

        // Expecting { token: "..." }
        var doc = await res.Content.ReadFromJsonAsync<JsonElement?>(_jsonOpts);
        if (doc.HasValue)
        {
            if (doc.Value.ValueKind == JsonValueKind.Object && doc.Value.TryGetProperty("token", out var t)) return t.GetString();
            if (doc.Value.ValueKind == JsonValueKind.String) return doc.Value.GetString();
        }

        var s = await res.Content.ReadAsStringAsync();
        return string.IsNullOrWhiteSpace(s) ? null : s.Trim('"');
    }

    public async Task LogoutAsync(string token)
    {
        var req = new HttpRequestMessage(HttpMethod.Post, "logout");
        req.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
        var res = await _http.SendAsync(req);
        res.EnsureSuccessStatusCode();
    }

    public async Task<string?> GetUsernameByTokenAsync(string token)
    {
        var req = new HttpRequestMessage(HttpMethod.Get, "bytoken");
        req.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
        var res = await _http.SendAsync(req);
        if (!res.IsSuccessStatusCode) return null;

        var doc = await res.Content.ReadFromJsonAsync<JsonElement?>(_jsonOpts);
        if (doc.HasValue && doc.Value.TryGetProperty("username", out var u)) return u.GetString();
        return null;
    }

    public async Task<string?> GetUsernameByIdAsync(int id)
    {
        var res = await _http.GetAsync($"{id}");
        if (!res.IsSuccessStatusCode) return null;

        var doc = await res.Content.ReadFromJsonAsync<JsonElement?>(_jsonOpts);
        if (doc.HasValue && doc.Value.TryGetProperty("username", out var u)) return u.GetString();
        return null;
    }
}
