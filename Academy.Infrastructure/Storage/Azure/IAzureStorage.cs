using Academy.Application.Common.Storage;
using Azure.Storage.Blobs;
using ManagedCode.Communication;

namespace Academy.Infrastructure.Storage.Azure
{
    public interface IAzureStorage : IStorage<BlobContainerClient, IStorageOptions>
    {
        Task<Result<Stream>> OpenReadStreamAsync(string fileName, CancellationToken cancellationToken = default);
        Task<Result<Stream>> OpenWriteStreamAsync(string fileName, CancellationToken cancellationToken = default);

        Stream GetBlobStream(string fileName, bool userBuffer = true, int bufferSize = BlobStream.DefaultBufferSize);
    }
}
