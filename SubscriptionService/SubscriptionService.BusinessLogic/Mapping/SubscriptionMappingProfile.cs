using AutoMapper;
using SubscriptionService.BusinessLogic.Extensions;
using SubscriptionService.BusinessLogic.Models.Messages;
using SubscriptionService.BusinessLogic.Models.Subscription;
using SubscriptionService.DataAccess.Entities;

namespace SubscriptionService.BusinessLogic.Mapping
{
    public class SubscriptionMappingProfile : Profile
    {
        public SubscriptionMappingProfile()
        {
            CreateMap<Subscription, GetSubscriptionDto>()
                .ForMember(dest => dest.Id, options => options.MapFrom(source => source.Id))
                .ForMember(dest => dest.UserId, options => options.MapFrom(source => source.UserId))
                .ForMember(dest => dest.TariffPlanName, options => options.MapFrom(src => src.TariffPlan.Name))
                .ForMember(dest => dest.SubscribedAt, options => options.MapFrom(source => source.SubscribedAt))
                .ForMember(dest => dest.NextFeeDate, options => options.MapFrom(source => source.NextFeeDate))
                .ForMember(dest => dest.Type, options => options.MapFrom(source => source.Type))
                .ForMember(dest => dest.Fee, options => options.MapFrom(source => source.Fee));

            CreateMap<CreateSubscriptionDto, Subscription>()
                .ForMember(dest => dest.UserId, options => options.MapFrom(source => source.UserId))
                .ForMember(dest => dest.Type, options => options.MapFrom(source => source.Type));

            CreateMap<UpdateSubscriptionDto, Subscription>()
                .ForMember(dest => dest.NextFeeDate, options => options.MapFrom(source => source.NextFeeDate))
                .ForMember(dest => dest.Fee, options => options.MapFrom(source => source.Fee))
                .ForAllMembers(options => options.IgnoreDefaultValues());

            CreateMap<Subscription, SubscriptionMadeMessage>()
                .ForMember(dest => dest.UserId, options => options.MapFrom(src => src.UserId))
                .ForMember(dest => dest.MaxPlaylistCount, options => options.MapFrom(src => src.TariffPlan.MaxPlaylistsCount));

            CreateMap<Subscription, SubscriptionCanceledMessage>()
                .ForMember(dest => dest.UserId, options => options.MapFrom(src => src.UserId));

            CreateMap<Subscription, GetSubscriptionWithUserNameDto>()
                .ForMember(dest => dest.Id, options => options.MapFrom(source => source.Id))
                .ForMember(dest => dest.UserId, options => options.MapFrom(source => source.UserId))
                .ForMember(dest => dest.TariffPlanName, options => options.MapFrom(src => src.TariffPlan.Name))
                .ForMember(dest => dest.SubscribedAt, options => options.MapFrom(source => source.SubscribedAt))
                .ForMember(dest => dest.NextFeeDate, options => options.MapFrom(source => source.NextFeeDate))
                .ForMember(dest => dest.Type, options => options.MapFrom(source => source.Type))
                .ForMember(dest => dest.Fee, options => options.MapFrom(source => source.Fee));
        }
    }
}