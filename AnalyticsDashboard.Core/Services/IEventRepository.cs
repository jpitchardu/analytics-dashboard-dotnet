using AnalyticsDashboard.Core.Models;

namespace AnalyticsDashboard.Core.Services;

public interface IEventRepository
{
  Task AddRangeAsync(IEnumerable<Event> events, CancellationToken cancellationToken = default);
}
