using Academy.Application.Common.Storage;

namespace Academy.Infrastructure.Storage.FileSystem.Options
{
    public class FileSystemStorageOptions : IStorageOptions
    {
        public string? BaseFolder { get; set; }

        public bool CreateContainerIfNotExists { get; set; } = true;
    }
}
