using AutoMapper;
using Confluent.Kafka;
using Microsoft.Extensions.Hosting;
using System.Text.Json;

namespace MusicService.Application.Consumers
{
    public abstract class BaseConsumerService<TValue> : BackgroundService
        where TValue : class
    {
        protected abstract string Topic { get; set; }

        protected readonly IConsumer<string, string> _consumer;
        protected readonly IServiceProvider _serviceProvider;
        protected readonly IMapper _mapper;

        protected BaseConsumerService(
            ConsumerConfig config,
            IServiceProvider serviceProvider,
            IMapper mapper)
        {
            _consumer = new ConsumerBuilder<string, string>(config).Build();
            _serviceProvider = serviceProvider;
            _mapper = mapper;
        }

        protected abstract Task HandleMessage(TValue message, CancellationToken cancellationToken = default);

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _consumer.Subscribe(Topic);

            await Task.Run(() => ConsumeMessages(stoppingToken));
        }

        private async Task ConsumeMessages(CancellationToken cancellationToken)
        {
            while (!cancellationToken.IsCancellationRequested)
            {
                var serializedMessage = _consumer.Consume(cancellationToken);
                var message = JsonSerializer.Deserialize<TValue>(serializedMessage.Message.Value)!;

                await HandleMessage(message, cancellationToken);
            }
        }

        public override void Dispose()
        {
            _consumer.Close();
            _consumer.Dispose();
        }
    }
}
