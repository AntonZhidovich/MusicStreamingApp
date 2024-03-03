namespace MusicService.Infrastructure.Extensions
{
    public static class PaginationExtension
    {
        public static IQueryable<T> GetPage<T>(this IQueryable<T> source, int currentPage, int pageSize)
        {
            return source
                .Skip(pageSize * (currentPage - 1))
                .Take(pageSize);
        }
    }
}
