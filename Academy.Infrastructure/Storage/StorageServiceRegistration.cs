using Academy.Application.Common.Storage;
using Academy.Infrastructure.Storage.AWS.Options;
using Academy.Infrastructure.Storage.Azure.Options;
using Academy.Infrastructure.Storage.FileSystem.Options;
using Amazon;
using Amazon.S3;
using Microsoft.Extensions.Configuration;

namespace Academy.Infrastructure.Storage
{
    internal static class StorageServiceRegistration
    {
        internal static IServiceCollection AddStorage(this IServiceCollection services, IConfiguration config)
        {
            StorageConfig storageConfig = config.GetSection(nameof(StorageConfig)).Get<StorageConfig>();
            var fileProvider = storageConfig.FileProvider;

            if (!string.IsNullOrEmpty(fileProvider))
            {
                if (fileProvider == "FileStorage")
                {
                    services.AddFileSystemStorage(new FileSystemStorageOptions
                    {
                        BaseFolder = Path.Combine(Environment.CurrentDirectory, "wwwroot"),
                    });
                }
                else if (fileProvider == "Azure")
                {
                    services.AddAzureStorage(new AzureStorageOptions
                    {
                        Container = storageConfig.Azure.Container,
                        ConnectionString = storageConfig.Azure.ConnectionString,
                    });
                }
                else if (fileProvider == "AWS")
                {
                    var awsConfig = new AmazonS3Config();
                    awsConfig.RegionEndpoint = RegionEndpoint.GetBySystemName(storageConfig.AWS.RegionEndpoint); ;
                    awsConfig.ForcePathStyle = true;
                    awsConfig.UseHttp = true;
                    services.AddAWSStorage(new AWSStorageOptions
                    {
                        PublicKey = storageConfig.AWS.PublicKey,
                        SecretKey = storageConfig.AWS.SecretKey,
                        Bucket = storageConfig.AWS.Bucket,
                        OriginalOptions = awsConfig
                    });
                }
            }
            return services;

        }
    }
}
