using AnalyticsDashboard.Core.Models;
using AnalyticsDashboard.Core.Services;
using AnalyticsDashboard.Infrastructure.Data;

namespace AnalyticsDashboard.Infrastructure.Repositories;

public class EventRepository : IEventRepository
{
  private readonly AnalyticsDbContext _context;

  public EventRepository(AnalyticsDbContext context)
  {
    _context = context;
  }

  public async Task AddRangeAsync(IEnumerable<Event> events, CancellationToken cancellationToken = default)
  {
    await _context.Events.AddRangeAsync(events, cancellationToken);
    await _context.SaveChangesAsync(cancellationToken);
  }
}