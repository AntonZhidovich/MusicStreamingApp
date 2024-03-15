using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Ocelot.DependencyInjection;
using System.Text;

namespace ApiGateway.Extensions
{
    public static class ConfigurationExtensions
    {
        public static IServiceCollection ConfigureOcelot(
            this IServiceCollection services, 
            IConfigurationManager configuration,
            IWebHostEnvironment environment) 
        {
            configuration
                .AddJsonFile("ocelot.json", optional: false, reloadOnChange: true)
                .AddJsonFile($"ocelot.{environment.EnvironmentName}.json", optional: true, reloadOnChange: true);

            services.AddOcelot(configuration);
            services.AddSwaggerForOcelot(configuration);

            return services;
        }

        public static IApplicationBuilder UseDownstreamSwagger(this WebApplication app)
        {
            if (app.Environment.IsDevelopment())
            {
                app.UseSwaggerForOcelotUI(options =>
                {
                    options.PathToSwaggerGenerator = "/swagger/docs";
                });
            }

            return app;
        }

        public static IServiceCollection AddJwtAuthentication(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddAuthentication(options =>
            options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(options =>
            {
                var key = configuration["JwtOptions:Key"]!;
                var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key));

                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidIssuer = configuration["JwtOptions:Issuer"],
                    ValidateLifetime = true,
                    IssuerSigningKey = securityKey,
                    ValidateIssuerSigningKey = true,
                    ValidateAudience = false
                };
            });

            return services;
        }

        public static IServiceCollection AddCorsPolicy(this IServiceCollection services)
        {
            services.AddCors(options =>
            {
                options.AddDefaultPolicy(config =>
                {
                    config.AllowAnyOrigin()
                        .AllowAnyMethod()
                        .AllowAnyHeader();
                });
            });

            return services;
        }
    }
}
