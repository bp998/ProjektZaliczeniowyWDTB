using Domain.Entities;
using HotChocolate.Types;

public class StudentType : ObjectType<Student>
{
    protected override void Configure(IObjectTypeDescriptor<Student> descriptor)
    {
        descriptor.Description("Reprezentuje studenta.");

        descriptor
            .Field(s => s.Id)
            .Description("Identyfikator studenta");

        descriptor
            .Field(s => s.FirstName)
            .Description("Imię studenta");

        descriptor
            .Field(s => s.LastName)
            .Description("Nazwisko studenta");

        descriptor
            .Field(s => s.BirthDate)
            .Description("Data urodzenia");

        descriptor
            .Field(s => s.Enrollments)
            .Ignore();
    }
}
