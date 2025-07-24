using FluentAssertions;
using AnalyticsDashboard.Core.Models;


namespace AnalyticsDashboard.Core.Tests.Models;

public class EventTests
{
  [Fact]
  public void Event_WhenCreated_ShouldHaveDefaultValues()
  {
    var eventObj = new Event();

    eventObj.Id.Should().NotBeEmpty();
    eventObj.Timestamp.Should().BeCloseTo(DateTime.UtcNow, TimeSpan.FromSeconds(1));
    eventObj.EventType.Should().NotBeNull();
    eventObj.UserId.Should().NotBeNull();
  }

  [Fact]
  public void Event_WhenCreatedWithParameter_ShouldSetPropertiesCorrectly()
  {

    var eventType = "user_click";
    var userId = "user_123";
    var timestamp = DateTime.UtcNow.AddMinutes(-5);
    var streamId = Guid.NewGuid();

    var eventObj = new Event
    {
      EventType = eventType,
      UserId = userId,
      Timestamp = timestamp,
      EventStreamId = streamId
    };

    eventObj.EventType.Should().Be(eventType);
    eventObj.UserId.Should().Be(userId);
    eventObj.Timestamp.Should().Be(timestamp);
    eventObj.EventStreamId.Should().Be(streamId);
  }

  [Theory]
  [InlineData("", "user123")]
  [InlineData("click", "")]
  [InlineData(null, "user123")]
  [InlineData("click", null)]
  public void Event_WhenCreatedWithInvalidData_ShouldThrowArgumentException(string eventType, string userId)
  {
    var streamId = Guid.NewGuid();

    var action = () => new Event(eventType, userId, DateTime.UtcNow, streamId);

    action.Should().Throw<ArgumentException>();
  }


}