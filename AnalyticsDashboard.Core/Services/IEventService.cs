using AnalyticsDashboard.Core.DTOs;
using AnalyticsDashboard.Core.Models;

namespace AnalyticsDashboard.Core.Services;

public interface IEventService
{
  Task<BulkCreateEventsResponse> CreateBulkEventsAsync(
    BulkCreateEventsRequest request,
    CancellationToken cancellationToken = default
  );



}