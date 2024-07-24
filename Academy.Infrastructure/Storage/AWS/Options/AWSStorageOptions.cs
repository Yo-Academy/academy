using Academy.Application.Common.Storage;
using Amazon.S3;

namespace Academy.Infrastructure.Storage.AWS.Options
{
    public class AWSStorageOptions : IStorageOptions
    {
        public string? PublicKey { get; set; }
        public string? SecretKey { get; set; }
        public string? Bucket { get; set; }
        public AmazonS3Config? OriginalOptions { get; set; } = new();

        public bool CreateContainerIfNotExists { get; set; } = true;
    }
}
