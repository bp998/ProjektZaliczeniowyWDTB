using Domain.Entities;

namespace Infrastructure.Persistance;

public static class DbSeeder
{
    public static void Seed(AppDbContext context)
    {
        if (context.Students.Any()) return;

        var student1 = new Student
        {
            FirstName = "Jan",
            LastName = "Kowalski",
            BirthDate = new DateTime(2000, 5, 12)
        };

        var student2 = new Student
        {
            FirstName = "Anna",
            LastName = "Nowak",
            BirthDate = new DateTime(1999, 8, 23)
        };

        var course1 = new Course
        {
            Title = "Programowanie w C#",
            Description = "Podstawy C# i .NET"
        };

        var course2 = new Course
        {
            Title = "Bazy danych",
            Description = "Relacyjne i nierelacyjne bazy danych"
        };

        var enrollment1 = new Enrollment
        {
            Student = student1,
            Course = course1,
            EnrolledAt = DateTime.UtcNow
        };

        var enrollment2 = new Enrollment
        {
            Student = student2,
            Course = course2,
            EnrolledAt = DateTime.UtcNow
        };

        context.Students.AddRange(student1, student2);
        context.Courses.AddRange(course1, course2);
        context.Enrollments.AddRange(enrollment1, enrollment2);

        context.SaveChanges();
    }
}
