using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SubscriptionService.BusinessLogic.Commands.CreateTariffPlan;
using SubscriptionService.BusinessLogic.Commands.DeleteTariffPlan;
using SubscriptionService.BusinessLogic.Commands.UpdateTariffPlan;
using SubscriptionService.BusinessLogic.Constants;
using SubscriptionService.BusinessLogic.Models;
using SubscriptionService.BusinessLogic.Models.TariffPlan;
using SubscriptionService.BusinessLogic.Queries.GetAllTariffPlans;
using SubscriptionService.BusinessLogic.Queries.GetTariffPlanByName;

namespace SubscriptionService.API.Controllers
{
    [Route("api/plans")]
    [ApiController]
    [Authorize(Roles = UserRoles.admin)]
    public class TariffPlanController : ControllerBase
    {
        private readonly ISender _sender;

        public TariffPlanController(ISender sender)
        {
            _sender = sender;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllAsync([FromQuery] GetPageRequest pageRequest)
        {
            var tariffPlans = await _sender.Send(new GetAllTariffPlansQuery(pageRequest), HttpContext.RequestAborted);

            return Ok(tariffPlans);
        }

        [HttpGet("name/{name}")]
        public async Task<IActionResult> GetAsync([FromRoute] string name)
        {
            var tariffPlan = await _sender.Send(new GetTariffPlanByNameQuery(name), HttpContext.RequestAborted);

            return Ok(tariffPlan);
        }

        [HttpPost]
        public async Task<IActionResult> CreateAsync([FromBody] CreateTariffPlanDto dto)
        {
            var tariffPlan = await _sender.Send(new CreateTariffPlanCommand(dto), HttpContext.RequestAborted);

            return Ok(tariffPlan);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateAsync([FromRoute] string id, [FromBody] UpdateTariffPlanDto dto)
        {
            var tariffPlan = await _sender.Send(new UpdateTariffPlanCommand(id, dto), HttpContext.RequestAborted);

            return Ok(tariffPlan);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAsync([FromRoute] string id)
        {
            await _sender.Send(new DeleteTariffPlanCommand(id), HttpContext.RequestAborted);

            return NoContent();
        }
    }
}
