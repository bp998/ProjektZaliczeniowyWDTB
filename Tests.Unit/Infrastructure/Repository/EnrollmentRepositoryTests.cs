using Domain.Entities;
using Infrastructure.Persistance;
using Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Xunit;

public class EnrollmentRepositoryTests
{
    private readonly AppDbContext _context;
    private readonly EnrollmentRepository _repository;

    public EnrollmentRepositoryTests()
    {
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase("EnrollmentDb")
            .Options;

        _context = new AppDbContext(options);
        _repository = new EnrollmentRepository(_context);
    }

    [Fact]
    public async Task AddAsync_AddsEnrollment()
    {
        var student = new Student { FirstName = "Adam", LastName = "Test", BirthDate = DateTime.UtcNow.AddYears(-23) };
        var course = new Course { Title = "Fizyka", Description = "Mechanika" };
        _context.Students.Add(student);
        _context.Courses.Add(course);
        await _context.SaveChangesAsync();

        var enrollment = new Enrollment { StudentId = student.Id, CourseId = course.Id };
        await _repository.AddAsync(enrollment);

        var result = await _repository.GetAllAsync();
        Assert.Single(result);
    }

    [Fact]
    public async Task IsStudentEnrolledInCourse_ReturnsTrue_WhenEnrolled()
    {
        var student = new Student { FirstName = "Joanna", LastName = "Kowalska", BirthDate = DateTime.UtcNow.AddYears(-25) };
        var course = new Course { Title = "Geografia", Description = "Mapa" };
        _context.Students.Add(student);
        _context.Courses.Add(course);
        await _context.SaveChangesAsync();

        var enrollment = new Enrollment { StudentId = student.Id, CourseId = course.Id };
        _context.Enrollments.Add(enrollment);
        await _context.SaveChangesAsync();

        var result = await _repository.IsStudentEnrolledInCourse(student.Id, course.Id);
        Assert.True(result);
    }
}
