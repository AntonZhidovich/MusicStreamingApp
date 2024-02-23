using AutoMapper;
using MediatR;
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

        public MakeSubscriptionPaymentCommandHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<GetSubscriptionDto> Handle(MakeSubscriptionPaymentCommand request,
            CancellationToken cancellationToken)
        {
            var subscription = await _unitOfWork.Subscriptions.GetByIdAsync(
                request.Id,
                cancellationToken);

            if (subscription == null)
            {
                throw new NotFoundException(ExceptionMessages.SubscriptionNotFound);
            }

            switch (subscription.Type)
            {
                case SubscriptionTypes.month:
                    subscription.NextFeeDate.AddMonths(1);
                    break;
                case SubscriptionTypes.annual:
                    subscription.NextFeeDate.AddYears(1);
                    break;
                default:
                    throw new BadRequestException(ExceptionMessages.IncorrctSubscriptionType);
            }

            _unitOfWork.Subscriptions.Update(subscription);

            await _unitOfWork.CommitAsync(cancellationToken);

            return _mapper.Map<GetSubscriptionDto>(subscription);
        }
    }
}
