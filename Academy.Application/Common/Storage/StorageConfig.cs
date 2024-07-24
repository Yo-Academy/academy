namespace Academy.Application.Common.Storage
{
    public class StorageConfig
    {
        public string FileProvider { get; set; }

        public AWS AWS { get; set; }

        public Azure Azure { get; set; }

    }

    public class AWS
    {
        public string PublicKey { get; set; }

        public string SecretKey { get; set; }

        public string Bucket { get; set; }

        public string RegionEndpoint { get; set; }
    }

    public class Azure
    {
        public string Container { get; set; }

        public string ConnectionString { get; set; }
    }
}
