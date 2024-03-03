namespace SubscriptionService.DataAccess.Extensions
{
    internal static class PaginationExtension
    {
        public static IQueryable<T> GetPage<T>(this IQueryable<T> source, int currentPage, int pageSize)
        {
            return source
                .Skip((currentPage - 1) * pageSize)
                .Take(pageSize);
        }
    }
}
