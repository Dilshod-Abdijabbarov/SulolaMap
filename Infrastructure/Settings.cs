
using Infrastructure.db;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure
{
    public static class Settings
    {
        public static IServiceCollection InfrastructureSetting(this IServiceCollection services,IConfiguration configuration)
        {
            services.AddDbContext<SulolaDbContext>(options =>
                options.UseNpgsql(configuration.GetConnectionString("SulolaConnectionString"),
                builder => { builder.EnableRetryOnFailure(5, TimeSpan.FromSeconds(10), null); }),
                ServiceLifetime.Transient);

            return services;
        }

        public static WebApplication MainConfigureMigration(this WebApplication app)
        {
            using (var scope = app.Services.CreateScope())
            {
                var dbContext = scope.ServiceProvider
                    .GetRequiredService<SulolaDbContext>();
                dbContext.Database.Migrate();
            }
            return app;
        }
    }
}
