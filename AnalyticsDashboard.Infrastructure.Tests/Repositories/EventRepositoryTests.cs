using AnalyticsDashboard.Core.Models;
using AnalyticsDashboard.Infrastructure.Data;
using AnalyticsDashboard.Infrastructure.Repositories;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;

namespace AnalyticsDashboard.Infrastructure.Tests.Repositories;

public class EventRepositoryTests : IDisposable
{
  private readonly AnalyticsDbContext _context;
  private readonly EventRepository _repository;

  public EventRepositoryTests()
  {
    var options = new DbContextOptionsBuilder<AnalyticsDbContext>()
      .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
      .Options;

    _context = new AnalyticsDbContext(options);
    _repository = new EventRepository(_context);
  }

  [Fact]
  public async Task AddRangeAsync_WithValidEvents_ShouldPersistToDatabase()
  {

    await _repository.AddRangeAsync(new List<Event>
    {
      new( "click", "user1", DateTime.UtcNow, Guid.NewGuid() ),
      new( "view", "user2", DateTime.UtcNow, Guid.NewGuid() ),
    });

    var events = await _context.Events.ToListAsync();

    events.Count.Should().Be(2);
    events.Should().Contain(e => e.EventType == "click" && e.UserId == "user1");
    events.Should().Contain(e => e.EventType == "view" && e.UserId == "user2");
  }

  [Fact]
  public async Task AddRangeAsync_WithDuplicateEvents_ShouldHandleGracefully()
  {

    var eventId = Guid.NewGuid();

    var events = new List<Event>
    {
      new( "click", "user1", DateTime.UtcNow, Guid.NewGuid() ) { Id = eventId },
      new( "click", "user1", DateTime.UtcNow, Guid.NewGuid() ) { Id = eventId },
    };

    var action = () => _repository.AddRangeAsync(events);

    await action.Should().ThrowAsync<InvalidOperationException>();
  }

  public void Dispose()
  {
    _context.Database.EnsureDeleted();
    _context.Dispose();
  }
}