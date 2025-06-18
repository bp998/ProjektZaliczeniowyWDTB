using Application.DTOs;
using Domain.Entities;
using Domain.Interfaces;
using Moq;
using Presentation.WebApi.Controllers;
using Microsoft.AspNetCore.Mvc;

namespace Tests.Unit.Controllers;

public class EnrollmentControllerTests
{
    private readonly Mock<IEnrollmentRepository> _enrollRepo = new();
    private readonly Mock<IStudentRepository> _studentRepo = new();
    private readonly Mock<ICourseRepository> _courseRepo = new();
    private readonly EnrollmentController _controller;

    public EnrollmentControllerTests()
    {
        _controller = new EnrollmentController(_enrollRepo.Object, _studentRepo.Object, _courseRepo.Object);
    }

    [Fact]
    public async Task Create_ReturnsCreatedAtAction_WhenValid()
    {
        var studentId = Guid.NewGuid();
        var courseId = Guid.NewGuid();

        _studentRepo.Setup(r => r.GetByIdAsync(studentId))
            .ReturnsAsync(new Student { Id = studentId, FirstName = "Jan", LastName = "Kowalski" });

        _courseRepo.Setup(r => r.GetByIdAsync(courseId))
            .ReturnsAsync(new Course { Id = courseId, Title = "Math" });

        _enrollRepo.Setup(r => r.IsStudentEnrolledInCourse(studentId, courseId))
            .ReturnsAsync(false);

        var dto = new EnrollmentCreateDto
        {
            StudentId = studentId,
            CourseId = courseId,
            EnrolledAt = DateTime.Now
        };

        var result = await _controller.Create(dto) as CreatedAtActionResult;

        Assert.NotNull(result);
        var returned = Assert.IsType<EnrollmentDto>(result.Value);
        Assert.Equal("Jan Kowalski", returned.StudentName);
        Assert.Equal("Math", returned.CourseTitle);
    }

    [Fact]
    public async Task Create_ReturnsConflict_WhenAlreadyEnrolled()
    {
        var studentId = Guid.NewGuid();
        var courseId = Guid.NewGuid();

        _studentRepo.Setup(r => r.GetByIdAsync(studentId)).ReturnsAsync(new Student());
        _courseRepo.Setup(r => r.GetByIdAsync(courseId)).ReturnsAsync(new Course());
        _enrollRepo.Setup(r => r.IsStudentEnrolledInCourse(studentId, courseId)).ReturnsAsync(true);

        var dto = new EnrollmentCreateDto
        {
            StudentId = studentId,
            CourseId = courseId,
            EnrolledAt = DateTime.Now
        };

        var result = await _controller.Create(dto);

        Assert.IsType<ConflictObjectResult>(result);
    }
    [Fact]
    public async Task Delete_ReturnsNoContent_WhenDeleted()
    {
        var id = Guid.NewGuid();

        var result = await _controller.Delete(id);

        Assert.IsType<NoContentResult>(result);
        _enrollRepo.Verify(r => r.DeleteAsync(id), Times.Once);
    }

}
