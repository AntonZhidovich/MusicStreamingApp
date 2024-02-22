using Microsoft.EntityFrameworkCore;
using SubscriptionService.API.ExceptionHandlers;
using SubscriptionService.BusinessLogic.Mapping;
using SubscriptionService.BusinessLogic.Queries.GetAllTariffPlans;
using SubscriptionService.DataAccess.Data;
using SubscriptionService.DataAccess.Repositories.Implementations;
using SubscriptionService.DataAccess.Repositories.Interfaces;

namespace SubscriptionService.API.Extensions
{
    public static class ConfigurationExtenstions
    {
        public static IServiceCollection AddServices(this IServiceCollection services)
        {
            services.AddAutoMapper(typeof(MappingProfile));
            services.AddMediatR(config => config.RegisterServicesFromAssemblyContaining(typeof(GetAllTariffPlansQuery)));
            services.AddExceptionHandler<GlobalExceptionHandler>();

            return services;
        }

        public static IServiceCollection AddDatabase(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<SubscriptionDbContext>(options =>
            options.UseSqlServer(configuration.GetConnectionString("sqlserver")));
            
            return services;
        }

        public static IServiceCollection AddRepositories(this IServiceCollection services)
        {
            services.AddScoped<ITariffPlanRepository, TariffPlanRepository>();
            services.AddScoped<ISubscriptionRepository, SubscriptionRepository>();
            services.AddScoped<IUnitOfWork, UnitOfWork>();

            return services;
        }
    }
}
