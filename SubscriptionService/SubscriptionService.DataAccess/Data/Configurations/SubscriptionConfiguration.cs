using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SubscriptionService.DataAccess.Constants;
using SubscriptionService.DataAccess.Entities;

namespace SubscriptionService.DataAccess.Data.Configurations
{
    internal class SubscriptionConfiguration : IEntityTypeConfiguration<Subscription>
    {
        public void Configure(EntityTypeBuilder<Subscription> builder)
        {
            builder.HasKey(sub => sub.Id);

            builder.HasIndex(sub => sub.UserName)
                .IsUnique();

            builder.HasOne(sub => sub.TariffPlan)
                .WithMany(plan => plan.Subscriptions)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Property(sub => sub.Id)
                .HasMaxLength(Constraints.idMaxLength);

            builder.Property(sub => sub.UserName)
                .IsRequired()
                .HasMaxLength(Constraints.usernameMaxLength);

            builder.Property(sub => sub.Type)
                .IsRequired()
                .HasMaxLength(Constraints.subscriptionTypeMaxLength);

            builder.Property(sub => sub.Fee).IsRequired();
            builder.Property(sub => sub.SubscribedAt).IsRequired();
        }
    }
}
