using AutoMapper;
using Confluent.Kafka;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using MusicService.Application.Models.Messages;
using MusicService.Application.Options;
using MusicService.Domain.Interfaces;

namespace MusicService.Application.Consumers
{
    public class SubscriptionCanceledConsumer : BaseConsumerService<SubscriptionCanceledMessage>
    {
        protected override string Topic { get; set; }
        protected override Func<SubscriptionCanceledMessage, CancellationToken, Task> MessageHandler { get; set; }

        public SubscriptionCanceledConsumer(IOptions<ConsumerConfig> config, IOptions<KafkaTopics> topics, IServiceProvider serviceProvider, IMapper mapper)
           : base(config.Value, serviceProvider, mapper)
        {
            Topic = topics.Value.SubscriptionCanceled;

            MessageHandler = async (message, cancellationToken) =>
            {
                using var scope = _serviceProvider.CreateScope();
                var playlistRepository = scope.ServiceProvider.GetService<IPlaylistRepository>()!;

                var unitOfWork = scope.ServiceProvider.GetService<IUnitOfWork>()!;

                var user = await unitOfWork.Users.GetByIdAsync(message.UserId);

                if (user == null)
                {
                    _consumer.Commit();
                    return;
                }

                await playlistRepository.DeleteUserPlaylistTariffAsync(user.UserName, cancellationToken);
            };
        }
    }
}
