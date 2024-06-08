using Dima.Api.Data;
using Dima.Core.Handlers;
using Dima.Core.Models;
using Dima.Core.Requests.Categories;
using Dima.Core.Responses;
using Microsoft.EntityFrameworkCore;

namespace Dima.Api.Handlers;

public class CategoryHandler(AppDbContext context) : ICategoryHandler
{
    public async Task<Response<Category?>> CreateAsync(CreateCategoryRequest request)
    {
        try
        {
            var category = new Category
            {
                Title = request.Title,
                Description = request.Description,
                UserId = request.UserId
            };

            await context.Categories.AddAsync(category);
            await context.SaveChangesAsync();

            return new Response<Category?>(category, 201, "Categoria criada com sucesso");
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex);
            return new Response<Category?>(null, 500, "Não foi possível criar a categoria");
        }

    }

    public async Task<Response<Category?>> DeleteAsync(DeleteCategoryRequest request)
    {
        try
        {
            var category = await context
                .Categories
                .FirstOrDefaultAsync(x => x.Id == request.Id && x.UserId == request.UserId);

            if (category is null)
                return new Response<Category?>(category, 400, "Categoria não encontrada");

            context.Categories.Remove(category);
            await context.SaveChangesAsync();

            return new Response<Category?>(category, 204, message: "Categoria deletada com sucesso");
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex);
            return new Response<Category?>(null, 500, "Não foi possível excluir a categoria");
        }
    }

    public async Task<PagedResponse<List<Category>>> GetAllAsync(GetAllCategoriesRequest request)
    {
        try
        {
            var query = context
                .Categories
                .AsNoTracking()
                .Where(x => x.UserId == request.UserId)
                .OrderBy(x => x.Title);

            var categories = await query
                .Skip((request.PageNumber - 1) * request.PageSize)
                .Take(request.PageSize)
                .ToListAsync();

            var count = await query.CountAsync();

            return new PagedResponse<List<Category>>(
                categories,
                count,
                request.PageNumber,
                request.PageSize);
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex);
            return new PagedResponse<List<Category>>(null, 500, "Não foi possível excluir a categoria");
        }
    }

    public async Task<Response<Category?>> GetByIdAsync(GetCategoryByIdRequest request)
    {
        try
        {
            var category = await context
                .Categories
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.Id == request.Id && x.UserId == request.UserId);

            return category is null
                ? new Response<Category?>(category, 400, "Categoria não encontrada")
                : new Response<Category?>(category);
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex);
            return new Response<Category?>(null, 500, "Não foi possível encontrar a categoria");
        }
    }

    public async Task<Response<Category?>> UpdateAsync(UpdateCategoryRequest request)
    {
        try
        {
            var category = await context
                .Categories
                .FirstOrDefaultAsync(x => x.Id == request.Id && x.UserId == request.UserId);

            if (category is null)
                return new Response<Category?>(category, 400, "Categoria não encontrada");

            category.Title = request.Title;
            category.Description = request.Description;

            context.Categories.Update(category);
            await context.SaveChangesAsync();

            return new Response<Category?>(category, message: "Categoria atualizada com sucesso");
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex);
            return new Response<Category?>(null, 500, "Não foi possível atualizar a categoria");
        }
    }
}