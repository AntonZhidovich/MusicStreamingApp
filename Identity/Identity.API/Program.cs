using Identity.API.Extensions;

namespace Identity.API
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            builder.Services.ApplyConfigurations(builder.Configuration);
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddJwtAuthentication(builder.Configuration);
            builder.Services.AddDataBase(builder.Configuration);
            builder.Services.AddRepositories();
            builder.Services.AddControllers();
            builder.Services.AddServices();
            builder.Services.ConfigureSwagger();

            var app = builder.Build();
            app.UseMiddleware();

            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();
            app.UseAuthentication();
            app.UseAuthorization();
            app.MapGrpcServices();
            app.MapControllers();
            app.Run();
        }
    }
}
