using Microsoft.EntityFrameworkCore;
using EntityFrameworkLearning.Api.Models;

namespace EntityFrameworkLearning.Api.Data;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }
    
    // DbSet properties represent database tables
    public DbSet<Product> Products { get; set; }
    public DbSet<Category> Categories { get; set; }
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // Configure the relationship between Product and Category
        modelBuilder.Entity<Product>()
            .HasOne(p => p.Category)        // Product has one Category
            .WithMany(c => c.Products)      // Category has many Products
            .HasForeignKey(p => p.CategoryId); // Foreign key relationship
        
        // Seed data for Categories
        modelBuilder.Entity<Category>().HasData(
            new Category { Id = 1, Name = "Electronics", Description = "Electronic devices and gadgets" },
            new Category { Id = 2, Name = "Books", Description = "Books and publications" },
            new Category { Id = 3, Name = "Clothing", Description = "Apparel and accessories" },
            new Category { Id = 4, Name = "Home & Garden", Description = "Home improvement and garden items" }
        );
        
        // Seed data for Products
        modelBuilder.Entity<Product>().HasData(
            new Product { 
                Id = 1, 
                Name = "Laptop", 
                Description = "High-performance laptop with latest specs", 
                Price = 999.99m, 
                CategoryId = 1,
                CreatedAt = DateTime.UtcNow
            },
            new Product { 
                Id = 2, 
                Name = "Smartphone", 
                Description = "Latest smartphone model with advanced features", 
                Price = 699.99m, 
                CategoryId = 1,
                CreatedAt = DateTime.UtcNow
            },
            new Product { 
                Id = 3, 
                Name = "Programming Book", 
                Description = "Learn C# programming from scratch", 
                Price = 49.99m, 
                CategoryId = 2,
                CreatedAt = DateTime.UtcNow
            },
            new Product { 
                Id = 4, 
                Name = "T-Shirt", 
                Description = "Comfortable cotton t-shirt", 
                Price = 19.99m, 
                CategoryId = 3,
                CreatedAt = DateTime.UtcNow
            },
            new Product { 
                Id = 5, 
                Name = "Garden Tool Set", 
                Description = "Complete set of essential garden tools", 
                Price = 89.99m, 
                CategoryId = 4,
                CreatedAt = DateTime.UtcNow
            }
        );
    }
} 