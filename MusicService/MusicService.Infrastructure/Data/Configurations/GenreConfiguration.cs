using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MusicService.Domain.Entities;

namespace MusicService.Infrastructure.Data.Configurations
{
    internal class GenreConfiguration : IEntityTypeConfiguration<Genre>
    {
        public void Configure(EntityTypeBuilder<Genre> builder)
        {
            builder.HasKey(genre => genre.Id);

            builder.HasIndex(genre => genre.Name)
                .IsUnique();
        }
    }
}
