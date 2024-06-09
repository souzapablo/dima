using System.Security.Claims;
using Dima.Api.Common.Api;
using Dima.Core.Handlers;
using Dima.Core.Requests.Categories;

namespace Dima.Api.Endpoints.Categories;

public class CreateCategoryEndpoint : IEndpoint
{
    public static void Map(IEndpointRouteBuilder app) =>
        app.MapPost("", HandleAsync)
            .WithName("Categories: Create")
            .WithSummary("Cria uma nova categoria")
            .WithDescription("Cria uma nova categoria")
            .WithOrder(1);

    private static async Task<IResult> HandleAsync(
        ClaimsPrincipal user,
        ICategoryHandler handler,
        CreateCategoryRequest request)
    {
        request.UserId = user.Identity?.Name ?? string.Empty;
        var result = await handler.CreateAsync(request);
        return result.IsSuccess
            ? TypedResults.Created($"/{result.Data?.Id}", result)
            : TypedResults.BadRequest(result);
    }
}