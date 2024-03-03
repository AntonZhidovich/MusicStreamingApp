using Microsoft.EntityFrameworkCore;
using SubscriptionService.DataAccess.Entities;

namespace SubscriptionService.DataAccess.Extensions
{
    public static class DbSeedExtension
    {
        public static void Seed(this ModelBuilder builder)
        {
            builder.Entity<TariffPlan>().HasData(
                new TariffPlan
                {
                    Id = "7dad33ee-b174-4c27-972e-ddd969145d52",
                    Name = "Base",
                    Description = "A minimal plan for those who simply enjoy music.",
                    MaxPlaylistsCount = 3,
                    MonthFee = 2.99,
                    AnnualFee = 29.99
                },
                new TariffPlan
                {
                    Id = "83e7cb90-520c-45c2-b1f2-222974cb74c5",
                    Name = "Enhanced",
                    Description = "Tariff plan for multifaceted personality who sets the mood of the day with personal playlists.",
                    MaxPlaylistsCount = 7,
                    MonthFee = 3.99,
                    AnnualFee = 35.99
                },
                new TariffPlan
                {
                    Id = "1ece6d0a-a08b-4839-a1c5-efe06496df64",
                    Name = "Push the boundaries",
                    Description = "Set playlists for every important moment of your life to share it with you favorite artists.",
                    MaxPlaylistsCount = 25,
                    MonthFee = 6.99,
                    AnnualFee = 59.99
                });
        }
    }
}
