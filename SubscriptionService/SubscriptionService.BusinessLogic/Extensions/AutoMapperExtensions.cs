using AutoMapper;

namespace SubscriptionService.BusinessLogic.Extensions
{
    internal static class AutoMapperExtensions
    {
        public static void IgnoreDefaultValues<TSource, TDestination>(this IMemberConfigurationExpression<TSource, TDestination, object> opt)
        {
            opt.Condition((src, dest, member) =>
            {
                var memberType = member?.GetType() ?? typeof(object);
                var defaultValue = memberType.IsValueType ? Activator.CreateInstance(memberType) : null;

                return !Equals(member, defaultValue);
            });
        }
    }
}
