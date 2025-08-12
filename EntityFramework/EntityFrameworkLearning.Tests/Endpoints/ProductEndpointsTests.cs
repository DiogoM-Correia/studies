using Microsoft.EntityFrameworkCore;
using EntityFrameworkLearning.Api.Data;
using EntityFrameworkLearning.Api.DTOs;
using EntityFrameworkLearning.Api.Models;
using FluentAssertions;
using Microsoft.AspNetCore.Http.HttpResults;

namespace EntityFrameworkLearning.Tests.Endpoints;

public class ProductEndpointsTests
{
    private DbContextOptions<ApplicationDbContext> CreateNewContextOptions()
    {
        return new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;
    }

    private async Task<ApplicationDbContext> CreateContextWithSeedDataAsync()
    {
        var options = CreateNewContextOptions();
        var context = new ApplicationDbContext(options);
        await context.Database.EnsureCreatedAsync();
        return context;
    }

    [Fact]
    public async Task GetProducts_ShouldReturnAllProducts()
    {
        // Arrange
        using var context = await CreateContextWithSeedDataAsync();

        // Act
        var products = await context.Products
            .Include(p => p.Category)
            .ToListAsync();
        
        var result = products.Select(p => p.ToDto()).ToList();

        // Assert
        result.Should().HaveCount(5);
        result.Should().Contain(p => p.Name == "Laptop");
        result.Should().Contain(p => p.Name == "Smartphone");
        result.Should().Contain(p => p.Name == "Programming Book");
        result.Should().Contain(p => p.Name == "T-Shirt");
        result.Should().Contain(p => p.Name == "Garden Tool Set");
        
        // Verify category information is included
        var laptop = result.First(p => p.Name == "Laptop");
        laptop.Category.Should().NotBeNull();
        laptop.Category.Name.Should().Be("Electronics");
    }

    [Fact]
    public async Task GetProductById_ShouldReturnProduct_WhenProductExists()
    {
        // Arrange
        using var context = await CreateContextWithSeedDataAsync();
        var productId = 1;

        // Act
        var product = await context.Products
            .Include(p => p.Category)
            .FirstOrDefaultAsync(p => p.Id == productId);
        
        if (product == null)
            throw new InvalidOperationException("Product not found");

        var result = product.ToDto();

        // Assert
        result.Should().NotBeNull();
        result.Id.Should().Be(productId);
        result.Name.Should().Be("Laptop");
        result.Category.Should().NotBeNull();
        result.Category.Name.Should().Be("Electronics");
    }

    [Fact]
    public async Task GetProductById_ShouldReturnNull_WhenProductDoesNotExist()
    {
        // Arrange
        using var context = await CreateContextWithSeedDataAsync();
        var nonExistentProductId = 999;

        // Act
        var product = await context.Products
            .Include(p => p.Category)
            .FirstOrDefaultAsync(p => p.Id == nonExistentProductId);

        // Assert
        product.Should().BeNull();
    }

    [Fact]
    public async Task CreateProduct_ShouldAddNewProduct_WhenValidDataProvided()
    {
        // Arrange
        using var context = await CreateContextWithSeedDataAsync();
        var createDto = new CreateProductDto
        {
            Name = "New Test Product",
            Description = "A new test product",
            Price = 149.99m,
            CategoryId = 1 // Electronics category
        };

        // Act
        var category = await context.Categories.FindAsync(createDto.CategoryId);
        if (category == null)
            throw new InvalidOperationException("Category not found");

        var product = new Product
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

        // Assert
        product.Id.Should().BeGreaterThan(0);
        product.Name.Should().Be(createDto.Name);
        product.Description.Should().Be(createDto.Description);
        product.Price.Should().Be(createDto.Price);
        product.CategoryId.Should().Be(createDto.CategoryId);
        product.Category.Should().NotBeNull();
        product.Category.Name.Should().Be("Electronics");
    }

    [Fact]
    public async Task CreateProduct_ShouldFail_WhenCategoryDoesNotExist()
    {
        // Arrange
        using var context = await CreateContextWithSeedDataAsync();
        var createDto = new CreateProductDto
        {
            Name = "New Test Product",
            Description = "A new test product",
            Price = 149.99m,
            CategoryId = 999 // Non-existent category
        };

        // Act
        var category = await context.Categories.FindAsync(createDto.CategoryId);

        // Assert
        category.Should().BeNull();
    }

