namespace AnalyticsDashboard.Core.Models;

public class EventStream
{
  public Guid Id { get; set; } = Guid.NewGuid();
  public string Name { get; set; } = string.Empty;

  public EventStream(string name)
  {
    Name = name;
  }

  public EventStream() { }
}