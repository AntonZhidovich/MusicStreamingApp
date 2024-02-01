using Identity.API.ExceptionHandlers;
using Identity.BusinessLogic.Mapping;
using Identity.BusinessLogic.Options;
using Identity.BusinessLogic.Services;
using Identity.DataAccess.Data;
using Identity.DataAccess.Entities;
using Identity.DataAccess.Repositories;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Identity.API
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.Configure<JwtOptions>(builder.Configuration.GetSection("JwtOptions"));
            builder.Services.Configure<IdentityOptions>(builder.Configuration.GetSection("IdentityOptions"));

            builder.Services.AddExceptionHandler<GlobalExceptionHandler>();
            builder.Services.AddControllers();
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();
            builder.Services.AddDbContext<UserDBContext>(
                options => options.UseSqlServer(builder.Configuration.GetConnectionString("SQLServer")));
            builder.Services.AddIdentity<User, IdentityRole>()
                .AddEntityFrameworkStores<UserDBContext>();
            builder.Services.AddAutoMapper(typeof(UserMappingProfile));
            builder.Services.AddScoped<ITokenRepository, TokenRepository>();
            builder.Services.AddScoped<IUserService, UserService>();
            builder.Services.AddScoped<ITokenService, TokenService>();
            builder.Services.AddScoped<ISignInService, SignInService>();

            var app = builder.Build();
            app.UseExceptionHandler(options => { });
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
