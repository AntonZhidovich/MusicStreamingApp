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
            builder.Services.AddSwaggerGen();
            builder.Services.AddDataBase(builder.Configuration);
            builder.Services.AddRepositories();
            builder.Services.AddControllers();
            builder.Services.AddServices();

            var app = builder.Build();
            app.UseMiddleware();
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
