using System.Net.Http.Headers;
using WebPageService.Interfaces;
using System.Text.Json;
using Domain.Services.Auth.DTOs;

namespace WebPageService.Repositories;

public class AuthRepository : IAuthRepository
{
    private readonly HttpClient _http;
    private readonly JsonSerializerOptions _jsonOpts = new() { PropertyNameCaseInsensitive = true };

    public AuthRepository(HttpClient http) => _http = http;

    public async Task<string> CreateUserAsync(UserDto dto)
    {
        var res = await _http.PostAsJsonAsync("create", dto);

        var json = await res.Content.ReadFromJsonAsync<JsonElement>(_jsonOpts);

        if (!res.IsSuccessStatusCode)
        {
            var msg = json.TryGetProperty("message", out var m)
                ? m.GetString()
                : "User creation failed";

            throw new InvalidOperationException(msg);
        }

        if (json.TryGetProperty("message", out var message))
            return message.GetString() ?? "User created";

        return "User created";
    }

    public async Task<bool> DeleteUserAsync(string token)
    {
        var req = new HttpRequestMessage(HttpMethod.Delete, "");
        req.Headers.Add("Authorization", token);

        var res = await _http.SendAsync(req);
        return res.IsSuccessStatusCode;
    }

    public async Task<string?> LoginAsync(UserDto dto)
    {
        var res = await _http.PostAsJsonAsync("login", dto);
        if (!res.IsSuccessStatusCode) return null;

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
        req.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue(token);
        var res = await _http.SendAsync(req);
        res.EnsureSuccessStatusCode();
    }

    public async Task<string?> GetUsernameByTokenAsync(string token)
    {
        var req = new HttpRequestMessage(HttpMethod.Get, "bytoken");
        req.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue(token);
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
    
    public async Task<int?> GetUserIdByTokenAsync(string token)
    {
        var req = new HttpRequestMessage(HttpMethod.Get, "bytoken/id");
        req.Headers.Authorization =
            new AuthenticationHeaderValue("Bearer", token);

        var res = await _http.SendAsync(req);
        if (!res.IsSuccessStatusCode)
            return null;

        var json = await res.Content.ReadFromJsonAsync<JsonElement>();
        if (json.TryGetProperty("userId", out var id))
            return id.GetInt32();

        return null;
    }
}
