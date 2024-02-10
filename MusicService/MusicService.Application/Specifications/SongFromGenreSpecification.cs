using MusicService.Domain.Entities;
using MusicService.Domain.Interfaces;
using System.Linq.Expressions;

namespace MusicService.Infrastructure.Specifications
{
    public class SongFromGenreSpecification : ISpecification<Song>
    {
        public Expression<Func<Song, bool>> Criteria { get; }

        public Expression<Func<Song, object>>? OrderBy { get; } = null;

        public Expression<Func<Song, object>>? OrderByDescending { get; }

        public List<string> Includes { get; }

        public SongFromGenreSpecification(string genre)
        {
            Song song = new Song();

            Includes = new List<string>
            {
                $"{nameof(song.Genres)}",
                $"{nameof(song.Release)}.{nameof(song.Release.Authors)}"
            };

            Criteria = song => song.Genres.Select(genre => genre.Name).Contains(genre);
            OrderByDescending = song => song.Release.ReleasedAt;
        }

        public bool IsSatisfiedBy(Song song)
        {
            return Criteria.Compile()(song);
        }
    }
}
