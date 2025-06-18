using GraphQL;
using GraphQL.Client.Abstractions;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Application.DTOs;

public class EnrollmentsGraphQLModel : PageModel
{
    private readonly IGraphQLClient _client;

    public List<EnrollmentDto> Enrollments { get; set; } = new();

    public EnrollmentsGraphQLModel(IGraphQLClient client)
    {
        _client = client;
    }

    public async Task OnGetAsync()
    {
        var query = new GraphQLRequest
        {
            Query = @"
            {
              enrollments {
                id
                enrolledAt
                course {
                  title
                }
                student {
                  firstName
                  lastName
                }
              }
            }"
        };

        var response = await _client.SendQueryAsync<Wrapper>(query);

        Enrollments = response.Data.Enrollments.Select(e => new EnrollmentDto
        {
            Id = e.Id,
            EnrolledAt = e.EnrolledAt,
            CourseTitle = e.Course.Title,
            StudentName = $"{e.Student.FirstName} {e.Student.LastName}"
        }).ToList();
    }

    public class Wrapper
    {
        public List<EnrollmentGql> Enrollments { get; set; } = new();
    }

    public class EnrollmentGql
    {
        public Guid Id { get; set; }
        public DateTime EnrolledAt { get; set; }
        public CourseDto Course { get; set; } = new();
        public StudentGql Student { get; set; } = new();
    }

    public class StudentGql
    {
        public string FirstName { get; set; } = "";
        public string LastName { get; set; } = "";
    }
}
