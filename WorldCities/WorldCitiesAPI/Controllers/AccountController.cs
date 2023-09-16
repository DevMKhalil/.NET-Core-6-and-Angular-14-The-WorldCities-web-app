using MediatR;
using Microsoft.AspNetCore.Mvc;
using WorldCitiesAPI.Application.Accounts.Queries.Login;
using WorldCitiesAPI.Common;

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

        [HttpGet("Login")]
        public async Task<ActionResult<LoginResult>> Login([FromQuery]LoginQuery loginQuery)
        {
            var result = await _mediator.Send(loginQuery);

            if (!result.Success)
                return Unauthorized(result);

            return Ok(result);
        }
    }
}
