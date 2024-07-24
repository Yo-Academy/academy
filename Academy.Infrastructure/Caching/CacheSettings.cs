namespace Academy.Infrastructure.Caching
{
    public class CacheSettings
    {
        public bool UseDistributedCache { get; set; }
        public bool PreferDatabase { get; set; }
        public bool PreferRedis { get; set; }
        public string? RedisURL { get; set; }
    }
}