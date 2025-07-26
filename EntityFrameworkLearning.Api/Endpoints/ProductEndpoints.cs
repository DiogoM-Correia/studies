using Microsoft.EntityFrameworkCore;
using EntityFrameworkLearning.Api.Data;
using EntityFrameworkLearning.Api.DTOs;
using Microsoft.OpenApi.Models;

namespace EntityFrameworkLearning.Api.Endpoints;

public static class ProductEndpoints
{
    public static void MapProductEndpoints(this WebApplication app)
    {
        var group = app.MapGroup("/products")
            .WithTags("Products")
            .WithOpenApi();

        // GET all products
        group.MapGet("/", async (ApplicationDbContext context) =>
        {
            var products = await context.Products
                .Include(p => p.Category)
                .ToListAsync();
            
            return Results.Ok(products.Select(p => p.ToDto()));
        })
        .WithName("GetProducts")
        .WithOpenApi(operation =>
        {
            operation.Summary = "Get all products";
            operation.Description = "Retrieves all products with their category information.";
            return operation;
        });

        // GET product by ID
        group.MapGet("/{id}", async (int id, ApplicationDbContext context) =>
        {
            var product = await context.Products
                .Include(p => p.Category)
                .FirstOrDefaultAsync(p => p.Id == id);
            
            if (product == null)
                return Results.NotFound($"Product with ID {id} not found");
            
            return Results.Ok(product.ToDto());
        })
        .WithName("GetProductById")
        .WithOpenApi(operation =>
        {
            operation.Summary = "Get product by ID";
            operation.Description = "Retrieves a specific product by its ID with category information.";
            return operation;
        });

        // POST create new product
        group.MapPost("/", async (CreateProductDto createDto, ApplicationDbContext context) =>
        {
            // Validate that category exists
            var category = await context.Categories.FindAsync(createDto.CategoryId);
            if (category == null)
                return Results.BadRequest($"Category with ID {createDto.CategoryId} not found");
            
            var product = new Models.Product
            {
                Name = createDto.Name,
                Description = createDto.Description,
                Price = createDto.Price,
                CategoryId = createDto.CategoryId,
                CreatedAt = DateTime.UtcNow
            };
            
            context.Products.Add(product);
            await context.SaveChangesAsync();
            
            // Reload with category for response
            await context.Entry(product).Reference(p => p.Category).LoadAsync();
            
            return Results.Created($"/products/{product.Id}", product.ToDto());
        })
        .WithName("CreateProduct")
        .WithOpenApi(operation =>
        {
            operation.Summary = "Create a new product";
            operation.Description = "Creates a new product with the provided information. Validates that the category exists.";
            return operation;
        });

        // PUT update product
        group.MapPut("/{id}", async (int id, UpdateProductDto updateDto, ApplicationDbContext context) =>
        {
            var product = await context.Products.FindAsync(id);
            if (product == null)
                return Results.NotFound($"Product with ID {id} not found");
            
            // Validate category if provided
            if (updateDto.CategoryId.HasValue)
            {
                var category = await context.Categories.FindAsync(updateDto.CategoryId.Value);
                if (category == null)
                    return Results.BadRequest($"Category with ID {updateDto.CategoryId.Value} not found");
            }
            
            // Update only provided properties
            if (updateDto.Name != null)
                product.Name = updateDto.Name;
            if (updateDto.Description != null)
                product.Description = updateDto.Description;
            if (updateDto.Price.HasValue)
                product.Price = updateDto.Price.Value;
            if (updateDto.CategoryId.HasValue)
                product.CategoryId = updateDto.CategoryId.Value;
            
            await context.SaveChangesAsync();
            
            // Reload with category for response
            await context.Entry(product).Reference(p => p.Category).LoadAsync();
            
            return Results.Ok(product.ToDto());
        })
        .WithName("UpdateProduct")
        .WithOpenApi(operation =>
        {
            operation.Summary = "Update a product";
            operation.Description = "Updates an existing product. Only provided properties will be updated.";
            return operation;
        });

        // DELETE product
        group.MapDelete("/{id}", async (int id, ApplicationDbContext context) =>
        {
            var product = await context.Products.FindAsync(id);
            if (product == null)
                return Results.NotFound($"Product with ID {id} not found");
            
            context.Products.Remove(product);
            await context.SaveChangesAsync();
            
            return Results.Ok($"Product with ID {id} deleted successfully");
        })
        .WithName("DeleteProduct")
        .WithOpenApi(operation =>
        {
            operation.Summary = "Delete a product";
            operation.Description = "Deletes a product by its ID.";
            return operation;
        });
    }
} 