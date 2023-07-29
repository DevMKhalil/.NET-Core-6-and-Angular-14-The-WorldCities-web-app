﻿using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.Reflection.Emit;
using WorldCitiesAPI.Domain.CityAggregate;

namespace WorldCitiesAPI.Data.EFConfiguration
{
    public class CityConfiguration : IEntityTypeConfiguration<City>
    {
        public void Configure(EntityTypeBuilder<City> builder)
        {
            builder.ToTable("Cities");
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id).IsRequired();
            builder.Property(x => x.Lat).HasColumnType("decimal(7,4)");
            builder.Property(x => x.Lon).HasColumnType("decimal(7,4)");
            builder.HasOne(x => x.Country).WithMany(y => y.Cities).HasForeignKey(x => x.CountryId).OnDelete(DeleteBehavior.Cascade);
        }
    }
}
