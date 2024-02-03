﻿using Identity.DataAccess.Specifications;
using System.Linq.Expressions;

namespace Identity.BusinessLogic.Specifications
{
    internal class OrSpecification<T> : ISpecification<T>
    {
        public Expression<Func<T, bool>> Criteria { get; }

        public OrSpecification(ISpecification<T> left, ISpecification<T> right)
        {
            var parameter = Expression.Parameter(typeof(T));
            var binary = Expression.OrElse(
                Expression.Invoke(left.Criteria, parameter),
                Expression.Invoke(right.Criteria, parameter));
            Criteria = Expression.Lambda<Func<T, bool>>(binary, parameter);
        }

        public bool IsSatisfiedBy(T entity)
        {
            return Criteria.Compile()(entity);
        }
    }
}
