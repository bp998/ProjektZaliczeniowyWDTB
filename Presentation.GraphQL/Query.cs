using Domain.Entities;
using HotChocolate;
using HotChocolate.Data;
using Infrastructure.Persistance;
using Microsoft.EntityFrameworkCore;

public class Query
{
    [UseProjection]
    [UseFiltering]
    [UseSorting]
    public IQueryable<Student> GetStudents([Service] IDbContextFactory<AppDbContext> factory)
    {
        return factory.CreateDbContext().Students;
    }

    [UseProjection]
    [UseFiltering]
    [UseSorting]
    public IQueryable<Course> GetCourses([Service] IDbContextFactory<AppDbContext> factory)
    {
        return factory.CreateDbContext().Courses;
    }

    [UseProjection]
    [UseFiltering]
    [UseSorting]
    public IQueryable<Enrollment> GetEnrollments([Service] IDbContextFactory<AppDbContext> factory)
    {
        return factory.CreateDbContext().Enrollments;
    }
}
