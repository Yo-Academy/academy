namespace Academy.Application.Common.Storage.Models
{
    public class UploadOptions : BaseOptions
    {
        public UploadOptions()
        {
        }

        public UploadOptions(string? fileName = null,
            string? directory = null,
            string? mimeType = null,
            Dictionary<string, string>? metadata = null,
            string? fileNamePrefix = null)
        {
            FileName = fileName ?? FileName;
            MimeType = mimeType;
            Directory = directory;
            Metadata = metadata;
            FileNamePrefix = fileNamePrefix;
        }

        public string? FileNamePrefix { get; set; }
        public string? MimeType { get; set; }
        public Dictionary<string, string>? Metadata { get; set; }
    }
}
