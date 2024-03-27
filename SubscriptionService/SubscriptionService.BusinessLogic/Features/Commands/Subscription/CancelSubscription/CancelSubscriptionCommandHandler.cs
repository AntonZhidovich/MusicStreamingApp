using AutoMapper;
using Hangfire;
using MediatR;
using Microsoft.Extensions.Logging;
using SubscriptionService.BusinessLogic.Constants;
using SubscriptionService.BusinessLogic.Exceptions;
using SubscriptionService.BusinessLogic.Features.Producers;
using SubscriptionService.BusinessLogic.Features.Services.Interfaces;
using SubscriptionService.BusinessLogic.Models.Messages;
using SubscriptionService.BusinessLogic.Models.Subscription;
using SubscriptionService.DataAccess.Repositories.Interfaces;

namespace SubscriptionService.BusinessLogic.Features.Commands.CancelSubscription
{
    public class CancelSubscriptionCommandHandler : IRequestHandler<CancelSubscriptionCommand>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IProducerService _producerService;
        private readonly IUserServiceGrpcClient _userServiceClient;
        private readonly IEmailSenderService _emailSender;
        private readonly ILogger<CancelSubscriptionCommandHandler> _logger;

        public CancelSubscriptionCommandHandler(
            IUnitOfWork unitOfWork, 
            IMapper mapper, 
            IProducerService producerService,
            IUserServiceGrpcClient userServiceClient,
            IEmailSenderService emailSender,
            ILogger<CancelSubscriptionCommandHandler> logger)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _producerService = producerService;
            _userServiceClient = userServiceClient;
            _emailSender = emailSender;
            _logger = logger;
        }

        public async Task Handle(CancelSubscriptionCommand request, CancellationToken cancellationToken)
        {
            var subscription = await _unitOfWork.Subscriptions.GetByIdAsync(
                request.SubscriptionId,
                cancellationToken);

            if (subscription == null)
            {
                _logger.LogError("Subscription with {SubscriptionId} not found.", request.SubscriptionId);

                throw new NotFoundException(ExceptionMessages.subscriptionNotFound);
            }

            _unitOfWork.Subscriptions.Delete(subscription);

            await _unitOfWork.CommitAsync(cancellationToken);

            await _producerService.ProduceSubscriptionCanceledAsync(_mapper.Map<SubscriptionCanceledMessage>(subscription));

            RecurringJob.RemoveIfExists(subscription.Id);

            var userInfo = await _userServiceClient.GetUserInfoAsync(subscription.UserId);

            if (userInfo != null)
            {
                var subscriptionDto = _mapper.Map<GetSubscriptionDto>(subscription);

                BackgroundJob.Enqueue(() => _emailSender.SendSubscriptionCanceledMessage(
                    new SubscriptionWithUserInfo
                    {
                        UserInfo = userInfo,
                        Subscription = subscriptionDto
                    }));
            }

            _logger.LogInformation("Subscription with {SubscriptionId} is canceled.", request.SubscriptionId);
        }
    }
}
