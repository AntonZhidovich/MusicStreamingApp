using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
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
        }
    }
}
