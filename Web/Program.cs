using Hangfire;
using Hangfire.PostgreSql;

var builder = WebApplication.CreateBuilder(args);
var connectionString = builder.Configuration.GetConnectionString("Hangfire");
var services = builder.Services;

services.AddWebMetrics();

services
    .AddHangfire(options =>
    {
        options.UsePostgreSqlStorage(connectionString);
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