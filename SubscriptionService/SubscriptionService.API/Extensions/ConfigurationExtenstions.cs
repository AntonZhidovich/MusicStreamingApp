﻿using Confluent.Kafka;
using FluentValidation;
using Hangfire;
using Identity.Grpc;
using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using RazorLight;
using SubscriptionService.API.ExceptionHandlers;
using SubscriptionService.BusinessLogic.Features.Behaviors;
using SubscriptionService.BusinessLogic.Features.Consumers;
using SubscriptionService.BusinessLogic.Features.Producers;
using SubscriptionService.BusinessLogic.Features.Queries.GetAllTariffPlans;
using SubscriptionService.BusinessLogic.Features.Services.Implementations;
using SubscriptionService.BusinessLogic.Features.Services.Interfaces;
using SubscriptionService.BusinessLogic.Mapping;
using SubscriptionService.BusinessLogic.Options;
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
            services.AddAutoMapper(typeof(SubscriptionMappingProfile));
            services.AddMediatR(config => config.RegisterServicesFromAssemblyContaining(typeof(GetAllTariffPlansQuery)));
            services.AddExceptionHandler<GlobalExceptionHandler>();
            services.AddKafka();
            services.AddScoped<IUserServiceGrpcClient, UserServiceGrpcClient>();

            services.AddScoped<IRazorLightEngine>(provider =>
            {
                return new RazorLightEngineBuilder()
                .UseEmbeddedResourcesProject(typeof(EmailSenderService).Assembly)
                .UseMemoryCachingProvider()
                .Build();
            });

            services.AddScoped<IEmailSenderService, EmailSenderService>();
            services.AddScoped<IEmailMessageRenderer, EmailMessageRenderer>();
            services.AddScoped<IBackgroundJobsService, BackgroundJobsService>();

            return services;
        }

        public static IServiceCollection AddDatabase(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<SubscriptionDbContext>(options =>
            options.UseSqlServer(configuration.GetConnectionString("sqlserver")));

            services.AddStackExchangeRedisCache(options =>
            {
                options.Configuration = configuration["RedisConfig:ConnectionString"];
            });
            
            return services;
        }

        public static IServiceCollection AddRepositories(this IServiceCollection services)
        {
            services.AddScoped<ITariffPlanRepository, TariffPlanRepository>();
            services.AddScoped<ISubscriptionRepository, SubscriptionRepository>();
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddScoped<ICacheRepository, CacheRepository>();

            return services;
        }

        public static IServiceCollection AddAutoValidation(this IServiceCollection services)
        {
            ValidatorOptions.Global.LanguageManager.Enabled = false;
            services.AddValidatorsFromAssemblyContaining(typeof(CreateTariffPlanValidator));
            services.AddScoped(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));

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

        public static IServiceCollection ApplyConfigurations(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<ConsumerConfig>(configuration.GetSection("KafkaConsumerConfig"));
            services.Configure<ProducerConfig>(configuration.GetSection("KafkaProducerConfig"));
            services.Configure<KafkaTopics>(configuration.GetSection("KafkaTopics"));
            services.Configure<SMTPConfig>(configuration.GetSection("SMTPConfig"));
            services.Configure<EmailSender>(configuration.GetSection("EmailSender"));

            return services;
        }

        public static IServiceCollection AddKafka(this IServiceCollection services)
        {
            services.AddHostedService<UserDeletedConsumer>();

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

        public static IServiceCollection AddHangfire(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddHangfire(options =>
            {
                options.UseSqlServerStorage(configuration["HangfireConfig:ConnectionString"]);
            });

            services.AddHangfireServer();

            return services;
        }
    }
}
