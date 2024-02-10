using Microsoft.EntityFrameworkCore;
using MusicService.Domain.Interfaces;

namespace MusicService.Infrastructure.Extensions
{
    public static class SpecificationExtension
    {
        public static IQueryable<T> ApplySpecification<T>(this IQueryable<T> query, ISpecification<T> specification)
            where T : class
        {
            query = specification.Includes.Aggregate(query, (current, navigation)  => current.Include(navigation));

            return query.Where(specification.Criteria)
                .OrderBy((specification.OrderBy ?? specification.OrderByDescending)!);
        }
    }
}
