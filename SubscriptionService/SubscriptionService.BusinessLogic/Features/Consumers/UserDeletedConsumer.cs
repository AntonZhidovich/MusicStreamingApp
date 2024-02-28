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
        protected override Func<UserDeletedMessage, CancellationToken, Task> MessageHandler { get; set; }

        public UserDeletedConsumer(IOptions<ConsumerConfig> config, IOptions<KafkaTopics> topics, IServiceProvider serviceProvider, IMapper mapper)
           : base(config.Value, serviceProvider, mapper)
        {
            Topic = topics.Value.UserDeleted;

            MessageHandler = async (message, cancellationToken) =>
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
            };
        }

    }
}
