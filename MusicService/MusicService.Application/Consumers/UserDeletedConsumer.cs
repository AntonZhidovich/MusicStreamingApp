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
        protected override Func<UserDeletedMessage, CancellationToken, Task> MessageHandler { get; set; }

        public UserDeletedConsumer(IOptions<ConsumerConfig> config, IOptions<KafkaTopics> topics, IServiceProvider serviceProvider, IMapper mapper)
           : base(config.Value, serviceProvider, mapper)
        {
            Topic = topics.Value.UserDeleted;

            MessageHandler = async (message, cancellationToken)=>
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

                await playlistRepository.DeleteUserPlaylistsAsync(user.UserName, cancellationToken);

                await playlistRepository.DeleteUserPlaylistTariffAsync(user.UserName, cancellationToken);

                _consumer.Commit();
            };
        }

    }
}
