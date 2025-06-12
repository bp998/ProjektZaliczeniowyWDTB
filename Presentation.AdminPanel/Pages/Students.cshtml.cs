using System.Net.Http.Json;
using Application.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.RazorPages;

[Authorize(Roles = "Admin")]
public class StudentsModel : PageModel
{
    public List<StudentDto> Students { get; set; } = new();

    public async Task OnGetAsync()
    {
        using var http = new HttpClient();
        var data = await http.GetFromJsonAsync<List<StudentDto>>("http://localhost:5260//api/student");
        if (data is not null)
            Students = data;
    }
}
