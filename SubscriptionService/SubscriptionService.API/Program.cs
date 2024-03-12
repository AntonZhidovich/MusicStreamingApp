using Hangfire;
using SubscriptionService.API.Extensions;
using SubscriptionService.API.Filters;
using SubscriptionService.API.Middleware;
using SubscriptionService.DataAccess.Data;

namespace SubscriptionService.API
{
    public static class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            builder.Services.ApplyConfigurations(builder.Configuration);
            builder.Services.AddDatabase(builder.Configuration);
            builder.Services.AddRepositories();
            builder.Services.AddServices();
            builder.Services.AddJwtAuthentication(builder.Configuration);
            builder.Services.AddControllers();
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwagger();
            builder.Services.AddGrpcClients(builder.Configuration);
            builder.Services.AddHangfire(builder.Configuration);
            builder.Host.UseLogging();

            var app = builder.Build();
            app.Services.MigrateDatabase<SubscriptionDbContext>();

            app.UseMiddleware<LoggingMiddleware>();
            app.UseExceptionHandler(options => { });

            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();
            app.UseAuthorization();

            app.UseHangfireDashboard(options: new DashboardOptions
            {
                Authorization = new[] { new HangfireAuthorizationFilter() }
            });

            app.MapControllers();
            app.Run();
        }
    }
}
