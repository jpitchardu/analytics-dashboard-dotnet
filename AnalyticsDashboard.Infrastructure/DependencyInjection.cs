using AnalyticsDashboard.Core.Services;
using AnalyticsDashboard.Infrastructure.Data;
using AnalyticsDashboard.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace AnalyticsDashboard.Infrastructure;

public static class DependencyInjection
{
  public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
  {
    // Database Context
    services.AddDbContext<AnalyticsDbContext>(options =>
    {
      var environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "Development";

      if (environment == "Development" || environment == "Testing")
      {
        // Use InMemory database for development and testing
        options.UseInMemoryDatabase("AnalyticsDashboard");
      }
      else
      {
        // Use PostgreSQL for production
        var connectionString = configuration.GetConnectionString("DefaultConnection")
                ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
        options.UseNpgsql(connectionString);
      }
    });

    // Repositories
    services.AddScoped<IEventRepository, EventRepository>();

    // Add other repositories here as they're created
    // services.AddScoped<IEventStreamRepository, EventStreamRepository>();
    // services.AddScoped<IUserRepository, UserRepository>();

    return services;
  }
}