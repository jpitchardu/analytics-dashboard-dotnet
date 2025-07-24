using System.Diagnostics;
using AnalyticsDashboard.Core.DTOs;
using AnalyticsDashboard.Core.Models;
using Microsoft.Extensions.Logging;

namespace AnalyticsDashboard.Core.Services;

public class EventService : IEventService
{
  private readonly IEventRepository _eventRepository;
  private readonly ILogger<EventService> _logger;

  public EventService(IEventRepository eventRepository, ILogger<EventService> logger)
  {
    _eventRepository = eventRepository;
    _logger = logger;
  }



  public async Task<BulkCreateEventsResponse> CreateBulkEventsAsync(
    BulkCreateEventsRequest request,
    CancellationToken cancellationToken = default
    )
  {
    var stopwatch = Stopwatch.StartNew();
    var response = new BulkCreateEventsResponse();

    try
    {
      var events = new HashSet<Event>();
      var failedEventCount = 0;

      request.Events.ForEach(e =>
      {
        try
        {
          events.Add(new Event(e.EventType, e.UserId, e.Timestamp, e.EventStreamId));
        }
        catch (Exception ex)
        {
          failedEventCount++;
          response.Errors.Add($"Event {e.EventType} failed to create: {ex.Message}");
        }
      });

      if (events.Count > 0)
      {
        await _eventRepository.AddRangeAsync(events, cancellationToken);
        response.SuccessCount = events.Count;
      }

      response.FailedCount = failedEventCount;

      _logger.LogInformation(
        "Successfully created {Count} events in {Time}ms",
        events.Count,
        stopwatch.ElapsedMilliseconds
      );

      _logger.LogInformation(
        "{Count} events failed to create: {Errors}",
        failedEventCount,
        string.Join(", ", response.Errors)
      );
    }
    catch (Exception ex)
    {
      _logger.LogError(ex, "Error creating bulk events");
      response.FailedCount = request.Events.Count;
      response.Errors.Add(ex.Message);
    }
    finally
    {
      stopwatch.Stop();
      response.ProcessingTime = stopwatch.Elapsed;
    }
    return response;
  }
}