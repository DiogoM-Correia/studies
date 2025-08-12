using System.ComponentModel.DataAnnotations;
using EntityFrameworkLearning.Api.Models;
using FluentAssertions;

namespace EntityFrameworkLearning.Tests.Models;

public class CategoryTests
{
    [Fact]
    public void Category_ShouldHaveRequiredProperties()
    {
        // Arrange & Act
        var category = new Category
        {
            Id = 1,
            Name = "Test Category",
            Description = "Test Description"
        };

        // Assert
        category.Id.Should().Be(1);
        category.Name.Should().Be("Test Category");
        category.Description.Should().Be("Test Description");
        category.Products.Should().NotBeNull();
        category.Products.Should().BeEmpty();
    }

    [Fact]
    public void Category_ShouldInitializeProductsAsEmptyList()
    {
        // Arrange & Act
        var category = new Category();

        // Assert
        category.Products.Should().NotBeNull();
        category.Products.Should().BeEmpty();
    }

    [Fact]
    public void Category_ShouldHaveCorrectValidationAttributes()
    {
        // Arrange
        var nameProperty = typeof(Category).GetProperty(nameof(Category.Name));
        var descriptionProperty = typeof(Category).GetProperty(nameof(Category.Description));

        // Assert
        nameProperty.Should().NotBeNull();
        nameProperty!.GetCustomAttributes(typeof(RequiredAttribute), true).Should().NotBeEmpty();
        nameProperty.GetCustomAttributes(typeof(MaxLengthAttribute), true).Should().NotBeEmpty();
        
        var maxLengthAttr = nameProperty.GetCustomAttributes(typeof(MaxLengthAttribute), true).First() as MaxLengthAttribute;
        maxLengthAttr!.Length.Should().Be(50);
    }

    [Fact]
    public void Category_ShouldAllowNullDescription()
    {
        // Arrange & Act
        var category = new Category
        {
            Name = "Test Category",
            Description = null
        };

        // Assert
        category.Description.Should().BeNull();
    }

    [Fact]
    public void Category_ShouldHaveProductsRelationship()
    {
        // Arrange
        var category = new Category { Id = 1, Name = "Test Category" };
        var product = new Product { Id = 1, Name = "Test Product", CategoryId = 1 };

        // Act
        category.Products.Add(product);

        // Assert
        category.Products.Should().HaveCount(1);
        category.Products.First().Should().Be(product);
    }
}
