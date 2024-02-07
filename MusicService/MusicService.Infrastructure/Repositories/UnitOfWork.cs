using MusicService.Domain.Interfaces;
using MusicService.Infrastructure.Data;

namespace MusicService.Infrastructure.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly MusicDbContext _context;

        public IAuthorRepository Authors { get; }
        public IGenreRepository Genres { get; }
        public ISongRepository Songs { get; }
        public IUserRepository Users { get; }

        public UnitOfWork(
            MusicDbContext context,
            IAuthorRepository authors,
            IGenreRepository genreRepository,
            ISongRepository songRepository,
            IUserRepository users)
        {
            _context = context;
            Authors = authors;
            Genres = genreRepository;
            Songs = songRepository;
            Users = users;
        }

        public async Task CommitAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}
