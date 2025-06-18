using GraphQL;
using GraphQL.Client.Abstractions;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Application.DTOs;

public class CoursesGraphQLModel : PageModel
{
    private readonly IGraphQLClient _client;

    public List<CourseDto> Courses { get; set; } = new();

    public CoursesGraphQLModel(IGraphQLClient client)
    {
        _client = client;
    }

    public async Task OnGetAsync()
    {
        var query = new GraphQLRequest
        {
            Query = @"
            {
              courses {
                id
                title
                description
              }
            }"
        };

        var response = await _client.SendQueryAsync<DataWrapper<CourseDto>>(
            query
        );

        Courses = response.Data.Courses;
    }

    public class DataWrapper<T>
    {
        public List<T> Courses { get; set; } = new();
    }
}
