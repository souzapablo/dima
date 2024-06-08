using Dima.Api.Data;
using Dima.Core.Models;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);
var connectionString = builder
    .Configuration
    .GetConnectionString("DefaultConnection") ?? string.Empty;

builder.Services.AddDbContext<AppDbContext>(options =>
    {
        options.UseSqlServer(connectionString);
    });

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(x =>
{
    x.CustomSchemaIds(n => n.FullName);
});
builder.Services.AddTransient<Hanlder>();
var app = builder.Build();

app.MapPost(
        "/v1/categories",
        (Request request, Hanlder handler)
        => handler.Handle(request))
    .WithName("Categories: Create")
    .WithDescription("Cria uma nova categoria")
    .Produces<Response>();

app.UseSwagger();
app.UseSwaggerUI();

app.Run();

public class Request
{
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
}

public class Response
{
    public long Id { get; set; }
    public string Title { get; set; } = string.Empty;
}

public class Hanlder(AppDbContext context)
{
    public Response Handle(Request request)
    {
        var category = new Category
        {
            Title = request.Title,
            Description = request.Description
        };
        
        context.Categories.Add(category);
        context.SaveChanges();

        return new Response
        {
            Id = category.Id,
            Title = category.Title
        };
    }
}