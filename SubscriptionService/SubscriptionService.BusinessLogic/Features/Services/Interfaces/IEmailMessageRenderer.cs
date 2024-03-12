using Identity.Grpc;

namespace SubscriptionService.BusinessLogic.Features.Services.Interfaces
{
    public interface IEmailMessageRenderer
    {
        Task<string> Render<TModel>(string path, TModel model);
    }
}
