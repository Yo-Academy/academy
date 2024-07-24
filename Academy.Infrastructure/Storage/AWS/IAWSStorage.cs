using Academy.Application.Common.Storage;
using Academy.Infrastructure.Storage.AWS.Options;
using Amazon.S3;

namespace Academy.Infrastructure.Storage.AWS
{
    public interface IAWSStorage : IStorage<IAmazonS3, AWSStorageOptions>
    {
    }
}
