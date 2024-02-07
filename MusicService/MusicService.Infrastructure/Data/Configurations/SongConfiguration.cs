using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MusicService.Domain.Entities;

namespace MusicService.Infrastructure.Data.Configurations
{
    internal class SongConfiguration : IEntityTypeConfiguration<Song>
    {
        public void Configure(EntityTypeBuilder<Song> builder)
        {
            builder.HasKey(song => song.Id);

            builder.HasMany(song => song.Genre)
                .WithMany(genre => genre.Songs);

            builder.HasOne(song => song.Release)
                .WithMany(release => release.Songs)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
