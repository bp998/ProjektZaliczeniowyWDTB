using Application.DTOs;
using Domain.Entities;
using Domain.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Presentation.WebApi.Controllers;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class StudentController : ControllerBase
{
    private readonly IStudentRepository _repository;

    public StudentController(IStudentRepository repository)
    {
        _repository = repository;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var students = await _repository.GetAllAsync();
        var result = students.Select(s => new StudentDto
        {
            Id = s.Id,
            FirstName = s.FirstName,
            LastName = s.LastName,
            BirthDate = s.BirthDate
        });

        return Ok(result);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> Get(Guid id)
    {
        var s = await _repository.GetByIdAsync(id);
        if (s is null) return NotFound();

        var dto = new StudentDto
        {
            Id = s.Id,
            FirstName = s.FirstName,
            LastName = s.LastName,
            BirthDate = s.BirthDate
        };

        return Ok(dto);
    }

    [Authorize(Roles = "Admin")]
    [HttpPost]
    public async Task<IActionResult> Create([FromBody] StudentCreateDto dto)
    {
        var student = new Student
        {
            FirstName = dto.FirstName,
            LastName = dto.LastName,
            BirthDate = dto.BirthDate
        };

        await _repository.AddAsync(student);

        var resultDto = new StudentDto
        {
            Id = student.Id,
            FirstName = student.FirstName,
            LastName = student.LastName,
            BirthDate = student.BirthDate
        };

        return CreatedAtAction(nameof(Get), new { id = student.Id }, resultDto);
    }


    [Authorize(Roles = "Admin")]
    [HttpPut("{id}")]
    public async Task<IActionResult> Update(Guid id, [FromBody] StudentDto dto)
    {
        var existing = await _repository.GetByIdAsync(id);
        if (existing is null) return NotFound();

        existing.FirstName = dto.FirstName;
        existing.LastName = dto.LastName;
        existing.BirthDate = dto.BirthDate;

        await _repository.UpdateAsync(existing);
        return NoContent();
    }

    [Authorize(Roles = "Admin")]
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        await _repository.DeleteAsync(id);
        return NoContent();
    }
}
