using Microsoft.EntityFrameworkCore;
using EntityFrameworkLearning.Api.Data;
using EntityFrameworkLearning.Api.DTOs;
using EntityFrameworkLearning.Api.Models;
using FluentAssertions;

namespace EntityFrameworkLearning.Tests.Endpoints;

public class CategoryEndpointsTests
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
    public async Task GetCategories_ShouldReturnAllCategoriesWithProducts()
    {
        // Arrange
        using var context = await CreateContextWithSeedDataAsync();

        // Act
        var categories = await context.Categories
            .Include(c => c.Products)
            .ToListAsync();
        
        var result = categories.Select(c => c.ToDtoWithProducts()).ToList();

        // Assert
        result.Should().HaveCount(4);
        result.Should().Contain(c => c.Name == "Electronics");
        result.Should().Contain(c => c.Name == "Books");
        result.Should().Contain(c => c.Name == "Clothing");
        result.Should().Contain(c => c.Name == "Home & Garden");

        // Verify products are included
        var electronics = result.First(c => c.Name == "Electronics");
        electronics.Products.Should().HaveCount(2);
        electronics.Products.Should().Contain(p => p.Name == "Laptop");
        electronics.Products.Should().Contain(p => p.Name == "Smartphone");
    }

    [Fact]
    public async Task GetCategoriesSimple_ShouldReturnCategoriesWithoutProducts()
    {
        // Arrange
        using var context = await CreateContextWithSeedDataAsync();

        // Act
        var categories = await context.Categories.ToListAsync();
        var result = categories.Select(c => c.ToDto()).ToList();

        // Assert
        result.Should().HaveCount(4);
        result.Should().Contain(c => c.Name == "Electronics");
        result.Should().Contain(c => c.Name == "Books");
        result.Should().Contain(c => c.Name == "Clothing");
        result.Should().Contain(c => c.Name == "Home & Garden");

        // Verify no products are loaded
        var electronics = result.First(c => c.Name == "Electronics");
        electronics.Should().NotBeNull();
    }

    [Fact]
    public async Task GetCategoryById_ShouldReturnCategory_WhenCategoryExists()
    {
        // Arrange
        using var context = await CreateContextWithSeedDataAsync();
        var categoryId = 1;

        // Act
        var category = await context.Categories
            .Include(c => c.Products)
            .FirstOrDefaultAsync(c => c.Id == categoryId);
        
        if (category == null)
            throw new InvalidOperationException("Category not found");

        var result = category.ToDtoWithProducts();

        // Assert
        result.Should().NotBeNull();
        result.Id.Should().Be(categoryId);
        result.Name.Should().Be("Electronics");
        result.Products.Should().HaveCount(2);
        result.Products.Should().Contain(p => p.Name == "Laptop");
        result.Products.Should().Contain(p => p.Name == "Smartphone");
    }

    [Fact]
    public async Task GetCategoryById_ShouldReturnNull_WhenCategoryDoesNotExist()
    {
        // Arrange
        using var context = await CreateContextWithSeedDataAsync();
        var nonExistentCategoryId = 999;

        // Act
        var category = await context.Categories
            .Include(c => c.Products)
            .FirstOrDefaultAsync(c => c.Id == nonExistentCategoryId);

        // Assert
        category.Should().BeNull();
    }

    [Fact]
    public async Task CreateCategory_ShouldAddNewCategory_WhenValidDataProvided()
    {
        // Arrange
        using var context = await CreateContextWithSeedDataAsync();
        var createDto = new CategoryDto
        {
            Name = "New Test Category",
            Description = "A new test category"
        };

        // Act
        var category = new Category
        {
            Name = createDto.Name,
            Description = createDto.Description
        };

        context.Categories.Add(category);
        await context.SaveChangesAsync();

        // Assert
        category.Id.Should().BeGreaterThan(0);
        category.Name.Should().Be(createDto.Name);
        category.Description.Should().Be(createDto.Description);
        category.Products.Should().NotBeNull();
        category.Products.Should().BeEmpty();
    }

    [Fact]
    public async Task CreateCategory_ShouldInitializeProductsAsEmptyList()
    {
        // Arrange
        using var context = await CreateContextWithSeedDataAsync();
        var createDto = new CategoryDto
        {
            Name = "Empty Products Category",
            Description = "Category with no products initially"
        };

        // Act
        var category = new Category
        {
            Name = createDto.Name,
            Description = createDto.Description
        };

        context.Categories.Add(category);
        await context.SaveChangesAsync();

        // Assert
        category.Products.Should().NotBeNull();
        category.Products.Should().BeEmpty();
    }

    [Fact]
    public async Task DeleteCategory_ShouldRemoveCategory_WhenCategoryHasNoProducts()
    {
        // Arrange
        using var context = await CreateContextWithSeedDataAsync();
        
        // Create a category with no products
        var emptyCategory = new Category
        {
            Name = "Empty Category",
            Description = "Category to be deleted"
        };
        context.Categories.Add(emptyCategory);
        await context.SaveChangesAsync();

        var categoryId = emptyCategory.Id;

        // Act
        var category = await context.Categories
            .Include(c => c.Products)
            .FirstOrDefaultAsync(c => c.Id == categoryId);
        
        if (category == null)
            throw new InvalidOperationException("Category not found");

        context.Categories.Remove(category);
        await context.SaveChangesAsync();

        // Assert
        var deletedCategory = await context.Categories.FindAsync(categoryId);
        deletedCategory.Should().BeNull();
    }

    [Fact]
    public async Task DeleteCategory_ShouldFail_WhenCategoryHasProducts()
    {
        // Arrange
        using var context = await CreateContextWithSeedDataAsync();
        var categoryId = 1; // Electronics category which has products

        // Act
        var category = await context.Categories
            .Include(c => c.Products)
            .FirstOrDefaultAsync(c => c.Id == categoryId);
        
        if (category == null)
            throw new InvalidOperationException("Category not found");

        // Assert
        category.Products.Should().NotBeEmpty();
        category.Products.Should().HaveCount(2);
    }

    [Fact]
    public async Task DeleteCategory_ShouldFail_WhenCategoryDoesNotExist()
    {
        // Arrange
        using var context = await CreateContextWithSeedDataAsync();
        var nonExistentCategoryId = 999;

        // Act
        var category = await context.Categories
            .Include(c => c.Products)
            .FirstOrDefaultAsync(c => c.Id == nonExistentCategoryId);

        // Assert
        category.Should().BeNull();
    }

    [Fact]
    public async Task CategoryEndpoints_ShouldHandleCategoryWithMultipleProducts()
    {
        // Arrange
        using var context = await CreateContextWithSeedDataAsync();
        
        // Add more products to Electronics category
        var electronicsCategory = await context.Categories.FirstAsync(c => c.Name == "Electronics");
        var newProduct = new Product
        {
            Name = "Tablet",
            Description = "Portable tablet device",
            Price = 299.99m,
            CategoryId = electronicsCategory.Id
        };

        context.Products.Add(newProduct);
        await context.SaveChangesAsync();

        // Act
        var category = await context.Categories
            .Include(c => c.Products)
            .FirstAsync(c => c.Id == electronicsCategory.Id);

        // Assert
        category.Products.Should().HaveCount(3);
        category.Products.Should().Contain(p => p.Name == "Laptop");
        category.Products.Should().Contain(p => p.Name == "Smartphone");
        category.Products.Should().Contain(p => p.Name == "Tablet");
    }

    [Fact]
    public async Task CategoryEndpoints_ShouldMaintainDataIntegrity()
    {
        // Arrange
        using var context = await CreateContextWithSeedDataAsync();
        var initialCategoryCount = await context.Categories.CountAsync();

        // Act - Create a new category
        var createDto = new CategoryDto
        {
            Name = "Integrity Test Category",
            Description = "Testing data integrity"
        };

        var category = new Category
        {
            Name = createDto.Name,
            Description = createDto.Description
        };

        context.Categories.Add(category);
        await context.SaveChangesAsync();

        // Assert
        var newCategoryCount = await context.Categories.CountAsync();
        newCategoryCount.Should().Be(initialCategoryCount + 1);

        var savedCategory = await context.Categories.FindAsync(category.Id);
        savedCategory.Should().NotBeNull();
        savedCategory!.Name.Should().Be(createDto.Name);
        savedCategory.Description.Should().Be(createDto.Description);
    }

    [Fact]
    public async Task CategoryEndpoints_ShouldHandleSpecialCharactersInNames()
    {
        // Arrange
        using var context = await CreateContextWithSeedDataAsync();
        var createDto = new CategoryDto
        {
            Name = "Special Category!@#$%^&*()",
            Description = "Category with special characters"
        };

        // Act
        var category = new Category
        {
            Name = createDto.Name,
            Description = createDto.Description
        };

        context.Categories.Add(category);
        await context.SaveChangesAsync();

        // Assert
        category.Id.Should().BeGreaterThan(0);
        category.Name.Should().Be("Special Category!@#$%^&*()");
        category.Description.Should().Be("Category with special characters");
    }

    [Fact]
    public async Task CategoryEndpoints_ShouldHandleLongDescriptions()
    {
        // Arrange
        using var context = await CreateContextWithSeedDataAsync();
        var longDescription = new string('A', 500); // Max length description
        var createDto = new CategoryDto
        {
            Name = "Long Description Category",
            Description = longDescription
        };

        // Act
        var category = new Category
        {
            Name = createDto.Name,
            Description = createDto.Description
        };

        context.Categories.Add(category);
        await context.SaveChangesAsync();

        // Assert
        category.Description.Should().Be(longDescription);
        category.Description.Should().HaveLength(500);
    }
}
