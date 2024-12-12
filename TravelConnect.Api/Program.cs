using TravelConnect.Infrastructure;

var builder = WebApplication.CreateBuilder(args);

var services = builder.Services;
var config = builder.Configuration;

services.AddInfrastructure(config);

var app = builder.Build();

app.UseInfrastructure(app.Environment);
app.Run();
