using Identity.BusinessLogic.Mapping;
using Identity.BusinessLogic.Options;
using Identity.BusinessLogic.Services;
using Identity.DataAccess.Data;
using Identity.DataAccess.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

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
            builder.Services.AddScoped<ISignInService, SignInService>();
            builder.Services.Configure<JwtOptions>(builder.Configuration.GetSection("JwtOptions"));
            builder.Services.AddScoped<ITokenService, TokenService>();

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
