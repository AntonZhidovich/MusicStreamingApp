using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MusicService.Domain.Entities;

namespace MusicService.Infrastructure.Data.Configurations
{
    internal class ReleaseConfiguration : IEntityTypeConfiguration<Release>
    {
        public void Configure(EntityTypeBuilder<Release> builder)
        {
            builder.HasKey(release => release.Id);

            builder.HasIndex(release => release.Name)
                .IsUnique();

            builder.HasMany(release => release.Authors)
                .WithMany(author => author.Releases);
        }
    }
}
