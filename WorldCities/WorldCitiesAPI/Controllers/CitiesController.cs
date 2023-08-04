using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WorldCitiesAPI.Application;
using WorldCitiesAPI.Application.Cities.Commands.CreateCity;
using WorldCitiesAPI.Application.Cities.Commands.DeleteCity;
using WorldCitiesAPI.Application.Cities.Commands.UpdateCity;
using WorldCitiesAPI.Application.Cities.Queries.Dtos;
using WorldCitiesAPI.Application.Cities.Queries.GetCities;
using WorldCitiesAPI.Application.Cities.Queries.GetCityById;
using WorldCitiesAPI.Data;
using WorldCitiesAPI.Domain.CityAggregate;

namespace WorldCitiesAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CitiesController : ControllerBase
    {
        private readonly IMediator _mediator;

        public CitiesController(IMediator mediator)
        {
            _mediator = mediator;
        }

        // GET: api/Cities
        // GET: api/Cities/?pageIndex=0&pageSize=10
        // GET: api/Cities/?pageIndex=0&pageSize=10&sortColumn=name& 
        //  sortOrder=asc
        [HttpGet]
        public async Task<ActionResult<ApiResult<CityDto>>> GetCities([FromQuery] GetCitiesQuery citiesQuery)
        {
            var result = await _mediator.Send(citiesQuery);

            return result;
        }

        // GET: api/Cities/5
        [HttpGet("{id}")]
        public async Task<ActionResult<CityDto>> GetCity(int id)
        {
            var cityResult = await _mediator.Send(new GetCityByIdQuery { CityId = id });

            if (cityResult.IsFailure)
                return BadRequest(cityResult.Error);

            return Ok(cityResult.Value);
        }

        // PUT: api/Cities/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutCity(CityDto cityDto)
        {
            var putResult = await _mediator.Send(new UpdateCityCommand
            {
                Id = cityDto.Id,
                CountryId = cityDto.CountryId,
                lat = cityDto.lat,
                Lon = cityDto.Lon,
                Name = cityDto.Name,
            });

            if (putResult.IsFailure)
                return BadRequest(putResult.Error);

            return NoContent();
        }

        // POST: api/Cities
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<CityDto>> PostCity(CityDto city)
        {
            var postResult = await _mediator.Send(new CreateCityCommand
            {
                Name = city.Name,
                Lon = city.Lon,
                lat = city.lat,
                CountryId = city.CountryId
            });

            if (postResult.IsFailure)
                return BadRequest(postResult.Error);

            return CreatedAtAction("GetCity", new { id = postResult.Value.Id }, postResult.Value);
        }

        // DELETE: api/Cities/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCity(int id)
        {
            var deleteResult = await _mediator.Send(new DeleteCityCommand { CityId = id });

            if (deleteResult.IsFailure)
                return BadRequest(deleteResult.Error);

            return NoContent();
        }
    }
}
