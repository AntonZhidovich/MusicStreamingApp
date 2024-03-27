using Confluent.Kafka;
using FluentValidation;
using FluentValidation.AspNetCore;
using Identity.Grpc;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Minio;
using MongoDB.Driver;
using MusicService.API.Middleware;
using MusicService.Application.Consumers;
using MusicService.Application.Interfaces;
using MusicService.Application.Options;
using MusicService.Application.Services;
using MusicService.Application.Validators;
using MusicService.Domain.Interfaces;
using MusicService.Infrastructure.Data;
using MusicService.Infrastructure.Options;
using MusicService.Infrastructure.Repositories;
using System.Text;

namespace MusicService.API.Extensions
{
    public static class ConfigurationExtensions
    {
        public static IServiceCollection ApplyConfigurations(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<MongoDbOptions>(configuration.GetSection("MongoDbOptions"));
            services.Configure<MinioOptions>(configuration.GetSection("MinioOptions"));
            services.Configure<ConsumerConfig>(configuration.GetSection("KafkaConsumerConfig"));
            services.Configure<KafkaTopics>(configuration.GetSection("KafkaTopics"));
            services.Configure<RedisConfig>(configuration.GetSection("RedisConfig"));

            return services;
        }

        public static IServiceCollection AddServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddExceptionHandler<GlobalExceptionHandler>();
            services.AddValidation();
            services.AddJwtAuthentication(configuration);
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddScoped<IAuthorService, AuthorService>();
            services.AddScoped<IGenreService, GenreService>();
            services.AddScoped<ISongService, SongService>();
            services.AddScoped<IReleaseService, ReleaseService>();
            services.AddScoped<IPlaylistService, PlaylistService>();
            services.AddScoped<IUserServiceGrpcClient, UserServiceGrpcClient>();
            services.AddKafka();
            services.AddGrpc();

            return services;
        }

        public static IServiceCollection AddDatabases(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<MusicDbContext>(options =>
            options.UseSqlServer(configuration.GetConnectionString("sqlserver")));

            services.AddSingleton<IMongoClient>(provider => new MongoClient(
                configuration["MongoDbOptions:ConnectionString"]));

            var endpoint = configuration["MinioOptions:Endpoint"];
            var accessKey = configuration["MinioOptions:AccessKey"];
            var secretKey = configuration["MinioOptions:SecretKey"];
            services.AddSingleton<IMinioClient>(provider => new MinioClient()
            .WithEndpoint(endpoint)
            .WithCredentials(accessKey, secretKey)
            .WithSSL(false)
            .Build());

            services.AddStackExchangeRedisCache(options =>
            {
                options.Configuration = configuration["RedisConfig:ConnectionString"];
            });

            return services;
        }

        public static IServiceCollection AddRepositories(this IServiceCollection services)
        {
            services.AddScoped<IAuthorRepository, AuthorRepository>();
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IGenreRepository, GenreRepository>();
            services.AddScoped<ISongRepository, SongRepository>();
            services.AddScoped<IReleaseRepository, ReleaseRepository>();
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddScoped<IPlaylistRepository, PlaylistRepository>();
            services.AddScoped<ISongSourceRepository, SongSourceRepository>();
            services.AddScoped<ICacheRepository, CacheRepository>();

            return services;
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

        private static IServiceCollection AddJwtAuthentication(this IServiceCollection services, IConfiguration configuration)
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
                    IssuerSigningKey = securityKey,
                    ValidateIssuerSigningKey = true,
                    ValidateAudience = false
                };
            });

            return services;
        }

        private static IServiceCollection AddValidation(this IServiceCollection services)
        {
            services.AddFluentValidationAutoValidation();
            services.AddValidatorsFromAssemblyContaining<CreateAuthorValidator>();

            return services;
        }

        public static IServiceCollection AddKafka(this IServiceCollection services)
        {
            services.AddHostedService<UserDeletedConsumer>();
            services.AddHostedService<UserUpdatedConsumer>();
            services.AddHostedService<SubscriptionMadeConsumer>();
            services.AddHostedService<SubscriptionCanceledConsumer>();

            return services;
        }

        public static IServiceCollection AddGrpcClients(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddGrpcClient<UserService.UserServiceClient>(options =>
            {
                options.Address = new Uri(configuration["GrpcConfig:Identity:Uri"]!);
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
