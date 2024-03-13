using MusicService.API.Extensions;
using MusicService.API.Middleware;
using MusicService.Application.Mapping;
using MusicService.Infrastructure.Data;

namespace MusicService.API
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            builder.Services.ApplyConfigurations(builder.Configuration);
            builder.Services.AddDatabases(builder.Configuration);
            builder.Services.AddRepositories();
            builder.Services.AddServices(builder.Configuration);
            builder.Services.AddAutoMapper(typeof(AuthorMappingProfile).Assembly);
            builder.Services.AddControllers();
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.ConfigureSwagger();
            builder.Services.AddGrpcClients(builder.Configuration);
            builder.Services.AddCorsPolicy();
            builder.Host.UseLogging();

            var app = builder.Build();
            app.Services.MigrateDatabase<MusicDbContext>();

            app.UseExceptionHandler(options => { });
            app.UseMiddleware<LoggingMiddleware>();

            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();
            app.UseAuthentication();
            app.UseAuthorization();
            app.MapControllers();
            app.MapGrpcServices();
            app.UseCors();
            app.Run();
        }
    }
}
