using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using WorldCitiesAPI.Domain.CountryAggregate;
using Microsoft.EntityFrameworkCore;
using CSharpFunctionalExtensions;
using WorldCitiesAPI.Resources;

namespace WorldCitiesAPI.Domain.CityAggregate
{
    [Table("Cities")]
    [Index(nameof(Name))]
    [Index(nameof(Lat))]
    [Index(nameof(Lon))]
    public class City
    {
        private City()
        {

        }
        #region Properties
        /// <summary>
        /// The unique id and primary key for this City
        /// </summary>
        [Key]
        [Required]
        public int Id { get; set; }
        /// <summary>
        /// City name (in UTF8 format)
        /// </summary>
        public string Name { get; set; } = null!;
        /// <summary>
        /// City latitude
        /// </summary>
        [Column(TypeName = "decimal(7,4)")]
        public decimal Lat { get; set; }
        /// <summary>
        /// City longitude
        /// </summary>
        [Column(TypeName = "decimal(7,4)")]
        public decimal Lon { get; set; }
        /// <summary>
        /// Country Id (foreign key)
        /// </summary>
        [ForeignKey(nameof(Country))]
        public int CountryId { get; set; }
        #endregion

        #region Navigation Properties
        /// <summary>
        /// The country related to this city.
        /// </summary>
        public Country Country { get; set; } = null!;
        #endregion

        public static Result<City> CreateCity(string name,decimal lat,decimal lon,Maybe<Country> maybeCountry)
        {
            if (maybeCountry.HasNoValue)
                return Result.Failure<City>(Resource.CountryNotFound);

            return Result.Success(new City
            {
                Name = name,
                Lat = lat,
                Lon = lon,
                CountryId = maybeCountry.Value.Id
            });
        }

        public Result<City> UpdateCity(string name, decimal lat, decimal lon, Maybe<Country> maybeCountry)
        {
            if (maybeCountry.HasNoValue)
                return Result.Failure<City>(Resource.CountryNotFound);

            this.Name = name;
            this.Lat = lat;
            this.Lon = lon;
            this.CountryId = maybeCountry.Value.Id;
            
            return Result.Success(this);
        }
    }
}
