using LangApp.API.DTOs.Auth;
using LangApp.API.Exceptions;
using LangApp.API.Services;
using Microsoft.AspNetCore.Mvc;

namespace LangApp.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class AuthController : ControllerBase
{
    private readonly IAuthService _authService;

    public AuthController(IAuthService authService)
    {
        _authService = authService;
    }

    [HttpPost("register")]
    public async Task<ActionResult<RegisterResponse>> Register(RegisterRequest request)
    {
        try
        {
            var result = await _authService.RegisterUser(request.Email, request.Password);
            return CreatedAtAction(nameof(Register), result);
        }
        catch (DuplicateEmailException ex)
        {
            return Conflict(new { message = ex.Message });
        }
    }

    [HttpPost("login")]
    public async Task<ActionResult<LoginResponse>> Login(LoginRequest request)
    {
        try
        {
            var result = await _authService.Login(request.Email, request.Password);
            return Ok(result);
        }
        catch (InvalidCredentialsException ex)
        {
            return Unauthorized(new { message = ex.Message });
        }
    }
}
