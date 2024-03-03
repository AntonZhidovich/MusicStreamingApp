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
            CreateMap<TariffPlan, GetTariffPlanDto>()
                .ForMember(dest => dest.Id, options => options.MapFrom(source => source.Id))
                .ForMember(dest => dest.Name, options => options.MapFrom(source => source.Name))
                .ForMember(dest => dest.Description, options => options.MapFrom(source => source.Description))
                .ForMember(dest => dest.MaxPlaylistsCount, options => options.MapFrom(source => source.MaxPlaylistsCount))
                .ForMember(dest => dest.MonthFee, options => options.MapFrom(source => source.MonthFee))
                .ForMember(dest => dest.AnnualFee, options => options.MapFrom(source => source.AnnualFee));

            CreateMap<CreateTariffPlanDto, TariffPlan>()
                .ForMember(dest => dest.Name, options => options.MapFrom(source => source.Name))
                .ForMember(dest => dest.Description, options => options.MapFrom(source => source.Description))
                .ForMember(dest => dest.MaxPlaylistsCount, options => options.MapFrom(source => source.MaxPlaylistsCount))
                .ForMember(dest => dest.MonthFee, options => options.MapFrom(source => source.MonthFee))
                .ForMember(dest => dest.AnnualFee, options => options.MapFrom(source => source.AnnualFee));


            CreateMap<UpdateTariffPlanDto, TariffPlan>()
                .ForMember(dest => dest.Name, options => options.MapFrom(source => source.Name))
                .ForMember(dest => dest.Description, options => options.MapFrom(source => source.Description))
                .ForMember(dest => dest.MaxPlaylistsCount, options => options.MapFrom(source => source.MaxPlaylistsCount))
                .ForMember(dest => dest.MonthFee, options => options.MapFrom(source => source.MonthFee))
                .ForMember(dest => dest.AnnualFee, options => options.MapFrom(source => source.AnnualFee))
                .ForAllMembers(options => options.IgnoreDefaultValues());
        }
    }
}
