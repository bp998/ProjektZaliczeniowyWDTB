using Application.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Net.Http.Headers;
using System.Net.Http.Json;

[Authorize]
public class EnrollmentsModel : PageModel
{
    public List<EnrollmentDto> Enrollments { get; set; } = new();

    public async Task<IActionResult> OnGetAsync()
    {
        var token = Request.Cookies["access_token"];
        using var http = new HttpClient();

        if (!string.IsNullOrWhiteSpace(token))
        {
            http.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Bearer", token);
        }

        try
        {
            var data = await http.GetFromJsonAsync<List<EnrollmentDto>>("http://localhost:5001/api/enrollment");
            if (data is not null)
                Enrollments = data;
        }
        catch (HttpRequestException ex)
        {
            ModelState.AddModelError("", $"Błąd połączenia z API: {ex.Message}");
        }

        return Page();
    }
}
