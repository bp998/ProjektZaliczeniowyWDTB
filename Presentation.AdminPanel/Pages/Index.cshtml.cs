using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Security.Claims;
using System.Text;
using System.Text.Json;

public class IndexModel : PageModel
{
    [BindProperty]
    public string Username { get; set; } = "";
    [BindProperty]
    public string Password { get; set; } = "";

    public string? ErrorMessage { get; set; }

    public async Task<IActionResult> OnPostAsync()
    {
        using var http = new HttpClient();
        var response = await http.PostAsync("http://localhost:5001/api/auth/login",
            new StringContent(JsonSerializer.Serialize(new { username = Username, password = Password }), Encoding.UTF8, "application/json"));

        if (!response.IsSuccessStatusCode)
        {
            ErrorMessage = "Nieprawidłowe dane logowania.";
            return Page();
        }

        var json = await response.Content.ReadAsStringAsync();
        var jwt = JsonDocument.Parse(json).RootElement.GetProperty("token").GetString();

        Response.Cookies.Append("access_token", jwt!, new CookieOptions
        {
            HttpOnly = true,
            Secure = false, 
            SameSite = SameSiteMode.Lax,
            Expires = DateTimeOffset.UtcNow.AddHours(1)
        });


        var handler = new System.IdentityModel.Tokens.Jwt.JwtSecurityTokenHandler();
        var token = handler.ReadJwtToken(jwt);
        var claims = token.Claims.ToList();

        var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
        var principal = new ClaimsPrincipal(identity);

        await HttpContext.SignInAsync("CookieAuth", principal);

        var role = claims.FirstOrDefault(c => c.Type == ClaimTypes.Role)?.Value;

        return role switch
        {
            "Admin" => RedirectToPage("/Admin"),
            "User" => RedirectToPage("/UserPanel"),
            _ => RedirectToPage("/Index")
        };
    }
}
