using Academy.Application.Common.Storage;
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;

namespace Academy.Infrastructure.Storage.Azure.Options
{
    public interface IAzureStorageOptions : IStorageOptions
    {
        public string? Container { get; set; }
        public PublicAccessType PublicAccessType { get; set; }
        public BlobClientOptions? OriginalOptions { get; set; }

        public string? ConnectionString { get; set; }
    }
}