using AutoMapper;
using Confluent.Kafka;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using MusicService.Application.Models.Messages;
using SubscriptionService.BusinessLogic.Options;
using SubscriptionService.DataAccess.Repositories.Interfaces;

namespace SubscriptionService.BusinessLogic.Features.Consumers
{
    public class UserDeletedConsumer : BaseConsumerService<UserDeletedMessage>
    {
        protected override string Topic { get; set; }

        public UserDeletedConsumer(IOptions<ConsumerConfig> config, IOptions<KafkaTopics> topics, IServiceProvider serviceProvider, IMapper mapper)
           : base(config.Value, serviceProvider, mapper)
        {
            Topic = topics.Value.UserDeleted;
        }

        protected override async Task HandleMessage(UserDeletedMessage message, CancellationToken cancellationToken = default)
        {
            using var scope = _serviceProvider.CreateScope();
            var unitOfWork = scope.ServiceProvider.GetService<IUnitOfWork>()!;

            var subscription = await unitOfWork.Subscriptions.GetByUserIdAsync(message.Id, cancellationToken);

            if (subscription == null)
            {
                _consumer.Commit();
                return;
            }

            unitOfWork.Subscriptions.Delete(subscription);

            await unitOfWork.CommitAsync(cancellationToken);
        }

    }
}
