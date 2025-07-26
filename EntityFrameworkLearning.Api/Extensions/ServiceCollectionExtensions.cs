using Microsoft.EntityFrameworkCore;
using EntityFrameworkLearning.Api.Data;
using Microsoft.OpenApi.Models;
using System.Text.Json.Serialization;

namespace EntityFrameworkLearning.Api.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services, IConfiguration configuration)
    {
        // Add OpenAPI
        services.AddOpenApi();

        // Configure Swagger/OpenAPI
        services.AddSwaggerGen(c =>
        {
            c.SwaggerDoc("v1", new OpenApiInfo
            {
                Title = "Entity Framework Learning API",
                Version = "v1",
                Description = "A comprehensive API demonstrating Entity Framework Core concepts including CRUD operations, relationships, and DTOs.",
                Contact = new OpenApiContact
                {
                    Name = "Entity Framework Learning",
                    Email = "learning@example.com"
                }
            });
            
            // Add XML comments if available
            var xmlFile = $"{System.Reflection.Assembly.GetExecutingAssembly().GetName().Name}.xml";
            var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
            if (File.Exists(xmlPath))
            {
                c.IncludeXmlComments(xmlPath);
            }
        });

        // Configure JSON serialization to handle circular references
        services.ConfigureHttpJsonOptions(options =>
        {
            options.SerializerOptions.ReferenceHandler = ReferenceHandler.Preserve;
            options.SerializerOptions.WriteIndented = true;
        });

        // Add Entity Framework
        services.AddDbContext<ApplicationDbContext>(options =>
            options.UseSqlite(configuration.GetConnectionString("DefaultConnection")));

        return services;
    }
} 