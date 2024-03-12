using AutoMapper;
using MediatR;
using SubscriptionService.BusinessLogic.Constants;
using SubscriptionService.BusinessLogic.Exceptions;
using SubscriptionService.BusinessLogic.Features.Producers;
using SubscriptionService.BusinessLogic.Models.Messages;
using SubscriptionService.DataAccess.Repositories.Interfaces;

namespace SubscriptionService.BusinessLogic.Features.Commands.CancelSubscription
{
    public class CancelSubscriptionCommandHandler : IRequestHandler<CancelSubscriptionCommand>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IProducerService _producerService;

        public CancelSubscriptionCommandHandler(IUnitOfWork unitOfWork, IMapper mapper, IProducerService producerService)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _producerService = producerService;
        }

        public async Task Handle(CancelSubscriptionCommand request, CancellationToken cancellationToken)
        {
            var subscription = await _unitOfWork.Subscriptions.GetByIdAsync(
                request.SubscriptionId,
                cancellationToken);

            if (subscription == null)
            {
                throw new NotFoundException(ExceptionMessages.subscriptionNotFound);
            }

            _unitOfWork.Subscriptions.Delete(subscription);

            await _unitOfWork.CommitAsync(cancellationToken);

            await _producerService.ProduceSubscriptionCanceledAsync(_mapper.Map<SubscriptionCanceledMessage>(subscription));
        }
    }
}
