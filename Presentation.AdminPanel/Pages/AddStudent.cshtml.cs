﻿using Application.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Net.Http.Headers;
using System.Net.Http.Json;

[Authorize(Roles = "Admin")]
public class AddStudentModel : PageModel
{
    [BindProperty]
    public StudentCreateDto Student { get; set; } = new();

    public async Task<IActionResult> OnPostAsync()
    {
        var token = Request.Cookies["access_token"];
        using var http = new HttpClient();

        if (!string.IsNullOrWhiteSpace(token))
            http.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

        var response = await http.PostAsJsonAsync("http://localhost:5001/api/student", Student);

        if (response.IsSuccessStatusCode)
            return RedirectToPage("/Students");

        ModelState.AddModelError("", "Nie udało się dodać studenta.");
        return Page();
    }
}
