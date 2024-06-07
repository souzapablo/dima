using Dima.Api.Data;
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
        "/v1/transactions",
        (Request request, Hanlder handler)
        => handler.Handle(request))
    .WithName("Transactions: Create")
    .Produces<Response>();

app.UseSwagger();
app.UseSwaggerUI();

app.Run();

public class Request
{
    public string Title { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; } = DateTime.Now;
    public int Type { get; set; }
    public decimal Amount { get; set; }
    public long CategoryId { get; set; }
    public string UserId { get; set; } = string.Empty;
}

public class Response
{
    public long Id { get; set; }
    public string Title { get; set; } = string.Empty;
}

public class Hanlder
{
    public Response Handle(Request request)
    {
        return new Response
        {
            Id = 1,
            Title = request.Title
        };
    }
}