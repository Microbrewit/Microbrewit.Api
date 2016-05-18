using AutoMapper;
using Microbrewit.Api.Model.Database;
using Microbrewit.Api.Model.DTOs;

namespace Microbrewit.Api.Mapper.CustomResolvers
{
    public class BreweryGeoLocationResolver : ValueResolver<Brewery,GeoLocationDto>
    {
        protected override GeoLocationDto ResolveCore(Brewery brewery)
        {
            return new GeoLocationDto
            {
                Latitude = brewery.Latitude,
                Longitude = brewery.Longitude
            };
        }
    }
}