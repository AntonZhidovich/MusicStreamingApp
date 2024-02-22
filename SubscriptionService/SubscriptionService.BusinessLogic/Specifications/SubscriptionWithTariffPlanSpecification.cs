using SubscriptionService.DataAccess.Entities;
using SubscriptionService.DataAccess.Specifications;
using System.Linq.Expressions;

namespace SubscriptionService.BusinessLogic.Specifications
{
    internal class SubscriptionWithTariffPlanSpecification
        : ISpecification<Subscription>
    {
        public Expression<Func<Subscription, bool>> Criteria { get; }

        public List<Expression<Func<Subscription, object>>> Includes { get; }

        public Expression<Func<Subscription, object>>? OrderBy { get; }  = null;

        public Expression<Func<Subscription, object>>? OrderByDescending { get; }

        public SubscriptionWithTariffPlanSpecification(string tariffPlanName)
        {
            Criteria = subscription => subscription.TariffPlan.Name.Trim().ToLower() == tariffPlanName.Trim().ToLower();

            //Criteria = subscription => true;

            Includes = new List<Expression<Func<Subscription, object>>>
            {
                subscription => subscription.TariffPlan
            };

            OrderByDescending = subscription => subscription.SubscribedAt;
        }

        public bool IsSatisfiedBy(Subscription entity)
        {
            return Criteria.Compile()(entity);
        }
    }
}
