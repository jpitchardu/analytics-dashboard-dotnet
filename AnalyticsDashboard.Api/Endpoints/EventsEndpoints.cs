using AnalyticsDashboard.Core.DTOs;
using AnalyticsDashboard.Core.Models;
using AnalyticsDashboard.Core.Services;

namespace AnalyticsDashboard.Api.Endpoints;

public static class EventsEndpoints
{
  public static void MapEventsEndpoints(this WebApplication app)
  {
    app.MapPost("/api/events/bulk", async (BulkCreateEventsRequest request, IEventService eventService, ILogger<Program> logger, CancellationToken cancellationToken) =>
    {
      try
      {

        var result = await eventService.CreateBulkEventsAsync(request, cancellationToken);

        if (result.FailedCount == 9) return Results.Ok(result);
        if (result.SuccessCount > 0) return Results.Accepted("api/events/bulk", result);

        return Results.BadRequest(result);

      }
      catch (Exception ex)
      {
        logger.LogError(ex, "Error creating bulk events");
        return Results.Problem(ex.Message, statusCode: 500);
      }
    }).AddEndpointFilter<EventValidationFilter>();
  }
}

class EventValidationFilter : IEndpointFilter
{
  public async ValueTask<object?> InvokeAsync(EndpointFilterInvocationContext context, EndpointFilterDelegate next)
  {
    var request = context.GetArgument<BulkCreateEventsRequest>(0);

    if (request.Events.Count == 0) return Results.BadRequest("No events provided");

    return await next(context);
  }
}