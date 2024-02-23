using AutoMapper;
using MediatR;
using SubscriptionService.BusinessLogic.Constants;
using SubscriptionService.BusinessLogic.Exceptions;
using SubscriptionService.BusinessLogic.Models.Subscription;
using SubscriptionService.DataAccess.Constants;
using SubscriptionService.DataAccess.Entities;
using SubscriptionService.DataAccess.Repositories.Interfaces;

namespace SubscriptionService.BusinessLogic.Features.Commands.MakeSubscription
{
    public class MakeSubscriptionCommandHandler
        : IRequestHandler<MakeSubscriptionCommand, GetSubscriptionDto>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public MakeSubscriptionCommandHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<GetSubscriptionDto> Handle(MakeSubscriptionCommand request, CancellationToken cancellationToken)
        {
            var tariffPlan = await _unitOfWork.TariffPlans.GetByIdAsync(
                request.Dto.TariffPlanId,
                cancellationToken);

            if (tariffPlan == null)
            {
                throw new NotFoundException(ExceptionMessages.tariffPlanNotFound);
            }

            var currentSubscription = await _unitOfWork.Subscriptions.GetByUserNameAsync(
                request.Dto.UserName,
                cancellationToken);

            if (currentSubscription != null)
            {
                throw new BadRequestException(ExceptionMessages.subscriptionExists);
            }

            var subscription = _mapper.Map<Subscription>(request.Dto);
            subscription.Id = Guid.NewGuid().ToString();
            subscription.SubscribedAt = DateTime.UtcNow;
            subscription.TariffPlan = tariffPlan;

            switch (subscription.Type)
            {
                case SubscriptionTypes.month:
                    subscription.NextFeeDate = DateTime.UtcNow.AddMonths(1);
                    subscription.Fee = tariffPlan.MonthFee;
                    break;
                case SubscriptionTypes.annual:
                    subscription.NextFeeDate = DateTime.UtcNow.AddYears(1);
                    subscription.Fee = tariffPlan.AnnualFee;
                    break;
                default:
                    throw new UnprocessableEntityException(ExceptionMessages.incorrctSubscriptionType);
            }

            await _unitOfWork.Subscriptions.CreateAsync(subscription, cancellationToken);

            await _unitOfWork.CommitAsync(cancellationToken);

            return _mapper.Map<GetSubscriptionDto>(subscription);
        }
    }
}
