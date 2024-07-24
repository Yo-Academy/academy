namespace Academy.Application.Common.Storage.Models
{
    public abstract class BaseOptions
    {
        public string FileName { get; set; } = $"{DefaultIdType.NewGuid():N}";
        public string? Directory { get; set; }

        // TODO: Check this
        public string FullPath => string.IsNullOrWhiteSpace(Directory) ? FileName : $"{Directory}/{FileName}";
    }
}
