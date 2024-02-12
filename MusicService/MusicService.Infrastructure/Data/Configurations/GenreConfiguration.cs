using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MusicService.Domain.Entities;

namespace MusicService.Infrastructure.Data.Configurations
{
    internal class GenreConfiguration : IEntityTypeConfiguration<Genre>
    {
        private const int idMaxLength = 50;
        private const int nameMaxLength = 30;
        private const int descriptionMaxLength = 300;

        public void Configure(EntityTypeBuilder<Genre> builder)
        {
            builder.HasKey(genre => genre.Id);

            builder.HasIndex(genre => genre.Name)
                .IsUnique();

            builder.Property(genre => genre.Id)
                .IsRequired()
                .HasMaxLength(idMaxLength);

            builder.Property (genre => genre.Name)
                .IsRequired()
                .HasMaxLength(nameMaxLength);

            builder.Property(genre => genre.Description).HasMaxLength(descriptionMaxLength);
        }
    }
}
