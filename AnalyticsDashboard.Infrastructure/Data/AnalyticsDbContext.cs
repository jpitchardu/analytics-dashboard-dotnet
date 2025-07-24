using AnalyticsDashboard.Core.Models;
using Microsoft.EntityFrameworkCore;

namespace AnalyticsDashboard.Infrastructure.Data;

public class AnalyticsDbContext : DbContext
{
  public AnalyticsDbContext(DbContextOptions<AnalyticsDbContext> options) : base(options) { }

  public DbSet<Event> Events { get; set; }
  public DbSet<EventStream> EventStreams { get; set; }

  protected override void OnModelCreating(ModelBuilder modelBuilder)
  {
    modelBuilder.Entity<Event>(entity =>
    {
      entity.HasKey(e => e.Id);
      entity.Property(e => e.EventType).HasMaxLength(100).IsRequired();
      entity.Property(e => e.UserId).HasMaxLength(100).IsRequired();
      entity.HasIndex(e => e.Timestamp).HasDatabaseName("IX_Events_Timestamp");

      entity.Ignore(e => e.Metadata);

      entity.HasOne(e => e.EventStream)
            .WithMany()
            .HasForeignKey(e => e.EventStreamId)
            .OnDelete(DeleteBehavior.Restrict);
    });

    modelBuilder.Entity<EventStream>(entity =>
    {
      entity.HasKey(e => e.Id);
      entity.Property(e => e.Name).HasMaxLength(200).IsRequired();
      entity.HasIndex(e => e.Name).IsUnique();
    });
  }
}