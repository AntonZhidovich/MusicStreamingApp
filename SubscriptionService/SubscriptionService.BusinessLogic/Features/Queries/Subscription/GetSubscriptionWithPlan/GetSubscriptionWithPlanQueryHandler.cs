using AutoMapper;
using MediatR;
using SubscriptionService.BusinessLogic.Extensions;
using SubscriptionService.BusinessLogic.Models;
using SubscriptionService.BusinessLogic.Models.Subscription;
using SubscriptionService.BusinessLogic.Specifications;
using SubscriptionService.DataAccess.Entities;
using SubscriptionService.DataAccess.Repositories.Interfaces;

namespace SubscriptionService.BusinessLogic.Features.Queries.GetSubscriptionWithPlan
{
    public class GetSubscriptionWithPlanQueryHandler
        : IRequestHandler<GetSubscriptionWithPlanQuery, PageResponse<GetSubscriptionDto>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public GetSubscriptionWithPlanQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<PageResponse<GetSubscriptionDto>> Handle(GetSubscriptionWithPlanQuery request,
            CancellationToken cancellationToken)
        {
            var specification = new SubscriptionWithTariffPlanSpecification(request.TariffPlanName);

            var subscriptions = await _unitOfWork.Subscriptions.ApplySpecificationAsync(specification,
                request.PageRequest.CurrentPage,
                request.PageRequest.PageSize,
                cancellationToken);

            var count = await _unitOfWork.Subscriptions.CountAsync(specification);

            return subscriptions.GetPageResponse<Subscription, GetSubscriptionDto>(count, request.PageRequest, _mapper);
        }
    }
}
