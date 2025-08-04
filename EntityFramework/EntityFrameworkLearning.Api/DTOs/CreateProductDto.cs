using System.ComponentModel.DataAnnotations;

namespace EntityFrameworkLearning.Api.DTOs;

/// <summary>
/// Data transfer object for creating a new product
/// </summary>
public class CreateProductDto
{
    /// <summary>
    /// Name of the product (required, max 100 characters)
    /// </summary>
    /// <example>Gaming Mouse</example>
    [Required]
    [MaxLength(100)]
    public string Name { get; set; } = string.Empty;
    
    /// <summary>
    /// Optional description of the product (max 500 characters)
    /// </summary>
    /// <example>High-performance gaming mouse with adjustable DPI</example>
    [MaxLength(500)]
    public string? Description { get; set; }
    
    /// <summary>
    /// Price of the product (must be greater than or equal to 0)
    /// </summary>
    /// <example>79.99</example>
    [Range(0, double.MaxValue, ErrorMessage = "Price must be greater than or equal to 0")]
    public decimal Price { get; set; }
    
    /// <summary>
    /// ID of the category this product belongs to (must be greater than 0)
    /// </summary>
    /// <example>1</example>
    [Range(1, int.MaxValue, ErrorMessage = "CategoryId must be greater than 0")]
    public int CategoryId { get; set; }
} 