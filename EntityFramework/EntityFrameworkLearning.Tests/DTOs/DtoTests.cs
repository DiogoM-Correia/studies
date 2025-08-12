using EntityFrameworkLearning.Api.DTOs;
using FluentAssertions;

namespace EntityFrameworkLearning.Tests.DTOs;

public class DtoTests
{
    [Fact]
    public void CategoryDto_ShouldHaveCorrectProperties()
    {
        // Arrange & Act
        var categoryDto = new CategoryDto
        {
            Id = 1,
            Name = "Test Category",
            Description = "Test Description"
        };

        // Assert
        categoryDto.Id.Should().Be(1);
        categoryDto.Name.Should().Be("Test Category");
        categoryDto.Description.Should().Be("Test Description");
    }

    [Fact]
    public void CategoryDto_ShouldAllowNullDescription()
    {
        // Arrange & Act
        var categoryDto = new CategoryDto
        {
            Id = 1,
            Name = "Test Category",
            Description = null
        };

        // Assert
        categoryDto.Description.Should().BeNull();
    }

    [Fact]
    public void ProductDto_ShouldHaveCorrectProperties()
    {
        // Arrange & Act
        var categoryDto = new CategoryDto
        {
            Id = 1,
            Name = "Test Category",
            Description = "Test Description"
        };

        var productDto = new ProductDto
        {
            Id = 1,
            Name = "Test Product",
            Description = "Test Description",
            Price = 99.99m,
            CategoryId = 1,
            Category = categoryDto,
            CreatedAt = DateTime.UtcNow
        };

        // Assert
        productDto.Id.Should().Be(1);
        productDto.Name.Should().Be("Test Product");
        productDto.Description.Should().Be("Test Description");
        productDto.Price.Should().Be(99.99m);
        productDto.CategoryId.Should().Be(1);
        productDto.Category.Should().NotBeNull();
        productDto.Category.Id.Should().Be(1);
        productDto.CreatedAt.Should().BeCloseTo(DateTime.UtcNow, TimeSpan.FromSeconds(1));
    }

    [Fact]
    public void ProductDto_ShouldAllowNullDescription()
    {
        // Arrange & Act
        var productDto = new ProductDto
        {
            Id = 1,
            Name = "Test Product",
            Description = null,
            Price = 99.99m,
            CategoryId = 1,
            Category = new CategoryDto(),
            CreatedAt = DateTime.UtcNow
        };

        // Assert
        productDto.Description.Should().BeNull();
    }

    [Fact]
    public void CategoryWithProductsDto_ShouldHaveCorrectProperties()
    {
        // Arrange & Act
        var products = new List<ProductDto>
        {
            new ProductDto
            {
                Id = 1,
                Name = "Product 1",
                Price = 99.99m,
                CategoryId = 1,
                Category = new CategoryDto(),
                CreatedAt = DateTime.UtcNow
            },
            new ProductDto
            {
                Id = 2,
                Name = "Product 2",
                Price = 149.99m,
                CategoryId = 1,
                Category = new CategoryDto(),
                CreatedAt = DateTime.UtcNow
            }
        };

        var categoryWithProductsDto = new CategoryWithProductsDto
        {
            Id = 1,
            Name = "Test Category",
            Description = "Test Description",
            Products = products
        };

        // Assert
        categoryWithProductsDto.Id.Should().Be(1);
        categoryWithProductsDto.Name.Should().Be("Test Category");
        categoryWithProductsDto.Description.Should().Be("Test Description");
        categoryWithProductsDto.Products.Should().HaveCount(2);
        categoryWithProductsDto.Products.First().Name.Should().Be("Product 1");
        categoryWithProductsDto.Products.Last().Name.Should().Be("Product 2");
    }

    [Fact]
    public void CategoryWithProductsDto_ShouldHandleEmptyProductsList()
    {
        // Arrange & Act
        var categoryWithProductsDto = new CategoryWithProductsDto
        {
            Id = 1,
            Name = "Empty Category",
            Description = "Category with no products",
            Products = new List<ProductDto>()
        };

        // Assert
        categoryWithProductsDto.Products.Should().NotBeNull();
        categoryWithProductsDto.Products.Should().BeEmpty();
    }

