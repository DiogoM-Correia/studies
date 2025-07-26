using Microsoft.EntityFrameworkCore;
using EntityFrameworkLearning.Api.Data;
using EntityFrameworkLearning.Api.DTOs;
using Microsoft.OpenApi.Models;

namespace EntityFrameworkLearning.Api.Endpoints;

public static class CategoryEndpoints
{
    public static void MapCategoryEndpoints(this WebApplication app)
    {
        var group = app.MapGroup("/categories")
            .WithTags("Categories")
            .WithOpenApi();

        // GET all categories with products
        group.MapGet("/", async (ApplicationDbContext context) =>
        {
            var categories = await context.Categories
                .Include(c => c.Products)
                .ToListAsync();
            
            return Results.Ok(categories.Select(c => c.ToDtoWithProducts()));
        })
        .WithName("GetCategories")
        .WithOpenApi(operation =>
        {
            operation.Summary = "Get all categories with products";
            operation.Description = "Retrieves all categories with their associated products.";
            return operation;
        });

        // GET categories without products (simple)
        group.MapGet("/simple", async (ApplicationDbContext context) =>
        {
            var categories = await context.Categories.ToListAsync();
            return Results.Ok(categories.Select(c => c.ToDto()));
        })
        .WithName("GetCategoriesSimple")
        .WithOpenApi(operation =>
        {
            operation.Summary = "Get all categories (simple)";
            operation.Description = "Retrieves all categories without their associated products for better performance.";
            return operation;
        });

        // GET category by ID
        group.MapGet("/{id}", async (int id, ApplicationDbContext context) =>
        {
            var category = await context.Categories
                .Include(c => c.Products)
                .FirstOrDefaultAsync(c => c.Id == id);
            
            if (category == null)
                return Results.NotFound($"Category with ID {id} not found");
            
            return Results.Ok(category.ToDtoWithProducts());
        })
        .WithName("GetCategoryById")
        .WithOpenApi(operation =>
        {
            operation.Summary = "Get category by ID";
            operation.Description = "Retrieves a specific category by its ID with all associated products.";
            return operation;
        });

        // POST create new category
        group.MapPost("/", async (CategoryDto createDto, ApplicationDbContext context) =>
        {
            var category = new Models.Category
            {
                Name = createDto.Name,
                Description = createDto.Description
            };
            
            context.Categories.Add(category);
            await context.SaveChangesAsync();
            
            return Results.Created($"/categories/{category.Id}", category.ToDto());
        })
        .WithName("CreateCategory")
        .WithOpenApi(operation =>
        {
            operation.Summary = "Create a new category";
            operation.Description = "Creates a new category with the provided information.";
            return operation;
        });

        // DELETE category (only if no products)
        group.MapDelete("/{id}", async (int id, ApplicationDbContext context) =>
        {
            var category = await context.Categories
                .Include(c => c.Products)
                .FirstOrDefaultAsync(c => c.Id == id);
            
            if (category == null)
                return Results.NotFound($"Category with ID {id} not found");
            
            if (category.Products.Any())
                return Results.BadRequest($"Cannot delete category '{category.Name}' because it has {category.Products.Count} products. Delete the products first.");
            
            context.Categories.Remove(category);
            await context.SaveChangesAsync();
            
            return Results.Ok($"Category with ID {id} deleted successfully");
        })
        .WithName("DeleteCategory")
        .WithOpenApi(operation =>
        {
            operation.Summary = "Delete a category";
            operation.Description = "Deletes a category by its ID. Categories with products cannot be deleted.";
            return operation;
        });
    }
} 