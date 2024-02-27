using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using MusicService.Domain.Constants;
using MusicService.Domain.Entities;
using System.Globalization;

namespace MusicService.Infrastructure.Data.Configurations
{
    internal class SongConfiguration : IEntityTypeConfiguration<Song>
    {
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
                .HasMaxLength(Constraints.idMaxLength);

            builder.Property(song => song.Title)
                .IsRequired()
                .HasMaxLength(Constraints.songTitleMaxLength);

            builder.Property(song => song.DurationMinutes)
                .HasConversion(src => src.ToString(), dest => TimeSpan.ParseExact(dest, Constraints.timeSpanFormat, CultureInfo.InvariantCulture))
                .IsRequired()
                .HasMaxLength(Constraints.timeSpanFormat.Length);

            builder.Property(song => song.SourceName)
                .IsRequired()
                .HasMaxLength(Constraints.songSourceMaxLength);
        }
    }
}
