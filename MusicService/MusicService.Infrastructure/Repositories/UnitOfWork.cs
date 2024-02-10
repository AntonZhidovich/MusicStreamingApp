using Microsoft.Extensions.DependencyInjection;
using MusicService.Domain.Interfaces;
using MusicService.Infrastructure.Data;

namespace MusicService.Infrastructure.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly MusicDbContext _context;
        private readonly IServiceProvider _serviceProvider;
        private IAuthorRepository? authors = null;
        private IGenreRepository? genres = null;
        private IReleaseRepository? releases = null;
        private ISongRepository? songs = null;
        private IUserRepository? users = null;

        public IAuthorRepository Authors => authors ??= _serviceProvider.GetService<IAuthorRepository>()!;
        public IGenreRepository Genres => genres ??= _serviceProvider.GetService<IGenreRepository>()!;
        public IReleaseRepository Releases => releases ??= _serviceProvider.GetService<IReleaseRepository>()!;
        public ISongRepository Songs => songs ??= _serviceProvider.GetService<ISongRepository>()!;
        public IUserRepository Users => users ??= _serviceProvider.GetService<IUserRepository>()!;

        public UnitOfWork( 
            MusicDbContext context,
            IServiceProvider serviceProvider)
        {
            _context = context;
            _serviceProvider = serviceProvider;
        }

        public async Task CommitAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}
