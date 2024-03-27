using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using SubscriptionService.BusinessLogic.Constants;
using SubscriptionService.BusinessLogic.Exceptions;
using SubscriptionService.BusinessLogic.Models.Subscription;
using SubscriptionService.DataAccess.Constants;
using SubscriptionService.DataAccess.Repositories.Interfaces;

namespace SubscriptionService.BusinessLogic.Features.Commands.MakeSubscriptionPayment
{
    public class MakeSubscriptionPaymentCommandHandler
        : IRequestHandler<MakeSubscriptionPaymentCommand, GetSubscriptionDto>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ILogger<MakeSubscriptionPaymentCommandHandler> _logger;

        public MakeSubscriptionPaymentCommandHandler(IUnitOfWork unitOfWork, IMapper mapper, ILogger<MakeSubscriptionPaymentCommandHandler> logger)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<GetSubscriptionDto> Handle(MakeSubscriptionPaymentCommand request,
            CancellationToken cancellationToken)
        {
            var subscription = await _unitOfWork.Subscriptions.GetByIdAsync(
                request.Id,
                cancellationToken);

            if (subscription == null)
            {
                _logger.LogError("Subscription with id {Id} not found.", request.Id);

                throw new NotFoundException(ExceptionMessages.subscriptionNotFound);
            }

            switch (subscription.Type)
            {
                case SubscriptionTypes.month:
                    subscription.NextFeeDate = subscription.NextFeeDate.AddMonths(1);
                    break;
                case SubscriptionTypes.annual:
                    subscription.NextFeeDate = subscription.NextFeeDate.AddYears(1);
                    break;
                default:
                    throw new BadRequestException(ExceptionMessages.incorrctSubscriptionType);
            }

            _unitOfWork.Subscriptions.Update(subscription);

            await _unitOfWork.CommitAsync(cancellationToken);

            _logger.LogInformation("Payment for subscription {Id} is made.", subscription.Id);

            return _mapper.Map<GetSubscriptionDto>(subscription);
        }
    }
}
