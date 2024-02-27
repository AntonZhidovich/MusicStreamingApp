using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MusicService.Domain.Constants;
using MusicService.Domain.Entities;
using System.Globalization;

namespace MusicService.Infrastructure.Data.Configurations
{
    internal class ReleaseConfiguration : IEntityTypeConfiguration<Release>
    {
        public void Configure(EntityTypeBuilder<Release> builder)
        {
            builder.HasKey(release => release.Id);

            builder.HasMany(release => release.Authors)
                .WithMany(author => author.Releases);

            builder.Property(release => release.Id)
                .IsRequired()
                .HasMaxLength(Constraints.idMaxLength);

            builder.Property(release => release.Name)
                .IsRequired()
                .HasMaxLength(Constraints.releaseNameMaxLength);

            builder.Property(release => release.Type)
                .IsRequired()
                .HasMaxLength(Constraints.releaseTypeMaxLength);

            builder.Property(release => release.DurationMinutes)
                .HasConversion(src => src.ToString(), dest => TimeSpan.ParseExact(dest, Constraints.timeSpanFormat, CultureInfo.InvariantCulture))
                .IsRequired()
                .HasMaxLength(Constraints.timeSpanFormat.Length);

            builder.Property(release => release.SongsCount).IsRequired();
            builder.Property(release => release.ReleasedAt).IsRequired();
        }
    }
}
