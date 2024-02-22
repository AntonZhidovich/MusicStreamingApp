using AutoMapper;
using SubscriptionService.BusinessLogic.Extensions;
using SubscriptionService.BusinessLogic.Models.Subscription;
using SubscriptionService.BusinessLogic.Models.TariffPlan;
using SubscriptionService.DataAccess.Entities;

namespace SubscriptionService.BusinessLogic.Mapping
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<TariffPlan, GetTariffPlanDto>();
            
            CreateMap<CreateTariffPlanDto, TariffPlan>();

            CreateMap<UpdateTariffPlanDto, TariffPlan>()
                .ForAllMembers(options => options.IgnoreDefaultValues());

            CreateMap<Subscription, GetSubscriptionDto>()
                .ForMember(dest => dest.TariffPlanName, options => options.MapFrom(src => src.TariffPlan.Name));

            CreateMap<CreateSubscriptionDto, Subscription>();

            CreateMap<UpdateSubscriptionDto, Subscription>()
                .ForAllMembers(options => options.IgnoreDefaultValues());
        }
    }
}