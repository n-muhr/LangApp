using LangApp.Core.Data;
using LangApp.Core.Models;
using Microsoft.EntityFrameworkCore;

namespace LangApp.API.Services
{
    public class AuthService : IAuthService
    {
        private readonly AppDbContext _context;
        private readonly IConfiguration _configuration;

        public AuthService(AppDbContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }

        public async Task<string> Login(string email, string password)
        {
            // TODO: Implement full JWT token generation and password verification using BCrypt
            // For now, return a placeholder token.
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == email);
            if (user == null || !VerifyPassword(password, user.PasswordHash))
            {
                throw new Exception("Invalid credentials.");
            }

            // Logic to generate and return the JWT token
            return "placeholder-jwt-token";
        }

        public async Task<User> RegisterUser(string email, string password)
        {
            // TODO: Implement password hashing (BCrypt) and saving the new user
            var newUser = new User { Email = email, PasswordHash = "hashed_password_placeholder" };
            _context.Users.Add(newUser);
            await _context.SaveChangesAsync();
            return newUser;
        }

        private bool VerifyPassword(string plainPassword, string hash)
        {
            // Placeholder for BCrypt.CheckPassword(plainPassword, hash)
            return true;
        }
    }
}
