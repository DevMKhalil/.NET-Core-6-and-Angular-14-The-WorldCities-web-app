﻿using AutoMapper;
using WorldCitiesAPI.Common.Mapping;
using WorldCitiesAPI.Domain.CityAggregate;

namespace WorldCitiesAPI.Application.Cities.Queries.Dtos
{
    public record CityDto : IMapFrom<City>
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public decimal lat { get; set; }
        public decimal Lon { get; set; }
        public int CountryId { get; set; }
        public string? CountryName { get; set; } = null!;

        public void Mapping(Profile profile)
        {
            profile.CreateMap<City, CityDto>();
        }
    }
}
