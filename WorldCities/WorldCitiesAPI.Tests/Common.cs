using AutoMapper;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorldCitiesAPI.Common.Mapping;
using WorldCitiesAPI.Data;

namespace WorldCitiesAPI.Tests
{
    public class Common
    {
        public static IMapper GetMapper()
        {
            var config = new MapperConfiguration(cfg => cfg.AddProfile<MappingProfile>());

            return config.CreateMapper();
        }

        public static ApplicationDbContext GetApplicationDbContext()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                    .UseInMemoryDatabase(databaseName: "WorldCities")
                    .Options;

            return new ApplicationDbContext(options);
        }
    }
}
