using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MusicService.Domain.Entities;

namespace MusicService.Infrastructure.Data.Configurations
{
    internal class ReleaseConfiguration : IEntityTypeConfiguration<Release>
    {
        private const int idMaxLength = 50;
        private const int nameMaxLength = 100;
        private const int typeMaxLength = 20;

        public void Configure(EntityTypeBuilder<Release> builder)
        {
            builder.HasKey(release => release.Id);

            builder.HasIndex(release => release.Name)
                .IsUnique();

            builder.HasMany(release => release.Authors)
                .WithMany(author => author.Releases);

            builder.Property(release => release.Id)
                .IsRequired()
                .HasMaxLength(idMaxLength);

            builder.Property(release => release.Name)
                .IsRequired()
                .HasMaxLength(nameMaxLength);

            builder.Property(release => release.Type)
                .IsRequired()
                .HasMaxLength(typeMaxLength);

            builder.Property(release => release.DurationMinutes).IsRequired();
            builder.Property(release => release.SongsCount).IsRequired();
            builder.Property(release => release.ReleasedAt).IsRequired();
        }
    }
}
