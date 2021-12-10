using App.Metrics;
using App.Metrics.Counter;

var builder = WebApplication.CreateBuilder(args);

var services = builder.Services;
services
    .AddMetrics(options =>
    {
        options.OutputMetrics.AsPrometheusPlainText();
    })
    .AddMetricsEndpoints()
    .AddMetricsReportingHostedService()
    .AddMetricsTrackingMiddleware();

services.AddHostedService<LabelMetricHostedService>();

var app = builder.Build();

app.UseMetricsAllMiddleware();

app.UseMetricsAllEndpoints();
app.MapGet("/", () => "Hello World!");
app.MapGet("/error", () => { throw new Exception("error"); });
app.MapPost("/error", () => { throw new InvalidOperationException("error"); });
app.MapGet("/notFound", context =>
{
    context.Response.StatusCode = 404;
    return Task.CompletedTask;
});
app.MapGet("/unauthorized", context =>
{
    context.Response.StatusCode = 401;
    return Task.CompletedTask;
});
app.MapGet("/customMetric", context =>
{
    var metrics = context.RequestServices.GetRequiredService<IMetrics>();

    metrics.Measure.Counter.Increment(new CounterOptions
    {
        Name = "counter_example",
        Tags = MetricTags.Concat(
            new MetricTags("key", "value"),
            new MetricTags("path", "customMetric"))
    });

    context.Response.StatusCode = 200;
    return Task.CompletedTask;
});

app.MapGet("/long-running", async context =>
{
    await Task.Delay(10000);
    context.Response.StatusCode = 200;
});

app.Run();