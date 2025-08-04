namespace EntityFrameworkLearning.Api.DTOs;

public class CategoryWithProductsDto
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    public ICollection<ProductDto> Products { get; set; } = new List<ProductDto>();
} 