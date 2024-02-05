using FluentValidation;
using FluentValidation.AspNetCore;
using Identity.API.ExceptionHandlers;
using Identity.BusinessLogic.Mapping;
using Identity.BusinessLogic.Options;
using Identity.BusinessLogic.Services.Implementations;
using Identity.BusinessLogic.Services.Interfaces;
using Identity.BusinessLogic.Validators;
using Identity.DataAccess.Data;
using Identity.DataAccess.Entities;
using Identity.DataAccess.Repositories.Implementations;
using Identity.DataAccess.Repositories.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Identity.API.Extensions
{
    public static class ConfigurationExtensions
    {
        public static IServiceCollection AddServices(this IServiceCollection services)
        {
            services.AddFluentValidationAutoValidation();
            services.AddValidatorsFromAssemblyContaining<RegisterUserValidator>();
            services.AddExceptionHandler<GlobalExceptionHandler>();
            services.AddAutoMapper(typeof(UserMappingProfile));
            services.AddIdentity<User, IdentityRole>().AddEntityFrameworkStores<UserDBContext>();
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<ITokenService, TokenService>();
            services.AddScoped<ISignInService, SignInService>();
            services.AddScoped<IRoleService, RoleService>();

            return services;
        }

        public static IServiceCollection AddRepositories(this IServiceCollection services)
        {
            services.AddScoped<ITokenRepository, TokenRepository>();
            services.AddScoped<IUserRepository, UserRepository>();

            return services;
        }

        public static IServiceCollection ApplyConfigurations(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<JwtOptions>(configuration.GetSection("JwtOptions"));
            services.Configure<IdentityOptions>(configuration.GetSection("IdentityOptions"));

            return services;
        }

        public static IServiceCollection AddDataBase(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<UserDBContext>(
                options => options.UseSqlServer(configuration.GetConnectionString("SQLServer")));

            return services;
        }

        public static IApplicationBuilder UseMiddleware(this IApplicationBuilder app)
        {
            app.UseExceptionHandler(options => { });

            return app;
        }
    }
}
