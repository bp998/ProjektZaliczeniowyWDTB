using Application.DTOs;
using Domain.Entities;
using Domain.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Presentation.WebApi.Controllers;
using Xunit;

namespace Tests.Unit.WebApi.Controllers;

public class StudentControllerTests
{
    [Fact]
    public async Task GetAll_ReturnsOkWithStudents()
    {
        // Arrange
        var students = new List<Student>
        {
            new Student { Id = Guid.NewGuid(), FirstName = "Anna", LastName = "Kowalska", BirthDate = new DateTime(2000, 1, 1) },
            new Student { Id = Guid.NewGuid(), FirstName = "Jan", LastName = "Nowak", BirthDate = new DateTime(1999, 5, 15) }
        };

        var mockRepo = new Mock<IStudentRepository>();
        mockRepo.Setup(r => r.GetAllAsync()).ReturnsAsync(students);

        var controller = new StudentController(mockRepo.Object);

        // Act
        var result = await controller.GetAll();

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        var returnedStudents = Assert.IsAssignableFrom<IEnumerable<StudentDto>>(okResult.Value);

        Assert.Equal(2, returnedStudents.Count());
        Assert.Contains(returnedStudents, s => s.FirstName == "Anna");
        Assert.Contains(returnedStudents, s => s.LastName == "Nowak");
    }
    [Fact]
    public async Task Get_ReturnsOkWithStudent_WhenStudentExists()
    {
        // Arrange
        var studentId = Guid.NewGuid();
        var student = new Student
        {
            Id = studentId,
            FirstName = "Anna",
            LastName = "Kowalska",
            BirthDate = new DateTime(2000, 1, 1)
        };

        var mockRepo = new Mock<IStudentRepository>();
        mockRepo.Setup(r => r.GetByIdAsync(studentId)).ReturnsAsync(student);

        var controller = new StudentController(mockRepo.Object);

        // Act
        var result = await controller.Get(studentId);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        var dto = Assert.IsType<StudentDto>(okResult.Value);
        Assert.Equal("Anna", dto.FirstName);
    }
    [Fact]
    public async Task Get_ReturnsNotFound_WhenStudentDoesNotExist()
    {
        // Arrange
        var studentId = Guid.NewGuid();

        var mockRepo = new Mock<IStudentRepository>();
        mockRepo.Setup(r => r.GetByIdAsync(studentId)).ReturnsAsync((Student?)null);

        var controller = new StudentController(mockRepo.Object);

        // Act
        var result = await controller.Get(studentId);

        // Assert
        Assert.IsType<NotFoundResult>(result);
    }
    [Fact]
    public async Task Create_ReturnsCreatedAtAction_WhenStudentIsValid()
    {
        // Arrange
        var dto = new StudentCreateDto
        {
            FirstName = "John",
            LastName = "Smith",
            BirthDate = new DateTime(1995, 3, 15)
        };

        var mockRepo = new Mock<IStudentRepository>();
        mockRepo.Setup(r => r.AddAsync(It.IsAny<Student>())).Returns(Task.CompletedTask);

        var controller = new StudentController(mockRepo.Object);

        // Act
        var result = await controller.Create(dto);

        // Assert
        var createdResult = Assert.IsType<CreatedAtActionResult>(result);
        var returnedDto = Assert.IsType<StudentDto>(createdResult.Value);
        Assert.Equal("John", returnedDto.FirstName);
    }
    [Fact]
    public async Task Delete_ReturnsNoContent()
    {
        // Arrange
        var id = Guid.NewGuid();

        var mockRepo = new Mock<IStudentRepository>();
        mockRepo.Setup(r => r.DeleteAsync(id)).Returns(Task.CompletedTask);

        var controller = new StudentController(mockRepo.Object);

        // Act
        var result = await controller.Delete(id);

        // Assert
        Assert.IsType<NoContentResult>(result);
    }

}
