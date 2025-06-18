using Infrastructure.Persistance;
using Microsoft.EntityFrameworkCore;
using HotChocolate.AspNetCore;
using HotChocolate.Data;
using HotChocolate.Types.Pagination;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddPooledDbContextFactory<AppDbContext>(options =>
    options.UseInMemoryDatabase("GraphQLDb"));


builder.Services
    .AddGraphQLServer()
    .AddQueryType<Query>()
    .ModifyRequestOptions(opt => opt.IncludeExceptionDetails = true)
    .AddMutationType<Mutation>()
    .AddProjections()
    .AddFiltering()
    .AddSorting()
    .SetPagingOptions(new PagingOptions { MaxPageSize = 50 })
    .RegisterDbContext<AppDbContext>();

var app = builder.Build();


using (var scope = app.Services.CreateScope())
{
    var factory = scope.ServiceProvider.GetRequiredService<IDbContextFactory<AppDbContext>>();
    using var db = factory.CreateDbContext();
    DbSeeder.Seed(db);
}



app.MapGraphQL();

app.Run();
