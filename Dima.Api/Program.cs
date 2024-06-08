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