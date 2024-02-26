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

            query = query.Where(specification.Criteria);

            if (specification.OrderBy != null)
            {
                query = query.OrderBy(specification.OrderBy);
            }
            else if (specification.OrderByDescending != null)
            {
                query = query.OrderByDescending(specification.OrderByDescending);
            }

            return query;
        }
    }
}
