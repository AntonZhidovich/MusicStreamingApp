using Serilog;
using Serilog.Sinks.Elasticsearch;

namespace Identity.API.Extensions
{
    public static class LoggerExtension
    {
        public static IHostBuilder UseLogging(this IHostBuilder host)
        {
            host.UseSerilog((context, configuration) =>
            {
                var application = context.HostingEnvironment.ApplicationName.FormatDashedName();
                var environment = context.HostingEnvironment.EnvironmentName.FormatDashedName();

                configuration.Enrich.FromLogContext()
                .Enrich.WithMachineName()
                .Enrich.WithEnvironmentName()
                .Enrich.WithProperty(nameof(context.HostingEnvironment.ApplicationName), application)
                .WriteTo.Console()
                .WriteTo.Elasticsearch(new ElasticsearchSinkOptions(new Uri(context.Configuration["ElasticsearchConfig:Uri"]!))
                {
                    IndexFormat = $"{application}-logs-{environment}-{DateTime.UtcNow:dd-MM-yyyy}"
                })
                .ReadFrom.Configuration(context.Configuration);

            });

            return host;
        }

        private static string FormatDashedName(this string str)
        {
            return str.Trim().ToLower().Replace('.', '-');
        }
    }
}
