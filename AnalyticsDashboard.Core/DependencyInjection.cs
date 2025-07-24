using AnalyticsDashboard.Core.Services;
using Microsoft.Extensions.DependencyInjection;

namespace AnalyticsDashboard.Core;

public static class DependencyInjection
{
  public static IServiceCollection AddCore(this IServiceCollection services)
  {
    // Business Services
    services.AddScoped<IEventService, EventService>();

    // Add other business services here as they're created
    // services.AddScoped<IAnalyticsService, AnalyticsService>();
    // services.AddScoped<IReportingService, ReportingService>();

    return services;
  }
}