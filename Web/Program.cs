var builder = WebApplication.CreateBuilder(args);
var services = builder.Services;

services.AddWebMetrics();

var app = builder.Build();

app.UseWebMetrics();

app.Run();
