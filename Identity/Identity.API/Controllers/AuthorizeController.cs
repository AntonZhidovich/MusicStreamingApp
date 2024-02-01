using Identity.BusinessLogic.Models;
using Identity.BusinessLogic.Models.UserService;
using Identity.BusinessLogic.Services;
using Microsoft.AspNetCore.Mvc;

namespace Identity.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthorizeController : ControllerBase
    {
        private readonly ISignInService _signInService;

        public AuthorizeController(
            ISignInService signInService, ITokenService tokenService)
        {      
            _signInService = signInService;
        }

        [HttpPost]
        [ProducesResponseType(200, Type = typeof(Tokens))]
        public async Task<IActionResult> SignInAsync([FromBody] CheckPasswordRequest request, CancellationToken cancellationToken)
        {
            var tokens = await _signInService.SignInAsync(request);
            return Ok(tokens);
        }

        [HttpPost("Refresh/")]
        [ProducesResponseType(200, Type = typeof(Tokens))]
        public async Task<IActionResult> SignInWithRefreshAsync([FromBody] Tokens tokens, CancellationToken cancellationToken)
        {
            var newTokens = await _signInService.SignInWithRefreshAsync(tokens);
            return Ok(newTokens);
        }
    }
}
