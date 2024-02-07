using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using MusicService.Application.Interfaces;
using MusicService.Application.Mapping;
using MusicService.Application.Services;
using MusicService.Domain.Interfaces;
using MusicService.Infrastructure.Data;
using MusicService.Infrastructure.Repositories;
using System.Reflection;
using System.Text;

namespace MusicService.API
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            builder.Services.AddDatabases(builder.Configuration);
            builder.Services.AddRepositories();
            builder.Services.AddServices(builder.Configuration);
            builder.Services.AddAutoMapper(typeof(MappingProfile));
            builder.Services.AddControllers();
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.ConfigureSwagger();

            var app = builder.Build();

            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();
            app.UseAuthentication();
            app.UseAuthorization();
            app.MapControllers();
            app.Run();
        }
    }
}
