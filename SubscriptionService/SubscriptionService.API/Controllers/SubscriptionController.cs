using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SubscriptionService.BusinessLogic.Constants;
using SubscriptionService.BusinessLogic.Features.Commands.CancelSubscription;
using SubscriptionService.BusinessLogic.Features.Commands.MakeSubscription;
using SubscriptionService.BusinessLogic.Features.Commands.MakeSubscriptionPayment;
using SubscriptionService.BusinessLogic.Features.Commands.UpdateSubscription;
using SubscriptionService.BusinessLogic.Features.Queries.GetAllSubscriptions;
using SubscriptionService.BusinessLogic.Features.Queries.GetSubscriptionWithPlan;
using SubscriptionService.BusinessLogic.Features.Queries.GetUserSubscription;
using SubscriptionService.BusinessLogic.Models;
using SubscriptionService.BusinessLogic.Models.Subscription;

namespace SubscriptionService.API.Controllers
{
    [Route("api/subscriptions")]
    [ApiController]
    [Authorize]
    public class SubscriptionController : ControllerBase
    {
        private readonly ISender _sender;

        public SubscriptionController(ISender sender)
        {
            _sender = sender;
        }

        [HttpGet]
        [Authorize(Roles = UserRoles.admin)]
        public async Task<IActionResult> GetAllAsync([FromQuery] GetPageRequest pageRequest)
        {
            var subscriptions = await _sender.Send(new GetAllSubscriptionsQuery(pageRequest), HttpContext.RequestAborted);

            return Ok(subscriptions);
        }

        [HttpGet("plan/name/{name}")]
        [Authorize(Roles = UserRoles.admin)]
        public async Task<IActionResult> GetWithPlanAsync([FromRoute] string name, [FromQuery] GetPageRequest pageRequest)
        {
            var subscriptions = await _sender.Send(
                new GetSubscriptionWithPlanQuery(pageRequest, name), 
                HttpContext.RequestAborted);

            return Ok(subscriptions);
        }

        [HttpGet("user/{userId}")]
        public async Task<IActionResult> GetUserSubscriptionAsync([FromRoute] string userId)
        {
            var subscription = await _sender.Send(new GetUserSubscriptionQuery(userId), HttpContext.RequestAborted);

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
