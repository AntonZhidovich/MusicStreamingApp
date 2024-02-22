using AutoMapper;
using MediatR;
using SubscriptionService.BusinessLogic.Constants;
using SubscriptionService.BusinessLogic.Exceptions;
using SubscriptionService.DataAccess.Repositories.Interfaces;

namespace SubscriptionService.BusinessLogic.Commands.CancelSubscription
{
    public class CancelSubscriptionCommandHandler : IRequestHandler<CancelSubscriptionCommand>
    {
        private readonly IUnitOfWork _unitOfWork;

        public CancelSubscriptionCommandHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task Handle(CancelSubscriptionCommand request, CancellationToken cancellationToken)
        {
            var subscription = await _unitOfWork.Subscriptions.GetByIdAsync(
                request.SubscriptionId,
                cancellationToken);

            if (subscription == null)
            {
                throw new NotFoundException(ExceptionMessages.SubscriptionNotFound);
            }

            _unitOfWork.Subscriptions.Delete(subscription);

            await _unitOfWork.CommitAsync(cancellationToken);
        }
    }
}