    [Fact]
    public async Task UpdateProduct_ShouldUpdateProduct_WhenValidDataProvided()
    {
        // Arrange
        using var context = await CreateContextWithSeedDataAsync();
        var productId = 1;
        var updateDto = new UpdateProductDto
        {
            Name = "Updated Laptop",
            Price = 1299.99m
        };

        // Act
        var product = await context.Products.FindAsync(productId);
        if (product == null)
            throw new InvalidOperationException("Product not found");

        // Update only provided properties
        if (updateDto.Name != null)
            product.Name = updateDto.Name;
        if (updateDto.Price.HasValue)
            product.Price = updateDto.Price.Value;

        await context.SaveChangesAsync();

        // Reload with category for response
        await context.Entry(product).Reference(p => p.Category).LoadAsync();

        // Assert
        product.Name.Should().Be("Updated Laptop");
        product.Price.Should().Be(1299.99m);
        product.Description.Should().Be("High-performance laptop with latest specs"); // Should remain unchanged
        product.Category.Should().NotBeNull();
    }

    [Fact]
    public async Task UpdateProduct_ShouldFail_WhenProductDoesNotExist()
    {
        // Arrange
        using var context = await CreateContextWithSeedDataAsync();
        var nonExistentProductId = 999;

        // Act
        var product = await context.Products.FindAsync(nonExistentProductId);

        // Assert
        product.Should().BeNull();
    }

    [Fact]
    public async Task UpdateProduct_ShouldFail_WhenCategoryDoesNotExist()
    {
        // Arrange
        using var context = await CreateContextWithSeedDataAsync();
        var productId = 1;
        var updateDto = new UpdateProductDto
        {
            CategoryId = 999 // Non-existent category
        };

        // Act
        var product = await context.Products.FindAsync(productId);
        if (product == null)
            throw new InvalidOperationException("Product not found");

        var category = await context.Categories.FindAsync(updateDto.CategoryId);

        // Assert
        category.Should().BeNull();
    }

    [Fact]
    public async Task DeleteProduct_ShouldRemoveProduct_WhenProductExists()
    {
        // Arrange
        using var context = await CreateContextWithSeedDataAsync();
        var productId = 1;

        // Act
        var product = await context.Products.FindAsync(productId);
        if (product == null)
            throw new InvalidOperationException("Product not found");

        context.Products.Remove(product);
        await context.SaveChangesAsync();

        // Assert
        var deletedProduct = await context.Products.FindAsync(productId);
        deletedProduct.Should().BeNull();
    }

    [Fact]
    public async Task DeleteProduct_ShouldFail_WhenProductDoesNotExist()
    {
        // Arrange
        using var context = await CreateContextWithSeedDataAsync();
        var nonExistentProductId = 999;

        // Act
        var product = await context.Products.FindAsync(nonExistentProductId);

        // Assert
        product.Should().BeNull();
    }

    [Fact]
    public async Task ProductEndpoints_ShouldHandlePartialUpdates()
    {
        // Arrange
        using var context = await CreateContextWithSeedDataAsync();
        var productId = 1;
        var originalProduct = await context.Products.FindAsync(productId);
        if (originalProduct == null)
            throw new InvalidOperationException("Product not found");

        var originalName = originalProduct.Name;
        var originalPrice = originalProduct.Price;
        var originalDescription = originalProduct.Description;

        var updateDto = new UpdateProductDto
        {
            Price = 899.99m
            // Only updating price, other properties should remain unchanged
        };

        // Act
        var product = await context.Products.FindAsync(productId);
        if (product == null)
            throw new InvalidOperationException("Product not found");

        if (updateDto.Price.HasValue)
            product.Price = updateDto.Price.Value;

        await context.SaveChangesAsync();

        // Assert
        product.Name.Should().Be(originalName);
        product.Price.Should().Be(899.99m);
        product.Description.Should().Be(originalDescription);
    }

    [Fact]
    public async Task ProductEndpoints_ShouldMaintainDataIntegrity()
    {
        // Arrange
        using var context = await CreateContextWithSeedDataAsync();
        var initialProductCount = await context.Products.CountAsync();

        // Act - Create a new product
        var createDto = new CreateProductDto
        {
            Name = "Integrity Test Product",
            Description = "Testing data integrity",
            Price = 99.99m,
            CategoryId = 1
        };

        var category = await context.Categories.FindAsync(createDto.CategoryId);
        if (category == null)
            throw new InvalidOperationException("Category not found");

        var product = new Product
        {
            Name = createDto.Name,
            Description = createDto.Description,
            Price = createDto.Price,
            CategoryId = createDto.CategoryId,
            CreatedAt = DateTime.UtcNow
        };

        context.Products.Add(product);
        await context.SaveChangesAsync();

        // Assert
        var newProductCount = await context.Products.CountAsync();
        newProductCount.Should().Be(initialProductCount + 1);

        var savedProduct = await context.Products.FindAsync(product.Id);
        savedProduct.Should().NotBeNull();
        savedProduct!.Name.Should().Be(createDto.Name);
        savedProduct.Price.Should().Be(createDto.Price);
        savedProduct.CategoryId.Should().Be(createDto.CategoryId);
    }
}
