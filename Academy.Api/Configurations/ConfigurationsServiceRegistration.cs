using Academy.Infrastructure.SecretManager;
using Amazon;
using Amazon.Runtime;

namespace Academy.API.Configurations
{
    public static class ConfigurationsServiceRegistration
    {
        internal static WebApplicationBuilder AddConfigurations(this WebApplicationBuilder builder)
        {
            const string configurationsDirectory = "Configurations";
            var env = builder.Environment;

            //builder.Configuration.AddJsonFile($"{configurationsDirectory}/secretmanager.json", optional: false, reloadOnChange: true);

            //bool retrieveSecrets = builder.Configuration.GetValue<bool>("secretmanager:retrieveSecrets");

            //if (retrieveSecrets)
            //{
            //    builder.Configuration.AddSecretsManager(
            //     region: RegionEndpoint.APSouth1,
            //     configurator: options =>
            //     {
            //         options.SecretFilter = entry => entry.Name.StartsWith($"dev-rsa-public-key");
            //     });
            //}
            //else
            //{
            //    builder.Configuration.AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true, reloadOnChange: true);
            //}

            builder.Configuration
                    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                    .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true, reloadOnChange: true)
                    .AddJsonFile($"{configurationsDirectory}/auditing.json", optional: false, reloadOnChange: true)
                    .AddJsonFile($"{configurationsDirectory}/auditing.{env.EnvironmentName}.json", optional: true, reloadOnChange: true)
                    .AddJsonFile($"{configurationsDirectory}/logger.json", optional: false, reloadOnChange: true)
                    .AddJsonFile($"{configurationsDirectory}/logger.{env.EnvironmentName}.json", optional: true, reloadOnChange: true)
                    .AddJsonFile($"{configurationsDirectory}/hangfire.json", optional: false, reloadOnChange: true)
                    .AddJsonFile($"{configurationsDirectory}/hangfire.{env.EnvironmentName}.json", optional: true, reloadOnChange: true)
                    .AddJsonFile($"{configurationsDirectory}/cache.json", optional: false, reloadOnChange: true)
                    .AddJsonFile($"{configurationsDirectory}/cache.{env.EnvironmentName}.json", optional: true, reloadOnChange: true)
                    .AddJsonFile($"{configurationsDirectory}/cors.json", optional: false, reloadOnChange: true)
                    .AddJsonFile($"{configurationsDirectory}/cors.{env.EnvironmentName}.json", optional: true, reloadOnChange: true)
                    .AddJsonFile($"{configurationsDirectory}/database.json", optional: false, reloadOnChange: true)
                    .AddJsonFile($"{configurationsDirectory}/database.{env.EnvironmentName}.json", optional: true, reloadOnChange: true)
                    //.AddJsonFile($"{configurationsDirectory}/mail.json", optional: false, reloadOnChange: true)
                    //.AddJsonFile($"{configurationsDirectory}/mail.{env.EnvironmentName}.json", optional: true, reloadOnChange: true)
                    //.AddJsonFile($"{configurationsDirectory}/middleware.json", optional: false, reloadOnChange: true)
                    //.AddJsonFile($"{configurationsDirectory}/middleware.{env.EnvironmentName}.json", optional: true, reloadOnChange: true)
                    .AddJsonFile($"{configurationsDirectory}/security.json", optional: false, reloadOnChange: true)
                    .AddJsonFile($"{configurationsDirectory}/security.{env.EnvironmentName}.json", optional: true, reloadOnChange: true)
                    .AddJsonFile($"{configurationsDirectory}/openapi.json", optional: false, reloadOnChange: true)
                    .AddJsonFile($"{configurationsDirectory}/openapi.{env.EnvironmentName}.json", optional: true, reloadOnChange: true)
                    .AddJsonFile($"{configurationsDirectory}/signalr.json", optional: false, reloadOnChange: true)
                    .AddJsonFile($"{configurationsDirectory}/signalr.{env.EnvironmentName}.json", optional: true, reloadOnChange: true)
                    .AddJsonFile($"{configurationsDirectory}/securityheaders.json", optional: false, reloadOnChange: true)
                    .AddJsonFile($"{configurationsDirectory}/securityheaders.{env.EnvironmentName}.json", optional: true, reloadOnChange: true)
                    .AddJsonFile($"{configurationsDirectory}/localization.json", optional: false, reloadOnChange: true)
                    .AddJsonFile($"{configurationsDirectory}/localization.{env.EnvironmentName}.json", optional: true, reloadOnChange: true)
                    .AddJsonFile($"{configurationsDirectory}/fileprovider.json", optional: false, reloadOnChange: true)
                    .AddJsonFile($"{configurationsDirectory}/fileprovider.{env.EnvironmentName}.json", optional: true, reloadOnChange: true)
                    .AddJsonFile($"{configurationsDirectory}/mail.json", optional: false, reloadOnChange: true)
                    .AddJsonFile($"{configurationsDirectory}/mail.{env.EnvironmentName}.json", optional: true, reloadOnChange: true)
                    .AddJsonFile($"{configurationsDirectory}/tenant.json", optional: false, reloadOnChange: true)
                    .AddJsonFile($"{configurationsDirectory}/tenant.{env.EnvironmentName}.json", optional: true, reloadOnChange: true)
                    .AddEnvironmentVariables();
            return builder;
        }

        public static IConfigurationBuilder AddSecretsManager(this IConfigurationBuilder configurationBuilder,
            AWSCredentials? credentials = null,
            RegionEndpoint? region = null,
            Action<SecretsManagerConfigurationProviderOptions>? configurator = null)
        {
            var options = new SecretsManagerConfigurationProviderOptions();

            configurator?.Invoke(options);

            var source = new SecretsManagerConfigurationSource(credentials, options);

            if (region is not null)
            {
                source.Region = region;
            }

            configurationBuilder.Add(source);

            return configurationBuilder;
        }
    }
}
