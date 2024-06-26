﻿using CSharpFunctionalExtensions;
using CSharpFunctionalExtensions.ValueTasks;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OfficeOpenXml;
using System.Security;
using WorldCitiesAPI.Domain.ApplicationUser;
using WorldCitiesAPI.Domain.CityAggregate;
using WorldCitiesAPI.Domain.CountryAggregate;

namespace WorldCitiesAPI.Application.Import.Commands.ImportFromExcel
{
    public record ImportResult(int numberOfCitiesAdded,int numberOfCountriesAdded);
    public class ImportFromExcelCommand : IRequest<Result<ImportResult>>
    {
    }

    public class ImportFromExcelCommandHandler : IRequestHandler<ImportFromExcelCommand, Result<ImportResult>>
    {
        private readonly IApplicationDbContext _context;
        private readonly IWebHostEnvironment _env;

        public ImportFromExcelCommandHandler(
            IApplicationDbContext context,
            IWebHostEnvironment env)
        {
            _context = context;
            _env = env;
        }

        public async Task<Result<ImportResult>> Handle(ImportFromExcelCommand request, CancellationToken cancellationToken)
        {
            // prevents non-development environments from running this method
            if (!_env.IsDevelopment())
                throw new SecurityException("Not allowed");
            var path = Path.Combine(
            _env.ContentRootPath,
            "Data/Source/worldcities.xlsx");
            using var stream = System.IO.File.OpenRead(path);
            using var excelPackage = new ExcelPackage(stream);

            // get the first worksheet
            var worksheet = excelPackage.Workbook.Worksheets[0];
            // define how many rows we want to process
            var nEndRow = worksheet.Dimension.End.Row;
            // initialize the record counters
            var numberOfCountriesAdded = 0;
            var numberOfCitiesAdded = 0;
            // create a lookup dictionary
            // containing all the countries already existing
            // into the Database (it will be empty on first run).
            var countriesByName = _context.Countries
            .AsNoTracking()
            .ToDictionary(x => x.Name, StringComparer.OrdinalIgnoreCase);

            // iterates through all rows, skipping the first one
            for (int nRow = 2; nRow <= nEndRow; nRow++)
            {
                var row = worksheet.Cells[
                nRow, 1, nRow, worksheet.Dimension.End.Column];
                var countryName = row[nRow, 5].GetValue<string>();
                var iso2 = row[nRow, 6].GetValue<string>();
                var iso3 = row[nRow, 7].GetValue<string>();
                // skip this country if it already exists in the database
                if (countriesByName.ContainsKey(countryName))
                    continue;

                // create the Country entity and fill it with xlsx data
                var countryResult = Country.CreateCountry(
                    countryName,
                    iso2,
                    iso3);

                if (countryResult.IsFailure)
                    return Result.Failure<ImportResult>(countryResult.Error);

                // add the new country to the DB context
                await _context.Countries.AddAsync(countryResult.Value);

                // store the country in our lookup to retrieve its Id later on
                countriesByName.Add(countryName, countryResult.Value);

                // increment the counter
                numberOfCountriesAdded++;
            }

            // save all the countries into the Database
            if (numberOfCountriesAdded > 0)
            {
                var saveResult = await _context.SaveChangesAsyncWithValidation();

                if (saveResult.IsFailure)
                    return Result.Failure<ImportResult>(saveResult.Error);
            }

            // create a lookup dictionary
            // containing all the cities already existing
            // into the Database (it will be empty on first run).
            var cities = _context.Cities
            .AsNoTracking()
            .ToDictionary(x => (
            Name: x.Name,
            Lat: x.Lat,
            Lon: x.Lon,
            CountryId: x.CountryId));

            // iterates through all rows, skipping the first one
            for (int nRow = 2; nRow <= nEndRow; nRow++)
            {
                var row = worksheet.Cells[
                nRow, 1, nRow, worksheet.Dimension.End.Column];
                var name = row[nRow, 1].GetValue<string>();
                var nameAscii = row[nRow, 2].GetValue<string>();
                var lat = row[nRow, 3].GetValue<decimal>();
                var lon = row[nRow, 4].GetValue<decimal>();
                var countryName = row[nRow, 5].GetValue<string>();

                // retrieve country Id by countryName
                var countryId = countriesByName[countryName].Id;
                var country = countriesByName[countryName];

                // skip this city if it already exists in the database
                if (cities.ContainsKey((
                        Name: name,
                        Lat: lat,
                        Lon: lon,
                        CountryId: countryId)))
                    continue;

                // create the City entity and fill it with xlsx data
                var cityResult = City.CreateCity(
                    name,
                    lat,
                    lon,
                    country);

                if (cityResult.IsFailure)
                    return Result.Failure<ImportResult>(cityResult.Error);

                // add the new city to the DB context
                _context.Cities.Add(cityResult.Value);

                // increment the counter
                numberOfCitiesAdded++;
            }

            // save all the cities into the Database
            if (numberOfCitiesAdded > 0)
            {
                var saveResult = await _context.SaveChangesAsyncWithValidation();

                if (saveResult.IsFailure)
                    return Result.Failure<ImportResult>(saveResult.Error);
            }

            return Result.Success(new ImportResult(numberOfCitiesAdded, numberOfCountriesAdded));
        }
    }
}
