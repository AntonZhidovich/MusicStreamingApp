using System.Text.Json.Serialization;

namespace MusicService.Application.Models.PlaylistService
{
    public class CreatePlaylistRequest
    {
        [JsonIgnore]
        public string Id { get; set; } = Guid.NewGuid().ToString();
        public string Name { get; set; }
        [JsonIgnore]
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        [JsonIgnore]
        public List<string> SongIds { get; set; } = new List<string>();
    }
}
