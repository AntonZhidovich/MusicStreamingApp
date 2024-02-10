using MusicService.Domain.Entities;
using MusicService.Domain.Interfaces;
using System.Linq.Expressions;

namespace MusicService.Application.Specifications
{
    public class SongByNameSpecification : ISpecification<Song>
    {
        public Expression<Func<Song, bool>> Criteria { get; }

        public Expression<Func<Song, object>>? OrderBy { get; } = null;

        public Expression<Func<Song, object>>? OrderByDescending { get; }

        public List<string> Includes { get; }

        public SongByNameSpecification(string name)
        {
            OrderByDescending = song => song.Release.ReleasedAt;
            Song song = new Song();

            Includes = new List<string>
            {
                $"{nameof(song.Genres)}",
                $"{nameof(song.Release)}.{nameof(song.Release.Authors)}"
            };

            Criteria = song => song.SourceName.Trim().ToLower() == name.Trim().ToLower();
            OrderByDescending = song => song.Release.ReleasedAt;
        }

        public bool IsSatisfiedBy(Song entity)
        {
            throw new NotImplementedException();
        }
    }
}
