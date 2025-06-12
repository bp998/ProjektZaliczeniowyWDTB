using System.Text;
using System.Text.Json;
using Application.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

[Authorize(Roles = "Admin")]
public class AddStudentModel : PageModel
{
    [BindProperty]
    public StudentDto NewStudent { get; set; } = new();

    public async Task<IActionResult> OnPostAsync()
    {
        using var http = new HttpClient();

        var json = JsonSerializer.Serialize(NewStudent);
        var content = new StringContent(json, Encoding.UTF8, "application/json");

        var result = await http.PostAsync("http://localhost:5260//api/student", content);

        if (result.IsSuccessStatusCode)
            return RedirectToPage("/Students");

        return Page();
    }
}
