using Identity.BusinessLogic.Models.Messages;

namespace Identity.BusinessLogic.Services.Interfaces
{
    public interface IProducerService
    {
        Task ProduceAsync<TValue>(string topic, string key, TValue value, CancellationToken cancellationToken = default) 
            where TValue : class;

        Task ProduceUserDeletedAsync(UserDeletedMessage message, CancellationToken cancellationToken = default);
        Task ProduceUserUpdatedAsync(UserUpdatedMessage message, CancellationToken cancellationToken = default);
    }
}
