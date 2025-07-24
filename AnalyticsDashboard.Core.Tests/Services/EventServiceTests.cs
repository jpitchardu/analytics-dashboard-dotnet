using AnalyticsDashboard.Core.DTOs;
using AnalyticsDashboard.Core.Models;
using AnalyticsDashboard.Core.Services;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using NSubstitute;
using NSubstitute.ExceptionExtensions;

namespace AnalyticsDashboard.Core.Tests.Services;

public class EventServiceTests
{
  private readonly IEventRepository _eventRepository;
  private readonly ILogger<EventService> _logger;
  private readonly EventService _eventService;

  public EventServiceTests()
  {
    _eventRepository = Substitute.For<IEventRepository>();
    _logger = Substitute.For<ILogger<EventService>>();
    _eventService = new EventService(_eventRepository, _logger);
  }

  [Fact]
  public async Task CreateBulkEventsAsync_WithValidEvents_ShouldReturnSuccessResponse()
  {
    // Arrange
    var streamId = Guid.NewGuid();
    var request = new BulkCreateEventsRequest
    {
      Events =
            [
                new( "click", DateTime.UtcNow, "user1", streamId ),
                new( "view", DateTime.UtcNow, "user2", streamId )
            ]
    };

    _eventRepository.AddRangeAsync(Arg.Any<IEnumerable<Event>>(), Arg.Any<CancellationToken>())
                   .Returns(Task.CompletedTask);

    // Act
    var result = await _eventService.CreateBulkEventsAsync(request);

    // Assert
    result.Should().NotBeNull();
    result.SuccessCount.Should().Be(2);
    result.FailedCount.Should().Be(0);
    result.Errors.Should().BeEmpty();

    await _eventRepository.Received(1).AddRangeAsync(
        Arg.Is<IEnumerable<Event>>(events => events.Count() == 2),
        Arg.Any<CancellationToken>());
  }

  [Fact]
  public async Task CreateBulkEventsAsync_WhenRepositoryThrows_ShouldReturnErrorResponse()
  {
    // Arrange
    var request = new BulkCreateEventsRequest
    {
      Events =
            [
                new( "click", DateTime.UtcNow, "user1", Guid.NewGuid() )
            ]
    };

    _eventRepository.AddRangeAsync(Arg.Any<IEnumerable<Event>>(), Arg.Any<CancellationToken>())
                   .ThrowsAsync(new InvalidOperationException("Database error"));

    // Act
    var result = await _eventService.CreateBulkEventsAsync(request);

    // Assert
    result.SuccessCount.Should().Be(0);
    result.FailedCount.Should().Be(1);
    result.Errors.Should().Contain("Database error");
  }

  [Fact]
  public async Task CreateBulkEventsAsync_WithMixedValidInvalidEvents_ShouldProcessValidOnesAndReportErrors()
  {
    // Arrange
    var streamId = Guid.NewGuid();
    var request = new BulkCreateEventsRequest
    {
      Events =
        [
            new( "click", DateTime.UtcNow, "user1", streamId ),
            new( "", DateTime.UtcNow, "user2", streamId ),
            new( "view", DateTime.UtcNow, "user3", streamId )
        ]
    };

    // Act
    var result = await _eventService.CreateBulkEventsAsync(request);

    // Assert
    result.SuccessCount.Should().Be(2);
    result.FailedCount.Should().Be(1);
    result.Errors.Should().HaveCount(1);
  }
}