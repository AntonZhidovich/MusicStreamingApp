using Identity.API.GrpcServices;

namespace Identity.API.Extensions
{
    public static class GrpcExtensions
    {
        public static IEndpointRouteBuilder MapGrpcServices(this IEndpointRouteBuilder app)
        {
            app.MapGrpcService<UserGrpcService>();

            return app;
        }
    }
}
