using Domain.Entities;
using Domain.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Presentation.WebApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class EnrollmentController : ControllerBase
{
    private readonly IEnrollmentRepository _repository;

    public EnrollmentController(IEnrollmentRepository repository)
    {
        _repository = repository;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var enrollments = await _repository.GetAllAsync();
        return Ok(enrollments);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> Get(Guid id)
    {
        var enrollment = await _repository.GetByIdAsync(id);
        if (enrollment is null) return NotFound();
        return Ok(enrollment);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] Enrollment enrollment)
    {
        var isAlreadyEnrolled = await _repository
            .IsStudentEnrolledInCourse(enrollment.StudentId, enrollment.CourseId);

        if (isAlreadyEnrolled)
            return Conflict("Student is already enrolled in this course.");

        await _repository.AddAsync(enrollment);
        return CreatedAtAction(nameof(Get), new { id = enrollment.Id }, enrollment);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(Guid id, [FromBody] Enrollment enrollment)
    {
        if (id != enrollment.Id) return BadRequest();
        await _repository.UpdateAsync(enrollment);
        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        await _repository.DeleteAsync(id);
        return NoContent();
    }
}
