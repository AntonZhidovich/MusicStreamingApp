using AutoMapper;
using Google.Protobuf.WellKnownTypes;
using Hangfire;
using MediatR;
using Microsoft.Extensions.Logging;
using SubscriptionService.BusinessLogic.Constants;
using SubscriptionService.BusinessLogic.Exceptions;
using SubscriptionService.BusinessLogic.Features.Producers;
using SubscriptionService.BusinessLogic.Features.Services.Interfaces;
using SubscriptionService.BusinessLogic.Models.Messages;
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
        private readonly IProducerService _producerService;
        private readonly IUserServiceGrpcClient _userServiceClient;
        private readonly IEmailSenderService _emailSender;
        private readonly IBackgroundJobsService _backgroundJobsService;
        private readonly ILogger<MakeSubscriptionCommandHandler> _logger;

        public MakeSubscriptionCommandHandler(
            IUnitOfWork unitOfWork, 
            IMapper mapper, 
            IProducerService producerService, 
            IUserServiceGrpcClient userServiceClient,
            IEmailSenderService emailSender,
            IBackgroundJobsService backgroundJobsService,
            ILogger<MakeSubscriptionCommandHandler> logger)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _producerService = producerService;
            _userServiceClient = userServiceClient;
            _emailSender = emailSender;
            _backgroundJobsService = backgroundJobsService;
            _logger = logger;
        }

        public async Task<GetSubscriptionDto> Handle(MakeSubscriptionCommand request, CancellationToken cancellationToken)
        {
            var userInfo = await _userServiceClient.GetUserInfoAsync(request.Dto.UserId, cancellationToken);

            _logger.LogInformation("Fetched info for user {Id} from identity.", request.Dto.UserId);

            var tariffPlan = await _unitOfWork.TariffPlans.GetByIdAsync(
                request.Dto.TariffPlanId,
                cancellationToken);

            if (tariffPlan == null)
            {
                _logger.LogError("Tariff plan with id {Id} not found.", request.Dto.TariffPlanId);

                throw new NotFoundException(ExceptionMessages.tariffPlanNotFound);
            }

            var currentSubscription = await _unitOfWork.Subscriptions.GetByUserIdAsync(
                request.Dto.UserId,
                cancellationToken);

            if (currentSubscription != null)
            {
                _logger.LogError("User {Id} already has a subscription.", request.Dto.UserId);

                throw new BadRequestException(ExceptionMessages.subscriptionExists);
            }

            var subscription = _mapper.Map<Subscription>(request.Dto);
            subscription.Id = Guid.NewGuid().ToString();
            subscription.SubscribedAt = DateTime.UtcNow;
            subscription.TariffPlan = tariffPlan;
            string subscriptionPaymentCron;

            switch (subscription.Type)
            {
                case SubscriptionTypes.month:
                    subscription.NextFeeDate = DateTime.UtcNow.AddMonths(1);
                    subscription.Fee = tariffPlan.MonthFee;
                    subscriptionPaymentCron = Cron.Monthly();
                    break;
                case SubscriptionTypes.annual:
                    subscription.NextFeeDate = DateTime.UtcNow.AddYears(1);
                    subscription.Fee = tariffPlan.AnnualFee;
                    subscriptionPaymentCron = Cron.Yearly();
                    break;
                default:
                    throw new UnprocessableEntityException(ExceptionMessages.incorrctSubscriptionType);
            }

            await _unitOfWork.Subscriptions.CreateAsync(subscription, cancellationToken);

            await _unitOfWork.CommitAsync(cancellationToken);

            _logger.LogInformation("Subscription with id {Id} is persisted.", subscription.Id);

            await _producerService.ProduceSubscriptionMadeAsync(_mapper.Map<SubscriptionMadeMessage>(subscription), cancellationToken);

            var subscriptionDto = _mapper.Map<GetSubscriptionDto>(subscription);

            BackgroundJob.Enqueue(() => _emailSender.SendSubscriptionMadeMessage(new SubscriptionWithUserInfo
            {
                UserInfo = userInfo,
                Subscription = subscriptionDto
            }));

            RecurringJob.AddOrUpdate(subscription.Id,
                () => _backgroundJobsService.MakeSubscriptionPayment(subscription.Id),
                subscriptionPaymentCron);

            _logger.LogInformation("All the tasks for subscription {Id} are created.", subscription.Id);

            return subscriptionDto;
        }
    }
}
