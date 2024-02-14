using Microsoft.AspNetCore.Http;

namespace MusicService.Application.Models.SongService
{
    public class UploadSongSourceRequest
    {
        public string AuthorName { get; set; }
        public IFormFile sourceFile { get; set; }
    }
}
