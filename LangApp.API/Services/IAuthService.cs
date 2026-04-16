using LangApp.Core.Models;

namespace LangApp.API.Services
{
    public interface IAuthService
    {
        Task<User> RegisterUser(string email, string password);
        Task<string> Login(string email, string password);
    }
}
