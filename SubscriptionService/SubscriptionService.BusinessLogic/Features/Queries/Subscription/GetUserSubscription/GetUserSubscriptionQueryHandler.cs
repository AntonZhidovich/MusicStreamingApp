using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using SubscriptionService.BusinessLogic.Constants;
using SubscriptionService.BusinessLogic.Exceptions;
using SubscriptionService.BusinessLogic.Models.Subscription;
using SubscriptionService.DataAccess.Repositories.Interfaces;

namespace SubscriptionService.BusinessLogic.Features.Queries.GetUserSubscription
{
    public class GetUserSubscriptionQueryHandler
        : IRequestHandler<GetUserSubscriptionQuery, GetSubscriptionDto>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ILogger<GetUserSubscriptionQueryHandler> _logger;

        public GetUserSubscriptionQueryHandler(IUnitOfWork unitOfWork, IMapper mapper, ILogger<GetUserSubscriptionQueryHandler> logger)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<GetSubscriptionDto> Handle(GetUserSubscriptionQuery request, CancellationToken cancellationToken)
        {
            var subscription = await _unitOfWork.Subscriptions.GetByUserIdAsync(request.UserId, cancellationToken);

            if (subscription == null)
            {
                _logger.LogError("Subscripion for user with id {UserId} not found.", request.UserId);

                throw new NotFoundException(ExceptionMessages.subscriptionNotFound);
            }

            return _mapper.Map<GetSubscriptionDto>(subscription);
        }
    }
}
