using MediatR;
using Microsoft.AspNetCore.Mvc;
using WorldCitiesAPI.Application.Accounts.Queries.Login;

namespace WorldCitiesAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly IMediator _mediator;

        public AccountController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost("Login")]
        public async Task<IActionResult> Login(LoginQuery loginQuery)
        {
            var result = await _mediator.Send(loginQuery);

            if (!result.Success)
                return Unauthorized(result);

            return Ok(result);
        }
    }
}
