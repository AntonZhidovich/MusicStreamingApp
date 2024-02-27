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

                await playlistRepository.DeleteUserPlaylistTariffAsync(message.UserName, cancellationToken);
            };
        }
    }
}
