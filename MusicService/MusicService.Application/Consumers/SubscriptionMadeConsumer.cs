using AutoMapper;
using Confluent.Kafka;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using MusicService.Application.Models.Messages;
using MusicService.Application.Options;
using MusicService.Domain.Entities;
using MusicService.Domain.Interfaces;

namespace MusicService.Application.Consumers
{
    public class SubscriptionMadeConsumer : BaseConsumerService<SubscriptionMadeMessage>
    {
        protected override string Topic { get; set; }

        public SubscriptionMadeConsumer(IOptions<ConsumerConfig> config, IOptions<KafkaTopics> topics, IServiceProvider serviceProvider, IMapper mapper)
           : base(config.Value, serviceProvider, mapper)
        {
            Topic = topics.Value.SubscriptionMade;
        }

        protected override async Task HandleMessage(SubscriptionMadeMessage message, CancellationToken cancellationToken = default)
        {
            using var scope = _serviceProvider.CreateScope();
            var playlistRepository = scope.ServiceProvider.GetService<IPlaylistRepository>()!;
            var userPlaylistTariff = _mapper.Map<UserPlaylistTariff>(message);
            userPlaylistTariff.Id = Guid.NewGuid().ToString();

            await playlistRepository.UpsertUserPlaylistTariffAsync(userPlaylistTariff, cancellationToken);

            _consumer.Commit();
        }
    }
}
