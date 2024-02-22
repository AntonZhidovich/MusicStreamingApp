using MediatR;
using Microsoft.AspNetCore.Mvc;
using SubscriptionService.BusinessLogic.Commands.CancelSubscription;
using SubscriptionService.BusinessLogic.Commands.MakeSubscription;
using SubscriptionService.BusinessLogic.Commands.MakeSubscriptionPayment;
using SubscriptionService.BusinessLogic.Commands.UpdateSubscription;
using SubscriptionService.BusinessLogic.Models;
using SubscriptionService.BusinessLogic.Models.Subscription;
using SubscriptionService.BusinessLogic.Queries.GetAllSubscriptions;
using SubscriptionService.BusinessLogic.Queries.GetUserSubscription;

namespace SubscriptionService.API.Controllers
{
    [Route("api/subscriptions")]
    [ApiController]
    public class SubscriptionController : ControllerBase
    {
        private readonly ISender _sender;

        public SubscriptionController(ISender sender)
        {
            _sender = sender;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllAsync([FromQuery] GetPageRequest pageRequest)
        {
            var subscriptions = await _sender.Send(new GetAllSubscriptionsQuery(pageRequest), HttpContext.RequestAborted);

            return Ok(subscriptions);
        }

        [HttpGet("user/{username}")]
        public async Task<IActionResult> GetUserSubscriptionAsync([FromRoute] string username)
        {
            var subscription = await _sender.Send(new GetUserSubscriptionQuery(username), HttpContext.RequestAborted);

            return Ok(subscription);
        }

        [HttpPost]
        public async Task<IActionResult> CreateAsync([FromBody] CreateSubscriptionDto dto)
        {
            var subscription = await _sender.Send(new MakeSubscriptionCommand(dto), HttpContext.RequestAborted);

            return Ok(subscription);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateAsync([FromRoute] string id, [FromBody] UpdateSubscriptionDto dto)
        {
            var subscription = await _sender.Send(new UpdateSubscriptionCommand(id, dto), HttpContext.RequestAborted);

            return Ok(subscription);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> CancelAsync([FromRoute] string id)
        {
            await _sender.Send(new CancelSubscriptionCommand(id), HttpContext.RequestAborted);

            return NoContent();
        }

        [HttpPost("{id}/payment")]
        public async Task<IActionResult> MakePaymentAsync([FromRoute] string id)
        {
            var subscription = await _sender.Send(new MakeSubscriptionPaymentCommand(id), HttpContext.RequestAborted);

            return Ok(subscription);
        }
    }
}
