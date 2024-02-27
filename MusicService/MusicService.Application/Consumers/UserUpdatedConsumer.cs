using AutoMapper;
using Confluent.Kafka;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using MusicService.Application.Models.Messages;
using MusicService.Application.Options;
using MusicService.Domain.Interfaces;

namespace MusicService.Application.Consumers
{
    public class UserUpdatedConsumer : BaseConsumerService<UserUpdatedMessage>
    {
        protected override string Topic { get; set; }
        protected override Func<UserUpdatedMessage, CancellationToken, Task> MessageHandler { get; set; }

        public UserUpdatedConsumer(IOptions<ConsumerConfig> config, IOptions<KafkaTopics> topics, IServiceProvider serviceProvider, IMapper mapper)
            : base(config.Value, serviceProvider, mapper)
        {
            Topic = topics.Value.UserUpdated;

            MessageHandler = async (message, cancellationToken) =>
            {
                using var scope = _serviceProvider.CreateScope();
                var unitOfWork = scope.ServiceProvider.GetService<IUnitOfWork>()!;

                var user = await unitOfWork.Users.GetByIdAsync(message.Id, cancellationToken);

                if (user == null)
                {
                    _consumer.Commit();
                    return;
                }

                _mapper.Map(message, user);
                unitOfWork.Users.Update(user);

                await unitOfWork.CommitAsync(cancellationToken);

                _consumer.Commit();
            };
        }
    }
}
