using System.ComponentModel.DataAnnotations;

namespace EntityFrameworkLearning.Api.DTOs;

public class UpdateProductDto
{
    [MaxLength(100)]
    public string? Name { get; set; }
    
    [MaxLength(500)]
    public string? Description { get; set; }
    
    [Range(0, double.MaxValue, ErrorMessage = "Price must be greater than or equal to 0")]
    public decimal? Price { get; set; }
    
    [Range(1, int.MaxValue, ErrorMessage = "CategoryId must be greater than 0")]
    public int? CategoryId { get; set; }
} 