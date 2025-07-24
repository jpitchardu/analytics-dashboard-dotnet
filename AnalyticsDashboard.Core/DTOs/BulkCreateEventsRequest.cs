namespace AnalyticsDashboard.Core.DTOs;

public class BulkCreateEventsRequest
{
  public List<CreateEventDto> Events { get; set; } = new();
}