using Identity.Grpc;
using SubscriptionService.BusinessLogic.Models.Subscription;
using SubscriptionService.BusinessLogic.Models.TariffPlan;

namespace SubscriptionService.BusinessLogic.Features.Services.Interfaces
{
    public interface IEmailSenderService
    {
        void SendSubscriptionMadeMessage(SubscriptionWithUserInfo info);
        void SendSubscriptionCanceledMessage(SubscriptionWithUserInfo info);
    }
}
