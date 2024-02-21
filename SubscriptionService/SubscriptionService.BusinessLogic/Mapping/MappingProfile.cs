using AutoMapper;
using SubscriptionService.BusinessLogic.Extensions;
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
        }
    }
}