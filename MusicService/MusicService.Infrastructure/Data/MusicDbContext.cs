using Microsoft.EntityFrameworkCore;
using MusicService.Domain.Entities;
using MusicService.Infrastructure.Data.Configurations;

namespace MusicService.Infrastructure.Data
{
    public class MusicDbContext : DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<Author> Authors { get; set; }
        public DbSet<Release> Releases { get; set; }
        public DbSet<Song> Songs { get; set; }
        public DbSet<Genre> Genres { get; set; }

        public MusicDbContext(DbContextOptions options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.ApplyConfiguration(new AuthorConfiguration());
            modelBuilder.ApplyConfiguration(new ReleaseConfiguration());
            modelBuilder.ApplyConfiguration(new SongConfiguration());
            modelBuilder.ApplyConfiguration(new GenreConfiguration());
            modelBuilder.ApplyConfiguration(new UserConfiguration());
            modelBuilder.Seed();
        }
    }
}
