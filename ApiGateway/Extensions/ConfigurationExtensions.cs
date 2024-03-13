using Ocelot.DependencyInjection;

namespace ApiGateway.Extensions
{
    public static class ConfigurationExtensions
    {
        public static IServiceCollection ConfigureOcelot(
            this IServiceCollection services, 
            IConfigurationManager configuration,
            IWebHostEnvironment environment) 
        {
            configuration
                .AddJsonFile("ocelot.json", optional: false, reloadOnChange: true)
                .AddJsonFile($"ocelot.{environment.EnvironmentName}.json", optional: true, reloadOnChange: true);

            services.AddOcelot(configuration);
            services.AddSwaggerForOcelot(configuration);

            return services;
        }

        public static IApplicationBuilder UseDownstreamSwagger(this WebApplication app)
        {
            if (app.Environment.IsDevelopment())
            {
                app.UseSwaggerForOcelotUI(options =>
                {
                    options.PathToSwaggerGenerator = "/swagger/docs";
                });
            }

            return app;
        }
    }
}
