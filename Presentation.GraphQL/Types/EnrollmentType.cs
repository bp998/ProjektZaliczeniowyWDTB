using Domain.Entities;
using HotChocolate.Types;
using Infrastructure.Persistance;

public class EnrollmentType : ObjectType<Enrollment>
{
    protected override void Configure(IObjectTypeDescriptor<Enrollment> descriptor)
    {
        descriptor.Description("Reprezentuje zapis na kurs.");

        descriptor.Field(e => e.Id);
        descriptor.Field(e => e.EnrolledAt);

        descriptor
            .Field(e => e.Student)
            .ResolveWith<Resolvers>(r => r.GetStudent(default!, default!))
            .UseDbContext<AppDbContext>()
            .Description("Student zapisany na kurs");

        descriptor
            .Field(e => e.Course)
            .ResolveWith<Resolvers>(r => r.GetCourse(default!, default!))
            .UseDbContext<AppDbContext>()
            .Description("Kurs, na który zapisano studenta");
    }

    private class Resolvers
    {
        public Student GetStudent(Enrollment enrollment, [ScopedService] AppDbContext db) =>
            db.Students.First(s => s.Id == enrollment.StudentId);

        public Course GetCourse(Enrollment enrollment, [ScopedService] AppDbContext db) =>
            db.Courses.First(c => c.Id == enrollment.CourseId);
    }
}
