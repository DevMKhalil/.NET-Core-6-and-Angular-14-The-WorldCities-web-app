using AutoMapper;
using System.Text.Json.Serialization;
using WorldCitiesAPI.Application.Cities.Queries.Dtos;
using WorldCitiesAPI.Common.Mapping;
using WorldCitiesAPI.Domain.CityAggregate;
using WorldCitiesAPI.Domain.CountryAggregate;

namespace WorldCitiesAPI.Application.Countries.Queries.Dtos
{
    public record CountryDto : IMapFrom<Country>
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        [JsonPropertyName("iso2")]
        public string ISO2 { get; set; } = null!;
        [JsonPropertyName("iso3")]
        public string ISO3 { get; set; } = null!;
        public int? TotCities { get; set; } = null!;

        public void Mapping(Profile profile)
        {
            profile.CreateMap<Country, CountryDto>();
        }
    }
}
