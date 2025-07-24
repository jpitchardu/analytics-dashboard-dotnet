namespace AnalyticsDashboard.Core.DTOs;

public class BulkCreateEventsResponse
{
  public int SuccessCount { get; set; }
  public int FailedCount { get; set; }
  public List<string> Errors { get; set; } = new();
  public TimeSpan ProcessingTime { get; set; }
}