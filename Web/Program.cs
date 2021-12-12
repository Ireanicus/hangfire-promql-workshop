using System.Reflection;
using App.Metrics;
using App.Metrics.Counter;
using Hangfire;
using Hangfire.Console;
using Hangfire.Console.Extensions;
using Hangfire.MissionControl;
using Hangfire.PostgreSql;
using Hangfire.RecurringJobAdmin;

var builder = WebApplication
    .CreateBuilder(args);

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
services.AddHostedService<RecurringJobsHostedService>();

services.AddSingleton<SampleJobs>();

services
    .AddSingleton<LogEverythingFilter>()
    .AddHangfireConsoleExtensions()
    .AddHangfire((provider, options) =>
    {
        options
            .UsePostgreSqlStorage(builder.Configuration.GetConnectionString("Hangfire"),
                new PostgreSqlStorageOptions
                {
                })
            .UseFilter(provider.GetRequiredService<LogEverythingFilter>())
            .UseRecurringJobAdmin(Assembly.GetEntryAssembly())
            .UseMissionControl(
                new MissionControlOptions
                {
                },
                Assembly.GetEntryAssembly())
            .UseConsole(new ConsoleOptions
            {
            });
    })
    .AddHangfireServer(options =>
    {
    });

var app = builder.Build();

app.UseMetricsAllMiddleware();
app.UseMetricsAllEndpoints();
app.UseHangfireDashboard();
app.UseHangfireDashboard("/readonly-hangfire", new DashboardOptions
{
    IsReadOnlyFunc = context => true
});

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