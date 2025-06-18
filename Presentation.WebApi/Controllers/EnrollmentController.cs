using Application.DTOs;
using Domain.Entities;
using Domain.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Presentation.WebApi.Controllers;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class EnrollmentController : ControllerBase
{
    private readonly IEnrollmentRepository _repository;
    private readonly IStudentRepository _studentRepository;
    private readonly ICourseRepository _courseRepository;

    public EnrollmentController(
        IEnrollmentRepository repository,
        IStudentRepository studentRepository,
        ICourseRepository courseRepository)
    {
        _repository = repository;
        _studentRepository = studentRepository;
        _courseRepository = courseRepository;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var enrollments = await _repository.GetAllAsync();

        var result = enrollments.Select(e => new EnrollmentDto
        {
            Id = e.Id,
            EnrolledAt = e.EnrolledAt,
            StudentName = $"{e.Student!.FirstName} {e.Student.LastName}",
            CourseTitle = e.Course!.Title
        });

        return Ok(result);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> Get(Guid id)
    {
        var e = await _repository.GetByIdAsync(id);
        if (e is null) return NotFound();

        var dto = new EnrollmentDto
        {
            Id = e.Id,
            EnrolledAt = e.EnrolledAt,
            StudentName = $"{e.Student!.FirstName} {e.Student.LastName}",
            CourseTitle = e.Course!.Title
        };

        return Ok(dto);
    }

    [Authorize(Roles = "Admin")]
    [HttpPost]
    public async Task<IActionResult> Create([FromBody] EnrollmentCreateDto dto)
    {
        var student = await _studentRepository.GetByIdAsync(dto.StudentId);
        var course = await _courseRepository.GetByIdAsync(dto.CourseId);

        if (student is null || course is null)
            return BadRequest("Student or course not found.");

        var alreadyEnrolled = await _repository.IsStudentEnrolledInCourse(dto.StudentId, dto.CourseId);
        if (alreadyEnrolled)
            return Conflict("Student already enrolled in this course.");

        var enrollment = new Enrollment
        {
            StudentId = dto.StudentId,
            CourseId = dto.CourseId,
            EnrolledAt = dto.EnrolledAt
        };

        await _repository.AddAsync(enrollment);

        var resultDto = new EnrollmentDto
        {
            Id = enrollment.Id,
            EnrolledAt = enrollment.EnrolledAt,
            StudentName = $"{student.FirstName} {student.LastName}",
            CourseTitle = course.Title
        };

        return CreatedAtAction(nameof(Get), new { id = enrollment.Id }, resultDto);
    }

    [Authorize(Roles = "Admin")]
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        await _repository.DeleteAsync(id);
        return NoContent();
    }
}
