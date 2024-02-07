using Microsoft.EntityFrameworkCore;
using MusicService.Application.Interfaces;
using MusicService.Application.Mapping;
using MusicService.Application.Services;
using MusicService.Domain.Interfaces;
using MusicService.Infrastructure.Data;
using MusicService.Infrastructure.Repositories;

namespace MusicService.API
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            builder.Services.AddDbContext<MusicDbContext>(options =>
            options.UseSqlServer(builder.Configuration.GetConnectionString("sqlserver")));
            builder.Services.AddScoped<IAuthorRepository, AuthorRepository>();
            builder.Services.AddScoped<IUserRepository, UserRepository>();
            builder.Services.AddAutoMapper(typeof(MappingProfile));
            builder.Services.AddScoped<IAuthorService, AuthorService>();
            builder.Services.AddControllers();
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            var app = builder.Build();

            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();
            app.UseAuthorization();
            app.MapControllers();
            app.Run();
        }
    }
}
