using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WorldCitiesAPI.Application.Cities.Commands.CreateCity;
using WorldCitiesAPI.Application.Cities.Commands.DeleteCity;
using WorldCitiesAPI.Application.Cities.Commands.UpdateCity;
using WorldCitiesAPI.Application.Cities.Queries.Dtos;
using WorldCitiesAPI.Application.Cities.Queries.GetCities;
using WorldCitiesAPI.Application.Cities.Queries.GetCityById;
using WorldCitiesAPI.Application.Countries.Commands.CreateCountry;
using WorldCitiesAPI.Application.Countries.Commands.DeleteCountry;
using WorldCitiesAPI.Application.Countries.Commands.UpdateCountry;
using WorldCitiesAPI.Application.Countries.Queries.Dtos;
using WorldCitiesAPI.Application.Countries.Queries.GetCountries;
using WorldCitiesAPI.Application.Countries.Queries.GetCountryById;
using WorldCitiesAPI.Data;
using WorldCitiesAPI.Domain.CityAggregate;
using WorldCitiesAPI.Domain.CountryAggregate;

namespace WorldCitiesAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CountriesController : ControllerBase
    {
        private readonly IMediator _mediator;

        public CountriesController(IMediator mediator)
        {
            _mediator = mediator;
        }

        // GET: api/Countries
        [HttpGet]
        public async Task<ActionResult<IEnumerable<CountryDto>>> GetCountries()
        {
            var result = await _mediator.Send(new GetCountriesQuery());

            return result;
        }

        // GET: api/Countries/5
        [HttpGet("{id}")]
        public async Task<ActionResult<CountryDto>> GetCountry(int id)
        {
            var countryResult = await _mediator.Send(new GetCountryByIdQuery { CountryId = id });

            if (countryResult.IsFailure)
                return BadRequest(countryResult.Error);

            return Ok(countryResult.Value);
        }

        // PUT: api/Countries/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutCountry(CountryDto country)
        {
            var putResult = await _mediator.Send(new UpdateCountryCommand
            {
                Id = country.Id,
                ISO2 = country.ISO2,
                ISO3 = country.ISO3,
                Name = country.Name,
            });

            if (putResult.IsFailure)
                return BadRequest(putResult.Error);

            return NoContent();
        }

        // POST: api/Countries
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<CountryDto>> PostCountry(CountryDto country)
        {
            var postResult = await _mediator.Send(new CreateCountryCommand
            {
                Name = country.Name,
                ISO3 = country.ISO3,
                ISO2 = country.ISO2,
            });

            if (postResult.IsFailure)
                return BadRequest(postResult.Error);

            return CreatedAtAction("GetCountry", new { id = postResult.Value.Id }, postResult.Value);
        }

        // DELETE: api/Countries/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCountry(int id)
        {
            var deleteResult = await _mediator.Send(new DeleteCountryCommand { CountryId = id });

            if (deleteResult.IsFailure)
                return BadRequest(deleteResult.Error);

            return NoContent();
        }
    }
}
