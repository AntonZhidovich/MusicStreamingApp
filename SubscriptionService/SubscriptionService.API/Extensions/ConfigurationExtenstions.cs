using Microsoft.EntityFrameworkCore;
using SubscriptionService.DataAccess.Data;

namespace SubscriptionService.API.Extensions
{
    public static class ConfigurationExtenstions
    {
        public static IServiceCollection AddServices(this IServiceCollection services)
        {
            return services;
        }

        public static IServiceCollection AddDatabase(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<SubscriptionDbContext>(options =>
            options.UseSqlServer(configuration.GetConnectionString("sqlserver")));
            
            return services;
        }
    }
}
