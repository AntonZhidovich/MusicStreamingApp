﻿namespace MusicService.Application.Models.ReleaseService
{
    public class AddSongToReleaseRequest
    {
        public string Title { get; set; }
        public List<string> Genres { get; set; }
        public int DurationMinutes { get; set; }
        public string SourceName { get; set; }
    }
}