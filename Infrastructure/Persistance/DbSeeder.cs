using Domain.Entities;
using System.Text;
using System.Security.Cryptography;

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

        var admin = new User
        {
            Username = "admin",
            PasswordHash = HashPassword("admin123"),
            Role = "Admin"
        };

        var user = new User
        {
            Username = "user",
            PasswordHash = HashPassword("user123"),
            Role = "User"
        };

        context.Users.AddRange(admin, user);

        static string HashPassword(string password)
        {
            using var sha = SHA256.Create();
            var bytes = Encoding.UTF8.GetBytes(password);
            var hash = sha.ComputeHash(bytes);
            return Convert.ToBase64String(hash);
        }


        context.Students.AddRange(student1, student2);
        context.Courses.AddRange(course1, course2);
        context.Enrollments.AddRange(enrollment1, enrollment2);

        context.SaveChanges();
    }
}
