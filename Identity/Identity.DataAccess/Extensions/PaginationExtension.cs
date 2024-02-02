using Identity.DataAccess.Entities;

namespace Identity.DataAccess.Extensions
{
    internal static class PaginationExtension
    {
        public static IQueryable<User> GetPage(this IQueryable<User> source, int currentPage, int pageSize)
        {
            return source
                .OrderByDescending(x => x.UserName)
                .Skip(pageSize * (currentPage - 1))
                .Take(pageSize);
        }    
    }
}
