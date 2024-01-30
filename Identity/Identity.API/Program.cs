using Identity.BusinessLogic.Mapping;
using Identity.BusinessLogic.Services;
using Identity.DataAccess.Data;
using Identity.DataAccess.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace Identity.API
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            builder.Services.AddControllers();
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();
            builder.Services.AddDbContext<UserDBContext>(
                options => options.UseSqlServer(builder.Configuration.GetConnectionString("SQLServer")));
            builder.Services.AddIdentity<User, IdentityRole>()
                .AddEntityFrameworkStores<UserDBContext>();
            builder.Services.AddAutoMapper(typeof(UserMappingProfile));
            builder.Services.AddScoped<IUserService, UserService>();

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
