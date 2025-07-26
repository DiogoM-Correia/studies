using EntityFrameworkLearning.Api.Models;

namespace EntityFrameworkLearning.Api.DTOs;

public static class EntityExtensions
{
    public static CategoryDto ToDto(this Category category)
    {
        return new CategoryDto
        {
            Id = category.Id,
            Name = category.Name,
            Description = category.Description
        };
    }
    
    public static ProductDto ToDto(this Product product)
    {
        return new ProductDto
        {
            Id = product.Id,
            Name = product.Name,
            Description = product.Description,
            Price = product.Price,
            CategoryId = product.CategoryId,
            Category = product.Category?.ToDto() ?? new CategoryDto(),
            CreatedAt = product.CreatedAt
        };
    }
    
    public static CategoryWithProductsDto ToDtoWithProducts(this Category category)
    {
        return new CategoryWithProductsDto
        {
            Id = category.Id,
            Name = category.Name,
            Description = category.Description,
            Products = category.Products.Select(p => p.ToDto()).ToList()
        };
    }
} 