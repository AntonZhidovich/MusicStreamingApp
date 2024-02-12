using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MusicService.Domain.Entities;

namespace MusicService.Infrastructure.Data.Configurations
{
    internal class SongConfiguration : IEntityTypeConfiguration<Song>
    {
        private const int titleMaxLength = 50;
        private const int idMaxLength = 50;
        private const int sourceMaxLength = 100;

        public void Configure(EntityTypeBuilder<Song> builder)
        {
            builder.HasKey(song => song.Id);

            builder.HasMany(song => song.Genres)
                .WithMany(genre => genre.Songs);

            builder.HasOne(song => song.Release)
                .WithMany(release => release.Songs)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Property(song => song.Id)
                .IsRequired()
                .HasMaxLength(idMaxLength);

            builder.Property(song => song.Title)
                .IsRequired()
                .HasMaxLength(titleMaxLength);

            builder.Property(song => song.DurationMinutes).IsRequired();

            builder.Property(song => song.SourceName)
                .IsRequired()
                .HasMaxLength(sourceMaxLength);
        }
    }
}
