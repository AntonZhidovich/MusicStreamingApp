using Confluent.Kafka;
using Identity.BusinessLogic.Models.Messages;
using Identity.BusinessLogic.Options;
using Identity.BusinessLogic.Services.Interfaces;
using Microsoft.Extensions.Options;
using System.Text.Json;

namespace Identity.BusinessLogic.Services.Implementations
{
    public class ProducerService : IProducerService
    {
        private readonly IProducer<string, string> _producer;
        private readonly KafkaTopics _topics;

        public ProducerService(IProducer<string, string> producer,
            IOptions<KafkaTopics> topics)
        {
            _producer = producer;
            _topics = topics.Value;
        }

        public async Task ProduceUserDeletedAsync(UserDeletedMessage message, CancellationToken cancellationToken = default)
        {
            await ProduceAsync(_topics.UserDeleted, message.Id, message, cancellationToken);
        }

        public async Task ProduceUserUpdatedAsync(UserUpdatedMessage message, CancellationToken cancellationToken = default)
        {
            await ProduceAsync(_topics.UserUpdated, message.Id, message, cancellationToken);
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
