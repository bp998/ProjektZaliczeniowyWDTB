using Domain.Entities;
using Infrastructure.Auth;
using Microsoft.Extensions.Configuration;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Xunit;

public class AuthServiceTests
{
    private readonly AuthService _authService;

    public AuthServiceTests()
    {
        var inMemorySettings = new Dictionary<string, string>
        {
            { "Jwt:Key", "superMegaBezpiecznyJWTkluczHaszujacy123" },
            { "Jwt:Issuer", "TestIssuer" },
            { "Jwt:Audience", "TestAudience" }
        };

        var configuration = new ConfigurationBuilder()
            .AddInMemoryCollection(inMemorySettings)
            .Build();

        _authService = new AuthService(configuration);
    }

    [Fact]
    public void GenerateToken_ValidUser_ReturnsValidJwtWithClaims()
    {
        // Arrange
        var user = new User
        {
            Username = "admin",
            Role = "Admin"
        };

        // Act
        var token = _authService.GenerateToken(user);

        // Assert
        Assert.False(string.IsNullOrWhiteSpace(token));

        var handler = new JwtSecurityTokenHandler();
        var jwt = handler.ReadJwtToken(token);

        var nameClaim = jwt.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Name);
        var roleClaim = jwt.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Role);

        Assert.Equal("admin", nameClaim?.Value);
        Assert.Equal("Admin", roleClaim?.Value);
        Assert.Equal("TestIssuer", jwt.Issuer);
        Assert.Equal("TestAudience", jwt.Audiences.First());
    }
}
