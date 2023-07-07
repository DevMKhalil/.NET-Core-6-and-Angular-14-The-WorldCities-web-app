using System.Security;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OfficeOpenXml;
using WorldCitiesAPI.Application;
using WorldCitiesAPI.Application.Import.Commands.ImportFromExcel;
using WorldCitiesAPI.Domain.CityAggregate;
using WorldCitiesAPI.Domain.CountryAggregate;

namespace WorldCitiesAPI.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class SeedController : Controller
    {
        private readonly IApplicationDbContext _context;
        private readonly IWebHostEnvironment _env;
        private readonly IMediator _mediator;

        public SeedController(
            IApplicationDbContext dbContext,
            IWebHostEnvironment env,
            IMediator mediator)
        {
            _context = dbContext;
            _env = env;
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
    }
}
