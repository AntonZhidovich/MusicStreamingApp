using AutoMapper;
using MediatR;
using SubscriptionService.BusinessLogic.Constants;
using SubscriptionService.BusinessLogic.Exceptions;
using SubscriptionService.BusinessLogic.Models.Subscription;
using SubscriptionService.DataAccess.Repositories.Interfaces;

namespace SubscriptionService.BusinessLogic.Features.Commands.UpdateSubscription
{
    public class UpdateSubscriptionCommandHandler
        : IRequestHandler<UpdateSubscriptionCommand, GetSubscriptionDto>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public UpdateSubscriptionCommandHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<GetSubscriptionDto> Handle(UpdateSubscriptionCommand request, CancellationToken cancellationToken)
        {
            var subscription = await _unitOfWork.Subscriptions.GetByIdAsync(
                request.Id,
                cancellationToken);

            if (subscription == null)
            {
                throw new NotFoundException(ExceptionMessages.SubscriptionNotFound);
            }

            _mapper.Map(request.Dto, subscription);
            _unitOfWork.Subscriptions.Update(subscription);

            await _unitOfWork.CommitAsync(cancellationToken);

            return _mapper.Map<GetSubscriptionDto>(subscription);
        }
    }
}
