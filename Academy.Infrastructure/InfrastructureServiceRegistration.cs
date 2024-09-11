using Academy.Application.Common.Storage;
using Academy.Infrastructure.Auth;
using Academy.Infrastructure.BackgroundJobs;
using Academy.Infrastructure.Caching;
using Academy.Infrastructure.Common;
using Academy.Infrastructure.Cors;
using Academy.Infrastructure.Localization;
using Academy.Infrastructure.Mailing;
using Academy.Infrastructure.Middleware;
using Academy.Infrastructure.Multitenancy;
using Academy.Infrastructure.Notifications;
using Academy.Infrastructure.OpenApi;
using Academy.Infrastructure.Persistence;
using Academy.Infrastructure.Persistence.Initialization;
using Academy.Infrastructure.SecurityHeaders;
using Academy.Infrastructure.Storage;
using Academy.Infrastructure.Storage.AWS;
using Academy.Infrastructure.Storage.AWS.Options;
using Academy.Infrastructure.Storage.Azure;
using Academy.Infrastructure.Storage.Azure.Options;
using Academy.Infrastructure.Storage.FileSystem;
using Academy.Infrastructure.Storage.FileSystem.Options;
using Academy.Infrastructure.Validations;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Routing;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace Academy.Infrastructure
{
    public static class InfrastructureServiceRegistration
    {
        public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration config, IWebHostEnvironment environment)
        {
            var applicationAssembly = typeof(Academy.Application.ApplicationServiceRegistration).GetTypeInfo().Assembly;

            services.AddMediatR(cfg =>
            {
                cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly());
            });

            return services
                .AddApiVersioning(config)
                .AddAuth(config)
                .AddBackgroundJobs(config)
                .AddCaching(config)
                .AddCorsPolicy(config)
                .AddBehaviours(applicationAssembly)
                 .AddHttpContextAccessor()
                .AddRequestLogging(config, environment)
                .AddLocalization(config)
                .AddMailService(config)
                .AddStorage(config)
                .AddMultitenancy()
                .AddNotifications(config)
                .AddOpenApiDocumentation(config)
                .AddPersistence()
                //.AddRequestTracingMiddleware(config, environment)
                .AddRouting(options => options.LowercaseUrls = true)
                .AddConfiguration(config)
                .AddIdentityEmailNullConfig(config)
                .AddServices();

        }

        private static IServiceCollection AddConfiguration(this IServiceCollection services, IConfiguration config)
        {
            return services.AddSingleton(config);
        }

        private static IServiceCollection AddIdentityEmailNullConfig(this IServiceCollection services, IConfiguration config)
        {
            return services.Configure<IdentityOptions>(options =>
            {
                options.User.RequireUniqueEmail = false; // Disable unique email requirement
            });
        }

        private static IServiceCollection AddApiVersioning(this IServiceCollection services, IConfiguration config)
        {
            var apiVersioningBuilder = services.AddApiVersioning(options =>
            {
                options.DefaultApiVersion = new ApiVersion(1, 0);
                options.AssumeDefaultVersionWhenUnspecified = true;
                options.ReportApiVersions = true;
            });

            var settings = config.GetSection(nameof(SwaggerSettings)).Get<SwaggerSettings>();
            if (settings != null && settings.Enable)
            {
                apiVersioningBuilder.AddApiExplorer(options =>
                {
                    options.GroupNameFormat = "'v'VVV";
                    options.SubstituteApiVersionInUrl = true;
                });
            }

            return services;
        }

        //private static IServiceCollection AddHealthCheck(this IServiceCollection services) =>
        //    services.AddHealthChecks().AddCheck<TenantHealthCheck>("Tenant").Services;

        public static async Task InitializeDatabasesAsync(this IServiceProvider services, CancellationToken cancellationToken = default)
        {
            // Create a new scope to retrieve scoped services
            using var scope = services.CreateScope();

            await scope.ServiceProvider.GetRequiredService<IDatabaseInitializer>()
                .InitializeDatabasesAsync(cancellationToken);
        }

        public static IApplicationBuilder UseInfrastructure(this IApplicationBuilder builder, IConfiguration config) =>
            builder
            .UseLocalization(config)
            .UseStaticFiles()
            .UseSecurityHeaders(config)
            .UseRequestLogging()
            .UseRouting()
            .UseCorsPolicy()
            .UseMultiTenancy()
            .UseAuthentication()
            .UseCurrentUser()
            .UseAuthorization()
            //.UseRequestTracingMiddleware()
            .UseHangfireDashboard(config)
            .UseOpenApiDocumentation(config);

        public static IEndpointRouteBuilder MapEndpoints(this IEndpointRouteBuilder builder)
        {
            //builder.MapControllers().RequireAuthorization();
            //builder.MapHealthCheck();
            builder.MapNotifications();
            return builder;
        }

        private static IEndpointConventionBuilder MapHealthCheck(this IEndpointRouteBuilder endpoints) =>
            endpoints.MapHealthChecks("/api/health");

        public static IServiceCollection AddAWSStorage(this IServiceCollection serviceCollection, AWSStorageOptions options)
        {
            CheckAWSConfiguration(options);
            serviceCollection.AddSingleton(options);
            serviceCollection.AddTransient<IAWSStorage, AWSStorage>();
            return serviceCollection.AddTransient<IStorage, AWSStorage>();
        }

        public static IServiceCollection AddAzureStorage(this IServiceCollection serviceCollection, IAzureStorageOptions options)
        {
            CheckAzureConfiguration(options);
            serviceCollection.AddSingleton(options);
            serviceCollection.AddTransient<IAzureStorage, AzureStorage>();
            return serviceCollection.AddTransient<IStorage, AzureStorage>();
        }

        public static IServiceCollection AddFileSystemStorage(this IServiceCollection serviceCollection, FileSystemStorageOptions options)
        {
            serviceCollection.AddSingleton(options);
            serviceCollection.AddTransient<IFileSystemStorage, FileSystemStorage>();
            return serviceCollection.AddTransient<IStorage, FileSystemStorage>();
        }


        private static void CheckAWSConfiguration(AWSStorageOptions options)
        {
            if (string.IsNullOrEmpty(options.PublicKey))
            {
                throw new Exception($"{nameof(options.PublicKey)} cannot be empty");
            }

            if (string.IsNullOrEmpty(options.SecretKey))
            {
                throw new Exception($"{nameof(options.SecretKey)} cannot be empty");
            }

            if (string.IsNullOrEmpty(options.Bucket))
            {
                throw new Exception($"{nameof(options.Bucket)} cannot be empty");
            } 
        }

        private static void CheckAzureConfiguration(IAzureStorageOptions options)
        {
            if (options is AzureStorageOptions azureStorageOptions)
            {
                if (string.IsNullOrEmpty(azureStorageOptions.ConnectionString))
                {
                    throw new Exception($"{nameof(azureStorageOptions.ConnectionString)} cannot be empty");
                }

                if (string.IsNullOrEmpty(azureStorageOptions.Container))
                {
                    throw new Exception($"{nameof(azureStorageOptions.Container)} cannot be empty");
                }
            }

            if (options is AzureStorageCredentialsOptions azureStorageCredentialsOptions)
            {
                if (string.IsNullOrEmpty(azureStorageCredentialsOptions.AccountName))
                {
                    throw new Exception(
                        $"{nameof(azureStorageCredentialsOptions.AccountName)} cannot be empty");
                }

                if (string.IsNullOrEmpty(azureStorageCredentialsOptions.ContainerName))
                {
                    throw new Exception(
                        $"{nameof(azureStorageCredentialsOptions.ContainerName)} cannot be empty");
                }

                if (azureStorageCredentialsOptions.Credentials is null)
                {
                    throw new Exception(
                        $"{nameof(azureStorageCredentialsOptions.Credentials)} cannot be null");
                }
            }
        }

    }
}
