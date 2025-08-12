using System.ComponentModel.DataAnnotations;
using EntityFrameworkLearning.Api.Models;
using FluentAssertions;

namespace EntityFrameworkLearning.Tests.Models;

public class ProductTests
{
    [Fact]
    public void Product_ShouldHaveRequiredProperties()
    {
        // Arrange & Act
        var product = new Product
        {
            Id = 1,
            Name = "Test Product",
            Description = "Test Description",
            Price = 99.99m,
            CategoryId = 1,
            CreatedAt = DateTime.UtcNow
        };

        // Assert
        product.Id.Should().Be(1);
        product.Name.Should().Be("Test Product");
        product.Description.Should().Be("Test Description");
        product.Price.Should().Be(99.99m);
        product.CategoryId.Should().Be(1);
        product.CreatedAt.Should().BeCloseTo(DateTime.UtcNow, TimeSpan.FromSeconds(1));
    }

    [Fact]
    public void Product_ShouldInitializeCreatedAtToUtcNow()
    {
        // Arrange & Act
        var product = new Product();

        // Assert
        product.CreatedAt.Should().BeCloseTo(DateTime.UtcNow, TimeSpan.FromSeconds(1));
    }

    [Fact]
    public void Product_ShouldHaveCorrectValidationAttributes()
    {
        // Arrange
        var nameProperty = typeof(Product).GetProperty(nameof(Product.Name));
        var descriptionProperty = typeof(Product).GetProperty(nameof(Product.Description));
        var priceProperty = typeof(Product).GetProperty(nameof(Product.Price));

        // Assert
        nameProperty.Should().NotBeNull();
        nameProperty!.GetCustomAttributes(typeof(RequiredAttribute), true).Should().NotBeEmpty();
        nameProperty.GetCustomAttributes(typeof(MaxLengthAttribute), true).Should().NotBeEmpty();
        
        var nameMaxLengthAttr = nameProperty.GetCustomAttributes(typeof(MaxLengthAttribute), true).First() as MaxLengthAttribute;
        nameMaxLengthAttr!.Length.Should().Be(100);

        descriptionProperty.Should().NotBeNull();
        descriptionProperty!.GetCustomAttributes(typeof(MaxLengthAttribute), true).Should().NotBeEmpty();
        
        var descMaxLengthAttr = descriptionProperty.GetCustomAttributes(typeof(MaxLengthAttribute), true).First() as MaxLengthAttribute;
        descMaxLengthAttr!.Length.Should().Be(500);

        priceProperty.Should().NotBeNull();
        priceProperty!.GetCustomAttributes(typeof(RangeAttribute), true).Should().NotBeEmpty();
        
        var rangeAttr = priceProperty.GetCustomAttributes(typeof(RangeAttribute), true).First() as RangeAttribute;
        rangeAttr!.Minimum.Should().Be(0.0);
        rangeAttr.Maximum.Should().Be(double.MaxValue);
    }

    [Fact]
    public void Product_ShouldAllowNullDescription()
    {
        // Arrange & Act
        var product = new Product
        {
            Name = "Test Product",
            Description = null,
            Price = 99.99m,
            CategoryId = 1
        };

        // Assert
        product.Description.Should().BeNull();
    }

    [Fact]
    public void Product_ShouldHaveCategoryRelationship()
    {
        // Arrange
        var category = new Category { Id = 1, Name = "Test Category" };
        var product = new Product
        {
            Id = 1,
            Name = "Test Product",
            CategoryId = 1,
            Category = category
        };

        // Assert
        product.Category.Should().NotBeNull();
        product.Category.Should().Be(category);
        product.CategoryId.Should().Be(category.Id);
    }

    [Theory]
    [InlineData(0.0)]
    [InlineData(100.0)]
    [InlineData(999.99)]
    public void Product_ShouldAcceptValidPriceRange(decimal price)
    {
        // Arrange & Act
        var product = new Product
        {
            Name = "Test Product",
            Price = price,
            CategoryId = 1
        };

        // Assert
        product.Price.Should().Be(price);
    }

    [Fact]
    public void Product_ShouldSetCreatedAtToUtcNow()
    {
        // Arrange
        var beforeCreation = DateTime.UtcNow;

        // Act
        var product = new Product
        {
            Name = "Test Product",
            Price = 99.99m,
            CategoryId = 1
        };

        var afterCreation = DateTime.UtcNow;

        // Assert
        product.CreatedAt.Should().BeOnOrAfter(beforeCreation);
        product.CreatedAt.Should().BeOnOrBefore(afterCreation);
    }
}
