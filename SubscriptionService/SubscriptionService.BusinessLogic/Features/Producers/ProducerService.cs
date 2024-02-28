using Confluent.Kafka;
using Microsoft.Extensions.Options;
using SubscriptionService.BusinessLogic.Models.Messages;
using SubscriptionService.BusinessLogic.Options;
using System.Text.Json;

namespace SubscriptionService.BusinessLogic.Features.Producers
{
    public class ProducerService : IProducerService
    {
        private readonly IProducer<string, string> _producer;
        private readonly KafkaTopics _topics;

        public ProducerService(IProducer<string, string> producer, IOptions<KafkaTopics> topics)
        {
            _producer = producer;
            _topics = topics.Value;
        }

        public async Task ProduceSubscriptionMadeAsync(SubscriptionMadeMessage message, CancellationToken cancellationToken = default)
        {
            await ProduceAsync(_topics.SubscriptionMade, message.UserId, message, cancellationToken);
        }

        public async Task ProduceSubscriptionCanceledAsync(SubscriptionCanceledMessage message, CancellationToken cancellationToken = default)
        {
            await ProduceAsync(_topics.SubscriptionCanceled, message.UserId, message, cancellationToken);
        }

        public async Task ProduceAsync<TValue>(string topic,
            string key,
            TValue value,
            CancellationToken cancellationToken = default)
            where TValue : class
        {
            var serializedValue = JsonSerializer.Serialize(value);

            var message = new Message<string, string>
            {
                Key = key,
                Value = serializedValue
            };

            await _producer.ProduceAsync(topic, message, cancellationToken);
        }
    }
}
