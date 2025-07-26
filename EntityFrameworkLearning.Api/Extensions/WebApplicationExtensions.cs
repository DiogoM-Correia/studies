using EntityFrameworkLearning.Api.Data;
using Swashbuckle.AspNetCore.SwaggerUI;

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

        // Ensure database is created
        using (var scope = app.Services.CreateScope())
        {
            var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
            context.Database.EnsureCreated();
        }

        return app;
    }
} 