using SubscriptionService.BusinessLogic.Models.Messages;

namespace SubscriptionService.BusinessLogic.Features.Producers
{
    public interface IProducerService
    {
        Task ProduceSubscriptionMadeAsync(SubscriptionMadeMessage message, CancellationToken cancellationToken = default);
        Task ProduceSubscriptionCanceledAsync(SubscriptionCanceledMessage message, CancellationToken cancellationToken = default);
        
        Task ProduceAsync<TValue>(string topic, string key, TValue value, CancellationToken cancellationToken = default)
            where TValue : class;
    }
}
