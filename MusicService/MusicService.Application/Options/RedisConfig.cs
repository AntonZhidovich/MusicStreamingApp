namespace MusicService.Application.Options
{
    public class RedisConfig
    {
        public string ConnectionString { get; set; }
        public int LifetimeMinutes { get; set; }
    }
}
