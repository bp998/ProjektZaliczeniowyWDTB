using Domain.Entities;
using HotChocolate.Types;

public class CourseType : ObjectType<Course>
{
    protected override void Configure(IObjectTypeDescriptor<Course> descriptor)
    {
        descriptor.Description("Reprezentuje kurs.");

        descriptor.Field(c => c.Id);
        descriptor.Field(c => c.Title);
        descriptor.Field(c => c.Description);
        descriptor.Field(c => c.Enrollments).Ignore();
    }
}
