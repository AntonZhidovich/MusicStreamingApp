using Identity.DataAccess.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Identity.DataAccess.Data
{
    internal class RefresTokenConfigureation : IEntityTypeConfiguration<RefreshToken>
    {
        public void Configure(EntityTypeBuilder<RefreshToken> builder)
        {
            builder
                .HasIndex(token => token.Token)
                .IsUnique(true);
            builder.HasKey(token => token.Token);
            builder
                .HasOne(token => token.User)
                .WithOne(user => user.RefreshToken)
                .HasForeignKey<RefreshToken>(token => token.UserId);
        }
    }
}
