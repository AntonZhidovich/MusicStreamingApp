using ApiGateway.Extensions;
using Ocelot.Middleware;

namespace ApiGateway
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddControllers();
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();
            builder.Services.AddJwtAuthentication(builder.Configuration);
            builder.Services.ConfigureOcelot(builder.Configuration, builder.Environment);

            var app = builder.Build();

            app.UseHttpsRedirection();
            app.UseAuthentication();
            app.UseAuthorization();
            app.UseDownstreamSwagger();
            app.UseOcelot();

            app.Run();
        }
    }
}
