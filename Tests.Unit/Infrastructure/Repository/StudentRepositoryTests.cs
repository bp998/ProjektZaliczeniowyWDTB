using Domain.Entities;
using Infrastructure.Persistance;
using Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Xunit;

public class StudentRepositoryTests
{
    private readonly AppDbContext _context;
    private readonly StudentRepository _repository;

    public StudentRepositoryTests()
    {
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase("StudentDb")
            .Options;

        _context = new AppDbContext(options);
        _repository = new StudentRepository(_context);
    }

    [Fact]
    public async Task AddAsync_AddsStudent()
    {
        // Arrange
        var student = new Student
        {
            FirstName = "Test",
            LastName = "User",
            BirthDate = DateTime.UtcNow
        };

        // Act
        await _repository.AddAsync(student);

        // Assert
        var all = await _repository.GetAllAsync();
        Assert.Contains(all, s => s.FirstName == "Test" && s.LastName == "User");
    }

    public async Task DeleteAsync_RemovesStudent()
    {
        // Arrange
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;

        var student = new Student
        {
            FirstName = "Test",
            LastName = "User",
            BirthDate = DateTime.UtcNow.AddYears(-18)
        };

        // Dodaj studenta do bazy
        using (var context = new AppDbContext(options))
        {
            context.Students.Add(student);
            await context.SaveChangesAsync();
        }

        // Usuń studenta
        using (var context = new AppDbContext(options))
        {
            var repo = new StudentRepository(context);
            await repo.DeleteAsync(student.Id);
        }

        // Sprawdź, że student został usunięty
        using (var context = new AppDbContext(options))
        {
            Assert.Empty(context.Students);
        }
    }

    [Fact]
    public async Task GetByIdAsync_ReturnsCorrectStudent()
    {
        var student = new Student { FirstName = "Anna", LastName = "Nowak", BirthDate = DateTime.UtcNow.AddYears(-19) };
        _context.Students.Add(student);
        await _context.SaveChangesAsync();

        var result = await _repository.GetByIdAsync(student.Id);
        Assert.NotNull(result);
        Assert.Equal("Anna", result!.FirstName);
    }
}
