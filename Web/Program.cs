using App.Metrics;

var builder = WebApplication.CreateBuilder(args);
var services = builder.Services;

services.AddMetrics()
    .AddMetricsEndpoints()
    .AddMetricsReportingHostedService()
    .AddMetricsTrackingMiddleware();

var app = builder.Build();



app.UseMetricsAllMiddleware();
app.UseMetricsAllEndpoints();

app.MapGet("/", () => "Hello World!");

app.Run();
