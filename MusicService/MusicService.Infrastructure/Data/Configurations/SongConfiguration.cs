using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using MusicService.Domain.Entities;
using System.Globalization;

namespace MusicService.Infrastructure.Data.Configurations
{
    internal class SongConfiguration : IEntityTypeConfiguration<Song>
    {
        private const int titleMaxLength = 50;
        private const int idMaxLength = 50;
        private const int sourceMaxLength = 100;
        private const string timeSpanFormat = "hh\\:mm\\:ss";

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

            builder.Property(song => song.DurationMinutes)
                .HasConversion(src => src.ToString(), dest => TimeSpan.ParseExact(dest, timeSpanFormat, CultureInfo.InvariantCulture))
                .IsRequired()
                .HasMaxLength(timeSpanFormat.Length);

            builder.Property(song => song.SourceName)
                .IsRequired()
                .HasMaxLength(sourceMaxLength);
        }
    }
}
