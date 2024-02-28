using Microsoft.EntityFrameworkCore;
using SubscriptionService.DataAccess.Entities;
using SubscriptionService.DataAccess.Extensions;
using System.Reflection;

namespace SubscriptionService.DataAccess.Data
{
    public class SubscriptionDbContext : DbContext
    {
        public DbSet<Subscription> Subscriptions { get; set; }
        public DbSet<TariffPlan> TariffPlans { get; set; }

        public SubscriptionDbContext(DbContextOptions options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
            modelBuilder.Seed();
            base.OnModelCreating(modelBuilder);
        }
    }
}