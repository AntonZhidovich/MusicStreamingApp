using AutoMapper;
using MediatR;
using SubscriptionService.BusinessLogic.Extensions;
using SubscriptionService.BusinessLogic.Features.Services.Interfaces;
using SubscriptionService.BusinessLogic.Models;
using SubscriptionService.BusinessLogic.Models.Subscription;
using SubscriptionService.BusinessLogic.Specifications;
using SubscriptionService.DataAccess.Entities;
using SubscriptionService.DataAccess.Repositories.Interfaces;

namespace SubscriptionService.BusinessLogic.Features.Queries.GetSubscriptionWithPlan
{
    public class GetSubscriptionWithPlanQueryHandler
        : IRequestHandler<GetSubscriptionWithPlanQuery, PageResponse<SubscriptionWithUserNameDto>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IUserServiceGrpcClient _userServiceClient;

        public GetSubscriptionWithPlanQueryHandler(IUnitOfWork unitOfWork, IMapper mapper, IUserServiceGrpcClient userServiceClient)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _userServiceClient = userServiceClient;
        }

        public async Task<PageResponse<SubscriptionWithUserNameDto>> Handle(GetSubscriptionWithPlanQuery request,
            CancellationToken cancellationToken)
        {
            var specification = new SubscriptionWithTariffPlanSpecification(request.TariffPlanName);

            var subscriptions = await _unitOfWork.Subscriptions.ApplySpecificationAsync(specification,
                request.PageRequest.CurrentPage,
                request.PageRequest.PageSize,
                cancellationToken);

            var count = await _unitOfWork.Subscriptions.CountAsync(specification, cancellationToken);

            var pageResponse = subscriptions.GetPageResponse<Subscription, SubscriptionWithUserNameDto>(count, request.PageRequest, _mapper);

            var users = await _userServiceClient.GetUsersInfoAsync(subscriptions.Select(subscription => subscription.UserId),
                cancellationToken);

            foreach (var subscription in pageResponse.Items)
            {
                var user = users.FirstOrDefault(user => user.Id == subscription.UserId)!;
                subscription.UserName = user.UserName;
            }

            return pageResponse;
        }
    }
}
