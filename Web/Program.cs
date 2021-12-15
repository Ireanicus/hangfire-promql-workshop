using Hangfire;
using Hangfire.Console;
using Hangfire.PostgreSql;
using Hangfire.Console.Extensions;
using Hangfire.MissionControl;
using System.Reflection;
using Hangfire.RecurringJobAdmin;

var builder = WebApplication.CreateBuilder(args);
var connectionString = builder.Configuration.GetConnectionString("Hangfire");
var services = builder.Services;

services.AddAuthorization(options =>
{
    options.AddPolicy("Anonymous", policy => policy.RequireAssertion(context => true));
});

services.AddAuthentication();

services.AddWebMetrics();

services.AddSingleton<RandomizerJob>();
services.AddSingleton<LogEverythingFilter>();
services.AddSingleton<QueueService>();
services.AddSingleton<CustomQueueFilter>();
services.AddSingleton<MultiQueueExample>();
services.AddSingleton<StateMetricFilter>();
services.AddSingleton<DurationHistogramFilter>();

services
    .AddHangfireConsoleExtensions()
    .AddHangfire((provider, options) =>
    {
        options
            .UseFilter(provider.GetRequiredService<LogEverythingFilter>())
            .UseFilter(provider.GetRequiredService<CustomQueueFilter>())
            .UseFilter(provider.GetRequiredService<StateMetricFilter>())
            .UseFilter(provider.GetRequiredService<DurationHistogramFilter>())
            .UsePostgreSqlStorage(connectionString, new PostgreSqlStorageOptions
            {
            })
            .UseConsole(new ConsoleOptions
            {
                FollowJobRetentionPolicy = true
            })
            .UseMissionControl(new MissionControlOptions
            {

            }, Assembly.GetEntryAssembly())
            .UseRecurringJobAdmin(Assembly.GetEntryAssembly());
    })
    .AddHangfireServer(options =>
    {
        options.Queues = new[] { "default" };
    });

services.AddHostedService<HangfireReccuringJobHostedService>();
services.AddHostedService<HangfireDashboardMetricsHostedService>();

var app = builder.Build();


app.UseWebMetrics();

app.UseHangfireDashboard("/hangfire", new DashboardOptions
{

});
app.UseHangfireDashboard("/hangfire-read", new DashboardOptions
{
    IsReadOnlyFunc = context => true,
    DisplayStorageConnectionString = false
});

app.UseAuthorization();
app.UseAuthentication();


app.MapHangfireDashboardWithAuthorizationPolicy("Anonymous", "/hangfire-auth", new DashboardOptions
{

});

app.Run();