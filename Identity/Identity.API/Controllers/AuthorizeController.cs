using Identity.BusinessLogic.Models.TokenService;
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
        [ProducesResponseType(200, Type = typeof(TokenResponse))]
        public async Task<IActionResult> SignInAsync([FromBody] CheckPasswordRequest request)
        {
            var tokens = await _signInService.SignInAsync(request);
            return Ok(tokens);
        }
    }
}
