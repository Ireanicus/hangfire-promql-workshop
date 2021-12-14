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

services.AddWebMetrics();

services.AddSingleton<RandomizerJob>();
services.AddSingleton<LogEverythingFilter>();

services
    .AddHangfireConsoleExtensions()
    .AddHangfire((provider, options) =>
    {
        options
            .UseFilter(provider.GetRequiredService<LogEverythingFilter>())
            .UsePostgreSqlStorage(connectionString)
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
    });

services.AddHostedService<HangfireReccuringJobHostedService>();

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

app.Run();