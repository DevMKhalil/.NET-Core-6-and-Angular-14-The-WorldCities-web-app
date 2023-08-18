using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Moq;
using System.Threading.Tasks;
using WorldCitiesAPI.Application.Countries.Commands.CreateCountry;
using WorldCitiesAPI.Controllers;
using WorldCitiesAPI.Data;
using WorldCitiesAPI.Domain;
using WorldCitiesAPI.Domain.CityAggregate;
using WorldCitiesAPI.Domain.CountryAggregate;
using Xunit;
using FluentAssertions;
using WorldCitiesAPI.Application.Cities.Commands.CreateCity;
using WorldCitiesAPI.Common.Mapping;
using WorldCitiesAPI.Resources;

namespace WorldCitiesAPI.Tests
{
    public class CitiesController_Tests
    {
        /// <summary>
        /// Test the GetCity() method
        /// </summary>
        [Fact]
        public async Task Handel_Should_ReturnSuccessResult_WhenCreateCity()
        {
            var mapper = Common.GetMapper();
            var context = Common.GetApplicationDbContext();

            // Arrange
            #region Country
            var createCountryCommand = new CreateCountryCommand { Name = "TestCountry", ISO2 = "TS", ISO3 = "TST" };

            var createCountryHandler = new CreateCountryCommandHandler(context, mapper);

            var createCountryResult = await createCountryHandler.Handle(createCountryCommand, default);

            createCountryResult.IsSuccess.Should().BeTrue(); 
            #endregion
            var createCityCommand = new CreateCityCommand { Name = "TestCity1", lat = 1, Lon = 1, CountryId = createCountryResult.Value.Id };
            var createCityHandler = new CreateCityCommandHandler(context, mapper);

            // Act
            var createCityResult = await createCityHandler.Handle(createCityCommand, default);

            // Assert
            createCityResult.IsSuccess.Should().BeTrue();
            createCityResult.Value.Should().NotBeNull();
            createCityResult.Value.Id.Should().BeGreaterThan(default(int));
        }

        [Fact]
        public async Task Handel_Should_ReturnFailureResult_WhenCountryIsNotSend()
        {
            var mapper = Common.GetMapper();
            var context = Common.GetApplicationDbContext();

            // Arrange
            var createCityCommand = new CreateCityCommand { Name = "TestCity1", lat = 1, Lon = 1, CountryId = default(int) };
            var createCityHandler = new CreateCityCommandHandler(context, mapper);

            // Act
            var createCityResult = await createCityHandler.Handle(createCityCommand, default);

            // Assert
            createCityResult.IsFailure.Should().BeTrue();
            createCityResult.Error.Should().Be(Resource.CountryNotFound);
        }
    }
}
