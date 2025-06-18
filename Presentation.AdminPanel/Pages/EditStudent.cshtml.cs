using Application.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System;
using System.Net.Http.Headers;
using System.Net.Http.Json;

[Authorize(Roles = "Admin")]
public class EditStudentModel : PageModel
{
    [BindProperty]
    public StudentDto Student { get; set; } = new();
    public async Task<IActionResult> OnGetAsync(Guid id)
    {
        var token = Request.Cookies["access_token"];
        using var http = new HttpClient();

        if (!string.IsNullOrWhiteSpace(token))
            http.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

        var student = await http.GetFromJsonAsync<StudentDto>($"http://localhost:5001/api/student/{id}");
        if (student is null)
            return NotFound();

        Student = student;
        return Page();
    }

    public async Task<IActionResult> OnPostAsync()
    {
        var token = Request.Cookies["access_token"];
        using var http = new HttpClient();

        if (!string.IsNullOrWhiteSpace(token))
            http.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

        var response = await http.PutAsJsonAsync($"http://localhost:5001/api/student/{Student.Id}", Student);

        if (response.IsSuccessStatusCode)
            return RedirectToPage("/StudentsPanel");

        ModelState.AddModelError("", "Nie udało się zaktualizować danych studenta.");
        return Page();
    }
}