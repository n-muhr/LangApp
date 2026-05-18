using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using LangApp.Client.Models.Auth;

namespace LangApp.Client.Services;

public class AuthStateService
{
    public string? Token { get; private set; }
    public string? Email { get; private set; }

    public bool IsAuthenticated => !string.IsNullOrEmpty(Token);

    public void SetAuth(LoginResponse login)
    {
        Token = login.Token;
        Email = login.Email;
    }

    public void Clear()
    {
        Token = null;
        Email = null;
    }

    public ClaimsPrincipal CreatePrincipal()
    {
        if (string.IsNullOrEmpty(Token))
        {
            return new ClaimsPrincipal(new ClaimsIdentity());
        }

        try
        {
            var handler = new JwtSecurityTokenHandler();
            var jwt = handler.ReadJwtToken(Token);
            var identity = new ClaimsIdentity(jwt.Claims, authenticationType: "jwt");
            return new ClaimsPrincipal(identity);
        }
        catch
        {
            return new ClaimsPrincipal(new ClaimsIdentity());
        }
    }
}
