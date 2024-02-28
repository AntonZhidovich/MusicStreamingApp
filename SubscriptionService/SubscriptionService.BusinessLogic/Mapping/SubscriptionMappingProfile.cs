using AutoMapper;
using SubscriptionService.BusinessLogic.Extensions;
using SubscriptionService.BusinessLogic.Models.Subscription;
using SubscriptionService.BusinessLogic.Models.TariffPlan;
using SubscriptionService.DataAccess.Entities;

namespace SubscriptionService.BusinessLogic.Mapping
{
    public class SubscriptionMappingProfile : Profile
    {
        public SubscriptionMappingProfile()
        {
            CreateMap<Subscription, GetSubscriptionDto>()
                .ForMember(dest => dest.TariffPlanName, options => options.MapFrom(src => src.TariffPlan.Name));

            CreateMap<CreateSubscriptionDto, Subscription>();

            CreateMap<UpdateSubscriptionDto, Subscription>()
                .ForAllMembers(options => options.IgnoreDefaultValues());
        }
    }
}