using Identity.DataAccess.Entities;

namespace Identity.DataAccess.Specifications
{
    public static class SpecificationExtension
    {
        public static IQueryable<User> ApplySpecification(this IQueryable<User> query, ISpecification<User> specification)
        {
            return query.Where(specification.Criteria);
        }
    }
}
