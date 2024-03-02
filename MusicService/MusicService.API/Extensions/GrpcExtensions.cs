using Identity.Grpc;
using MusicService.API.GrpcServices;

namespace MusicService.API.Extensions
{
    public static class GrpcExtensions
    {
        public static IEndpointRouteBuilder MapGrpcServices(this IEndpointRouteBuilder app)
        {
            app.MapGrpcService<MusicGrpcService>();

            return app;
        }
    }
}
