using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MusicService.Domain.Entities;

namespace MusicService.Infrastructure.Data.Configurations
{
    internal class AuthorConfiguration : IEntityTypeConfiguration<Author>
    {
        private const int idMaxLength = 50;
        private const int nameMaxLength = 50;
        private const int descriptionMaxLength = 300;

        public void Configure(EntityTypeBuilder<Author> builder)
        {
            builder.HasKey(author => author.Id);

            builder.HasMany(author => author.Users)
                .WithOne(user => user.Author)
                .OnDelete(DeleteBehavior.SetNull)
                .HasForeignKey(user => user.AuthorId);

            builder.HasIndex(author => author.Name)
                .IsUnique();

            builder.Property(author => author.Id)
                .IsRequired()
                .HasMaxLength(idMaxLength);

            builder.Property(author => author.Name)
                .IsRequired()
                .HasMaxLength(nameMaxLength);

            builder.Property(author => author.Description).HasMaxLength(descriptionMaxLength);

            builder.Property(author => author.CreatedAt).IsRequired();
            builder.Property(author => author.IsBroken).IsRequired();
            builder.Property(author => author.BrokenAt).IsRequired();
        }
    }
}
