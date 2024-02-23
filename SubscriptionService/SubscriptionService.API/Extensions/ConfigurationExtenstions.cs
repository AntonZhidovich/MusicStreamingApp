using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using SubscriptionService.API.ExceptionHandlers;
using SubscriptionService.BusinessLogic.Features.Queries.GetAllTariffPlans;
using SubscriptionService.BusinessLogic.Mapping;
using SubscriptionService.BusinessLogic.Validators;
using SubscriptionService.DataAccess.Data;
using SubscriptionService.DataAccess.Repositories.Implementations;
using SubscriptionService.DataAccess.Repositories.Interfaces;
using System.Text;

namespace SubscriptionService.API.Extensions
{
    public static class ConfigurationExtenstions
    {
        public static IServiceCollection AddServices(this IServiceCollection services)
        {
            services.AddAutoValidation();
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

        public static IServiceCollection AddAutoValidation(this IServiceCollection services)
        {
            services.AddValidatorsFromAssemblyContaining(typeof(CreateTariffPlanValidator));
            services.AddFluentValidationAutoValidation();

            return services;
        }

        public static IServiceCollection AddJwtAuthentication(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(options =>
            {
                var key = configuration["JwtOptions:Key"];
                var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key!));

                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidIssuer = configuration["JwtOptions:Issuer"],
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = securityKey,
                    ValidateAudience = false
                };
            });

            return services;
        }

        public static IServiceCollection AddSwagger(this IServiceCollection services)
        {
            services.AddSwaggerGen(options =>
            {
                options.AddSecurityDefinition(JwtBearerDefaults.AuthenticationScheme, new OpenApiSecurityScheme
                {
                    Scheme = JwtBearerDefaults.AuthenticationScheme,
                    BearerFormat = "JWT",
                    In = ParameterLocation.Header,
                    Name = "Authorization",
                    Type = SecuritySchemeType.Http
                });

                var securityRequirement = new OpenApiSecurityRequirement();

                var jwtScheme = new OpenApiSecurityScheme
                {
                    Reference = new OpenApiReference { Id = JwtBearerDefaults.AuthenticationScheme, Type = ReferenceType.SecurityScheme }
                };

                securityRequirement.Add(jwtScheme, new List<string>());
                options.AddSecurityRequirement(securityRequirement);
            });

            return services;
        }
    }
}
