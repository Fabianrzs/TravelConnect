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

var app = builder.Build();

app.UseInfrastructure(app.Environment);
app.Run();
