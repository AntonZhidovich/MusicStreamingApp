using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MusicService.Domain.Constants;
using MusicService.Domain.Entities;

namespace MusicService.Infrastructure.Data.Configurations
{
    internal class AuthorConfiguration : IEntityTypeConfiguration<Author>
    {
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
                .HasMaxLength(Constraints.idMaxLength);

            builder.Property(author => author.Name)
                .IsRequired()
                .HasMaxLength(Constraints.authorNameMaxLength);

            builder.Property(author => author.Description)
                .HasMaxLength(Constraints.descriptionMaxLength);

            builder.Property(author => author.CreatedAt).IsRequired();
            builder.Property(author => author.IsBroken).IsRequired();
            builder.Property(author => author.BrokenAt).IsRequired();
        }
    }
}
