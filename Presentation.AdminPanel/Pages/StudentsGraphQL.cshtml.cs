using GraphQL;
using GraphQL.Client.Abstractions;
using GraphQL.Client.Http;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Net.Http.Headers;

public class StudentsGraphQLModel : PageModel
{
   



    private readonly IGraphQLClient _client;

    public StudentsGraphQLModel(IGraphQLClient client)
    {
        _client = client;
    }


    public List<StudentVm> Students { get; set; } = new();

    public async Task OnGetAsync()
    {
        if (_client is GraphQLHttpClient httpClient)
        {
            httpClient.HttpClient.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Bearer", Request.Cookies["access_token"]);
        }

        var request = new GraphQLRequest
        {
            Query = @"
            query {
                students {
                    id
                    firstName
                    lastName
                    birthDate
                }
            }"
        };

        var response = await _client.SendQueryAsync<ResponseWrapper>(request);
        Students = response.Data?.Students ?? new();
    }


    public class ResponseWrapper
    {
        public List<StudentVm> Students { get; set; } = new();
    }

    public class StudentVm
    {
        public Guid Id { get; set; }
        public string FirstName { get; set; } = "";
        public string LastName { get; set; } = "";
        public DateTime BirthDate { get; set; }
    }
}
