using TravelConnect.Infrastructure;

var builder = WebApplication.CreateBuilder(args);

var services = builder.Services;
var configuration = builder.Configuration;

var label = Environment.GetEnvironmentVariable("APP_CONFIG_LABEL");

var appConfigEndpoint = Environment.GetEnvironmentVariable("APP_CONFIG_ENDPOINT");

configuration.AddAzureAppConfiguration(options =>
{
    options.Connect(appConfigEndpoint)
    .Select("*", label);
});

services.AddInfrastructure(configuration);

builder.WebHost.UseUrls("http://0.0.0.0:80");

var app = builder.Build();

app.UseInfrastructure();
app.Run();
