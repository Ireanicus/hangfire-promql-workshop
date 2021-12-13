using App.Metrics;
using App.Metrics.Counter;

var builder = WebApplication.CreateBuilder(args);
var services = builder.Services;

services.AddHostedService<RamMetricHostedService>();

services
    .AddMetrics(options =>
    {
        options.OutputMetrics.AsPrometheusPlainText();
    })
    .AddMetricsEndpoints()
    .AddMetricsReportingHostedService()
    .AddMetricsTrackingMiddleware();

var app = builder.Build();



app.UseMetricsAllMiddleware();
app.UseMetricsAllEndpoints();

app.MapGet("/", () => "Hello World!");
app.MapGet("/error", context => throw new Exception());
app.MapGet("/simple", context =>
{
    var metrics = context.RequestServices.GetRequiredService<IMetrics>();
    metrics.Measure.Counter.Increment(new CounterOptions
    {
        Name = "simple_metric",
        MeasurementUnit = Unit.None,
        //Tags = new MetricTags(new[] { "key1", "key2" }, new[] { "value1", "value2" })
        Tags = MetricTags.Concat(new MetricTags("key1", "value1"),
         new MetricTags("key2", Random.Shared.NextInt64().ToString()))
    });

    context.Response.StatusCode = 200;
    return Task.CompletedTask;
});

app.Run();
