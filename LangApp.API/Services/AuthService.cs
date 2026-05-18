using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using LangApp.API.DTOs.Auth;
using LangApp.API.Exceptions;
using LangApp.Core.Data;
using LangApp.Core.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace LangApp.API.Services;

public class AuthService : IAuthService
{
    private const int TokenExpirationDays = 7;

    private readonly AppDbContext _context;
    private readonly IConfiguration _configuration;

    public AuthService(AppDbContext context, IConfiguration configuration)
    {
        _context = context;
        _configuration = configuration;
    }

    public async Task<LoginResponse> Login(string email, string password)
    {
        var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == email);
        if (user == null || !BCrypt.Net.BCrypt.Verify(password, user.PasswordHash))
        {
            throw new InvalidCredentialsException();
        }

        var expiresAt = DateTime.UtcNow.AddDays(TokenExpirationDays);
        var token = GenerateJwtToken(user, expiresAt);

        return new LoginResponse
        {
            Token = token,
            Email = user.Email,
            ExpiresAt = expiresAt
        };
    }

    public async Task<RegisterResponse> RegisterUser(string email, string password)
    {
        var normalizedEmail = email.Trim().ToLowerInvariant();
        var exists = await _context.Users.AnyAsync(u => u.Email.ToLower() == normalizedEmail);
        if (exists)
        {
            throw new DuplicateEmailException();
        }

        var newUser = new User
        {
            Email = email.Trim(),
            PasswordHash = BCrypt.Net.BCrypt.HashPassword(password)
        };

        _context.Users.Add(newUser);
        await _context.SaveChangesAsync();

        return new RegisterResponse
        {
            Id = newUser.Id,
            Email = newUser.Email
        };
    }

    private string GenerateJwtToken(User user, DateTime expiresAt)
    {
        var secret = _configuration["JWT_SECRET"]
            ?? throw new InvalidOperationException("JWT_SECRET is not configured.");
        var issuer = _configuration["JWT_ISSUER"] ?? "LangApp";
        var audience = _configuration["JWT_AUDIENCE"] ?? "LangApp.Client";

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secret));
        var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var claims = new[]
        {
            new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
            new Claim(JwtRegisteredClaimNames.Email, user.Email),
            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString())
        };

        var token = new JwtSecurityToken(
            issuer: issuer,
            audience: audience,
            claims: claims,
            expires: expiresAt,
            signingCredentials: credentials);

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}
