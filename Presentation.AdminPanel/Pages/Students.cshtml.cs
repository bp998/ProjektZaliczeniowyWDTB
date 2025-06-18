using System.Net.Http.Json;
using Application.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Net.Http.Headers;
using Microsoft.AspNetCore.Mvc;

[Authorize]
public class StudentsModel : PageModel
{
    public List<StudentDto> Students { get; set; } = new();

    public string? ErrorMessage { get; set; }

    public async Task<IActionResult> OnGetAsync()
    {
        var token = Request.Cookies["access_token"];
        using var http = new HttpClient();

        // Dodanie tokenu do nagłówka
        if (!string.IsNullOrWhiteSpace(token))
        {
            http.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Bearer", token);
        }

        try
        {
            // TEST: Ręczne żądanie dla logowania odpowiedzi
            var response = await http.GetAsync("http://localhost:5001/api/student");
            var content = await response.Content.ReadAsStringAsync();

            if (!response.IsSuccessStatusCode)
            {
                ErrorMessage = $"Błąd API: {response.StatusCode} | Treść: {content}";
                ModelState.AddModelError("", ErrorMessage);
                return Page();
            }

            // Jeśli odpowiedź OK, parsujemy JSON
            Students = await response.Content.ReadFromJsonAsync<List<StudentDto>>() ?? new();
        }
        catch (HttpRequestException ex)
        {
            ErrorMessage = $"Błąd połączenia z API: {ex.Message}";
            ModelState.AddModelError("", ErrorMessage);
        }

        return Page();
    }
}
