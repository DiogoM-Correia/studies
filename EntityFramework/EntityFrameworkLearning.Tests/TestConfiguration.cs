using Microsoft.EntityFrameworkCore;
using EntityFrameworkLearning.Api.Data;

namespace EntityFrameworkLearning.Tests;

/// <summary>
/// Common test configuration and utilities for Entity Framework tests
/// </summary>
public static class TestConfiguration
{
    /// <summary>
    /// Creates a new in-memory database context for testing
    /// </summary>
    /// <param name="databaseName">Optional database name, defaults to a new GUID</param>
    /// <returns>DbContextOptions for ApplicationDbContext</returns>
    public static DbContextOptions<ApplicationDbContext> CreateNewContextOptions(string? databaseName = null)
    {
        return new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase(databaseName ?? Guid.NewGuid().ToString())
            .Options;
    }

    /// <summary>
    /// Creates a new ApplicationDbContext with seed data for testing
    /// </summary>
    /// <param name="databaseName">Optional database name, defaults to a new GUID</param>
    /// <returns>ApplicationDbContext with seed data</returns>
    public static async Task<ApplicationDbContext> CreateContextWithSeedDataAsync(string? databaseName = null)
    {
        var options = CreateNewContextOptions(databaseName);
        var context = new ApplicationDbContext(options);
        await context.Database.EnsureCreatedAsync();
        return context;
    }

    /// <summary>
    /// Creates a new ApplicationDbContext without seed data for testing
    /// </summary>
    /// <param name="databaseName">Optional database name, defaults to a new GUID</param>
    /// <returns>ApplicationDbContext without seed data</returns>
    public static ApplicationDbContext CreateContextWithoutSeedData(string? databaseName = null)
    {
        var options = CreateNewContextOptions(databaseName);
        return new ApplicationDbContext(options);
    }
}
