﻿namespace MusicService.Application.Models.DTOs
{
    public class FullPlaylistDto
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string UserName { get; set; }
        public DateTime CreatedAt { get; set; }
        public List<SongDto> Songs { get; set; }
    }
}
