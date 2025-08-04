namespace EntityFrameworkLearning.Api.DTOs;

/// <summary>
/// Data transfer object for product information returned by the API
/// </summary>
public class ProductDto
{
    /// <summary>
    /// Unique identifier for the product
    /// </summary>
    /// <example>1</example>
    public int Id { get; set; }
    
    /// <summary>
    /// Name of the product
    /// </summary>
    /// <example>Gaming Laptop</example>
    public string Name { get; set; } = string.Empty;
    
    /// <summary>
    /// Optional description of the product
    /// </summary>
    /// <example>High-performance gaming laptop with latest graphics</example>
    public string? Description { get; set; }
    
    /// <summary>
    /// Price of the product in decimal format
    /// </summary>
    /// <example>999.99</example>
    public decimal Price { get; set; }
    
    /// <summary>
    /// Foreign key reference to the category this product belongs to
    /// </summary>
    /// <example>1</example>
    public int CategoryId { get; set; }
    
    /// <summary>
    /// Category information for this product
    /// </summary>
    public CategoryDto Category { get; set; } = null!;
    
    /// <summary>
    /// Date and time when the product was created
    /// </summary>
    /// <example>2024-01-15T10:30:00Z</example>
    public DateTime CreatedAt { get; set; }
} 