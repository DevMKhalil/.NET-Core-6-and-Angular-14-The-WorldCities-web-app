﻿using AutoMapper;
using WorldCitiesAPI.Application.Cities.Queries.Dtos;
using WorldCitiesAPI.Common.Mapping;
using WorldCitiesAPI.Domain.CityAggregate;
using WorldCitiesAPI.Domain.CountryAggregate;

namespace WorldCitiesAPI.Application.Countries.Queries.Dtos
{
    public record CountryDto : IMapFrom<Country>
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string ISO2 { get; set; }
        public string ISO3 { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<Country, CountryDto>();
        }
    }
}