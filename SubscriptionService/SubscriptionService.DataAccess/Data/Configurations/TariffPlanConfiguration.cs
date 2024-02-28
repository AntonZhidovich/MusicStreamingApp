using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SubscriptionService.DataAccess.Constants;
using SubscriptionService.DataAccess.Entities;

namespace SubscriptionService.DataAccess.Data.Configurations
{
    public class TariffPlanConfiguration : IEntityTypeConfiguration<TariffPlan>
    {
        public void Configure(EntityTypeBuilder<TariffPlan> builder)
        {
            builder.HasKey(plan => plan.Id);

            builder.HasIndex(plan => plan.Name)
                .IsUnique();

            builder.Property(plan => plan.Id)
                .HasMaxLength(Constraints.idMaxLength);

            builder.Property(plan => plan.Name)
                .HasMaxLength(Constraints.tariffPlanNameMaxLength);

            builder.Property(plan => plan.Description)
                .IsRequired()
                .HasMaxLength(Constraints.descpriptionMaxLength);

            builder.Property(plan => plan.MaxPlaylistsCount).IsRequired();
            builder.Property(plan => plan.MonthFee).IsRequired();
            builder.Property(plan => plan.AnnualFee).IsRequired();
        }
    }
}