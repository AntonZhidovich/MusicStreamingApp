using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using SubscriptionService.BusinessLogic.Extensions;
using SubscriptionService.BusinessLogic.Features.Services.Interfaces;
using SubscriptionService.BusinessLogic.Models;
using SubscriptionService.BusinessLogic.Models.Subscription;
using SubscriptionService.DataAccess.Entities;
using SubscriptionService.DataAccess.Repositories.Interfaces;

namespace SubscriptionService.BusinessLogic.Features.Queries.GetAllSubscriptions
{
    public class GetAllSubscriptionsQueryHandler
        : IRequestHandler<GetAllSubscriptionsQuery, PageResponse<GetSubscriptionWithUserNameDto>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IUserServiceGrpcClient _userServiceClient;
        private readonly ILogger<GetAllSubscriptionsQueryHandler> _logger;

        public GetAllSubscriptionsQueryHandler(IUnitOfWork unitOfWork, IMapper mapper, IUserServiceGrpcClient userServiceClient, ILogger<GetAllSubscriptionsQueryHandler> logger)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _userServiceClient = userServiceClient;
            _logger = logger;
        }

        public async Task<PageResponse<GetSubscriptionWithUserNameDto>> Handle(GetAllSubscriptionsQuery request,
            CancellationToken cancellationToken)
        {
            var subscriptions = await _unitOfWork.Subscriptions.GetAllAsync(
                request.GetPageRequest.CurrentPage,
                request.GetPageRequest.PageSize,
                cancellationToken);

            var count = await _unitOfWork.Subscriptions.CountAsync(cancellationToken);

            var pageResponse = subscriptions.GetPageResponse<Subscription, GetSubscriptionWithUserNameDto>(count, request.GetPageRequest, _mapper);

            var usernamesById = (await _userServiceClient.GetIdUserNameMap(subscriptions.Select(subscription => subscription.UserId),
                cancellationToken: cancellationToken)).UsernamesById;

            _logger.LogInformation("Fetched id-username map from Identity.");

            foreach (var subscription in pageResponse.Items)
            {
                subscription.UserName = usernamesById[subscription.UserId];
            }

            return pageResponse;
        }
    }
}
