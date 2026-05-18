using LangApp.API.DTOs.Auth;

namespace LangApp.API.Services;

public interface IAuthService
{
    Task<RegisterResponse> RegisterUser(string email, string password);
    Task<LoginResponse> Login(string email, string password);
}
