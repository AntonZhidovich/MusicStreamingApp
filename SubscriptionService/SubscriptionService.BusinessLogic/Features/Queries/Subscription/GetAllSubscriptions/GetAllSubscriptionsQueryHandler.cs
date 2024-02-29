using AutoMapper;
using MediatR;
using SubscriptionService.BusinessLogic.Extensions;
using SubscriptionService.BusinessLogic.Features.Services.Interfaces;
using SubscriptionService.BusinessLogic.Models;
using SubscriptionService.BusinessLogic.Models.Subscription;
using SubscriptionService.DataAccess.Entities;
using SubscriptionService.DataAccess.Repositories.Interfaces;

namespace SubscriptionService.BusinessLogic.Features.Queries.GetAllSubscriptions
{
    public class GetAllSubscriptionsQueryHandler
        : IRequestHandler<GetAllSubscriptionsQuery, PageResponse<SubscriptionWithUserNameDto>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IUserServiceGrpcClient _userServiceClient;

        public GetAllSubscriptionsQueryHandler(IUnitOfWork unitOfWork, IMapper mapper, IUserServiceGrpcClient userServiceClient)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _userServiceClient = userServiceClient;
        }

        public async Task<PageResponse<SubscriptionWithUserNameDto>> Handle(GetAllSubscriptionsQuery request,
            CancellationToken cancellationToken)
        {
            var subscriptions = await _unitOfWork.Subscriptions.GetAllAsync(
                request.GetPageRequest.CurrentPage,
                request.GetPageRequest.PageSize,
                cancellationToken);

            var count = await _unitOfWork.Subscriptions.CountAsync(cancellationToken);

            var pageResponse = subscriptions.GetPageResponse<Subscription, SubscriptionWithUserNameDto>(count, request.GetPageRequest, _mapper);

            var usernamesById = (await _userServiceClient.GetIdUserNameMap(subscriptions.Select(subscription => subscription.UserId),
                cancellationToken: cancellationToken)).UsernamesById;

            foreach (var subscription in pageResponse.Items)
            {
                subscription.UserName = usernamesById[subscription.UserId];
            }

            return pageResponse;
        }
    }
}
