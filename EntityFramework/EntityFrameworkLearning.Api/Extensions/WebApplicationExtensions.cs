using EntityFrameworkLearning.Api.Data;
using Swashbuckle.AspNetCore.SwaggerUI;
using Microsoft.EntityFrameworkCore;

namespace EntityFrameworkLearning.Api.Extensions;

public static class WebApplicationExtensions
{
    public static WebApplication ConfigureApplication(this WebApplication app)
    {
        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Entity Framework Learning API v1");
                c.RoutePrefix = "swagger";
                c.DocumentTitle = "Entity Framework Learning API Documentation";
                c.DefaultModelsExpandDepth(2);
                c.DefaultModelRendering(ModelRendering.Example);
            });
        }

        app.UseHttpsRedirection();

        // Ensure database is created and migrated
        using (var scope = app.Services.CreateScope())
        {
            var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
            var logger = scope.ServiceProvider.GetRequiredService<ILogger<ApplicationDbContext>>();
            
            try
            {
                // Wait for database to be ready
                var retryCount = 0;
                var maxRetries = 10;
                
                while (retryCount < maxRetries)
                {
                    try
                    {
                        context.Database.EnsureCreated();
                        logger.LogInformation("Database created successfully");
                        break;
                    }
                    catch (Exception ex)
                    {
                        retryCount++;
                        logger.LogWarning("Database connection attempt {RetryCount} failed: {Message}", retryCount, ex.Message);
                        
                        if (retryCount >= maxRetries)
                        {
                            logger.LogError("Failed to connect to database after {MaxRetries} attempts", maxRetries);
                            throw;
                        }
                        
                        Thread.Sleep(2000); // Wait 2 seconds before retrying
                    }
                }
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "An error occurred while creating the database");
                throw;
            }
        }

        return app;
    }
} 