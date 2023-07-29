using CSharpFunctionalExtensions;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using WorldCitiesAPI.Domain.CityAggregate;

namespace WorldCitiesAPI.Domain.CountryAggregate
{
    [Table("Countries")]
    [Index(nameof(Name))]
    [Index(nameof(ISO2))]
    [Index(nameof(ISO3))]
    public class Country
    {
        private Country()
        {
            Cities = new();
        }
        #region Properties
        /// <summary>
        /// The unique id and primary key for this Country
        /// </summary>
        [Key]
        [Required]
        public int Id { get; set; }
        /// <summary>
        /// Country name (in UTF8 format)
        /// </summary>
        public string Name { get; set; } = null!;
        /// <summary>
        /// Country code (in ISO 3166-1 ALPHA-2 format)
        /// </summary>
        public string ISO2 { get; set; } = null!;
        /// <summary>
        /// Country code (in ISO 3166-1 ALPHA-3 format)
        /// </summary>
        public string ISO3 { get; set; } = null!;
        #endregion

        #region Navigation Properties
        /// <summary>
        /// A collection of all the cities related to this country.
        /// </summary>
        public List<City> Cities { get; set; }
        #endregion

        public static Result<Country> CreateCountry(string name,string iso2,string iso3)
        {
            return Result.Success(new Country
            {
                Name = name,
                ISO2 = iso2,
                ISO3 = iso3
            });
        }

        public Result<Country> UpdateCountry(string name, string iso2, string iso3)
        {
            return Result.Success(new Country
            {
                Name = name,
                ISO2 = iso2,
                ISO3 = iso3
            });
        }
    }
}
