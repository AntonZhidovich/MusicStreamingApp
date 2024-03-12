namespace SubscriptionService.BusinessLogic.Features.Services.Interfaces
{
    public interface IBackgroundJobsService
    {
        void MakeSubscriptionPayment(string subscriptionId);
    }
}
