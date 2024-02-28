using AutoMapper;
using SubscriptionService.BusinessLogic.Extensions;
using SubscriptionService.BusinessLogic.Models.TariffPlan;
using SubscriptionService.DataAccess.Entities;

namespace SubscriptionService.BusinessLogic.Mapping
{
    public class TariffPlanMappingProfile : Profile
    {
        public TariffPlanMappingProfile()
        {
            CreateMap<TariffPlan, GetTariffPlanDto>();

            CreateMap<CreateTariffPlanDto, TariffPlan>();

            CreateMap<UpdateTariffPlanDto, TariffPlan>()
                .ForAllMembers(options => options.IgnoreDefaultValues());
        }
    }
}
