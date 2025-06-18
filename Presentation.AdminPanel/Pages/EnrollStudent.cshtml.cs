using Application.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Net.Http.Headers;
using System.Net.Http.Json;

[Authorize(Roles = "Admin")]
public class EnrollStudentModel : PageModel
{
    [BindProperty]
    public EnrollmentCreateDto Enrollment { get; set; } = new();

    public List<StudentDto> Students { get; set; } = new();
    public List<CourseDto> Courses { get; set; } = new();

    public async Task<IActionResult> OnGetAsync()
    {
        try
        {
            var token = Request.Cookies["access_token"];
            using var http = new HttpClient();

            if (!string.IsNullOrWhiteSpace(token))
            {
                http.DefaultRequestHeaders.Authorization =
                    new AuthenticationHeaderValue("Bearer", token);
            }

            Students = await http.GetFromJsonAsync<List<StudentDto>>("http://localhost:5001/api/student") ?? new();
            Courses = await http.GetFromJsonAsync<List<CourseDto>>("http://localhost:5001/api/course") ?? new();

            // Dodaj właściwość FullName, jeśli nie ma w DTO
            foreach (var student in Students)
            {
                student.FullName = $"{student.FirstName} {student.LastName}";
            }

            return Page();
        }
        catch (Exception ex)
        {
            ModelState.AddModelError("", $"Błąd ładowania danych: {ex.Message}");
            return Page();
        }
    }

    public async Task<IActionResult> OnPostAsync()
    {
        try
        {
            var token = Request.Cookies["access_token"];
            using var http = new HttpClient();

            if (!string.IsNullOrWhiteSpace(token))
            {
                http.DefaultRequestHeaders.Authorization =
                    new AuthenticationHeaderValue("Bearer", token);
            }

            var response = await http.PostAsJsonAsync("http://localhost:5001/api/enrollment", Enrollment);

            if (response.IsSuccessStatusCode)
                return RedirectToPage("/Enrollments");

            var errorContent = await response.Content.ReadAsStringAsync();
            ModelState.AddModelError("", $"Błąd zapisu: {response.StatusCode} | {errorContent}");

            await OnGetAsync(); // przeładuj dane
            return Page();
        }
        catch (Exception ex)
        {
            ModelState.AddModelError("", $"Błąd połączenia z API: {ex.Message}");
            await OnGetAsync();
            return Page();
        }
    }
}
