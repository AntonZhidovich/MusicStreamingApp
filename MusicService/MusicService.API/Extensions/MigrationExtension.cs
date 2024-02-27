using Microsoft.EntityFrameworkCore;

namespace MusicService.API.Extensions
{
    public static class MigrationExtension
    {
        public static IServiceProvider MigrateDatabase<T>(this IServiceProvider provider)
            where T : DbContext
        {
            using var scope = provider.CreateScope();

            var dbContext = scope.ServiceProvider.GetService<T>()!;
            dbContext.Database.Migrate();

            return provider;
        }
    }
}
