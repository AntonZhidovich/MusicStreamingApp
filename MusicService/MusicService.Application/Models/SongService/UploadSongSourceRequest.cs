using Microsoft.AspNetCore.Http;

namespace MusicService.Application.Models.SongService
{
    public class UploadSongSourceRequest
    {
        public IFormFile sourceFile { get; set; }
    }
}
