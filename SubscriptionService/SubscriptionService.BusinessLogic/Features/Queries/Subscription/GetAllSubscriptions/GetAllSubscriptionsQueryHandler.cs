using AutoMapper;
using MediatR;
using SubscriptionService.BusinessLogic.Extensions;
using SubscriptionService.BusinessLogic.Models;
using SubscriptionService.BusinessLogic.Models.Subscription;
using SubscriptionService.DataAccess.Entities;
using SubscriptionService.DataAccess.Repositories.Interfaces;

namespace SubscriptionService.BusinessLogic.Features.Queries.GetAllSubscriptions
{
    public class GetAllSubscriptionsQueryHandler
        : IRequestHandler<GetAllSubscriptionsQuery, PageResponse<GetSubscriptionDto>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public GetAllSubscriptionsQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<PageResponse<GetSubscriptionDto>> Handle(GetAllSubscriptionsQuery request,
            CancellationToken cancellationToken)
        {
            var subscriptions = await _unitOfWork.Subscriptions.GetAllAsync(
                request.GetPageRequest.CurrentPage,
                request.GetPageRequest.PageSize,
                cancellationToken);

            var count = await _unitOfWork.Subscriptions.CountAsync(cancellationToken);

            return subscriptions.GetPageResponse<Subscription, GetSubscriptionDto>(count, request.GetPageRequest, _mapper);
        }
    }
}
