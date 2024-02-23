using AutoMapper;
using MediatR;
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

        public GetUserSubscriptionQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<GetSubscriptionDto> Handle(GetUserSubscriptionQuery request, CancellationToken cancellationToken)
        {
            var subscription = await _unitOfWork.Subscriptions.GetByUserNameAsync(request.UserName, cancellationToken);

            if (subscription == null)
            {
                throw new NotFoundException(ExceptionMessages.subscriptionNotFound);
            }

            return _mapper.Map<GetSubscriptionDto>(subscription);
        }
    }
}
