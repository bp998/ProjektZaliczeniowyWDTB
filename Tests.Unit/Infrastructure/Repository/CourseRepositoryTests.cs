using Domain.Entities;
using Infrastructure.Persistance;
using Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Xunit;

public class CourseRepositoryTests
{
    private readonly AppDbContext _context;
    private readonly CourseRepository _repository;

    public CourseRepositoryTests()
    {
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase("CourseDb")
            .Options;

        _context = new AppDbContext(options);
        _repository = new CourseRepository(_context);
    }

    [Fact]
    public async Task AddAsync_AddsCourse()
    {
        var course = new Course { Title = "Biologia", Description = "Test" };

        await _repository.AddAsync(course);
        var result = await _repository.GetAllAsync();

        Assert.Single(result);
        Assert.Equal("Biologia", result.First().Title);
    }

    [Fact]
    public async Task DeleteAsync_removesCourse()
    {
        // Arrange
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;

        var course = new Course { Title = "Biologia", Description = "Test" };

        using (var context = new AppDbContext(options))
        {
            context.Courses.Add(course);
            await context.SaveChangesAsync();
        }

        // Act
        using (var context = new AppDbContext(options))
        {
            var repository = new CourseRepository(context);
            await repository.DeleteAsync(course.Id);
        }

        // Assert
        using (var context = new AppDbContext(options))
        {
            Assert.Empty(context.Courses);
        }
    }

    [Fact]
    public async Task GetByIdAsync_ReturnsCorrectCourse()
    {
        var course = new Course { Title = "Matma", Description = "Równania" };
        _context.Courses.Add(course);
        await _context.SaveChangesAsync();

        var result = await _repository.GetByIdAsync(course.Id);
        Assert.NotNull(result);
        Assert.Equal("Matma", result!.Title);
    }
}
