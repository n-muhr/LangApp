using LangApp.Client.Models.Auth;

namespace LangApp.Client.Services;

public interface IAuthApiClient
{
    Task<AuthApiResult<LoginResponse>> LoginAsync(LoginRequest request);
    Task<AuthApiResult<RegisterResponse>> RegisterAsync(RegisterRequest request);
}
