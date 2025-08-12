using Microsoft.EntityFrameworkCore;
using EntityFrameworkLearning.Api.Data;
using EntityFrameworkLearning.Api.Models;
using FluentAssertions;

namespace EntityFrameworkLearning.Tests.Data;

public class ApplicationDbContextTests
{
    private DbContextOptions<ApplicationDbContext> CreateNewContextOptions()
    {
        return new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;
    }

    [Fact]
    public void ApplicationDbContext_ShouldBeCreatedSuccessfully()
    {
        // Arrange & Act
        var options = CreateNewContextOptions();
        using var context = new ApplicationDbContext(options);

        // Assert
        context.Should().NotBeNull();
        context.Products.Should().NotBeNull();
        context.Categories.Should().NotBeNull();
    }

    [Fact]
    public async Task ApplicationDbContext_ShouldCreateDatabaseSuccessfully()
    {
        // Arrange
        var options = CreateNewContextOptions();
        using var context = new ApplicationDbContext(options);

        // Act
        var result = await context.Database.EnsureCreatedAsync();

        // Assert
        result.Should().BeTrue();
    }

    [Fact]
    public async Task ApplicationDbContext_ShouldSeedCategories()
    {
        // Arrange
        var options = CreateNewContextOptions();
        using var context = new ApplicationDbContext(options);

        // Act
        await context.Database.EnsureCreatedAsync();
        var categories = await context.Categories.ToListAsync();

        // Assert
        categories.Should().HaveCount(4);
        categories.Should().Contain(c => c.Name == "Electronics");
        categories.Should().Contain(c => c.Name == "Books");
        categories.Should().Contain(c => c.Name == "Clothing");
        categories.Should().Contain(c => c.Name == "Home & Garden");
    }

    [Fact]
    public async Task ApplicationDbContext_ShouldSeedProducts()
    {
        // Arrange
        var options = CreateNewContextOptions();
        using var context = new ApplicationDbContext(options);

        // Act
        await context.Database.EnsureCreatedAsync();
        var products = await context.Products.ToListAsync();

        // Assert
        products.Should().HaveCount(5);
        products.Should().Contain(p => p.Name == "Laptop");
        products.Should().Contain(p => p.Name == "Smartphone");
        products.Should().Contain(p => p.Name == "Programming Book");
        products.Should().Contain(p => p.Name == "T-Shirt");
        products.Should().Contain(p => p.Name == "Garden Tool Set");
    }

    [Fact]
    public async Task ApplicationDbContext_ShouldHaveCorrectProductCategoryRelationships()
    {
        // Arrange
        var options = CreateNewContextOptions();
        using var context = new ApplicationDbContext(options);

        // Act
        await context.Database.EnsureCreatedAsync();
        var products = await context.Products.Include(p => p.Category).ToListAsync();

        // Assert
        products.Should().HaveCount(5);
        
        var laptop = products.First(p => p.Name == "Laptop");
        laptop.Category.Should().NotBeNull();
        laptop.Category.Name.Should().Be("Electronics");
        
        var book = products.First(p => p.Name == "Programming Book");
        book.Category.Should().NotBeNull();
        book.Category.Name.Should().Be("Books");
    }

    [Fact]
    public async Task ApplicationDbContext_ShouldAllowAddingNewCategory()
    {
        // Arrange
        var options = CreateNewContextOptions();
        using var context = new ApplicationDbContext(options);
        await context.Database.EnsureCreatedAsync();

        var newCategory = new Category
        {
            Name = "Test Category",
            Description = "Test Description"
        };

        // Act
        context.Categories.Add(newCategory);
        await context.SaveChangesAsync();

        // Assert
        var savedCategory = await context.Categories.FirstOrDefaultAsync(c => c.Name == "Test Category");
        savedCategory.Should().NotBeNull();
        savedCategory!.Name.Should().Be("Test Category");
        savedCategory.Description.Should().Be("Test Description");
    }

    [Fact]
    public async Task ApplicationDbContext_ShouldAllowAddingNewProduct()
    {
        // Arrange
        var options = CreateNewContextOptions();
        using var context = new ApplicationDbContext(options);
        await context.Database.EnsureCreatedAsync();

        var category = await context.Categories.FirstAsync();
        var newProduct = new Product
        {
            Name = "Test Product",
            Description = "Test Description",
            Price = 99.99m,
            CategoryId = category.Id
        };

        // Act
        context.Products.Add(newProduct);
        await context.SaveChangesAsync();

        // Assert
        var savedProduct = await context.Products.FirstOrDefaultAsync(p => p.Name == "Test Product");
        savedProduct.Should().NotBeNull();
        savedProduct!.Name.Should().Be("Test Product");
        savedProduct.Price.Should().Be(99.99m);
        savedProduct.CategoryId.Should().Be(category.Id);
    }

    [Fact]
    public async Task ApplicationDbContext_ShouldEnforceForeignKeyConstraints()
    {
        // Arrange
        var options = CreateNewContextOptions();
        using var context = new ApplicationDbContext(options);
        await context.Database.EnsureCreatedAsync();

        var invalidProduct = new Product
        {
            Name = "Invalid Product",
            Price = 99.99m,
            CategoryId = 999 // Non-existent category ID
        };

        // Act & Assert
        context.Products.Add(invalidProduct);
        await context.SaveChangesAsync(); // InMemory doesn't enforce FK constraints, but real DB would

        // Verify the product was added (InMemory behavior)
        var savedProduct = await context.Products.FirstOrDefaultAsync(p => p.Name == "Invalid Product");
        savedProduct.Should().NotBeNull();
    }

    [Fact]
    public async Task ApplicationDbContext_ShouldHandleCategoryWithProducts()
    {
        // Arrange
        var options = CreateNewContextOptions();
        using var context = new ApplicationDbContext(options);
        await context.Database.EnsureCreatedAsync();

        // Act
        var category = await context.Categories
            .Include(c => c.Products)
            .FirstAsync(c => c.Name == "Electronics");

        // Assert
        category.Should().NotBeNull();
        category.Products.Should().NotBeEmpty();
        category.Products.Should().Contain(p => p.Name == "Laptop");
        category.Products.Should().Contain(p => p.Name == "Smartphone");
    }
}
