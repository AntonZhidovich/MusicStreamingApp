namespace MusicService.Application.Options
{
    public class MongoDbOptions
    {
        public string DatabaseName { get; set; }
        public string PlaylistsCollectionName { get; set; }
        public string ConnectionString { get; set; }
    }
}
