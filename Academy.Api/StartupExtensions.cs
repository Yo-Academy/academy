using Academy.Application;
using Academy.Infrastructure;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace Academy.API
{
    public static class StartupExtensions
    {
        public static WebApplication ConfigureServices(
            this WebApplicationBuilder builder)
        {

            builder.Services.AddControllers().AddNewtonsoftJson(options =>
            {
                // Configure various settings here
                options.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver(); // Convert property names to camelCase
                options.SerializerSettings.NullValueHandling = NullValueHandling.Include; // Include null values during serialization
                options.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore; // Ignore circular references
                                                                                                 // Add any other configuration options you need
            });

            builder.Services
                .AddInfrastructureServices(builder.Configuration, builder.Environment)
                .AddApplicationServices();

            builder.Services.AddResponseCaching();

            return builder.Build();
        }

        public static WebApplication ConfigurePipeline(this WebApplication app)
        {
            if (app.Environment.IsDevelopment())
            {
            }
            app.UseInfrastructure(app.Configuration);

            app.UseHttpsRedirection();

            app.MapControllers();
            app.UseResponseCaching();
            app.MapEndpoints();

            return app;
        }
    }
}