namespace Academy.Application.Common.Storage
{
    public interface IStorageOptions
    {
        public bool CreateContainerIfNotExists { get; set; }
    }
}
