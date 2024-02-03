using System.Linq.Expressions;

namespace Identity.DataAccess.Specifications
{
    public interface ISpecification<T>
    {
        Expression<Func<T, bool>> Criteria { get; }
        bool IsSatisfiedBy(T entity);
    }
}
