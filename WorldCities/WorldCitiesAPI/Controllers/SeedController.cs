using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WorldCitiesAPI.Application.Accounts.Commands.CreateDefaultUsers;
using WorldCitiesAPI.Application.Import.Commands.ImportFromExcel;

namespace WorldCitiesAPI.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    [Authorize(Roles = "Administrator")]
    public class SeedController : ControllerBase
    {
        private readonly IMediator _mediator;

        public SeedController(IMediator mediator)
        {
            _mediator = mediator;
        }
        [HttpGet]
        public async Task<ActionResult> Import()
        {
            var result = await _mediator.Send(new ImportFromExcelCommand());

            if (result.IsFailure)
                return BadRequest(result.Error);

            return new JsonResult(new
            {
                Cities = result.Value.numberOfCitiesAdded,
                Countries = result.Value.numberOfCountriesAdded
            });
        }

        [HttpGet]
        public async Task<ActionResult> CreateDefaultUsers()
        {
            var result = await _mediator.Send(new CreateDefaultUsersCommand());

            if (result.IsFailure)
                return BadRequest(result.Error);

            return new JsonResult(new
            {
                Count = result.Value.Count,
                Users = result.Value
            });
        }
    }
}
