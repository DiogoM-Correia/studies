using EntityFrameworkLearning.Api.DTOs;
using EntityFrameworkLearning.Api.Models;
using FluentAssertions;

namespace EntityFrameworkLearning.Tests.DTOs;

public class EntityExtensionsTests
{
    [Fact]
    public void ToDto_ShouldConvertCategoryToCategoryDto()
    {
        // Arrange
        var category = new Category
        {
            Id = 1,
            Name = "Test Category",
            Description = "Test Description"
        };

        // Act
        var result = category.ToDto();

        // Assert
        result.Should().NotBeNull();
        result.Id.Should().Be(category.Id);
        result.Name.Should().Be(category.Name);
        result.Description.Should().Be(category.Description);
    }

    [Fact]
    public void ToDto_ShouldConvertProductToProductDto()
    {
        // Arrange
        var category = new Category
        {
            Id = 1,
            Name = "Test Category",
            Description = "Test Description"
        };

        var product = new Product
        {
            Id = 1,
            Name = "Test Product",
            Description = "Test Description",
            Price = 99.99m,
            CategoryId = 1,
            Category = category,
            CreatedAt = DateTime.UtcNow
        };

        // Act
        var result = product.ToDto();

        // Assert
        result.Should().NotBeNull();
        result.Id.Should().Be(product.Id);
        result.Name.Should().Be(product.Name);
        result.Description.Should().Be(product.Description);
        result.Price.Should().Be(product.Price);
        result.CategoryId.Should().Be(product.CategoryId);
        result.CreatedAt.Should().Be(product.CreatedAt);
        result.Category.Should().NotBeNull();
        result.Category.Id.Should().Be(category.Id);
        result.Category.Name.Should().Be(category.Name);
        result.Category.Description.Should().Be(category.Description);
    }

    [Fact]
    public void ToDto_ShouldHandleProductWithNullCategory()
    {
        // Arrange
        var product = new Product
        {
            Id = 1,
            Name = "Test Product",
            Description = "Test Description",
            Price = 99.99m,
            CategoryId = 1,
            Category = null!,
            CreatedAt = DateTime.UtcNow
        };

        // Act
        var result = product.ToDto();

        // Assert
        result.Should().NotBeNull();
        result.Category.Should().NotBeNull();
        result.Category.Id.Should().Be(0);
        result.Category.Name.Should().BeEmpty();
        result.Category.Description.Should().BeNull();
    }

    [Fact]
    public void ToDtoWithProducts_ShouldConvertCategoryWithProducts()
    {
        // Arrange
        var category = new Category
        {
            Id = 1,
            Name = "Test Category",
            Description = "Test Description"
        };

        var products = new List<Product>
        {
            new Product
            {
                Id = 1,
                Name = "Product 1",
                Price = 99.99m,
                CategoryId = 1,
                Category = category
            },
            new Product
            {
                Id = 2,
                Name = "Product 2",
                Price = 149.99m,
                CategoryId = 1,
                Category = category
            }
        };

        category.Products = products;

        // Act
        var result = category.ToDtoWithProducts();

        // Assert
        result.Should().NotBeNull();
        result.Id.Should().Be(category.Id);
        result.Name.Should().Be(category.Name);
        result.Description.Should().Be(category.Description);
        result.Products.Should().HaveCount(2);
        result.Products.First().Name.Should().Be("Product 1");
        result.Products.Last().Name.Should().Be("Product 2");
    }

    [Fact]
    public void ToDtoWithProducts_ShouldHandleCategoryWithNoProducts()
    {
        // Arrange
        var category = new Category
        {
            Id = 1,
            Name = "Test Category",
            Description = "Test Description"
        };

        // Act
        var result = category.ToDtoWithProducts();

        // Assert
        result.Should().NotBeNull();
        result.Products.Should().NotBeNull();
        result.Products.Should().BeEmpty();
    }

    [Fact]
    public void ToDto_ShouldPreserveAllCategoryProperties()
    {
        // Arrange
        var category = new Category
        {
            Id = 999,
            Name = "Complex Category Name",
            Description = "A very detailed description with special characters: !@#$%^&*()"
        };

        // Act
        var result = category.ToDto();

        // Assert
        result.Should().NotBeNull();
        result.Id.Should().Be(999);
        result.Name.Should().Be("Complex Category Name");
        result.Description.Should().Be("A very detailed description with special characters: !@#$%^&*()");
    }

    [Fact]
    public void ToDto_ShouldPreserveAllProductProperties()
    {
        // Arrange
        var category = new Category { Id = 1, Name = "Test Category" };
        var product = new Product
        {
            Id = 888,
            Name = "Premium Product",
            Description = "High-quality product with premium features",
            Price = 1999.99m,
            CategoryId = 1,
            Category = category,
            CreatedAt = new DateTime(2024, 1, 1, 12, 0, 0, DateTimeKind.Utc)
        };

        // Act
        var result = product.ToDto();

        // Assert
        result.Should().NotBeNull();
        result.Id.Should().Be(888);
        result.Name.Should().Be("Premium Product");
        result.Description.Should().Be("High-quality product with premium features");
        result.Price.Should().Be(1999.99m);
        result.CategoryId.Should().Be(1);
        result.CreatedAt.Should().Be(new DateTime(2024, 1, 1, 12, 0, 0, DateTimeKind.Utc));
    }
}
