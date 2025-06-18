using Domain.Entities;
using HotChocolate;
using Infrastructure.Persistance;

public class Mutation
{
    public async Task<Student> AddStudent(
        string firstName, string lastName, DateTime birthDate,
        [Service] AppDbContext db)
    {
        var student = new Student { FirstName = firstName, LastName = lastName, BirthDate = birthDate };
        db.Students.Add(student);
        await db.SaveChangesAsync();
        return student;
    }

    public async Task<Course> AddCourse(
        string title, string description,
        [Service] AppDbContext db)
    {
        var course = new Course { Title = title, Description = description };
        db.Courses.Add(course);
        await db.SaveChangesAsync();
        return course;
    }

    public async Task<Enrollment> EnrollStudent(
        Guid studentId, Guid courseId,
        [Service] AppDbContext db)
    {
        var enrollment = new Enrollment { StudentId = studentId, CourseId = courseId };
        db.Enrollments.Add(enrollment);
        await db.SaveChangesAsync();
        return enrollment;
    }
}
