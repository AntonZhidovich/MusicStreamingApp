using Identity.DataAccess.Entities;
using Identity.DataAccess.Specifications;
using System.Linq.Expressions;

namespace Identity.BusinessLogic.Specifications
{
    internal class UsersFromRegionSpecification : ISpecification<User>
    {
        public Expression<Func<User, bool>> Criteria { get; }
        
        public UsersFromRegionSpecification(string region) 
        {
            Criteria = user => user.Region.Trim().ToLower() == region.Trim().ToLower();
        }

        public bool IsSatisfiedBy(User user)
        {
            return Criteria.Compile()(user);
        }
    }
}
