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
app.MapGet("/error", () => { throw new Exception("error"); });

app.Run();