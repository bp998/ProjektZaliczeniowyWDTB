using Application.DTOs;
using Domain.Entities;
using Domain.Interfaces;
using Moq;
using Presentation.WebApi.Controllers;
using Microsoft.AspNetCore.Mvc;

namespace Tests.Unit.Controllers;

public class CourseControllerTests
{
    private readonly Mock<ICourseRepository> _mockRepo = new();
    private readonly CourseController _controller;

    public CourseControllerTests()
    {
        _controller = new CourseController(_mockRepo.Object);
    }

    [Fact]
    public async Task GetAll_ReturnsAllCourses()
    {
        var courses = new List<Course>
        {
            new() { Id = Guid.NewGuid(), Title = "Math", Description = "Algebra" },
            new() { Id = Guid.NewGuid(), Title = "Physics", Description = "Mechanics" }
        };

        _mockRepo.Setup(r => r.GetAllAsync()).ReturnsAsync(courses);

        var result = await _controller.GetAll() as OkObjectResult;

        Assert.NotNull(result);
        var returned = Assert.IsAssignableFrom<IEnumerable<CourseDto>>(result.Value);
        Assert.Equal(2, returned.Count());
    }

    [Fact]
    public async Task Create_ReturnsCreatedAtAction()
    {
        var dto = new CourseCreateDto { Title = "Biology", Description = "Genetics" };

        var result = await _controller.Create(dto) as CreatedAtActionResult;

        Assert.NotNull(result);
        var returned = Assert.IsType<CourseDto>(result.Value);
        Assert.Equal(dto.Title, returned.Title);
    }
    [Fact]
    public async Task Update_ReturnsNoContent_WhenCourseExists()
    {
        var courseId = Guid.NewGuid();
        var existingCourse = new Course { Id = courseId, Title = "Old", Description = "OldDesc" };

        _mockRepo.Setup(r => r.GetByIdAsync(courseId)).ReturnsAsync(existingCourse);

        var updatedDto = new CourseDto { Id = courseId, Title = "New", Description = "NewDesc" };

        var result = await _controller.Update(courseId, updatedDto);

        Assert.IsType<NoContentResult>(result);
        _mockRepo.Verify(r => r.UpdateAsync(It.IsAny<Course>()), Times.Once);
    }

    [Fact]
    public async Task Update_ReturnsNotFound_WhenCourseNotExists()
    {
        var courseId = Guid.NewGuid();
        _mockRepo.Setup(r => r.GetByIdAsync(courseId)).ReturnsAsync((Course?)null);

        var dto = new CourseDto { Id = courseId, Title = "Test", Description = "Desc" };
        var result = await _controller.Update(courseId, dto);

        Assert.IsType<NotFoundResult>(result);
    }

    [Fact]
    public async Task Delete_ReturnsNoContent_WhenDeleted()
    {
        var id = Guid.NewGuid();

        var result = await _controller.Delete(id);

        Assert.IsType<NoContentResult>(result);
        _mockRepo.Verify(r => r.DeleteAsync(id), Times.Once);
    }

}