    [Fact]
    public void CreateProductDto_ShouldHaveCorrectProperties()
    {
        // Arrange & Act
        var createProductDto = new CreateProductDto
        {
            Name = "New Product",
            Description = "New Product Description",
            Price = 199.99m,
            CategoryId = 1
        };

        // Assert
        createProductDto.Name.Should().Be("New Product");
        createProductDto.Description.Should().Be("New Product Description");
        createProductDto.Price.Should().Be(199.99m);
        createProductDto.CategoryId.Should().Be(1);
    }

    [Fact]
    public void CreateProductDto_ShouldAllowNullDescription()
    {
        // Arrange & Act
        var createProductDto = new CreateProductDto
        {
            Name = "New Product",
            Description = null,
            Price = 199.99m,
            CategoryId = 1
        };

        // Assert
        createProductDto.Description.Should().BeNull();
    }

    [Fact]
    public void UpdateProductDto_ShouldHaveCorrectProperties()
    {
        // Arrange & Act
        var updateProductDto = new UpdateProductDto
        {
            Name = "Updated Product",
            Description = "Updated Description",
            Price = 299.99m,
            CategoryId = 2
        };

        // Assert
        updateProductDto.Name.Should().Be("Updated Product");
        updateProductDto.Description.Should().Be("Updated Description");
        updateProductDto.Price.Should().Be(299.99m);
        updateProductDto.CategoryId.Should().Be(2);
    }

    [Fact]
    public void UpdateProductDto_ShouldAllowNullValues()
    {
        // Arrange & Act
        var updateProductDto = new UpdateProductDto
        {
            Name = null,
            Description = null,
            Price = null,
            CategoryId = null
        };

        // Assert
        updateProductDto.Name.Should().BeNull();
        updateProductDto.Description.Should().BeNull();
        updateProductDto.Price.Should().BeNull();
        updateProductDto.CategoryId.Should().BeNull();
    }

    [Fact]
    public void UpdateProductDto_ShouldAllowPartialUpdates()
    {
        // Arrange & Act
        var updateProductDto = new UpdateProductDto
        {
            Name = "Only Name Updated"
            // Other properties are null, allowing partial updates
        };

        // Assert
        updateProductDto.Name.Should().Be("Only Name Updated");
        updateProductDto.Description.Should().BeNull();
        updateProductDto.Price.Should().BeNull();
        updateProductDto.CategoryId.Should().BeNull();
    }

    [Fact]
    public void DTOs_ShouldHandleEdgeCases()
    {
        // Arrange & Act
        var categoryDto = new CategoryDto
        {
            Id = 0,
            Name = "",
            Description = ""
        };

        var productDto = new ProductDto
        {
            Id = 0,
            Name = "",
            Description = "",
            Price = 0.00m,
            CategoryId = 0,
            Category = new CategoryDto(),
            CreatedAt = DateTime.MinValue
        };

        // Assert
        categoryDto.Id.Should().Be(0);
        categoryDto.Name.Should().Be("");
        categoryDto.Description.Should().Be("");

        productDto.Id.Should().Be(0);
        productDto.Name.Should().Be("");
        productDto.Description.Should().Be("");
        productDto.Price.Should().Be(0.00m);
        productDto.CategoryId.Should().Be(0);
        productDto.CreatedAt.Should().Be(DateTime.MinValue);
    }

    [Fact]
    public void DTOs_ShouldHandleLargeValues()
    {
        // Arrange & Act
        var largeId = int.MaxValue;
        var largePrice = decimal.MaxValue;
        var longName = new string('A', 100);
        var longDescription = new string('B', 500);

        var categoryDto = new CategoryDto
        {
            Id = largeId,
            Name = longName,
            Description = longDescription
        };

        var productDto = new ProductDto
        {
            Id = largeId,
            Name = longName,
            Description = longDescription,
            Price = largePrice,
            CategoryId = largeId,
            Category = new CategoryDto(),
            CreatedAt = DateTime.MaxValue
        };

        // Assert
        categoryDto.Id.Should().Be(int.MaxValue);
        categoryDto.Name.Should().HaveLength(100);
        categoryDto.Description.Should().HaveLength(500);

        productDto.Id.Should().Be(int.MaxValue);
        productDto.Name.Should().HaveLength(100);
        productDto.Description.Should().HaveLength(500);
        productDto.Price.Should().Be(decimal.MaxValue);
        productDto.CategoryId.Should().Be(int.MaxValue);
        productDto.CreatedAt.Should().Be(DateTime.MaxValue);
    }
}
