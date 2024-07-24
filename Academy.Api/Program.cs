
using Academy.API.Configurations;
using Academy.Infrastructure;
using Academy.Infrastructure.Logging.Serilog;

var builder = WebApplication.CreateBuilder(args);
var app = builder
    .AddConfigurations()
    .RegisterSerilog()
    .ConfigureServices();

await app.Services.InitializeDatabasesAsync();

app.ConfigurePipeline();

app.Run();
