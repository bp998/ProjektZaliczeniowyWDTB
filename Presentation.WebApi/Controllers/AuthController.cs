using Infrastructure.Auth;
using Infrastructure.Persistance;
using Microsoft.AspNetCore.Mvc;
using System.Security.Cryptography;
using System.Text;

namespace Presentation.WebApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly AppDbContext _context;
    private readonly AuthService _authService;

    public AuthController(AppDbContext context, AuthService authService)
    {
        _context = context;
        _authService = authService;
    }

    [HttpPost("login")]
    public IActionResult Login([FromBody] LoginRequest request)
    {
        var user = _context.Users.FirstOrDefault(u => u.Username == request.Username);
        if (user is null || !VerifyPassword(request.Password, user.PasswordHash))
            return Unauthorized("Invalid credentials");

        var token = _authService.GenerateToken(user);
        return Ok(new { token });
    }

    private bool VerifyPassword(string password, string hash)
    {
        var passwordBytes = Encoding.UTF8.GetBytes(password);
        var hashBytes = Convert.FromBase64String(hash);
        using var sha = SHA256.Create();
        var computedHash = sha.ComputeHash(passwordBytes);
        return hashBytes.SequenceEqual(computedHash);
    }
}

public class LoginRequest
{
    public string Username { get; set; } = "";
    public string Password { get; set; } = "";
}
