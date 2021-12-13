using System.Reflection;
using App.Metrics;
using App.Metrics.Counter;

var builder = WebApplication.CreateBuilder(args);
var services = builder.Services;

services.AddHostedService<RamMetricHostedService>();
services.AddHostedService<LabelMetricHostedService>();

services
    .AddMetrics(options =>
    {
        options.OutputMetrics.AsPrometheusPlainText();
        options.Configuration.Configure(config =>
        {
            config.GlobalTags.Add("version2",
                Assembly.GetEntryAssembly()?.GetName()?.Version?.ToString());
        });
    })
    .AddMetricsEndpoints()
    .AddMetricsReportingHostedService()
    .AddMetricsTrackingMiddleware();

var app = builder.Build();



app.UseMetricsAllMiddleware();
app.UseMetricsAllEndpoints();

app.MapGet("/", () => "Hello World!");

app.MapGet("/error", () => { throw new Exception("error"); });
app.MapPost("/error", () => { throw new InvalidOperationException("error"); });
app.MapPut("/error", () => { throw new InvalidOperationException("error"); });
app.MapDelete("/error", () => { throw new IndexOutOfRangeException("error"); });

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
