using Dima.Api.Data;
using Dima.Api.Handlers;
using Dima.Core.Handlers;
using Dima.Core.Models;
using Dima.Core.Requests.Categories;
using Dima.Core.Responses;
using Microsoft.AspNetCore.Mvc;
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
builder.Services.AddTransient<ICategoryHandler, CategoryHandler>();
var app = builder.Build();

app.MapPost(
        "/v1/categories",
        async (CreateCategoryRequest request,
        ICategoryHandler handler)
        => await handler.CreateAsync(request))
    .WithName("Categories: Create")
    .WithDescription("Cria uma nova categoria")
    .Produces<Response<Category?>>();

app.MapPut(
        "/v1/categories/{id}",
        async (long id,
            UpdateCategoryRequest request,
        ICategoryHandler handler)
        =>
        {
            request.Id = id;
            return await handler.UpdateAsync(request);
        })
    .WithName("Categories: Update")
    .WithDescription("Atualiza uma nova categoria")
    .Produces<Response<Category?>>();

app.MapDelete(
    "/v1/categories/{id}",
    async (long id,
    ICategoryHandler handler)
    =>
    {
        var request = new DeleteCategoryRequest
        {
            UserId = "Marcelo",
            Id = id
        };

        return await handler.DeleteAsync(request);
    })
.WithName("Categories: Delete")
.WithDescription("Exclui uma nova categoria")
.Produces<Response<Category?>>();

app.MapGet(
    "/v1/categories",
    async (
    ICategoryHandler handler)
    =>
    {
        var request = new GetAllCategoriesRequest
        {
            UserId = "Marcelo"
        };

        return await handler.GetAllAsync(request);
    })
.WithName("Categories: GetAll")
.WithDescription("Obtém todas as categoria de um usuário")
.Produces<PagedResponse<List<Category>?>>();

app.MapGet(
    "/v1/categories/{id}",
    async (long id,
    ICategoryHandler handler)
    =>
    {
        var request = new GetCategoryByIdRequest
        {
            UserId = "Marcelo",
            Id = id
        };

        return await handler.GetByIdAsync(request);
    })
.WithName("Categories: GetById")
.WithDescription("Obtém uma categoria pelo id")
.Produces<Response<Category?>>();
app.UseSwagger();
app.UseSwaggerUI();

app.Run();