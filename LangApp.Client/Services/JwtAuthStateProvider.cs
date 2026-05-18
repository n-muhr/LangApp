using System.Security.Claims;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Server.ProtectedBrowserStorage;

namespace LangApp.Client.Services;

public class JwtAuthStateProvider : AuthenticationStateProvider
{
    private const string TokenStorageKey = "auth_token";
    private const string EmailStorageKey = "auth_email";

    private readonly AuthStateService _authState;
    private readonly ProtectedLocalStorage _localStorage;

    public JwtAuthStateProvider(AuthStateService authState, ProtectedLocalStorage localStorage)
    {
        _authState = authState;
        _localStorage = localStorage;
    }

    public override async Task<AuthenticationState> GetAuthenticationStateAsync()
    {
        if (!_authState.IsAuthenticated)
        {
            await TryRestoreFromStorageAsync();
        }

        var principal = _authState.CreatePrincipal();
        return new AuthenticationState(principal);
    }

    public async Task SignInAsync(string token, string email)
    {
        _authState.SetAuth(new Models.Auth.LoginResponse
        {
            Token = token,
            Email = email,
            ExpiresAt = DateTime.UtcNow
        });

        await _localStorage.SetAsync(TokenStorageKey, token);
        await _localStorage.SetAsync(EmailStorageKey, email);

        NotifyAuthenticationStateChanged(GetAuthenticationStateAsync());
    }

    public async Task SignOutAsync()
    {
        _authState.Clear();

        try
        {
            await _localStorage.DeleteAsync(TokenStorageKey);
            await _localStorage.DeleteAsync(EmailStorageKey);
        }
        catch
        {
            // storage may be unavailable during prerender
        }

        NotifyAuthenticationStateChanged(GetAuthenticationStateAsync());
    }

    private async Task TryRestoreFromStorageAsync()
    {
        try
        {
            var tokenResult = await _localStorage.GetAsync<string>(TokenStorageKey);
            var emailResult = await _localStorage.GetAsync<string>(EmailStorageKey);

            if (tokenResult.Success && emailResult.Success &&
                !string.IsNullOrEmpty(tokenResult.Value) && !string.IsNullOrEmpty(emailResult.Value))
            {
                _authState.SetAuth(new Models.Auth.LoginResponse
                {
                    Token = tokenResult.Value,
                    Email = emailResult.Value,
                    ExpiresAt = DateTime.UtcNow
                });
            }
        }
        catch
        {
            // ProtectedLocalStorage throws during static SSR
        }
    }
}
