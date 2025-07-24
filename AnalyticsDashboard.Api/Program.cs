using AnalyticsDashboard.Api;
using AnalyticsDashboard.Api.Endpoints;
using AnalyticsDashboard.Core;
using AnalyticsDashboard.Infrastructure;

var builder = WebApplication.CreateBuilder(args);

// Layer-based service registration (clean architecture)
builder.Services
    .AddApi()                                    // Controllers, Swagger, API-specific services
    .AddCore()                                   // Business services (EventService, etc.)
    .AddInfrastructure(builder.Configuration);   // Database, repositories, data access

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseRouting();

app.MapEventsEndpoints();

app.Run();

// Expose Program class for testing
public partial class Program { }
