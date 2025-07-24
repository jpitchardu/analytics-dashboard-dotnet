namespace AnalyticsDashboard.Core.Models;

public class Event
{
  public Guid Id { get; set; } = Guid.NewGuid();
  public string EventType { get; set; } = string.Empty;
  public string UserId { get; set; } = string.Empty;
  public DateTime Timestamp { get; set; } = DateTime.UtcNow;
  public Dictionary<string, object>? Metadata { get; set; }
  public Guid EventStreamId { get; set; }
  public EventStream EventStream { get; set; } = null!;

  public Event(string eventType, string userId, DateTime timestamp, Guid eventStreamId, Dictionary<string, object>? metadata = null)
  {
    if (string.IsNullOrWhiteSpace(eventType))
    {
      throw new ArgumentException("Event type is required");
    }

    if (string.IsNullOrWhiteSpace(userId))
    {
      throw new ArgumentException("User ID is required");
    }

    if (eventStreamId == Guid.Empty)
    {
      throw new ArgumentException("Event stream ID is required");
    }

    EventType = eventType;
    UserId = userId;
    Timestamp = timestamp;
    EventStreamId = eventStreamId;
    Metadata = metadata;
  }

  public Event() { }
}