using Confluent.Kafka;
using FluentValidation;
using FluentValidation.AspNetCore;
using Identity.API.Middleware;
using Identity.BusinessLogic.Mapping;
using Identity.BusinessLogic.Options;
using Identity.BusinessLogic.Services.Implementations;
using Identity.BusinessLogic.Services.Interfaces;
using Identity.BusinessLogic.Validators;
using Identity.DataAccess.Data;
using Identity.DataAccess.Entities;
using Identity.DataAccess.Repositories.Implementations;
using Identity.DataAccess.Repositories.Interfaces;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using MusicService.Grpc;
using System.Text;

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
            
            services.AddIdentityCore<User>()
                .AddRoles<IdentityRole>()
                .AddEntityFrameworkStores<UserDBContext>();
            
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<ITokenService, TokenService>();
            services.AddScoped<ISignInService, SignInService>();
            services.AddScoped<IRoleService, RoleService>();
            services.AddKafka();
            services.AddScoped<IMusicUserGrpcServiceClient, MusicUserGrpcServiceClient>();
            services.AddGrpc();

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
            services.Configure<ProducerConfig>(configuration.GetSection("KafkaProducerConfig"));
            services.Configure<KafkaTopics>(configuration.GetSection("KafkaTopics"));

            return services;
        }

        public static IServiceCollection AddDataBase(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<UserDBContext>(
                options => options.UseSqlServer(configuration.GetConnectionString("SQLServer")));

            return services;
        }

        public static IServiceCollection AddJwtAuthentication(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddAuthentication(options =>
            options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(options =>
            {
                var key = configuration["JwtOptions:Key"];
                var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key!));

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

        public static IApplicationBuilder UseMiddleware(this IApplicationBuilder app)
        {
            app.UseMiddleware<LoggingMiddleware>();
            app.UseExceptionHandler(options => { });

            return app;
        }

        public static IServiceCollection ConfigureSwagger(this IServiceCollection services)
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

        public static IServiceCollection AddKafka(this IServiceCollection services)
        {
            services.AddScoped(provider =>
            {
                var configOptions = provider.GetService<IOptions<ProducerConfig>>()!;

                var producer = new ProducerBuilder<string, string>(configOptions.Value)
                    .Build();

                return producer;
            });

            services.AddScoped<IProducerService, ProducerService>();

            return services;
        }

        public static IServiceCollection AddGrpcClients(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddGrpcClient<MusicUserService.MusicUserServiceClient>(options =>
            {
                options.Address = new Uri(configuration["GrpcConfig:MusicService:Uri"]!);
            })
            .ConfigurePrimaryHttpMessageHandler(() =>
            {
                var handler = new HttpClientHandler();
                handler.ServerCertificateCustomValidationCallback = HttpClientHandler.DangerousAcceptAnyServerCertificateValidator!;

                return handler;
            });

            return services;
        }

        public static IServiceCollection AddCorsPolicy(this IServiceCollection services, IConfiguration configuration)
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
