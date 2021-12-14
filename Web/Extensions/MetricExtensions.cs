
using System.Reflection;
using App.Metrics;
using App.Metrics.Counter;

public static class MetricExtensions
{
    public static IServiceCollection AddWebMetrics(this IServiceCollection services)
    {

        services.AddHostedService<RamMetricHostedService>();
        services.AddHostedService<LabelMetricHostedService>();

        return services
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
    }

    public static WebApplication UseWebMetrics(this WebApplication app)
    {
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

        return app;
    }
}
