using System.Runtime.InteropServices;
using System.Text;
using System.Text.Json;
using AnalyticsDashboard.Core.DTOs;
using AnalyticsDashboard.Infrastructure.Data;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace AnalyticsDashboard.Api.Tests.Endpoints;

public class EventEndpointsTests : IClassFixture<WebApplicationFactory<Program>>, IDisposable
{
  private readonly WebApplicationFactory<Program> _factory;
  private readonly HttpClient _client;

  public EventEndpointsTests(WebApplicationFactory<Program> factory)
  {
    _factory = factory.WithWebHostBuilder(builder =>
    {
      builder.ConfigureServices(services =>
      {
        var descriptor = services.SingleOrDefault(d => d.ServiceType == typeof(DbContextOptions<AnalyticsDbContext>));
        if (descriptor != null) services.Remove(descriptor);

        services.AddDbContext<AnalyticsDbContext>(options =>
        {
          options.UseInMemoryDatabase(Guid.NewGuid().ToString());
        });
      });
    });

    _client = _factory.CreateClient();
  }

  [Fact]
  public async Task CreateBulkEvents_WithValidData_ShouldReturn200AndProcessEvents()
  {
    // Arrange
    var streamId = Guid.NewGuid();
    var request = new BulkCreateEventsRequest
    {
      Events =
            [
                new("click", DateTime.UtcNow,"user1", streamId),
                new("view", DateTime.UtcNow ,"user2", streamId)
            ]
    };

    var json = JsonSerializer.Serialize(request);
    var content = new StringContent(json, Encoding.UTF8, "application/json");

    // Act
    var response = await _client.PostAsync("/api/events/bulk", content);

    // Assert
    response.IsSuccessStatusCode.Should().BeTrue();

    var responseContent = await response.Content.ReadAsStringAsync();
    var result = JsonSerializer.Deserialize<BulkCreateEventsResponse>(responseContent, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

    result.Should().NotBeNull();
    result.SuccessCount.Should().Be(2);
    result.FailedCount.Should().Be(0);
  }


  public void Dispose()
  {
    _factory.Dispose();
  }
}