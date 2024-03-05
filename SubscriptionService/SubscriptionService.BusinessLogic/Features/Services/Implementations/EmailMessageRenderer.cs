using Identity.Grpc;
using RazorLight;
using SubscriptionService.BusinessLogic.Features.Services.Interfaces;

namespace SubscriptionService.BusinessLogic.Features.Services.Implementations
{
    public class EmailMessageRenderer : IEmailMessageRenderer
    {
        private readonly IRazorLightEngine _razorEngine;

        public EmailMessageRenderer(IRazorLightEngine razorEngine)
        {
            _razorEngine = razorEngine;
        }

        public async Task<string> Render<TModel>(string path, TModel model)
        {
            return await _razorEngine.CompileRenderAsync(path, model);
        }
    }
}
