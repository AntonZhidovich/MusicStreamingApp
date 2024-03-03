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
        : IRequestHandler<GetSubscriptionWithPlanQuery, PageResponse<GetSubscriptionWithUserNameDto>>
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

        public async Task<PageResponse<GetSubscriptionWithUserNameDto>> Handle(GetSubscriptionWithPlanQuery request,
            CancellationToken cancellationToken)
        {
            var specification = new SubscriptionWithTariffPlanSpecification(request.TariffPlanName);

            var subscriptions = await _unitOfWork.Subscriptions.ApplySpecificationAsync(specification,
                request.PageRequest.CurrentPage,
                request.PageRequest.PageSize,
                cancellationToken);

            var count = await _unitOfWork.Subscriptions.CountAsync(specification, cancellationToken);

            var pageResponse = subscriptions.GetPageResponse<Subscription, GetSubscriptionWithUserNameDto>(count, request.PageRequest, _mapper);

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
