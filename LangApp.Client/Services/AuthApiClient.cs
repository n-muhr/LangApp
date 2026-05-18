using System.Net;
using System.Net.Http.Json;
using System.Text.Json;
using LangApp.Client.Models.Auth;

namespace LangApp.Client.Services;

public class AuthApiClient : IAuthApiClient
{
    private readonly HttpClient _httpClient;

    public AuthApiClient(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<AuthApiResult<LoginResponse>> LoginAsync(LoginRequest request)
    {
        var apiRequest = new { request.Email, request.Password };

        HttpResponseMessage response;
        try
        {
            response = await _httpClient.PostAsJsonAsync("api/auth/login", apiRequest);
        }
        catch (HttpRequestException)
        {
            return AuthApiResult<LoginResponse>.Fail(AuthErrorKind.Network,
                $"Nepodařilo se připojit k serveru ({_httpClient.BaseAddress}).");
        }
        catch (TaskCanceledException)
        {
            return AuthApiResult<LoginResponse>.Fail(AuthErrorKind.Network,
                "Server neodpověděl včas. Zkontrolujte, že API běží.");
        }

        if (response.IsSuccessStatusCode)
        {
            var data = await response.Content.ReadFromJsonAsync<LoginResponse>();
            return data is null
                ? AuthApiResult<LoginResponse>.Fail(AuthErrorKind.Unknown, "Neplatná odpověď serveru.")
                : AuthApiResult<LoginResponse>.Success(data);
        }

        if (response.StatusCode == HttpStatusCode.Unauthorized)
        {
            return AuthApiResult<LoginResponse>.Fail(AuthErrorKind.InvalidCredentials,
                "Neplatný e-mail nebo heslo.");
        }

        var message = await ReadErrorMessageAsync(response);
        return AuthApiResult<LoginResponse>.Fail(AuthErrorKind.Unknown, message);
    }

    public async Task<AuthApiResult<RegisterResponse>> RegisterAsync(RegisterRequest request)
    {
        var apiRequest = new { request.Email, request.Password };

        HttpResponseMessage response;
        try
        {
            response = await _httpClient.PostAsJsonAsync("api/auth/register", apiRequest);
        }
        catch (HttpRequestException)
        {
            return AuthApiResult<RegisterResponse>.Fail(AuthErrorKind.Network,
                $"Nepodařilo se připojit k serveru ({_httpClient.BaseAddress}).");
        }
        catch (TaskCanceledException)
        {
            return AuthApiResult<RegisterResponse>.Fail(AuthErrorKind.Network,
                "Server neodpověděl včas. Zkontrolujte, že API běží.");
        }

        if (response.IsSuccessStatusCode)
        {
            var data = await response.Content.ReadFromJsonAsync<RegisterResponse>();
            return data is null
                ? AuthApiResult<RegisterResponse>.Fail(AuthErrorKind.Unknown, "Neplatná odpověď serveru.")
                : AuthApiResult<RegisterResponse>.Success(data);
        }

        if (response.StatusCode == HttpStatusCode.Conflict)
        {
            return AuthApiResult<RegisterResponse>.Fail(AuthErrorKind.DuplicateEmail,
                "Účet s tímto e-mailem již existuje.");
        }

        var message = await ReadErrorMessageAsync(response);
        return AuthApiResult<RegisterResponse>.Fail(AuthErrorKind.Unknown, message);
    }

    private static async Task<string?> ReadErrorMessageAsync(HttpResponseMessage response)
    {
        try
        {
            var json = await response.Content.ReadFromJsonAsync<JsonElement>();
            if (json.TryGetProperty("message", out var message))
            {
                return message.GetString();
            }
        }
        catch
        {
            // ignore parse errors
        }

        return $"Chyba serveru ({(int)response.StatusCode}).";
    }
}
