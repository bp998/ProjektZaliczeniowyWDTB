﻿using Application.DTOs;
using Domain.Entities;
using Domain.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Presentation.WebApi.Controllers;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class CourseController : ControllerBase
{
    private readonly ICourseRepository _repository;

    public CourseController(ICourseRepository repository)
    {
        _repository = repository;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var courses = await _repository.GetAllAsync();
        var result = courses.Select(c => new CourseDto
        {
            Id = c.Id,
            Title = c.Title,
            Description = c.Description
        });

        return Ok(result);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> Get(Guid id)
    {
        var c = await _repository.GetByIdAsync(id);
        if (c is null) return NotFound();

        var dto = new CourseDto
        {
            Id = c.Id,
            Title = c.Title,
            Description = c.Description
        };

        return Ok(dto);
    }

    [Authorize(Roles = "Admin")]
    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CourseCreateDto dto)
    {
        var course = new Course
        {
            Title = dto.Title,
            Description = dto.Description
        };

        await _repository.AddAsync(course);

        var resultDto = new CourseDto
        {
            Id = course.Id,
            Title = course.Title,
            Description = course.Description
        };

        return CreatedAtAction(nameof(Get), new { id = course.Id }, resultDto);
    }

    [Authorize(Roles = "Admin")]
    [HttpPut("{id}")]
    public async Task<IActionResult> Update(Guid id, [FromBody] CourseDto dto)
    {
        var existing = await _repository.GetByIdAsync(id);
        if (existing is null) return NotFound();

        existing.Title = dto.Title;
        existing.Description = dto.Description;

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
