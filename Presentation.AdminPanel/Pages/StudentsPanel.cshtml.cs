using Application.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Net.Http.Headers;
using System.Net.Http.Json;

[Authorize(Roles = "Admin")]
public class StudentsPanelModel : PageModel
{
    public List<StudentDto> Students { get; set; } = new();

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
            var data = await http.GetFromJsonAsync<List<StudentDto>>("http://localhost:5001/api/student");
            if (data is not null)
                Students = data;
        }
        catch (HttpRequestException ex)
        {
            ModelState.AddModelError("", $"Błąd połączenia z API: {ex.Message}");
        }

        return Page();
    }
    public async Task<IActionResult> OnPostDeleteAsync(Guid id)
    {
        var token = Request.Cookies["access_token"];
        using var http = new HttpClient();

        if (!string.IsNullOrWhiteSpace(token))
            http.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

        var response = await http.DeleteAsync($"http://localhost:5001/api/student/{id}");

        if (!response.IsSuccessStatusCode)
        {
            ModelState.AddModelError("", $"Nie udało się usunąć studenta: {response.StatusCode}");
        }

        return RedirectToPage();
    }
}
