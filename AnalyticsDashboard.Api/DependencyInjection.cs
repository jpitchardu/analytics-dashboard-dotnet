using Microsoft.Extensions.DependencyInjection;

namespace AnalyticsDashboard.Api;

public static class DependencyInjection
{
  public static IServiceCollection AddApi(this IServiceCollection services)
  {
    // MVC Controllers
    services.AddControllers();

    // API Documentation
    services.AddEndpointsApiExplorer();
    services.AddSwaggerGen();

    // API-specific services can be added here
    // services.AddScoped<IAuthenticationService, AuthenticationService>();
    // services.AddScoped<IValidationService, ValidationService>();
    // services.AddCors(...);

    return services;
  }
}