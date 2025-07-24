namespace AnalyticsDashboard.Core.DTOs;

public record CreateEventDto(
  string EventType,
  DateTime Timestamp,
  string UserId,
  Guid EventStreamId
);