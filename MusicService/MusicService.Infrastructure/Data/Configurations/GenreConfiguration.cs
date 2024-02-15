using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MusicService.Domain.Constants;
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

            builder.Property(genre => genre.Id)
                .IsRequired()
                .HasMaxLength(Constraints.idMaxLength);

            builder.Property (genre => genre.Name)
                .IsRequired()
                .HasMaxLength(Constraints.genreNameMaxLength);

            builder.Property(genre => genre.Description)
                .HasMaxLength(Constraints.descriptionMaxLength);
        }
    }
}
