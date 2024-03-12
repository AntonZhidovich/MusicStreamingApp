using AutoMapper;
using Confluent.Kafka;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using MusicService.Application.Models.Messages;
using MusicService.Application.Options;
using MusicService.Domain.Interfaces;

namespace MusicService.Application.Consumers
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

            var user = await unitOfWork.Users.GetByIdAsync(message.Id, cancellationToken);

            if (user == null)
            {
                _consumer.Commit();
                return;
            }

            unitOfWork.Users.Delete(user);

            await unitOfWork.CommitAsync(cancellationToken);

            var playlistRepository = scope.ServiceProvider.GetService<IPlaylistRepository>()!;

            await playlistRepository.DeleteUserPlaylistsAsync(user.Id, cancellationToken);

            await playlistRepository.DeleteUserPlaylistTariffAsync(user.Id, cancellationToken);

            _consumer.Commit();
        }
    }
}
